using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int puntos;
    [Header("Fases")]
    Fases fase;

    public Fases[] fases;

    [SerializeField] int faseCounter;

    [Header("Spawn")]
    public SpawnNaves spawn;

    [SerializeField]float timer;

    private PlayerHP playerHealth;

    [SerializeField] private Text puntosText;
     void Start()
    {
        puntos = 0;
        fase = fases[0];
        faseCounter = 0;
        timer = 0;
        playerHealth = GameObject.FindGameObjectWithTag("canon").GetComponent<PlayerHP>();

        spawn.list = fase.list;
    }
     void Update()
    {
        timer += Time.deltaTime;
        fase = fases[faseCounter];       
      
        if (fase.tiempoSiguienteFase > timer)
        {
            
        }
        else
        {            
           checkFase();
        }

        puntosText.text = puntos.ToString();
        //Debug.Log(fase.myName);
        //Debug.Log(puntos);
    }

    void checkFase()
    {
        timer = 0;
        if (faseCounter < fases.Length - 1)
        {
            faseCounter++;
        spawn.list = fase.list ;
        }
        else
        {
            SceneManager.LoadScene("PosGame");
            Debug.Log("Se acabó");
        }
        
      
    }    
}
