using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using AnimoNex.input;
using AnimoNex.types;
using AnimoNex.game;
using AnimoNex.game.effects;
namespace AnimoNex
{
    class howto
    {
        public static Texture2D howtoTex;
        public static Texture2D controls;
        public static float timeWaited = 0;
        public static void draw(SpriteBatch sb,Texture2D tex1)
        {

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //shaders.MainMenuBlurBegin();
            sb.Draw(tex1, Vector2.Zero, Color.White);
            //shaders.MainMenuBlurEnd();
            sb.End();

            sb.Begin();
            if (timeWaited < 3)
            {
                if (timeWaited > 0.5f)
                {
                    if (Controller.is_a_pressed(PlayerIndex.One))
                        timeWaited = 3;
                }

                sb.Draw(howtoTex, Vector2.Zero, Color.White);
            }
            else
            {
                if (Controller.is_a_pressed(PlayerIndex.One))
                    timeWaited = 5;
                sb.Draw(controls, Vector2.Zero, Color.White);
            }
            timeWaited += 0.005f;
            if (timeWaited > 5)
            {
                Game1.currentMenu = Game1.activeMenu.main;
                timeWaited = 0;
            }

            sb.End();
        }

    }
}
