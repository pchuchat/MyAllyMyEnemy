using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwap : MonoBehaviour
{
    [SerializeField]
    private GameObject trigger;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip music;

    [SerializeField]
    private bool fadeOut;

    private bool hasTriggered = false;

    private float changeTimer = -1f;

    void Update()
    {
        if (!hasTriggered)
        {
            if (trigger.tag == "triggered")
            {
                hasTriggered = true;

                if (fadeOut) {
                    changeTimer = 1f;
                }
                else
                {
                    source.clip = music;
                    source.Play();
                }
            }
        }

        if (changeTimer > -1f)
        {

            if (changeTimer > 0)
            {
                changeTimer -= Time.deltaTime;
                source.volume = changeTimer;
            }
            else
            {
                changeTimer = -1f;
                source.volume = 1f;
                source.clip = music;
                source.Play();
            }
        }
    }
}
