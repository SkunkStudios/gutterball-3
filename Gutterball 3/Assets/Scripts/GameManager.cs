using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Skunk Studios by Upload to Github
public class GameManager : MonoBehaviour
{
    public enum PinMode { Tenpin, Spare }
    public static PinMode pinMode;
    public enum Alley { Retro, Zen, Jungle, Iceberg, Wacky, Mineshaft, Barnyard, Cosmic }
    public static Alley chooseAlleys;
    public int[] isLockAlleys = new int[8];
    public string[] nameAlleys = new string[8];
    public Sprite[] spriteAlleys = new Sprite[8];
    public string[] AlleyNames { get { return nameAlleys; } }

    public ChooseBall[] chooseBalls;
    public CompuObj[] compuObj = new CompuObj[17];
    public static bool isMusic = true;
    public static bool isSound = true;
    public static bool isCrowd = true;
    public static bool isVoice = true;
    public static bool isChars = true;
    public static bool isParticle = true;
    public static bool isReflect = true;
    public static bool isWeather = true;
    public static bool isShake = true;
    public static bool isHighScore = false;
    public static int qualityIndex;
    public static int resolutionIndex;
    public int lockRegistered;
    public List<string> urlInfoScreen = new List<string>();
    public static bool isOpening = true;
    public static int startBall = 0;
    public static int chooseBallIndex = 0;
    public static int turnBalls1 = 0;
    public static int turnBalls2 = 0;
    public static int turnBalls3 = 0;
    public static int turnBalls4 = 0;
    public static int turnBallsCPU = 0;
    public static int turnNameIndex1;
    public static int turnNameIndex2;
    public static int turnNameIndex3;
    public static int turnNameIndex4;
    public static Resolution[] resolutions;
    public Material lockBallMat;
    public static int unlockRegister;
    public static int unlockBallEarn;
    public static int unlockBallScore;
    public static int unlockBallSpare;
    public List<PlayerObj> bowler = new List<PlayerObj>();
    public List<ScoreBowler> r_hs = new List<ScoreBowler>();
    public List<ScoreBowler> w_hs = new List<ScoreBowler>();
    public List<ScoreBowler> i_hs = new List<ScoreBowler>();
    public List<ScoreBowler> j_hs = new List<ScoreBowler>();
    public List<ScoreBowler> z_hs = new List<ScoreBowler>();
    public List<ScoreBowler> c_hs = new List<ScoreBowler>();
    public List<ScoreBowler> b_hs = new List<ScoreBowler>();
    public List<ScoreBowler> m_hs = new List<ScoreBowler>();
    public static int moneys;
    public static int bombBalls;
    public static int hyperBalls;
    public static int lightningBalls;

    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
	{
        urlInfoScreen = FileData.ReadListFromSAV<string>("InfoURL");
        bowler = FileData.ReadListFromSAV<PlayerObj>("SaveBowler");
        r_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Retro");
        w_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Wacky");
        i_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Iceberg");
        j_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Jungle");
        z_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Zen");
        c_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Cosmic");
        b_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Barnyard");
        m_hs = FileData.ReadListFromSAV<ScoreBowler>("HS_Mineshaft");
        resolutions = Screen.resolutions;
        SavePrefs();
    }

    public void SavePrefs()
    {
        QualitySettings.GetQualityLevel();
        isMusic = (PlayerPrefs.GetInt("SaveMusic") == 0);
        isSound = (PlayerPrefs.GetInt("SaveSound") == 0);
        isCrowd = (PlayerPrefs.GetInt("SaveCrowd") == 0);
        isVoice = (PlayerPrefs.GetInt("SaveVoice") == 0);
        isChars = (PlayerPrefs.GetInt("SaveChars") == 0);
        isParticle = (PlayerPrefs.GetInt("SaveParticle") == 0);
        isReflect = (PlayerPrefs.GetInt("SaveReflect") == 0);
        isWeather = (PlayerPrefs.GetInt("SaveWeather") == 0);
        isShake = (PlayerPrefs.GetInt("SaveShake") == 0);
        qualityIndex = PlayerPrefs.GetInt("SaveQuality", QualitySettings.GetQualityLevel());
        resolutionIndex = PlayerPrefs.GetInt("SaveResolution", resolutions.Length - 1);
        unlockRegister = PlayerPrefs.GetInt("UnlockRegister", lockRegistered);
        unlockBallEarn = PlayerPrefs.GetInt("SaveBallEarn", 4);
        unlockBallScore = PlayerPrefs.GetInt("SaveBallScore", 40);
        unlockBallSpare = PlayerPrefs.GetInt("SaveBallSpare", 50);
        turnNameIndex1 = PlayerPrefs.GetInt("SavePlayer1", 0);
        turnNameIndex2 = PlayerPrefs.GetInt("SavePlayer2", 1);
        turnNameIndex3 = PlayerPrefs.GetInt("SavePlayer3", 2);
        turnNameIndex4 = PlayerPrefs.GetInt("SavePlayer4", 3);
        if (turnNameIndex1 >= bowler.Count)
        {
            turnNameIndex1 = 0;
            PlayerPrefs.SetInt("SavePlayer1", 0);
        }
        if (turnNameIndex2 >= bowler.Count)
        {
            turnNameIndex2 = 1;
            PlayerPrefs.SetInt("SavePlayer2", 1);
        }
        if (turnNameIndex3 >= bowler.Count)
        {
            turnNameIndex3 = 2;
            PlayerPrefs.SetInt("SavePlayer3", 2);
        }
        if (turnNameIndex4 >= bowler.Count)
        {
            turnNameIndex4 = 3;
            PlayerPrefs.SetInt("SavePlayer4", 3);
        }
        for (int prefsAlleys = 0; prefsAlleys < isLockAlleys.Length; prefsAlleys++)
        {
            isLockAlleys[prefsAlleys] = PlayerPrefs.GetInt("SaveAlleys" + prefsAlleys, isLockAlleys[prefsAlleys]);
        }
        for (int prefsBalls = 0; prefsBalls < chooseBalls.Length; prefsBalls++)
        {
            chooseBalls[prefsBalls].isLock = PlayerPrefs.GetInt("SaveBalls" + prefsBalls, chooseBalls[prefsBalls].isLock);
        }
        chooseAlleys = (Alley)PlayerPrefs.GetInt("ChooseAlleys");
        pinMode = (PinMode)PlayerPrefs.GetInt("PinModes");
        moneys = PlayerPrefs.GetInt("SaveMoney", 5000);
        bombBalls = PlayerPrefs.GetInt("SaveBomb", 3);
        hyperBalls = PlayerPrefs.GetInt("SaveHyper", 3);
        lightningBalls = PlayerPrefs.GetInt("SaveLightning", 3);
    }

    public IEnumerator DownloadTexture(string MediaUrl, Material ballMat)
    {
        for (int customBalls = 0; customBalls < chooseBalls.Length; customBalls++)
        {
            if (chooseBalls[customBalls].urlTextureBall != "")
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
                        ballMat.mainTexture = uwrTexture;
                    }
                }
            }
        }
    }
}
