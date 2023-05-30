using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNaves : MonoBehaviour
{
    public List<GameObject> list;
 
    [SerializeField] float cadenciaSpawn;

    private float auxRandom;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 2.0f, cadenciaSpawn);
    }

    // Update is called once per frame

    void Spawn()
    {        
        if (list[0] != null)
        {
            GameObject naveGO = Instantiate(list[Random.Range(0, list.Count)], new Vector3(transform.position.x + Random.Range(-40, 40), transform.position.y + Random.Range(-5, 45), transform.position.z + Random.Range(-30, 30)), transform.rotation); ;
        
        }     
        
    }
}
