using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBG : MonoBehaviour
{
    void Awake()
    {
        transform.position = new Vector3(0, 5000, 0);
    }

    void LateUpdate ()
	{
		transform.position = new Vector3(0, 5000, 0);
	}
}
