using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using SupraRealities.SupraSpace.Utilities.ObjectPoolPattern;

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
    [SerializeField] private PooledObject balafuerte;
    [SerializeField] private float cadenciaFuerte;
    [SerializeField] private GameObject vfxDisparoFuerte;

    private ObjectPool balasGrandesPool;
    
      

    [Header("BalaPequena")]
    [SerializeField] private PooledObject balafloja;
    [SerializeField] private float cadenciaFloja;
    [SerializeField] private GameObject vfxDisparoFlojo;

    private ObjectPool balasPequenasPool;



    [Header("Vibracion")]
    public float IzqODer;

    public float duracion;
    public float potencia;

    UnityEngine.XR.InputDevice rightHand;
    UnityEngine.XR.InputDevice leftHand;


    private bool puedoDisparar;

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

    private void Start()
    {
        puedoDisparar = true;
        balasGrandesPool = new ObjectPool(balafuerte);
        balasPequenasPool = new ObjectPool(balafloja);
    }

    private void Update()
    {
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
             
        if (inputActionReset.action.ReadValue<float>() > 0.8f)
        {
            //SceneManager.LoadScene("PosGame");
        }
    }
 
    private IEnumerator disparoflojo()
    {
        if (IzqODer == 0)
        {
            rightHand.SendHapticImpulse(0, potencia, duracion);
        }
        else
        {
            leftHand.SendHapticImpulse(0, potencia, duracion);
        }

        //DISPARO
        PooledObject balaPequenaInstance = balasPequenasPool.GetObject();
        balaPequenaInstance.transform.SetPositionAndRotation(posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        balaPequenaInstance.transform.localScale = new Vector3(4, 4, 4);
        puedoDisparar = false;

        //VFX
        GameObject vfxgo = Instantiate(vfxDisparoFlojo, posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        StartCoroutine(delayDisparo(vfxgo));

        yield return new WaitForSeconds(cadenciaFloja);
        puedoDisparar = true;
      
    }
    private IEnumerator disparofuerte()
    {
        if (IzqODer == 0)
        {
            rightHand.SendHapticImpulse(0, potencia, duracion);
        }
        else
        {
            leftHand.SendHapticImpulse(0, potencia, duracion);
        }

        //DISPARO
        PooledObject balaGrandeInstance = balasGrandesPool.GetObject();
        balaGrandeInstance.transform.SetPositionAndRotation(posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        puedoDisparar = false;

        //VFX
        GameObject vfxGoFuerte = Instantiate(vfxDisparoFuerte, posicionDisparo.transform.position, posicionDisparo.transform.rotation);
        StartCoroutine(delayDisparo(vfxGoFuerte));

        yield return new WaitForSeconds(cadenciaFuerte);
        puedoDisparar = true;
    }

    private IEnumerator delayDisparo(GameObject vfxgo)
    {        
        yield return new WaitForSeconds(0.8f);
        Destroy(vfxgo);
    }

    private IEnumerator delayDisparo2(GameObject vfxGoFuerte)
    {       
        yield return new WaitForSeconds(0.8f);
        Destroy(vfxGoFuerte);
    }

}
