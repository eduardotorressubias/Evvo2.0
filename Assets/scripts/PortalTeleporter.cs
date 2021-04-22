using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
   
    public CharacterController player;
    public Transform reciver;
    public Transform playerCamera;

    private bool playerIsOverlapping = false;


    // Update is called once per frame

    void Update()
    {
        
        if (playerIsOverlapping)
        {
          
            Vector3 portalToPlayer = player.transform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            //Si es true : el player traspaso el portal
            if(dotProduct < 0f)
            {
                Debug.Log("entra2");
                //Teleportar
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciver.rotation);
                if(rotationDiff >= 360)
                {
                    rotationDiff = 0;
                }
                rotationDiff += 90;
                player.transform.Rotate(Vector3.up, rotationDiff);
                Debug.Log("1: "+playerCamera.transform.rotation);
                playerCamera.transform.rotation=Quaternion.Euler(0f, 20f,0f);
                Debug.Log("2: " + playerCamera.transform.rotation);


                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.enabled = false;
                player.transform.position = reciver.position + positionOffset;
                player.enabled = true;

                playerIsOverlapping = false;
            }
        }
        //Debug.Log(player.position);


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}
