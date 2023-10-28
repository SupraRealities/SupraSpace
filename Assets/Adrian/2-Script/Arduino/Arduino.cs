using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{
    static public SerialPort sp;

    [SerializeField] private int activo = 0;
    [SerializeField] private int activoNecesario = 5;

    [SerializeField] private string numeroCom = "COM3";

    private void Start()
    {
        sp = new SerialPort(numeroCom, 9600);
        
        activo = 0;
        activoNecesario = activo + 1;
        if (!sp.IsOpen)
        {
            sp.Open();
        }
    }

    private void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                activo = int.Parse(sp.ReadLine());
            }
            catch (System.Exception catchedException)
            {
                throw catchedException;
            }
        }
        if (activo == activoNecesario)
        {
            StartCoroutine(aumentoActivoNecesario());
            SceneManager.LoadScene("PreGame");
        }
    }
  
    IEnumerator aumentoActivoNecesario()
    {
        yield return new WaitForSeconds(0);
        activoNecesario = activo + 1;
        yield return new WaitForSeconds(1f);
    }
}
