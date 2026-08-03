using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedWater : MonoBehaviour
{
	public int rendererCounts;
	public float speedX = 0.1f;
    public float speedY = 0.1f;
    public bool is2Renderer;
    private float curX;
    private float curY;

    // Use this for initialization
    void Start ()
	{
        curX = GetComponent<Renderer>().material.mainTextureOffset.x;
        curY = GetComponent<Renderer>().material.mainTextureOffset.y;
    }

    void FixedUpdate ()
	{
        curX += Time.deltaTime * speedX;
        curY += Time.deltaTime * speedY;
        if (is2Renderer)
        {
            GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex", new Vector2(curX, curY));
            GetComponent<Renderer>().materials[8].SetTextureOffset("_MainTex", new Vector2(curX, curY));
        }
        else
        {
            GetComponent<Renderer>().materials[rendererCounts].SetTextureOffset("_MainTex", new Vector2(curX, curY));
        }
    }
}
