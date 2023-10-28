using SupraRealities.SupraSpace.Utilities.ObjectPoolPattern;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Fase", menuName = "Fase")]
public class StageData : ScriptableObject
{
    public List<PooledObject> list;
    //public GameObject nave;
    //public GameObject nave2;
    public string stageName;

    public float tiempoSiguienteFase;

    //public float probNav;
    //public float probNav2;
    
}
