using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBG : MonoBehaviour
{
	void LateUpdate ()
	{
		transform.position = new Vector3(0, 5000, 0);
	}
}
