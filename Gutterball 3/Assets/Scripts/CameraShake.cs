using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private GameManager gameManager;
    private float shakeHit;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeHit > 0)
        {
            shakeHit -= Time.deltaTime * 15;
        }
        else if (shakeHit < 0)
        {
            shakeHit = 0;
        }
        transform.localPosition = new Vector3(Random.Range(-shakeHit, shakeHit), Random.Range(-shakeHit, shakeHit), Random.Range(-shakeHit, shakeHit));
    }

    public void Shake(float setShake)
    {
        if (GameManager.isShake)
        {
            shakeHit += setShake;
        }
    }
}
