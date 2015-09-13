///
//Peng: Particle Engine for XNA
//Written by: Kirk Roerig
//Date: 4 - 4 - 09
//Updated for animoNex
///

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using AnimoNex.game.effects;
using AnimoNex.types;

namespace AnimoNex.game.effects
{
    public class PEngine
    {
        public static particle[] particles;
        public static emmitter[] emmitters;
        #region emmitters
        public static void emmitter_create(Vector2 Position, Vector2 Target, Vector2 max, Vector2 min, Texture2D[] Textures, bool Cycle, int Life, float friction, bool Grow, bool Shrink, bool Fade, int size_max, int size_inital, particleType Type)
        {
            emmitter New = new emmitter(Position, Target, max, min, Textures, Cycle, Life, friction, Grow, Shrink, Fade, size_max, size_inital, Type);

            try
            {
                emmitter[] old = emmitters;
                emmitters = new emmitter[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    emmitters[i] = old[i];
                }
                emmitters[old.Length] = New;
            }
            catch
            {
                emmitters = new emmitter[1];
                emmitters[0] = New;
            }
        }
        public static void emmitter_kill(int e)
        {
            try
            {
                emmitter[] lower, upper;

                lower = new emmitter[e];
                upper = new emmitter[emmitters.Length - (e + 1)];

                for (int i = 0; i != lower.Length; i++)
                {
                    lower[i] = emmitters[i];
                }
                for (int i = e + 1; i != emmitters.Length; i++)
                {
                    upper[i - (e + 1)] = emmitters[i];
                }
                emmitters = new emmitter[emmitters.Length - 1];

                for (int i = 0; i != emmitters.Length; i++)
                {
                    if (i < lower.Length)
                    {
                        emmitters[i] = lower[i];
                    }
                    else
                    {
                        emmitters[i] = upper[i - lower.Length];
                    }
                }


            }
            catch
            {

            }
        }
        public static void emmiter_update(Vector2 offset)
        {
            for (int i = 0; i != emmitters.Length; i++)
            {
                emmitter_update(i, offset);

            }
        }
        private static void emmitter_update(int i, Vector2 offset)
        {
            emmitters[i].position += offset;

            if (emmitters[i].cycle)
            {
                if (emmitters[i].nextTex != emmitters[i].textures.Length - 1)   //select next texture
                {
                    emmitters[i].nextTex++;
                }
                else
                {
                    emmitters[i].nextTex = 0;
                }
            }
            else
            {
                emmitters[i].nextTex = Convert.ToInt32(RandomNumber(0, emmitters[i].textures.Length - 1)); //select random texture
            }
            Vector2 v = randomVector(emmitters[i].randMin, emmitters[i].randMax);
            Vector3 velocity = new Vector3(v, 0);
            Vector3 position = new Vector3(emmitters[i].position, 0);
            particle_create(position, velocity, emmitters[i].textures[emmitters[i].nextTex],
                            emmitters[i].friction, emmitters[i].sizeInit, emmitters[i].sizeMax, emmitters[i].life,
                            emmitters[i].grow, emmitters[i].shrink, emmitters[i].fade, emmitters[i].type);
        }
        #endregion
        #region particles
        public static void particle_update()
        {
            for (int i = 0; i != particles.Length; i++)
            {
                UpdateParticle(i);
            }
        }
        public static void particle_update(Vector2 offset)
        {
            //try
            //{
            if(particles !=null && particles.Length != 0)
                for (int i = 0; i < particles.Length; i++)
                {
                    Vector3 puddlePos = UpdateParticle(i);

                    if (puddlePos != Vector3.Zero)
                    {
                        effects.debrisEffect.addDebris(AnimoNex.game.effects.particles.blood_puddle[gfunc.roundUp(gfunc.RandomNumber(0, 1))].texture,
                                                       new Vector2(puddlePos.X, puddlePos.Y),
                                                       0,
                                                       debrisType.blood,
                                                       (byte)gfunc.RandomNumber(100, 140));

                    }
                }
            //}
            //catch { }
        }
        private static Vector3 UpdateParticle(int i)
        {
            Vector3 PuddlePos = Vector3.Zero;
            if (particles.Length > i)
            {
                particles[i].position += particles[i].Velocity;
                particles[i].angle += particles[i].angular_velo;
                particles[i].Velocity *= particles[i].friction;
                particles[i].angular_velo *= particles[i].friction;

                if (particles[i].type == particleType.blood ||
                    particles[i].type == particleType.shell ||
                    particles[i].type == particleType.gib)
                {
                    particles[i].Velocity.Z -= physics.phyFunc.gravity;
                }

                if (particles[i].life != -1)
                {
                    particles[i].lived++;
                }
                if (particles[i].grow)
                {
                    if (particles[i].size_current < particles[i].size_final)
                    {
                        particles[i].size_current += (particles[i].size_final / particles[i].life);
                    }
                }
                if (particles[i].shrink)
                {
                    if (particles[i].size_current > particles[i].size_final)
                    {
                        particles[i].size_current -= (particles[i].size_init / particles[i].life);
                    }
                }
                if (particles[i].position.Z > 0)
                {
                    if (particles[i].type == particleType.shell ||
                        particles[i].type == particleType.gib)
                    {
                        particles[i].Velocity.Z = -particles[i].Velocity.Z;
                        particles[i].position.Z = 0;
                        if (particles[i].type == particleType.shell)
                        {
                            weapFunc.PlayARShell();
                        }

                        #region createBloodPuddle
                        if (particles[i].type == particleType.gib)
                        {
                            CharFunc.PlayGib();
                            if (LvlFunc.isInRegion(new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-5, 5)), world.levels[world.i_currentLvl].regions[LvlFunc.findFloor(world.levels[world.i_currentLvl])]))
                            {
                                int numPuddles = AnimoNex.game.effects.particles.blood_puddle.Length - 1;
                                debrisEffect.addDebris(AnimoNex.game.effects.particles.blood_puddle[gfunc.roundUp(gfunc.RandomNumber(0, numPuddles))].texture,
                                new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-20, 20)),
                                0, debrisType.blood, 180);
                            }
                            else
                            {
                                int numPuddles = AnimoNex.game.effects.particles.blood_puddle_wall.Length - 1;
                                debrisEffect.addDebris(AnimoNex.game.effects.particles.blood_puddle_wall[gfunc.roundUp(gfunc.RandomNumber(0, numPuddles))].texture,
                                new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-20, 20)),
                                0, debrisType.blood, 180);
                            }
                        }
                        #endregion

                        if (Math.Abs(particles[i].Velocity.Z) < 7f)
                        {
                            if (particles[i].type == particleType.shell)
                            {
                                debrisEffect.addDebris(particles[i].texture, new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-20, 20)), particles[i].angle, debrisType.shell, 255);
                            }
                            if (particles[i].type == particleType.gib)
                            {

                                if (LvlFunc.isInRegion(new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-5, 5)), world.levels[world.i_currentLvl].regions[LvlFunc.findFloor(world.levels[world.i_currentLvl])]))
                                {
                                    #region createBloodPuddle
                                    int numPuddles = AnimoNex.game.effects.particles.blood_puddle.Length - 1;
                                    debrisEffect.addDebris(AnimoNex.game.effects.particles.blood_puddle[gfunc.roundUp(gfunc.RandomNumber(0, numPuddles))].texture,
                                                   new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-5, 5)),
                                                   0, debrisType.blood, 255);
                                    CharFunc.PlayGib();
                                    #endregion
                                    debrisEffect.addDebris(particles[i].texture, new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-20, -20)), particles[i].angle, debrisType.shell, 255);
                                }
                                else
                                {
                                    int numPuddles = AnimoNex.game.effects.particles.blood_puddle_wall.Length - 1;
                                    debrisEffect.addDebris(AnimoNex.game.effects.particles.blood_puddle_wall[gfunc.roundUp(gfunc.RandomNumber(0, numPuddles))].texture,
                                    new Vector2(particles[i].position.X, particles[i].position.Y + gfunc.RandomNumber(-20, 20)),
                                    0, debrisType.blood, 180);
                                }
                            }
                            particle_kill(i);
                        }

                    }
                    if(i<particles.Length)
                    if (particles[i].type == particleType.blood)
                    {
                        particles[i].lived = particles[i].life;
                    }
                }
                if (i < particles.Length)
                if (particles[i].life != -1)
                {
                    if (particles[i].lived >= particles[i].life)
                    {
                        if (particles[i].type == particleType.blood)
                        {

                            particle splash = AnimoNex.game.effects.particles.blood_splash[gfunc.roundUp(gfunc.RandomNumber(0, 2))];
                            splash.position = particles[i].position;
                            PuddlePos = splash.position;
                            CharFunc.PlayBloodSplat();
                            particle_create(splash);
                        }

                        particle_kill(i);

                    }
                }
            }
            return PuddlePos;
        }
        public static void particle_create(Vector3 pos, Vector3 velo, Texture2D tex, float friction, int sizeInit, int sizeMax, int life, bool Grow, bool Shrink, bool Fade, particleType ptype)
        {
            particle New = new particle(pos, velo, tex, friction, sizeInit, sizeMax, life, Grow, Shrink, Fade, ptype);

            if(particles != null && particles.Length !=0)
            {
                particle[] old = particles;
                particles = new particle[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    particles[i] = old[i];
                }
                particles[old.Length] = New;
            }
            else
            {
                particles = new particle[1];
                particles[0] = New;
            }
        }
        public static void particle_create(particle Particle)
        {
            particle New = Particle;

            if(particles != null && particles.Length !=0)
            {
                particle[] old = particles;
                particles = new particle[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    particles[i] = old[i];
                }
                particles[old.Length] = New;
            }
            else
            {
                particles = new particle[1];
                particles[0] = New;
            }
        }
        public static void particle_kill(int p)
        {
            //try
            {
                particle[] lower, upper;

                lower = new particle[p];
                upper = new particle[particles.Length - (p + 1)];

                for (int i = 0; i != lower.Length; i++)
                {
                    lower[i] = particles[i];
                }
                for (int i = p + 1; i != particles.Length; i++)
                {
                    upper[i - (p + 1)] = particles[i];
                }
                particles = new particle[particles.Length - 1];

                for (int i = 0; i != particles.Length; i++)
                {
                    if (i < lower.Length)
                    {
                        particles[i] = lower[i];
                    }
                    else
                    {
                        particles[i] = upper[i - lower.Length];
                    }
                }


            }
            //catch
            {

            }
        }
        public static Color particle_get_Fade(int p)
        {
            Color color = new Color(255, 255, 255, 255);
            if (particles[p].fade)
            {
                float x = Math.Abs(particles[p].life - particles[p].lived), y = particles[p].life;
                float iAlpha = x / y;
                byte alpha = Convert.ToByte(iAlpha * 255);
                color = new Color(255, 255, 255, alpha);
            }
            return color;
        }
        public static Rectangle particle_get_bounds(int p)
        {
            Vector2 textureDims = new Vector2(particles[p].texture.Width, particles[p].texture.Height);
            if (particles[p].size_current != -1)
            {
                textureDims.X = particles[p].size_current;
                textureDims.Y = particles[p].size_current;
            }
            Rectangle rectangle = new Rectangle(Convert.ToInt32(particles[p].position.X - .5f * (textureDims.X)),
                                                Convert.ToInt32(particles[p].position.Y - .5f * (textureDims.Y)),
                                                Convert.ToInt32(textureDims.X), Convert.ToInt32(textureDims.Y));
            return rectangle;
        }
        public static void particle_draw(SpriteBatch e, Vector2 offset)
        {
            //Game1.graphics.GraphicsDevice.SetRenderTarget(0,shaders.rt);

            if (particles != null && particles.Length != 0)
            {
                e.Begin(SpriteSortMode.Immediate, BlendState.Additive);

                for (int i = 0; i != particles.Length; i++)
                {
                    //Game1.graphics.GraphicsDevice.SetRenderTarget(0, shaders.rt);


                    Vector2 dims = new Vector2(particles[i].texture.Width, particles[i].texture.Height);
                    Vector2 pos = new Vector2(particles[i].position.X, particles[i].position.Y + particles[i].position.Z);
                    pos -= offset;
                    e.Draw(particles[i].texture, pos, null, particle_get_Fade(i), particles[i].angle, dims / 2, particles[i].size_current, SpriteEffects.None, 0);

                    //Game1.graphics.GraphicsDevice.SetRenderTarget(0, null);
                    //Texture2D tex = shaders.rt.GetTexture();


                    //shaders.DesaturateBegin();
                    //shaders.DesaturateEnd();
                    if (particles[i].type == particleType.blood)
                    {
                        //shaders.refractionBegin();
                        //shaders.refractionEnd();
                    }
                    if (particles[i].type == particleType.smoke)
                    {
                        shaders.dissapearBegin();
                        shaders.dissapearEnd();
                    }


                }


                e.End();

            }
            //catch { }
        }

        #endregion
        private static Vector2 randomVector(Vector2 min, Vector2 max)
        {
            Vector2 output = Vector2.Zero;
            output.X = RandomNumber(min.X, max.X);
            output.Y = RandomNumber(min.Y, max.Y);
            return output;
        }
        private static Random m_Rand = new Random();
        public static float RandomNumber(double min, double max)
        {
            return Convert.ToSingle((max - min) * m_Rand.NextDouble() + min);
        }
    }
    public struct particle
    {
        public Vector3 position;
        public float angle;
        public Vector3 Velocity;
        public float angular_velo;
        public particleType type;
        public Texture2D texture;
        public float friction;
        public float size_init;
        public float size_current;
        public float size_final;

        public int life;
        public int lived;
        public bool grow;
        public bool shrink;
        public bool fade;

        public particle(Vector3 pos, Texture2D tex, particleType Type)
        {
            type = Type;
            position = pos;
            Velocity = Vector3.Zero;
            texture = tex;
            friction = 0f;
            size_init = tex.Width;
            size_final = size_init;
            life = -1;              //-1 makes live forever
            grow = false;
            shrink = false;
            fade = false;
            angle = angular_velo = 0;
            lived = 0;
            size_current = size_init;
        }
        public particle(Vector3 pos, Vector3 velo, Texture2D tex, particleType Type)
        {
            type = Type;
            position = pos;
            Velocity = velo;
            texture = tex;
            friction = 0f;
            size_init = tex.Width;
            size_final = size_init;
            life = -1;              //-1 makes live forever
            grow = false;
            shrink = false;
            fade = false;
            angle = angular_velo = 0;
            lived = 0;
            size_current = size_init;
        }
        public particle(Vector3 pos, Vector3 velo, Texture2D tex, float Friction, float SideInit, float MaxInit, int Life, particleType Type)
        {
            position = pos;
            Velocity = velo;
            texture = tex;
            friction = Friction;
            size_init = SideInit;
            size_final = MaxInit;
            life = Life;              //-1 makes live forever
            grow = false;
            shrink = false;
            fade = false;
            type = Type;
            lived = 0;
            size_current = size_init;
            angle = angular_velo = 0;
        }
        public particle(Vector3 pos, Vector3 velo, Texture2D tex, float Friction, float SideInit, float MaxInit, int Life, bool Grow, bool Shrink, bool Fade, particleType Type)
        {
            position = pos;
            Velocity = velo;
            texture = tex;
            friction = Friction;
            size_init = SideInit;
            size_final = MaxInit;
            life = Life;              //-1 makes live forever
            grow = Grow;
            shrink = Shrink;
            fade = Fade;
            type = Type;
            lived = 0;
            size_current = size_init;
            angle = angular_velo = 0;
        }
    }
    public struct emmitter
    {
        public Texture2D[] textures;
        public particleType type;
        public bool grow, shrink, fade, cycle;
        public int nextTex, sizeInit, sizeMax, life;
        public float friction;
        public Vector2 position, direction, randMin, randMax;
        public emmitter(Vector2 Position, Texture2D[] Textures, int Life, particleType Type)
        {
            position = Position;
            direction = position;
            randMax = Vector2.One;
            randMin = -Vector2.One;
            textures = Textures;
            nextTex = 0;
            sizeInit = -1;
            sizeMax = -1; //-1 being texture size
            life = Life;
            friction = 0;
            grow = false;
            shrink = false;
            fade = false;
            cycle = false;
            type = Type;

        }
        public emmitter(Vector2 Position, Vector2 Target, Vector2 max, Vector2 min, Texture2D[] Textures, bool Cycle, int Life, float Friction, bool Grow, bool Shrink, bool Fade, int size_max, int size_inital, particleType Type)
        {
            position = Position;
            direction = Target; // target
            randMax = max;
            randMin = min;
            textures = Textures;
            nextTex = 0;
            sizeInit = size_inital;
            sizeMax = size_max; //-1 being texture size
            life = Life;
            friction = Friction;
            grow = Grow;
            shrink = Shrink;
            fade = Fade;
            cycle = Cycle;
            type = Type;
        }
    }
    public enum particleType
    {
        fire,
        smoke,
        sparks,
        blood,
        shell,
        gib
    };
}
