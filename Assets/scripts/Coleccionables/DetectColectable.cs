using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectColectable : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    public float range;
    private bool isPlayerHere, pressF = false;
    public GameObject sprite;
    public GameObject portal;
    public GameObject errorPortal;
    private bool error= false;
    private float counterTime;
    public GameObject pieza1, pieza2;
    public bool palanca;
    public bool palanca2;
    public bool portalBoss;
    private bool portalBoss_acces= false;
    private PlayerAnimation puertaAnim;
    public Animator arbolAnim;
    public Animator portal_Boss;
    
    void Start()
    {
        puertaAnim = FindObjectOfType<PlayerAnimation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (portalBoss_acces)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= 1.3f)
            {
                portal.SetActive(true);
                counterTime = 0;
            }
        }

        if(error == true)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= 9f)
            {
                errorPortal.SetActive(false);
                counterTime = 0;
                error = false;
            }
        }
        isPlayerHere = Physics.CheckSphere(transform.position, range, whatIsPlayer);

        if (isPlayerHere)
        {
            if (!pressF)
            {
                sprite.SetActive(true);
            }

            if (Input.GetKey("f"))
            {
                sprite.SetActive(false);
                if (!palanca)
                {
                    PortalEnable();
                }
                else if(palanca)
                {
                    AbrePuerta();
                }
                else if (portalBoss){
                    AbreBoss();
                }

                pressF = true;
            }
        }
        else
        {
            sprite.SetActive(false);
           
            pressF = false;
        }
    }

    void PortalEnable()
    {
        if(ScoreManager.FindObjectOfType<ScoreManager>().score == 2)
        {
            portal.SetActive(true);
            pieza1.SetActive(true);
            pieza2.SetActive(true);
        }
        else
        {
            errorPortal.SetActive(true);
            error = true;
        }
    }

    void AbrePuerta()
    {
        if (PalancaManager.FindObjectOfType<PalancaManager>().score == 1)
        {
            if (palanca2)
            {
                arbolAnim.SetTrigger("abrir");
            }
            else
            {
                puertaAnim.OpenDoor();
            }


        }
        else if(PalancaManager.FindObjectOfType<PalancaManager>().score != 1)
        {
            errorPortal.SetActive(true);
            error = true;
        }
        

    }
    void AbreBoss()
    {
        if (PiedraMagicaManager.FindObjectOfType<PiedraMagicaManager>().score == 2)
        {

            
                portalBoss_acces = true;
                portal_Boss.SetTrigger("abrir");

            
        }

        else if (PiedraMagicaManager.FindObjectOfType<PiedraMagicaManager>().score != 2)
        {
            errorPortal.SetActive(true);
            error = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);


    }
}

