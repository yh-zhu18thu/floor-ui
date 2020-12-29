using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimator : MonoBehaviour {
    private Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;

        }
        set
        {
            _animator = value;
        }
    }

    public void PlayAnimation(string Parameter,bool Enable)
    {
        if (animator)
        {
            animator.SetBool(Parameter, Enable);
        }
    }
    public void PlayAnimation(string Parameter, int Value)
    {
        if (animator)
        {
            animator.SetInteger(Parameter, Value);
        }
    }
    public void PlayAnimation(string Parameter, float Value)
    {
        if (animator)
        {
            animator.SetFloat(Parameter, Value);
        }
    }
}
