using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiedraMagica : MonoBehaviour
{
    public static int PiedraValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PiedraMagicaManager.instance.ChangeScore(PiedraValue);
        }
    }
}
