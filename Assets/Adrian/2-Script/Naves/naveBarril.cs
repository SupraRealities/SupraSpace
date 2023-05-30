using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class naveBarril : MonoBehaviour
{
    public enum naveStates { desplazandose,kamikaze, Die}
    public naveStates naveState;

    [Header("Referencias")]
           
    [SerializeField] GameObject explosionGO;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject explosionSFX;

    [Header("Parametros")]
    public int health;
    [SerializeField] float speedRotation;
    [SerializeField] float radioExplosion;
    [SerializeField] float pausaKamikaze;
    [SerializeField] float distanciaKamikaze;
        
    [SerializeField] float speedMovement;
    [SerializeField] int puntosDados;
       
    private Transform player;
    private Transform destino;
    private bool puedoDisparar ;
    private bool puedoDisparar2 ;
    private bool canRotate = true;
    private bool canMove = true;
    private bool haRebotado = true;
    private float distanciaDestino;
    private bool auxMuerte = true;
    private bool auxKamikaze = true;
    private SphereCollider myColider;

    private Animator myAnim;
     
    private void Awake()
    {
        naveState = naveStates.desplazandose;

        player = GameObject.FindGameObjectWithTag("canon").transform;
        myColider = GetComponent<SphereCollider>();
        myAnim = GetComponent<Animator>();

        explosionGO.SetActive(false);
        distanciaDestino = 100f;      
    }
   
    // Update is called once per frame
    void Update()
    {
        switch (naveState)
        {
            case naveStates.desplazandose:
                Atacando();
                break;
            case naveStates.Die:
                if (auxMuerte) StartCoroutine(muerte());
                break;
            case naveStates.kamikaze:
                Kamikaze();
                break;
        }              

        if (canRotate) Rotation();
                
        if (canMove) Moving();
        distanciaDestino = Vector3.Distance(transform.position, player.position);
    }

    void Atacando()
    {
        canRotate = true;       
        canMove = true;

        if (health <= 0) naveState = naveStates.Die;
        if (distanciaDestino <= distanciaKamikaze) naveState = naveStates.kamikaze;
        
    }
    void Kamikaze()
    {
        if (auxKamikaze) StartCoroutine(delayKamikaze());

        if (distanciaDestino <= 5f) naveState = naveStates.Die;
        if (health <= 0) naveState = naveStates.Die;
    }
   
    void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speedMovement * Time.deltaTime);
    }

    void Rotation()
    {
        Quaternion lookDirection = Quaternion.LookRotation(player.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, speedRotation * Time.deltaTime);
    }     
   IEnumerator delayKamikaze()
    {
        auxKamikaze = false;
        canMove = false;
        yield return new WaitForSeconds(pausaKamikaze);
        myAnim.Play("Parpadeo_Bomba");
                //vfx velocidad
       
        canMove = true;
        speedMovement = speedMovement * 2f;

    }
    IEnumerator muerte()
    {
        LevelManager.puntos = LevelManager.puntos + puntosDados;
        canMove = false;
        canRotate = false;
        Destroy(gameObject,0.2f);

        explosionGO.SetActive(true);
        auxMuerte = false;
        explosionGO.GetComponent<Animator>().Play("scaleExplosion");

        myColider.radius = radioExplosion;
        GameObject vfxGo = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(vfxGo, 2f);
        GameObject sfxGO = Instantiate(explosionSFX, transform.position, transform.rotation);
        Destroy(sfxGO, 2f);
        //gameObject.SetActive(false);
        
        yield return null;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balafloja"))
        {            
            myAnim.Play("Hurt_Barril"); // posible bugg de pararse 
            health--;
            Destroy(other.gameObject);            
        }
        if (other.CompareTag("balafuerte"))
        {
            naveState = naveStates.Die;
        }

        if (other.CompareTag("canon"))
        {
            other.gameObject.GetComponent<PlayerHP>().recibeDanno(30);
        }
    }
}
