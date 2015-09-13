using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using AnimoNex.input;
using AnimoNex.types;
using AnimoNex.game.effects;
using AnimoNex.game.pda;

namespace AnimoNex.game
{
    class CharFunc
    {
        public static Texture2D Tex_null;
        public static Texture2D shadow;
        public static int ThisPlayerKills;
        public static Cue bloodSplat, bodyFalling, fleshImpact, gib;
        public static Cue zombHit, zombDie, zombStrike, zombMoan;
        public static Cue charDie,charEat,charFlee;
        public static Cue headshot;
        public static bool thisPlayerLiving = true;

        #region sounds
        public static void PlayCharFlee()
        {
            charFlee = sounds.soundBank.GetCue("char_zombSeen");
            charFlee.Play();
        }
        public static void PlayCharEat()
        {
            charDie = sounds.soundBank.GetCue("char_eat");
            charDie.Play();
        }
        public static void PlayCharDie()
        {
            charDie = sounds.soundBank.GetCue("char_mauled");
            charDie.Play();
        }
        public static void PlayZombHit()
        {
            zombHit= sounds.soundBank.GetCue("zomb_hit");
            zombHit.Play();
        }
        public static void PlayZombStrike()
        {
            zombStrike = sounds.soundBank.GetCue("zomb_strike");
            zombStrike.Play();
        }
        public static void PlayZombMoan()
        {
            zombMoan = sounds.soundBank.GetCue("zomb_moan");
            zombMoan.Play();
        }
        public static void PlayZombDie()
        {
            zombDie = sounds.soundBank.GetCue("zomb_die");
            zombDie.Play();
        }
        public static void PlayBloodSplat()
        {
            bloodSplat = sounds.soundBank.GetCue("char_bloodSplat");
            bloodSplat.Play();
        }
        public static void PlayBodyFalling()
        {
            bodyFalling = sounds.soundBank.GetCue("char_bodyFalling");
            bodyFalling.Play();
        }
        public static void PlayFleshImpact()
        {
            fleshImpact = sounds.soundBank.GetCue("char_fleshImpact");
            fleshImpact.Play();
        }
        public static void PlayGib()
        {
            gib = sounds.soundBank.GetCue("char_gib");
            gib.Play();
        }
        public static void PlayHeadshot()
        {
            headshot = sounds.soundBank.GetCue("headshot");
            headshot.Play();
        }
        #endregion 

        #region Spawning
        public static void addActor(Vector2 pos, float Z, CharType Char_Type, ControlType Cont_Type, string name, bool randParts)
        {
            try
            {
                character[] old = world.levels[world.i_currentLvl].char_living;
                world.levels[world.i_currentLvl].char_living = new character[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[world.i_currentLvl].char_living[i] = old[i];
                }
                if (randParts)
                {
                    world.levels[world.i_currentLvl].char_living[old.Length] = CreateActor(pos, Z, Char_Type, Cont_Type, true);
                    world.levels[world.i_currentLvl].char_living[old.Length].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_current = world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_max = 100;
                }
                else
                {
                    world.levels[world.i_currentLvl].char_living[old.Length] = CreateActor(pos, Z, Char_Type, Cont_Type, name);
                    world.levels[world.i_currentLvl].char_living[old.Length].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_current = world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_max = 100;
                }
                thisPlayerLiving = true;
            }
            catch
            {
                world.levels[world.i_currentLvl].char_living = new character[1];
                if (randParts)
                {
                    world.levels[world.i_currentLvl].char_living[0] = CreateActor(pos, Z, Char_Type, Cont_Type, true);
                    world.levels[world.i_currentLvl].char_living[0].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[0].attrib.hp_current = world.levels[world.i_currentLvl].char_living[0].attrib.hp_max = 100;
                }
                else
                {
                    world.levels[world.i_currentLvl].char_living[0] = CreateActor(pos, Z, Char_Type, Cont_Type, name);
                    world.levels[world.i_currentLvl].char_living[0].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[0].attrib.hp_current = world.levels[world.i_currentLvl].char_living[0].attrib.hp_max = 100;
                }
                thisPlayerLiving = true;
            }
        }
        public static void addActor(Vector2 pos, float Z, CharType Char_Type, ControlType Cont_Type, string name, bool randParts, int level)
        {
            try
            {
                character[] old = world.levels[level].char_living;
                world.levels[level].char_living = new character[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[level].char_living[i] = old[i];
                }
                if (randParts)
                {
                    world.levels[level].char_living[old.Length] = CreateActor(pos, Z, Char_Type, Cont_Type, true);
                    world.levels[level].char_living[old.Length].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_current = world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_max = 100;
                }
                else
                {
                    world.levels[level].char_living[old.Length] = CreateActor(pos, Z, Char_Type, Cont_Type, name);
                    world.levels[level].char_living[old.Length].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_current = world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_max = 100;
                }
            }
            catch
            {
                world.levels[level].char_living = new character[1];
                if (randParts)
                {
                    world.levels[level].char_living[0] = CreateActor(pos, Z, Char_Type, Cont_Type, true);
                    world.levels[level].char_living[0].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[0].attrib.hp_current = world.levels[world.i_currentLvl].char_living[0].attrib.hp_max = 100;
                }
                else
                {
                    world.levels[level].char_living[0] = CreateActor(pos, Z, Char_Type, Cont_Type, name);
                    world.levels[level].char_living[0].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[0].attrib.hp_current = world.levels[world.i_currentLvl].char_living[0].attrib.hp_max = 100;
                }
            }
        }
        public static void addActor(character c,int level)
        {
            try
            {
                character[] old = world.levels[level].char_living;
                world.levels[level].char_living = new character[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[level].char_living[i] = old[i];
                }

                world.levels[level].char_living[old.Length] = c;
                //world.levels[world.i_currentLvl].char_living[old.Length].attrib = new skill(100);
                //world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_current = world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_max = 100;
            }
            catch
            {
                world.levels[level].char_living = new character[1];

                world.levels[level].char_living[0] = c;
                //world.levels[world.i_currentLvl].char_living[0].attrib = new skill(100);
                //world.levels[world.i_currentLvl].char_living[0].attrib.hp_current = world.levels[world.i_currentLvl].char_living[0].attrib.hp_max = 100;
            }
        }
        public static void addActor(character c)
        {
            try
            {
                character[] old = world.levels[world.i_currentLvl].char_living;
                world.levels[world.i_currentLvl].char_living = new character[old.Length + 1];

                for (int i = 0; i != old.Length; i++)
                {
                    world.levels[world.i_currentLvl].char_living[i] = old[i];
                }

                world.levels[world.i_currentLvl].char_living[old.Length] = c;
                    //world.levels[world.i_currentLvl].char_living[old.Length].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_current = world.levels[world.i_currentLvl].char_living[old.Length].attrib.hp_max = 100;
            }
            catch
            {
                world.levels[world.i_currentLvl].char_living = new character[1];

                    world.levels[world.i_currentLvl].char_living[0] = c;
                    //world.levels[world.i_currentLvl].char_living[0].attrib = new skill(100);
                    //world.levels[world.i_currentLvl].char_living[0].attrib.hp_current = world.levels[world.i_currentLvl].char_living[0].attrib.hp_max = 100;
            }
        }
        public static character CreateActor(Vector2 pos, float Z, CharType type, ControlType ctype, bool randomParts)
        {
            character output;
            string name = "Herbert the Pervert";
            while(LvlFunc.nameExists(name))
            {
             name = gfunc.RandomName(true);
            } 

            int CharID = (int)gfunc.RandomNumber(0, world.characters.Length - 1);
            output = world.characters[CharID];

            for (int i = 0; i != output.body.Length; i++)
            {
                output.body[i].maxHP = output.body[i].currentHP = 200;
            }

            if (randomParts)
            {
                int legs = gfunc.roundUp(gfunc.RandomNumber(0, world.characters.Length - 1));
                int arms = gfunc.roundUp(gfunc.RandomNumber(0, world.characters.Length - 1));
                int torso = gfunc.roundUp(gfunc.RandomNumber(0, world.characters.Length - 1));
                int head = gfunc.roundUp(gfunc.RandomNumber(0, world.characters.Length - 1));
                for (int i = 0; i != output.body.Length; i++)
                {

                    if (output.body[i].name == "TORSO_LOWER" || output.body[i].name == "TORSO_UPPER")
                    {
                        output.body[i].texture = world.characters[torso].body[i].texture;
                    }
                    if (output.body[i].name == "HEAD")
                    {
                        output.body[i].texture = world.characters[head].body[i].texture;
                    }
                    if (output.body[i].name == "BICEP_R" || output.body[i].name == "BICEP_L" ||
                        output.body[i].name == "FOREARM_R" || output.body[i].name == "FOREARM_L" ||
                        output.body[i].name == "HAND_R" || output.body[i].name == "HAND_L")
                    {
                        output.body[i].texture = world.characters[arms].body[i].texture;
                    }
                    if (output.body[i].name == "THIGH_R" || output.body[i].name == "THIGH_L" ||
                        output.body[i].name == "CALF_R" || output.body[i].name == "CALF_L" ||
                        output.body[i].name == "FOOT_R" || output.body[i].name == "FOOT_L")
                    {
                        output.body[i].texture = world.characters[legs].body[i].texture;
                    }
                }
            }
            for (int i = 0; i != output.body.Length; i++)
            {
                if (output.body[i].name == "HEAD")
                {
                    output.body[i].maxHP = output.body[i].currentHP = 250;
                }
                output.body[i].maxHP = output.body[i].currentHP = 250;
            }
             output.attrib.hp_current=output.attrib.hp_max= 1000;

            output = setAnimation(output, "human_idle_stand");
            output.B_baseProps.v_position = pos;
            output.B_baseProps.f_Position_Z = Z;
            output.Type = type;
            output.controlType = ctype;
            if (ctype == ControlType.player_current)
            {
                output.name = Gamer.SignedInGamers[0].Gamertag;
            }
            else
            {
                output.name = name;
            }
            output.special_tag = -1;
            output.Inventory.maxWeight = 20;
            //output.attrib.hp_max = 500;

            if (output.controlType == ControlType.NPC)
            {
                output.brain.active = true;
                output.brain.ais_senses = new aiSenses(500, 700, 40, 0, 0, 0);
            }

            return output;
        }
        public static character CreateActor(Vector2 pos, float Z, CharType type, ControlType ctype, string name)
        {
            character output = new character();
            int tag = 0;
            if (name == "")
            {
                int CharID =6;
                while(world.characters[CharID].name == "SPECOPS")
                {
                    CharID = (int)gfunc.roundUp(gfunc.RandomNumber(0, world.characters.Length - 1));
                }
                //output = world.characters[CharID];
                output = character.setTo(world.characters[CharID]);

            }
            else
            {
                for (int i = 0; i != world.characters.Length; i++)
                {
                    if (world.characters[i].name == name)
                    {
                        tag = i;
                    }
                }
                //output = world.characters[tag];
                output = character.setTo(world.characters[tag]);
            }

            for (int i = 0; i != output.body.Length; i++)
            {
                if (output.body[i].name == "HEAD")
                {
                    output.body[i].maxHP = output.body[i].currentHP = 250;
                }
                output.body[i].maxHP = output.body[i].currentHP = 250;
            }
            output.attrib.hp_current = output.attrib.hp_max = 1000;

            output = setAnimation(output, "human_idle_stand");
            output.B_baseProps.v_position = pos;
            output.B_baseProps.f_Position_Z = Z;
            output.Type = type;
            output.controlType = ctype;
            if (ctype == ControlType.player_current)
            {
                output.name = Gamer.SignedInGamers[0].Gamertag;
            }
            else
            {
                output.name = gfunc.RandomName(true);
            }
            output.special_tag = -1;
            output.Inventory.maxWeight = 20;

            if (output.controlType == ControlType.NPC)
            {
                output.brain.active = true;
                output.brain.ais_senses = new aiSenses(500, 700, 40, 0, 0, 0);
            }

            return output;
        } 
        #endregion

