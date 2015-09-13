using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using AnimoNex.types;
using AnimoNex.game.effects;
using AnimoNex.game.pda;

namespace AnimoNex.game
{
    class weapFunc
    {
        public static Projectile[] projectiles = new Projectile[0];
        public static Texture2D tracer;
        public static Cue ar_shell, pistol_shell;
        public static Cue fire_9mm, fire_ar, fire_22, fire_12, fire_357, fire_50;
        public static Cue reload_22, reload_ar, reload_pistol, reload_revolver, reload_shotgun;
        public static Cue weap_dry;
        public static character reload(character c)
        {
            if (c.current_weap.name != null && c.current_weap.name != "unarmed")
            {
                int tagFound = -1;


                tagFound = FindAmmoType(c);

                if (tagFound != -1)
                {
                    if (c.Inventory.items[tagFound].quality > 0)
                    {
                        float oldQuanity = c.Inventory.items[tagFound].quality;
                        c.Inventory.items[tagFound].quality = c.Inventory.items[tagFound].quality - (c.current_weap.clipSize - c.current_weap.clipCurrent);
                        if (oldQuanity > 0)
                        {
                            c.current_weap.clipCurrent += (int)(oldQuanity - (oldQuanity - (c.current_weap.clipSize - c.current_weap.clipCurrent)));
                        }
                        c.current_weap.wear = 0;

                        if (c.Inventory.items[tagFound].quality <= 0)
                        {
                            c.Inventory.items = itemFunc.killItem_INVEN(c.Inventory.items, tagFound);
                        }
                        //int quanity = (int)c.Inventory.items[tagFound].quality - c.current_weap.clipSize;
                        //c.Inventory.items[tagFound].quality = c.current_weap.clipCurrent;
                        //c.current_weap.clipCurrent = quanity;
                    }
                }
                //itemFunc.killItem_INVEN
            }
            return c;
        }
        public static void PlayReloadShotgun()
        {
            reload_shotgun = sounds.soundBank.GetCue("reload_shotgun");
            reload_shotgun.Play();
        }
        public static void PlayReloadRevolver()
        {
            reload_revolver = sounds.soundBank.GetCue("reload_revolver");
            reload_revolver.Play();
        }
        public static void PlayReloadPistol()
        {
            reload_pistol = sounds.soundBank.GetCue("reload_pistol");
            reload_pistol.Play();
        }
        public static void PlayReloadAR()
        {
            reload_ar = sounds.soundBank.GetCue("reload_ar");
            reload_ar.Play();
        }
        public static void PlayReload22()
        {
            reload_22 = sounds.soundBank.GetCue("reload_22");
            reload_22.Play();
        }
        public static void PlayFire50()
        {
            fire_50 = sounds.soundBank.GetCue("fire_50");
            fire_50.Play();
        }
        public static void PlayFire357()
        {
            fire_357 = sounds.soundBank.GetCue("fire_38");
            fire_357.Play();
        }
        public static void PlayFire12()
        {
            fire_12 = sounds.soundBank.GetCue("fire_12");
            fire_12.Play();
        }
        public static void PlayFireAR()
        {
            fire_ar = sounds.soundBank.GetCue("fire_ar");
            fire_ar.Play();
        }
        public static void PlayFire22()
        {
            fire_22 = sounds.soundBank.GetCue("fire_22");
            fire_22.Play();
        }
        public static void PlayDryFire()
        {
                weap_dry = sounds.soundBank.GetCue("weap_dry");
                weap_dry.Play();
        }
        public static void PlayFire9mm()
        {
            fire_9mm = sounds.soundBank.GetCue("fire_9mm");
            fire_9mm.Play();
        }
        public static void PlayPistolShell()
        {
            pistol_shell = sounds.soundBank.GetCue("shell_pistol");
            pistol_shell.Play();
        }
        public static void PlayARShell()
        {
            ar_shell = sounds.soundBank.GetCue("shell_ar");
            ar_shell.Play();
        }

