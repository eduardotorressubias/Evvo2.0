using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvvoJump : MonoBehaviour
{
    //en estre script miramos si existe un colider con tag slidedown, si lo hay desactivara la funcion slowdown de playercotroller,
    //lo que nos permitira subir escaleras etc
    private PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "slidedown" || other.tag =="Scene")
        {
           // Debug.Log("colapsa");
            player.slope = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "slidedown" || other.tag == "Scene")
        {
           // Debug.Log("colapsa");
            player.slope = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "slidedown" || other.tag == "Scene")
        {
           // Debug.Log("hola");
            player.slope = true;
        }
    }

}
