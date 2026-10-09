using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ScoreMasterBall3
{

    // Returns a list of cumulative scores, like a normal score card.
    public static List<int> ScoreCumulative(List<int> rolls)
    {
        List<int> cumulativeScores = new List<int>();
        int runningTotal = 0;

        foreach (int frameScore in ScoreFrames(rolls))
        {
            runningTotal += frameScore;
            cumulativeScores.Add(runningTotal);
        }

        return cumulativeScores;
    }

    // Return a list of individual frame scores.
    public static List<int> ScoreFrames(List<int> rolls)
    {
        List<int> frames = new List<int>();

        // Index i points to 3rd bowl of frame
        for (int i = 1; i < rolls.Count; i += 3)
        {
            if (frames.Count == 10) { break; }              // Prevents 11th frame score

            if (rolls[i - 2] + rolls[i - 1] + rolls[i] < 10)
            {               // Normal "OPEN" frame
                frames.Add(rolls[i - 2] + rolls[i - 1] + rolls[i]);
            }

            if (rolls.Count - i <= 1) { break; }                // Ensure at least 1 look-ahead available

            if (rolls[i - 1] == 10)
            {
                i--;                                        // STRIKE frame has just one bowl
                frames.Add(10 + rolls[i + 1] + rolls[i + 2]);
            }
            else if (rolls[i - 1] + rolls[i] == 10)
            {       // SPARE bonus
                frames.Add(10 + rolls[i + 1]);
            }
            else if (rolls[i - 2] + rolls[i - 1] + rolls[i] == 10)
            {       // TEN bonus
                frames.Add(10);
            }
        }
        return frames;
    }
}