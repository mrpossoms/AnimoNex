using System;
using System.Collections.Generic;
using System.Threading;
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
using AnimoNex.game.pda;
using AnimoNex.game.effects;
using AnimoNex.Initialize;

namespace AnimoNex
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static SpriteFont debug;
        public static float angOffset=0;
        public static int time_ticks;
        public static GameTime gt;
        public static bool DoneLoading = false;
        public static string loadingProgress;
        public static Texture2D loadScreenGfx;
        public static Texture2D loadScreenBlood;
        public static byte loadScreenAlpha = 0;
        public static float splash = 0;
        public static bool DoneInitializing = false;
        public static activeMenu currentMenu = activeMenu.main;

        Thread Thread_ContentLoading = new Thread(thread_load);
        //Thread Thread_CharacterUpdate = new Thread(thread_character);
        //Thread Thread_CharacterDraw = new Thread(thread_charDraw);
        //Thread Thread_ParticleUpdate = new Thread(thread_partUpdate);

        //private MessageBase currentMessage;
        //bool DashInUse = false;

        public static Effect desat;
        public static Effect hiContrast;
        public static Effect disappear;
        public static Effect refract;
        public static Effect bloom;
        public static Effect death;
        public static Effect blurMainMenu;
        public static Effect invert;
        public static Effect lightFlicker;
        public static Texture t_shad_waterFall;
        public static Texture2D deathOverlay,winOverlay,mainMenu;
        public static Texture2D triad;

        public static GraphicsDeviceManager graphics;
        public static SpriteBatch SprtBtch;
        public static ContentManager ctMngr;
        //GamerServicesComponent services;

        SpriteBatch spriteBatch;
        //public static Vector2 v_resolution = new Vector2(1680, 1050);
        public static Vector2 v_resolution = new Vector2(800, 600);
        public static RenderTarget2D rt;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //services = new GamerServicesComponent(this);

            Components.Add(new GamerServicesComponent(this));

            //graphics.SynchronizeWithVerticalRetrace = false;

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            Controller.initilize();
            graphics.PreferredBackBufferWidth = (int)v_resolution.X;
            graphics.PreferredBackBufferHeight = (int)v_resolution.Y;
            graphics.ToggleFullScreen();
            //graphics.ToggleFullScreen();
            //graphics.GraphicsDevice.RenderState.MultiSampleAntiAlias = true;
            //graphics.GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.SixteenSamples; // Set it to what Video card can support
            //graphics.GraphicsDevice.PresentationParameters.MultiSampleQuality = 32;// Set it to what the video card can support!

            //graphics.SynchronizeWithVerticalRetrace = false;
            rt = new RenderTarget2D(GraphicsDevice,
                        (int)v_resolution.X,
                        (int)v_resolution.Y);

        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ctMngr = Content;
            SprtBtch = spriteBatch;
            //Thread_ContentLoading.
            Thread_ContentLoading.Start();
            
            //KeyboardRequest kreq = currentMessage as KeyboardRequest;
            //Guide.BeginShowKeyboardInput(PlayerIndex.One, "Name", "Enter your first name.", "", MessageEnded,kreq);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            world.animations = null;
            world.characters = null;
            world.items = null;
            world.levels = null;
            world.weapons = null;
            spriteBatch.Dispose();
            ctMngr.Dispose();
            rt.Dispose();
            bloom.Dispose();
        }


        protected override void Update(GameTime gameTime)
        {
            if (DoneLoading)
            {
                if (Thread_ContentLoading != null)
                {
                    Thread_ContentLoading = null;
                }
                gt = gameTime;

                // Allows the game to exit
                //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                //    this.Exit();
                    PEngine.particle_update(camera.v_pos);
                    CharFunc.updateAll();

                switch (currentMenu)
                {
                    case activeMenu.game:
                        GamePlay();
                    break;

                    case activeMenu.main:
                        credits.progress = 0;
                        //Update_SignIn();
                        mainMenuFunc.Update();
                    break;
                    case activeMenu.credits:
                        credits.update();
                    break;         
                }
                //GamePlay();

                // TODO: Add your update logic here
                //DELETEME = CharFunc.RenderToTexture(DELETEME, spriteBatch, graphics.GraphicsDevice);
                sounds.audio.Update();
                sounds.musicAudio.Update();
            }
            base.Update(gameTime);
        }

        private static void GamePlay()
        {
                weapFunc.UpdateBullets();
                AIfunc.doLowlevelProcessing();
                itemFunc.update();
                UI.updatePDA();
                UI.updatePump();
        }
        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Black);
            if (DoneLoading)
            {
                graphics.GraphicsDevice.SetRenderTarget(rt);
                GraphicsDevice.Clear(Color.Transparent);

                    world.drawSky(spriteBatch);
                    LvlFunc.draw(spriteBatch, camera.v_pos);

                    debrisEffect.draw(spriteBatch);
                    itemFunc.drawItem(spriteBatch);
                    CharFunc.drawAll(spriteBatch);
                    PEngine.particle_draw(spriteBatch, camera.v_pos);
                    weapFunc.drawProjectiles(spriteBatch);

                rt.GraphicsDevice.SetRenderTarget(null);
                Texture2D tex = rt;

                //graphics.GraphicsDevice.SetRenderTarget(rt);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                    shaders.HighContrastBegin();
                        spriteBatch.Draw(tex, Vector2.Zero, Color.White);
                    shaders.HighContrastEnd();
                spriteBatch.End();
                rt.GraphicsDevice.SetRenderTarget(null);
                Texture2D tex1 = rt;


                switch (currentMenu)
                {
                    case activeMenu.game:
                        GameDraw(tex1);
                        break;

                    case activeMenu.main:
                        mainMenuFunc.Draw(spriteBatch,tex1);
                        break;
                    case activeMenu.credits:
                        credits.draw(spriteBatch,tex1);
                        break;
                    case activeMenu.howtoplay:
                        howto.draw(spriteBatch,tex1);
                        break;
                }

                //GameDraw(tex1); //draw stuff specific to game play

                time_ticks++;
            }
            else
            {
                gt = gameTime;

                if (splash < 1 && triad != null)
                {
                    GraphicsDevice.Clear(Color.Gray);
                    spriteBatch.Begin();
                    splash += 0.005f;
                    spriteBatch.Draw(triad, v_resolution / 2, null, Color.White, 0, new Vector2(triad.Width, triad.Height) / 2,1, SpriteEffects.None, 0);

                    spriteBatch.End();
                }
                else
                {
                    if (debug != null)
                    {
                    GraphicsDevice.SetRenderTarget(rt);
                    GraphicsDevice.Clear(Color.Transparent);

                        if (loadScreenAlpha < 254)
                        {
                            loadScreenAlpha += 2;
                        }
                        else
                        {
                            loadScreenAlpha = 254;
                        }
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

                        spriteBatch.Draw(loadScreenBlood, new Vector2(v_resolution.X * .625f, v_resolution.Y * .03f), Color.White);
                        shaders.refractionBegin();
                        shaders.refractionEnd();
                        spriteBatch.End();

                        graphics.GraphicsDevice.SetRenderTarget(null);
                        Texture2D tex1 = rt;

                        spriteBatch.Begin();
                        spriteBatch.Draw(loadScreenGfx, new Rectangle(0, 0, (int)v_resolution.X, (int)v_resolution.Y), new Color(Vector4.One));
                        spriteBatch.DrawString(debug, "[Loading..." + loadingProgress + "]", new Vector2(v_resolution.X / 6, v_resolution.Y * .92f), new Color(new Vector4(Color.Maroon.ToVector3(), loadScreenAlpha)));
                        spriteBatch.Draw(tex1, Vector2.Zero, Color.White);
                        spriteBatch.End();





                    }
                }
            }
            base.Draw(gameTime);

        }

        private void GameDraw(Texture2D tex1)
        {


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            if (!CharFunc.thisPlayerLiving || AIfunc.livingZombies == 0)
            {
                shaders.deathBegin();
                spriteBatch.Draw(tex1, Vector2.Zero, Color.White);

                shaders.deathEnd();
            }
            else
            {
                shaders.bloomBegin(0.4f);
                spriteBatch.Draw(tex1, Vector2.Zero, Color.White);
                shaders.bloomEnd();
            }
            spriteBatch.End();

            if (world.levels[world.i_currentLvl].indoor && CharFunc.thisPlayerLiving)
            {
                graphics.GraphicsDevice.SetRenderTarget(null);
                Texture2D tex2 = rt;

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                shaders.lightFlickerBegin();
                    spriteBatch.Draw(tex2, Vector2.Zero, Color.White);
                    shaders.lightFlickerEnd();
                spriteBatch.End();

            }

            scoreFunc.drawScore(spriteBatch);
            UI.draw(spriteBatch);
            UI.drawMessages(spriteBatch);
            
            if (!CharFunc.thisPlayerLiving || AIfunc.livingZombies==0)
            {
                spriteBatch.Begin();
                if (!CharFunc.thisPlayerLiving)
                {
                    spriteBatch.Draw(deathOverlay, new Rectangle(0, 0, (int)v_resolution.X, (int)v_resolution.Y), new Color(255, 255, 255, (byte)(120 * shaders.deathAlpha)));
                }
                else
                {
                    spriteBatch.Draw(winOverlay, new Rectangle(0, 0, (int)v_resolution.X/2, (int)v_resolution.Y/2), new Color(255, 255, 255, (byte)(120 * shaders.deathAlpha)));
                }
                if (shaders.deathAlpha >= 1)
                {
                    //character p = world.levels[world.i_currentLvl].char_living[CharFunc.ThisPlayerTag];

                    spriteBatch.Draw(mainMenu, Game1.v_resolution / 2, null, Color.White, 0, new Vector2(mainMenu.Width, mainMenu.Height) / 2, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(UI.pump_font, "Kills: " + CharFunc.ThisPlayerKills,
                                           (Game1.v_resolution / 2) + new Vector2(0, 60), Color.White);
                }
                spriteBatch.End();
                if (shaders.deathAlpha >= 1)
                {
                    //spriteBatch.Draw(mainMenu, Game1.v_resolution / 2, null, Color.White, 0, new Vector2(mainMenu.Width, mainMenu.Height) / 2, 1, SpriteEffects.None, 0);
                    if (Controller.is_a(PlayerIndex.One))
                    {
                        currentMenu = activeMenu.main;
                        for (int i = 0; i != world.levels.Length; i++)
                        {
                            if (world.levels[i].name != "mainmenu")
                            {
                                world.levels[i].char_living = null;
                                world.levels[i].items = null;
                                world.levels[i].debris_current = null;
                            }
                        }
                        LvlFunc.setActiveLevelTo("mainmenu");
                    }
                }
            }
        }
        public static void initializeGamePlay()
        {
            shaders.deathAlpha = 0.1f;
            sounds.musicSoundBank.PlayCue("game_music");
            camera.v_pos = new Vector2(150, 50);
            LvlFunc.populateItems(10);
            LvlFunc.setActiveLevelTo("Anderson St. NE");
            UI.initializePDA();
            //item temp = world.items[0];
            //temp.B_base.v_position = new Vector2(250, 500);
            //temp.B_base.f_Position_Z = 20;
            //itemFunc.createItem(world.items[1], new Vector2(370, 500), 100);
            //itemFunc.createItem(world.items[2], new Vector2(380, 500), 100);
            CharFunc.addActor(new Vector2(250, 500), 0, CharType.survivor, ControlType.player_current, "SPECOPS", false);
                world.levels[world.i_currentLvl].char_living[0].Inventory = itemFunc.createItem_INVEN(world.levels[world.i_currentLvl].char_living[0].Inventory,
                                                                                                      world.items[20],
                                                                                                      Vector2.Zero, 0);
            world.levels[world.i_currentLvl].char_living[0].Primary_weap = world.weapons[6];
            world.levels[world.i_currentLvl].char_living[0].Primary_weap.clipCurrent = 6;
            world.levels[world.i_currentLvl].char_living[0].current_weap = world.levels[world.i_currentLvl].char_living[0].Primary_weap;
            for (int i = 0; i != 5; i++)
            {
                CharFunc.addActor(LvlFunc.randomPositon(), 0, CharType.zombie, ControlType.NPC, "", false);
                //CharFunc.addActor(LvlFunc.randomPositon(), 0, CharType.soldier, ControlType.NPC, "HAZMAT", false);
                //CharFunc.addActor(LvlFunc.randomPositon(), 0, CharType.zombie, ControlType.NPC, "BROWN", false);
            }
                        LvlFunc.populateChar(10);

            //world.levels[world.i_currentLvl].char_living[world.levels[world.i_currentLvl].char_living.Length - 1].current_weap = world.levels[world.i_currentLvl].char_living[0].Primary_weap;
            Game1.currentMenu = Game1.activeMenu.game;
        }
        #region Thread_loadContent
        static ThreadStart thread_load = delegate
 {
     refract = ctMngr.Load<Effect>("graphics/shaders/refraction");
     t_shad_waterFall = ctMngr.Load<Texture2D>("graphics/shaders/waterfall");
     loadScreenGfx = ctMngr.Load<Texture2D>("graphics/textures/ui/animoload");
     loadScreenBlood = ctMngr.Load<Texture2D>("graphics/textures/ui/blood drip");
     triad = ctMngr.Load<Texture2D>("graphics/textures/ui/triad");
     debug = ctMngr.Load<SpriteFont>("arial");
     #region LoadMainContent
     loadingProgress = "Sky textures";
     world.sky_game = load_content.LoadSky(ctMngr);

     loadingProgress = "Animations";
     world.animations = load_content.LoadAnims(ctMngr);

     loadingProgress = "Character data";
     world.characters = load_content.LoadCharacters(ctMngr);

     loadingProgress = "Level data";
     world.levels = load_content.LoadLevels(ctMngr);

     loadingProgress = "Weapon index";
     world.weapons = load_content.LoadWeapons(ctMngr);

     loadingProgress = "Item index";
     world.items = load_content.loadItems(ctMngr);

     loadingProgress = "User interface";
     UI.pda_player = load_content.loadPda(ctMngr);

     loadingProgress = "Particle textures";
     particles.LoadAndInit(ctMngr);

     loadingProgress = "Menu Items";
     load_content.loadMainMenu(ctMngr);

     #endregion

     loadingProgress = "shaders";
     desat = ctMngr.Load<Effect>("graphics/shaders/desaturated"); //desaturated");
     hiContrast = ctMngr.Load<Effect>("graphics/shaders/hicontrast");
     disappear = ctMngr.Load<Effect>("graphics/shaders/disappear");
     bloom = ctMngr.Load<Effect>("graphics/shaders/bloom");
     blurMainMenu = ctMngr.Load<Effect>("graphics/shaders/blurBlkWht");
     invert = ctMngr.Load<Effect>("graphics/shaders/invert");
     lightFlicker = ctMngr.Load<Effect>("graphics/shaders/lightFlicker");
     death = ctMngr.Load<Effect>("graphics/shaders/bloodpool");
     deathOverlay = ctMngr.Load<Texture2D>("graphics/textures/ui/deathoverlay");
     winOverlay = ctMngr.Load<Texture2D>("graphics/textures/ui/win");
     credits.kirk = ctMngr.Load<Texture2D>("graphics/textures/ui/kirk");
     credits.jake = ctMngr.Load<Texture2D>("graphics/textures/ui/jake");
     credits.nick = ctMngr.Load<Texture2D>("graphics/textures/ui/nick");
     howto.howtoTex = ctMngr.Load<Texture2D>("graphics/textures/ui/how to");
     howto.controls = ctMngr.Load<Texture2D>("graphics/textures/ui/controlz");
     scoreFunc.scoreSplat = ctMngr.Load<Texture2D>("graphics/textures/ui/bloodScore");
     mainMenu = ctMngr.Load<Texture2D>("graphics/textures/ui/main_inv");
     loadingProgress = "Fonts";
     CharFunc.Tex_null = ctMngr.Load<Texture2D>("graphics/textures/null");
     weapFunc.tracer = ctMngr.Load<Texture2D>("graphics/textures/weapons/effects/tracers/0");
     UI.pump_font = ctMngr.Load<SpriteFont>("chiller");

     loadingProgress = "Sounds";
     sounds.LoadSoundContent(load_content.BasePath);

     //sounds.soundBank.PlayCue("mm_intro");
     mainMenuFunc.mm_introMusic.Play();
     //////////////////////////////////////////////////////////
     //for now im putting the init code in the loading thread
     ////////////////////////////////////////////////////////
     loadingProgress = "Initializing";
     camera.v_pos = new Vector2(150, 50);
     //LvlFunc.populateChar(0);
     //LvlFunc.populateItems(400);
     LvlFunc.setActiveLevelTo("mainmenu");
     UI.initializePDA();
     //item temp = world.items[0];
     //temp.B_base.v_position = new Vector2(250, 500);
     //temp.B_base.f_Position_Z = 20;
     //itemFunc.createItem(world.items[1], new Vector2(370, 500), 100);
     //itemFunc.createItem(world.items[2], new Vector2(380, 500), 100);

     for (int i = 0; i != 50; i++)
     {
         CharFunc.addActor(LvlFunc.randomPositon(), 0, CharType.zombie, ControlType.NPC, "", false);
         //CharFunc.addActor(LvlFunc.randomPositon(), 0, CharType.soldier, ControlType.NPC, "HAZMAT", false);
         //CharFunc.addActor(LvlFunc.randomPositon(), 0, CharType.zombie, ControlType.NPC, "BROWN", false);
     }
     //CharFunc.addActor(new Vector2(250, 500), 0, CharType.survivor, ControlType.player_current, "BROWN", false);
     //world.levels[world.i_currentLvl].char_living[world.levels[world.i_currentLvl].char_living.Length-1].Primary_weap = world.weapons[11];
     //world.levels[world.i_currentLvl].char_living[world.levels[world.i_currentLvl].char_living.Length - 1].Primary_weap.clipCurrent = 100;
     //world.levels[world.i_currentLvl].char_living[world.levels[world.i_currentLvl].char_living.Length - 1].current_weap = world.levels[world.i_currentLvl].char_living[0].Primary_weap;


     DoneLoading = true;
 }; 
        #endregion


        public enum activeMenu
        {
            game, main,credits,howtoplay
        };
    }
}
