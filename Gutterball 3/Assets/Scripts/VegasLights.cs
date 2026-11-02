using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegasLights : MonoBehaviour
{
    public GameObject[] lights = new GameObject[8];

    public IEnumerator StrikeLights()
    {
        for (int i = 0; i < 20; i++)
        {
            lights[0].SetActive(true);
            lights[1].SetActive(true);
            lights[2].SetActive(true);
            lights[3].SetActive(true);
            lights[4].SetActive(true);
            lights[5].SetActive(true);
            lights[6].SetActive(true);
            lights[7].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            lights[0].SetActive(false);
            lights[1].SetActive(false);
            lights[2].SetActive(false);
            lights[3].SetActive(false);
            lights[4].SetActive(false);
            lights[5].SetActive(false);
            lights[6].SetActive(false);
            lights[7].SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public IEnumerator SpareLights()
    {
        lights[3].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lights[3].SetActive(false);
        lights[2].SetActive(true);
        lights[7].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lights[2].SetActive(false);
        lights[7].SetActive(false);
        lights[1].SetActive(true);
        lights[6].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lights[1].SetActive(false);
        lights[6].SetActive(false);
        lights[0].SetActive(true);
        lights[5].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lights[0].SetActive(false);
        lights[5].SetActive(false);
        lights[4].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lights[4].SetActive(false);
    }
}
