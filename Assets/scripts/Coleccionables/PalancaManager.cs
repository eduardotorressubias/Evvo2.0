using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PalancaManager : MonoBehaviour
{
    public static PalancaManager instance;
    public TextMeshProUGUI text;
    public int score;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        text.text = "X" + score.ToString();
    }
}
