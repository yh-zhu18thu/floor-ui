using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using OpenCvSharp.Aruco;
using OpenCvSharp;
using UnityEngine.UI;

namespace Tsinghua.HCI.IoThingsLab
{
    public class CertHandler : CertificateHandler {
        protected override bool ValidateCertificate (byte[] certificateData) {
            return true;
        }
    }
    /// <summary>
    /// Very basic class I created for the needs of teams who need to get the camera or video value
    /// If you want to stream live video better not use this class but use Video Player Component instead
    /// </summary>
    public class ExternalVideoStreaming : MonoBehaviour
    {
        /// <summary>
        // store the center(s) of the first two detected markers
        /// </summary>
        Point2f[] centers;
        /// <summary>
        /// give the centers' size, only 0/1/2 are possible
        /// </summary>
        int centerSize;
        //double[*,*] camMatrix;


        public string ip = null;
        public bool applyEstimationPose = true;

        // to zyh
        // first access the centerSize then read centers[]
        // for images, left up corner is Point2f(0, 0), right down corner is Point2f(640, 480)


        // Use this for initialization
        void Start()
        {
            /*
            camMatrix = new double[3,3];
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; i < 3; ++j)
                {
                    if (i == j) camMatrix[i,j] = 1.0;
                    else camMatrix[i,j] = 0.0;
                }
            }
            */
            centers = new Point2f[2];
            centerSize = 0;
            if (ip == null)
            {
                ip = "183.172.48.100";
            }
            InvokeRepeating("SetTexture", 2.0f, 0.1f);
        }

        void SetTexture()
        {
            StartCoroutine(GetTexture());
        }
        IEnumerator GetTexture()
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://" + ip + ":8080/shot.jpg");
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
                DetectorParameters detectorParameters = DetectorParameters.Create();

                // Dictionary holds set of all available markers
                Dictionary dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_250);

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
                    Debug.Log(ids[i]);
                    // Debug.Log(corners[i][0] + " " + corners[i][1] + " " + corners[i][2] + " " + corners[i][3]);
                    if (i == 0)
                    {
                        centers[0] = new Point2f((corners[0][0].X + corners[0][1].X + corners[0][2].X + corners[0][3].X) / 4,
                        (corners[0][0].Y + corners[0][1].Y + corners[0][2].Y + corners[0][3].Y) / 4);
                        Debug.Log(centers[0] + "");
                    }
                    
                    if (i == 1)
                    {
                        centers[1] = new Point2f((corners[1][0].X + corners[1][1].X + corners[1][2].X + corners[1][3].X) / 4,
                        (corners[1][0].Y + corners[1][1].Y + corners[1][2].Y + corners[1][3].Y) / 4);
                        Debug.Log(centers[1] + "");
                    }
                }
                CvAruco.DrawDetectedMarkers(mat, corners, ids);
                /*
                 
                if (applyEstimationPose)
                {
                    CvAruco.DrawAxis(mat, camMatrix, null, rvec, tvec, 0.1f);
                }
                */
                
                // Create Unity output texture with detected markers
                Texture2D outputTexture = OpenCvSharp.Unity.MatToTexture(mat);

                myTexture = outputTexture;

                transform.GetComponent<MeshRenderer>().material.mainTexture = myTexture;

            }
        }
    }
}