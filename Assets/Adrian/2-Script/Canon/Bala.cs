using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] float velocidadBala;
    [SerializeField] GameObject vfxImpact;
    //[SerializeField] GameObject vfxExplosion;

    private AudioSource myAudio;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();   
    }  

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * velocidadBala;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("enemigos"))
        {
            Debug.Log("layer enemigos");
            GameObject vfxImpactGO = Instantiate(vfxImpact, transform.position, Quaternion.identity);
            Destroy(vfxImpactGO, 1f);

            Destroy(this.gameObject);
        }
    }
}
