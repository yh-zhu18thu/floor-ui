using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCtrl : MonoBehaviour
{
    void Show()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    void Hide()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void changeStatus()
    {
                  Debug.Log("watching TV");
        gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // changeStatus() ; 
    }
}