        #region Updates
        public static void updateAll()
        {
            if (world.levels[world.i_currentLvl].char_living.Length != 0)
            {
                bool running = true;



                for (int i = 0; i < world.levels[world.i_currentLvl].char_living.Length && running; i++)
                {
                    //world.levels[world.i_currentLvl].char_living[i] = CharFunc.updateAnimation(world.levels[world.i_currentLvl].char_living[i], sb);
                    //DirectAnimations(i);
                    if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.player_current &&
                        thisPlayerLiving)
                    {
                        ThisPlayerKills = world.levels[world.i_currentLvl].char_living[i].attrib.kills;
                    }

                    if (!UI.show)
                    {
                        Translate(i);
                        if(world.levels[world.i_currentLvl].char_living.Length > i)
                        if (world.levels[world.i_currentLvl].char_living[i].brain.active)
                        {
                            world.levels[world.i_currentLvl].char_living[i] = AIfunc.updateNode(world.levels[world.i_currentLvl].char_living[i]);
                        }
                        world.levels[world.i_currentLvl].char_living[i] = updateAttributes(world.levels[world.i_currentLvl].char_living[i]);
                        DirectAnimations(i);
                        world.levels[world.i_currentLvl].char_living[i] = CharFunc.updateAnimation(world.levels[world.i_currentLvl].char_living[i]);
                        CharUseDoor(i);
                    }
                    if (world.levels[world.i_currentLvl].char_living.Length > i)
                    {
                        HumanDirectControl(i);
                    }
                    else
                    {
                        running = false;    // makes sure that this loop doesnt continue running
                        // for no longer existent characters
                    }
                }
            }
        }

        private static void CharUseDoor(int i)
        {
            if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
            {
                character temp = LvlFunc.CharacterUseDoor(world.levels[world.i_currentLvl].char_living[i], i);

                if (temp.controlType == ControlType.player_current)
                {
                    for (int j = 0; j != world.levels[world.i_currentLvl].char_living.Length; j++)
                    {
                        if (world.levels[world.i_currentLvl].char_living[j].name == temp.name)
                        {
                            world.levels[world.i_currentLvl].char_living[j] = temp; //find the trasported char and assign props to him
                        }
                    }
                }
            }
        }
        private static character updateAttributes(character input)
        {
            if (input.Type != CharType.zombie)
            {
                if (input.attrib.hp_current > 0)
                {
                    input.attrib.score++;
                    #region Hunger
                    if (input.attrib.hun_current < 1)
                    {
                        input.attrib.hun_current += 0.00001f; //hunger
                        input.attrib.hp_current -= (float)Math.Abs(input.attrib.hp_current - (input.attrib.hp_max / (input.attrib.hun_current + 1)));
                        if (input.controlType == ControlType.player_current)
                        {
                            if (input.attrib.hun_current > .9f && input.attrib.hun_current < .901f)
                                UI.addMessage("Hunger is at 90%");
                            if (input.attrib.hun_current > .5f && input.attrib.hun_current < .501f)
                                UI.addMessage("Hunger is at 50%");
                            if (input.attrib.hun_current > .75f && input.attrib.hun_current < .751f)
                                UI.addMessage("Hunger is at 75%");
                        }
                    }
                    else
                    {
                        input.attrib.hp_current -= 0.001f; //if hunger is max then kill player slowly
                    }
                    #endregion

                    #region Stamina
                    if (input.attrib.sta_current < input.attrib.sta_max)
                    {
                        input.attrib.sta_current += (1 / ((input.attrib.hun_current / 2) + 1)) / (250f);
                        input.attrib.sta_prog += 1f / 2500f;
                        if (input.attrib.sta_prog > input.attrib.sta_max)
                        {
                            input.attrib.sta_max++;
                            input.attrib.sta_prog = 0;
                            if (input.controlType == ControlType.player_current)
                                UI.addMessage("Stamina increased to level " + input.attrib.sta_max.ToString());
                        }
                    }
                    #endregion

                    #region StealthAndStrength
                    input.attrib.stl_current = (input.attrib.stl_max /
                ((gfunc.findVelocity(input.B_baseProps.v_velocity) + ((input.attrib.hun_current + 1) * (input.Inventory.CurWeight)) + 1))); //update stealth
                    input.attrib.str_current = input.attrib.str_max / (input.attrib.hun_current + 1);

                    if (gfunc.findVelocity(input.B_baseProps.v_velocity) > 0)
                    {
                        input.attrib.stl_prog += (1f / 2500f) * input.attrib.stl_current;
                        if (input.attrib.stl_prog > input.attrib.stl_max)
                        {
                            input.attrib.stl_max++;
                            input.attrib.stl_prog = 0;
                            if (input.controlType == ControlType.player_current)
                                UI.addMessage("Stealth increased to level " + input.attrib.stl_max.ToString());
                        }
                        input.attrib.str_prog += ((1f / 2500f) * ((input.Inventory.CurWeight)));
                        if (input.attrib.str_prog > input.attrib.str_max)
                        {
                            input.attrib.str_max++;
                            input.attrib.str_prog = 0;
                            if (input.controlType == ControlType.player_current)
                                UI.addMessage("Strength increased to level " + input.attrib.str_max.ToString());
                        }
                    }
                    #endregion

                    input.attrib.med_current = input.attrib.med_max / input.attrib.hun_current; //med update
                    input.attrib.mech_current = input.attrib.mech_max / input.attrib.hun_current; //med update
                }
            }
            return input;
        }
        private static void Translate(int i)
        {
                if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
                {
                    if (world.levels[world.i_currentLvl].char_living[i].action == "idle" || world.levels[world.i_currentLvl].char_living[i].action == "reload")
                        world.levels[world.i_currentLvl].char_living[i].B_baseProps = physics.phyFunc.update(world.levels[world.i_currentLvl].char_living[i].B_baseProps);
                }
                world.levels[world.i_currentLvl].char_living[i].current_weap.shooting = false;
        }
        private static void HumanDirectControl(int i)
        {
            #region CurrentHumanPlayerControls
            if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.player_current)
            {
                KeyboardState ks = Keyboard.GetState();
                #region EquipPDA
                if (Controller.is_y(PlayerIndex.One))
                {
                    if (UI.translationDone)
                    {
                        if (UI.show)
                        {
                            UI.show = false;
                            UI.showMenu = false;
                            UI.translationDone = false;
                        }
                        else
                        {
                            UI.show = true;
                            UI.showMenu = true;
                            UI.translationDone = false;
                        }
                    }
                }
                #endregion
                if (!UI.show && UI.translationDone)
                {
                    #region DoPlayerControls
                    if (world.levels[world.i_currentLvl].char_living[i].action == "idle" || world.levels[world.i_currentLvl].char_living[i].action == "reload")
                    {
                        Vector2 v_RightStick = Controller.rStick(PlayerIndex.One);
                        Vector2 v_LeftStick = Controller.lStick(PlayerIndex.One);
                        if (ks.IsKeyDown(Keys.A))
                        {
                            v_LeftStick.X = -1;
                        }
                        if (ks.IsKeyDown(Keys.D))
                        {
                            v_LeftStick.X = 1;
                        }
                        if(ks.IsKeyDown(Keys.W))
                        {
                            v_LeftStick.Y = 1;
                        }
                        if (ks.IsKeyDown(Keys.S))
                        {
                            v_LeftStick.Y = -1;
                        }

                        Vector2 v_oldPos = world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position;
                        float moveMod = 1f;

                        v_LeftStick.X = -v_LeftStick.X;

                        if (Controller.rTrigger(PlayerIndex.One) != 0)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].current_weap.name != "unarmed" &&
                                world.levels[world.i_currentLvl].char_living[i].current_weap.name != null)
                            {
                                world.levels[world.i_currentLvl].char_living[i] = characterFireWeapon(world.levels[world.i_currentLvl].char_living[i]);
                            }
                        }
                        else
                        {
                            world.levels[world.i_currentLvl].char_living[i].current_weap.shooting = false;
                            world.levels[world.i_currentLvl].char_living[i].current_weap.timeWaited = world.levels[world.i_currentLvl].char_living[i].current_weap.ROFdelay;
                        }

