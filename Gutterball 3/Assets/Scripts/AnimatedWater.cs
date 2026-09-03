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

    void Awake()
    {
        curX = GetComponent<Renderer>().material.mainTextureOffset.x;
        curY = GetComponent<Renderer>().material.mainTextureOffset.y;
    }

    void FixedUpdate ()
	{
        curX += Time.deltaTime * speedX / 3;
        curY += Time.deltaTime * speedY / 3;
        if (curX >= 1)
        {
            curX = 0;
        }
        if (curY >= 1)
        {
            curY = 0;
        }
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
