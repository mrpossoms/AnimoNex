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
using AnimoNex.Initialize;

namespace AnimoNex.game.effects
{
    class particles
    {
        public static Texture2D[] TempTex;
        public static particle[] blood_spray;
        public static particle[] blood_splash;
        public static particle[] blood_spurt;
        public static particle[] blood_puddle;
        public static particle[] blood_puddle_wall;
        public static particle[] shell_ar;
        public static particle[] shell_pistol;
        public static particle[] shell_shotgun;
        public static particle[] smoke_local;
        public static particle[] smoke_ambi;
        public static particle[] gib;
        public static void LoadAndInit(ContentManager Content)
        {
            #region BloodEffects
            TempTex = load_content.LoadParticles(Content, "blood_spray");
            blood_spray = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                blood_spray[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        .5f,
                                        1.5f,
                                        100,
                                        true,
                                        false,
                                        true,
                                        particleType.blood);
            }
            TempTex = load_content.LoadParticles(Content, "blood_splash");
            blood_splash = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                blood_splash[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        0,
                                        1.5f,
                                        10,
                                        true,
                                        false,
                                        true,
                                        particleType.smoke);
            }

            TempTex = load_content.LoadParticles(Content, "blood_spurt");
            blood_spurt = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                blood_spurt[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        0,
                                        1.5f,
                                        10,
                                        true,
                                        false,
                                        true,
                                        particleType.smoke);
            }
            TempTex = load_content.LoadParticles(Content, "blood_puddle");
            blood_puddle = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                blood_puddle[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        0,
                                        1,
                                        20,
                                        true,
                                        false,
                                        true,
                                        particleType.smoke);
            }
            TempTex = load_content.LoadParticles(Content, "blood_puddle_wall");
            blood_puddle_wall = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                blood_puddle_wall[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        0,
                                        1,
                                        20,
                                        true,
                                        false,
                                        true,
                                        particleType.smoke);
            }
            TempTex = load_content.LoadParticles(Content, "gibs");
            gib = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                gib[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        1,
                                        1,
                                        200,
                                        false,
                                        false,
                                        false,
                                        particleType.gib);
            }
            #endregion
            #region SpentShells
            TempTex = load_content.LoadParticles(Content, "spent_shells_ar");
            shell_ar = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                shell_ar[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        -1,
                                        -1,
                                        300,
                                        false,
                                        false,
                                        false,
                                        particleType.shell);
            }
            TempTex = load_content.LoadParticles(Content, "spent_shells_pistol");
            shell_pistol = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                shell_pistol[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        -1,
                                        -1,
                                        300,
                                        false,
                                        false,
                                        false,
                                        particleType.shell);
            }
            TempTex = load_content.LoadParticles(Content, "spent_shells_shotgun");
            shell_shotgun = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                shell_shotgun[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        -1,
                                        -1,
                                        300,
                                        false,
                                        false,
                                        false,
                                        particleType.shell);
            }
            #endregion
            #region SmokeEffects
            TempTex = load_content.LoadParticles(Content, "local_smoke");
            smoke_local = new particle[TempTex.Length];
            for (int i = 0; i != TempTex.Length; i++)
            {
                smoke_local[i] = new particle(Vector3.Zero,
                                        Vector3.Zero,
                                        TempTex[i],
                                        .99f,
                                        0,
                                        2.5f,
                                        100,
                                        true,
                                        false,
                                        true,
                                        particleType.smoke);
            }
                TempTex = load_content.LoadParticles(Content, "ambient_smoke");
                smoke_ambi = new particle[TempTex.Length];
                for (int i = 0; i != TempTex.Length; i++)
                {
                    smoke_ambi[i] = new particle(Vector3.Zero,
                                            Vector3.Zero,
                                            TempTex[i],
                                            .99f,
                                            0,
                                            2.5f,
                                            200,
                                            true,
                                            false,
                                            true,
                                            particleType.smoke);
            #endregion
                }
            }
        }
}
