using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float maxSpeed;
    public Vector3 minPosition, maxPosition;
    public float maxSize;

    private float speed;
    private float size;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
        transform.eulerAngles = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
        speed = Random.Range(0, maxSpeed);
        size = Random.Range(0, maxSize);
        transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed / 5 * Time.deltaTime, Space.World);
        if (transform.localPosition.x >= maxPosition.x)
        {
            transform.localPosition = new Vector3(minPosition.x, Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
            transform.eulerAngles = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
            speed = Random.Range(0, maxSpeed);
            size = Random.Range(0, maxSize);
            transform.localScale = new Vector3(size, size, size);
        }
    }
}
