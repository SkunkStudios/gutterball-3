using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMat : MonoBehaviour
{
    public Material reflectOn;
    public Material reflectOff;
    public Material reflectOffL;
    public Material reflectOffR;
    public bool is2Renderer;
    public bool isWebReflect;

    void Awake ()
	{
        if (GetComponent<PlaneReflection>() != null)
        {
            if (isWebReflect && Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GetComponent<PlaneReflection>().enabled = false;
            }
            else
            {
                GetComponent<PlaneReflection>().enabled = GameManager.isReflect;
            }
        }
        if (GetComponent<Water>() != null)
        {
            GetComponent<Water>().enabled = GameManager.isReflect;
        }
        Material[] mats = GetComponent<Renderer>().materials;
        if (GameManager.isReflect)
        {
            if (isWebReflect && Application.platform == RuntimePlatform.WebGLPlayer)
            {
                if (is2Renderer)
                {
                    mats[0] = reflectOffL;
                    mats[3] = reflectOffR;
                }
                else
                {
                    mats[0] = reflectOff;
                }
            }
            else
            {
                if (is2Renderer)
                {
                    mats[0] = reflectOn;
                    mats[3] = reflectOn;
                }
                else
                {
                    mats[0] = reflectOn;
                }
            }
        }
        else
        {
            if (is2Renderer)
            {
                mats[0] = reflectOffL;
                mats[3] = reflectOffR;
            }
            else
            {
                mats[0] = reflectOff;
            }
        }
        GetComponent<Renderer>().materials = mats;
    }
}