        public static int FindAmmoType(character c)
        {
            string target = "";
            int output = -1;
            #region FindAmmoName
            switch (c.current_weap.Pt)
            {
                case ProjectileType.AR223:
                    target = "ammo_223";
                    break;
                case ProjectileType.AR762:
                    target = "ammo_762";
                    break;
                case ProjectileType.CAR22:
                    target = "ammo_22";
                    break;
                case ProjectileType.P357:
                    target = "ammo_357";
                    break;
                case ProjectileType.P38:
                    target = "ammo_38";
                    break;
                case ProjectileType.SG_pellet:
                    target = "ammo_12";
                    break;
                case ProjectileType.SMG45:
                    target = "ammo_45";
                    break;
                case ProjectileType.SMG9:
                    target = "ammo_9";
                    break;
                case ProjectileType.SR308:
                    target = "ammo_308";
                    break;
                case ProjectileType.SR50:
                    target = "ammo_50";
                    break;
            }
            #endregion
            if (c.Inventory.items != null)
            {
                for (int i = 0; i != c.Inventory.items.Length; i++)
                {
                    if (c.Inventory.items[i].gameTag == target)
                    {
                        if (c.Inventory.items[i].quality > 0)
                        {
                            output = i;
                        }
                    }
                }
            }
            return output;
        }
        public static int FindAmmoType(character c, weapon w)
        {
            string target = "";
            int output = -1;
            #region FindAmmoName
            switch (w.Pt)
            {
                case ProjectileType.AR223:
                    target = "ammo_223";
                    break;
                case ProjectileType.AR762:
                    target = "ammo_762";
                    break;
                case ProjectileType.CAR22:
                    target = "ammo_22";
                    break;
                case ProjectileType.P357:
                    target = "ammo_357";
                    break;
                case ProjectileType.P38:
                    target = "ammo_38";
                    break;
                case ProjectileType.SG_pellet:
                    target = "ammo_12";
                    break;
                case ProjectileType.SMG45:
                    target = "ammo_45";
                    break;
                case ProjectileType.SMG9:
                    target = "ammo_9";
                    break;
                case ProjectileType.SR308:
                    target = "ammo_308";
                    break;
                case ProjectileType.SR50:
                    target = "ammo_50";
                    break;
            }
            #endregion
            if (c.Inventory.items != null)
            {
                for (int i = 0; i != c.Inventory.items.Length; i++)
                {
                    if (c.Inventory.items[i].gameTag == target)
                    {
                        if (c.Inventory.items[i].quality != 0)
                        {
                            output = i;
                        }
                    }
                }
            }
            return output;
        }
        public static character FireShot(character c, Vector2 Muzzle_pos, float ang, ProjectileType pt, int charTag)
        {
            Vector2 pos = Muzzle_pos;
            float Z = 10;   //change later
            float velocity = 0;
            int damage = 0;

            //add velocities for each type of ammunition
            SetBulletProperties(pt, ref velocity, ref damage);

            velocity /= 5;

            Vector2 V_velo = gfunc.TranslateOnAng(velocity, ang);

            Projectile bullet = new Projectile(tracer,
                                               new Base(pos, Z, ang, V_velo, 0, 0.01f),
                                               pt, 1000);
            bullet.damage = damage;
            if (c.angle > -.5f)
            {
                c.angle -= 0.002f * damage;//recoil
            }
            else
            {
                c.angle = -.49f;
            }
            bullet.owner = charTag;
            bullet.B_base.airBorn = true;
            CreateBullet(bullet);
            return c;
        }
        private static void SetBulletProperties(ProjectileType pt, ref float velocity, ref int damage)
        {
            #region BulletVelocities
            switch (pt)
            {
                case ProjectileType.AR223:
                    velocity = 200.34f;
                    damage = 35;
                    break;

                case ProjectileType.AR762:
                    velocity = 175.04f;
                    damage = 40;
                    break;

                case ProjectileType.CAR22:
                    velocity = 150.28f;
                    damage = 10;
                    break;

                case ProjectileType.P357:
                    velocity = 100.86f;
                    damage = 30;
                    break;

                case ProjectileType.P38:
                    velocity = 100.03f;
                    damage = 25;
                    break;

                case ProjectileType.SG_pellet:
                    velocity = 150.04f;
                    damage = 10;
                    break;

                case ProjectileType.SMG45:
                    velocity = 125.07f;
                    damage = 25;
                    break;

                case ProjectileType.SMG9:
                    velocity = 125.32f;
                    damage = 15;
                    break;

                case ProjectileType.SR308:
                    velocity = 250.87f;
                    damage = 45;
                    break;

                case ProjectileType.SR50:
                    velocity = 250.44f;
                    damage = 50;
                    break;

                case ProjectileType.debug:
                    velocity = 130f;
                    damage = 10;
                    break;
            }
            #endregion
        }
        public static weapon FindWeapon(string name)
        {
            weapon output = new weapon();
            for (int i = 0; i != world.weapons.Length; i++)
            {
                if (world.weapons[i].name == name)
                {
                    output = world.weapons[i];
                }
            }
            return output;
        }
        public static void CreateBullet(Projectile bullet)
        {
            if(projectiles != null && projectiles.Length >0)
            {
                Projectile[] old = projectiles;
                projectiles = new Projectile[old.Length+1];

                for (int i = 0; i != old.Length; i++)
                {
                    projectiles[i] = old[i];
                }

                projectiles[old.Length] = bullet;
            }
            else
            {
                projectiles = new Projectile[1];
                projectiles[0] = bullet;
            }
        }
        public static void KillBullet(int b)
        {
            Projectile[] lower = new Projectile[b];
            Projectile[] upper = new Projectile[projectiles.Length - (b+1)];

            for (int i = 0; i != projectiles.Length; i++)
            {
                if (i < b)
                {
                    lower[i] = projectiles[i];
                }
                if (i > b)
                {
                    upper[i-lower.Length-1] = projectiles[i];
                }
            }
            projectiles = new Projectile[lower.Length + upper.Length];
            for (int i = 0; i != projectiles.Length; i++)
            {
                if (i < lower.Length)
                {
                    projectiles[i] = lower[i];
                }
                else
                {
                    projectiles[i] = upper[i-lower.Length];
                }

            }
        }
        public static void UpdateBullets()
        {
            if (projectiles != null && projectiles.Length != 0)
            {
                for (int i = 0; i < projectiles.Length; i++)
                {
                        if (projectiles[i].lived < projectiles[i].maxLife/10 && projectiles[i].B_base.airBorn)
                        {
                            projectiles[i].B_base = physics.phyFunc.updateProjectile(projectiles[i].B_base);
                            projectiles[i].lived++;
                            bool coll = false;
                            for (int j = 0; j != world.levels[world.i_currentLvl].char_living.Length; j++)
                            {
                                character old = world.levels[world.i_currentLvl].char_living[j];
                                world.levels[world.i_currentLvl].char_living[j] = CharFunc.collided_projectile(world.levels[world.i_currentLvl].char_living[j], j, projectiles[i]);
                                if (world.levels[world.i_currentLvl].char_living[j].attrib.hp_current != old.attrib.hp_current)
                                {
                                    coll = true;
                                }

                            }
                            if (coll)
                            {
                                KillBullet(i);
                            }
                        }
                        else
                        {
                            KillBullet(i);
                        }
                }
            }
        }
        public static void drawProjectiles(SpriteBatch sb)
        {
            if (projectiles != null && projectiles.Length != 0)
            {
                sb.Begin();
                for (int i = 0; i != projectiles.Length; i++)
                {
                    sb.Draw(projectiles[i].texture,
                                        (projectiles[i].B_base.v_position - new Vector2(0, projectiles[i].B_base.f_Position_Z) - camera.v_pos),
                                        null,
                                        Color.White,
                                        projectiles[i].B_base.f_angle,//+(float)Math.PI,
                                        new Vector2(projectiles[i].texture.Width / 2, projectiles[i].texture.Height / 2),//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                                        camera.scale,
                                        SpriteEffects.None,
                                        0);
                }
                sb.End();
            }
        }
        public static Vector2 findMuzzlePos(Vector2 player, Vector2 hand, Vector2 muzzle, float angle,bool right)
        {
            float dist = Vector2.Distance(hand, hand + muzzle);
            Vector2 resultant = Vector2.Zero;

            resultant = new Vector2(dist * (float)Math.Cos(angle) + hand.X,
                                dist * (float)Math.Sin(angle) + hand.Y);

            if (!right)
            {
                resultant.X += (player.X - (hand.X + -muzzle.X)) * 2; 
            }


            return resultant;
        }
        public static float drawWeapon(SpriteBatch sb,character c,int bodyPrt, bool right)
        {
            Vector2 gripPivot = c.current_weap.gripPos;
            Vector2 clipPos = c.current_weap.clipPos;
            Vector2 muzzPos = c.current_weap.muzzPos;
            Vector2 actionPos = c.current_weap.actionPos;
            muzzPos.Y += c.current_weap.muzzleFlash[0].Height / 2;
            Vector2 handPos = c.body[bodyPrt].position;
            Texture2D tempTex = c.current_weap.bolt_forward;
            int muzzTex = 0;
            if (c.current_weap.shooting || c.current_weap.clipCurrent ==0)
            {
                tempTex = c.current_weap.bolt_back;
                muzzTex = (int)gfunc.RandomNumber(0, 2);
            }

            float angle_out = 0;

            if (right)
            {
                if(c.action != "reload")
                sb.Draw(c.current_weap.clip,
                    (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z) - camera.v_pos) + handPos,
                    null,
                    Color.White,
                    c.body[bodyPrt].angle,//+(float)Math.PI,
                    clipPos,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                    camera.scale,
                    SpriteEffects.None,
                    0);
                sb.Draw(tempTex,
                                    (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z) - camera.v_pos) + handPos,
                                    null,
                                    Color.White,
                                    c.body[bodyPrt].angle,//+(float)Math.PI,
                                    gripPivot,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                                    camera.scale,
                                    SpriteEffects.None,
                                    0);
                if (c.current_weap.shooting)
                {
                    sb.Draw(c.current_weap.muzzleFlash[muzzTex],
                        (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z) - camera.v_pos) + handPos,
                        null,
                        Color.White,
                        c.body[bodyPrt].angle,//+(float)Math.PI,
                        muzzPos,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                        camera.scale,
                        SpriteEffects.None,
                        0);

                }

