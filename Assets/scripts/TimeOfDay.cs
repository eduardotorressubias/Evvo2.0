using System.Collections;
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
    private float numero = 0;
    private float numeroResta = 0;
    private float frames = 0;



    private void Start()
    {
        numeroResta = transitionTime;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        counterTime += Time.deltaTime;
        numero += 0.02f;
        numeroResta -= 0.02f;
        frames = frames + 1;

        if (numero >= transitionTime)
        {
            numero = 0;
        }
        if (numeroResta <= 0)
        {
            numeroResta = transitionTime;
        }


        if (counterTime>= transitionTime)
        {
            counterTime = 0;
        }
        else
        {
            if (night.weight >= 0.9)
            {
                nightOn = true;
            }
            else if (night.weight <= 0.1)
            {
                nightOn = false;
            }
            if (nightOn)
            {
                Debug.Log("frames: " + frames);
                if (frames == 49)
                {
                    night.weight = numeroResta / transitionTime;
                    Debug.Log("numero resta " + numeroResta);
                    frames = 0;
                }
                
            }
            else
            {
                if (frames == 49)
                {
                    Debug.Log("numero " + numero);
                    night.weight = numero / transitionTime;
                    frames = 0;
                }
                

            }

            
        }
       
    }
}
