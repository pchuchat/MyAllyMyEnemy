using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Janne Kettunen
// Class for playing a random sound from a list with a chance to not play any sound at all

public class RandomSoundPlayer : MonoBehaviour
{
    [Tooltip("AudioSource for sounds played")] [SerializeField] private AudioSource source;

    /// <summary>
    /// Plays a random sound from given list with a chance of no sound
    /// </summary>
    /// <param name="clips">List of clips to choose from</param>
    /// <param name="chanceToPlay">Chance to play the sound</param>
    public void Play(List<AudioClip> clips, float chanceToPlay)
    {
        if (Random.Range(0f, 100f) <= chanceToPlay)
        {
            source.clip = clips[Random.Range(0, clips.Count)];
            source.Play();
        }
    }
}
