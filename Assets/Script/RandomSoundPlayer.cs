using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: Janne Kettunen
// Class for playing a random sound from a list with a chance to not play any sound at all
[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [Tooltip("Optional audio source to copy for sounds")][SerializeField] private AudioSource source;

    private AudioSource tempSource;

    /// <summary>
    /// Plays a random sound from given list with optional chance of playing the sound
    /// </summary>
    /// <param name="clips">List of clips to choose from</param>
    /// <param name="chanceToPlay">Optional chance to play, on default always play</param>
    public void Play(List<AudioClip> clips, float chanceToPlay = 100f)
    {
        if (clips.Count == 0) return;
        if (Random.Range(0f, 100f) <= chanceToPlay)
        {
            if (source != null) tempSource = Instantiate(source);
            else tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.clip = clips[Random.Range(0, clips.Count)];
            tempSource.Play();
            Destroy(source, tempSource.clip.length);
        }
    }
}
