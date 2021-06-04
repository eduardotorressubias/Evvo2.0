using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static bool save = false;
    private void Awake()
    {
    
            LoadFromPlayerPrefs();
   
    }

    void SaveToPlayerPrefs(string savePositionData, string saveRotationData/*, string savePuerta*/)
    {
        // Saving data to PlayerPrefs
        PlayerPrefs.SetString(name + "_position", savePositionData);
        PlayerPrefs.SetString(name + "_rotation", saveRotationData);
        //PlayerPrefs.SetString(name + "_bool_Puerta", savePuerta);
       // Debug.Log(savePuerta);

    }

    public void LoadFromPlayerPrefs()
    {
        string loadPositionData = PlayerPrefs.GetString(name + "_position", "");
        string loadRotationData = PlayerPrefs.GetString(name + "_rotation", "");
        //string loadPuerta1 = PlayerPrefs.GetString(name + "_bool_Puerta", "");
        

        //PlayerAnimation.open = JsonUtility.FromJson<bool>(loadPuerta1);
        transform.position = JsonUtility.FromJson<Vector3>(loadPositionData);
        Physics.SyncTransforms();
        transform.rotation = JsonUtility.FromJson<Quaternion>(loadRotationData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "check")
        {
            CheckPoint.save = true;
            guardado();
            
        }
    }

    void guardado()
    {
        string savePositionData = JsonUtility.ToJson(transform.position);
        string saveRotationData = JsonUtility.ToJson(transform.rotation);
        //string savePuerta = JsonUtility.ToJson(PlayerAnimation.open);
        //if (PlayerAnimation.open)
        //{
        //    string open = ""
        //}
        //else
        //{

        //}

        SaveToPlayerPrefs(savePositionData, saveRotationData/*, savePuerta*/);
    }
}