                angle_out = c.body[bodyPrt].angle;

            }
            else
            {
                gripPivot.X = c.current_weap.bolt_forward.Width - gripPivot.X;
                clipPos.X = c.current_weap.clip.Width - clipPos.X;
                muzzPos.X = c.current_weap.muzzleFlash[0].Width - muzzPos.X;
                handPos.X = -handPos.X;
                if (c.action != "reload")
                sb.Draw(c.current_weap.clip,
                    (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z) - camera.v_pos) + handPos,
                    null,
                    Color.White,
                    -c.body[bodyPrt].angle,//+(float)Math.PI,
                    clipPos,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                    camera.scale,
                    SpriteEffects.FlipHorizontally,
                    0);
                sb.Draw(tempTex,
                                    (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z) - camera.v_pos) + handPos,
                                    null,
                                    Color.White,
                                    -c.body[bodyPrt].angle,//+(float)Math.PI,
                                    gripPivot,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                                    camera.scale,
                                    SpriteEffects.FlipHorizontally,
                                    0);
                if (c.current_weap.shooting)
                {
                    //muzzPos.X = -muzzPos.X;
                    sb.Draw(c.current_weap.muzzleFlash[muzzTex],
                        (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z) - camera.v_pos) + handPos,
                        null,
                        Color.White,
                        -c.body[bodyPrt].angle,//+(float)Math.PI,
                        muzzPos,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                        camera.scale,
                        SpriteEffects.FlipHorizontally,
                        0);
                }
                angle_out = -c.body[bodyPrt].angle + (float)Math.PI;
            }
            return angle_out;
        }
    }
}
