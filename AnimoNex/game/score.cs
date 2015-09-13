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
using AnimoNex.game.pda;
using AnimoNex.game.effects;

namespace AnimoNex.game
{
    class scoreFunc
    {
        public static Texture2D scoreSplat;
        public static int PlayerScore = 0;
        public static void drawScore(SpriteBatch sb)
        {
            sb.Begin();
                sb.Draw(scoreSplat, new Vector2(Game1.v_resolution.X, 0), null, Color.White, 0, new Vector2(scoreSplat.Width, 0), 1, SpriteEffects.None, 0);
                sb.DrawString(UI.pump_font,"Score: " + PlayerScore.ToString(),new Vector2(Game1.v_resolution.X-200,20),Color.White);
            sb.End();
        }
    }
}
