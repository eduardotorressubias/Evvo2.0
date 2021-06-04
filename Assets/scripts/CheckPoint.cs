using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    static bool save = false;
    private void Awake()
    {
    
            LoadFromPlayerPrefs();
   
    }

    void SaveToPlayerPrefs(string savePositionData, string saveRotationData)
    {
        // Saving data to PlayerPrefs
        PlayerPrefs.SetString(name + "_position", savePositionData);
        PlayerPrefs.SetString(name + "_rotation", saveRotationData);
    }

    public void LoadFromPlayerPrefs()
    {
        string loadPositionData = PlayerPrefs.GetString(name + "_position", "");
        string loadRotationData = PlayerPrefs.GetString(name + "_rotation", "");

        transform.position = JsonUtility.FromJson<Vector3>(loadPositionData);
        Physics.SyncTransforms();
        transform.rotation = JsonUtility.FromJson<Quaternion>(loadRotationData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "check")
        {

            guardado();
            save = true;
        }
    }

    void guardado()
    {
        string savePositionData = JsonUtility.ToJson(transform.position);
        string saveRotationData = JsonUtility.ToJson(transform.rotation);

        SaveToPlayerPrefs(savePositionData, saveRotationData);
    }
}
