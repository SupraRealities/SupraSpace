using SupraRealities.SupraSpace.Utilities.ObjectPoolPattern;
using System.Collections;
using UnityEngine;

public class naveRapida : PooledObject
{
    public enum naveStates { Acercandose, Attack, Die }
    public naveStates naveState;

    [Header("Referencias")]
    [SerializeField] private Transform destiny;
    [SerializeField] private GameObject bala;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject tpVFX;
    [SerializeField] private Transform posDisparo;
    [SerializeField] private Transform zonatp;
    [SerializeField] private PooledEnemy thisEnemy;

    [Header("Parametros")]
    public int health;
    [SerializeField] private float speedMovement;
    [SerializeField] private float speedRotation;
    [SerializeField] private float distanciaDestino;
    [SerializeField] private float rangoAleatorio;
    [SerializeField] private float cadenciaDisparo;
    [SerializeField] private int puntosDados;

    private Transform player;
    private bool canMove;
    private bool canRotate;
    private bool auxMovimiento = true;
    private bool puedoDisparar = true;
    private bool auxMuerte;

    private void Start()
    {
        auxMuerte = true;

        rangoAleatorio = 40f + Random.Range(-10, 10);
        naveState = naveStates.Acercandose;

        player = GameObject.FindGameObjectWithTag("canon").transform;
    }

    private void Update()
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

    private void acercandose()
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

    private void attacking()
    {
        canMove = false;
        if (puedoDisparar) StartCoroutine(rafagaDisparo());
        if (auxMovimiento) StartCoroutine(movimientoAleatorio());
    }

    private void move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destiny.position, speedMovement * Time.deltaTime);
    }
    private void rotate()
    {
        Quaternion lookDirection = Quaternion.LookRotation(player.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, speedRotation * Time.deltaTime);
    }
    private void muerte()
    {
        if (auxMuerte)
        {
            LevelManager.puntos = LevelManager.puntos + puntosDados;
            auxMuerte = false;
        }

        //vfx
        GameObject vfxGo = Instantiate(explosionVFX, transform.position, transform.rotation);

        Invoke("RecycleThisEnemy", 0.1f);
        Destroy(vfxGo, 0.9f);
    }

    private void RecycleThisEnemy()
    {
        thisEnemy.Recycle();
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

    private IEnumerator rafagaDisparo()
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
