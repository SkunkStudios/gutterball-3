using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public AudioSource introMusic;
    public AudioSource introThrow;
    public AudioSource introRoll;
    public AudioSource introHitPin;
    public IntroRigidbody introRB1;
    public IntroRigidbody introRB2;
    public GameObject loadScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.isMusic)
        {
            introMusic.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject chars in GameObject.FindGameObjectsWithTag("Chars"))
        {
            chars.SetActive(GameManager.isChars);
        }
        foreach (GameObject particles in GameObject.FindGameObjectsWithTag("Particles"))
        {
            particles.SetActive(GameManager.isParticle);
        }
        if (Input.GetMouseButtonDown(0))
        {
            IntroSkip();
        }
    }

    public void IntroThrow()
    {
        if (GameManager.isSound)
        {
            introThrow.Play();
        }
    }

    public void IntroRoll()
    {
        if (GameManager.isSound)
        {
            introRoll.Play();
        }
    }

    public void IntroHitPin()
    {
        introRoll.Stop();
        if (GameManager.isSound)
        {
            introHitPin.Play();
        }
    }

    public void IntroBowl1()
    {
        introRB1.BowlForce();
    }

    public void IntroBowl2()
    {
        introRB2.BowlForce();
    }

    public void IntroSkip()
    {
        PlayerPrefs.SetInt("IntroSkip", 1);
        introMusic.Stop();
        introThrow.Stop();
        introRoll.Stop();
        introHitPin.Stop();
        loadScreen.SetActive(true);
        SceneManager.LoadScene("Main");
    }
}
