using SupraRealities.SupraSpace.Utilities.ObjectPoolPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : PooledObject
{
    [SerializeField] private GameObject vfxImpact;
    [SerializeField] private float velocidadBala;
    [SerializeField] private float tiempoDeVida = 0.7f;

    private AudioSource myAudio;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();   
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * velocidadBala;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemigos"))
        {
            Debug.Log("layer enemigos");
            GameObject vfxImpactGO = Instantiate(vfxImpact, transform.position, Quaternion.identity);
            Destroy(vfxImpactGO, 1f);

            Recycle();
        }
    }

    internal override void OnTakenFromPool()
    {
        base.OnTakenFromPool();
        Invoke("Recycle", tiempoDeVida);
    }
}
