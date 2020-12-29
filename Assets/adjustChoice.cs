using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using OpenCvSharp.Aruco;
using OpenCvSharp;
using Tsinghua.HCI.IoThingsLab;

public class CertHandler : CertificateHandler {
        protected override bool ValidateCertificate (byte[] certificateData) {
            return true;
        }
}

public class adjustChoice : MonoBehaviour
{
    // Start iSs called before the first frame update
    


    
    public GameObject canvas_;
    public GameObject light;
    public GameObject door0;
    public GameObject door1;
    public GameObject tv;
    public GameObject speaker;
    public GameObject sleep;

    public float initZRotation;
    public float angleBiasRange; //允许多大的注视角度偏差

    public int certainFrameNumber; //注视多少帧之后确认选中

    public Camera mainCamera;
    private GameObject singleChoice1;
    private GameObject singleChoice2;
    private GameObject singleChoice3;
    private GameObject singleChoice4;

    private bool onShown =false;

    private Vector2 forwardDir;

    private RaycastHit hit;

    private Vector3 camStartPoint;

    private int[] centerAngle = {15,-15,-45,45};

    private int currentSelected = -1;

    private int accumulateCount = 0;

    public bool footSelection = true;

    Point2f[] centers;

    int centerSize;

    public string ip = "183.172.48.100";


    public GameObject cameraQuad;

    private Dictionary dictionary;

    public int refreshingRate; 

    private int count = 0;

    public int screenWidth = 1920;
    public int screenHeight = 1080;

    public int freezingTimeLimit = 80;

    private int freezingTime = 0;

    public GameObject[] hintLights;

    public float zoomRate;

    Vector3 translateFoot2RealWorld(float x,float y){
        float movedX = x - screenWidth/2;
        float movedY = -y +  screenHeight/2;
        Vector3 xDir = cameraQuad.transform.right;
        Vector3 yDir = cameraQuad.transform.up;
        return cameraQuad.transform.position + movedX*xDir*zoomRate + movedY*yDir*zoomRate;
    }

    bool compareMarkers(float a, float b){
        if(Mathf.Abs(a-screenWidth/2)>Mathf.Abs(b-screenWidth/2)){
            return true;
        }else{
            return false;
        }
    }
    
