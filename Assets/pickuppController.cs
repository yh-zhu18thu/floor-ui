using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickuppController : MonoBehaviour
{

    public Camera mainCamera;
    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        Vector3 camPos = mainCamera.transform.position;
        if (Mathf.Abs(camPos.x - transform.position.x) < 0.3 && Mathf.Abs(camPos.z - transform.position.z) < 0.3)
        {
            transform.gameObject.SetActive(false);
            Debug.Log("trigger");
        }
    }
}
