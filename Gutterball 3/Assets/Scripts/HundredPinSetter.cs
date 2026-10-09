using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HundredPinSetter : MonoBehaviour
{
    public GameHundredPin game;
    public Vector3 offset;
    public Vector3 pos;
    public Vector3 ballPos;
    public float rot;
    public Vector3 winPos;
    public Vector2 winRot;
    public float rotSetX;
    public float rotOffset;
    public float offsetY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DownPins()
    {
        foreach (HundredPin pin in GameObject.FindObjectsOfType<HundredPin>())
        {
            pin.PinDown();
        }
    }

    public void OutOfPins()
    {
        foreach (HundredPin pin in GameObject.FindObjectsOfType<HundredPin>())
        {
            pin.OutOfPin();
        }
    }

    public void ResetPins()
    {
        for (int i = 0; i < GameObject.FindObjectOfType<GameHundredPin>().pins.Length; i++)
        {
            GameObject.FindObjectOfType<GameHundredPin>().pins[i].SetActive(true);
        }
        foreach (HundredPin pin in GameObject.FindObjectsOfType<HundredPin>())
        {
            pin.Reset();
        }
    }

    public void PerformAction(ActionMasterOldHundredPin.Action action)
    {
        if (action == ActionMasterOldHundredPin.Action.EndGame)
        {
            throw new UnityException("Don't know how to handle end game yet");
        }
    }
}
