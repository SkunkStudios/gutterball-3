using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMat : MonoBehaviour
{
    public Material reflectOn;
    public Material reflectOff;
    public bool is2Renderer;

    void Start ()
	{
        Material[] mats = GetComponent<Renderer>().materials;
        if (GameManager.isReflect)
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
        else
        {
            if (is2Renderer)
            {
                mats[0] = reflectOff;
                mats[3] = reflectOff;
            }
            else
            {
                mats[0] = reflectOff;
            }
        }
        GetComponent<Renderer>().materials = mats;
    }
}
