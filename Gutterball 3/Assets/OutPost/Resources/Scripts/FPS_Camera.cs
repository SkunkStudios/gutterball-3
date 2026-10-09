using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Camera : MonoBehaviour {
    public Camera FPSCam;
    public float HSpeed;
    public float VSpeed;
    public bool invert = true;

    public float Speed;

    float h;
    float v;
    float i = -1;
    // Use this for initialization
    void Start ()
    {
        if (!FPSCam) { FPSCam = Camera.main; }
        if (HSpeed == 0) { HSpeed = 15f; }
        if (VSpeed == 0) { VSpeed = 15f; }
        if (Speed == 0) { Speed = 5f; }
        FPSCam.transform.Rotate(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (invert) i=-1;
        else i = 1;

        transform.Rotate(0, HSpeed * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
        FPSCam.transform.Rotate(i * VSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime, 0, 0);

        if (Input.GetMouseButton(0)) { transform.Translate(0, Speed * Time.deltaTime, 0); }
        if (Input.GetMouseButton(1)) { transform.Translate(0, - Speed * Time.deltaTime, 0); }
        if (Input.GetMouseButton(2)) { transform.position = new Vector3(transform.position.x, 6, transform.position.z); }
        if (Input.GetAxis("Mouse ScrollWheel")>0) { FPSCam.fieldOfView = FPSCam.fieldOfView - 1; }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { FPSCam.fieldOfView = FPSCam.fieldOfView + 1; }


        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) { transform.Translate(-Speed * Time.deltaTime, 0, Speed * Time.deltaTime); }
        else
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) { transform.Translate(Speed * Time.deltaTime, 0, Speed * Time.deltaTime); }
            else
            {
                if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) { transform.Translate(-Speed * Time.deltaTime, 0, -Speed * Time.deltaTime); }
                else
                {
                    if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) { transform.Translate(Speed * Time.deltaTime, 0, -Speed * Time.deltaTime); }
                    else
                    {
                        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { transform.Translate(0, 0, Speed * Time.deltaTime); }
                        else
                        {
                            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { transform.Translate(0, 0, -Speed * Time.deltaTime); }
                            else
                            {
                                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { transform.Translate(-Speed * Time.deltaTime, 0, 0); }
                                else
                                {
                                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { transform.Translate(Speed * Time.deltaTime, 0, 0); }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
