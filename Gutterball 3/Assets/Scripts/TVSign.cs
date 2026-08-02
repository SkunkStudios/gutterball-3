using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TVSign : MonoBehaviour
{

	public RawImage backScreen;
    public RawImage frontScreen;

    private Game game;
    private GameManager gameManager;
    private Texture2D[] screens;
    private int infoIndex;

    void Start ()
	{
        game = GameObject.FindObjectOfType<Game>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        screens = Resources.LoadAll<Texture2D>("TVScreen");
        BackScreenUI();
    }

    public void BackScreenUI()
    {
        if (gameManager.urlInfoScreen.Count == 0)
        {
            infoIndex = Random.Range(0, 2);
        }
        else if (gameManager.urlInfoScreen.Count > 0)
        {
            infoIndex = Random.Range(0, 3);
        }
        if (infoIndex == 0)
        {
            backScreen.texture = screens[Random.Range(0, screens.Length)];
        }
        else if (infoIndex == 1)
        {
            backScreen.texture = game.firstPersonCam;
        }
        else if (infoIndex == 2)
        {
            StartCoroutine(DownloadImage(gameManager.urlInfoScreen[Random.Range(0, gameManager.urlInfoScreen.Count)], backScreen));
        }
    }

    public void FrontScreenUI()
    {
        if (gameManager.urlInfoScreen.Count == 0)
        {
            infoIndex = Random.Range(0, 2);
        }
        else if (gameManager.urlInfoScreen.Count > 0)
        {
            infoIndex = Random.Range(0, 3);
        }
        if (infoIndex == 0)
        {
            frontScreen.texture = screens[Random.Range(0, screens.Length)];
        }
        else if (infoIndex == 1)
        {
            frontScreen.texture = game.firstPersonCam;
        }
        else if (infoIndex == 2)
        {
            StartCoroutine(DownloadImage(gameManager.urlInfoScreen[Random.Range(0, gameManager.urlInfoScreen.Count)], frontScreen));
        }
    }

    IEnumerator DownloadImage(string MediaUrl, RawImage screen)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(MediaUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                screen.texture = uwrTexture;
            }
        }
    }
}
