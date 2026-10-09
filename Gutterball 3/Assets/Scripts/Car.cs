using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarPath
{
    public Vector3 pos;
    public Vector3 minPos;
    public Vector3 maxPos;
    public float rot;
}

public class Car : MonoBehaviour
{
    public CarPath[] paths;
    public float minSpeed;
    public float maxSpeed;

    private float speed;
    private int randomPath;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        randomPath = Random.Range(0, paths.Length);
        transform.position = new Vector3(Random.Range(paths[randomPath].minPos.x, paths[randomPath].maxPos.x), paths[randomPath].pos.y, Random.Range(paths[randomPath].minPos.z, paths[randomPath].maxPos.z));
        transform.eulerAngles = new Vector3(0, paths[randomPath].rot, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        if (transform.position.x < paths[randomPath].minPos.x || transform.position.x > paths[randomPath].maxPos.x || transform.position.z < paths[randomPath].minPos.z || transform.position.z > paths[randomPath].maxPos.z)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            randomPath = Random.Range(0, paths.Length);
            transform.position = paths[randomPath].pos;
            transform.eulerAngles = new Vector3(0, paths[randomPath].rot, 0);
        }
    }
}
