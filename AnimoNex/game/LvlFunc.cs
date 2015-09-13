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
using AnimoNex.game.pda;
using AnimoNex.types;
using AnimoNex.game.effects;
using AnimoNex.game;

namespace AnimoNex.game
{
    class LvlFunc
    {

        public static Vector2 randomPositon()
        {
            Vector2 output = Vector2.Zero;
            int floor = findFloor(world.levels[world.i_currentLvl]);
            output = gfunc.randomPos(world.levels[world.i_currentLvl].regions[floor].corners[0], world.levels[world.i_currentLvl].regions[floor].corners[1]);
            return output;
        }
        public static Vector2 randomPositon(int level)
        {
            Vector2 output = Vector2.Zero;
            int floor = findFloor(world.levels[level]);
            output = gfunc.randomPos(world.levels[level].regions[floor].corners[0], world.levels[level].regions[floor].corners[1]);
            return output;
        }

        public static void populateChar(int population)
        {

            for (int i = 0; i != world.levels.Length; i++)
            {
                int people = gfunc.roundUp(gfunc.RandomNumber(0, population/world.levels.Length));
                population -= people;
                if (world.levels[i].name != "mainmenu" && !world.levels[i].indoor)
                {
                    if (population > 0)
                    {
                        for (int j = 0; j != population; j++)
                        {
                            CharFunc.addActor(LvlFunc.randomPositon(i), 0, CharType.survivor, ControlType.NPC, "", false, i);
                        }
                        CharFunc.addActor(LvlFunc.randomPositon(i), 0, CharType.zombie, ControlType.NPC, "", false, i);
                    }
                }

            }
        }
        public static void populateAMMO()
        {
            for (int i = 0; i != world.levels.Length; i++)
            {
                if (world.levels[i].name != "mainmenu")
                {
                    for (int j = 0; j != world.items.Length; j++)
                    {
                        if (world.items[j].type == itemType.ammo)
                        {
                            if (gfunc.roundUp(gfunc.RandomNumber(0, 4)) == 1)
                            {
                                itemFunc.createItem(world.items[j],
                                LvlFunc.randomPositon(i),
                                gfunc.RandomNumber(0, 0),
                                i,
                                gfunc.RandomNumber(world.items[j].quality, world.items[j].quality));
                            }
                        }
                    }
                }
            }
        }
        public static void populateItems(int count)
        {
            int items = gfunc.roundUp(gfunc.RandomNumber(0, count / world.levels.Length));
            for (int i = 0; i != world.levels.Length; i++)
            {
                //count += items;
                if (world.levels[i].name != "mainmenu")
                if (count > 0)
                {
                    for (int j = 0; j != count; j++)
                    {
                        int item = gfunc.roundUp(gfunc.RandomNumber(0, world.items.Length - 1));
                        if (world.items[item].type == itemType.weapon)
                        {
                            if (world.levels[i].indoor)// && gfunc.roundUp(gfunc.RandomNumber(0, 3)) == 1)
                            {
                                itemFunc.createItem(world.items[item],
                                                    LvlFunc.randomPositon(i),
                                                    gfunc.RandomNumber(0, 0),
                                                    i,
                                                    gfunc.RandomNumber(world.items[item].quality, world.items[item].quality));
                                populateAMMO();
                            }
                        }
                        else
                        {
                            itemFunc.createItem(world.items[item],
                                                LvlFunc.randomPositon(i),
                                                gfunc.RandomNumber(0, 0),
                                                i,
                                                gfunc.RandomNumber(world.items[item].quality, world.items[item].quality));
                        }
                    }
                }

            }
        }
        public static bool nameExists(string name)
        {
            bool output = false;
            for (int i = 0; i != world.levels.Length; i++)
            {
                for (int j = 0; j != world.levels[i].char_living.Length; j++)
                {
                    if (world.levels[i].char_living[j].name == name)
                    {
                        output = true;
                    }
                }
            }
            return output;
        }
        public static bool nameExists(string name,int level)
        {
            bool output = false;
            if(world.levels[level].char_living != null)
                for (int j = 0; j != world.levels[level].char_living.Length; j++)
                {
                    if (world.levels[level].char_living[j].name == name)
                    {
                        output = true;
                    }
                }
            return output;
        }
        public static void setActiveLevelTo(string name)
        {

            //level lvl_out = new level();
            weapFunc.projectiles = null;
            PEngine.particles = null;
            world.i_currentLvl = findLevel(name);
            if (name != "mainmenu")
            {
                UI.addMessage(name);
                camera.v_pos = findRegionCenter(findFloor(world.levels[findLevel("mainmenu")]))+new Vector2(0,-400);
            }

        }

