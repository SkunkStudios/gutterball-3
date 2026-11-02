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
    public Vector3 minStop;
    public Vector3 maxStop;
    public int stopIndex;
    public int halfIndex;
    public int goIndex;
}

public class Car : MonoBehaviour
{
    public CarPath[] paths;
    public GameObject[] wheels;
    public float minSpeed;
    public float maxSpeed;
    public TrafficLights trafficLights;

    private float speed;
    private int randomPath;
    private bool isStop;

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
        if (transform.position.x > paths[randomPath].minStop.x && transform.position.x < paths[randomPath].maxStop.x && trafficLights != null && trafficLights.lightsIndex == paths[randomPath].stopIndex || transform.position.z > paths[randomPath].minStop.z && transform.position.z < paths[randomPath].maxStop.z && trafficLights != null && trafficLights.lightsIndex == paths[randomPath].stopIndex || transform.position.x > paths[randomPath].minStop.x && transform.position.x < paths[randomPath].maxStop.x && trafficLights != null && trafficLights.lightsIndex == paths[randomPath].halfIndex || transform.position.z > paths[randomPath].minStop.z && transform.position.z < paths[randomPath].maxStop.z && trafficLights != null && trafficLights.lightsIndex == paths[randomPath].halfIndex)
        {
            isStop = true;
        }
        else if (trafficLights != null && trafficLights.lightsIndex == paths[randomPath].goIndex)
        {
            isStop = false;
        }
        if (trafficLights == null || trafficLights != null && !isStop || transform.position.x > paths[randomPath].minStop.x && transform.position.x < paths[randomPath].maxStop.x && isStop || transform.position.z > paths[randomPath].minStop.z && transform.position.z < paths[randomPath].maxStop.z && isStop)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(new Vector3(speed * 10, 0, 0) * Time.deltaTime);
            }
        }
        if (transform.position.x < paths[randomPath].minPos.x || transform.position.x > paths[randomPath].maxPos.x || transform.position.z < paths[randomPath].minPos.z || transform.position.z > paths[randomPath].maxPos.z)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            randomPath = Random.Range(0, paths.Length);
            transform.position = paths[randomPath].pos;
            transform.eulerAngles = new Vector3(0, paths[randomPath].rot, 0);
        }
    }
}
