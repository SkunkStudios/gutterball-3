using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreDisplayBall3 : MonoBehaviour
{

    public enum Strikes { Strike, Double, Turkey }
    public Strikes strikes;

    public Text[] rollTexts, frameTexts;


    public void FillRolls(List<int> rolls)
    {
        string scoresString = FormatRolls(rolls);
        for (int i = 0; i < scoresString.Length; i++)
        {
            rollTexts[i].text = scoresString[i].ToString();
            //  turn ++;
        }

    }

    public void FillFrames(List<int> frames)
    {
        for (int i = 0; i < frames.Count; i++)
        {
            frameTexts[i].text = frames[i].ToString();
        }
    }

    public void ResetStrike()
    {
        strikes = Strikes.Strike;
    }

    public void AllStrike()
    {
        strikes++;
        if (strikes > Strikes.Turkey)
        {
            strikes = Strikes.Strike;
        }
    }

    public static string FormatRolls(List<int> rolls)
    {
        string output = "";

        for (int i = 0; i < rolls.Count; i++)
        {
            int box = output.Length + 1;                            // Score box 1 to 30 

            if (rolls[i] == 0 || box % 3 == 0 && rolls[i] == 10)
            {                                   // Always enter 0 as -
                output += "-";
            }
            else if (box % 2 >= 28 && rolls[i - 1] + rolls[i] == 10)
            {   // SPARE anywhere
                output += "/";
            }
            else if (box % 2 == 0 && rolls[i - 1] + rolls[i] == 10)
            {   // SPARE anywhere
                output += "/ ";
            }
            else if (box >= 28 && rolls[i] == 10)
            {               // STRIKE in frame 10
                output += "X";
            }
            else if (rolls[i] == 10)
            {                           // STRIKE in frame 1-9
                output += "  X";
            }
            else
            {
                output += rolls[i].ToString();
                // Normal 1-9 bowl
            }
        }

        return output;
    }
}