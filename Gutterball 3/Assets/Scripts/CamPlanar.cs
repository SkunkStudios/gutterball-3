using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlanar : MonoBehaviour
{
    private Vector2 resolution;

    void Awake()
    {
        resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        GetComponent<Camera>().targetTexture.Release();
        GetComponent<Camera>().targetTexture.width = Mathf.RoundToInt(resolution.x) * 256 / Mathf.RoundToInt(resolution.y);
        GetComponent<Camera>().targetTexture.height = 256;
    }

    void LateUpdate()
    {
        GetComponent<Camera>().fieldOfView = Camera.main.fieldOfView;
        transform.position = new Vector3(Camera.main.transform.position.x, -Camera.main.transform.position.y, Camera.main.transform.position.z);
        transform.rotation = Quaternion.Euler(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0);

        resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        GetComponent<Camera>().targetTexture.Release();
        GetComponent<Camera>().targetTexture.width = Mathf.RoundToInt(resolution.x) * 256 / Mathf.RoundToInt(resolution.y);
        GetComponent<Camera>().targetTexture.height = 256;
    }
}
