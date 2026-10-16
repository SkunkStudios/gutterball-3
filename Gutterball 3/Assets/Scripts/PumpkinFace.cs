using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinFace : MonoBehaviour
{
    public Sprite angryFace;
    public Sprite gutterFace;
    public Sprite idleFace;
    public Sprite smileFace;

    private float faceTime;
    private bool isFace;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IdleFace());
    }

    // Update is called once per frame
    void Update()
    {
        if (faceTime > 0 && isFace)
        {
            faceTime -= Time.deltaTime;
        }
        else if (faceTime <= 0 && isFace)
        {
            StartCoroutine(IdleFace());
            isFace = false;
        }
    }

    IEnumerator IdleFace()
    {
        GetComponent<SpriteRenderer>().sprite = idleFace;
        yield return new WaitForSeconds(Random.Range(0, 30));
        while (true)
        {
            if (!isFace)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(Random.Range(0, 30));
        }
    }

    public void AngryFace()
    {
        StopCoroutine(IdleFace());
        GetComponent<SpriteRenderer>().sprite = angryFace;
        faceTime = 15;
        isFace = true;
    }

    public void GutterFace()
    {
        StopCoroutine(IdleFace());
        GetComponent<SpriteRenderer>().sprite = gutterFace;
        faceTime = 15;
        isFace = true;
    }

    public void SmileFace()
    {
        StopCoroutine(IdleFace());
        GetComponent<SpriteRenderer>().sprite = smileFace;
        faceTime = 15;
        isFace = true;
    }
}
