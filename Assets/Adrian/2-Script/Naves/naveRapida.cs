using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class naveRapida : MonoBehaviour
{
    public enum naveStates { Acercandose, Attack, Die }
    public naveStates naveState;

    [Header("Referencias")]
    [SerializeField] Transform destiny;
    [SerializeField] GameObject bala;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject tpVFX;
    [SerializeField] Transform posDisparo;
    [SerializeField] Transform zonatp;

    [Header("Parametros")]
    public int health;
    [SerializeField] float speedMovement;
    [SerializeField] float speedRotation;
    [SerializeField] float distanciaDestino;
    [SerializeField] float rangoAleatorio;
    [SerializeField] float cadenciaDisparo;
    [SerializeField] int puntosDados;

    private Transform player;
    private bool canMove;
    private bool canRotate;
    private bool auxMovimiento = true;
    private bool puedoDisparar = true;
    private bool auxMuerte;
   // LevelManager codigo;
    
    // Start is called before the first frame update
    void Start()
    {
        auxMuerte = true;

        rangoAleatorio = 40f + Random.Range(-10, 10);
        naveState = naveStates.Acercandose;

        player = GameObject.FindGameObjectWithTag("canon").transform;
      //  codigo = GameObject.FindGameObjectWithTag("Codigo").t;
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (naveState)
        {
            case naveStates.Acercandose:
                acercandose();
                break;
            case naveStates.Attack:
                attacking();
                break;
            case naveStates.Die:
                muerte();
                break;
        }

        if (canMove) move();
        if (canRotate) rotate();
        distanciaDestino = Vector3.Distance(transform.position, destiny.position);

        if (health <= 0) naveState = naveStates.Die;

       
    }
        void acercandose()
        {
            canMove = true;
            canRotate = true;

            destiny.position = GameObject.FindGameObjectWithTag("canon").transform.position;
             if (distanciaDestino <= rangoAleatorio)
             {
            zonatp.parent = null;
            naveState = naveStates.Attack;
             }
        }
        void attacking()
        {
            canMove = false;
            if (puedoDisparar) StartCoroutine(rafagaDisparo());
            if (auxMovimiento) StartCoroutine(movimientoAleatorio());
        }
        void move()
        {
            transform.position = Vector3.MoveTowards(transform.position, destiny.position, speedMovement * Time.deltaTime);
        }
        void rotate()
        {
            Quaternion lookDirection = Quaternion.LookRotation(player.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, speedRotation * Time.deltaTime);
        }
        void muerte()
        {

        if (auxMuerte)
        {
            LevelManager.puntos = LevelManager.puntos + puntosDados;
            auxMuerte = false;
        }

            //vfx
            GameObject vfxGo = Instantiate(explosionVFX, transform.position, transform.rotation);

            Destroy(gameObject, 0.1f);
            Destroy(vfxGo, 0.9f);

        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balafloja"))
        {
            health--;
        }
        if (other.CompareTag("balafuerte"))
        {
            naveState = naveStates.Die;
        }
    }
    IEnumerator rafagaDisparo()
    {
            puedoDisparar = false;
            //DISPARO
            GameObject go = Instantiate(bala, posDisparo.transform.position, posDisparo.transform.rotation);

            Debug.Log("disparo izquierda");
            //VFX
            //GameObject vfxgo = Instantiate(vfxDisparo, posDisparo.transform.position, posDisparo.transform.rotation);

            // Destroy(vfxgo, 1f);
            Destroy(go, 4f);
            yield return new WaitForSeconds(cadenciaDisparo);

            puedoDisparar = true;

    }
    IEnumerator movimientoAleatorio()
    {
       auxMovimiento = false;
       canMove = false;
       yield return new WaitForSeconds(4f);
        //VFX tp
       GameObject vfxtp = Instantiate(tpVFX, transform.position, Quaternion.identity);
       yield return new WaitForSeconds(2f);

       transform.position = new Vector3(zonatp.position.x + Random.Range(-3, 3), zonatp.position.y + Random.Range(-2, 2), zonatp.position.z + Random.Range(-3, 3));
       canMove = true;
       auxMovimiento = true;
    }
       
}
