using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Animator sing;

    private void onTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sing.SetBool("Armature|Armature|mixamo_com|Layer0", true);
        }
    }

    private void onTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sing.SetBool("Armature|Armature|mixamo_com|Layer0", false);
        }
    }
}