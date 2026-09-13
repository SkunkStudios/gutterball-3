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
		yield return new WaitForSeconds(1.5f);
        splashScreen.SetActive(true);
        yield return new WaitForSeconds(30f);
        splashScreen.SetActive(false);
        ronkatScreen.SetActive(true);
        yield return new WaitForSeconds(18f);
        ronkatScreen.SetActive(false);
        warningScreen.SetActive(true);
        yield return new WaitForSeconds(18f);
        warningScreen.SetActive(false);
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
