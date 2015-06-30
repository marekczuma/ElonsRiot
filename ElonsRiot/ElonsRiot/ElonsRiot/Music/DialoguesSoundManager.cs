using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot.Music
{
    public static class DialoguesSoundManager
    {
        static SoundEffect soundEffect;
        static float volume = 1.0f;
        static float pitch = 0.0f; //przyspieszanie dźwięku
        static float pan = 0.0f; //panorama
        static List<SoundEffectInstance> dialoguesEffectInstance;
        /// <summary>
        /// Tutaj dodajemy wszystkie dźwięki w grze. 
        /// </summary>
        /// <param name="content"></param>
        public static void Initialize(ContentManager content)
        {
            dialoguesEffectInstance = new List<SoundEffectInstance>();
            soundEffect = content.Load<SoundEffect>("Dialogi/01"); //0
            dialoguesEffectInstance.Add(soundEffect.CreateInstance());
        }
        /// <summary>
        /// Jeśli chcemy ustawić jakieś inne wartosci niz domyslne dla wybranego dzwieku
        /// </summary>
        /// <param name="volue">glosnosc </param>
        /// <param name="pan"> panorama </param>
        /// <param name="pitch"> przyspieszenie dzwieku</param>
        /// <param name="isLooped">czy zapetlic dziek</param>
        /// <param name="index"> index w liscie soudEffectInstance</param>
        public static void SetSoundEffectInstance(float volue, float pan, float pitch, bool isLooped, int index)
        {
            dialoguesEffectInstance[index].Volume = volume;
            dialoguesEffectInstance[index].Pan = pan;
            dialoguesEffectInstance[index].Pitch = pitch;
            dialoguesEffectInstance[index].IsLooped = isLooped;
        }
        /// <summary>
        /// Zeby wlaczyc wybrany dzwiek z listy (jesli dzwiek nie jest zapetlony uruchomi sie raz)
        /// </summary>
        /// <param name="index"></param>
        public static void PlaySound(int index)
        {
            dialoguesEffectInstance[index].Play();
        }
        /// <summary>
        /// zeby wylaczyc wybrany dzwiek z listy
        /// </summary>
        /// <param name="index"></param>
        public static void StopSound(int index)
        {
            dialoguesEffectInstance[index].Stop();
        }
    }
}
