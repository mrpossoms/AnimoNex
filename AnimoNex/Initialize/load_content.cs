using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AnimoNex.types;
using AnimoNex.game;
using AnimoNex.game.pda;

namespace AnimoNex.Initialize
{
    class load_content
    {
        public static string BasePath;
        public static level[] LoadLevels(ContentManager content) //path takes you to "\world\levels\"
        {
            StreamReader sr;                              //sr is the obj created to help us read file contents.
            #region FindNumberOfLevels
            int i_levelCount = 0; bool b_invalid = false; //these vars are created to help find the number of levels in the dir.

            while (!b_invalid)
            {
                #region CheckValidity
                try
                {
                    sr = new StreamReader(content.RootDirectory + "/world/levels/" + i_levelCount.ToString() + ".alvl");
                }
                catch
                {
                    b_invalid = true;
                }
                #endregion
                #region KeepCount
                if (!b_invalid)
                {
                    i_levelCount++;
                }
                #endregion
            }
            #endregion

            level[] l_out = new level[i_levelCount];      //variable is created that is large enough to hold the world

            #region LoadLevels
            for (int i = 0; i != i_levelCount; i++)
            {
                sr = new StreamReader(BasePath+ "/world/levels/" + i.ToString() + ".alvl");
                //try
                //{
                    l_out[i].name = sr.ReadLine();
                    l_out[i].props = new prop[Convert.ToInt32(sr.ReadLine())];
                    for (int j = 0; j != l_out[i].props.Length; j++)
                    {
                        l_out[i].props[j].name = sr.ReadLine();
                        l_out[i].props[j].texture_path = sr.ReadLine();
                        l_out[i].props[j].texture = content.Load<Texture2D>(l_out[i].props[j].texture_path);
                        l_out[i].props[j].X = Convert.ToInt32(Convert.ToInt32(sr.ReadLine()));// * vars.globalScale);
                        l_out[i].props[j].Y = Convert.ToInt32(Convert.ToInt32(sr.ReadLine()));// * vars.globalScale);
                        l_out[i].props[j].angle = Convert.ToSingle(sr.ReadLine());
                        l_out[i].props[j].scale = Convert.ToSingle(sr.ReadLine());
                    }
                //}
                //catch { l_out[i].props = new prop[0]; }
                try
                {
                    l_out[i].regions = new region[Convert.ToUInt32(sr.ReadLine())];
                    for (int j = 0; j != l_out[i].regions.Length; j++)
                    {
                        l_out[i].regions[j].name = sr.ReadLine();
                        #region GetType
                        string temp = sr.ReadLine();
                        if (temp == "floor")
                        {
                            l_out[i].regions[j].type = regionType.floor;
                        }
                        if (temp == "building")
                        {
                            l_out[i].regions[j].type = regionType.building;
                        }
                        if (temp == "portal")
                        {
                            l_out[i].regions[j].type = regionType.portal;
                        }
                        #endregion
                        l_out[i].regions[j].corners = new Vector2[4];

                        #region DealWithCornerShit
                        Vector2 smallest = Vector2.Zero;
                        Vector2 largest = Vector2.Zero;
                        for (int k = 0; k != 4; k++)
                        {
                            l_out[i].regions[j].corners[k] = new Vector2(Convert.ToSingle(sr.ReadLine()), Convert.ToSingle(sr.ReadLine()));
                            if (l_out[i].regions[j].corners[k].X <= smallest.X || smallest.X == 0)
                            {
                                if (l_out[i].regions[j].corners[k].Y <= smallest.Y || smallest.Y == 0)
                                {
                                    smallest = l_out[i].regions[j].corners[k];
                                }
                            }
                            if (l_out[i].regions[j].corners[k].X >= largest.X || largest.X == 0)
                            {
                                if (l_out[i].regions[j].corners[k].Y >= largest.Y || largest.Y == 0)
                                {
                                    largest = l_out[i].regions[j].corners[k];
                                }
                            }
                        }
                        l_out[i].regions[j].corners = new Vector2[2];
                        l_out[i].regions[j].corners[0] = smallest;
                        l_out[i].regions[j].corners[1] = largest; 
                        #endregion
                        l_out[i].regions[j].special = sr.ReadLine();
                        l_out[i].regions[j].toLevel = sr.ReadLine();

                    }
                }
                catch { l_out[i].regions = new region[0]; }
                l_out[i].row = Convert.ToInt32 (sr.ReadLine());
                l_out[i].column = Convert.ToInt32(sr.ReadLine());
                l_out[i].indoor = Convert.ToBoolean(sr.ReadLine());
            }
            #endregion

            return l_out;
        }
        public static Animation[] LoadAnims(ContentManager content)
        {
            StreamReader sr;
            BasePath = content.RootDirectory;
            sr = new StreamReader(BasePath + "/world/animations/animList.txt");
            int i_numAnim = Convert.ToInt32(sr.ReadLine());

            Animation[] ani_out = new Animation[i_numAnim];

            for (int i = 0; i != i_numAnim; i++)
            {
                string name =sr.ReadLine();
                StreamReader sr1 = new StreamReader(BasePath + "/world/animations/" + name + ".anim");
                ani_out[i].f_frame = new frame[Convert.ToInt32(sr1.ReadLine())];
                ani_out[i].name = name;
                for (int k = 0; k != ani_out[i].f_frame.Length; k++)
                {
                    int components = Convert.ToInt32(sr1.ReadLine());
                    ani_out[i].f_frame[k].angle = new float[components];
                    ani_out[i].f_frame[k].position = new Vector2[components];

                    for (int j = 0; j != components; j++)
                    {
                        ani_out[i].f_frame[k].angle[j] = Convert.ToSingle(sr1.ReadLine());
                        ani_out[i].f_frame[k].position[j].X = Convert.ToSingle(sr1.ReadLine());
                        ani_out[i].f_frame[k].position[j].Y = Convert.ToSingle(sr1.ReadLine());
                        ani_out[i].f_frame[k].key_frame = Convert.ToBoolean(sr1.ReadLine());
                    }

                }
                sr1.Close();
            }
            sr.Close();

            return ani_out;
        }
        public static character[] LoadCharacters(ContentManager content)
        {
            CharFunc.shadow = content.Load<Texture2D>("graphics/textures/characters/shadow");
            StreamReader sr;
            sr = new StreamReader(content.RootDirectory + "/world/characters/charList.txt");
            int i_numChar = Convert.ToInt32(sr.ReadLine());

            character[] char_out = new character[i_numChar];
            string name;

            for (int j = 0; j != i_numChar; j++)
            {
                name = sr.ReadLine();

                //for (int i = 0; i != i_numChar; i++)
                //{
                    char_out[j] = LoadCharacter(name,content);
                //}
            }

            return char_out;
        }
        private static character LoadCharacter(string name,ContentManager content)
        {
            character c_temp = new character(name,Vector2.Zero,ControlType.player_current,CharType.survivor);
            c_temp.B_baseProps.v_position = new Vector2(0, 0);
            StreamReader sr = new StreamReader(content.RootDirectory + "/world/characters/" + name + ".guy");
            //sr.ReadLine(); //attributes start;
            #region LoadAttributes
            c_temp.accuracy = Convert.ToSingle(sr.ReadLine());
            c_temp.angle = Convert.ToSingle(sr.ReadLine());
            c_temp.armor = Convert.ToInt32(sr.ReadLine());
            c_temp.attrib.hp_max = Convert.ToInt32(sr.ReadLine());
            c_temp.attrib.mech_max = Convert.ToInt32(sr.ReadLine());
            c_temp.attrib.med_max = Convert.ToInt32(sr.ReadLine());
            int fake = Convert.ToInt32(sr.ReadLine());
            c_temp.attrib.sta_max = Convert.ToInt32(sr.ReadLine());
            c_temp.attrib.stl_max = Convert.ToInt32(sr.ReadLine());
            c_temp.attrib.str_max = Convert.ToInt32(sr.ReadLine());
            #endregion
            //sr.ReadLine(); //blocks start
            c_temp.body = new block[Convert.ToInt32(sr.ReadLine())];
            for (int i = 0; i != c_temp.body.Length; i++)
            {
                //sr.ReadLine();//block starts
                #region getChildren
                c_temp.body[i].children = new int[Convert.ToInt32(sr.ReadLine())];
                for (int j = 0; j != c_temp.body[i].children.Length; j++)
                {
                    c_temp.body[i].children[j] = Convert.ToInt32(sr.ReadLine());
                }
                #endregion
                #region getParents
                string temp = sr.ReadLine();

                c_temp.body[i].parents = new int[Convert.ToInt32(temp)];
                for (int j = 0; j != c_temp.body[i].parents.Length; j++)
                {
                    c_temp.body[i].parents[j] = Convert.ToInt32(sr.ReadLine());
                }

                #endregion
                int numTex = Convert.ToInt32(sr.ReadLine());
                c_temp.body[i].texture = new Texture2D[6];
                c_temp.body[i].TexturePath = new string[6];
                c_temp.body[i].TexturePath[0] = sr.ReadLine();
                for (int j = 0; j != 6; j++)
                {
                    c_temp.body[i].texture[j] = content.Load<Texture2D>(("graphics/textures/characters/" + c_temp.body[i].TexturePath[0] + j.ToString()));
                }
                c_temp.body[i].v_collison_map = gfunc.collision.MakeCollisionMap(Vector2.Zero, c_temp.body[i].texture[0], gfunc.i_collision_accuracy);

                c_temp.body[i].angle = Convert.ToSingle(sr.ReadLine());
                c_temp.body[i].height = Convert.ToInt32(sr.ReadLine());
                c_temp.body[i].width = Convert.ToInt32(sr.ReadLine());
                c_temp.body[i].name = sr.ReadLine();
                c_temp.body[i].zorder = Convert.ToInt32(sr.ReadLine());
                Vector2 pivot = Vector2.Zero, pos = Vector2.Zero;
                pivot.X = Convert.ToSingle(sr.ReadLine());
                pivot.Y = Convert.ToSingle(sr.ReadLine());
                c_temp.body[i].pivot = pivot;
                pos.X = Convert.ToSingle(sr.ReadLine());
                pos.Y = Convert.ToSingle(sr.ReadLine());
                c_temp.body[i].position = (pos + c_temp.B_baseProps.v_position);

                if (c_temp.body[i].v_collison_mapOR == null)
                {
                    c_temp.body[i].v_collison_mapOR = gfunc.collision.MakeCollisionMap(Vector2.Zero, c_temp.body[i].texture[0], gfunc.i_collision_accuracy);
                }
                else
                {
                    c_temp.body[i].v_collison_map = new Vector2[c_temp.body[i].v_collison_mapOR.Length];
                    for (int j = 0; j != c_temp.body[i].v_collison_mapOR.Length; j++)
                    {
                        c_temp.body[i].v_collison_map[j] = c_temp.body[i].v_collison_mapOR[j];
                    }
                }

            }
            return c_temp;
        }
        public static weapon[] LoadWeapons(ContentManager content)
        {
            StreamReader sr;
            sr = new StreamReader(content.RootDirectory + "/world/weapons/weaponlist.txt");
            int i_numWeap = Convert.ToInt32(sr.ReadLine());

            weapon[] output = new weapon[i_numWeap];

            for (int i = 0; i != i_numWeap; i++)
            {
                string name  = sr.ReadLine();

                StreamReader sr1 = new StreamReader(content.RootDirectory + "/world/weapons/" + name + ".gun");
                {
                                output[i].name = sr1.ReadLine(); //.ReadString();
                                output[i].inaccuracy = Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].mass = Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                string GunTexPath = sr1.ReadLine(); //.ReadString();
                                    output[i].bolt_back = content.Load<Texture2D>("graphics/textures/weapons" + GunTexPath + "_boltback");
                                    output[i].bolt_forward = content.Load<Texture2D>("graphics/textures/weapons" + GunTexPath + "_boltforeward");
                                    output[i].muzzleFlash = new Texture2D[3];
                                    for (int j = 0; j != 3; j++)
                                    {
                                        output[i].muzzleFlash[j] = content.Load<Texture2D>("graphics/textures/weapons/" + name + "/muzzleFlash/" + j.ToString()); ;
                                    }
                                output[i].gripPos.X = Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].gripPos.Y = Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                string ClipTexPath = sr1.ReadLine(); //.ReadString();
                                    output[i].clip = content.Load<Texture2D>("graphics/textures/weapons/" + ClipTexPath);
                                output[i].clipPos.X = output[i].gripPos.X+Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].clipPos.Y = output[i].gripPos.Y+Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].muzzPos.X = output[i].gripPos.X + Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].muzzPos.Y = output[i].gripPos.Y + Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].actionPos.X = output[i].gripPos.X + Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].actionPos.Y = output[i].gripPos.Y + Convert.ToSingle(sr1.ReadLine()); //.ReadSingle();
                                output[i].clipSize = Convert.ToInt32(sr1.ReadLine()); //.ReadInt32();
                                output[i].ROFdelay = Convert.ToInt32(sr1.ReadLine()); //.ReadInt32();
                                //ClipSize.Text = Convert.ToString(clipSize);
                                output[i].Pt = (ProjectileType)Enum.ToObject(typeof(ProjectileType), Convert.ToInt32(sr1.ReadLine()));
                                output[i].Stance = (StanceType)Enum.ToObject(typeof(StanceType), Convert.ToInt32(sr1.ReadLine()));
                                string auto = sr1.ReadLine();
                                if (auto == "True")
                                {
                                    output[i].auto = true;
                                }
                                else
                                {
                                    output[i].auto = false;
                                }
                                output[i].clipCurrent = output[i].clipSize;
                }
                sr1.Close();
            }
            return output;

        }
        public static Texture2D[] LoadParticles(ContentManager content,string name)
        {
            //string name = "blood";
            Texture2D sr;                              //sr is the obj created to help us read file contents.
            #region FindNumberOfTextures
            int i_partcount = 0; bool b_invalid = false; //these vars are created to help find the number of levels in the dir.

            while (!b_invalid)
            {
                #region CheckValidity
                try
                {
                    sr = content.Load<Texture2D>("graphics/textures/particles/"+name+"/" + i_partcount.ToString());
                }
                catch
                {
                    b_invalid = true;
                }
                #endregion
                #region KeepCount
                if (!b_invalid)
                {
                    i_partcount++;
                }
                #endregion
            }
            #endregion

            Texture2D[] textures = new Texture2D[i_partcount];

            for (int i = 0; i != i_partcount; i++)
            {
                textures[i] = content.Load<Texture2D>("graphics/textures/particles/" + name + "/" + i.ToString());
            }

            return textures;
        }
        public static sky LoadSky(ContentManager content)
        {
            sky sky_out = new sky(null, null, null, 2);
            sky_out.day = content.Load<Texture2D>("graphics/textures/sky/sky_day");
            sky_out.night = content.Load<Texture2D>("graphics/textures/sky/sky_night");
            sky_out.front = content.Load<Texture2D>("graphics/textures/sky/city_f");
            sky_out.mid = content.Load<Texture2D>("graphics/textures/sky/city_m");
            sky_out.back = content.Load<Texture2D>("graphics/textures/sky/city_b");
            sky_out.grad = content.Load<Texture2D>("graphics/textures/sky/gradient");

            return sky_out;
        }
        public static pda loadPda(ContentManager content)
        {
            pda output;
            output.tex_pda = content.Load<Texture2D>("graphics/textures/ui/pda/pda");
            output.tex_pda_bckgrnd = content.Load<Texture2D>("graphics/textures/ui/pda/backgrounds/1");
            //output.tex_pda_glint = content.Load<Texture2D>("graphics/textures/ui/pda/glint");
            output.tex_icon_inventory = content.Load<Texture2D>("graphics/textures/ui/pda/icons/icon_inventory");
            output.tex_icon_status = content.Load<Texture2D>("graphics/textures/ui/pda/icons/icon_status");
            output.tex_icon_map = content.Load<Texture2D>("graphics/textures/ui/pda/icons/icon_map");
            output.tex_icon_drop = content.Load<Texture2D>("graphics/textures/ui/pda/icons/icon_drop");
            output.tex_icon_equip = content.Load<Texture2D>("graphics/textures/ui/pda/icons/icon_equip");
            output.tex_icon_examine = content.Load<Texture2D>("graphics/textures/ui/pda/icons/icon_examine"); 
            output.font = content.Load<SpriteFont>("pda");
            output.color = Color.LightSkyBlue;

            return output;
        }
        public static item[] loadItems(ContentManager content)
        {
            StreamReader sr;
            BasePath = content.RootDirectory;
            sr = new StreamReader(content.RootDirectory + "/world/items/itemlist.txt");
            int i_numitems = Convert.ToInt32(sr.ReadLine());

            item[] output = new item[i_numitems];

            for (int i = 0; i != i_numitems; i++)
            {
                string name = sr.ReadLine();

                StreamReader sr1 = new StreamReader(content.RootDirectory + "/world/items/" + name + ".ANITEM");
                {
                    output[i].name = sr1.ReadLine();
                    output[i].gameTag = sr1.ReadLine();
                    output[i].special = sr1.ReadLine();
                    output[i].quality = Convert.ToSingle(sr1.ReadLine());
                    output[i].texture = content.Load<Texture2D>("graphics/textures/items/" + sr1.ReadLine());
                    output[i].icon = content.Load<Texture2D>("graphics/textures/items/" + sr1.ReadLine());
                    @output[i].description = sr1.ReadLine().Replace("\\n", "\n");
                    output[i].type = (itemType)Enum.ToObject(typeof(itemType), Convert.ToInt32(sr1.ReadLine()));
                    output[i].usable = Convert.ToBoolean(sr1.ReadLine());
                    output[i].weight = Convert.ToSingle(sr1.ReadLine());
                }
                sr1.Close();
            }
            sr.Close();

            return output;
        }
        public static void loadMainMenu(ContentManager content)
        {
            mainMenuFunc.items = new menuItem[3];
            mainMenuFunc.title = content.Load<Texture2D>("graphics/textures/UI/mainmenu/mm_title");
            mainMenuFunc.items[0].texture = content.Load<Texture2D>("graphics/textures/UI/mainmenu/mm_start");
            mainMenuFunc.items[0].destination = new Vector2(Game1.v_resolution.X * .6f, Game1.v_resolution.Y /2);
            mainMenuFunc.items[0].type = buttontype.start;
            mainMenuFunc.items[1].texture = content.Load<Texture2D>("graphics/textures/UI/how2 inv");
            mainMenuFunc.items[1].destination = new Vector2(Game1.v_resolution.X * .6f, Game1.v_resolution.Y * .7f);
            mainMenuFunc.items[1].type = buttontype.resume;
            mainMenuFunc.items[2].texture = content.Load<Texture2D>("graphics/textures/UI/mainmenu/mm_credits");
            mainMenuFunc.items[2].destination = new Vector2(Game1.v_resolution.X * .6f, Game1.v_resolution.Y * .9f);
            mainMenuFunc.items[2].type = buttontype.credits;
        }
    }
}
