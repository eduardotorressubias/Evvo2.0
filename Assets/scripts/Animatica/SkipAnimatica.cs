using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipAnimatica : MonoBehaviour
{
    private float timeCounter=0f, timeCounter2=0f;
    private float start = 2f;
    public GameObject skip;
    public Animator animator;
    private bool load= false;

    
    private void Start()
    {
        skip.SetActive(false);
        load = false;
    }
    void Update()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= start)
        {
            skip.SetActive(true);
            
            if (Input.anyKeyDown)
            {
                animator.SetTrigger("Out");

                load = true;

            }
        }
        if(load == true)
        {
            timeCounter2 += Time.deltaTime;

            if (timeCounter2 >= 1f)
            {
                
                LoadGameScene();
                timeCounter = 0;
                timeCounter2 = 0;
            }
        }
    }
    
    void LoadGameScene()
    {
        
        SceneManager.LoadScene("GameScene");
    }
  


}
