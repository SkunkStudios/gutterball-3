using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinFace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IdleFace());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IdleFace()
    {
        yield return new WaitForSeconds(Random.Range(0, 30));
        while (true)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(Random.Range(0, 30));
        }
    }
}
