using Microsoft.Xna.Framework;
using AnimoNex.types;
using System;

namespace AnimoNex.game
{
    class camera
    {
        public static Vector2 v_pos;
        public static float scale = 1;
        static float f_deltaV = 60;
        public static bool Pointing_R = true;
        public static int levelChangedTick = 0;
        public static Vector2 offset()
        {
            Vector2 output = camera.v_pos * camera.scale + (Game1.v_resolution / 2);
            return output;
        }
        public static void follow(Base B_Object,float height,float angle,bool pointingRight)
        {
            Vector2 Object = B_Object.v_position - new Vector2(0, B_Object.f_Position_Z);

            Object.Y -= height / 2;
            Vector2 temp = Game1.v_resolution / 2;
            Vector2 oldCamPos = v_pos;

            if (pointingRight)
            {
                temp.X /= 4;
            }
            else
            {
                temp.X = Game1.v_resolution.X-(temp.X / 4);
            }
            Object -= temp;

            float f_velocity = (Vector2.Distance(v_pos, Object) / (f_deltaV * 0.1f));
            Vector2 newVelocity = gfunc.TranslateOnAng(f_velocity,gfunc.PointAt(v_pos, Object));  //find the new velocity
            newVelocity.Y += (Game1.v_resolution.Y / (float)Math.PI) * (float)Math.Sin(angle);

            int floor = LvlFunc.findFloor(world.levels[world.i_currentLvl]);

            v_pos.X += newVelocity.X;
            if (!LvlFunc.isInRegion(new Vector2(0, Game1.v_resolution.Y) + v_pos, world.levels[world.i_currentLvl].regions[floor]) ||
                !LvlFunc.isInRegion(Game1.v_resolution + v_pos, world.levels[world.i_currentLvl].regions[floor]))
            {
                v_pos.X = oldCamPos.X;
            }
            v_pos.Y += newVelocity.Y;
            if (!LvlFunc.isInRegion(new Vector2(0, Game1.v_resolution.Y)+v_pos, world.levels[world.i_currentLvl].regions[floor]) ||
                !LvlFunc.isInRegion(Game1.v_resolution + v_pos, world.levels[world.i_currentLvl].regions[floor]))
            {
                v_pos.Y = oldCamPos.Y;
            }

            //v_pos.Y += ;

        }
        public static void follow(Vector2 Object)
        {
            Vector2 temp = Game1.v_resolution / 2;
            Object -= temp;
            float f_velocity = (Vector2.Distance(v_pos, Object) / (f_deltaV * 0.1f));

            v_pos = gfunc.TranslateOnAng(v_pos, f_velocity, gfunc.PointAt(v_pos, Object));

        }
    }
}
