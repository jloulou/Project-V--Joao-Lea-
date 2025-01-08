using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger_Simple : MonoBehaviour
{
    private AudioSource audioS;
    private GameObject player;
    public AudioClip triggerSound;
    public bool triggerEnter = true;
    public bool triggerExit = false;
    public bool destroySelf = false;
    public float volume = 1.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
            return;
        }

        audioS = player.GetComponent<AudioSource>();
        if (audioS == null)
        {
            Debug.LogError("No AudioSource found on the Player. Please add an AudioSource component.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerEnter && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone.");
            PlaySound();

            if (destroySelf)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerExit && other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger zone.");
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (audioS != null && triggerSound != null)
        {
            audioS.PlayOneShot(triggerSound, volume);
        }
        else
        {
            Debug.LogWarning("AudioSource or TriggerSound is missing!");
        }
    }
}
