using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoBotones : MonoBehaviour
{
    public AudioSource click;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        click.clip = clip;
    }

    public void Reproducir ()
    {
        click.Play();
    }
}
