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
    private float frames = 0;

   

    // Update is called once per frame
    void FixedUpdate()
    {
        counterTime += Time.deltaTime;
        numero += 0.02f;

        frames = frames + 1;
      
        
        
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
                if (frames == 50)
                {
                    night.weight = numero / transitionTime; 
                    Debug.Log("numero wuau " + numero);
                    frames = 0;
                }
                
            }
            else
            {
               
                if (frames == 50)
                {
                    Debug.Log("numero " + numero);
                    night.weight = numero / transitionTime;
                    frames = 0;
                }
                

            }

            
        }
       
    }
}
