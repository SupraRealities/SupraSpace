using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nave : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform puntoOrigen, puntoRebote;
    [SerializeField] GameObject vfxExplosion;

    [Header("Parametros")]
    [SerializeField] float speedMovement;
    [SerializeField] bool haRebotado;
    [SerializeField] int healInicial;

    private float distanciaDestino;
    private Transform destino;
    private int heal;
    
   
    private void Awake()
    {
        puntoOrigen = GameObject.Find("Origen").transform;
        puntoRebote = GameObject.Find("Rebote").transform;

        heal = healInicial;
    }

    // Update is called once per frame
    void Update()
    {
        if(distanciaDestino < 1f)
        {
            if (haRebotado)
            {
                haRebotado = false;
            }
            else
            {
                haRebotado = true;
            }
        }
        if (haRebotado)
        {
            destino = puntoOrigen;
        }
        else
        {
            destino = puntoRebote;
        }
        transform.position = Vector3.MoveTowards(transform.position, destino.position, speedMovement * Time.deltaTime);
        distanciaDestino = Vector3.Distance(transform.position, destino.position);
        
        if(heal <= 0f)
        {
            GameObject go = Instantiate(vfxExplosion, transform.position, transform.rotation);
            Destroy(go.gameObject, 1f);

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      
        Destroy(other.gameObject);
        

        if(other.tag == "balafuerte")
        {
        heal = heal - 2;

        }
        else if(other.tag == "balafloja")
        {
            heal--;
        }
    }
}
