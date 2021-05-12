using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditTimer : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeLoading = 28f;

    [SerializeField]
    private string sceneNameToLoad;

    private float timeElapsed;
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > delayBeforeLoading)
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