        private static int findLevel(string name)
        {
            int i_level = -1;
            for (int i = 0; i != world.levels.Length && i_level ==-1; i++)
            {
                if (world.levels[i].name == name)
                {
                    i_level = i;
                }
            }
            return i_level;
        }
        public static character CharacterUseDoor(character player,int player_index)
        {
            int i_portalInUse=-1;
            character output = player;

            for (int i = 0; i != world.levels[world.i_currentLvl].regions.Length; i++) //cycle thru regions in current level
            {
                if (world.levels[world.i_currentLvl].regions[i].type == regionType.portal) //if its a portal...
                {
                    if (isInRegion(player.B_baseProps.v_position-new Vector2(0,20),
                                  world.levels[world.i_currentLvl].regions[i])) // if were in the portal..
                    {
                        i_portalInUse = i; //show this a portal in use
                     }
                 }
            }

            if (i_portalInUse != -1) //if portal in use is valid
            {
                string s_doorName = world.levels[world.i_currentLvl].regions[i_portalInUse].special; //find the door name that we are jumping to.
                int lvlTag=0;

                world.levels[world.i_currentLvl] = removeCharacter(world.levels[world.i_currentLvl], player_index); //remove the player were teleporting from the current level
                if (player.controlType == ControlType.player_current)
                {
                    //world.levels[world.i_currentLvl] = temp;                       //run this if char is the current player                                               
                    LvlFunc.setActiveLevelTo(world.levels[world.i_currentLvl].regions[i_portalInUse].toLevel);
                    CharFunc.addActor(player);
                    #region findPlayer
                    if (world.levels[world.i_currentLvl].char_living != null &&
                       world.levels[world.i_currentLvl].char_living.Length > 0)
                        for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.player_current)
                            {
                                world.levels[world.i_currentLvl].char_living[i].brain.tick_last = Game1.time_ticks;
                            }
                        }
                    #endregion
                }
                else
                {
                    lvlTag = LvlFunc.findLevel(world.levels[world.i_currentLvl].regions[i_portalInUse].toLevel);
                      if(lvlTag!=-1)
                    {
                        CharFunc.addActor(player,lvlTag);
                    }
                }
                #region FindPositionInRegion

                int doorID = -1;
                int floorID=-1;
                if(player.controlType == ControlType.player_current)
                {
                    doorID = findDoor(s_doorName);
                    floorID = findFloor(world.levels[world.i_currentLvl]);
                }
                else
                {
                    if (lvlTag != -1)
                    {
                        doorID = findDoor(s_doorName,lvlTag);
                        floorID = findFloor(world.levels[lvlTag]);
                    }
                    else
                    {
                        floorID = -1;
                    }
                }
                if (floorID != -1 && doorID != -1)
                {

                    output = player;

                    if (player.controlType == ControlType.player_current)
                    {
                        Vector2 doorCenter = findRegionCenter(doorID);
                        Vector2 floorCenter = findRegionCenter(floorID);

                        float radius = findRegionRadius(doorCenter, doorID);
                        float angle = gfunc.PointAt(doorCenter, floorCenter);

                        output.B_baseProps.v_position = doorCenter;
                        output.B_baseProps.v_position += gfunc.TranslateOnAng(radius, angle);
                        output.B_baseProps.v_velocity = Vector2.Zero;
                        camera.v_pos = floorCenter-(Game1.v_resolution/2)-new Vector2(0,200);
                        camera.levelChangedTick = Game1.time_ticks;
                    }
                    else
                    {
                        Vector2 doorCenter = findRegionCenter(doorID, lvlTag);
                        Vector2 floorCenter = findRegionCenter(floorID, lvlTag);

                        float radius = findRegionRadius(doorCenter, doorID,lvlTag);
                        float angle = gfunc.PointAt(doorCenter, floorCenter);

                        int charTag = findNPC(player.name,lvlTag);
                        world.levels[lvlTag].char_living[charTag].B_baseProps.v_position = doorCenter;
                        world.levels[lvlTag].char_living[charTag].B_baseProps.v_position += gfunc.TranslateOnAng(radius, angle);
                        world.levels[lvlTag].char_living[charTag].B_baseProps.v_velocity = Vector2.Zero;
                    }
                }
                //world.levels[world.i_currentLvl].char_living[findPlayer()].B_baseProps.v_position = gfunc.TranslateOnAng(radius, angle); 
                #endregion


                //player.B_baseProps.v_position = world.levels[world.i_currentLvl].regions[].corners[0];
            }

