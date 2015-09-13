using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using AnimoNex.game;
using AnimoNex.types;

namespace AnimoNex.physics
{
    class phyFunc
    {
        public static float gravity = -0.6f; //M/s2

        public static Base update(Base Object)
        {
            Object = Accelerate(Object);
            Object = Translate(Object);

            return Object;
        }
        public static Base updateProjectile(Base Object)
        {
            Object = AccelerateProj(Object);
            Object = TranslateProj(Object);

            return Object;
        }

        private static Base Translate(Base Object)
        {
            Vector2 oldPos = Object.v_position;

            
            Object.f_Position_Z += Object.f_Velocity_Z;
            Object.f_angle += Object.f_angular_velocity;

            Object.v_position.X += Object.v_velocity.X;
            if (!LvlFunc.isInRegion(Object.v_position, regionType.floor))
            {
                Object.v_position.X = oldPos.X;
                //Object.v_velocity = Vector2.Zero;
            }
            Object.v_position.Y += Object.v_velocity.Y;
            if (!LvlFunc.isInRegion(Object.v_position, regionType.floor))
            {
                Object.v_position.Y = oldPos.Y;
                //Object.v_velocity = Vector2.Zero;
            }

            return Object;
        }

        private static Base Accelerate(Base Object)
        {

            Object.v_velocity *= .4f;
            //Object.f_angular_velocity *= .98f;

            if (Object.f_Position_Z > 0.1f)
            {
                Object.f_Velocity_Z += gravity;
                Object.airBorn = true;
                Object.v_velocity *= 1.6f;

                if (Object.f_angular_velocity == 0)
                {
                    Object.f_angular_velocity = gfunc.RandomNumber(-0.1, 0.1);
                }
            }
            else
            {
                Object.airBorn = false;

                if (Object.f_Velocity_Z < 0.5f)
                {

                    if (Math.Abs(Object.f_Velocity_Z) > 0.1)
                    {
                        Object.f_Velocity_Z = -(Object.f_Velocity_Z * Object.f_elasticity);
                    }
                    else
                    {
                        Object.f_Velocity_Z = 0;
                    }

                    Object.f_angular_velocity = 0;
                    if(Object.f_Velocity_Z!=0)
                    {
                        if (Object.f_angle < (float)Math.PI / 4)
                        {
                            Object.f_angle = 0 + gfunc.RandomNumber(-0.1, 0.1);
                        }
                        else
                        {
                            Object.f_angle = (float)Math.PI + gfunc.RandomNumber(-0.1, 0.1);
                        }
                    }
                }
            }

            return Object;
        }

        private static Base TranslateProj(Base Object)
        {
            Vector2 oldPos = Object.v_position;

            Object.v_position += Object.v_velocity;
            Object.f_Position_Z += Object.f_Velocity_Z;


            return Object;
        }
        private static Base AccelerateProj(Base Object)
        {

            //Object.v_velocity *= .4f;

            if (Object.f_Position_Z > 0.1f)
            {
                //Object.f_Velocity_Z += gravity;
                Object.airBorn = true;
                //Object.v_velocity *= 1.6f;
            }
            else
            {
                Object.airBorn = false;
            }

            return Object;
        }

    }
}
