using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tsinghua.HCI.IoTVRP;

namespace MicroLight
{
    public class BackgoundPlayAnimator : PlayAnimator
    {
        public void SetHover()
        {
            //Debug.Log("hover setting");
            base.PlayAnimation("Hover", true);
        }
        public void ReSetHover()
        {
            base.PlayAnimation("Hover", false);
        }
        public void Light()
        {
            Debug.Log("Light");
            GameObject audiosource = GameObject.Find("confirmSound_light");
            audiosource.GetComponent<AudioSource>().Play();
            GameObject light = GameObject.Find("LightItem11");
            light.GetComponent<LightItem>().Toggle();
        }

        public void TV()
        {
            Debug.Log("TV");
            GameObject audiosource = GameObject.Find("confirmSound_TV");
            audiosource.GetComponent<AudioSource>().Play();
            GameObject tv = GameObject.Find("ImageItem_tv");
            tv.GetComponent<ImageCtrl>().changeStatus();
            Debug.Log("watching TV");
        }

        public void Door0()
        {
            Debug.Log("Door0");
            GameObject audiosource = GameObject.Find("confirmSound_door");
            audiosource.GetComponent<AudioSource>().Play();
            GameObject tv = GameObject.Find("01_low_1");
            tv.GetComponent<DoorCtrl>().changeStatus();
        }

        public void Door1()
        {
            Debug.Log("Door1");
            GameObject audiosource = GameObject.Find("confirmSound_door");
            audiosource.GetComponent<AudioSource>().Play();
            GameObject tv = GameObject.Find("01_low_2");
            tv.GetComponent<DoorCtrl>().changeStatus();
        }

        public void speaker()
        {
            Debug.Log("Speaker");
            GameObject audiosource = GameObject.Find("confirmSound_speaker");
            audiosource.GetComponent<AudioSource>().Play();
            GameObject speaker = GameObject.Find("Speaker_Small_1");
            speaker.GetComponent<Tsinghua.HCI.IoThingsLab.AudioItem>().ToggleAudioSource();
        }

        public void sleep()
        {
            Debug.Log("sleep");
            GameObject audiosource = GameObject.Find("confirmSound");
            audiosource.GetComponent<AudioSource>().Play();
            GameObject light1 = GameObject.Find("LightItem_4");
            light1.GetComponent<LightItem>().Toggle();
            GameObject light = GameObject.Find("LightItem_3");
            light.GetComponent<LightItem>().Toggle();
        }
    }
}