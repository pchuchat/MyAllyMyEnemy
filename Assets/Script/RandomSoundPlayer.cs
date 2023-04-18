using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Janne Kettunen
// Class for playing a random sound from a list with a chance to not play any sound at all
[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Plays a random sound from given list with optional chance of playing the sound
    /// </summary>
    /// <param name="clips">List of clips to choose from</param>
    /// <param name="chanceToPlay">Optional chance to play, on default always play</param>
    public void Play(List<AudioClip> clips, float chanceToPlay = 100f)
    {
        if (Random.Range(0f, 100f) <= chanceToPlay)
        {
            source.clip = clips[Random.Range(0, clips.Count)];
            source.Play();
        }
    }
}
