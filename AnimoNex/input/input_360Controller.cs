using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimoNex.input
{
    class Controller
    {
        static  GamePadState controller;
        static int time_rumbled = 0;
        static float rumble_r, rumble_l;
        static int delay = 5;
        static PlayerIndex[] pIndex = new PlayerIndex[4];
        static bool a = false;

        public static void initilize()
        {
            pIndex[0] = PlayerIndex.One;
            pIndex[1] = PlayerIndex.Two;
            pIndex[2] = PlayerIndex.Three;
            pIndex[3] = PlayerIndex.Four;
        }

        public static bool is_a(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.A == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_a_pressed(PlayerIndex player)
        {
            bool output = false;
            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.A == ButtonState.Pressed)
                {
                    if (!a)
                    {
                        output = true;
                        a = true;
                    }
                }
                else
                {
                    a = false;
                }
            }
            return output;
        }
        public static bool is_b(PlayerIndex player)
        {
                        bool output = false;
                        foreach (PlayerIndex p in pIndex)
                        {
                            controller = GamePad.GetState(p);
                            if (controller.Buttons.B == ButtonState.Pressed)
                            {
                                output = true;
                            }
                        }
            return output;
        }
        public static bool is_x(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.X == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_y(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.Y == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_start(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.Start == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_back(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.Back == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_rShoulder(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_lShoulder(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_rStick(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.RightStick == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_lStick(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.Buttons.LeftStick == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static float rTrigger(PlayerIndex player)
        {
            float output;
            controller = GamePad.GetState(player);
            output = controller.Triggers.Right;
            return output;
        }
        public static float lTrigger(PlayerIndex player)
        {
            float output;
            controller = GamePad.GetState(player);
            output = controller.Triggers.Left;
            return output;
        }
        public static Vector2 rStick(PlayerIndex player)
        {
            Vector2 output = Vector2.Zero;
            controller = GamePad.GetState(player);
            output = new Vector2(controller.ThumbSticks.Right.X, controller.ThumbSticks.Right.Y);
            return output;
        }
        public static Vector2 lStick(PlayerIndex player)
        {
            Vector2 output = Vector2.Zero;
            controller = GamePad.GetState(player);
            output = new Vector2(controller.ThumbSticks.Left.X, controller.ThumbSticks.Left.Y);
            return output;
        }
        public static bool is_Dup(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.DPad.Up == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_Ddown(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.DPad.Down == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_Dright(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.DPad.Right == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static bool is_Dleft(PlayerIndex player)
        {
            bool output = false;

            foreach (PlayerIndex p in pIndex)
            {
                controller = GamePad.GetState(p);
                if (controller.DPad.Left == ButtonState.Pressed)
                {
                    output = true;
                }
            }
            return output;
        }
        public static void set_rumble(PlayerIndex player,float r_magnitude,float l_magnitude,int Delay)
        {
            if (Game1.time_ticks - time_rumbled >= delay)
            {
                GamePad.SetVibration(player, l_magnitude, r_magnitude);

                rumble_r = r_magnitude;
                rumble_l = l_magnitude;

                if (r_magnitude != 0 || l_magnitude != 0)
                {
                    time_rumbled = Game1.time_ticks;
                    delay = Delay;
                }
            }
            else
            {
                if (r_magnitude != 0 || l_magnitude != 0)
                {
                    if (r_magnitude > rumble_r)
                    {
                        rumble_r = r_magnitude;
                    }
                    if (l_magnitude > rumble_l)
                    {
                        rumble_l = l_magnitude;
                    }
                    GamePad.SetVibration(player, rumble_l, rumble_r);
                }
            }
        }
    }
}
