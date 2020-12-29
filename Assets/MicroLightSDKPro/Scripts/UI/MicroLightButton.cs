
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
[ExecuteInEditMode,
             RequireComponent(typeof(BoxCollider)),
    RequireComponent(typeof(RectTransform)),
    RequireComponent(typeof(Image)),
    RequireComponent(typeof(Button))]
public class MicroLightButton : MonoBehaviour
{
    //[HideInInspector]
    //public Rigidbody _Rigidbody;

    //public Rigidbody Rigidbody
    //{
    //    get
    //    {
    //        if (_Rigidbody == null)
    //        {
    //            _Rigidbody = GetComponent<Rigidbody>();
    //        }
    //        return _Rigidbody;
    //    }
    //}



    [HideInInspector]
    public BoxCollider _boxCollider;

    public BoxCollider boxCollider
    {
        get
        {
            if (_boxCollider == null)
            {
                _boxCollider = GetComponent<BoxCollider>();
            }
            return _boxCollider;
        }
    }


    [HideInInspector]
    public RectTransform _rectTransform;

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    [HideInInspector]
    public Image _image;

    public Image image
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            return _image;
        }
    }
    [HideInInspector]
    public Button _button;

    public Button button
    {
        get
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
            return _button;
        }
    }

 
    // State Events
    [SerializeField]
    [FormerlySerializedAs("OnPress")]
    private UnityEvent OnPress = new UnityEvent();
    [SerializeField]
    [FormerlySerializedAs("OnUnpress")]
    private UnityEvent OnUnpress = new UnityEvent();
    // Use this for initialization
    protected virtual void Start()
    {
        boxCollider.size = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 1);
       
    }
  
    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
    }
    protected virtual void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay");

    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit");
    }

    // during Soft Contact, controller colliders are triggers

    protected virtual void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter");
        if (Application.isPlaying)
        {

        }
    }

    protected  void OnTriggerStay(Collider collider)
    {
        Debug.Log("OnTriggerStay");
        if (Application.isPlaying)
        {
            
                    if (OnPress != null)
                    {
                        OnPress.Invoke();
                    }
                    
        }
    }
    protected  void OnTriggerExit(Collider collider)
    {
        Debug.Log("OnTriggerExit");
        if (Application.isPlaying)
        {
            
                        if (OnUnpress != null)
                        {
                            OnUnpress.Invoke();
                        }
                        
        }
    }
    
}

