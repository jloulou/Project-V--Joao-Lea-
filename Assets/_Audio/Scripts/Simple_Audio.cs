using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Audio : MonoBehaviour
{
    private AudioSource audioS;
    public Light connectedSpotLight;
    public bool triggerEnter = true;
    public bool triggerExit = false;
    public bool destroySelf = false;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
        if (audioS == null)
        {
            Debug.LogWarning("No AudioSource component found on " + gameObject.name);
        }

        if (connectedSpotLight == null)
        {
            Debug.LogWarning("No Spot Light reference set on " + gameObject.name);
        }
        else if (connectedSpotLight.type != LightType.Spot)
        {
            Debug.LogWarning("Connected light on " + gameObject.name + " is not a Spot Light");
        }

        // Make sure the spotlight is initially off
        if (connectedSpotLight != null)
        {
            connectedSpotLight.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerEnter)
        {
            if (other.CompareTag("Player"))
            {
                audioS.Play(); //----------------------SOM--------------------------

                if (connectedSpotLight != null)
                {
                    connectedSpotLight.enabled = true;  //----------------------LIGHT--------------------------
                }

                if (destroySelf)
                {
                    StartCoroutine(DestroyAfterAudio());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerExit)
        {
            if (other.CompareTag("Player"))
            {
                audioS.Play(); //----------------------SOM--------------------------

                if (connectedSpotLight != null)
                {
                    connectedSpotLight.enabled = false;  //----------------------LIGHT--------------------------
                }
            }
        }
    }

    private IEnumerator DestroyAfterAudio()
    {
        // Wait until the audio has finished playing
        while (audioS.isPlaying)
        {
            yield return null;
        }

        // Then destroy the object
        Destroy(this.gameObject);
    }
}