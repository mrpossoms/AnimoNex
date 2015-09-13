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
using AnimoNex.game.pda;


namespace AnimoNex.game
{
    class itemFunc
    {
        public static void createItem(item New,Vector2 pos, float z)
        {
            try
            {
                item[] old = world.levels[world.i_currentLvl].items;
                world.levels[world.i_currentLvl].items = new item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[world.i_currentLvl].items[i] = old[i];
                }

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                world.levels[world.i_currentLvl].items[old.Length] = New;

            }
            catch
            {
                world.levels[world.i_currentLvl].items = new item[1];

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                world.levels[world.i_currentLvl].items[0] = New;
            }
        }
        public static void createItem(item New, Vector2 pos, float z,Vector2 velocity)
        {
            try
            {
                item[] old = world.levels[world.i_currentLvl].items;
                world.levels[world.i_currentLvl].items = new item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[world.i_currentLvl].items[i] = old[i];
                }

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.v_velocity = velocity;
                New.B_base.f_Position_Z = z;
                world.levels[world.i_currentLvl].items[old.Length] = New;

            }
            catch
            {
                world.levels[world.i_currentLvl].items = new item[1];

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.v_velocity = velocity;
                New.B_base.f_Position_Z = z;
                world.levels[world.i_currentLvl].items[0] = New;
            }
        }
        public static void createItem(item New, Vector2 pos, float z,int level)
        {
            try
            {
                item[] old = world.levels[level].items;
                world.levels[level].items = new item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[level].items[i] = old[i];
                }

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                world.levels[level].items[old.Length] = New;

            }
            catch
            {
                world.levels[level].items = new item[1];

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                world.levels[level].items[0] = New;
            }
        }
        public static void createItem(item New, Vector2 pos, float z, int level,float quality)
        {
            try
            {
                item[] old = world.levels[level].items;
                world.levels[level].items = new item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[level].items[i] = old[i];
                }

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                New.quality = quality;
                world.levels[level].items[old.Length] = New;

            }
            catch
            {
                world.levels[level].items = new item[1];

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                New.quality = quality;
                world.levels[level].items[0] = New;
            }
        }
        public static void createItem(item New, Vector2 pos, float z, Vector2 velocity,int level)
        {
            try
            {
                item[] old = world.levels[level].items;
                world.levels[level].items = new item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[level].items[i] = old[i];
                }

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.v_velocity = velocity;
                New.B_base.f_Position_Z = z;
                world.levels[level].items[old.Length] = New;

            }
            catch
            {
                world.levels[level].items = new item[1];

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.v_velocity = velocity;
                New.B_base.f_Position_Z = z;
                world.levels[level].items[0] = New;
            }
        }
        public static inventory createItem_INVEN(inventory input, item New, Vector2 pos, float z)
        {
            try
            {
                item[] old = input.items;
                input.items = new item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    input.items[i] = old[i];
                }

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                input.items[old.Length] = New;
                input.CurWeight += New.weight;

            }
            catch
            {
                input.items = new item[1];

                New.B_base.f_elasticity = .50f;
                New.B_base.v_position = pos;
                New.B_base.f_Position_Z = z;
                input.items[0] = New;
                input.CurWeight += New.weight;
            }

            return input;
        }
        public static void killItem(int toKill)
        {
            item[] lower = new item[toKill];
            item[] upper = new item[world.levels[world.i_currentLvl].items.Length - (toKill + 1)];

            for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
            {
                if (i != toKill)
                {
                    if (i < toKill)
                    {
                        lower[i] = world.levels[world.i_currentLvl].items[i];
                    }
                    if (i > toKill)
                    {
                        upper[i] = world.levels[world.i_currentLvl].items[toKill - (i + 1)];
                    }
                }
            }

            world.levels[world.i_currentLvl].items = new item[lower.Length + upper.Length];

            for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
            {
                if (i < lower.Length)
                {
                    world.levels[world.i_currentLvl].items[i] = lower[i];
                }
                else
                {
                    world.levels[world.i_currentLvl].items[i] = upper[i - lower.Length];
                }
            }

        }
        public static void killItem(int toKill,int level)
        {
            
            item[] lower = new item[toKill];
            item[] upper = new item[world.levels[level].items.Length - (toKill + 1)];

            for (int i = 0; i != world.levels[level].items.Length; i++)
            {
                if (i != toKill)
                {
                    if (i < toKill)
                    {
                        lower[i] = world.levels[level].items[i];
                    }
                    if (i > toKill)
                    {
                        upper[i - (toKill + 1)] = world.levels[level].items[i];
                    }
                }
            }

            world.levels[level].items = new item[world.levels[level].items.Length - 1];

            for (int i = 0; i != world.levels[level].items.Length; i++)
            {
                if (i < lower.Length)
                {
                    world.levels[level].items[i] = lower[i];
                }
                else
                {
                    world.levels[level].items[i] = upper[i - lower.Length];
                }
            }

        }
        public static item[] killItem_INVEN(item[] input, int toKill)
        {
            item[] lower = new item[toKill];
            item[] upper = new item[input.Length - (toKill + 1)];

            for (int i = 0; i != input.Length; i++)
            {
                if (i != toKill)
                {
                    if (i < toKill)
                    {
                        lower[i] = input[i];
                    }
                    if (i > toKill)
                    {
                        upper[i - (toKill + 1)] = input[i];
                    }
                }
            }

            input = new item[lower.Length + upper.Length];

            for (int i = 0; i != input.Length; i++)
            {
                if (i < lower.Length)
                {
                    input[i] = lower[i];
                }
                else
                {
                    input[i] = upper[i - lower.Length];
                }
            }
            return input;

        }
        public static void update()
        {
            if (world.levels[world.i_currentLvl].items != null && world.levels[world.i_currentLvl].items.Length != 0)
            {
                for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
                {
                    world.levels[world.i_currentLvl].items[i].B_base = physics.phyFunc.update(world.levels[world.i_currentLvl].items[i].B_base);
                }
            }
        }
        public static void drawItem(SpriteBatch sb)
        {
            if (world.levels[world.i_currentLvl].items != null && world.levels[world.i_currentLvl].items.Length != 0)
            {
                for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
                {
                    sb.Begin();
                    //shaders.DesaturateBegin();
                        Vector2 dims = new Vector2(world.levels[world.i_currentLvl].items[i].texture.Width,world.levels[world.i_currentLvl].items[i].texture.Height);
                    
                        sb.Draw(world.levels[world.i_currentLvl].items[i].texture,
                                (world.levels[world.i_currentLvl].items[i].B_base.v_position-new Vector2(0,world.levels[world.i_currentLvl].items[i].B_base.f_Position_Z)) -camera.v_pos,
                                null,
                                Color.White,
                                world.levels[world.i_currentLvl].items[i].B_base.f_angle,
                                dims / 2,
                                camera.scale,
                                SpriteEffects.None,
                                0);
                        //shaders.DesaturateEnd();

                                
                    sb.End();
                }
            }
        }
        public static item createItemFromWeapon(weapon w)
        {
            item output = new item();
            for (int i = 0; i != world.weapons.Length; i++)
            {
                if (world.items[i].name == w.name)
                {
                    output = world.items[i];
                    output.quality = w.wear;
                }
            }
            return output;
        }
        public static int findInventoryItem(string name, character c)
        {
            int output = -1;
            for (int i = 0; i != c.Inventory.items.Length; i++)
            {
                if (c.Inventory.items[i].name == name)
                {
                    output = i;
                }
            }
            return output;
        }
    }
}
