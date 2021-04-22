using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject freelookPlayer;
    public GameObject freelookPortal;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            freelookPlayer.SetActive(false);
            freelookPortal.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            freelookPlayer.SetActive(true);
            freelookPortal.SetActive(false);
        }
    }
}