                        if (Controller.rStick(PlayerIndex.One).X < 0)
                        {
                            world.levels[world.i_currentLvl].char_living[i].pointingRight = false;
                        }
                        if (Controller.rStick(PlayerIndex.One).X > 0)
                        {
                            world.levels[world.i_currentLvl].char_living[i].pointingRight = true;
                        }


                        camera.follow(world.levels[world.i_currentLvl].char_living[i].B_baseProps,
                                      300,
                                      world.levels[world.i_currentLvl].char_living[i].angle / 4,
                                      world.levels[world.i_currentLvl].char_living[i].pointingRight);

                        if (world.levels[world.i_currentLvl].char_living[i].B_baseProps.airBorn)
                        {
                            moveMod = .4f;
                        }
                        else
                        {
                            moveMod = 1f;

                            if (Controller.is_rShoulder(PlayerIndex.One))
                            {
                                //world.levels[world.i_currentLvl].char_living[i] = weapFunc.reload(world.levels[world.i_currentLvl].char_living[i]);
                                if (world.levels[world.i_currentLvl].char_living[i].current_weap.name != null &&
                                    world.levels[world.i_currentLvl].char_living[i].current_weap.name != "unarmed"&&
                                    world.levels[world.i_currentLvl].char_living[i].action != "reload")
                                {
                                    world.levels[world.i_currentLvl].char_living[i].action = "reload";
                                    character c = world.levels[world.i_currentLvl].char_living[i];
                                    switch (c.current_weap.Pt)
                                    {
                                        case ProjectileType.AR223:
                                        case ProjectileType.AR762:
                                            weapFunc.PlayReloadAR();
                                            break;
                                        case ProjectileType.P357:
                                        case ProjectileType.P38:
                                            weapFunc.PlayReloadRevolver();
                                            break;
                                        case ProjectileType.CAR22:
                                            weapFunc.PlayReload22();
                                            break;
                                        case ProjectileType.SMG9:
                                        case ProjectileType.SR50:
                                        case ProjectileType.SMG45:
                                            weapFunc.PlayReloadPistol();
                                            break;
                                        case ProjectileType.SG_pellet:
                                            weapFunc.PlayReloadShotgun();
                                            break;
                                    }
                                }
                            }
                            if (Controller.is_lStick(PlayerIndex.One))
                            {
                                moveMod *= 3;
                            }
                            if (Controller.is_a(PlayerIndex.One))
                            {
                                world.levels[world.i_currentLvl].char_living[i].B_baseProps.f_Velocity_Z = 15f;
                            }
                            if (Controller.is_x(PlayerIndex.One))
                            {
                                //world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_pickup");
                                if (world.levels[world.i_currentLvl].char_living[i].action != "pickup")
                                {
                                    world.levels[world.i_currentLvl].char_living[i].action = "pickup";
                                }
                                else
                                {
                                    world.levels[world.i_currentLvl].char_living[i].action = "idle";
                                }
                                //pickupItem(i);
                            }
                            #region DpadWeaponEquip
                            if (Controller.is_Dleft(PlayerIndex.One))
                            {
                                CharFunc.equipWeapon(i, "primary");
                            }
                            if (Controller.is_Dup(PlayerIndex.One))
                            {
                                CharFunc.equipWeapon(i, "secondary");
                            }
                            if (Controller.is_Dright(PlayerIndex.One))
                            {
                                CharFunc.equipWeapon(i, "sidearm");
                            }
                            #endregion


                        }

                        v_LeftStick *= ((moveMod * 30) * ((float)((Math.Log10(world.levels[world.i_currentLvl].char_living[i].attrib.sta_current) + 1) / Math.E) + 1)) / 3;

