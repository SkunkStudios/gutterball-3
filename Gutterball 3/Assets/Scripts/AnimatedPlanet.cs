using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPlanet : MonoBehaviour
{
	public float SpeedX, SpeedY;
	private float CurrX, CurrY;
	// Use this for initialization
	void Start ()
	{
		CurrX = GetComponent<Renderer>().material.mainTextureOffset.x;
		CurrY = GetComponent<Renderer>().material.mainTextureOffset.y;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Random.Range(-CurrX * 360, CurrX * 360), Random.Range(-CurrY * 360, CurrY * 360)));
        GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", new Vector2(Random.Range(-CurrX * 360, CurrX * 360), Random.Range(-CurrY * 360, CurrY * 360)));
    }

    // Update is called once per frame
    void Update ()
	{
        CurrX += Time.deltaTime * SpeedX;
        CurrY += Time.deltaTime * SpeedY;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(CurrX, CurrY));
        GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", new Vector2(CurrX, CurrY));
    }
}
