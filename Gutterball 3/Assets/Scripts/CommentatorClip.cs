using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[System.Serializable]
public class VoiceClip
{
	public string[] voices;
    [HideInInspector]
	public int voiceIndex;
    [HideInInspector]
    public int voiceRandom;
    [HideInInspector]
    public int[] voiceCount;
    [HideInInspector]
    public int nextVoice;
}

public class CommentatorClip : MonoBehaviour
{
    public string commentators;
    public VoiceClip Strike;
    public VoiceClip Spare;
    public VoiceClip Gutterball;
    public VoiceClip Double;
    public VoiceClip Turkey;
    public VoiceClip Split;
    public VoiceClip Split710;
    public VoiceClip SplitPick;
    public VoiceClip Most;
    public VoiceClip Few;
    public VoiceClip One;
    public VoiceClip Miss;
    public VoiceClip EndBad;
    public VoiceClip EndOk;
    public VoiceClip EndGood;
    public VoiceClip EndGreat;
    public string intro;

    // Use this for initialization
    void Start ()
	{
        Strike.voiceCount = new int[Strike.voices.Length];
        Spare.voiceCount = new int[Spare.voices.Length];
        Gutterball.voiceCount = new int[Gutterball.voices.Length];
        Double.voiceCount = new int[Double.voices.Length];
        Turkey.voiceCount = new int[Turkey.voices.Length];
        Split.voiceCount = new int[Split.voices.Length];
        Split710.voiceCount = new int[Split710.voices.Length];
        SplitPick.voiceCount = new int[SplitPick.voices.Length];
        Most.voiceCount = new int[Most.voices.Length];
        Few.voiceCount = new int[Few.voices.Length];
        One.voiceCount = new int[One.voices.Length];
        Miss.voiceCount = new int[Miss.voices.Length];
        EndBad.voiceCount = new int[EndBad.voices.Length];
        EndOk.voiceCount = new int[EndOk.voices.Length];
        EndGood.voiceCount = new int[EndGood.voices.Length];
        EndGreat.voiceCount = new int[EndGreat.voices.Length];
        for (int i = 0; i < Strike.voiceCount.Length; i++)
        {
            Strike.voiceCount[i] = Strike.voiceCount.Length;
        }
        for (int i = 0; i < Spare.voiceCount.Length; i++)
        {
            Spare.voiceCount[i] = Spare.voiceCount.Length;
        }
        for (int i = 0; i < Gutterball.voiceCount.Length; i++)
        {
            Gutterball.voiceCount[i] = Gutterball.voiceCount.Length;
        }
        for (int i = 0; i < Double.voiceCount.Length; i++)
        {
            Double.voiceCount[i] = Double.voiceCount.Length;
        }
        for (int i = 0; i < Turkey.voiceCount.Length; i++)
        {
            Turkey.voiceCount[i] = Turkey.voiceCount.Length;
        }
        for (int i = 0; i < Split.voiceCount.Length; i++)
        {
            Split.voiceCount[i] = Split.voiceCount.Length;
        }
        for (int i = 0; i < Split710.voiceCount.Length; i++)
        {
            Split710.voiceCount[i] = Split710.voiceCount.Length;
        }
        for (int i = 0; i < SplitPick.voiceCount.Length; i++)
        {
            SplitPick.voiceCount[i] = SplitPick.voiceCount.Length;
        }
        for (int i = 0; i < Most.voiceCount.Length; i++)
        {
            Most.voiceCount[i] = Most.voiceCount.Length;
        }
        for (int i = 0; i < Few.voiceCount.Length; i++)
        {
            Few.voiceCount[i] = Few.voiceCount.Length;
        }
        for (int i = 0; i < One.voiceCount.Length; i++)
        {
            One.voiceCount[i] = One.voiceCount.Length;
        }
        for (int i = 0; i < Miss.voiceCount.Length; i++)
        {
            Miss.voiceCount[i] = Miss.voiceCount.Length;
        }
        for (int i = 0; i < EndBad.voiceCount.Length; i++)
        {
            EndBad.voiceCount[i] = EndBad.voiceCount.Length;
        }
        for (int i = 0; i < EndOk.voiceCount.Length; i++)
        {
            EndOk.voiceCount[i] = EndOk.voiceCount.Length;
        }
        for (int i = 0; i < EndGood.voiceCount.Length; i++)
        {
            EndGood.voiceCount[i] = EndGood.voiceCount.Length;
        }
        for (int i = 0; i < EndGreat.voiceCount.Length; i++)
        {
            EndGreat.voiceCount[i] = EndGreat.voiceCount.Length;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void ResetVoice()
    {
        Strike.voiceIndex = Random.Range(0, Strike.voices.Length);
        Spare.voiceIndex = Random.Range(0, Spare.voices.Length);
        Gutterball.voiceIndex = Random.Range(0, Gutterball.voices.Length);
        Double.voiceIndex = Random.Range(0, Double.voices.Length);
        Turkey.voiceIndex = Random.Range(0, Turkey.voices.Length);
        Split.voiceIndex = Random.Range(0, Split.voices.Length);
        Split710.voiceIndex = Random.Range(0, Split710.voices.Length);
        SplitPick.voiceIndex = Random.Range(0, SplitPick.voices.Length);
        Most.voiceIndex = Random.Range(0, Most.voices.Length);
        Few.voiceIndex = Random.Range(0, Few.voices.Length);
        One.voiceIndex = Random.Range(0, One.voices.Length);
        Miss.voiceIndex = Random.Range(0, Miss.voices.Length);
        EndBad.voiceIndex = Random.Range(0, EndBad.voices.Length);
        EndOk.voiceIndex = Random.Range(0, EndOk.voices.Length);
        EndGood.voiceIndex = Random.Range(0, EndGood.voices.Length);
        EndGreat.voiceIndex = Random.Range(0, EndGreat.voices.Length);
        for (int i = 0; i < Strike.voiceCount.Length; i++)
        {
            Strike.voiceCount[i] = Strike.voiceCount.Length;
        }
        for (int i = 0; i < Spare.voiceCount.Length; i++)
        {
            Spare.voiceCount[i] = Spare.voiceCount.Length;
        }
        for (int i = 0; i < Gutterball.voiceCount.Length; i++)
        {
            Gutterball.voiceCount[i] = Gutterball.voiceCount.Length;
        }
        for (int i = 0; i < Double.voiceCount.Length; i++)
        {
            Double.voiceCount[i] = Double.voiceCount.Length;
        }
        for (int i = 0; i < Turkey.voiceCount.Length; i++)
        {
            Turkey.voiceCount[i] = Turkey.voiceCount.Length;
        }
        for (int i = 0; i < Split.voiceCount.Length; i++)
        {
            Split.voiceCount[i] = Split.voiceCount.Length;
        }
        for (int i = 0; i < Split710.voiceCount.Length; i++)
        {
            Split710.voiceCount[i] = Split710.voiceCount.Length;
        }
        for (int i = 0; i < SplitPick.voiceCount.Length; i++)
        {
            SplitPick.voiceCount[i] = SplitPick.voiceCount.Length;
        }
        for (int i = 0; i < Most.voiceCount.Length; i++)
        {
            Most.voiceCount[i] = Most.voiceCount.Length;
        }
        for (int i = 0; i < Few.voiceCount.Length; i++)
        {
            Few.voiceCount[i] = Few.voiceCount.Length;
        }
        for (int i = 0; i < One.voiceCount.Length; i++)
        {
            One.voiceCount[i] = One.voiceCount.Length;
        }
        for (int i = 0; i < Miss.voiceCount.Length; i++)
        {
            Miss.voiceCount[i] = Miss.voiceCount.Length;
        }
        for (int i = 0; i < EndBad.voiceCount.Length; i++)
        {
            EndBad.voiceCount[i] = EndBad.voiceCount.Length;
        }
        for (int i = 0; i < EndOk.voiceCount.Length; i++)
        {
            EndOk.voiceCount[i] = EndOk.voiceCount.Length;
        }
        for (int i = 0; i < EndGood.voiceCount.Length; i++)
        {
            EndGood.voiceCount[i] = EndGood.voiceCount.Length;
        }
        for (int i = 0; i < EndGreat.voiceCount.Length; i++)
        {
            EndGreat.voiceCount[i] = EndGreat.voiceCount.Length;
        }
    }
}
