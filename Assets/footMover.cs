using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footMover : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera mainCamera;

    void Start()
    {
        transform.position = new Vector3(mainCamera.transform.position.x,transform.position.y,mainCamera.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mainCamera.transform.position.x,transform.position.y,mainCamera.transform.position.z);
    }
}
