using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PinSetter : MonoBehaviour
{
    public Game game;
    public string alleySong;
    public Transform returnPoint;
    public Vector3 offset;
    public Vector3 pos;
    public Vector3 ballPos;
    public float rot;
    public CamReplay[] replays;
    public Transform[] reacts;
    public Vector3 winPos;
    public Vector3 winRot;
    public Vector3 scordCardPos;
    public float rotSetX;
    public float rotOffset;
    public float rotScoreCard;
    public float offsetY;
    public float offsetZ;
    public bool isGravity = true;
    public bool isSplash;
    public bool isRain;
    public bool isThunder;
    public ParticleSystem psDrop;
    public AnimationScript[] strikeAnimations;
    public AnimationScript[] spareAnimations;
    public AnimationScript[] introAnimations;
    public AnimationScript[] gutterballAnimations;
    public AnimationScript[] gutterballAnimationTwoTimes;
    public AnimationScript[] doubleAnimations;
    public AnimationScript[] turkeyAnimations;
    public BoxCollider gutter;
    public Material pinOn;
    public Material pinOff;

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if (GameManager.pinMode == GameManager.PinMode.Spare)
        {
            ResetPinsFall();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScooperPins()
    {
        animator.SetBool("Scooper", true);
    }

    public void SkipScooper()
    {
        animator.SetBool("Scooper", false);
    }

    public void StopScooper()
    {
        animator.StopPlayback();
    }

    public void DownPins()
    {
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.PinDown();
        }
    }

    public void RaisePins()
    {
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.Raise();
        }
    }

    public void StopPins()
    {
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.Stop();
        }
    }

    public void LowerPins()
    {
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.Lower();
        }
    }

    public void LandPins()
    {
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.Land();
        }
    }

    public void OutOfPins()
    {
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.OutOfPin();
        }
    }

    public void ResetPins()
    {
        for (int i = 0; i < GameObject.FindObjectOfType<Game>().pins.Length; i++)
        {
            GameObject.FindObjectOfType<Game>().pins[i].SetActive(true);
        }
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.Reset();
        }
    }

    public void ResetPinsFall()
    {
        for (int i = 0; i < GameObject.FindObjectOfType<Game>().pins.Length; i++)
        {
            GameObject.FindObjectOfType<Game>().pins[i].SetActive(true);
        }
        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            pin.pinLight.material = pinOff;
            pin.ResetFall(Random.Range(-1, 1));
        }
    }

    public void PerformAction(ActionMasterOld.Action action)
    {
        if (action == ActionMasterOld.Action.EndGame)
        {
            throw new UnityException("Don't know how to handle end game yet");
        }
    }

    public void PerformAction3(ActionMasterOldBall3.Action action)
    {
        if (action == ActionMasterOldBall3.Action.EndGame)
        {
            throw new UnityException("Don't know how to handle end game yet");
        }
    }
}
