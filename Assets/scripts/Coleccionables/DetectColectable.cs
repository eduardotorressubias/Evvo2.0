﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectColectable : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    public float range;
    private bool isPlayerHere, pressF = false;
    public GameObject sprite;
    public GameObject portal;
    public GameObject errorPortal;
    private bool error= false;
    private float counterTime;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(error == true)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= 9f)
            {
                errorPortal.SetActive(false);
                counterTime = 0;
                error = false;
            }
        }
        isPlayerHere = Physics.CheckSphere(transform.position, range, whatIsPlayer);

        if (isPlayerHere)
        {
            if (!pressF)
            {
                sprite.SetActive(true);
            }

            if (Input.GetKey("f"))
            {
                sprite.SetActive(false);
                PortalEnable();
                

                pressF = true;
            }


        }
        else
        {
            sprite.SetActive(false);
           
            pressF = false;
        }
    }

    void PortalEnable()
    {
        if(ScoreManager.FindObjectOfType<ScoreManager>().score == 2)
        {
            portal.SetActive(true);
        }
        else
        {
            errorPortal.SetActive(true);
            error = true;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);


    }
}