                        world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_velocity -= v_LeftStick * (world.levels[world.i_currentLvl].char_living[i].attrib.sta_current/world.levels[world.i_currentLvl].char_living[i].attrib.sta_max);
                        if (world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_velocity != Vector2.Zero)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].attrib.sta_current > 0)
                            {
                                world.levels[world.i_currentLvl].char_living[i].attrib.sta_current -= (((world.levels[world.i_currentLvl].char_living[i].Inventory.CurWeight / 10) + 1) * gfunc.findVelocity(world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_velocity)) / 5000f;
                                //world.levels[world.i_currentLvl].char_living[i].attrib.sta_current = (float)Math.Abs(world.levels[world.i_currentLvl].char_living[i].attrib.sta_current);
                            }
                        }

                        float oldAng = world.levels[world.i_currentLvl].char_living[i].angle;
                        world.levels[world.i_currentLvl].char_living[i].angle -= (v_RightStick.Y / 20);

                        if (world.levels[world.i_currentLvl].char_living[i].angle > .61f || world.levels[world.i_currentLvl].char_living[i].angle < -.61f)
                        {
                            world.levels[world.i_currentLvl].char_living[i].angle = oldAng;
                        }
                    }
                    #endregion
                }
                else
                {
                    Vector2 stick_r = Controller.lStick(PlayerIndex.One);
                    UI.player_tag = i;

                    if (stick_r.Y != 0)
                    {
                        UI.f_selectedItem -= stick_r.Y / 8;
                    }
                    if (Controller.is_a(PlayerIndex.One))
                    {
                        if (UI.itemTranslationDone)
                        {
                            if (UI.f_selectedItem < 0 || gfunc.roundUp(UI.f_selectedItem) >= UI.m_current.items.Length)
                                UI.f_selectedItem = 0;
                            UI.m_current.items[gfunc.roundUp(UI.f_selectedItem)].selected = true;
                        }
                    }
                    if (Controller.is_b(PlayerIndex.One))
                    {
                        UI.back();
                    }
                }
            }
            #endregion
            Controller.set_rumble(PlayerIndex.One, 0, 0, 0); //kill rumble
        }
        public static void characterFireWeapon(int i)
        {

            if (world.levels[world.i_currentLvl].char_living[i].current_weap.timeWaited >= world.levels[world.i_currentLvl].char_living[i].current_weap.ROFdelay &&
                world.levels[world.i_currentLvl].char_living[i].currentAnim.name != "human_sprint_twohanded_armed" && !world.levels[world.i_currentLvl].char_living[i].B_baseProps.airBorn)
            {
                if (world.levels[world.i_currentLvl].char_living[i].current_weap.clipCurrent > 0)
                {
                    skill attrib = world.levels[world.i_currentLvl].char_living[i].attrib;
                    float inaccuracy = (world.levels[world.i_currentLvl].char_living[i].current_weap.inaccuracy / attrib.str_current);
                    world.levels[world.i_currentLvl].char_living[i].current_weap.shooting = true;
                    Vector2 muzzPos = weapFunc.findMuzzlePos(world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position,
                                                             world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position + world.levels[world.i_currentLvl].char_living[i].body[7].position,
                                                             world.levels[world.i_currentLvl].char_living[i].current_weap.muzzPos,
                                                             world.levels[world.i_currentLvl].char_living[i].angle,
                                                             world.levels[world.i_currentLvl].char_living[i].pointingRight);
                    Vector2 actPos = weapFunc.findMuzzlePos(world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position,
                                                             world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position + world.levels[world.i_currentLvl].char_living[i].body[7].position,
                                                             world.levels[world.i_currentLvl].char_living[i].current_weap.actionPos,
                                                             world.levels[world.i_currentLvl].char_living[i].angle,
                                                             world.levels[world.i_currentLvl].char_living[i].pointingRight);
                    PlayGunSounds(i);

                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Pt == ProjectileType.SG_pellet)
                    {
                        for (int r = 0; r != 3; r++)
                        {
                            float angle = gfunc.RandomNumber(-inaccuracy, inaccuracy);
                            world.levels[world.i_currentLvl].char_living[i] = weapFunc.FireShot(world.levels[world.i_currentLvl].char_living[i],
                                              muzzPos,
                                              world.levels[world.i_currentLvl].char_living[i].current_weap.angle + angle,
                                              world.levels[world.i_currentLvl].char_living[i].current_weap.Pt, i);
                        }
                    }
                    else
                    {

                        world.levels[world.i_currentLvl].char_living[i] = weapFunc.FireShot(world.levels[world.i_currentLvl].char_living[i],
                                                                          muzzPos,
                                                                          world.levels[world.i_currentLvl].char_living[i].current_weap.angle + gfunc.RandomNumber(-inaccuracy, inaccuracy),
                                                                          world.levels[world.i_currentLvl].char_living[i].current_weap.Pt, i);
                    }
                    world.levels[world.i_currentLvl].char_living[i].current_weap.clipCurrent--;
                    world.levels[world.i_currentLvl].char_living[i].current_weap.timeWaited = 0;

                    int force = 5;

                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Pt != ProjectileType.P357 &&
                        world.levels[world.i_currentLvl].char_living[i].current_weap.Pt != ProjectileType.P38)
                    {

                        #region CreateShell

                        effects.particle p_temp = effects.particles.shell_ar[0];

                        switch (world.levels[world.i_currentLvl].char_living[i].current_weap.Pt)
                        {
                            case ProjectileType.AR223:
                            case ProjectileType.AR762:
                            case ProjectileType.SR308:
                            case ProjectileType.SR50:
                                p_temp = effects.particles.shell_ar[0];
                                break;
                            case ProjectileType.CAR22:
                            case ProjectileType.SMG45:
                            case ProjectileType.SMG9:
                                p_temp = effects.particles.shell_pistol[0];
                                break;
                            case ProjectileType.SG_pellet:
                                p_temp = effects.particles.shell_shotgun[0];
                                break;
                        }

                        p_temp.Velocity = new Vector3(gfunc.randomPos(-new Vector2(1, 0) * force, new Vector2(1, 0) * force),
                                                      gfunc.RandomNumber(-force, 1));
                        p_temp.friction = .95f;

                        p_temp.position = new Vector3(actPos.X,
                                                      world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position.Y,
                                                      actPos.Y - (world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position.Y + (float)gfunc.RandomNumber(-2, 2)));
                        p_temp.angular_velo = gfunc.RandomNumber(-.5, .5);
                        effects.PEngine.particle_create(p_temp);
                        #endregion

                    }
                    #region CreateSmoke
                    effects.particle p_temp1 = effects.particles.smoke_local[gfunc.roundUp(gfunc.RandomNumber(0, 1))];
                    p_temp1.position = new Vector3(actPos.X,
                                                  world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position.Y,
                                                  actPos.Y - world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position.Y);
                    force = 5;
                    p_temp1.Velocity = new Vector3(gfunc.randomPos(-new Vector2(.25f, .25f) * force, new Vector2(.25f, -.75f) * force), 0);
                    p_temp1.angular_velo = gfunc.RandomNumber(-.1, .1);
                    effects.PEngine.particle_create(p_temp1);

                    force = 3;
                    p_temp1.Velocity = new Vector3(gfunc.randomPos(-new Vector2(.25f, .25f) * force, new Vector2(.25f, -.75f) * force), 0);
                    p_temp1.angular_velo = gfunc.RandomNumber(-.1, .1);
                    p_temp1.position = new Vector3(muzzPos, 0);
                    effects.PEngine.particle_create(p_temp1);
                    #endregion
                    if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.player_current)
                        Controller.set_rumble(PlayerIndex.One, 0, world.levels[world.i_currentLvl].char_living[i].current_weap.ROFdelay, 5); //shake the controller when firing
                }
                else
                {
                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.wear != -1)
                    {
                        world.levels[world.i_currentLvl].char_living[i].current_weap.wear = -1;
                        weapFunc.PlayDryFire();
                    }
                }
            }
            else
            {
                world.levels[world.i_currentLvl].char_living[i].current_weap.shooting = false;
                if (world.levels[world.i_currentLvl].char_living[i].current_weap.auto)
                    world.levels[world.i_currentLvl].char_living[i].current_weap.timeWaited++;
            }
        }

        private static void PlayGunSounds(int i)
        {
            switch (world.levels[world.i_currentLvl].char_living[i].current_weap.Pt)
            {
                case ProjectileType.AR223:
                case ProjectileType.AR762:
                    weapFunc.PlayFireAR();
                    break;
                case ProjectileType.SMG9:
                    weapFunc.PlayFire9mm();
                    break;
                case ProjectileType.CAR22:
                    weapFunc.PlayFire22();
                    break;
                case ProjectileType.SG_pellet:
                    weapFunc.PlayFire12();
                    break;
                case ProjectileType.P357:
                case ProjectileType.P38:
                    weapFunc.PlayFire357();
                    break;
                case ProjectileType.SR50:
                    weapFunc.PlayFire50();
                    break;
            }
        }
        public static character characterFireWeapon(character  c)
        {

            if (c.current_weap.timeWaited >= c.current_weap.ROFdelay &&
                c.currentAnim.name != "human_sprint_twohanded_armed" && !c.B_baseProps.airBorn)
            {
                if (c.current_weap.clipCurrent > 0)
                {
                    skill attrib = c.attrib;
                    float inaccuracy = (c.current_weap.inaccuracy / attrib.str_current);
                    c.current_weap.shooting = true;
                    Vector2 muzzPos = weapFunc.findMuzzlePos(c.B_baseProps.v_position,
                                                             c.B_baseProps.v_position + c.body[7].position,
                                                             c.current_weap.muzzPos,
                                                             c.angle,
                                                             c.pointingRight);
                    Vector2 actPos = weapFunc.findMuzzlePos(c.B_baseProps.v_position,
                                                             c.B_baseProps.v_position + c.body[7].position,
                                                             c.current_weap.actionPos,
                                                             c.angle,
                                                             c.pointingRight);
                    int i = findCharacter(c);

                    PlayGunSounds(i);

                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Pt == ProjectileType.SG_pellet)
                    {
                        for (int r = 0; r != 3; r++)
                        {
                            float angle = gfunc.RandomNumber(-inaccuracy, inaccuracy);
                            c = weapFunc.FireShot(c, muzzPos,
                            world.levels[world.i_currentLvl].char_living[i].current_weap.angle + angle,
                            world.levels[world.i_currentLvl].char_living[i].current_weap.Pt, i);
                        }
                    }
                    else
                    {
                        c = weapFunc.FireShot(c, muzzPos,
                                          world.levels[world.i_currentLvl].char_living[i].current_weap.angle + gfunc.RandomNumber(-inaccuracy, inaccuracy),
                                          world.levels[world.i_currentLvl].char_living[i].current_weap.Pt, i);
                    }

                    c.current_weap.clipCurrent--;
                    c.current_weap.timeWaited = 0;

                    int force = 5;

                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Pt != ProjectileType.P357 &&
                    world.levels[world.i_currentLvl].char_living[i].current_weap.Pt != ProjectileType.P38)
                    {
                        #region CreateShell

                        effects.particle p_temp = effects.particles.shell_ar[0];

                        switch (c.current_weap.Pt)
                        {
                            case ProjectileType.AR223:
                            case ProjectileType.AR762:
                            case ProjectileType.SR308:
                            case ProjectileType.SR50:
                                p_temp = effects.particles.shell_ar[0];
                                break;
                            case ProjectileType.CAR22:
                            case ProjectileType.SMG45:
                            case ProjectileType.SMG9:
                                p_temp = effects.particles.shell_pistol[0];
                                break;
                            case ProjectileType.SG_pellet:
                                p_temp = effects.particles.shell_shotgun[0];
                                break;
                        }

                        p_temp.Velocity = new Vector3(gfunc.randomPos(-new Vector2(1, 0) * force, new Vector2(1, 0) * force),
                                                      gfunc.RandomNumber(-force, 1));
                        p_temp.friction = .95f;

                        p_temp.position = new Vector3(actPos.X,
                                                      c.B_baseProps.v_position.Y,
                                                      actPos.Y - (c.B_baseProps.v_position.Y + (float)gfunc.RandomNumber(-2, 2)));
                        p_temp.angular_velo = gfunc.RandomNumber(-.5, .5);
                        effects.PEngine.particle_create(p_temp);
                        #endregion
                    }
                    #region CreateSmoke
                    effects.particle p_temp1 = effects.particles.smoke_local[gfunc.roundUp(gfunc.RandomNumber(0, 1))];
                    p_temp1.position = new Vector3(actPos.X,
                                                  c.B_baseProps.v_position.Y,
                                                  actPos.Y - c.B_baseProps.v_position.Y);
                    force = 5;
                    p_temp1.Velocity = new Vector3(gfunc.randomPos(-new Vector2(.25f, .25f) * force, new Vector2(.25f, -.75f) * force), 0);
                    p_temp1.angular_velo = gfunc.RandomNumber(-.1, .1);
                    effects.PEngine.particle_create(p_temp1);

                    force = 3;
                    p_temp1.Velocity = new Vector3(gfunc.randomPos(-new Vector2(.25f, .25f) * force, new Vector2(.25f, -.75f) * force), 0);
                    p_temp1.angular_velo = gfunc.RandomNumber(-.1, .1);
                    p_temp1.position = new Vector3(muzzPos, 0);
                    effects.PEngine.particle_create(p_temp1);
                    #endregion
                    if (c.controlType == ControlType.player_current)
                        Controller.set_rumble(PlayerIndex.One, 0, c.current_weap.ROFdelay, 5); //shake the controller when firing
                }
                else
                {
                    if (c.current_weap.wear != -1)
                    {
                        c.current_weap.wear = -1;
                        weapFunc.PlayDryFire();
                    }
                }
            }
            else
            {
                c.current_weap.shooting = false;
                if (c.current_weap.auto)
                    c.current_weap.timeWaited++;
            }
            return c;
        }
        public static void assignWeapon(weapon w,int c,string slot)
        {
            character Char = world.levels[world.i_currentLvl].char_living[c];

            switch (slot)
            {
                case "primary":
                    if (Char.Primary_weap.name != null && Char.Primary_weap.name != "unarmed")
                    {
                        //Char.Inventory = itemFunc.createItem_INVEN(Char.Inventory,
                        //                          itemFunc.createItemFromWeapon(w),
                        //                          Vector2.Zero,
                        //                          0);

                    }
                    //Char = RemoveWeapon(Char,w.name);
                        Char.Primary_weap = w;
                    break;

                case "secondary":

                    //Char = RemoveWeapon(Char, w.name);
                        Char.Secondary_weap = w;
                    break;

                case "sidearm":

                    //Char = RemoveWeapon(Char, w.name);
                        Char.SideArm_weap = w;
                    break;
            }
            Char.current_weap = w;
            world.levels[world.i_currentLvl].char_living[c] = Char;
        }
        public static void equipWeapon(int c, string slot)
        {
            character Char = world.levels[world.i_currentLvl].char_living[c];

            try
            {
                switch (Char.weaponInUse)
                {
                    case "primary":
                        Char.Primary_weap = Char.current_weap;
                        break;
                    case "secondary":
                        Char.Secondary_weap = new weapon();
                        break;
                    case "sidearm":
                        Char.SideArm_weap = Char.current_weap;
                        break;
                }

                switch (slot)
                {
                    case "primary":
                            Char.weaponInUse = "primary";
                            Char.current_weap = Char.Primary_weap;
                        break;

                    case "secondary":
                            Char.weaponInUse = "secondary";
                            Char.current_weap = new weapon();
                        break;

                    case "sidearm":
                        Char.weaponInUse = "sidearm";
                        Char.current_weap = Char.SideArm_weap;
                        break;
                }
            }
            catch { }

            world.levels[world.i_currentLvl].char_living[c] = Char;
        }

        private static character RemoveWeapon(character Char)
        {
            int thisWeapon = itemFunc.findInventoryItem(Char.current_weap.name, Char);
            Char.Inventory.items = itemFunc.killItem_INVEN(Char.Inventory.items, thisWeapon);
            return Char;
        }
        private static character RemoveWeapon(character Char,string name)
        {
            int thisWeapon = itemFunc.findInventoryItem(name, Char);
            Char.Inventory.items = itemFunc.killItem_INVEN(Char.Inventory.items, thisWeapon);
            return Char;
        }
        public static void equipFood(int c, item food)
        {
            PlayCharEat();
            character Char = world.levels[world.i_currentLvl].char_living[c];
            float healing = .25f;

            if (food.special == "heal")
            {
                healing = (food.quality/100)*2;
            }

            Char.attrib.hun_current *= 1 - (food.quality / 100);

            if (world.levels[world.i_currentLvl].char_living[c].controlType == ControlType.player_current)
            UI.addMessage("Hunger decreased by " + food.quality.ToString() + "%");

            Char.attrib.hp_current *= (1 + ((food.quality / 100) * healing));

            if (world.levels[world.i_currentLvl].char_living[c].controlType == ControlType.player_current)
            UI.addMessage("Healed by " + (food.quality * healing).ToString() + "%");

            world.levels[world.i_currentLvl].char_living[c] = Char;
        }
        public static int findCharacter(character c)
        {
            int o = -1;
            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
                {
                    if (world.levels[world.i_currentLvl].char_living[i].name == c.name)
                    {
                        o = i;
                    }
                }
            }
            return o;
        }
        #region PickupAndDropItems
        private static void pickupItem(int i)
        {
            int tag = -1;
            for (int j = 0; j != world.levels[world.i_currentLvl].items.Length; j++)
            {
                Vector2 offset = Vector2.Zero;

                if (world.levels[world.i_currentLvl].char_living[i].pointingRight)
                {
                    offset = new Vector2(120, 0);
                }
                else
                {
                    offset = new Vector2(-120, 0);
                }

                if (Vector2.Distance(world.levels[world.i_currentLvl].items[j].B_base.v_position,
                                    world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position + offset) < 45)
                {
                        world.levels[world.i_currentLvl].char_living[i].Inventory = itemFunc.createItem_INVEN(world.levels[world.i_currentLvl].char_living[i].Inventory,
                                                                                                     world.levels[world.i_currentLvl].items[j],
                                                                                                     world.levels[world.i_currentLvl].items[j].B_base.v_position,
                                                                                                     0);
                        tag = j;

                        j = world.levels[world.i_currentLvl].items.Length - 1;
                  
                }
            }
            if (tag != -1)
            {
                world.levels[world.i_currentLvl].items = itemFunc.killItem_INVEN(world.levels[world.i_currentLvl].items, tag);
            }
        }
        private static int pickup_targetItem(character c)
        {
            int tag = -1;
            if(world.levels[world.i_currentLvl].items != null)
            for (int j = 0; j != world.levels[world.i_currentLvl].items.Length; j++)
            {
                Vector2 offset = Vector2.Zero;

                if (c.pointingRight)
                {
                    offset = new Vector2(100, 0);
                }
                else
                {
                    offset = new Vector2(-100, 0);
                }

                if (Vector2.Distance(world.levels[world.i_currentLvl].items[j].B_base.v_position,
                                    c.B_baseProps.v_position + offset) < 40 && world.levels[world.i_currentLvl].items[j].usable)
                {
                    tag = j;

                    j = world.levels[world.i_currentLvl].items.Length - 1;
                }
            }
            return tag;
        }
        private static character pickup_placeInInventory(int item, character Char)
        {
            try
            {
                if (item != -1)
                {
                    if (Char.Inventory.maxWeight > Char.Inventory.CurWeight + world.levels[world.i_currentLvl].items[item].weight)
                    {
                        Char.Inventory = itemFunc.createItem_INVEN(Char.Inventory,
                                                                 world.levels[world.i_currentLvl].items[item],
                                                                 world.levels[world.i_currentLvl].items[item].B_base.v_position,
                                                                 0);
                        world.levels[world.i_currentLvl].items = itemFunc.killItem_INVEN(world.levels[world.i_currentLvl].items, item);
                    }
                    else
                    {
                        if (Char.controlType == ControlType.player_current)
                        {
                            UI.addMessage("You cannot carry so much weight.");
                        }
                        world.levels[world.i_currentLvl].items[item].B_base.v_position -= new Vector2(-9999, -9999);

                    }

                }
            }
            catch
            {
                if (Char.controlType == ControlType.NPC)
                    Char= AIfunc.packagePickupKill(Char);
            }
            return Char;
        }
        private static character DropItem(character input)
        {
            if (input.special_tag < input.Inventory.items.Length)
            {
                item temp = input.Inventory.items[input.special_tag];
                if (input.pointingRight)
                {
                    temp.B_base.v_velocity.X = 45 + gfunc.RandomNumber(-20, 20);
                }
                else
                {
                    temp.B_base.v_velocity.X = -(45 + gfunc.RandomNumber(-20, 20));
                }
                itemFunc.createItem(temp, input.B_baseProps.v_position, 100);
                input.Inventory.CurWeight -= input.Inventory.items[input.special_tag].weight;
                input.Inventory.items = itemFunc.killItem_INVEN(input.Inventory.items, input.special_tag);
                input.special_tag = -1;
            }
            //world.levels[world.i_currentLvl].items[input.special_tag].B_base.v_position = new Vector2(-9999, -9999);
            return input;
        }
        private static character dropInventory(character input)
        {
            if (input.Inventory.items != null)
            {
                while (input.Inventory.items.Length != 0)
                {
                    Vector2 velocity = gfunc.randomPos(-Vector2.One, Vector2.One)*50;
                    velocity.Y /= 2;
                    itemFunc.createItem(input.Inventory.items[0], input.B_baseProps.v_position, 200, velocity);
                    input.Inventory.items = itemFunc.killItem_INVEN(input.Inventory.items, 0);
                }
            }
            return input;
        }
        private static character dropInventory(character input,int level)
        {
            if (input.Inventory.items != null)
            {
                while (input.Inventory.items.Length != 0)
                {
                    Vector2 velocity = gfunc.randomPos(-Vector2.One, Vector2.One) * 50;
                    velocity.Y /= 2;
                    itemFunc.createItem(input.Inventory.items[0], input.B_baseProps.v_position, 200, level, input.Inventory.items[0].quality);
                    input.Inventory.items = itemFunc.killItem_INVEN(input.Inventory.items, 0);
                }
            }
            return input;
        }
        #endregion
        #region Animation
        private static void DirectAnimations(int i)
        {
            #region ChangeAnimsAccoringToMovment
            if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0 &&
                world.levels[world.i_currentLvl].char_living[i].action !="dead")
            {

                if (!world.levels[world.i_currentLvl].char_living[i].B_baseProps.airBorn)
                {
                    float X = world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_velocity.X;
                    float Y = world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_velocity.Y;
                    float TempVelo = (float)Math.Sqrt((X * X) + (Y * Y));

                    if (world.levels[world.i_currentLvl].char_living[i].action != "idle")
                    {
                        switch (world.levels[world.i_currentLvl].char_living[i].action)
                        {
                            case "pickup":
                                world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_pickup");
                                break;
                            case "drop":
                                world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_drop");
                                break;
                            case "attack":
                                if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.zombie)
                                {
                                    world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "zomb_attack");
                                }
                                break;
                            case "reload":
                                    if (TempVelo > .1f)
                                    {
                                        if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_twoHanded)
                                        {
                                            world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_walk_twohanded_reload");
                                        }
                                        else if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_oneHanded)
                                        {
                                            world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_walk_onehanded_reload");
                                        }
                                    }
                                    else
                                    {
                                        if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_twoHanded)
                                        {
                                            world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_twohanded_reload");
                                        }
                                        else if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_oneHanded)
                                        {
                                            world.levels[world.i_currentLvl].char_living[i] = setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_onehanded_reload");
                                        }

                                    }
                                break;
                        }
                    }
                    else
                    {
                        if (TempVelo > .1f)
                        {
                            if (TempVelo < 10)
                            {
                                if (world.levels[world.i_currentLvl].char_living[i].Type != CharType.zombie)
                                {
                                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.name != null && world.levels[world.i_currentLvl].char_living[i].current_weap.name != "unarmed")
                                    {
                                        if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_twoHanded)
                                        {
                                            world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_walk_twohanded_armed_alert");
                                        }
                                        if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_oneHanded)
                                        {
                                            world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_walk_onehanded");
                                        }
                                    }
                                    else
                                    {
                                        world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_walk"); //if moving slower than 20 walk
                                    }
                                }
                                else
                                {
                                    world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "zomb_walk");
                                }
                            }
                            else
                            {
                                if (world.levels[world.i_currentLvl].char_living[i].Type != CharType.zombie)
                                {
                                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_twoHanded)
                                    {
                                        world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_sprint_twohanded_armed");
                                    }
                                    else
                                    {
                                        world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_sprint"); //if moving faster than 20 run (change to)
                                    }
                                }
                                else
                                {
                                    world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "zomb_run");
                                }
                            }
                        }
                        else
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].Type != CharType.zombie) //if not moving
                            {
                                if (world.levels[world.i_currentLvl].char_living[i].current_weap.name != null && world.levels[world.i_currentLvl].char_living[i].current_weap.name != "unarmed")
                                {
                                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_twoHanded)
                                    {
                                        world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_twohanded_alert");
                                    }
                                    if (world.levels[world.i_currentLvl].char_living[i].current_weap.Stance == StanceType.Gun_oneHanded)
                                    {
                                        world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_onehanded");
                                    }
                                }
                                else
                                {
                                    world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_idle_stand"); //if not falling and not zomb
                                }
                            }
                            else
                            {
                                world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "zomb_idle"); //if not falling and zomb
                            }
                        }
                    }
                    if (world.levels[world.i_currentLvl].char_living[i].controlType == ControlType.NPC)
                    {
                        if (X < 0)
                        {
                            world.levels[world.i_currentLvl].char_living[i].pointingRight = false;
                        }
                        else
                        {
                            world.levels[world.i_currentLvl].char_living[i].pointingRight = true;
                        }
                    }
                }
                else
                {
                    world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_fall"); //if falling
                }
            }
            else
            {       //if the char is dead
                if (world.levels[world.i_currentLvl].char_living[i].Type != CharType.zombie)
                {
                    world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "human_unarmed_die");
                }
                else
                {
                    world.levels[world.i_currentLvl].char_living[i] = CharFunc.setAnimation(world.levels[world.i_currentLvl].char_living[i], "zomb_death");
                }
            }
            #endregion
        }
        public static character setAnimation(character input, string animName)
        {
            if (input.currentAnim.name != animName)
            {
                //string oldAnim = input.currentAnim.name;
                int i_taggedAnim = -1;

                for (int i = 0; i != world.animations.Length; i++)
                {
                    if (world.animations[i].name == animName)
                    {
                        i_taggedAnim = i;
                    }
                }
                if (i_taggedAnim != -1)
                {
                    input.currentAnim = world.animations[i_taggedAnim];

                    // Vector2 largest = Vector2.Zero;
                    //for (int i = 0; i != input.currentAnim.f_frame.Length; i++)
                    //{
                    //    input.f_currentFrame = i;
                    //    Vector2 temp = GetTextureDimensions(input);
                    //    if (temp.X > largest.X)
                    //        largest.X = temp.X;
                    //    if (temp.Y > largest.Y)
                    //        largest.Y = temp.Y;
                    //}
                    if(input.action != "reload")
                    input.f_currentFrame = 0;
                    //input.v_renderDims = largest;
                }

            }
            return input;
        }
        public static character updateAnimation(character input)
        {
            if (input.f_currentFrame >= input.currentAnim.f_frame.Length - 1)
            {
                if (input.attrib.hp_current > 0 && input.action != "dead")
                {
                    if (input.action == "pickup")
                    {
                        if (input.controlType == ControlType.NPC)
                        {
                            if (input.brain.current_package_tag != "")
                            {
                                input.action = "idle";
                                input.special_tag = -1;
                                input = AIfunc.packagePickupKill(input);
                            }
                        }
                        else
                        {
                            input = pickup_placeInInventory(input.special_tag, input);
                            input.action = "idle";
                            input.special_tag = -1;
                        }

                    }
                    if (input.action == "attack" && input.Type == CharType.zombie)
                    {
                        input = AIfunc.packageAttackKill(input);
                    }
                    if (input.action == "drop")
                    {
                        input.action = "idle";
                    }
                    if (input.action == "reload")
                    {
                        input.action = "idle";
                        input = weapFunc.reload(input);
                    }

                    input.f_currentFrame = 0;
                }
            }


            float[] oldAngle = new float[input.body.Length];
            for (int i = 0; i != input.body.Length; i++)
            {
                //if (input.body[i].v_collison_mapOR == null)
                //{
                //    input.body[i].v_collison_mapOR = gfunc.collision.MakeCollisionMap(Vector2.Zero, input.body[i].texture[0], gfunc.i_collision_accuracy);
                //}
                //else
                //{
                //    input.body[i].v_collison_map = new Vector2[input.body[i].v_collison_mapOR.Length];
                //    for (int j = 0; j != input.body[i].v_collison_mapOR.Length; j++)
                //    {
                //        input.body[i].v_collison_map[j] = input.body[i].v_collison_mapOR[j];
                //    }
                //}

                oldAngle[i] = input.body[i].angle;

                input.body[i].position = input.currentAnim.f_frame[(int)input.f_currentFrame].position[i];
                input.body[i].angle = input.currentAnim.f_frame[(int)input.f_currentFrame].angle[i];

                input.body[i].v_collison_map = gfunc.collision.RotateCollisionMap(input.body[i].v_collison_map, input.body[i].pivot, input.body[i].angle);
            }
            if (input.B_baseProps.v_velocity != Vector2.Zero)
            {
                float velocity = gfunc.findVelocity(input.B_baseProps.v_velocity);
                if (input.controlType == ControlType.player_current)
                Controller.set_rumble(PlayerIndex.One, velocity / 50, 0, 2);
            }
            if (input.f_currentFrame >= 15)
            {
                    if (input.action == "pickup" && input.special_tag == -1)
                    {
                        if (input.controlType != ControlType.NPC)
                        {
                            input.special_tag = pickup_targetItem(input);
                            if(input.special_tag != -1)
                            world.levels[world.i_currentLvl].items[input.special_tag].B_base.v_position += new Vector2(-9999, -9999);
                        }
                        else
                        {
                            int tempTag = Convert.ToInt32(input.brain.current_package_tag);
                            if (tempTag < world.levels[world.i_currentLvl].items.Length)
                            {
                                input.special_tag = tempTag;
                                world.levels[world.i_currentLvl].items[tempTag].B_base.v_position += new Vector2(-9999, -9999);
                            }
                        }
                    }
                    if (input.action == "drop" && input.special_tag != -1 && input.f_currentFrame >= 25)
                    {
                        input = DropItem(input);
                    }
                    if (input.f_currentFrame <= 15.5f)
                    {
                        if (input.action == "attack" && input.Type == CharType.zombie)
                        {
                            int tag = AIfunc.findNPC(input.brain.current_package_tag);
                            if (tag != -1 &&
                                Vector2.Distance(world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position, input.B_baseProps.v_position) < 100 )
                            {
                                PlayZombStrike();
                                if (world.levels[world.i_currentLvl].char_living[tag].controlType == ControlType.player_current)
                                {
                                    if (gfunc.timeWaited(world.levels[world.i_currentLvl].char_living[tag].brain.tick_last, 200))
                                    {
                                        UI.show = false;
                                        UI.showMenu = false;
                                        UI.translationDone = false;
                                        Zombify(tag);
                                    }
                                }
                                else
                                {
                                    Zombify(tag);
                                }
                            }
                        }
                    }
            }
            if (input.f_currentFrame < input.currentAnim.f_frame.Length - 1)
            {
                if (input.action == "pickup")
                {
                    input.f_currentFrame += 1;
                }
                else
                {
                    input.f_currentFrame += .45f;
                }
            }

            input = CharFunc.translate(input, 1, input.angle + input.body[1].angle);


            return input;
        }

        public static void Zombify(int tag)
        {

                world.levels[world.i_currentLvl].char_living[tag].attrib.hp_current = 0;                         //zomb damage here
                if (world.levels[world.i_currentLvl].char_living[tag].attrib.hp_current <= 0)
                {
                    PlayCharDie();
                    if (world.levels[world.i_currentLvl].char_living[tag].controlType == ControlType.player_current)
                    { thisPlayerLiving = false; }
                    world.levels[world.i_currentLvl].char_living[tag] = dropInventory(world.levels[world.i_currentLvl].char_living[tag]);
                    world.levels[world.i_currentLvl].char_living[tag].controlType = ControlType.NPC;
                    world.levels[world.i_currentLvl].char_living[tag].brain.aip_current = aiPackage.turnIntoZomb;
                    world.levels[world.i_currentLvl].char_living[tag].brain.active = true;
                    world.levels[world.i_currentLvl].char_living[tag].brain.tick_last = Game1.time_ticks;
                }
        }
        public static character Zombify(character c)
        {

            c.attrib.hp_current = 0;                         //zomb damage here
            if (c.attrib.hp_current <= 0)
            {
                //PlayCharDie();
                c.controlType = ControlType.NPC;
                c.brain.aip_current = aiPackage.turnIntoZomb;
                c.brain.active = true;
                c.brain.tick_last = Game1.time_ticks;
            }
            return c;
        }
        public static void Zombify(int tag,int level)
        {
            {
                world.levels[level].char_living[tag].attrib.hp_current = 0f;                        //zomb damage here
                if (world.levels[level].char_living[tag].attrib.hp_current <= 0)
                {
                    if (world.levels[level].char_living[tag].controlType == ControlType.player_current)
                    { thisPlayerLiving = false; }
                    world.levels[level].char_living[tag] = dropInventory(world.levels[level].char_living[tag], level);
                    world.levels[level].char_living[tag].controlType = ControlType.NPC;
                    world.levels[level].char_living[tag].brain.aip_current = aiPackage.turnIntoZomb;
                    world.levels[level].char_living[tag].brain.active = true;
                    world.levels[level].char_living[tag].brain.tick_last = Game1.time_ticks;
                }
            }
        }
        public static void drawItemName(SpriteBatch sb, character input)
        {
            int targTag = pickup_targetItem(input);
            if (targTag != -1)
            {
                string name = world.levels[world.i_currentLvl].items[targTag].name;
                sb.DrawString(UI.pump_font, name, world.levels[world.i_currentLvl].items[targTag].B_base.v_position - camera.v_pos - new Vector2(20,60), Color.Red);
            }

        }
        #endregion
        public static character collided_projectile(character c,int charTag,Projectile p)
        {
            //character output = c;
            if(Vector2.Distance(c.B_baseProps.v_position+new Vector2(0,-100),p.B_base.v_position)<190)
            if (c.attrib.hp_current > 0 || c.currentAnim.f_frame.Length/2 >c.f_currentFrame)
            {

                if(p.owner < world.levels[world.i_currentLvl].char_living.Length)
                if (p.owner != charTag && world.levels[world.i_currentLvl].char_living[p.owner].Type != c.Type)
                {
                    Vector2 pos = p.B_base.v_position;
                    Vector2 velocity = p.B_base.v_velocity;

                    for (int i = 0; i != c.body.Length; i++)
                    {
                        Vector2 v_temp = (c.B_baseProps.v_position - new Vector2(0, c.B_baseProps.f_Position_Z)) + c.body[i].position - (c.body[i].pivot);
                        Vector2 dims = new Vector2(c.body[i].texture[0].Width, c.body[i].texture[0].Height);
                        //float dist = (float)Math.Sqrt((dims.X * dims.X) + (dims.Y * dims.Y));
                        //if (Vector2.Distance(v_temp + c.body[i].position, pos) <= dist/8)
                        {
                            for (int j = 0; j != c.body[i].v_collison_map.Length; j++)
                            {
                                if (Vector2.Distance(v_temp + c.body[i].v_collison_map[j], pos) < gfunc.i_collision_accuracy*4)
                                {
                                    #region ManagesDamageLevels

                                    int damageMult = 1;
                                    if (c.body[i].name == "HEAD")
                                    {
                                        damageMult = 100;
                                        if (world.levels[world.i_currentLvl].char_living[p.owner].controlType == ControlType.player_current &&
                                            c.special_tag != 1337)
                                        {
                                            PlayHeadshot();
                                            c.special_tag = 1337;
                                        }

                                        {
                                            world.levels[world.i_currentLvl].char_living[p.owner].attrib.score += 2000;
                                        }

                                    }

                                    c.body[i].currentHP -= p.damage;
                                    if (c.attrib.hp_current > 0)
                                    {
                                        c.attrib.hp_current -= p.damage * damageMult;
                                    }

                                    if (c.body[i].currentHP < 0)
                                    {
                                        
                                        c.body[i].currentHP = 0;
                                        c.action = "dead";
                                        c = dropInventory(c);
                                        if (c.Type == CharType.zombie && c.action != "dead")
                                        {
                                            PlayZombDie();
                                        }
                                    }

                                    int NumDmgLvls = c.body[i].texture.Length;
                                    int oldDmgLvl = c.body[i].damageLevel;
                                    int DmgLvl = gfunc.roundUp((NumDmgLvls - 1) - ((c.body[i].currentHP + 1) / (c.body[i].maxHP / (NumDmgLvls - 1))));
                                    if (oldDmgLvl != DmgLvl)
                                    {
                                        if(gfunc.roundUp(gfunc.RandomNumber(0,4))==1)
                                        createGibs(ref c, ref p);
                                    }
                                    c.body[i].damageLevel = DmgLvl;
                                    #endregion

                                    particle temp;
                                    if (c.attrib.hp_current > 0 || world.levels[world.i_currentLvl].char_living.Length<5)
                                    {
                                        PlayFleshImpact();
                                        if (c.Type == CharType.zombie)
                                        {
                                            PlayZombHit();
                                        }
                                        #region CreateBloodSpray
                                        temp = particles.blood_spray[gfunc.roundUp(gfunc.RandomNumber(0, 4))];
                                        temp.Velocity = (new Vector3(velocity.X / 4, 0, velocity.Y / 4) + new Vector3(gfunc.randomPos(-Vector2.One * 10, Vector2.One * 10), 0)) / 2;
                                        temp.position = new Vector3(pos.X, c.B_baseProps.v_position.Y, pos.Y - c.B_baseProps.v_position.Y);
                                        temp.angular_velo = gfunc.RandomNumber(-.25f, .25f);
                                        PEngine.particle_create(temp);
                                        #endregion
                                        #region CreateBloodSpurt
                                    temp = particles.blood_spurt[gfunc.roundUp(gfunc.RandomNumber(0, 3))];
                                    temp.position = new Vector3(pos.X, c.B_baseProps.v_position.Y, pos.Y - c.B_baseProps.v_position.Y);
                                    PEngine.particle_create(temp);
                                    //temp. 
                                    #endregion
                                    }
                                    if (c.attrib.hp_current < 0)
                                    {
                                        if (c.controlType == ControlType.player_current)
                                        { thisPlayerLiving = false; }
                                        world.levels[world.i_currentLvl].char_living[p.owner].attrib.kills++;
                                        //if (world.levels[world.i_currentLvl].char_living[p.owner].controlType == ControlType.player_current)
                                        {
                                            world.levels[world.i_currentLvl].char_living[p.owner].attrib.score += 1000;
                                        }
                                        c.attrib.hp_current = 0;
                                        c = dropInventory(c);
                                        if (c.body[findHead(c)].damageLevel < 4)
                                        {
                                            c = Zombify(c);
                                        }
                                        else
                                        {
                                            c.brain.active = false;
                                        }
                                    }
                                    //output = true;

                                }
                            }
                        }
                    }
                }
            }
            return c;
        }
        public static int findHead(character c)
        {
            int output = -1;
            for (int i = 0; i != c.body.Length; i++)
            {
                if (c.body[i].name == "HEAD")
                {
                    output = i;
                }
            }
            return output;
        }
        private static void createGibs(ref character c, ref Projectile p)
        {
            int gibTag = gfunc.roundUp(gfunc.RandomNumber(0, particles.gib.Length - 1));
            effects.particle p_temp = effects.particles.gib[gibTag];

            int force = 10;
            p_temp.Velocity = new Vector3(gfunc.randomPos(-new Vector2(1, 1) * force, new Vector2(1, 1) * force),
                                          gfunc.RandomNumber(-force, 1));
            p_temp.friction = .95f;
            p_temp.angular_velo = gfunc.RandomNumber(-.1, .1);
            p_temp.position = new Vector3(p.B_base.v_position.X,
                                          c.B_baseProps.v_position.Y,
                                          p.B_base.v_position.Y - (c.B_baseProps.v_position.Y + (float)gfunc.RandomNumber(-2, 2)));
            p_temp.angular_velo = gfunc.RandomNumber(-.5, .5);
            effects.PEngine.particle_create(p_temp);
        }
        #endregion

        #region Drawing
        public static void drawAll(SpriteBatch sb)
        {
            float[] ys = new float[world.levels[world.i_currentLvl].char_living.Length];
            for(int i = 0; i!=world.levels[world.i_currentLvl].char_living.Length;i++)
            {
                ys[i] = world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position.Y;
            }

            int[] tags = gfunc.sortBasedOnY(ys);

            sb.Begin();//SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            for(int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                world.levels[world.i_currentLvl].char_living[tags[i]] = draw(sb, world.levels[world.i_currentLvl].char_living[tags[i]], world.levels[world.i_currentLvl].char_living[tags[i]].pointingRight);
                if (world.levels[world.i_currentLvl].char_living[tags[i]].controlType == ControlType.player_current)
                {
                    drawItemName(sb, world.levels[world.i_currentLvl].char_living[tags[i]]);
                    scoreFunc.PlayerScore = world.levels[world.i_currentLvl].char_living[tags[i]].attrib.score;
                }
                //string S_debug = "AI Package: " + world.levels[world.i_currentLvl].char_living[tags[i]].brain.aip_current.ToString() + "\n" +
                 //              "Health: " + world.levels[world.i_currentLvl].char_living[tags[i]].attrib.hp_current.ToString() + "\n" +
                 //              "Name: " + world.levels[world.i_currentLvl].char_living[tags[i]].name.ToString() + "\n" +
                 //              "Type: " + world.levels[world.i_currentLvl].char_living[tags[i]].Type.ToString() + "\n" +
                 //              "Health: " + world.levels[world.i_currentLvl].char_living[tags[i]].attrib.hp_current.ToString() + "\n" +
                //               "Action: " + world.levels[world.i_currentLvl].char_living[tags[i]].action.ToString() + "\n";
                //sb.DrawString(Game1.debug, S_debug, world.levels[world.i_currentLvl].char_living[tags[i]].B_baseProps.v_position-camera.v_pos,Color.White);
            }

            sb.End();
        }
        public static character draw(SpriteBatch sb, character Character, bool pointingRight)
        {
            character reordered = Character;

            if(!Character.reordered)
               reordered = ReorderSprites(Character);

            sb.Draw(shadow,
                (reordered.B_baseProps.v_position+ new Vector2(0,20) - camera.v_pos),
                null,
                Color.White,
                0,//+(float)Math.PI,
                new Vector2(shadow.Width,shadow.Height)/2,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                camera.scale*2,
                SpriteEffects.FlipVertically,
                0);    

                for (int i = 0; i != reordered.body.Length; i++)
                {
                    Color blockCol = Color.White;
                    switch (Character.Type)
                    {
                        case CharType.survivor:
                            blockCol = Color.GhostWhite;
                            if (Character.controlType == ControlType.player_current)
                                blockCol = Color.White;
                            break;
                        case CharType.zombie:
                            blockCol = Color.Gray;
                            break;

                    }


                    if (!pointingRight)
                    {

                        int DmgLvl = reordered.body[i].damageLevel;
                        Vector2 tempPivot = reordered.body[i].pivot;
                        Vector2 tempPos = reordered.body[i].position;
                        tempPivot.X = reordered.body[i].texture[DmgLvl].Width- tempPivot.X;
                        tempPos.X = -tempPos.X;

                        if (reordered.body[i].name == "HAND_L" && reordered.current_weap.bolt_back != null)
                        {
                            Character.current_weap.angle = weapFunc.drawWeapon(sb, reordered, i, false);
                        }
                        if(Character.special_tag!=-1)
                        DrawItemInHand(sb, ref Character, ref reordered, i, ref blockCol);

                        sb.Draw(reordered.body[i].texture[DmgLvl],
                                            (reordered.B_baseProps.v_position - new Vector2(0, Character.B_baseProps.f_Position_Z) - camera.v_pos) + tempPos,
                                            null,
                                            blockCol,
                                            -reordered.body[i].angle,//+(float)Math.PI,
                                            tempPivot,//new Vector2(reordered.body[i].texture[DmgLvl].Width,reordered.body[i].texture[DmgLvl].Height) - reordered.body[i].pivot,
                                            camera.scale,
                                            SpriteEffects.FlipHorizontally,
                                            0);
                    }
                    else
                    {
                        int DmgLvl = reordered.body[i].damageLevel;

                        if (reordered.body[i].name == "HAND_L" && reordered.current_weap.bolt_back!=null)
                        {
                            Character.current_weap.angle = weapFunc.drawWeapon(sb, reordered, i, true);
                        }

                        if (Character.special_tag != -1)
                        DrawItemInHand(sb, ref Character, ref reordered, i, ref blockCol);

                        sb.Draw(reordered.body[i].texture[DmgLvl],
                                            (reordered.B_baseProps.v_position - new Vector2(0, Character.B_baseProps.f_Position_Z) - camera.v_pos) + reordered.body[i].position,
                                            null,
                                            blockCol,
                                            reordered.body[i].angle,
                                            reordered.body[i].pivot,
                                            camera.scale,
                                            SpriteEffects.None,
                                            0);
                       // Vector2 v_temp = (reordered.B_baseProps.v_position - new Vector2(0, Character.B_baseProps.f_Position_Z) - camera.v_pos) + reordered.body[i].position-(reordered.body[i].pivot);
                       // for (int j = 0; j != reordered.body[i].v_collison_map.Length; j++)
                       // {
                       //     sb.Draw(Tex_null, v_temp+reordered.body[i].v_collison_map[j], Color.Red);
                       // }
                    }

                }

                return Character;
        }
        private static void DrawItemInHand(SpriteBatch sb, ref character Character, ref character reordered, int i, ref Color blockCol)
        {
            if (reordered.body[i].name == "HAND_R" && reordered.action == "pickup")
            {
                int item = reordered.special_tag;
                if (world.levels[world.i_currentLvl].items.Length > item)
                {
                    Vector2 center = Vector2.Zero;
                    Vector2 pos = reordered.body[i].position;
                    if (Character.pointingRight)
                    {
                        //try
                        {
                            center = new Vector2(world.levels[world.i_currentLvl].items[item].texture.Width - 20, world.levels[world.i_currentLvl].items[item].texture.Height) / 2;
                            sb.Draw(world.levels[world.i_currentLvl].items[item].texture,
                                    (reordered.B_baseProps.v_position - new Vector2(0, Character.B_baseProps.f_Position_Z) - camera.v_pos) + pos,
                                    null,
                                    blockCol,
                                    reordered.body[i].angle,
                                    center,
                                    camera.scale,
                                    SpriteEffects.None,
                                    0);
                        }
                        //catch { }
                    }
                    else
                    {
                        //try
                        {
                            center = new Vector2(world.levels[world.i_currentLvl].items[item].texture.Width - 20, world.levels[world.i_currentLvl].items[item].texture.Height) / 2;
                            pos.X = -pos.X;
                            sb.Draw(world.levels[world.i_currentLvl].items[item].texture,
                                    (reordered.B_baseProps.v_position - new Vector2(0, Character.B_baseProps.f_Position_Z) - camera.v_pos) + pos,
                                    null,
                                    blockCol,
                                    reordered.body[i].angle + (float)Math.PI / 2,
                                    center,
                                    camera.scale,
                                    SpriteEffects.FlipHorizontally,
                                    0);
                        }
                        //catch { }
                    }
                }
            }
        }
        public static character ReorderSprites(character input)
        {
            character old = input;
            if (input.body != null)
            {
                #region Define_vars
                input.body = new block[old.body.Length];
                bool[] usedBlock = new bool[old.body.Length];
                for (int i = 0; i != usedBlock.Length; i++)
                {
                    usedBlock[i] = false;
                }
                //int nextNewBlock;
                #endregion

                for (int i = 0; i != input.body.Length; i++)
                {
                    #region FindNextBlock
                    int smallest = 99999, smallestID = -1;
                    for (int j = 0; j != old.body.Length; j++)
                    {
                        if (usedBlock[j] == false && old.body[j].zorder < smallest)
                        {
                            smallest = old.body[j].zorder;
                            smallestID = j;

                        }
                    }
                    usedBlock[smallestID] = true;
                    #endregion
                    //old.body[smallestID].position
                    input.body[i] = old.body[smallestID];
                }
            }
            input.Primary_weap = old.Primary_weap;
            input.Secondary_weap = old.Secondary_weap;
            input.SideArm_weap = old.SideArm_weap;
            input.reordered = true;
            return input;
        } 
        #endregion

        #region LimbTranslation
        public static character translate(character current, int j, float changeInAng, Vector2 newVec)
        {
            current.body[j].angle += changeInAng;
            current.body[j].position += newVec;
            //try
            //{
                for (int i = 0; i != current.body.Length; i++)
                {
                    if (i != j)
                    {
                        float distance = Vector2.Distance(current.body[i].position, current.body[j].position);
                        if (IsChild(i, j, current))
                        {
                            current.body[i].position = OrbitPivot(current.body[j].position, current.body[i].position, changeInAng);
                            current.body[i].angle += changeInAng;
                            current.body[i].position += newVec;
                        }
                    }
                }
            //}
            //catch { }
            return current;
        }
        public static character translate(character current, int j, float setToAng)
        {
            float old_angle = current.body[j].angle;
            current.body[j].angle = setToAng;
            //current.body[j].position += newVec;
            //try
            //{
                if (old_angle != setToAng)
                {
                    for (int i = 0; i != current.body.Length; i++)
                    {
                        if (i != j)
                        {
                            float distance = Vector2.Distance(current.body[i].position, current.body[j].position);
                            if (IsChild(i, j, current))
                            {
                                current.body[i].position = OrbitPivot(current.body[j].position, current.body[i].position, setToAng, old_angle);
                                current.body[i].angle += (float)(setToAng - old_angle);
                                //current.body[i].v_collison_map = gfunc.collision.RotateCollisionMap(current.body[i].v_collison_map,
                                                                                //current.body[i].pivot,
                                                                                //current.body[i].angle - old_angle);
                                //current.body[i].position += newVec;
                            }
                        }
                    }
                    old_angle = setToAng;
                }
            //}
            //catch { }
            return current;
        }
        public static Vector2 OrbitPivot(Vector2 pivot, Vector2 obj, float angle)
        {
            float dist = Vector2.Distance(pivot, obj);
            Vector2 angCalc = pivot - obj;
            float offset = gfunc.PointAt(pivot, obj);//Convert.ToSingle(Math.Atan2(angCalc.Y/angCalc.X)); 
            Vector2 output = Vector2.Zero;
            output.X = Convert.ToSingle(dist * Math.Cos(Convert.ToDouble(offset + angle)) + pivot.X);
            output.Y = Convert.ToSingle(dist * Math.Sin(Convert.ToDouble(offset + angle)) + pivot.Y);
            return output;
        }
        private static Vector2 OrbitPivot(Vector2 pivot, Vector2 obj, float angle, float angle_old)
        {
            float dist = Vector2.Distance(pivot, obj);
            Vector2 angCalc = pivot - obj;
            float offset = gfunc.PointAt(pivot, obj);//Convert.ToSingle(Math.Atan2(angCalc.Y/angCalc.X)); 
            Vector2 output = Vector2.Zero;
            output.X = Convert.ToSingle(dist * Math.Cos(offset + -(angle_old - angle)) + pivot.X);
            output.Y = Convert.ToSingle(dist * Math.Sin(offset + -(angle_old - angle)) + pivot.Y);
            return output;
        }
        private static bool IsChild(int posChild, int Parent, character current)
        {
            bool result = false;
            //try
            {
                for (int i = 0; i != current.body[Parent].children.Length; i++)
                {
                    if (current.body[Parent].children[i] == posChild)
                    {
                        result = true;
                    }
                }
            }
            //catch { }
            return result;
        } 
        #endregion
    }
}
