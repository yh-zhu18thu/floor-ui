using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace MicroLight
{
 
    public class MicroLightUI : MonoBehaviour
    {
        public UIType uIType;
   

        public UnityEvent ProcessSucess;


        public void SendProcessSucess()
        {
            if(ProcessSucess!=null)
            {
                ProcessSucess.Invoke();
            }
        }
    }
}
