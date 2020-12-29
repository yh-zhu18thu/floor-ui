using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InputTracking = UnityEngine.XR.InputTracking ;
using Node = UnityEngine.XR.XRNode;
public class GetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().text=InputTracking.GetLocalPosition(Node.LeftHand).ToString();
    }
}
