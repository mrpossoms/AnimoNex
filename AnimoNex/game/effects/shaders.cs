using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimoNex.game.effects
{
    class shaders
    {
        public static float deathAlpha = 0.1f;
        //public static RenderTarget2D rt = new RenderTarget2D(Game1.graphics.GraphicsDevice,
          //                                                  (int)Game1.v_resolution.X,
            //                                                (int)Game1.v_resolution.Y,
              //                                             0,
                //                                            SurfaceFormat.Color);                                                                                    
        public static Vector2 MoveInCircle(GameTime gameTime, float speed)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            float x = (float)Math.Cos(time);
            float y = (float)Math.Sin(time);

            return new Vector2(x, y);
        }
        public static void HighContrastBegin()
        {
            //Game1.hiContrast.Begin(SaveStateMode.None);
            //Game1.hiContrast.CurrentTechnique.Passes[0].Begin();
        }

        public static void HighContrastEnd()
        {
            //Game1.hiContrast.End();
            //Game1.hiContrast.CurrentTechnique.Passes[0].End();
        }
        public static void MainMenuBlurBegin()
        {
            //Game1.blurMainMenu.Begin();
            //Game1.blurMainMenu.CurrentTechnique.Passes[0].Begin();
        }
        public static void MainMenuBlurEnd()
        {
            //Game1.blurMainMenu.End();
            //Game1.blurMainMenu.CurrentTechnique.Passes[0].End();
        }
        public static void InvertBegin()
        {
            //Game1.invert.Begin();
            //Game1.invert.CurrentTechnique.Passes[0].Begin();
        }
        public static void InvertEnd()
        {
            //Game1.invert.End();
            //Game1.invert.CurrentTechnique.Passes[0].End();
        }
        public static void DesaturateBegin()
        {
            //Game1.desat.Begin(SaveStateMode.None );
            //Game1.desat.CurrentTechnique.Passes[0].Begin();
        }
        public static void DesaturateEnd()
        {
            //Game1.desat.End();
            //Game1.desat.CurrentTechnique.Passes[0].End();
        }
        public static void bloomBegin(float alpha)
        {
            Game1.bloom.Parameters["v"].SetValue(.005f);
            Game1.bloom.Parameters["a"].SetValue(alpha);


            //Game1.bloom.Begin();
            //Game1.bloom.CurrentTechnique.Passes[0].Begin();
        }
        public static void bloomEnd()
        {
            //Game1.bloom.End();
            //Game1.bloom.CurrentTechnique.Passes[0].End();
        }
        public static void lightFlickerBegin()
        {
            if(gfunc.roundUp(gfunc.RandomNumber(0,50))==1)
            Game1.lightFlicker.Parameters["light"].SetValue(gfunc.RandomNumber(0,.75));
            else
            Game1.lightFlicker.Parameters["light"].SetValue(0);

            //Game1.lightFlicker.Begin();
            //Game1.lightFlicker.CurrentTechnique.Passes[0].Begin();
        }
        public static void lightFlickerEnd()
        {
            Game1.lightFlicker.Parameters["light"].SetValue(gfunc.RandomNumber(0, 1));


            //Game1.lightFlicker.End();
            //Game1.lightFlicker.CurrentTechnique.Passes[0].End();
        }
        public static void dissapearBegin()
        {
            Game1.graphics.GraphicsDevice.Textures[1] = Game1.t_shad_waterFall;
                        Game1.disappear.Parameters["OverlayScroll"].SetValue(
                                                MoveInCircle(Game1.gt, 1f) * 0.1f);
            //Game1.disappear.Begin();
            //Game1.disappear.CurrentTechnique.Passes[0].Begin();
        }
        public static void dissapearEnd()
        {
            //Game1.disappear.End();
            //Game1.disappear.CurrentTechnique.Passes[0].End();
        }
        public static void refractionBegin()
        {
            Game1.graphics.GraphicsDevice.Textures[1] = Game1.t_shad_waterFall;
            Game1.refract.Parameters["DisplacementScroll"].SetValue(
                                    MoveInCircle(Game1.gt, .5f)*.2f);
            //Game1.refract.Begin();
            //Game1.refract.CurrentTechnique.Passes[0].Begin();
        }
        public static void refractionEnd()
        {
            //Game1.refract.End();
            //Game1.refract.CurrentTechnique.Passes[0].End();
        }
        public static void deathBegin()
        {
            if (deathAlpha < 1)
            {
                deathAlpha *= 1.01f;
            }
            Game1.graphics.GraphicsDevice.Textures[1] = Game1.t_shad_waterFall;
            Game1.death.Parameters["alpha"].SetValue(deathAlpha);
            Game1.death.Parameters["DisplacementScroll"].SetValue(
                                    MoveInCircle(Game1.gt, .5f)*.25f);
            //Game1.death.Begin();
            //Game1.death.CurrentTechnique.Passes[0].Begin();
        }
        public static void deathEnd()
        {
            //Game1.death.End();
            //Game1.death.CurrentTechnique.Passes[0].End();
        }

    }
}
