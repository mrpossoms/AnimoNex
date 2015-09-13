using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnimoNex.types;

namespace AnimoNex.game
{
    class gfunc //gfunc or game functions, a series of useful methods that are used throughout the code.
    {
        public static int i_collision_accuracy = 4;

        #region MathMethods
        public static float PointAt(Vector2 obj, Vector2 target)
        {
            float f_ang;
            f_ang = (Single)Math.Atan2((double)(obj.Y - target.Y), (double)(obj.X - target.X));
            f_ang += (float)Math.PI;
            return f_ang;
        }
        public static int roundUp(float input)
        {
            int Out;
            int NonDecimal = (int)input;
            if (input - NonDecimal >= .5f)
            {
                Out = 1 + (int)input;
            }
            else
            {
                Out = (int)input;
            }
            return Out;
        }
        private static Random m_Rand = new Random();
        public static int[] sortBasedOnY(float[] input)
        {
            float smallest = -1;
            bool[] used = new bool[input.Length];
            int tempTag = 0;
            int[] tag = new int[input.Length];

            for (int i = 0; i != used.Length; i++) { used[i] = false; }

            for (int j = 0; j != input.Length; j++)
            {
                for (int i = 0; i != input.Length; i++)
                {
                    if (!used[i])
                    {
                        if (input[i] < smallest || smallest == -1)
                        {
                            smallest = input[i];
                            tempTag = i;
                        }
                    }
                }
                tag[j] = tempTag;
                used[tempTag] = true;
                smallest = -1;
            }
            return tag;
        }
        public static float RandomNumber(double min, double max)
        {
            float OUT = (float)((max - min) * m_Rand.NextDouble() + min);
            return OUT;
        }
        public static string RandomName(bool male)
        {
            string fullname = "";
            int first = (int)RandomNumber(0, 50);
            int last = (int)RandomNumber(0, 51);

            LastName Ln = (LastName)Enum.ToObject(typeof(LastName), last);
            FirstNameM Fn = (FirstNameM)Enum.ToObject(typeof(FirstNameM), first);

            fullname = Fn.ToString() + " " + Ln.ToString();

            return fullname;
        }
        public static float findVelocity(Vector2 vector)
        {
            float velocity = (float)Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));
            return velocity;
        }
        public static Vector2 randomPos(Vector2 min, Vector2 max)
        {
            Vector2 output = Vector2.Zero;
            output.X = Convert.ToSingle(RandomNumber(min.X, max.X));
            output.Y = Convert.ToSingle(RandomNumber(min.Y, max.Y));
            return output;
        }
        public static Vector2 TranslateOnAng(Vector2 pos, float velocity, float angle)
        {
            pos.X += (float)(velocity * Math.Cos(angle));
            pos.Y += (float)(velocity * Math.Sin(angle));

            return pos;
        }
        public static Vector2 TranslateOnAng(float velocity, float angle)
        {
            Vector2 pos = Vector2.Zero;
            pos.X = (float)(velocity * Math.Cos(angle));
            pos.Y = (float)(velocity * Math.Sin(angle));

            return pos;
        } 
        #endregion
        public class quality// : Texture2D 
        {
            public static Texture2D resize(Texture2D input, TextureQuality quality, GraphicsDevice e)
            {
                #region declaration_and_assignment
                Texture2D new_texture;                          //define the varible that will hold the new texture data
                Color[] pixelArray_old, pixelArray_new;         //define the color array's for the new (and old) texture
                int new_width, new_height;  //define variables for the width and height of the new texture
                float scaleFactor = 1;
                if (quality == TextureQuality.high) { scaleFactor = 1; }
                if (quality == TextureQuality.medium) { scaleFactor = .5f; }
                if (quality == TextureQuality.low) { scaleFactor = .25f; }
                if (quality == TextureQuality.lowest) { scaleFactor = .1f; }
                new_width = Convert.ToInt32(input.Width * scaleFactor);  //assign new resolution based on scalefactor
                new_height = Convert.ToInt32(input.Height * scaleFactor);

                pixelArray_old = new Color[input.Width * input.Height];
                pixelArray_new = new Color[new_width * new_height];     //define the pixel array for the new texture

                input.GetData<Color>(pixelArray_old);                   //assign values to the array
                #endregion



                int x = 0, y = 0;
                for (int i = 0; i != pixelArray_new.Length; i++)
                {
                    pixelArray_new[i] = pixelArray_old[x + y];
                    x += Convert.ToInt32(1 / scaleFactor);
                    if (x >= input.Width)
                    {
                        y += Convert.ToInt32(input.Width * (1 / scaleFactor));
                        x = Convert.ToInt32((1 / scaleFactor) - 1);
                    }
                }


                new_texture = new Texture2D(e, new_width, new_height);
                new_texture.SetData<Color>(pixelArray_new);     //assign to texture

                return new_texture;
            }
        }
        public static bool timeWaited(int tick, int time)
        {
            bool output = false;

            if ((Game1.time_ticks -tick) >= time)
            {
                output = true;
            }
            return output;
        }
        public class collision
        {
            public static Vector2[] MakeCollisionMap(Vector2 pos1, Texture2D tex1, int accuracy)
            {
                Vector2[] points1 = new Vector2[0];

                Color[] collisionData1 = new Color[tex1.Width * tex1.Height];
                tex1.GetData<Color>(collisionData1);

                for (int j = 1; j < tex1.Height; j += tex1.Height / accuracy)
                {
                    for (int i = 0; i < tex1.Width; i += tex1.Width / accuracy)
                    {
                        if (isPixelTransparent(new Vector2(i, j), tex1.Width, collisionData1, pos1) == false)
                        {
                            if (points1.Length != 0)
                            {
                                Vector2[] temp;
                                temp = points1;
                                points1 = new Vector2[temp.Length + 1];
                                for (int k = 0; k != temp.Length; k++)
                                {
                                    points1[k] = temp[k];
                                }
                                points1[temp.Length] = new Vector2(i, j) + pos1;
                            }
                            else
                            {
                                points1 = new Vector2[1];
                                points1[0] = new Vector2(i, j) + pos1;
                            }
                        }
                    }
                }
                return points1;
            }
            public static Vector2[] RotateCollisionMap(Vector2[] map, Vector2 pivot, float angle)
            {
                Vector2[] output = map;
                float old_Ang = angle;

                for (int i = 0; i != output.Length; i++)
                {
                    //angle = old_Ang;
                    float dist = Vector2.Distance(pivot, output[i]);
                    float AngOffset = PointAt(pivot, output[i]);
                    output[i].X = (dist * (float)Math.Cos(angle + AngOffset) + pivot.X);
                    output[i].Y = (dist * (float)Math.Sin(angle + AngOffset) + pivot.Y);
                }

               return output;
            }
            private static bool isPixelTransparent(Vector2 pos, int width, Color[] collisionDat, Vector2 del)
            {
                bool temp = true;

                if (collisionDat[Convert.ToInt32(pos.X + ((pos.Y - 1) * width))].A != 0)
                {
                    temp = false;
                    //pos += del;
                    //gv.spriteBatch.Draw(gv.textures[1], new Rectangle(Convert.ToInt32((pos.X - .5f * width)), Convert.ToInt32((pos.Y - .5f * width)), 1, 1), Color.Red);
                }
                return temp;
            }
            public static bool pixlesCollide(Vector2 pos1, GameTex tex1, Vector2 pos2, GameTex tex2, int accuracy)
            {
                bool collide = false;
                if (Vector2.Distance(pos1, pos2) < tex1.texture.Width / 2 + tex2.texture.Width / 2)
                {
                    for (int i = 0; i != tex1.collData.Length; i++)
                    {
                        for (int j = 0; j != tex2.collData.Length; j++)
                        {
                            if (Vector2.Distance(tex1.collData[i], tex2.collData[j]) <= tex2.texture.Width / accuracy)
                            {
                                collide = true;
                            }
                        }
                    }
                }

                return collide;
            }

        }
        public struct GameTex
        {
            public Texture2D texture;
            public Vector2[] collData;

            public GameTex(Texture2D Texture, Vector2[] CollData)
            {
                texture = Texture;
                collData = CollData;
            }
        }
        public enum TextureQuality
        {
            high = 0, medium = 1, low = 2, lowest = 3
        };
    }
}
