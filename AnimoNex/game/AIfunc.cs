using System;
using System.Collections.Generic;
using System.Text;
using AnimoNex.types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimoNex.game
{
    class AIfunc
    {
        public static int livingZombies=0;
        static int currentLevelLowLvl = 0;
        public static character updateNode(character input)
        {
            if (input.brain.aip_current != aiPackage.turnIntoZomb)
            {
                //input = pakageIdle(input);
                if (input.brain.aip_current != aiPackage.attack &&
                   input.brain.aip_current != aiPackage.pickupItem &&
                    input.brain.aip_current != aiPackage.flee)
                {
                    if(input.brain.aip_current!=aiPackage.follow)
                    input = packageWander(input);
                }
                if (input.brain.aip_current != aiPackage.attack &&
                    input.brain.aip_current != aiPackage.flee &&
                    input.brain.aip_current != aiPackage.follow)
                {
                    input = packagePickUpItem(input);
                }
                if (input.brain.aip_current != aiPackage.pickupItem &&
                   input.brain.aip_current != aiPackage.attack &&
                    input.brain.aip_current != aiPackage.follow)
                {
                    input = packageFlee(input);
                }
                if (input.brain.aip_current != aiPackage.attack)
                {
                    //input = packageFollow(input);
                }
                
                //if ()
                {
                    input = packageAttack(input);
                }
            }
            else
            {
                input = packageTurnIntoZomb(input);
            }
            //input = packageKeepDistance(input);
            return input;
        }
        private static character packageTurnIntoZomb(character input)
        {
            if (input.attrib.hp_current <= 0)
            {
                if (input.brain.aip_current == aiPackage.turnIntoZomb)
                {
                    if (timeWaited(input.brain, 750))
                    {
                        input.brain.aip_current = aiPackage.none;
                        input.attrib.hp_max = 1000;
                        input.attrib.hp_current = 1000;
                        input = CharFunc.setAnimation(input, "zomb_idle");
                        input.action = "idle";
                        input.Type = CharType.zombie;
                    }
                }
            }
            return input;
        }
        private static character packageIdle(character input)
        {
            if (input.brain.aip_current != aiPackage.idle || timeWaited(input.brain, (int)gfunc.RandomNumber(200, 500)))
            {
                input.brain.tick_last = Game1.time_ticks;

                input.B_baseProps.v_velocity.X = gfunc.RandomNumber(-0.1, 0.1);
            }
            return input;
        }
        private static character packageWander(character input)
        {
            if (input.brain.aip_current == aiPackage.idle || Vector2.Distance(input.B_baseProps.v_position, input.brain.destination) <= 10 ||
                timeWaited(input.brain,200))
            {
                input.brain.tick_last = Game1.time_ticks;
                input.brain.aip_current = aiPackage.wander;
                input.brain.destination = LvlFunc.randomPositon();//gfunc.randomPos(new Vector2(175, 400), new Vector2(1000, 750));
                input.current_weap = input.Primary_weap;
                input.f_currentFrame = 0;
                if (input.Type == CharType.zombie)
                    CharFunc.PlayZombMoan();
            }
            else
            {
                float angle = gfunc.PointAt(input.B_baseProps.v_position, input.brain.destination);
                
                    input.B_baseProps.v_velocity += gfunc.TranslateOnAng(5,angle);
            }
            return input;
        }
        private static character packageFollow(character input)
        {
            if (input.brain.aip_current != aiPackage.follow)// || timeWaited(input.brain, (int)gfunc.RandomNumber(200, 500)))
            {
                input.brain.tick_last = Game1.time_ticks;
                for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
                {
                    if (world.levels[world.i_currentLvl].char_living[i].Type == input.Type)
                    {
                        if (world.levels[world.i_currentLvl].char_living[i].attrib.kills > input.attrib.kills)
                        {
                            if (Vector2.Distance(world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position, input.B_baseProps.v_position)
                                <= input.brain.ais_senses.radius_sight)
                            {
                                    input.brain.current_package_tag = i.ToString();
                                    input.brain.aip_current = aiPackage.follow;

                            }
                        }
                    }
                }
            }
            else
            {
                    int tag = Convert.ToInt32(input.brain.current_package_tag);
                    if (tag > world.levels[world.i_currentLvl].char_living.Length && tag < world.levels[world.i_currentLvl].char_living.Length)
                    {
                        float distance = Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);

                        if (distance > 180)
                        {
                            float angle = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);
                            float velocity = distance * .03f;
                            input.B_baseProps.v_velocity += gfunc.TranslateOnAng(velocity, angle);
                        }
                        else
                        {
                            input.brain.current_package_tag = "";
                            input.brain.aip_current = aiPackage.idle;
                        }
                    }
                    else
                    {
                        input.brain.aip_current = aiPackage.idle;
                    }
            }
            return input;
        }
        private static character packageAttack(character input)
        {
            #region ZombieAttack
            if (input.Type == CharType.zombie)
            {
                if (input.brain.aip_current != aiPackage.attack)
                {
                    if (timeWaited(input.brain, 65))
                    {
                        input.brain.tick_last = Game1.time_ticks;
                        for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].Type != input.Type)
                            {
                                if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
                                {
                                    if (Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position) <
                                       input.brain.ais_senses.radius_hearing)
                                    {
                                        if (world.levels[world.i_currentLvl].char_living[i].attrib.stl_current < gfunc.RandomNumber(.75, .90))
                                        {
                                            //if (collidingWithNPC(input,40) == -1)
                                            //{
                                            CharFunc.PlayZombMoan();
                                            input.brain.current_package_tag = world.levels[world.i_currentLvl].char_living[i].name;
                                            input.brain.aip_current = aiPackage.attack;
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int tag = findNPC(input.brain.current_package_tag);  //tag is target
                    if (tag != -1)
                    {
                        float distance = Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);
                        if (distance > 100)
                        {
                            float angle = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);
                            float velocity = distance * .03f;
                            tag = collidingWithNPC(input, 40);
                            input.B_baseProps.v_velocity = gfunc.TranslateOnAng(velocity, angle);
                            if (tag == -1)
                            {
                                input.brain.secondary_package_tag = "none";
                            }
                            else
                            {
                                //input.B_baseProps.v_velocity=Vector2.Zero;
                                //input.B_baseProps.v_velocity.Y *= -3;
                                input.B_baseProps.v_velocity /= 3;

                                //input.B_baseProps.v_velocity += gfunc.randomPos(input.B_baseProps.v_velocity, input.B_baseProps.v_velocity * 2);
                                input.brain.secondary_package_tag = "slowed";
                            }
                        }
                        else
                        {
                            input.action = "attack";
                        }
                    }
                    else
                    {
                        input = packageAttackKill(input);
                    }
                }
            }
            #endregion
            if (input.Type != CharType.zombie)
            {
                if (input.brain.aip_current != aiPackage.attack)
                {
                    if (findWeapon(input) != -1)
                    {
                        for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
                        {
                            CharType target = findLargestThreat(input);
                            int tag = findClosestNPC(input, target);
                            if (tag != -1)
                            {
                                input.brain.aip_current = aiPackage.attack;
                                input.brain.current_package_tag = tag.ToString();
                            }
                        }
                    }
                }
                else
                {
                    input.current_weap.shooting = false;
                    if (input.action != "reload")
                    {
                            int tag = Convert.ToInt32(input.brain.current_package_tag);
                            if (tag < world.levels[world.i_currentLvl].char_living.Length)
                            {
                                float targetDist = Vector2.Distance(world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position, input.B_baseProps.v_position);
                                if (targetDist < input.brain.ais_senses.radius_sight)
                                {
                                    if (world.levels[world.i_currentLvl].char_living[tag].attrib.hp_current > 0)
                                    {
                                        #region IfUnarmedEquipWeapon
                                        if (input.current_weap.name == "unarmed" || input.current_weap.name == null) //if no weapon is equiped
                                        {
                                            int weapTag = findWeapon(input); //find weapon that has ammo

                                            if (weapTag != -1)
                                            {
                                                input.current_weap = weapFunc.FindWeapon(input.Inventory.items[weapTag].gameTag);
                                                input.action = "reload";
                                            }
                                            else
                                            {
                                                input = packageAttackKill(input);
                                            }
                                        }
                                        #endregion
                                        if (input.current_weap.clipCurrent > 0) //if the clipisnt empty
                                        {

                                            #region Aim
                                            float desiredAngle = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);

                                            if (input.B_baseProps.v_position.X > world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position.X)
                                            {
                                                input.pointingRight = false;
                                                input.B_baseProps.v_velocity += gfunc.TranslateOnAng(0.001f, desiredAngle);
                                            }
                                            else
                                            {
                                                input.pointingRight = true;
                                                input.B_baseProps.v_velocity += gfunc.TranslateOnAng(0.001f, desiredAngle);
                                            }

                                            if (!input.pointingRight)
                                            {
                                                desiredAngle = -desiredAngle + (float)Math.PI;
                                            }
                                            input.angle = desiredAngle;


                                            #endregion

                                            #region Shoot
                                            if (Math.Abs(input.angle - desiredAngle) < .25f)
                                            {
                                                input = CharFunc.characterFireWeapon(input);
                                            }
                                            #endregion

                                        }
                                        else
                                        {
                                            int weapTag = findWeaponAndAmmo(input);
                                            if (weapTag != -1)
                                            {
                                                input.current_weap = weapFunc.FindWeapon(input.Inventory.items[weapTag].name);
                                                input.action = "reload";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        input = packageAttackKill(input);
                                    }
                                }
                                else
                                {
                                    float desiredAngle = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);
                                    float velocity = 5;
                                    input.B_baseProps.v_velocity += gfunc.TranslateOnAng(velocity, desiredAngle);

                                    if (!input.pointingRight)
                                    {
                                        desiredAngle = -desiredAngle + (float)Math.PI;
                                    }

                                    input.angle = desiredAngle;
                                }
                            }
                            else
                            {
                                input = packageAttackKill(input);
                            }
                    }

                }
            }
            return input;
        }
        public static character packageAttackKill(character input)
        {
            input.action = "idle";
            input.angle = 0;
            input.brain.aip_current = aiPackage.idle;
            input.brain.current_package_tag = "";
            input.brain.secondary_package_tag = "";
            return input;
        }
        public static character packagePickupKill(character input)
        {
            input.brain.destination = Vector2.Zero;
            input.brain.current_package_tag = "";
            input.brain.aip_current = aiPackage.idle;

            return input;
        }
        private static character packageKeepDistance(character input)
        {
            if (input.brain.aip_current == aiPackage.keepDistance)
            {
                if(timeWaited(input.brain,100))
                {
                    input.brain.aip_current = aiPackage.none;
                    input.brain.current_package_tag = "-1";
                }
                float angle = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[Convert.ToInt32(input.brain.current_package_tag)].B_baseProps.v_position);
                input.B_baseProps.v_velocity = gfunc.TranslateOnAng(4,angle + (float)Math.PI );
            }
            return input;
        }
        private static character packagePickUpItem(character input)
        {
            if (input.Type != CharType.zombie)
            {
                if (world.levels[world.i_currentLvl].items != null && world.levels[world.i_currentLvl].items.Length != 0)
                {
                    if (input.brain.aip_current != aiPackage.pickupItem)
                    {
                        //int tag = findClosestItem(input);

                        int tag = -1;
                        int tempammo = findClosestAmmo(input);
                        if (findWeapon(input) == -1)
                        {
                            tag = findClosestGun(input);
                        }
                        else if ((tempammo != -1))
                        {
                            tag = tempammo;
                        }
                        if (tag != -1)
                        {
                            tag = findClosestItem(input);
                        }

                        if (tag != -1 && world.levels[world.i_currentLvl].items.Length != 0 &&
                            world.levels[world.i_currentLvl].items[tag].weight < (input.Inventory.maxWeight - input.Inventory.CurWeight))
                        {
                            if (world.levels[world.i_currentLvl].items[tag].type != itemType.junk)
                            {
                                input.brain.aip_current = aiPackage.pickupItem;
                                input.brain.current_package_tag = tag.ToString();
                                input.brain.destination = world.levels[world.i_currentLvl].items[tag].B_base.v_position;
                            }
                        }

                    }
                    else
                    {
                        float distance = Vector2.Distance(input.B_baseProps.v_position, input.brain.destination);
                        if (distance > 5)
                        {
                            //int tag = Convert.ToInt32(input.brain.current_package_tag);
                            float angle = gfunc.PointAt(input.B_baseProps.v_position, input.brain.destination);
                            float velocity = 5f;
                            input.B_baseProps.v_velocity += gfunc.TranslateOnAng(velocity, angle);
                        }
                        else
                        {
                            input.action = "pickup";
                        }
                    }

                }
            }
            return input;
        }
        private static character packageFlee(character input)
        {
            if (input.brain.aip_current != aiPackage.flee)
            {
                if (input.Type != CharType.zombie)
                {
                    for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
                    {
                        if (input.Type != world.levels[world.i_currentLvl].char_living[i].Type && world.levels[world.i_currentLvl].char_living[i].attrib.hp_current >0)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].current_weap.name != "unarmed" &&
                                world.levels[world.i_currentLvl].char_living[i].current_weap.name != null ||
                                world.levels[world.i_currentLvl].char_living[i].Type == CharType.zombie)
                            {
                                if (Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position) < input.brain.ais_senses.radius_hearing)
                                {
                                    input.brain.aip_current = aiPackage.flee;
                                    CharFunc.PlayCharFlee();
                                    float ang = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position);
                                    float velocity = 10;
                                    input.brain.destination = gfunc.TranslateOnAng(velocity, ang + (float)Math.PI);
                                    input.brain.current_package_tag = i.ToString();
                                    input.brain.tick_last = Game1.time_ticks;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //int tag = Convert.ToInt32(input.brain.current_package_tag);
                float dist = Vector2.Distance(input.B_baseProps.v_position, input.brain.destination);
                if (!timeWaited(input.brain, 50))
                {
                    //float angle = gfunc.PointAt(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[tag].B_baseProps.v_position);
                    input.B_baseProps.v_velocity += input.brain.destination;//gfunc.TranslateOnAng(velocity, angle + (float)Math.PI);
                }
                else
                {
                    input.brain.aip_current = aiPackage.idle;
                    input.brain.current_package_tag = "";
                }
            }
            return input;
        }
        public static void doLowlevelProcessing()
        {
            if (currentLevelLowLvl < world.levels.Length) //be sure that the current level is not out of bounds
            {
                int i = currentLevelLowLvl;

                if (world.levels[i].char_living != null && world.levels[i].char_living.Length != 0 &&
                    i!=world.i_currentLvl) //be sure that there are characters to operate on
                {
                    PickUpItems(i);
                    SpreadInfection(i);
                    MoveAbout(i);
                }
                currentLevelLowLvl++;
            }
            else { currentLevelLowLvl = 0; }

            livingZombies = 0;
            for(int i =0;i!=world.levels.Length;i++)
            {
                if (world.levels[i].name != "mainmenu")
                {
                     livingZombies += LvlFunc.getZombieCount(i);
                }
            }
        }

        private static void SpreadInfection(int i)
        {
            int c = LvlFunc.findNPC(i, true);
            float z = LvlFunc.getZombieCount(i);
            if (c != -1 && z != 0)
            {
                if (findWeaponAndAmmo(world.levels[i].char_living[c]) == -1) //unarmed
                {
                    z /= 2;
                }
                if (gfunc.roundUp(gfunc.RandomNumber(0, 20 / z)) == 1)
                {
                    CharFunc.Zombify(c, i);
                }
            }
        }

        private static void PickUpItems(int i)
        {
            if (gfunc.roundUp(gfunc.RandomNumber(0, 4)) == 1) //have a 10 percent chance of traveling through another portal
            {
                int c = LvlFunc.findNPC(i,true);

                if (c != -1 && world.levels[i].items != null && world.levels[i].items.Length != 0)
                {
                    //if (world.levels[i].char_living[c].Type != CharType.zombie)
                    {
                        int itm = -1;

                        itm = LvlFunc.findItem(i);

                        if (world.levels[i].items[itm].weight + world.levels[i].char_living[c].Inventory.CurWeight < world.levels[i].char_living[c].Inventory.maxWeight)
                        {
                            world.levels[i].char_living[c].Inventory = itemFunc.createItem_INVEN(world.levels[i].char_living[c].Inventory,
                                                                    world.levels[i].items[itm],
                                                                    Vector2.Zero, 0);
                            itemFunc.killItem(itm, i);
                        }
                    }
                }
            }
        }

        private static void MoveAbout(int i)
        {

            if (gfunc.roundUp(gfunc.RandomNumber(0, 1)) == 1) //have a 10 percent chance of traveling through another portal
            {
                int c = LvlFunc.findNPC(i,false);
                int p = LvlFunc.findRandomPortal(i);
                if (c != -1 && p != -1)
                {
                    if (world.levels[i].char_living[c].brain.aip_current != aiPackage.turnIntoZomb)
                        LvlFunc.CharacterUseDoor_LowLvl(i, world.levels[i].char_living[c].name, p);
                }
            }
        }
        #region HelperMethods
        private static CharType findLargestThreat(character input)
        {
            CharType output = CharType.zombie;
            int zombies = 0, bandits = 0, soldiers = 0, survivors = 0;

            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
                {
                    if(findWeaponAndAmmo(world.levels[world.i_currentLvl].char_living[i]) !=-1 || world.levels[world.i_currentLvl].char_living[i].Type == CharType.zombie)
                    if (world.levels[world.i_currentLvl].char_living[i].Type != input.Type)
                    {
                        if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.zombie)
                            zombies++;
                        if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.survivor)
                            survivors++;
                        if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.soldier)
                            soldiers++;
                        if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.bandit)
                            bandits++;
                    }
                }
            }
            int highest = 0;

            #region FindMost
            if (zombies > highest)
            {
                highest = zombies;
                output = CharType.zombie;
            }
            if (bandits > highest)
            {
                highest = bandits;
                output = CharType.bandit;
            }
            if (soldiers > highest)
            {
                highest = soldiers;
                output = CharType.soldier;
            }
            if (survivors > highest)
            {
                highest = survivors;
                output = CharType.survivor;
            }
            #endregion

            return output;
        }
        private static CharType findSmallestThreat(character input)
        {
            CharType output = CharType.zombie;
            int zombies = 0, bandits = 0, soldiers = 0, survivors = 0;

            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
                {
                    if (findWeaponAndAmmo(world.levels[world.i_currentLvl].char_living[i]) != -1 || world.levels[world.i_currentLvl].char_living[i].Type == CharType.zombie)
                        if (world.levels[world.i_currentLvl].char_living[i].Type != input.Type)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.zombie)
                                zombies++;
                            if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.survivor)
                                survivors++;
                            if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.soldier)
                                soldiers++;
                            if (world.levels[world.i_currentLvl].char_living[i].Type == CharType.bandit)
                                bandits++;
                        }
                }
            }
            int smallest = 1000;

            #region FindMost
            if (zombies < smallest)
            {
                smallest = zombies;
                output = CharType.zombie;
            }
            if (bandits < smallest)
            {
                smallest = bandits;
                output = CharType.bandit;
            }
            if (soldiers < smallest)
            {
                smallest = soldiers;
                output = CharType.soldier;
            }
            if (survivors > smallest)
            {
                smallest = survivors;
                output = CharType.survivor;
            }
            #endregion

            return output;
        }
        public static int collidingWithNPC(character input, float dist)
        {
            int output = -1;
            //float output = 0;

            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (input.name != world.levels[world.i_currentLvl].char_living[i].name)
                {
                    if (Vector2.Distance(input.B_baseProps.v_position + input.B_baseProps.v_velocity,
                                        world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position + world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_velocity) < dist)
                    {
                        //if (world.levels[world.i_currentLvl].char_living[i].brain.aip_current == input.brain.aip_current)
                        {
                            if (world.levels[world.i_currentLvl].char_living[i].brain.secondary_package_tag != "slowed")
                            {
                                output = i;
                            }
                        }
                    }
                }
            }

            return output;
        }
        private static bool timeWaited(AInode brain, int time)
        {
            bool output = false;

            if ((Game1.time_ticks - brain.tick_last) >= time)
            {
                output = true;
            }
            return output;
        }
        private static int findClosestItem(character input)
        {
            int tag = -1;
            float currentLowest = 9999f;
            for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
            {
                float dist = Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].items[i].B_base.v_position);
                if (dist < input.brain.ais_senses.radius_sight)
                {
                    if (dist < currentLowest)
                    {
                        tag = i;
                        currentLowest = dist;
                    }
                }
            }
            return tag;
        }
        private static int findClosestGun(character input)
        {
            int tag = -1;
            float currentLowest = 9999f;
            for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
            {
                float dist = Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].items[i].B_base.v_position);
                if (dist < input.brain.ais_senses.radius_sight)
                {
                    if (dist < currentLowest && world.levels[world.i_currentLvl].items[i].type == itemType.weapon)
                    {
                        tag = i;
                        currentLowest = dist;
                    }
                }
            }
            return tag;
        }
        private static int findClosestAmmo(character input)
        {
            int tag = -1;
            float currentLowest = 9999f;
            for (int i = 0; i != world.levels[world.i_currentLvl].items.Length; i++)
            {
                float dist = Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].items[i].B_base.v_position);
                if (dist < input.brain.ais_senses.radius_sight)
                {
                    if (dist < currentLowest && world.levels[world.i_currentLvl].items[i].type == itemType.ammo)
                    {
                        tag = i;
                        currentLowest = dist;
                    }
                }
            }
            return tag;
        }
        private static int findClosestNPC(character input, CharType type)
        {
            int tag = -1;
            float currentLowest = 9999f;
            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].Type == type && world.levels[world.i_currentLvl].char_living[i].attrib.hp_current > 0)
                {
                    float dist = Vector2.Distance(input.B_baseProps.v_position, world.levels[world.i_currentLvl].char_living[i].B_baseProps.v_position);
                    if (dist < input.brain.ais_senses.radius_sight)
                    {
                        if (dist < currentLowest)
                        {
                            tag = i;
                            currentLowest = dist;
                        }
                    }
                }
            }
            return tag;
        }
        public static int findNPC(string name)
        {
            int tag = -1;
            for (int i = 0; i != world.levels[world.i_currentLvl].char_living.Length; i++)
            {
                if (world.levels[world.i_currentLvl].char_living[i].name == name)
                {
                    tag = i;
                }
            }
            return tag;
        }
        private static int findWeapon(character input)
        {
            int output = -1;
            if (input.Inventory.items != null && input.Inventory.items.Length != 0)
            {
                int weap_tag = -1;
                float weap_quality = 0;
                #region FindWeapon
                for (int i = 0; i != input.Inventory.items.Length; i++)
                {
                    if (input.Inventory.items[i].type == itemType.weapon)
                    {
                        if (input.Inventory.items[i].quality > weap_quality)
                        {
                            weapon weap_possible = weapFunc.FindWeapon(input.Inventory.items[i].name);
                            weap_tag = i;
                            weap_quality = input.Inventory.items[i].quality;
                        }
                    }


                }
                #endregion

                output = weap_tag;
            }
            return output;
        }
        private static int findWeaponAndAmmo(character input)
        {
            int output = -1;
            if (input.Inventory.items != null && input.Inventory.items.Length != 0)
            {
                int weap_tag = -1;
                float weap_quality = 0;
                #region FindWeaponWithAmmo
                for (int i = 0; i != input.Inventory.items.Length; i++)
                {
                    if (input.Inventory.items[i].type == itemType.weapon)
                    {
                        if (input.Inventory.items[i].quality > weap_quality)
                        {
                            weapon weap_possible = weapFunc.FindWeapon(input.Inventory.items[i].name);
                            int ammoTag = weapFunc.FindAmmoType(input, weap_possible);
                            if (ammoTag != -1)
                            {
                                weap_tag = i;
                                weap_quality = input.Inventory.items[i].quality;
                            }
                        }
                    }
                }
                #endregion

                output = weap_tag;
            }
            return output;
        } 
        #endregion
    }
}
