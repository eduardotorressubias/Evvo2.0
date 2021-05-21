using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject freelookPlayer;
    public GameObject freelookPortal;

    private void Start()
    {
        for (int i = 0; i < freelookPlayer.transform.childCount; i++)
        {
            if (freelookPlayer.transform.GetChild(i).gameObject.activeSelf)
            {
                OptionsMenu.current_camera = freelookPlayer.transform.GetChild(i).gameObject;
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OptionsMenu.current_camera.SetActive(false);
            freelookPortal.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            freelookPortal.SetActive(false);
            OptionsMenu.current_camera.SetActive(true);
        }
    }
}
