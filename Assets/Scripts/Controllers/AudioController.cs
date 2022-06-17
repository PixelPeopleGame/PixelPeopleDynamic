using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialControllers
{
    public class AudioController : Singleton<AudioController>
    {
        [field: SerializeField]
        public AudioSource AudioPlayer { get; private set; }

        //de audio speler voor de app. om een geluid hier in te zetten zorgen dat een naam en clip on de zelfe index staan
        [field: SerializeField]
        private string[] SoundNames { get; set; }

        [field: SerializeField]
        public AudioClip[] SoundEffects { get; set; }

        //gebruik de naam in de SoundNames array om geluid aftespelen
        public void RequestSounds(string Name)
        {
            Name = Name.ToLower();

            for (int i = 0; i < SoundNames.Length; i++)
            {
                if (SoundNames[i] == Name)
                {
                    try
                    {
                        AudioPlayer.clip = SoundEffects[i];
                    }
                    catch
                    {
                        AudioPlayer.clip = SoundEffects[0];
                    }
                }
            }

            AudioPlayer.Play();
        }
    }
}
