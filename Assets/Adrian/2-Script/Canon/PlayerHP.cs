using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] float vidaPorcentaje;
    [SerializeField] float vidaMax;
    public float vida;

    [SerializeField] Text hpText;
    // Start is called before the first frame update
    void Start()
    {
        vida = vidaMax;
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = vidaPorcentaje.ToString("F0");
        vidaPorcentaje = (100f / vidaMax) * vida;
    }
    //danno = daño
   public void recibeDanno(float danno)
    {
       //Debug.Log("recibodanno");
       vida -= danno;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "balafloja")
        //{
        //    vida = vida - 30f;
        //}
    }
}
