using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace AnimoNex.game.effects
{
    class sounds
    {
        public static AudioEngine audio, musicAudio;
        public static WaveBank waves,music;
        public static SoundBank soundBank,musicSoundBank;

        public static void LoadSoundContent(string basePath)
        {
            basePath += "/";
            audio = new AudioEngine(basePath + "sound/music/AnimoNex.xgs");
            waves = new WaveBank(audio, basePath + "sound/music/Wave Bank.xwb");
            soundBank = new SoundBank(audio, basePath + "sound/music/Sound Bank.xsb");

            musicAudio = new AudioEngine(basePath + "sound/music/AnimoNexMusic.xgs");
            music = new WaveBank(musicAudio,basePath + "sound/music/musicWaves.xwb",0,8);
            musicSoundBank = new SoundBank(musicAudio, basePath + "sound/music/musicSoundBank.xsb");

            musicAudio.Update();
            mainMenuFunc.mm_introMusic = sounds.musicSoundBank.GetCue("mm_intro");
            mainMenuFunc.mm_LoopingMusic = sounds.musicSoundBank.GetCue("mm_loop");

        }
    }
}
