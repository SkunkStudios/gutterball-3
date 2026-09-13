using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    int randomComet;

    // Start is called before the first frame update
    void Start()
    {
        randomComet = Random.Range(0, 2);

        if (randomComet == 0)
        {
            transform.localPosition = new Vector3(500, Random.Range(-150, 500), -400);
        }
        else if (randomComet == 1)
        {
            transform.localPosition = new Vector3(-500, Random.Range(-150, 500), -400);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (randomComet == 0)
        {
            transform.Translate(-30 / 5, -15 / 5, 0 * Time.deltaTime, Space.World);

            if (transform.localPosition.x <= -500)
            {
                randomComet = 1;
                transform.localPosition = new Vector3(-500, Random.Range(-150, 600), -400);
            }
        }
        else if (randomComet == 1)
        {
            transform.Translate(30 / 5, -15 / 5, 0 * Time.deltaTime, Space.World);

            if (transform.localPosition.x >= 500)
            {
                randomComet = 0;
                transform.localPosition = new Vector3(500, Random.Range(-150, 600), -400);
            }
        }
    }
}
