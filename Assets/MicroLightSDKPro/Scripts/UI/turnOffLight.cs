using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tsinghua.HCI.IoThingsLab;
namespace MicroLight
{
    public class turnOffLight : PlayAnimator
    {
        public void SetHover()
        {
            base.PlayAnimation("Hover", true);
        }
        public void ReSetHover()
        {
            base.PlayAnimation("Hover", false);
        }
        public void Confirm()
        {
            GameObject light = GameObject.Find("LightItem");
            light.GetComponent<LightItem>().TurnOff();
        }
    }
}