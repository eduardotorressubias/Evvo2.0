using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    static public Transform [] checkpoint;
    public static int check = 0;
    void Awake()
    {
        for (int i = 0; i < checkpoint.Length; i++)
        {
            
            if (PlayerPrefs.GetInt("CheckPoint") == i)
            {
                transform.position = checkpoint[i].position;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "check")
        {
            Debug.Log("hola");
            PlayerPrefs.SetInt("CheckPoint", check);
            check++;
        }

    }
}
