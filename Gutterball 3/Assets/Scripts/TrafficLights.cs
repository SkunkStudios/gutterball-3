using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{
    public Light[] redTrafficLights1;
    public Light[] redTrafficLights2;
    public Light[] yellowTrafficLights1;
    public Light[] yellowTrafficLights2;
    public Light[] greenTrafficLights1;
    public Light[] greenTrafficLights2;
    public Light[] redWalkingLights1;
    public Light[] redWalkingLights2;
    public Light[] greenWalkingLights1;
    public Light[] greenWalkingLights2;

    [HideInInspector]
    public int lightsIndex;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTrafficLights());
    }

    // Update is called once per frame
    void Update()
    {
        if (lightsIndex == 0)
        {
            foreach (Light redTrafficLight1 in redTrafficLights1)
            {
                redTrafficLight1.enabled = true;
            }
            foreach (Light redTrafficLight2 in redTrafficLights2)
            {
                redTrafficLight2.enabled = false;
            }
            foreach (Light yellowTrafficLight1 in yellowTrafficLights1)
            {
                yellowTrafficLight1.enabled = false;
            }
            foreach (Light yellowTrafficLight2 in yellowTrafficLights2)
            {
                yellowTrafficLight2.enabled = false;
            }
            foreach (Light greenTrafficLight1 in greenTrafficLights1)
            {
                greenTrafficLight1.enabled = false;
            }
            foreach (Light greenTrafficLight2 in greenTrafficLights2)
            {
                greenTrafficLight2.enabled = true;
            }
            foreach (Light redWalkingLight1 in redWalkingLights1)
            {
                redWalkingLight1.enabled = false;
            }
            foreach (Light redWalkingLight2 in redWalkingLights2)
            {
                redWalkingLight2.enabled = true;
            }
            foreach (Light greenWalkingLight1 in greenWalkingLights1)
            {
                greenWalkingLight1.enabled = true;
            }
            foreach (Light greenWalkingLight2 in greenWalkingLights2)
            {
                greenWalkingLight2.enabled = false;
            }
        }
        else if (lightsIndex == 1)
        {
            foreach (Light redTrafficLight1 in redTrafficLights1)
            {
                redTrafficLight1.enabled = false;
            }
            foreach (Light redTrafficLight2 in redTrafficLights2)
            {
                redTrafficLight2.enabled = true;
            }
            foreach (Light yellowTrafficLight1 in yellowTrafficLights1)
            {
                yellowTrafficLight1.enabled = false;
            }
            foreach (Light yellowTrafficLight2 in yellowTrafficLights2)
            {
                yellowTrafficLight2.enabled = false;
            }
            foreach (Light greenTrafficLight1 in greenTrafficLights1)
            {
                greenTrafficLight1.enabled = true;
            }
            foreach (Light greenTrafficLight2 in greenTrafficLights2)
            {
                greenTrafficLight2.enabled = false;
            }
            foreach (Light redWalkingLight1 in redWalkingLights1)
            {
                redWalkingLight1.enabled = true;
            }
            foreach (Light redWalkingLight2 in redWalkingLights2)
            {
                redWalkingLight2.enabled = false;
            }
            foreach (Light greenWalkingLight1 in greenWalkingLights1)
            {
                greenWalkingLight1.enabled = false;
            }
            foreach (Light greenWalkingLight2 in greenWalkingLights2)
            {
                greenWalkingLight2.enabled = true;
            }
        }
        else if (lightsIndex == 2)
        {
            foreach (Light redTrafficLight1 in redTrafficLights1)
            {
                redTrafficLight1.enabled = true;
            }
            foreach (Light redTrafficLight2 in redTrafficLights2)
            {
                redTrafficLight2.enabled = false;
            }
            foreach (Light yellowTrafficLight1 in yellowTrafficLights1)
            {
                yellowTrafficLight1.enabled = false;
            }
            foreach (Light yellowTrafficLight2 in yellowTrafficLights2)
            {
                yellowTrafficLight2.enabled = true;
            }
            foreach (Light greenTrafficLight1 in greenTrafficLights1)
            {
                greenTrafficLight1.enabled = false;
            }
            foreach (Light greenTrafficLight2 in greenTrafficLights2)
            {
                greenTrafficLight2.enabled = false;
            }
            foreach (Light redWalkingLight1 in redWalkingLights1)
            {
                redWalkingLight1.enabled = false;
            }
            foreach (Light redWalkingLight2 in redWalkingLights2)
            {
                redWalkingLight2.enabled = true;
            }
            foreach (Light greenWalkingLight1 in greenWalkingLights1)
            {
                greenWalkingLight1.enabled = true;
            }
            foreach (Light greenWalkingLight2 in greenWalkingLights2)
            {
                greenWalkingLight2.enabled = false;
            }
        }
        else if (lightsIndex == 3)
        {
            foreach (Light redTrafficLight1 in redTrafficLights1)
            {
                redTrafficLight1.enabled = false;
            }
            foreach (Light redTrafficLight2 in redTrafficLights2)
            {
                redTrafficLight2.enabled = true;
            }
            foreach (Light yellowTrafficLight1 in yellowTrafficLights1)
            {
                yellowTrafficLight1.enabled = true;
            }
            foreach (Light yellowTrafficLight2 in yellowTrafficLights2)
            {
                yellowTrafficLight2.enabled = false;
            }
            foreach (Light greenTrafficLight1 in greenTrafficLights1)
            {
                greenTrafficLight1.enabled = false;
            }
            foreach (Light greenTrafficLight2 in greenTrafficLights2)
            {
                greenTrafficLight2.enabled = false;
            }
            foreach (Light redWalkingLight1 in redWalkingLights1)
            {
                redWalkingLight1.enabled = true;
            }
            foreach (Light redWalkingLight2 in redWalkingLights2)
            {
                redWalkingLight2.enabled = false;
            }
            foreach (Light greenWalkingLight1 in greenWalkingLights1)
            {
                greenWalkingLight1.enabled = false;
            }
            foreach (Light greenWalkingLight2 in greenWalkingLights2)
            {
                greenWalkingLight2.enabled = true;
            }
        }
    }

    IEnumerator WaitTrafficLights()
    {
        lightsIndex = Random.Range(0, 2);
        while (true)
        {
            yield return new WaitForSeconds(30);
            if (lightsIndex == 0)
            {
                lightsIndex = 2;
            }
            else if (lightsIndex == 1)
            {
                lightsIndex = 3;
            }
            yield return new WaitForSeconds(9);
            if (lightsIndex == 2)
            {
                lightsIndex = 1;
            }
            else if (lightsIndex == 3)
            {
                lightsIndex = 0;
            }
        }
    }
}
