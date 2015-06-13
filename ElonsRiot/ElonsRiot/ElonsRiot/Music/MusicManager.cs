using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot.Music
{
    public static class MusicManager
    {
        static SoundEffect soundEffect;
        static float volume = 1.0f;
        static float pitch = 0.0f; //przyspieszanie dźwięku
        static float pan = 0.0f; //panorama
        static List<SoundEffectInstance> soundEffectInstance;
        public static void Initialize(ContentManager content )
        {
            soundEffectInstance = new List<SoundEffectInstance>();
          soundEffect = content.Load<SoundEffect>("Sounds/MyTree");
          soundEffectInstance.Add(soundEffect.CreateInstance());
          soundEffect = content.Load<SoundEffect>("Sounds/EXPLOSIO");
          soundEffectInstance.Add(soundEffect.CreateInstance());
        }
        public static void SetSoundEffectInstance(float volue,float pan, float pitch, bool isLooped,int index)
        { 
            soundEffectInstance[index].Volume = volume;
            soundEffectInstance[index].Pan = pan;
            soundEffectInstance[index].Pitch = pitch;
            soundEffectInstance[index].IsLooped = isLooped;
        }
        public static void PlaySound(int index)
        {
            soundEffectInstance[index].Play();   
        }
        public static void StopSound(int index)
        {
            soundEffectInstance[index].Stop(); 
        }
    }
}
