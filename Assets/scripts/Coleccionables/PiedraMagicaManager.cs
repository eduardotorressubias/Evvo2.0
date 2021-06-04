using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PiedraMagicaManager : MonoBehaviour
{
    public static PiedraMagicaManager instance;
    public TextMeshProUGUI text;
    public int score;

    public static bool piedra = false;
    void Start()
    {
        if(piedra)
        {
            score = 1; 
        }
        text.text = "X" + score.ToString();

        if (instance == null)
        {
            instance = this;
        }
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;

        if (score == 1)
        {
            piedra = true;
        }

        text.text = "X" + score.ToString();
    }
}
