using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ResetArduino : MonoBehaviour
{
   // public Arduino arduinoScript;
    
    void Awake()
    {
        if (Arduino.sp.IsOpen)
        {
            Arduino.sp.WriteLine("1");
        }
    }
    // Start is called before the first frame update
    
    void Start()
    {               
            if (Arduino.sp.IsOpen)
            {
                Arduino.sp.WriteLine("1");
            }       
       
    }
    

  
}
