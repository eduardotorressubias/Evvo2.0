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
    public GameObject pajaros, pajaros2;
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
            if (night.weight >= 0.99f)
            {
                night.weight = 1f;
                pajaros.SetActive(false);
                pajaros.SetActive(true);
                pajaros2.SetActive(false);
                pajaros2.SetActive(true);
                nightOn = true;
            }
            else if (night.weight <= 0.01)
            {
                pajaros.SetActive(false);
                pajaros.SetActive(true);
                pajaros2.SetActive(false);
                pajaros2.SetActive(true);
                night.weight = 0f;
                nightOn = false;
            }
            if (nightOn)
            {
                if (frames == 49)
                {
                    night.weight = numeroResta / transitionTime;
                    frames = 0;
                }
                
            }
            else
            {
                if (frames == 49)
                {
                    night.weight = numero / transitionTime;
                    frames = 0;
                }
            }
        }
    }
}
