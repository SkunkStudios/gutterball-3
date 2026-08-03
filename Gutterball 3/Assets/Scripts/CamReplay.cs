using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamReplay : MonoBehaviour
{
    public Transform[] endPoint;

    public IEnumerator ReplayMove()
    {
        GameObject.FindObjectOfType<CameraFollow>().transform.position = transform.position;
        GameObject.FindObjectOfType<CameraFollow>().transform.rotation = transform.rotation;
        float time = 0;
        int randomReplay = Random.Range(0, endPoint.Length);

        while (time < 1)
        {
            GameObject.FindObjectOfType<CameraFollow>().transform.position = Vector3.Lerp(transform.position, endPoint[randomReplay].position, time);
            GameObject.FindObjectOfType<CameraFollow>().transform.rotation = Quaternion.Lerp(transform.rotation, endPoint[randomReplay].rotation, time);
            time += Time.deltaTime * 0.5f;
            yield return null;
        }

        GameObject.FindObjectOfType<CameraFollow>().transform.position = endPoint[randomReplay].position;
        GameObject.FindObjectOfType<CameraFollow>().transform.rotation = endPoint[randomReplay].rotation;
        GameObject.FindObjectOfType<Game>().camType = Game.CameraType.Replay2;
    }
}
