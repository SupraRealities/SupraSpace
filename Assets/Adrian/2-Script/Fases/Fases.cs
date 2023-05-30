using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "Nueva Fase", menuName = "Fase")]

public class Fases : ScriptableObject
{
    public List<GameObject> list;
    //public GameObject nave;
    //public GameObject nave2;
    public string myName;

    public float tiempoSiguienteFase;

    //public float probNav;
    //public float probNav2;
    
}
