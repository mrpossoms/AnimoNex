using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnimoNex.types;
using AnimoNex.game.effects;

namespace AnimoNex.game
{
    class world
    {
        #region GameObjectRepository
        public static level[] levels;
        public static character[] characters;
        public static Animation[] animations;
        public static weapon[] weapons;
        public static item[] items;
        #endregion

        public static sky sky_game;
        public static int i_currentLvl;


        public static void updateSky()
        {

        }
        public static void drawSky(SpriteBatch sb)
        {
            Game1.graphics.GraphicsDevice.SetRenderTarget(Game1.rt);
            Game1.graphics.GraphicsDevice.Clear(Color.Transparent);

            int i_hour = DateTime.Now.Hour;
            byte alpha, alpha1;
            if (i_hour >= 0 && i_hour <= 12)
            {
                alpha = (byte)(21.25f * i_hour);
                alpha1=(byte)(255 / ((i_hour*.30f)+1));
            }
            else
            {
                alpha = (byte)(255 / (((i_hour-13) * .30f) + 1));
                alpha1 = (byte)(21.25f * i_hour);
            }
            sb.Begin();
                sb.Draw(sky_game.night, new Rectangle(0, 0, (int)Game1.v_resolution.X, (int)Game1.v_resolution.Y), new Color(255, 255, 255, alpha1));
                sb.Draw(sky_game.day, new Rectangle(0, 0, (int)Game1.v_resolution.X, (int)Game1.v_resolution.Y), new Color(255, 255, 255, alpha));
                sb.Draw(sky_game.back, new Vector2(-camera.v_pos.X * 0.1f, -100 * (1 + camera.v_pos.Y * 0.001f)), Color.Gray);
                sb.Draw(sky_game.mid, new Vector2(-camera.v_pos.X * 0.2f, -300 * (1 + camera.v_pos.Y * 0.001f)), Color.Gray);
                sb.Draw(sky_game.front, new Vector2(-camera.v_pos.X * 0.3f, -300 * (1 + camera.v_pos.Y * 0.001f)), Color.Gray);
                sb.Draw(sky_game.grad, Vector2.Zero, Color.White);
            sb.End();
        }


    }
    struct sky
    {
        public Texture2D grad;
        public Texture2D night, day, cloud;
        public Texture2D front, mid, back;
        public Vector2[] v_cloudPos;
        public sky(Texture2D Night, Texture2D Day, Texture2D Cloud, int Clouds)
        {
            grad = front = mid = back = null;
            night = Night;
            day = Day;
            cloud = Cloud;
            v_cloudPos = new Vector2[Clouds];
        }
    }
}
