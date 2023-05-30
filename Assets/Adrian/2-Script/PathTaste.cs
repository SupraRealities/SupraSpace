using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathTaste : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5;
    float distanceTravelled;

    private bool aux = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aux)
        {
            seguir();
            StartCoroutine(delayy());
        }
       
    }
    void seguir()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
    IEnumerator delayy()
    {
        yield return new WaitForSeconds(1f);
        aux = false;
        yield return new WaitForSeconds(2f);
        aux = true;
    }
}