            return output;
        }
        public static void CharacterUseDoor_LowLvl(int level, string name,int portal)
        {
            int i_portalInUse = portal; //portal that the npc is using
            int player_index = LvlFunc.findNPC(name, level); //find the npc's id from his name
            character output;
            character player = output = world.levels[level].char_living[player_index]; //set player and output to the active player


            if (i_portalInUse != -1)
            {
                string s_doorName = world.levels[level].regions[i_portalInUse].special;
                int lvlTag = 0;

                world.levels[level] = removeCharacter(world.levels[level], player_index); // remove char from current level


                lvlTag = LvlFunc.findLevel(world.levels[level].regions[i_portalInUse].toLevel);
                    if (lvlTag != -1)
                    {
                        if (world.levels[world.i_currentLvl].name == world.levels[lvlTag].name)
                        {
                            if(!nameExists(player.name,lvlTag))
                            CharFunc.addActor(player); //add to current level
                        }
                        else
                        {
                            if (!nameExists(player.name, lvlTag))
                            CharFunc.addActor(player, lvlTag); //add to other level
                        }
                    }

                #region FindPositionInRegion

                int doorID = -1;
                int floorID = -1;

                    if (lvlTag != -1)
                    {
                        doorID = findDoor(s_doorName, lvlTag);
                        floorID = findFloor(world.levels[lvlTag]);
                    }
                    else
                    {
                        floorID = -1;
                    }

                if (floorID != -1 && doorID != -1)
                {

                    output = player;

                    {
                        Vector2 doorCenter = findRegionCenter(doorID, lvlTag);
                        Vector2 floorCenter = findRegionCenter(floorID, lvlTag);

                        float radius = findRegionRadius(doorCenter, doorID, lvlTag);
                        float angle = gfunc.PointAt(doorCenter, floorCenter);



                            if (world.levels[world.i_currentLvl].name == world.levels[lvlTag].name)
                            {
                                int charTag = findNPC(output.name);
                                if (charTag < world.levels[world.i_currentLvl].char_living.Length && charTag != -1)
                                {
                                    world.levels[world.i_currentLvl].char_living[charTag].B_baseProps.v_position = doorCenter;
                                    world.levels[world.i_currentLvl].char_living[charTag].B_baseProps.v_position += gfunc.TranslateOnAng(radius, angle);
                                    world.levels[world.i_currentLvl].char_living[charTag].B_baseProps.v_velocity = Vector2.Zero;
                                }
                            }
                            else
                            {

                                    int charTag = findNPC(output.name, lvlTag);
                                    if (charTag < world.levels[lvlTag].char_living.Length && charTag != -1)
                                    {
                                    world.levels[lvlTag].char_living[charTag].B_baseProps.v_position = randomPositon(lvlTag);
                                    //world.levels[lvlTag].char_living[charTag].B_baseProps.v_position += gfunc.TranslateOnAng(radius, angle);
                                    world.levels[lvlTag].char_living[charTag].B_baseProps.v_velocity = Vector2.Zero;
                                }
                            }


                    }
                }
                //world.levels[world.i_currentLvl].char_living[findPlayer()].B_baseProps.v_position = gfunc.TranslateOnAng(radius, angle); 
                #endregion


                //player.B_baseProps.v_position = world.levels[world.i_currentLvl].regions[].corners[0];
            }

        }
        public static int findDoor(string name)
        {
            int door = -1;
            //try
            {
                for (int i = 0; i != world.levels[world.i_currentLvl].regions.Length; i++)
                {
                    if (world.levels[world.i_currentLvl].regions[i].type == regionType.portal)
                    {
                        if (world.levels[world.i_currentLvl].regions[i].name == name)
                        {
                            door = i;
                        }
                    }
                }
            }
            //catch { }
            return door;
        }
        public static int findDoor(string name,int level)
        {
            int door = -1;
            //try
            {
                for (int i = 0; i != world.levels[level].regions.Length; i++)
                {
                    if (world.levels[level].regions[i].type == regionType.portal)
                    {
                        if (world.levels[level].regions[i].name == name)
                        {
                            door = i;
                        }
                    }
                }
            }
            //catch { }
            return door;
        }
        public static int findPlayer()
        {
            int output=-1;

            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.player_current)
                {
                    output = i;
                }
            }
            return output;
        }
        public static int findNPC(string name, int level)
        {
            int output = -1;
            if (world.levels[level].char_living != null)
            {
                for (int i = 0; i != world.levels[level].char_living.Length; i++)
                {
                    if (world.levels[level].char_living[i].controlType == ControlType.NPC)
                    {
                        if (world.levels[level].char_living[i].name == name)
                        {
                            output = i;
                        }
                    }
                }
            }
            return output;
        }
        public static int findNPC(string name)
        {
            int output = -1;

            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.NPC)
                {
                    if (world.levels[world.i_currentLvl].char_living[i].name == name)
                    {
                        output = i;
                    }
                }
            }
            return output;
        }
        public static int getZombieCount(int level)
        {
            int zombies = 0;

            if(world.levels[level].char_living!=null)
            for (int i = 0; i != world.levels[level].char_living.Length; i++)
            {
                if (world.levels[level].char_living[i].Type == CharType.zombie && world.levels[level].char_living[i].action != "dead")
                {
                    zombies++;
                }
            }
            return zombies;
        }
        public static int findNPC(int level,bool human)
        {
            int output = -1;
            if (world.levels[level].char_living != null)
            {
                #region FindLivingCharacters
                int[] living = new int[0];
                for (int i = 0; i != world.levels[level].char_living.Length; i++)
                {
                    if (world.levels[level].char_living[i].brain.active && world.levels[level].char_living[i].attrib.hp_current > 0)
                    {
                        if (world.levels[level].char_living[i].Type != CharType.zombie || !human)
                        {
                            int[] old = living;
                            living = new int[old.Length + 1];
                            for (int j = 0; j != old.Length; j++)
                            {
                                living[j] = old[j];
                            }
                            living[old.Length] = i;
                        }

                    }
                }

                #endregion
                if (living.Length > 0)
                    output = living[gfunc.roundUp(gfunc.RandomNumber(0, living.Length - 1))];
            }
            return output;
        }
        public static level removeCharacter(level l, int Char)
        {
            character[] lower = new character[Char];
            character[] upper = new character[l.char_living.Length - (Char + 1)];

            for (int i = 0; i != l.char_living.Length; i++)
            {
                if (i < Char)
                {
                    lower[i] = l.char_living[i];
                }
                if (i > Char)
                {
                    upper[i - (Char + 1)] = l.char_living[i];
                }
            }
            l.char_living = new character[l.char_living.Length - 1];

            for (int i = 0; i != l.char_living.Length; i++)
            {
                if (i < lower.Length)
                {
                    l.char_living[i] = lower[i];
                }
                else
                {
                    l.char_living[i] = upper[i - lower.Length];
                }
            }

            return l;

        }
        public static level removeCharacter(level l, string name)
        {

            int Char = findNPC(name) ;
            character[] lower = new character[Char];
            character[] upper = new character[l.char_living.Length - (Char + 1)];

            for (int i = 0; i != l.char_living.Length; i++)
            {
                if (i < Char)
                {
                    lower[i] = l.char_living[i];
                }
                if (i > Char)
                {
                    upper[i - (Char + 1)] = l.char_living[i];
                }
            }
            l.char_living = new character[l.char_living.Length - 1];

            for (int i = 0; i != l.char_living.Length; i++)
            {
                if (i < lower.Length)
                {
                    l.char_living[i] = lower[i];
                }
                else
                {
                    l.char_living[i] = upper[i - lower.Length];
                }
            }

            return l;

        }
        public static int findItem(int level)
        {
            return gfunc.roundUp(gfunc.RandomNumber(0, world.levels[level].items.Length-1));
        }
        public static bool isInRegion(Vector2 position, region Region)
        {
            bool InRegion = false;
            //position -= camera.v_pos;
            Vector2[] corners = new Vector2[2];
            for (int i = 0; i != 2; i++)
            {
                corners[i] = Region.corners[i];
            }
            //position -= camera.offset;

            if (position.X > corners[0].X && position.X < corners[1].X)
            {
                if (position.Y < corners[1].Y && position.Y > corners[0].Y)
                {
                    InRegion = true;
                }
            }

            return InRegion;
        }
        public static int findFloor(level lvl)
        {
            int tag = -1;
            if (lvl.regions[0].type == regionType.floor)
            {
                tag = 0;
            }
            else
            {
                for (int i = 0; i != lvl.regions.Length; i++)
                {
                    if (lvl.regions[i].type == regionType.floor)
                    {
                        tag = i;
                    }
                }
            }
            return tag;
        }
        public static bool isInRegion(Vector2 position, regionType Region)
        {
            bool InRegion = false;
            //position -= camera.v_pos;
            Vector2[] corners;
            for (int j = 0; j != world.levels[world.i_currentLvl].regions.Length; j++)
            {
                if (world.levels[world.i_currentLvl].regions[j].type == Region)
                {
                    corners = new Vector2[2];
                    for (int i = 0; i != 2; i++)
                    {
                        corners[i] = world.levels[world.i_currentLvl].regions[j].corners[i];
                    }
                    //position -= camera.offset;
                    if (position.X > corners[0].X && position.X < corners[1].X)
                    {
                        if (position.Y < corners[1].Y && position.Y > corners[0].Y)
                        {
                            InRegion = true;
                        }
                    }
                }
            }

            return InRegion;
        }
        public static int findRandomPortal(int level)
        {
            int output = -1;

            if(world.levels[level].regions.Length >1)
            while (output == -1)
            {
                int rand = gfunc.roundUp(gfunc.RandomNumber(1, world.levels[level].regions.Length - 1));
                if (world.levels[level].regions[rand].type == regionType.portal)
                {
                    output = rand;
                }
            }

            return output;
        }
        public static Vector2 findRegionCenter(int region)
        {
            Vector2 output = Vector2.Zero;
            for (int i = 0; i != world.levels[world.i_currentLvl].regions[region].corners.Length; i++)
            {
                output += world.levels[world.i_currentLvl].regions[region].corners[i];
            }
            output /= world.levels[world.i_currentLvl].regions[region].corners.Length;
            return output;
        }
        public static Vector2 findRegionCenter(int region,int level)
        {
            Vector2 output = Vector2.Zero;
            for (int i = 0; i != world.levels[level].regions[region].corners.Length; i++)
            {
                output += world.levels[level].regions[region].corners[i];
            }
            output /= world.levels[level].regions[region].corners.Length;
            return output;
        }
        public static float findRegionRadius(Vector2 center,int region)
        {
            float output = Vector2.Distance(center, world.levels[world.i_currentLvl].regions[region].corners[0])+5;
            return output;
        }
        public static float findRegionRadius(Vector2 center, int region,int level)
        {
            float output = Vector2.Distance(center, world.levels[level].regions[region].corners[0])+5;
            if (output < 100)
            {
                output += 50;
            }
            return output;
        }
        public static void draw(SpriteBatch sb, Vector2 camPos, level current)
        {
            sb.Begin();
            //shaders.DesaturateBegin();
                for (int i = 0; i != current.props.Length; i++)
                {
                    sb.Draw(current.props[i].texture,
                            (new Vector2(current.props[i].X, current.props[i].Y) - camPos)*camera.scale+(Game1.v_resolution/2),
                            null,
                            Color.White,
                            0,
                            new Vector2(current.props[i].texture.Width / 2, current.props[i].texture.Height / 2),
                            camera.scale,
                            SpriteEffects.None,
                            0);
                }
                //shaders.DesaturateEnd();
            sb.End();
        }
        public static int FindBackGround()
        {
            int output = -1;
                for (int i = 0; i != world.levels.Length; i++)
                {
                    if (world.levels[i].column == world.levels[world.i_currentLvl].column &&
                        world.levels[i].row == world.levels[world.i_currentLvl].row - 1)
                    {
                        output = i;
                    }
                }
                return output;
        }
        public static void draw(SpriteBatch sb, Vector2 camPos)
        {
            sb.Begin();//SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            //shaders.DesaturateBegin();

            //if (world.levels[world.i_currentLvl].row > 0)
            //{
            //    int backgroundLvl = FindBackGround();
            //    if (backgroundLvl != -1)
            ///    {
            //        for (int i = 0; i != world.levels[backgroundLvl].props.Length; i++)
            //        {
            //            sb.Draw(world.levels[backgroundLvl].props[i].texture,
            //                    (new Vector2(world.levels[backgroundLvl].props[i].X, world.levels[backgroundLvl].props[i].Y) - (camera.v_pos + new Vector2(200, -200))*new Vector2(.65f,.65f)),
            //                    null,
            //                    Color.White,
            //                    0,
            //                    new Vector2(world.levels[backgroundLvl].props[i].texture.Width / 2, world.levels[backgroundLvl].props[i].texture.Height / 2),
            //                    camera.scale,
            //                    SpriteEffects.None,
            //                    0);
            //        }
            //    }
            //}
            for (int i = 0; i != world.levels[world.i_currentLvl].props.Length; i++)
            {
                sb.Draw(world.levels[world.i_currentLvl].props[i].texture,
                        (new Vector2(world.levels[world.i_currentLvl].props[i].X, world.levels[world.i_currentLvl].props[i].Y) - camera.v_pos),
                        null,
                        Color.White,
                        0,
                        new Vector2(world.levels[world.i_currentLvl].props[i].texture.Width / 2, world.levels[world.i_currentLvl].props[i].texture.Height / 2),
                        camera.scale,
                        SpriteEffects.None,
                        0);
            }

            //shaders.DesaturateEnd();
            sb.End();
        }
    }
}
