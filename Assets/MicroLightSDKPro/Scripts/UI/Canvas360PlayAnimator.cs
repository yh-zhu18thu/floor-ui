using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas360PlayAnimator : PlayAnimator
{
    public GameObject canvas;
    public void Start()
    {
        //SetMax();
    }

    public void FixedUpdate()
    {
        if (canvas.activeSelf)
        {
            SetMax();
        }

    }

    public void SetMax()
    {
        base.PlayAnimation("Max", true);
    }

    public void ReMax()
    {
        base.PlayAnimation("Max", false);
    }


}
