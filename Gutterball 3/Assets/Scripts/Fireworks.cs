using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
	public Color[] colorFireworks;

    // Use this for initialization
    void Start ()
	{
        if (GameObject.FindObjectOfType<Game>() != null)
        {
            GameObject.FindObjectOfType<Game>().PlayClip("FireworksPop");
        }
        if (GameObject.FindObjectOfType<GameHundredPin>() != null)
        {
            GameObject.FindObjectOfType<GameHundredPin>().PlayClip("FireworksPop");
        }
    }
}
