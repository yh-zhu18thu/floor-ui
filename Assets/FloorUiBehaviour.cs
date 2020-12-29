using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUiBehaviour : MonoBehaviour
{


    public int frameNumber;
    public double sightBoundary;

    public float floorHeight;
    private Queue<double> downQueue;
    public Camera mainCamera;
    private GameObject floor;
    private GameObject footDisplay;
    private bool onShown;
    Vector2 forwardDir;

    private RaycastHit hit;

    private Vector3 camStartPoint;

    public bool fixedAwake;

    public double stableAwakeParameter;

    private Queue<Vector3> lookingPosQueue;

    private int time=0;
    

    Vector3 lookingPosition(){
        Ray camRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2));
        //float distanceOnDirection = (camRay.origin.y - floorHeight) / camRay.direction.y;
        //return camRay.origin - camRay.direction*distanceOnDirection;
        Physics.Raycast(camRay.origin,camRay.direction,out hit, Mathf.Infinity);
        return hit.point;
    }

    


    // Start is called before the first frame update

    void Start()
    {
        onShown = false;
        downQueue = new Queue<double>();
        lookingPosQueue = new Queue<Vector3>();
        floor = GameObject.Find("FloorCanvas");
        floor.SetActive(false);
        footDisplay = GameObject.Find("footDisplay");
        footDisplay.SetActive(false);
        transform.position = new Vector3(mainCamera.transform.position.x, floorHeight,mainCamera.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 lookingPos = lookingPosition();
        if(!onShown){
            //Debug.Log(lookingPos.ToString());
            transform.position = new Vector3(mainCamera.transform.position.x,-0.1f,mainCamera.transform.position.z);
            //transform.position = new Vector3(lookingPos.x, floorHeight,lookingPos.z);
            Vector3 rotate3 = new Vector3(lookingPos.x-mainCamera.transform.position.x,0f,lookingPos.z-mainCamera.transform.position.z);
            Quaternion rot=Quaternion.LookRotation(rotate3);
            transform.rotation = Quaternion.Lerp (transform.rotation,rot,Time.deltaTime*100);
            //forwardDirection = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2)).direction;
            forwardDir = new Vector2(lookingPos.x-mainCamera.transform.position.x,lookingPos.z-mainCamera.transform.position.z);
        }
        Vector3 realRotation = mainCamera.transform.eulerAngles;
        if (downQueue.Count >= frameNumber) {
            downQueue.Dequeue();
        }
        if (lookingPosQueue.Count >= frameNumber){
            lookingPosQueue.Dequeue();
        }
        downQueue.Enqueue(realRotation.x);
        lookingPosQueue.Enqueue(lookingPos);
        double sum = 0;
        foreach (double down in downQueue) {
            sum += down;
        }
        double avg = sum / downQueue.Count;

        Vector3 posSum = Vector3.zero;
        foreach (Vector3 position in lookingPosQueue){
            posSum += position;
        }
        Vector3 posAvg = posSum / downQueue.Count;

        double squDifSum=0;
        foreach (Vector3 position in lookingPosQueue){
            squDifSum+=(posAvg-position).magnitude;
        }
        double squDif = squDifSum/lookingPosQueue.Count;
        //Debug.Log("current avg is "+avg);
        //Debug.Log("current squDif is : "+squDif);
        //Debug.Log("current euler rotation is" + mainCamera.transform.eulerAngles.ToString());
        //Debug.Log("current local euler rotation is" + mainCamera.transform.localEulerAngles.ToString());
        //Debug.Log("current considering size is: "+downQueue.Count+" the current sum is : "+sum +" the current avg is : "+avg);
        if (avg > sightBoundary && avg < 90 && downQueue.Count >= frameNumber) {
            if(!onShown){
                if(!fixedAwake || squDif <= stableAwakeParameter){
                    floor.SetActive(true);
                    footDisplay.SetActive(true);
                    onShown = true;
                }
            }
            
            //Debug.Log("forward direction "+ forwardDirection.ToString());
        }else if(avg < 20 || avg >=90 || downQueue.Count < frameNumber ){
            if(onShown){
                floor.SetActive(false);
                footDisplay.SetActive(false);
                onShown = false;
            }
        }
    }
}
