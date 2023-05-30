using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaBasica : MonoBehaviour
{
    [SerializeField] float velocidadBala;
    [SerializeField] float speedRotation;
    [SerializeField] float dañoBala;

    Transform destiny;
    Vector3 direccion;
    PlayerHP playerHp;
    // Start is called before the first frame update
  
     void Awake()
    {
        destiny = GameObject.FindGameObjectWithTag("canon").transform;
       // playerHp = GameObject.FindGameObjectWithTag("canon").GetComponent<PlayerHP>();
        //Quaternion lookDirection = Quaternion.LookRotation(destiny.position+ new Vector3(Random.Range(-2,2), Random.Range(-2, 2), Random.Range(-2, 2)) - transform.position);
       // transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, speedRotation * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {        
        transform.position += transform.forward * Time.deltaTime * velocidadBala;       

    }

     void OnTriggerEnter(Collider other)
     {
        if (other.gameObject.CompareTag("canon"))
        {
            Debug.Log("le estoy dando al cañon con las balas");

        }
        if(other.gameObject.tag == "canon")
        {
           playerHp =  other.transform.gameObject.GetComponent<PlayerHP>();
           playerHp.recibeDanno(dañoBala);
           LevelManager.puntos = LevelManager.puntos - 5;
           Destroy(this.gameObject);
            
            //vfx daño
        }

        if(other.gameObject.tag == "balafloja")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
     }
}
