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

namespace AnimoNex.game
{
    class mainMenuFunc
    {
        static Vector2 cameraDestination;
        static Vector2 cameraVelocity=Vector2.Zero;
        static int tick;
        static int oldSelection;
        static float f_deltaV = 80;
        static float f_selectedItem = 0;
        static bool looping = false;

        public static Texture2D title;
        //public static menuItem mi_startgame, mi_continue, mi_credits;
        public static menuItem[] items;

        public static Cue mm_introMusic;
        public static Cue mm_LoopingMusic;
        public static Cue mm_selectionChange;
        public static Cue mm_select;

        public static void Update()
        {
            if (!mm_introMusic.IsPlaying && !looping)
            {
                mm_LoopingMusic.Resume();
                looping = true;
            }

            CameraPan();
            traslateOptions();
            ManageSelection();
        }

        private static void ManageSelection()
        {
            mainMenuFunc.mm_select = sounds.soundBank.GetCue("mm_select");
            mainMenuFunc.mm_selectionChange = sounds.soundBank.GetCue("mm_selectionChange");

            f_selectedItem -= Controller.lStick(PlayerIndex.One).Y / 8;
            int i_item = gfunc.roundUp(f_selectedItem);

            KeyboardState ks = Keyboard.GetState();

            if ((Controller.is_a_pressed(PlayerIndex.One) || ks.IsKeyDown(Keys.A)) && !mm_select.IsPlaying)
            {
                //sounds.soundBank.PlayCue(mm_select.Name);
                if (items[i_item].type == buttontype.start)
                {
                    if (!Guide.IsVisible)
                    {
                        if (Gamer.SignedInGamers.Count < 1)
                        {
                            Guide.ShowSignIn(1, false);
                        }
                        else
                        {
                            mm_LoopingMusic.Pause();
                            mm_introMusic.Pause();
                            Game1.initializeGamePlay();
                        }
                    }
                }
                if (items[i_item].type == buttontype.credits)
                {
                    mm_LoopingMusic.Pause();
                    mm_introMusic.Pause();
                    Game1.currentMenu = Game1.activeMenu.credits;
                }
                if (items[i_item].type == buttontype.resume)
                {
                    mm_LoopingMusic.Pause();
                    mm_introMusic.Pause();
                    Game1.currentMenu = Game1.activeMenu.howtoplay;
                }
                mm_select.Play();
            }
            if (i_item >= items.Length)
            {
                f_selectedItem = 0;
            }
            else if (i_item < 0)
            {
                f_selectedItem = items.Length - 1;
            }
            if (i_item != oldSelection && !mm_selectionChange.IsPlaying)
            {
                mm_selectionChange.Play();
            }
            oldSelection = i_item;
        }
        public static void Draw(SpriteBatch sb,Texture2D tex1)
        {

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                shaders.MainMenuBlurBegin();
                    sb.Draw(tex1, Vector2.Zero, Color.White);
                shaders.MainMenuBlurEnd();
            sb.End();
            sb.Begin();

            for (int i = 0; i != items.Length; i++)
            {
                Color col = Color.Red;
                float scale = 1f;
                int selected = gfunc.roundUp(f_selectedItem);

                if (i == selected)
                {
                    col = Color.White;
                    scale = 1.25f;
                }

                sb.Draw(items[i].texture, items[i].position,
                        null,
                        col,
                        0,
                        new Vector2(items[i].texture.Width, items[i].texture.Height) / 2,
                        scale,
                        SpriteEffects.None, 1);
            }
            sb.Draw(title, new Vector2(Game1.v_resolution.X / 2, 0), null, Color.White, 0, new Vector2(title.Width / 2, 0), 1, SpriteEffects.None, .25f);
            sb.End();
        }
        static void traslateOptions()
        {
            for (int i = 0; i != items.Length;i++)
            {
                float f_velocity = (Vector2.Distance(items[i].position, items[i].destination) / (f_deltaV * 0.1f));
                Vector2 newVelocity = gfunc.TranslateOnAng(f_velocity, gfunc.PointAt(items[i].position, items[i].destination));  //find the new velocity
                items[i].position += newVelocity;
            }
        }


        private static void CameraPan()
        {
            if (!gfunc.timeWaited(tick, 250))
            {
                cameraVelocity += gfunc.TranslateOnAng(0.1f, gfunc.PointAt(camera.v_pos, cameraDestination));
            }
            else
            {
                tick = Game1.time_ticks;
                cameraDestination = LvlFunc.randomPositon();
            }

            int floor = LvlFunc.findFloor(world.levels[world.i_currentLvl]);
            cameraVelocity *= .89f;
            Vector2 oldCamPos = camera.v_pos;
            camera.v_pos.X += cameraVelocity.X;
            if (!LvlFunc.isInRegion(new Vector2(0, Game1.v_resolution.Y) + camera.v_pos, world.levels[world.i_currentLvl].regions[floor]) ||
                !LvlFunc.isInRegion(Game1.v_resolution + camera.v_pos, world.levels[world.i_currentLvl].regions[floor]))
            {
                camera.v_pos.X = oldCamPos.X;
            }
            camera.v_pos.Y += cameraVelocity.Y;
            if (!LvlFunc.isInRegion(new Vector2(0, Game1.v_resolution.Y) + camera.v_pos, world.levels[world.i_currentLvl].regions[floor]) ||
                !LvlFunc.isInRegion(Game1.v_resolution + camera.v_pos, world.levels[world.i_currentLvl].regions[floor]))
            {
                camera.v_pos.Y = oldCamPos.Y;
            }
        }

    }
    struct menuItem
    {
        public Texture2D texture;
        public Vector2 position;
        public  Vector2 destination;
        public buttontype type;
        public menuItem(Texture2D tex, Vector2 pos, buttontype Type)
        {
            texture = tex;
            position = pos;
            destination = Vector2.Zero;
            type = Type;
        }
    }
    enum buttontype
    {
        start,resume,credits
    };
}