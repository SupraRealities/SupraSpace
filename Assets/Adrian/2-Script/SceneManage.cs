using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SceneManage : MonoBehaviour
{
    [Header("ResetVista")]
    [SerializeField] Transform resetTransform;
    [SerializeField] GameObject player;
    [SerializeField] Camera playerHead;
    [SerializeField] InputActionReference inputActionReset;
    private float timer;

    [Space]

    [Header("Controlador de escenas")]
    [SerializeField] private GameObject naveInicial;
   
    public enum escenastates { Pre, Game, Post }
    public escenastates escenaState;
    [Space]
    [SerializeField] private PlayerHP playerScript;
    [Space]
    [SerializeField] private float tiempoPostGame;
    [SerializeField] private Animator fundidoAnim;

    [Space]

    [SerializeField] GameObject cartelResetVista;
    [SerializeField] GameObject cartelDisparaNave;


    [SerializeField] Text puntosFinal;

    private bool auxbool = true;
    
     void Start()
    {
        if(playerScript != null)playerScript = playerScript.GetComponent<PlayerHP>();
        timer = 0;

        if (escenaState == escenastates.Post)
        {
            if (Arduino.sp.IsOpen)
            {
                Arduino.sp.WriteLine("1");
            }
        }
              
    }
    // Update is called once per frame
    void Update()
    {
        switch (escenaState)
        {
             case escenastates.Pre:
                PreGame();
                break;
            case escenastates.Game:
                inGame();
                break;
            case escenastates.Post:
                if(auxbool) StartCoroutine(DelayPostGame());
                break;
        }


        if (inputActionReset != null && (inputActionReset.action.IsPressed()))
        {
            timer += Time.deltaTime;
            if(timer >= 1.5)
            {
            ResetPosition();

            }
            Debug.Log("Pulso boton");
        }
        else
        {
            timer = 0;
        }
    }
    void PreGame()
    {
        if(naveInicial != null)
        {
            naveInicial.GetComponent<NaveBasica>().naveState = NaveBasica.naveStates.PreGame;

            if(naveInicial.GetComponent<NaveBasica>().health <= 0)
            {
                StartCoroutine(fundidoNegro());
                
            }
        }
    }
    void inGame()
    {
        if(playerScript.vida <= 0)
        {
            SceneManager.LoadScene("PosGame");
        }
    }
    public void ResetPosition()
    {
        if (escenaState == escenastates.Pre)
        {
            cartelResetVista.SetActive(false);
            cartelDisparaNave.SetActive(true);
        }
        timer = 0f;
       
        float rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;

        player.transform.Rotate(xAngle: 0, rotationAngleY, zAngle: 0);
        Vector3 distanceDiff = resetTransform.position - player.transform.position;

        player.transform.position += distanceDiff;
    }

    IEnumerator fundidoNegro()
    {
        fundidoAnim.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
    IEnumerator DelayPostGame()
    {
        if (LevelManager.puntos <= 0)
        {
            LevelManager.puntos = 0;
        }
        puntosFinal.text = LevelManager.puntos.ToString();
        auxbool = false;
        Debug.Log("entro en la corroutina");
        yield return new WaitForSeconds(tiempoPostGame);
        
        SceneManager.LoadScene("PrePago");

    }
}
