using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnimoNex.game;
using AnimoNex.types;

namespace AnimoNex.game.effects
{
    class debrisEffect
    {
        public static void addDebris(Texture2D tex,Vector2 position,float angle,debrisType type,byte alpha)
        {
            //try
            if(world.levels[world.i_currentLvl].debris_current != null && world.levels[world.i_currentLvl].debris_current.Length > 0)
            {
                debris[] old = world.levels[world.i_currentLvl].debris_current;

                world.levels[world.i_currentLvl].debris_current = new debris[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[world.i_currentLvl].debris_current[i] = old[i];
                }
                world.levels[world.i_currentLvl].debris_current[old.Length] = new debris(position, tex, new Color(255, 255, 255, alpha), type);
                world.levels[world.i_currentLvl].debris_current[old.Length].angle = angle;
            }
            else
            {
                world.levels[world.i_currentLvl].debris_current = new debris[1];
                world.levels[world.i_currentLvl].debris_current[0] = new debris(position, tex, new Color(255, 255, 255, alpha), type);
                world.levels[world.i_currentLvl].debris_current[0].angle = angle;
            }
        }
        public static void draw(SpriteBatch sb)
        {
            if (world.levels[world.i_currentLvl].debris_current != null &&
                world.levels[world.i_currentLvl].debris_current.Length != 0)
            {
                sb.Begin();//SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                for (int i = 0; i != world.levels[world.i_currentLvl].debris_current.Length; i++)
                {
                    debris temp = world.levels[world.i_currentLvl].debris_current[i];
                    sb.Draw(temp.texture,
                            temp.position - camera.v_pos,
                            null,
                            temp.color,
                            temp.angle,
                            new Vector2(temp.texture.Width,temp.texture.Height)/2,
                            1,
                            SpriteEffects.None,
                            0);
                }
                sb.End();
            }
        }
    }
}
