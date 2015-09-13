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

namespace AnimoNex.game.pda
{
    class UI
    {
        #region PDA stuff
        #region PDAdispVars
        public static pda pda_player;
        public static bool show = false;
        public static bool translationDone = false;
        public static bool itemTranslationDone = false;
        static float f_deltaV = 80;
        static float scale = 1;
        static Vector2 pos;
        #endregion
        public static int player_tag = -1;
        public static float f_selectedItem = 0;
        static inventory inven;
        static float f_scroll_pos;
        static int selectedItem = 0, lastItem = -1;
        static int NextTag = 0;
        #endregion
        #region MessagePump
        public static SpriteFont pump_font;
        public static string[] queue;
        public static int time_displayed;
        public static float displayTime;
        #endregion
        //static item[] PDAitems;

        #region PDA methods
        public static bool showMenu = false;

        public static menu m_current;

        public static void initializePDA()
        {
            #region SetInitalPosAndScale
            if (Game1.v_resolution.Y < pda_player.tex_pda.Height)
            {
                scale = Game1.v_resolution.Y / pda_player.tex_pda.Height;
            }
            pos = Game1.v_resolution / 2;
            pos.Y += pda_player.tex_pda.Height;
            #endregion
            buildMainMenu();
        }
        public static void updatePDA()
        {
            if (translationDone && show)
            {
                if (m_current.name == "inventory" || m_current.name == "item")
                {
                    //if (m_current.items[0].text != "Empty")
                    {
                        try
                        {
                            if (world.levels[world.i_currentLvl].char_living[player_tag].Inventory.items.Length != inven.items.Length)
                            {
                                buildInventory(world.levels[world.i_currentLvl].char_living[player_tag].Inventory);
                            }
                        }
                        catch { }
                    }
                }
                #region HandelSelections

                selectedItem = gfunc.roundUp(f_selectedItem);
                if (selectedItem >= m_current.items.Length)
                {
                    selectedItem = m_current.items.Length - 1;
                    f_selectedItem = selectedItem;
                }
                if (selectedItem < 0)
                {
                    selectedItem = 0;
                    f_selectedItem = selectedItem;
                }

                if (m_current.items[selectedItem].pos.Y >= 225 ||
                    m_current.items[selectedItem].pos.Y <= -225)
                {
                    if (m_current.items[selectedItem].pos.Y + f_scroll_pos >= 225)
                    {
                        f_scroll_pos = 225 - m_current.items[selectedItem].pos.Y;
                    }
                    if (m_current.items[selectedItem].pos.Y + f_scroll_pos <= -225)
                    {
                        f_scroll_pos = -225 - m_current.items[selectedItem].pos.Y;
                    }
                }
                #endregion
                #region HandleMenuSwitches

                for (int i = 0; i != m_current.items.Length && itemTranslationDone; i++)
                {
                    try
                    {
                        if (itemTranslationDone && m_current.items[i].selected && m_current.items[i].name == "btn_equip")
                        {

                            {

                                switch (m_current.items[0].type)
                                {
                                    case itemType.weapon:
                                        switch(weapFunc.FindWeapon(m_current.items[0].name).Pt)
                                        {
                                            case ProjectileType.AR223:
                                            case ProjectileType.AR762:
                                            case ProjectileType.CAR22:
                                            case ProjectileType.SG_pellet:
                                            case ProjectileType.SR308:
                                                CharFunc.assignWeapon(weapFunc.FindWeapon(m_current.items[0].name), player_tag, "primary");
                                            break;

                                            case ProjectileType.P357:
                                            case ProjectileType.P38:
                                            case ProjectileType.SMG45:
                                            case ProjectileType.SMG9:
                                            case ProjectileType.SR50:
                                                CharFunc.assignWeapon(weapFunc.FindWeapon(m_current.items[0].name), player_tag, "sidearm");
                                            break;

                                        }
                                        //if (Controller.(PlayerIndex.One))
                                        //{
                                        //    CharFunc.assignWeapon(weapFunc.FindWeapon(m_current.items[0].name), player_tag, "primary");
                                        //}
                                        //if (Controller.is_Dup(PlayerIndex.One))
                                        //{
                                        //    CharFunc.assignWeapon(weapFunc.FindWeapon(m_current.items[0].name), player_tag, "secondary");
                                        //}
                                        //if (Controller.is_Dright(PlayerIndex.One))
                                        //{
                                        //    CharFunc.assignWeapon(weapFunc.FindWeapon(m_current.items[0].name), player_tag, "sidearm");
                                        //}
                                        break;

                                    case itemType.food:
                                        CharFunc.equipFood(player_tag, inven.items[m_current.items[0].myTag]);
                                        world.levels[world.i_currentLvl].char_living[player_tag].Inventory.CurWeight -= world.levels[world.i_currentLvl].char_living[player_tag].Inventory.items[m_current.items[0].myTag].weight;
                                        world.levels[world.i_currentLvl].char_living[player_tag].Inventory.items = itemFunc.killItem_INVEN(world.levels[world.i_currentLvl].char_living[player_tag].Inventory.items, m_current.items[0].myTag);

                                        break;
                                }

                                buildMainMenu();
                                lastItem = -1;
                                itemTranslationDone = false;
                                showMenu = false;
                                show = false;
                                translationDone = false;
                            }
                            f_selectedItem = selectedItem = 0;


                        }
                    }
                    catch { i = m_current.items.Length - 1; }
                    try
                    {
                        if (itemTranslationDone && m_current.items[i].selected && m_current.items[i].name == "btn_drop")
                        {
                            if (world.levels[world.i_currentLvl].char_living[player_tag].Inventory.items[lastItem].type == itemType.weapon)
                            {
                                world.levels[world.i_currentLvl].char_living[player_tag].current_weap = new weapon();
                                world.levels[world.i_currentLvl].char_living[player_tag].Primary_weap = new weapon();
                                world.levels[world.i_currentLvl].char_living[player_tag].Secondary_weap = new weapon();
                            }
                            world.levels[world.i_currentLvl].char_living[player_tag].action = "drop";
                            world.levels[world.i_currentLvl].char_living[player_tag].special_tag = lastItem;
                            back();
                            //if (inven.items.Length <= 1)
                            {
                                buildMainMenu();
                                lastItem = -1;
                                itemTranslationDone = false;
                                showMenu = false;
                                show = false;
                                translationDone = false;
                            }
                            f_selectedItem = selectedItem = 0;


                        }
                    }
                    catch { i = m_current.items.Length - 1; }
                    try
                    {
                        if (itemTranslationDone && m_current.items[i].selected && m_current.items[i].name == "btn_examine")
                        {
                            buildExamineMenu();
                            f_selectedItem = selectedItem = 0;
                            m_current.items[i].selected = false;
                            itemTranslationDone = false;
                            showMenu = true;

                        }
                    }
                    catch { i = m_current.items.Length - 1; }

                    try
                    {
                        if (itemTranslationDone && m_current.items[i].selected && m_current.name == "inventory")
                        {
                            if (m_current.items[selectedItem].myTag != -1)
                            {
                                lastItem = m_current.items[selectedItem].myTag;
                                buildItemMenu(m_current.items[selectedItem].myTag);
                                f_selectedItem = selectedItem = 0;
                                m_current.items[i].selected = false;
                                itemTranslationDone = false;
                                showMenu = true;
                            }

                        }
                    }
                    catch { i = m_current.items.Length - 1; }

                    try
                    {
                        if (itemTranslationDone && m_current.items[i].selected && m_current.items[i].name == "btn_inven")
                        {
                            buildInventory(world.levels[world.i_currentLvl].char_living[player_tag].Inventory);
                            f_selectedItem = selectedItem = 0;
                            m_current.items[i].selected = false;
                            itemTranslationDone = false;
                            showMenu = true;

                        }
                    }
                    catch { i = m_current.items.Length - 1; }
                    try
                    {
                        if (itemTranslationDone && m_current.items[i].selected && m_current.items[i].name == "btn_status")
                        {
                            buildStatusMenu();
                            f_selectedItem = selectedItem = 0;
                            m_current.items[i].selected = false;
                            itemTranslationDone = false;
                            showMenu = true;

                        }
                    }
                    catch { i = m_current.items.Length - 1; }
                }
                #endregion
            }

            #region Translate
            if (show)
            {
                if (!translationDone)
                {
                    Vector2 temp = Game1.v_resolution / 2;
                    //pos -= temp;
                    float f_velocity = (Vector2.Distance(temp, pos) / (f_deltaV * 0.1f));

                    pos = gfunc.TranslateOnAng(pos, f_velocity, gfunc.PointAt(pos, temp));

                    if (Vector2.Distance(temp, pos) <= 1)
                    {
                        pos = temp;
                    }

                    if (pos == temp)
                    {
                        translationDone = true;
                    }
                    else
                    {
                        translationDone = false;
                    }
                }
            }
            else
            {
                if (!translationDone)
                {
                    Vector2 temp = (Game1.v_resolution / 2) + new Vector2(0, pda_player.tex_pda.Height);
                    //pos -= temp;
                    float f_velocity = (Vector2.Distance(temp, pos) / (f_deltaV * 0.1f));

                    pos = gfunc.TranslateOnAng(pos, f_velocity, gfunc.PointAt(pos, temp));

                    if (Vector2.Distance(temp, pos) <= 1)
                    {
                        pos = temp;
                    }

                    if (pos == temp)
                    {
                        translationDone = true;
                    }
                    else
                    {
                        translationDone = false;
                    }
                }
            }
            #endregion
            #region TranslateItems
            if (show && translationDone && showMenu)
            {
                for (int i = 0; i != m_current.items.Length; i++)
                {
                    Vector2 home = new Vector2(142, 225);
                    Vector2 temp = new Vector2(142, 225 - (i * 64));

                    float f_velocity = (Vector2.Distance(temp, m_current.items[i].pos) / (f_deltaV * 0.1f));

                    m_current.items[i].alpha = (byte)(255 / (Vector2.Distance(m_current.items[i].pos, temp) + 1));

                    m_current.items[i].pos = gfunc.TranslateOnAng(m_current.items[i].pos, f_velocity, gfunc.PointAt(m_current.items[i].pos, temp));

                    if (Vector2.Distance(temp, m_current.items[i].pos) <= 1)
                    {
                        m_current.items[i].pos = temp;
                        if (i == m_current.items.Length - 1)
                        {
                            itemTranslationDone = true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i != m_current.items.Length; i++)
                {
                    Vector2 home = new Vector2(142, 225 - (i * 64));
                    Vector2 temp = new Vector2(142, 225);

                    float f_velocity = (Vector2.Distance(temp, m_current.items[i].pos) / (f_deltaV * 0.1f));

                    m_current.items[i].alpha = (byte)((Vector2.Distance(m_current.items[i].pos, temp) + 1) / 1);

                    m_current.items[i].pos = gfunc.TranslateOnAng(m_current.items[i].pos, f_velocity, gfunc.PointAt(m_current.items[i].pos, temp));

                    if (Vector2.Distance(temp, m_current.items[i].pos) <= 1)
                    {
                        m_current.items[i].pos = temp;
                        if (i == m_current.items.Length - 1 && !showMenu)
                        {
                            itemTranslationDone = true;
                        }
                    }
                }
            }
            #endregion
        }
        public static void back()
        {
            if (itemTranslationDone)
            {
                switch (m_current.name)
                {
                    case "item":
                        buildInventory(world.levels[world.i_currentLvl].char_living[player_tag].Inventory);
                        lastItem = -1;
                        itemTranslationDone = false;
                        break;
                    case "status":
                        buildMainMenu();
                        itemTranslationDone = false;
                        break;
                    case "inventory":
                        buildMainMenu();
                        itemTranslationDone = false;
                        break;
                    case "examine":
                        buildItemMenu(lastItem);
                        itemTranslationDone = false;
                        break;
                    case "MainMenu":
                        show = false; ;
                        showMenu = false;
                        translationDone = false;
                        break;
                }

                //showMenu = false;
                showMenu = true;
            }
        }
        private static menu buildInventory(inventory inventory)
        {
            inven = inventory;
            menu old = m_current;
            m_current = new menu("inventory", null);
            try
            {
                for (int i = 0; i != inven.items.Length; i++)
                {
                    addItem(new menu_item(inven.items[i].icon,
                                          inven.items[i].name,
                                          inven.items[i].gameTag), i);
                    m_current.items[i].selected = false;
                }
            }
            catch
            {
                addItem(new menu_item(pda_player.tex_icon_inventory, "Empty", "none"), -1);
            }
            if (inven.items.Length == 0)
            {
                addItem(new menu_item(pda_player.tex_icon_inventory, "Empty", "none"), -1);
            }
            menu output = m_current;
            //m_current = old;
            showMenu = true;
            translationDone = false;
            show = true;
            return output;
        }
        private static menu buildItemMenu(int item)
        {
            menu old = m_current;
            m_current = new menu("item", null);

            addItem(new menu_item(inven.items[item].icon,
                                  inven.items[item].name,
                                  inven.items[item].gameTag), item);
            if (inven.items[item].type != itemType.junk)
            {
                addItem(new menu_item(pda_player.tex_icon_equip,
                                      "Equip",
                                      "btn_equip"), 0);
                m_current.items[0].type = inven.items[item].type;
            }
            addItem(new menu_item(pda_player.tex_icon_drop,
                      "Drop",
                      "btn_drop"), 0);
            addItem(new menu_item(pda_player.tex_icon_examine,
                      "Examine",
                      "btn_examine"), 0);

            menu output = m_current;
            //m_current = old;
            showMenu = true;
            translationDone = false;
            show = true;
            return output;
        }
        private static void buildStatusMenu()
        {
            m_current = new menu("status", null);
            addItem(new menu_item(pda_player.tex_icon_status,
                                  world.levels[world.i_currentLvl].char_living[player_tag].name + "'s status",
                                  "title"), 0);
            addItem(new menu_item(pda_player.tex_icon_status,
                      "Health: " + ((int)world.levels[world.i_currentLvl].char_living[player_tag].attrib.hp_current).ToString() + "%",
                      "title"), 0);
            addItem(new menu_item(pda_player.tex_icon_status,
          "Hunger: " + ((int)(world.levels[world.i_currentLvl].char_living[player_tag].attrib.hun_current * 100)).ToString() + "%",
          "title"), 0);
            //addItem(new menu_item(pda_player.tex_icon_status,
          //"Mechanical skill: " + ((int)world.levels[world.i_currentLvl].char_living[player_tag].attrib.mech_max).ToString(),
          //"title"), 0);
            //addItem(new menu_item(pda_player.tex_icon_status,
            //"Medical skill: " + ((int)world.levels[world.i_currentLvl].char_living[player_tag].attrib.med_max).ToString(),
            //"title"), 0);
            addItem(new menu_item(pda_player.tex_icon_status,
            "Stealth skill: " + ((int)world.levels[world.i_currentLvl].char_living[player_tag].attrib.stl_max).ToString(),
            "title"), 0);
            addItem(new menu_item(pda_player.tex_icon_status,
            "Strength: " + ((int)world.levels[world.i_currentLvl].char_living[player_tag].attrib.str_max).ToString(),
            "title"), 0);
            addItem(new menu_item(pda_player.tex_icon_status,
            "Stamina: " + ((int)world.levels[world.i_currentLvl].char_living[player_tag].attrib.sta_current).ToString(),
            "title"), 0);

            addItem(new menu_item(pda_player.tex_icon_status,
          "Encumbrance: " + (world.levels[world.i_currentLvl].char_living[player_tag].Inventory.CurWeight).ToString() + "/" + ((int)world.levels[world.i_currentLvl].char_living[player_tag].Inventory.maxWeight).ToString(),
          "title"), 0);
            addItem(new menu_item(pda_player.tex_icon_status,
          "Kills: " + (world.levels[world.i_currentLvl].char_living[player_tag].attrib.kills).ToString(),
          "title"), 0);

            showMenu = true;
            translationDone = false;
            show = true;

        }
        private static void buildExamineMenu()
        {
            int itemID = m_current.items[0].myTag;

            m_current = new menu("examine", null);

            addItem(new menu_item(inven.items[itemID].icon,
                      inven.items[itemID].name,
                      inven.items[itemID].gameTag), itemID);
            if (inven.items[itemID].type != itemType.ammo)
            {
                addItem(new menu_item(inven.items[itemID].icon,
                                      "Quality: " + inven.items[itemID].quality.ToString(),
                                      "btn_quality"), 0);
            }
            else
            {
                addItem(new menu_item(inven.items[itemID].icon,
                                      "Quanity: " + inven.items[itemID].quality.ToString(),
                                      "btn_quality"), 0);
            }
            //char[] seperators = { \n };
            //inven.items[itemID].description = Convert.ToString(inven.items[itemID].description.Split(seperators));
            addItem(new menu_item(inven.items[itemID].icon,
                      "Weight: " + inven.items[itemID].weight.ToString() + "Kg",
                      "btn_weight"), 0);
            addItem(new menu_item(inven.items[itemID].icon,
                                  inven.items[itemID].description,
                                  "btn_description"), 0);

        }
        private static menu buildMainMenu()
        {
            m_current = new menu("MainMenu", null);
            addItem(new menu_item(pda_player.tex_icon_inventory, "Inventory", "btn_inven"), 0);
            addItem(new menu_item(pda_player.tex_icon_status, "Status", "btn_status"), 0);
            //addItem(new menu_item(pda_player.tex_icon_map, "Map", "btn_map"), 0);
            return m_current;
        }
        public static void addItem(menu_item New, int index)
        {
            try
            {
                menu_item[] old = m_current.items;
                m_current.items = new menu_item[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    m_current.items[i] = old[i];
                }
                New.myTag = index;
                New.pos = new Vector2(142, 225);
                New.selected = false;
                m_current.items[old.Length] = New;
            }
            catch
            {
                m_current.items = new menu_item[1];
                New.myTag = index;
                New.pos = new Vector2(142, 225);
                New.selected = false;
                m_current.items[0] = New;
            }
            NextTag++;
        }
        public static void draw(SpriteBatch sb)
        {
            sb.Begin();

            //Vector2 pos = Game1.v_resolution / 2;



            sb.Draw(pda_player.tex_pda_bckgrnd,
                    pos,
                    null,
                    Color.White,
                    0,
                    new Vector2(pda_player.tex_pda.Width, pda_player.tex_pda.Height) / 2,
                    scale,
                    SpriteEffects.None,
                    0); //background

            sb.Draw(pda_player.tex_pda,
                    pos,
                    null,
                    Color.White,
                    0,
                    new Vector2(pda_player.tex_pda.Width, pda_player.tex_pda.Height) / 2,
                    scale,
                    SpriteEffects.None,
                    0); //pda

            drawItem(sb);

            sb.End();
        }
        public static void drawItem(SpriteBatch sb)
        {
            for (int i = 0; i != m_current.items.Length; i++)
            {
                Color color = Color.SkyBlue;

                color.A = m_current.items[i].alpha;
                if (m_current.name == "inventory")
                {
                    if (inven.items != null && inven.items.Length != 0)
                        if (inven.items[i].quality == 0)
                        {
                            color.B = 50;
                            color.G = 50;
                            color.R = 255;
                            color.A = 255;
                        }
                }
                if (i == selectedItem)
                {
                    color.B = 255;
                    color.G = 255;
                    color.R = 255;
                    color.A = 255;
                }

                Vector2 temp = pos - ((m_current.items[i].pos + new Vector2(0, f_scroll_pos))) * scale;
                if (m_current.items[i].pos.Y + f_scroll_pos <= 225 &&
                    m_current.items[i].pos.Y + f_scroll_pos >= -225)
                {
                    sb.Draw(m_current.items[i].icon,
                        temp,
                        null,
                        color,
                        0,
                        new Vector2(32, 32),
                        scale,
                        SpriteEffects.None,
                        0); //glint
                    sb.DrawString(pda_player.font, m_current.items[i].text, temp + new Vector2(32, 0), color);
                }
            }
        }

        #endregion

        public static void addMessage(string message)
        {
            try
            {
                string[] old = queue;
                queue = new string[old.Length + 1];
                for (int i = 0; i != old.Length; i++)
                {
                    queue[i] = old[i];
                }
                queue[old.Length] = message;
            }
            catch
            {
                queue = new string[1];
                queue[0] = message;
            }
        }
        public static void killLastMessage()
        {
            if (queue != null && queue.Length != 0)
            {
                string[] old = queue;
                queue = new string[old.Length - 1];
                for (int i = 0; i != old.Length; i++)
                {
                    if (i != 0)
                    {
                        queue[i - 1] = old[i];
                    }
                }
            }
        }
        public static void updatePump()
        {
            displayTime = 255;

            if (Game1.time_ticks - time_displayed > displayTime)
            {
                killLastMessage();
                time_displayed = Game1.time_ticks;
            }
        }
        public static void drawMessages(SpriteBatch sb)
        {
            if (queue != null && queue.Length != 0)
            {
                sb.Begin();
                float delay = 0.5f;
                float x = Game1.time_ticks - time_displayed;
                byte alpha = (byte)(-Math.Abs((((delay * 255) - x) * ((delay * 255) - x)) / ((delay * delay) * 255)) + 255);
                Color Color = new Color(0, 0, 0, (byte)(alpha / 25));

                for (int i = 0; i != 100; i++)
                {
                    Vector2 pos = new Vector2((float)(gfunc.RandomNumber(4, 9) * Math.Cos(i * .0628f) + 20), (float)(gfunc.RandomNumber(4, 9) * Math.Sin(i * .0628f) + 20));

                    sb.DrawString(pump_font, queue[0], pos, Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    //sb.DrawString(pump_font, queue[0], new Vector2(i*.8f,20), Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    //sb.DrawString(pump_font, queue[0], new Vector2(20, i * .8f), Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    //sb.DrawString(pump_font, queue[0], new Vector2(i * .8f, i * .8f), Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    //sb.DrawString(pump_font, queue[0], new Vector2(-i * .8f, -i * .8f), Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
                for (int i = 0; i != 50; i++)
                {
                }
                Color = new Color(255, 0, 0, alpha);

                sb.DrawString(pump_font, queue[0], Vector2.One * 20, Color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                sb.End();
            }
        }
    }
    public struct pda
    {
        public Texture2D tex_pda;
        public Texture2D tex_pda_bckgrnd;
        //public Texture2D tex_pda_glint;
        public Texture2D tex_icon_inventory, tex_icon_status, tex_icon_map, tex_icon_equip, tex_icon_drop, tex_icon_examine;
        public SpriteFont font;
        public Color color;
        public pda(Texture2D pda, Texture2D background, SpriteFont Font, Color col)
        {
            tex_pda = pda;
            tex_pda_bckgrnd = background;
            //tex_pda_glint = glint;
            font = Font;
            color = col;

            tex_icon_equip = tex_icon_drop = tex_icon_examine = tex_icon_inventory = tex_icon_status = tex_icon_map = null;
        }
    }
    public struct menu_item
    {
        public Texture2D icon;
        public byte alpha;
        public Vector2 pos;
        public string text, name;
        public int myTag;
        public itemType type;
        public int[] children;
        public bool selected;
        public menu_item(Texture2D ico, string Text, string Name)
        {
            alpha = 0;
            icon = ico;
            text = Text;
            name = Name;
            myTag = 0;
            type = itemType.junk;
            children = null;
            pos = Vector2.Zero;
            selected = false;
        }
    }
    public struct menu
    {
        public string name;
        public menu_item[] items;
        public menu(string Name, menu_item[] Items)
        {
            name = Name;
            items = Items;
        }
    }

}
