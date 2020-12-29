
using UnityEngine;
using System.Collections;
public class WebCam : MonoBehaviour
{
    public string deviceName;
    WebCamTexture camTexture;
    // Use this for initialization
    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            camTexture = new WebCamTexture(deviceName, 400, 300, 12);
            transform.GetComponent<MeshRenderer>().material.mainTexture = camTexture;
            camTexture.Play();
        }
    }
}