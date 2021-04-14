﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class TimeOfDay : MonoBehaviour
{
    [SerializeField] Volume day;
    [SerializeField] Volume night;

    private float counterTime;
    public float transitionTime;
    public bool nightOn = false;

   

    // Update is called once per frame
    void Update()
    {
        counterTime += Time.deltaTime;
        Debug.Log(counterTime);
       if(counterTime>= transitionTime)
        {
            counterTime = 0;
        }
        else
        {
            if (night.weight >= 1)
            {
                nightOn = true;
            }
            else if (night.weight <= 0)
            {
                nightOn = false;
            }
            if (nightOn)
            {
                night.weight = night.weight + (counterTime * -0.00001f);
            }
            else
            {
                night.weight = night.weight + (counterTime * 0.00001f);
            }

            //day.weight = day.weight+(counterTime * - 0.01f);
            //night.weight = night.weight+(counterTime * 0.01f);
        }
       
    }
}