     void SetTexture()
    {
        StartCoroutine(GetTexture());
    }
        IEnumerator GetTexture()
        {
            //Debug.Log("getting marker");
            if(onShown && footSelection && (++count)%refreshingRate==0){
                //Debug.Log("getting marker");
                UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://" + ip + ":8080/photo.jpg");
                www.certificateHandler = new CertHandler();
                yield return www.SendWebRequest();
                double[] rvec = new double[3];
                double[] tvec = new double[3];

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    cameraQuad.GetComponent<MeshRenderer>().material.mainTexture = myTexture;

                    if(myTexture!=null){
                        DetectorParameters detectorParameters = DetectorParameters.Create();

                // Dictionary holds set of all available markers

                // Variables to hold results
                    Point2f[][] corners;
                
                    int[] ids;
                    Point2f[][] rejectedImgPoints;

                // Create Opencv image from unity texture
                    Mat mat = OpenCvSharp.Unity.TextureToMat((Texture2D)myTexture);

                // Convert image to grasyscale
                    Mat grayMat = new Mat();
                    Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);

                // Detect and draw markers
                    CvAruco.DetectMarkers(grayMat, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);
                    centerSize = ids.Length > 1 ? 2 : ids.Length == 0 ? 0 : 1;
                    
                    for (int i = 0; i < ids.Length; ++i)
                    {
                        //Debug.Log(ids[i]);
                        // Debug.Log(corners[i][0] + " " + corners[i][1] + " " + corners[i][2] + " " + corners[i][3]);
                        if (i == 0)
                        {
                            centers[0] = new Point2f((corners[0][0].X + corners[0][1].X + corners[0][2].X + corners[0][3].X) / 4,
                            (corners[0][0].Y + corners[0][1].Y + corners[0][2].Y + corners[0][3].Y) / 4);
                            //Debug.Log(centers[0] + " is " + ids[i]);
                        }
                    
                        if (i == 1)
                        {
                            centers[1] = new Point2f((corners[1][0].X + corners[1][1].X + corners[1][2].X + corners[1][3].X) / 4,
                            (corners[1][0].Y + corners[1][1].Y + corners[1][2].Y + corners[1][3].Y) / 4);
                            //Debug.Log(centers[1] + " is " + ids[i]);
                        }
                    }
                    lookingDistrict();
                }
                //Debug.Log("calling times: "+count);
                }
            }    
        }
    Vector3 lookingPosition(){
        Ray camRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2));
        //float distanceOnDirection = (camRay.origin.y - floorHeight) / camRay.direction.y;
        //return camRay.origin - camRay.direction*distanceOnDirection;
        Physics.Raycast(camRay.origin,camRay.direction,out hit, Mathf.Infinity);
        return hit.point;
    }

    int markerSelection(){
       if(centerSize>0){
           Vector3 marker1Position = translateFoot2RealWorld(centers[0].X,centers[0].Y);
           double marker1Distance = (new Vector3(camStartPoint.x,0,camStartPoint.z)-marker1Position).magnitude;
           if (centerSize==2){
               Vector3 marker2Position = translateFoot2RealWorld(centers[1].X,centers[1].Y);
                double marker2Distance = (new Vector3(camStartPoint.x,0,camStartPoint.z)-marker2Position).magnitude;
                if(marker2Distance>marker1Distance){
                    marker1Position = marker2Position;
                    marker1Distance = marker2Distance;
                }
           }
           //Debug.Log("markerDistance: "+ marker1Distance);
           if(marker1Distance>=0.75f){
               Vector2 nowDir = new Vector2(-camStartPoint.x+marker1Position.x, -camStartPoint.z+marker1Position.z);
                Vector3 crossResult = Vector3.Cross(forwardDir,nowDir);
                float angle = Vector2.Angle(forwardDir, nowDir);
                angle = crossResult.z > 0 ? -angle : angle;
                angle = angle+60;
                //Debug.Log("current angle is: "+angle);
                int selectRank = -1;
                float minAngleBias = 1000;
                for(int i=0;i<4;i++){
                    float angleBias = Mathf.Abs(centerAngle[i]-angle);
                    if(angleBias<minAngleBias){
                        selectRank = i;
                        minAngleBias = angleBias;
                    }
                }
                return selectRank;
           }else{
               return -1;
           }
       }else{
           return -1;
       }
    }

    int lookingDistrict(){
        if(onShown){
            if(freezingTime>0){
                freezingTime--;
                singleChoice1.SendMessage("ReSetHover");
                singleChoice2.SendMessage("ReSetHover");
                singleChoice3.SendMessage("ReSetHover");
                singleChoice4.SendMessage("ReSetHover");
                return 0;
            }
           Ray camRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2));
            Physics.Raycast(camRay.origin,camRay.direction,out hit, Mathf.Infinity);
            Vector3 lookingPos = hit.point;
            Vector2 nowDir = new Vector2(-camStartPoint.x+lookingPos.x, -camStartPoint.z+lookingPos.z);
            Vector3 crossResult = Vector3.Cross(forwardDir,nowDir);
            float angle = Vector2.Angle(forwardDir, nowDir);
            angle = crossResult.z > 0 ? -angle : angle;
            //Debug.Log("current angle is: "+angle);
            int selectRank = -1;
            float minAngleBias = 1000;
            if (!footSelection){
                for(int i=0;i<4;i++){
                    float angleBias = Mathf.Abs(centerAngle[i]-angle);
                    if(angleBias<minAngleBias){
                        selectRank = i;
                        minAngleBias = angleBias;
                    }
                }
            }else{
                selectRank = markerSelection();
            }
            Debug.Log("current select: "+ selectRank +" times "+accumulateCount);
            if(selectRank!=-1 && currentSelected == selectRank && ((minAngleBias<angleBiasRange && !footSelection) || footSelection)){
                accumulateCount ++;
                if(accumulateCount >= certainFrameNumber){
                    accumulateCount = 0;
                    freezingTime = freezingTimeLimit;
                    switch (selectRank)
                    {
                        case 0:
                            singleChoice1.SendMessage("Light");
                            singleChoice1.SendMessage("ReSetHover");
                            break;
                        case 1:
                            singleChoice2.SendMessage("TV");
                            singleChoice2.SendMessage("ReSetHover");
                            break;
                        case 2:
                            singleChoice3.SendMessage("Door0");
                            singleChoice3.SendMessage("ReSetHover");
                            break;
                        case 3:
                            singleChoice4.SendMessage("speaker");
                            singleChoice4.SendMessage("ReSetHover");
                            break;
                        default:
                            break;
                    }
                    if (Mathf.Abs(mainCamera.transform.position.x - hintLights[selectRank].transform.position.x) < 1 && Mathf.Abs(mainCamera.transform.position.z - hintLights[selectRank].transform.position.z) < 1)
                    {
                        hintLights[selectRank].transform.gameObject.SetActive(false);
                        Debug.Log("trigger");
                        if(selectRank<hintLights.Length-1){
                            hintLights[selectRank+1].transform.gameObject.SetActive(true);
                        }
                    }
                    return 0;
                }
            }else{
                accumulateCount = 0;
                singleChoice1.SendMessage("ReSetHover");
                singleChoice2.SendMessage("ReSetHover");
                singleChoice3.SendMessage("ReSetHover");
                singleChoice4.SendMessage("ReSetHover");
            }
            if(selectRank!=-1 && ((minAngleBias<angleBiasRange && !footSelection) || footSelection)){
                currentSelected = selectRank;
                switch (selectRank)
                {
                    case 0:
                        singleChoice1.SendMessage("SetHover");
                        singleChoice2.SendMessage("ReSetHover");
                        singleChoice3.SendMessage("ReSetHover");
                        singleChoice4.SendMessage("ReSetHover");
                        break;
                    case 1:
                        singleChoice2.SendMessage("SetHover");
                        singleChoice1.SendMessage("ReSetHover");
                        singleChoice3.SendMessage("ReSetHover");
                        singleChoice4.SendMessage("ReSetHover");
                        break;
                    case 2:
                        singleChoice3.SendMessage("SetHover");
                        singleChoice2.SendMessage("ReSetHover");
                        singleChoice1.SendMessage("ReSetHover");
                        singleChoice4.SendMessage("ReSetHover");
                        break;
                    case 3:
                        singleChoice4.SendMessage("SetHover");
                        singleChoice2.SendMessage("ReSetHover");
                        singleChoice3.SendMessage("ReSetHover");
                        singleChoice1.SendMessage("ReSetHover");
                        break;
                    default:
                        Debug.Log("woc");
                        break;
                }
            }
            //Debug.Log("angle: " + angle);
        }
        return 1;
    }

    void Awake()
    {
        singleChoice1 = Instantiate(light, new Vector3(0, 0, 0), Quaternion.identity);
        singleChoice1.transform.parent = canvas_.transform;
        singleChoice1.transform.Rotate(new Vector3(90, 0, initZRotation-15));
        singleChoice1.transform.localPosition = new Vector3(0, 0, 0);
        singleChoice1.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        
        singleChoice2 = Instantiate(tv, new Vector3(0, 0, 0), Quaternion.identity);
        singleChoice2.transform.parent = canvas_.transform;
        singleChoice2.transform.Rotate(new Vector3(90, 0, initZRotation+15));
        singleChoice2.transform.localPosition = new Vector3(0, 0, 0);
        singleChoice2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        singleChoice3 = Instantiate(door0, new Vector3(0, 0, 0), Quaternion.identity);
        singleChoice3.transform.parent = canvas_.transform;
        singleChoice3.transform.Rotate(new Vector3(90, 0, initZRotation+45));
        singleChoice3.transform.localPosition = new Vector3(0, 0, 0);
        singleChoice3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        singleChoice4 = Instantiate(speaker, new Vector3(0, 0, 0), Quaternion.identity);
        singleChoice4.transform.parent = canvas_.transform;
        singleChoice4.transform.Rotate(new Vector3(90, 0, initZRotation+315));
        singleChoice4.transform.localPosition = new Vector3(0, 0, 0);
        singleChoice4.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        onShown = false;
        centers = new Point2f[2];
        centerSize = 0;
        dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_250);
        InvokeRepeating("SetTexture", 2.0f, 0.1f);
        Debug.Log("hintLights length is: "+hintLights.Length);
        for(int i=1;i<hintLights.Length;i++){
            hintLights[i].transform.gameObject.SetActive(false);
        }
        if(footSelection){
            certainFrameNumber = 4;
            freezingTimeLimit = 4;
        }
    }

    void OnEnable(){
        Debug.Log("ui is awaked");
        onShown = true;
        Vector3 lookingPos = lookingPosition();
        camStartPoint = mainCamera.transform.position;
        forwardDir = new Vector2(transform.up.x,transform.up.z);
    }

    void onDisable(){
        Debug.Log("ui disabled");
        onShown = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onShown && !footSelection){
            lookingDistrict();
        }
    }
}
