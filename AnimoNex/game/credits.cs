using System;
using System.Collections.Generic;
using System.Threading;
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
using AnimoNex.game.pda;
using AnimoNex.game.effects;
using AnimoNex.Initialize;

namespace AnimoNex.game
{
    class credits
    {
        public static Texture2D kirk,jake,nick;
        public static float progress = 0;
        public static void update()
        {
            progress += 0.0025f;
            if (progress >= 3)
            {
                Game1.currentMenu = Game1.activeMenu.main;
            }
        }
        public static void draw(SpriteBatch sb,Texture2D tex1)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //shaders.MainMenuBlurBegin();
            sb.Draw(tex1, Vector2.Zero, Color.White);
            //shaders.MainMenuBlurEnd();
            sb.End();

            sb.Begin();
            if (progress < 1)
            {
                sb.Draw(kirk, new Rectangle(0, 0, (int)Game1.v_resolution.X, (int)Game1.v_resolution.Y), Color.White);
            }
            if (progress >= 1 && progress < 2)
            {
                sb.Draw(jake, new Rectangle(0, 0, (int)Game1.v_resolution.X, (int)Game1.v_resolution.Y), Color.White);
            }
            if (progress >= 2 && progress < 3)
            {
                sb.Draw(nick, new Rectangle(0, 0, (int)Game1.v_resolution.X, (int)Game1.v_resolution.Y), Color.White);
            }
            sb.End();
        }
    }
}
