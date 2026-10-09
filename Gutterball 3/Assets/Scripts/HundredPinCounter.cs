using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HundredPinCounter : MonoBehaviour
{
    public GameHundredPin game;
    public static int pinCount = 100;

    private BallHundredPin ball;
    private int lastStandingCount = -1;
    private int lastSettledCount = 100;
    private int standing;
    private int pinFall;
    private int pinDown;

    // Use this for initialization
    void Start()
    {
        ball = GameObject.FindObjectOfType<BallHundredPin>();
    }

    // Update is called once per frame
    void Update()
    {
        game.pinsCounter = lastSettledCount - standing;
    }

    public void Reset()
    {
        lastSettledCount = 100;
        pinCount = lastSettledCount;
    }

    public void UpdateStandingCountAndSettle()
    {
        // Update the lastStandingCount
        // Call PinsHaveSettled() when they have
        int currentStanding = CountStanding();

        if (currentStanding != lastStandingCount)
        {
            lastStandingCount = currentStanding;
            return;
        }
    }

    public void PinsHaveSettled()
    {
        standing = CountStanding();
        pinFall = lastSettledCount - standing;
        lastSettledCount = standing;
        pinCount = lastSettledCount;
        game.Bowl(pinFall);

        lastStandingCount = -1; // Indicates pins have settled, and ball not back in box
    }

    int CountStanding()
    {
        int standing = 0;

        foreach (HundredPin pin in GameObject.FindObjectsOfType<HundredPin>())
        {
            if (pin.IsStanding())
            {
                standing++;
            }
        }

        return standing;
    }
}
