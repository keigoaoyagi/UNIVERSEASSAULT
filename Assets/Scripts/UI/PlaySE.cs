using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlaySound", 2);
    }

    // Update is called once per frame
    private void PlaySound()
    {
        audioSource.Play();
    }
}
