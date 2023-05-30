using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] InputActionReference inputActionL;
    [SerializeField] InputActionReference inputActionR;
    [SerializeField] InputActionReference inputActionSpace;
    [SerializeField] InputActionReference inputActionReset;
    [SerializeField] InputActionReference inputActionJoystick;
    [SerializeField] InputActionReference inputActionJoystickX;
    [SerializeField] InputActionReference inputActionJoystickY;
    [SerializeField] Transform posicionDisparo;
    [SerializeField] GameObject mandoDer;
    private AudioSource myAudio;

    [Header("BalaGrande")]
    [SerializeField] GameObject balafuerte;
    [SerializeField] float cadenciaFuerte;
    [SerializeField] GameObject vfxDisparoFuerte;
    
      

    [Header("BalaPequena")]
    [SerializeField] GameObject balafloja;
    [SerializeField] float cadenciaFloja;
    [SerializeField] GameObject vfxDisparoFlojo;
         
        

    [Header("Vibracion")]
    public float IzqODer;

    public float duracion;
    public float potencia;

    UnityEngine.XR.InputDevice rightHand;
    UnityEngine.XR.InputDevice leftHand;


    private bool puedoDisparar;
    
   
    void Start()
    {
        puedoDisparar = true;
       
     // myAudio = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;
        InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;
        var rightControllers = new List<UnityEngine.XR.InputDevice>();
        var leftControllers = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics, rightControllers);
        InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, leftControllers);

        if (IzqODer == 0)
        {
            if (rightControllers.Count > 0)
            {

                rightHand = rightControllers[0];
                rightHand.SendHapticImpulse(0, 0.5f, 1.0f);
            }
        }
        else
        {
            if (leftControllers.Count > 0)
            {
                print("Left hand found");
                leftHand = leftControllers[0];
                rightHand.SendHapticImpulse(0, 0.5f, 1.0f);
            }
        }
    }  

    void Update()
    {
        //if (inputActionJoystick.action.triggered)
        //{
        //    if (puedoDisparar) StartCoroutine(disparoflojo());
        //}
        //Debug.Log("X" + inputActionJoystickX.action.ReadValue)
        //if ()
        //{
        //    if (puedoDisparar) StartCoroutine(disparoflojo());
        //}


      //  DisparoFlojo
        if (inputActionR.action.ReadValue<float>() > 0.8f || inputActionSpace.action.ReadValue<float>() > 0.8f)
        {

            if (puedoDisparar) StartCoroutine(disparoflojo());

        }
        //DisparoFuerte
        if (inputActionL.action.ReadValue<float>() > 0.8f || inputActionSpace.action.ReadValue<float>() > 0.8f)
        {         
            if (puedoDisparar) StartCoroutine(disparofuerte());            
        }
             
        if(inputActionReset.action.ReadValue<float>() > 0.8f)
        {
            //SceneManager.LoadScene("PosGame");
        }
    }
 
    IEnumerator disparoflojo()
    {
        if (IzqODer == 0)
            rightHand.SendHapticImpulse(0, potencia, duracion);
        else
            leftHand.SendHapticImpulse(0, potencia, duracion);

        //DISPARO
        GameObject go = Instantiate(balafloja, posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        puedoDisparar = false;
        go.transform.localScale = new Vector3(4, 4, 4);
        //VFX
        GameObject vfxgo = Instantiate(vfxDisparoFlojo, posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        StartCoroutine(delayDisparo(vfxgo));
            
        StartCoroutine(destruirBala(go));

        yield return new WaitForSeconds(cadenciaFloja);
        puedoDisparar = true;
      
    }
    IEnumerator disparofuerte()
    {
        if (IzqODer == 0)
            rightHand.SendHapticImpulse(0, potencia, duracion);
        else
            leftHand.SendHapticImpulse(0, potencia, duracion);

        //DISPARO
        GameObject go = Instantiate(balafuerte, posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        puedoDisparar = false;
        //VFX
        GameObject vfxGoFuerte = Instantiate(vfxDisparoFuerte, posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        StartCoroutine(delayDisparo(vfxGoFuerte));
                 

        StartCoroutine(destruirBalaGrande(go));

        yield return new WaitForSeconds(cadenciaFuerte);
        puedoDisparar = true;

    }
    IEnumerator destruirBala(GameObject go)
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(go);
    }
    IEnumerator destruirBalaGrande(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        Destroy(go);
    }
    IEnumerator delayDisparo(GameObject vfxgo)
    {        
        yield return new WaitForSeconds(0.8f);
        Destroy(vfxgo);
        
    }
    IEnumerator delayDisparo2(GameObject vfxGoFuerte)
    {       
        yield return new WaitForSeconds(0.8f);
        Destroy(vfxGoFuerte);

    }

}
