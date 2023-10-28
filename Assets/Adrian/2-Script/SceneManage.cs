using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SceneManage : MonoBehaviour
{
    [Header("View's reset")]
    [SerializeField] private Transform resetTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerHead;
    [SerializeField] private InputActionReference inputActionReset;
    [SerializeField] private float pressingTimeRequiredToResetView = 1.5f;
    private float currentPassedTimePressingReset;

    [Space]

    [Header("Controlador de escenas")]
    [SerializeField] private GameObject naveInicial;
   
    public enum SceneStates { Pre, Game, Post }
    public SceneStates escenaState;

    [Space]
    [SerializeField] private PlayerHP playerHP;

    [Space]
    [SerializeField] private float delayBeforeLoadPrePago;
    [SerializeField] private Animator fundidoAnim;

    [Space]
    [SerializeField] private GameObject cartelResetVista;
    [SerializeField] private GameObject cartelDisparaNave;

    [SerializeField] private Text totalScoreText;

    private bool resultsScreenAlreadyLoading;
    
    private void Start()
    {
        if (playerHP != null)
        {
            playerHP = playerHP.GetComponent<PlayerHP>();
        }
        currentPassedTimePressingReset = 0;

        if (escenaState == SceneStates.Post)
        {
            if (Arduino.sp.IsOpen)
            {
                Arduino.sp.WriteLine("1");
            }
        }
    }

    private void Update()
    {
        switch (escenaState)
        {
             case SceneStates.Pre:
                PreGameUpdate();
                break;
            case SceneStates.Game:
                InGameUpdate();
                break;
            case SceneStates.Post:
                PostGameUpdate();
                break;
        }

        ManageViewportReset();
    }

    private void PreGameUpdate()
    {
        if (naveInicial != null)
        {
            naveInicial.GetComponent<NaveBasica>().naveState = NaveBasica.naveStates.PreGame;

            if (naveInicial.GetComponent<NaveBasica>().health <= 0)
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    private IEnumerator FadeOut()
    {
        fundidoAnim.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

    private void InGameUpdate()
    {
        if (playerHP.vida <= 0)
        {
            SceneManager.LoadScene("PosGame");
        }
    }

    private void PostGameUpdate()
    {
        if (resultsScreenAlreadyLoading)
        {
            return;
        }

        resultsScreenAlreadyLoading = true;
        ShowGameResults();
        StartCoroutine(ReturnToPrePagoAfterDelay());
    }

    private void ShowGameResults()
    {
        if (LevelManager.puntos <= 0)
        {
            LevelManager.puntos = 0;
        }
        totalScoreText.text = LevelManager.puntos.ToString();
    }

    private IEnumerator ReturnToPrePagoAfterDelay()
    {
        Debug.Log("entro en la corroutina");
        yield return new WaitForSeconds(delayBeforeLoadPrePago);

        SceneManager.LoadScene("PrePago");
    }

    private void ManageViewportReset()
    {
        if (inputActionReset != null && (inputActionReset.action.IsPressed()))
        {
            currentPassedTimePressingReset += Time.deltaTime;
            if (currentPassedTimePressingReset >= pressingTimeRequiredToResetView)
            {
                ResetPosition();
            }
            Debug.Log("Pulso boton");
        }
        else
        {
            currentPassedTimePressingReset = 0;
        }
    }

    public void ResetPosition()
    {
        if (escenaState == SceneStates.Pre)
        {
            cartelResetVista.SetActive(false);
            cartelDisparaNave.SetActive(true);
        }
        currentPassedTimePressingReset = 0f;
       
        //float rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;

        //player.transform.Rotate(xAngle: 0, rotationAngleY, zAngle: 0);
        Vector3 distanceDiff = resetTransform.position - player.transform.position;

        player.transform.position += distanceDiff;
    }
}
