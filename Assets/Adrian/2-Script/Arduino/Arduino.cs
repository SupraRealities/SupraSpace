using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{
    static public SerialPort sp ;
    public int activo = 0;
    public int activoNecesario;

    public string numeroCom;

    

    //public GameObject texto;
    // Start is called before the first frame update
    void Start()
    {
        sp = new SerialPort(numeroCom, 9600);
       
        // sp = new SerialPort("COM3", 9600);
        
        activo = 0;
        activoNecesario = activo + 1;
        if (!sp.IsOpen)
        {
            sp.Open();
            Debug.Log("spopen");
        }
       
        // sp.ReadTimeout = 1;
        //texto.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
        if (sp.IsOpen)
        {
            //string data = sp.ReadLine();
            //Debug.Log(data);
            try
            {
               // print(sp.ReadLine());
                activo = int.Parse(sp.ReadLine());



            }
            catch (System.Exception)
            {


            }
        }
        if (activo == activoNecesario)
        {
           // texto.SetActive(true);
            StartCoroutine(aumentoActivoNecesario());
            SceneManager.LoadScene("PreGame");

        }

    }
  
    IEnumerator aumentoActivoNecesario()
    {
        yield return new WaitForSeconds(0);
        activoNecesario = activo + 1;
        yield return new WaitForSeconds(1f);
        //texto.SetActive(false);
    }
}
