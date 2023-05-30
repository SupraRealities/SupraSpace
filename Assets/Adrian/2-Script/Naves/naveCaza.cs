using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class naveCaza : MonoBehaviour
{
    public enum naveStates { Acercandose, Attack, Die, Floating, PreGame }
    public naveStates naveState;

    [Header("Referencias")]

    [SerializeField] Transform posDisparo;
    [SerializeField] Transform posDisparo2;
    [SerializeField] GameObject bala;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject vfxDisparo;
        

    [Header("Parametros")]
    public int health;
    [SerializeField] float speedRotation;
    [SerializeField] float cadenciaDisparo;
    [SerializeField] float startDelayDisparo;
    [SerializeField] float speedMovement;
    [SerializeField] float distanciaPlayer;
    [SerializeField] int puntosDados;
     
   [SerializeField] private Transform player;

    private bool puedoDisparar;
    private bool puedoDispararIzq = true;
    private bool puedoDispararDer = true;
    private bool canRotate = true;
    private bool canMove = true;

    private float rangoDisparo;
    private bool auxMuerte = true;
    private bool auxDelayAttack = true;
    private bool auxDelayFloating = true;
    private bool offsetRotacion = true;

    float distanceTravelled;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("canon").transform;

        naveState = naveStates.Acercandose;

        StartCoroutine(StartDelayDisparo());
        rangoDisparo = 10f + Random.Range(10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        switch (naveState)
        {
            case naveStates.Attack:
                Atacando();
                break;
            case naveStates.Die:
                Death();
                break;
            case naveStates.Floating:
                floating();
                break;
            case naveStates.Acercandose:
                acercandose();
                break;
            case naveStates.PreGame:
                preGame();
                break;
        }

        distanciaPlayer = Vector3.Distance(transform.position, player.position);

        if (canRotate) Rotation();

        if (puedoDisparar) Disparo();

        if (canMove) Moving();

        if (health <= 0)
        {
            naveState = naveStates.Die;
        }
    }
    void preGame()
    {
        canMove = false;
        canRotate = false;
        puedoDisparar = false;

    }
    void acercandose()
    {
        canMove = true;
        canRotate = true;

        if (distanciaPlayer <= rangoDisparo)
        {           
            naveState = naveStates.Attack;
        }
    }
    void Atacando()
    {
        puedoDisparar = true;
        canMove = false;
        canRotate = true;
        offsetRotacion = true;

        if (auxDelayAttack) StartCoroutine(tiempoAtacando());

    }
    void floating()
    {
        canMove = false;
        canRotate = false;
        puedoDisparar = false;
              

        if (auxDelayFloating) StartCoroutine(tiempoVolando());
    }
    void Death()
    {
        canMove = false;
        canRotate = false;
        puedoDisparar = false;
        if (auxMuerte) StartCoroutine(muerte());
        Destroy(gameObject, 1f);
    }

    void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speedMovement * Time.deltaTime);
      //  transform.parent.position = Vector3.MoveTowards(transform.parent.position, player.position, speedMovement * Time.deltaTime);
    }

    void Rotation()
    {
        Quaternion lookDirection = Quaternion.LookRotation(player.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, speedRotation * Time.deltaTime);
    }

    void Disparo()
    {
        if (puedoDispararIzq) StartCoroutine(disparo());
        if (puedoDispararDer) StartCoroutine(disparo2());
    }

    //Delay para que no dispare desde el principio
    IEnumerator StartDelayDisparo()
    {
        puedoDisparar = false;
        puedoDispararDer = false;

        yield return new WaitForSeconds(startDelayDisparo);

        puedoDisparar = true;
        puedoDispararDer = true;
    }

    IEnumerator disparo()
    {
        puedoDispararIzq = false;
        //DISPARO
        GameObject go = Instantiate(bala, posDisparo.transform.position, posDisparo.transform.rotation);


        //VFX
        GameObject vfxgo = Instantiate(vfxDisparo, posDisparo.transform.position, posDisparo.transform.rotation);

        Destroy(vfxgo, 1f);
        Destroy(go, 4f);

        yield return new WaitForSeconds(cadenciaDisparo);
        puedoDispararIzq = true;
    }
    IEnumerator disparo2()
    {
        puedoDispararDer = false;
        //DISPARO
        GameObject go2 = Instantiate(bala, posDisparo2.transform.position, posDisparo2.transform.rotation);


        //VFX
        GameObject vfxgo = Instantiate(vfxDisparo, posDisparo2.transform.position, posDisparo2.transform.rotation);

        Destroy(vfxgo, 1f);
        Destroy(go2, 4f);

        yield return new WaitForSeconds(cadenciaDisparo);
        puedoDispararDer = true;
    }
    IEnumerator tiempoAtacando()
    {
        auxDelayAttack = false;
        yield return new WaitForSeconds(Random.Range(4f, 6f));
        naveState = naveStates.Floating;
        auxDelayAttack = true;
    }
    IEnumerator tiempoVolando()
    {
        auxDelayFloating = false;
       
        offsetRotacion = false;
        yield return new WaitForSeconds(0.5f);
        naveState = naveStates.Attack;
        auxDelayFloating = true;
    }
    IEnumerator muerte()
    {
        LevelManager.puntos = LevelManager.puntos + puntosDados;
        auxMuerte = false;
        GameObject vfxGo = Instantiate(explosionVFX, transform.position, transform.rotation);

        Destroy(gameObject, 0.1f);
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balafloja") )
        {
            health--;
            
        }

        if (other.CompareTag("naveAlien")|| other.CompareTag("balafuerte"))
        {
            naveState = naveStates.Die;
        }
    }
}
