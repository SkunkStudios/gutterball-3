using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
	public GameObject splashScreen;
	public GameObject ronkatScreen;
	public GameObject warningScreen;
	public GameObject loadScreen;
    public GameObject websiteText;
    public GameObject presentsText;

    // Use this for initialization
    void Start ()
	{
		StartCoroutine(StartSplash());
        if (GameObject.FindObjectOfType<GameManager>().gameDistribution == GameManager.GameDistribution.None && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            websiteText.SetActive(true);
        }
        else
        {
            presentsText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update ()
	{
		
	}

	IEnumerator StartSplash()
	{
        splashScreen.GetComponent<Animator>().speed = 1 / Time.timeScale;
        yield return new WaitForSeconds(0.5f * Time.timeScale);
        splashScreen.SetActive(true);
        yield return new WaitForSeconds(10f * Time.timeScale);
        splashScreen.SetActive(false);
        ronkatScreen.SetActive(true);
        yield return new WaitForSeconds(6f * Time.timeScale);
        ronkatScreen.SetActive(false);
        loadScreen.SetActive(true);
        if (PlayerPrefs.GetInt("IntroSkip") == 0 && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            SceneManager.LoadScene("Intro");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
