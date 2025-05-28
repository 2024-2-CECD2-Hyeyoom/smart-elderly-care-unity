using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public float openDelay = 1f;
    public bool doorOpen = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(OpenAndClose());
        }
    }

    IEnumerator OpenAndClose()
    {
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        doorOpen = true;
        Debug.Log("¹® ¿­¸²");

        yield return new WaitForSeconds(openDelay);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        doorOpen = false;
        Debug.Log("¹® ´ÝÈû");
    }
}

