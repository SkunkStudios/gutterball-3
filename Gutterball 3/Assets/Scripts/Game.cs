using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PinSplit
{
    public bool isPin1;
    public bool isPin2;
    public bool isPin3;
    public bool isPin4;
    public bool isPin5;
    public bool isPin6;
    public bool isPin7;
    public bool isPin8;
    public bool isPin9;
    public bool isPin10;
}

public class Game : MonoBehaviour
{
    public enum BallPowerUps { Off, Bomb, Hyper, Lightning }
    public BallPowerUps powerUps;
    public enum BallType { MoveX, ThrowBall, SpinBall, FallBall }
    public BallType ballType;
    public enum CameraType { Intro, MoveX, DropBall, FollowBall, LookBall, Replay, Replay2, Anim, ReturnBall, MoveCam, ReactCam }
    public CameraType camType;
    public static GameManager.Alley alleyLockType;
    public enum Players { OnePlayer, TwoPlayer, ThreePlayer, FourPlayer, Computer }
    public static Players allPlayers;
    public enum GameState { Menu, Intro, Game, Replay, EndGame }
    public static GameState type;
    public enum Commentators { Baxter, Maria, Natasha, Jensen, Master }
    public static Commentators voices1;
    public static Commentators voices2;
    private List<int> rolls1 = new List<int>();
    private List<int> rolls2 = new List<int>();
    private List<int> rolls3 = new List<int>();
    private List<int> rolls4 = new List<int>();
    private List<int> scores1 = new List<int>();
    private List<int> scores2 = new List<int>();
    private List<int> scores3 = new List<int>();
    private List<int> scores4 = new List<int>();
    private int score1;
    private int score2;
    private int score3;
    private int score4;
    private int strikes1;
    private int strikes2;
    private int strikes3;
    private int strikes4;
    private int spares1;
    private int spares2;
    private int spares3;
    private int spares4;
    private int gutters1;
    private int gutters2;
    private int gutters3;
    private int gutters4;
    private int frames1 = 1;
    private int frames2 = 1;
    private int frames3 = 1;
    private int frames4 = 1;
    private int allStrikes1 = 1;
    private int allStrikes2 = 1;
    private int allStrikes3 = 1;
    private int allStrikes4 = 1;
    private int turns = 0;
    private int nextTurn = 0;
    private int playerTurn = 0;
    private int ballTurn = 0;
    private int wins = 1;
    private int stage = 1;
    private int spareBalls = 5;

    public GameObject[] alleys = new GameObject[8];
    public GameObject[] alleyScores = new GameObject[8];
    public enum AnimationScenes { Off, Strike, Spare, Double, Turkey }
    public AnimationScenes animations;
    public enum Crowds { NoCrowd, CheerBig, CheerMed, CrowdOk, CrowdHohum, CrowdCrap, Laugh, Oooh, Firework }
    public Crowds crowdType;
    public ScoreDisplay[] scoreDisplay;
    public GameObject[] scoreCards = new GameObject[2];
    public GameObject[] playersBallTwo = new GameObject[4];
    public GameObject[] winGameBalls = new GameObject[18];
    public Text[] winGameName1;
    public Text[] winGameName2;
    public Text[] winGameName3;
    public Text[] winGameName4;
    public Text[] winGameNameCPU;
    public Text[] winGameScore1;
    public Text[] winGameScore2;
    public Text[] winGameScore3;
    public Text[] winGameScore4;
    public Text[] winGamePrize1;
    public Text[] winGamePrize2;
    public Text[] winGamePrize3;
    public Text[] winGamePrize4;
    public int pinsCounter;
    public int maxBalls = 2;
    public int throwBall = 0;
    public int gutterAnimation = 0;
    public bool isPin = false;
    public PinCounter pinCounter;
    public AudioSource opening;
    public AudioSource crowdAudio;
    public AudioSource rollCrowd;
    public AudioSource winCrowd;
    public AudioSource commentatorAudio;
    public AudioSource fireworksMulti;
    public AudioSource sfx;
    public AudioSource rainPorch;
    public bool isSplit;
    public PinSplit[] pinSplits;
    public CameraFollow cameraFollow;
    public bool isComputer;
    public bool isReplay;
    public bool isReplayRecord = false;
    public bool isCurrentReplay = false;
    public float currentReplayIndex;
    public GameObject customBallButton;
    public GameObject startButton;
    public GameObject menuRegisterButton;
    public GameObject[] ballsRegisterButton;
    public GameObject[] nextButton;
    public GameObject[] bowlButton;
    public GameObject alleyRegistered;
    public GameObject ballLocked;
    public GameObject ballNeed;
    public GameObject ballUnlock;
    public GameObject ballUnlocked;
    public GameObject ballRegistered;
    public GameObject bowlerUIElement;
    public Transform bowlerWrapper;
    public GameObject scoreUIElement;
    public Transform[] scoreWrapper = new Transform[8];
    public int regCount = 0;
    public GameObject menuCam;
    public GameObject gameCam;
    public GameObject scoreCardCam;
    public RenderTexture firstPersonCam;
    public Animation thunderAnimation;
    public GameObject menuUI;
    public GameObject gameUI;
    public GameObject replayText;
    public GameObject ballObject;
    public MeshRenderer ballRender;
    public MeshRenderer winBallRender1;
    public MeshRenderer winBallRender2;
    public MeshRenderer winBallRender3;
    public MeshRenderer winBallRender4;
    public MeshRenderer winBallRenderCPU;
    public Text alleyText;
    public Text playerNameText;
    public Text ballNameText;
    public Text ballDataText;
    public Text ballNameMenuText;
    public Text ballDataMenuText;
    public Text ballDataCustomText;
    public Text bowlerText;
    public Text qualityText;
    public Text resolutionText;
    public TextMesh playerName1;
    public TextMesh playerName2;
    public TextMesh playerName3;
    public TextMesh playerName4;
    public GameObject playerName;
    public Text scoreTextBall2;
    public Text ballsText;
    public Text stagesText;
    public Text stagesWinText;
    public GameObject chooseBallUI;
    public GameObject powerUpUI;
    public Image selectAlleysUI;
    public GameObject[] trueObjects;
    public GameObject[] falseObjects;
    public GameObject loadingUI;
    public GameObject menuAlleyUI;
    public GameObject highScoreUI;
    public GameObject registerUI;
    public GameObject lockAlleyUI;
    public GameObject unlockAlleyUI;
    public GameObject unlockedText;
    public GameObject unlockedAlley;
    public GameObject unlockedBallBeat;
    public GameObject unlockedBallEarn;
    public GameObject unlockedBallScore;
    public GameObject unlockedBallSpare;
    public Text lockAlleyText;
    public GameObject registerFail;
    public GameObject registerComplete;
    public InputField keyField;
    public InputField customBallNameField;
    public Slider customBallLbs;
    public Slider customBallSpeed;
    public Slider customBallSpin;
    public InputField bowlerField;
    public InputField customBallFileField;
    public InputField infoFileField;
    public GameObject[] pins;
    public GameObject sendingEmail;
    public GameObject emailSent;
    public GameObject arrowBowler;
    public Button[] bowlerButton = new Button[4];
    public AudioClip bowling1;
    public AudioClip bowling2;
    public AudioClip bowling4;
    public AudioClip bowling6;
    public AudioClip bowling10;
    public GameObject fireworks;
    public GameObject[] falseScoreCardsUI;
    public GameObject gutterHintUI;
    public Text moneyText;
    public Text bombBallText;
    public Text hyperBallText;
    public Text lightningBallText;
    public Text bombShopText;
    public Text hyperShopText;
    public Text lightningShopText;
    public Button bombBallButton;
    public Button hyperBallButton;
    public Button lightningBallButton;

    private AudioSource music;
    private GameObject[] sounds;
    private bool isIntro = true;
    private GameManager gameManager;
    private Commentator commentator;
    private Ball ball;
    private Pin pin1;
    private Pin pin2;
    private Pin pin3;
    private Pin pin4;
    private Pin pin5;
    private Pin pin6;
    private Pin pin7;
    private Pin pin8;
    private Pin pin9;
    private Pin pin10;
    private Crowd crowd;
    private Coroutine b;
    private ActionReplay[] replays;
    private bool is710;
    private int commentatorIndex = 0;
    private bool isEndGame;
    private int strikeAnimIndex;
    private int spareAnimIndex;
    private int introAnimIndex;
    private int gbAnimIndex;
    private int gbAnimIndex2X;
    private int doubleAnimIndex;
    private int turkeyAnimIndex;
    private int replayIndex;
    private int reactIndex;
    private float replayTime = 0;
    private int rainIndex = 0;
    private bool isAnim = false;
    private int chargeBallIndex;
    private bool isResetPins = false;
    private int pinCounts;
    private int hintCount;
    private int addCash = 0;
    public delegate void OnHighscoreListChanged(List<ScoreBowler> list);
    public static event OnHighscoreListChanged onHighscoreListChanged;

    // Use this for initialization
    void Start ()
	{
        pinCounter.Reset();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        crowdAudio = GameObject.Find("Crowd").GetComponent<AudioSource>();
        rollCrowd = GameObject.Find("RollingCrowd").GetComponent<AudioSource>();
        winCrowd = GameObject.Find("WinCrowd").GetComponent<AudioSource>();
        sfx = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        rainPorch = GameObject.Find("RainPorch").GetComponent<AudioSource>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.SavePrefs();
        for (int i = 0; i < gameManager.bowler.Count; i++)
        {
            GameObject element = Instantiate(bowlerUIElement, bowlerWrapper) as GameObject;
            element.GetComponent<BowlerUI>().BowlerUpdate(gameManager.bowler[i].playerName, gameManager.bowler[i].playerMoney, gameManager.bowler[i].playerWin, gameManager.bowler[i].playerLoss, gameManager.bowler[i].playerStrikes, gameManager.bowler[i].playerSpares, gameManager.bowler[i].playerGutters);
        }
        for (int i = 0; i < gameManager.r_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[0]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.r_hs[i].playerName, gameManager.r_hs[i].playerScore, gameManager.r_hs[i].playerStrikes, gameManager.r_hs[i].playerSpares, gameManager.r_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.z_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[1]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.z_hs[i].playerName, gameManager.z_hs[i].playerScore, gameManager.z_hs[i].playerStrikes, gameManager.z_hs[i].playerSpares, gameManager.z_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.j_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[2]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.j_hs[i].playerName, gameManager.j_hs[i].playerScore, gameManager.j_hs[i].playerStrikes, gameManager.j_hs[i].playerSpares, gameManager.j_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.i_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[3]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.i_hs[i].playerName, gameManager.i_hs[i].playerScore, gameManager.i_hs[i].playerStrikes, gameManager.i_hs[i].playerSpares, gameManager.i_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.w_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[4]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.w_hs[i].playerName, gameManager.w_hs[i].playerScore, gameManager.w_hs[i].playerStrikes, gameManager.w_hs[i].playerSpares, gameManager.w_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.m_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[5]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.m_hs[i].playerName, gameManager.m_hs[i].playerScore, gameManager.m_hs[i].playerStrikes, gameManager.m_hs[i].playerSpares, gameManager.m_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.b_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[6]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.b_hs[i].playerName, gameManager.b_hs[i].playerScore, gameManager.b_hs[i].playerStrikes, gameManager.b_hs[i].playerSpares, gameManager.b_hs[i].playerGutters);
        }
        for (int i = 0; i < gameManager.c_hs.Count; i++)
        {
            GameObject element = Instantiate(scoreUIElement, scoreWrapper[7]) as GameObject;
            element.GetComponent<ScoreUI>().BowlerUpdate(i + 1, gameManager.c_hs[i].playerName, gameManager.c_hs[i].playerScore, gameManager.c_hs[i].playerStrikes, gameManager.c_hs[i].playerSpares, gameManager.c_hs[i].playerGutters);
        }
        for (int customBalls = 0; customBalls < gameManager.chooseBalls.Length; customBalls++)
        {
            if (customBalls > 84)
            {
                gameManager.chooseBalls[customBalls].ballName = PlayerPrefs.GetString("CustomBallName" + customBalls, gameManager.chooseBalls[customBalls].ballName);
                gameManager.chooseBalls[customBalls].lbs = PlayerPrefs.GetInt("CustomBallLbs" + customBalls, 12);
                gameManager.chooseBalls[customBalls].speed = PlayerPrefs.GetInt("CustomBallSpeed" + customBalls, 55);
                gameManager.chooseBalls[customBalls].spin = PlayerPrefs.GetInt("CustomBallSpin" + customBalls, 50);
                gameManager.chooseBalls[customBalls].urlTextureBall = PlayerPrefs.GetString("CustomBallURL" + customBalls);
                StartCoroutine(gameManager.DownloadTexture(PlayerPrefs.GetString("CustomBallURL" + customBalls), gameManager.chooseBalls[customBalls].ballMat));
            }
        }
        switch (GameManager.chooseAlleys)
        {
            case GameManager.Alley.Retro:
                alleys[0].SetActive(true);
                break;
            case GameManager.Alley.Wacky:
                alleys[1].SetActive(true);
                break;
            case GameManager.Alley.Iceberg:
                alleys[2].SetActive(true);
                break;
            case GameManager.Alley.Jungle:
                alleys[3].SetActive(true);
                break;
            case GameManager.Alley.Zen:
                alleys[4].SetActive(true);
                break;
            case GameManager.Alley.Cosmic:
                alleys[5].SetActive(true);
                break;
            case GameManager.Alley.Barnyard:
                alleys[6].SetActive(true);
                break;
            case GameManager.Alley.Mineshaft:
                alleys[7].SetActive(true);
                break;
        }
        foreach (GameObject chars in GameObject.FindGameObjectsWithTag("Chars"))
        {
            chars.SetActive(GameManager.isChars);
        }
        foreach (GameObject particles in GameObject.FindGameObjectsWithTag("Particles"))
        {
            particles.SetActive(GameManager.isParticle);
        }
        ball = GameObject.FindObjectOfType<Ball>();
        switch (GameManager.pinMode)
        {
            case GameManager.PinMode.Tenpin:
                maxBalls = 2;
                scoreCards[1].SetActive(true);
                playerName.SetActive(true);
                break;
            case GameManager.PinMode.Spare:
                maxBalls = 1;
                scoreCards[0].SetActive(true);
                playerName.SetActive(false);
                break;
        }
        crowd = GameObject.FindObjectOfType<Crowd>();
        commentator = GameObject.FindObjectOfType<Commentator>();
        pin1 = GameObject.Find("Pin (1)").GetComponent<Pin>();
        pin2 = GameObject.Find("Pin (2)").GetComponent<Pin>();
        pin3 = GameObject.Find("Pin (3)").GetComponent<Pin>();
        pin4 = GameObject.Find("Pin (4)").GetComponent<Pin>();
        pin5 = GameObject.Find("Pin (5)").GetComponent<Pin>();
        pin6 = GameObject.Find("Pin (6)").GetComponent<Pin>();
        pin7 = GameObject.Find("Pin (7)").GetComponent<Pin>();
        pin8 = GameObject.Find("Pin (8)").GetComponent<Pin>();
        pin9 = GameObject.Find("Pin (9)").GetComponent<Pin>();
        pin10 = GameObject.Find("Pin (10)").GetComponent<Pin>();
        pins = GameObject.FindGameObjectsWithTag("Pin");
        replays = GameObject.FindObjectsOfType<ActionReplay>();
        reactIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().reacts.Length);
        if (GameManager.pinMode == GameManager.PinMode.Spare)
        {
            GameObject.FindObjectOfType<PinSetter>().ResetPinsFall();
        }
        if (GameManager.isHighScore)
        {
            if (GameManager.unlockRegister == 0)
            {
                highScoreUI.SetActive(true);
                ShowAlleyScores();
            }
            else if (GameManager.unlockRegister == 1)
            {
                registerUI.SetActive(true);
            }
        }
        else
        {
            menuAlleyUI.SetActive(true);
        }
        unlockedAlley.GetComponent<Text>().text = gameManager.nameAlleys[(int)alleyLockType] + " Alley";
        unlockedBallBeat.GetComponent<Text>().text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName + " Ball";
        unlockedBallEarn.GetComponent<Text>().text = gameManager.chooseBalls[GameManager.unlockBallEarn].ballName + " Ball";
        unlockedBallScore.GetComponent<Text>().text = gameManager.chooseBalls[GameManager.unlockBallScore].ballName + " Ball";
        unlockedBallSpare.GetComponent<Text>().text = gameManager.chooseBalls[GameManager.unlockBallSpare].ballName + " Ball";
        introAnimIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().introAnimations.Length);
        scoreCardCam.transform.position = new Vector3(GameObject.FindObjectOfType<PinSetter>().scordCardPos.x, GameObject.FindObjectOfType<PinSetter>().scordCardPos.y, GameObject.FindObjectOfType<PinSetter>().scordCardPos.z);
        scoreCardCam.transform.rotation = Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotScoreCard, 180, 0);
        if (GameManager.isOpening && type == GameState.Menu)
        {
            music.clip = Resources.Load<AudioClip>("Music/menu");
            ChargeAlleys();
            StartChargeBalls();
            opening.Play();
        }
        if (!music.isPlaying && !GameManager.isOpening && type == GameState.Menu)
        {
            music.clip = Resources.Load<AudioClip>("Music/menu");
            music.Play();
        }
        if (type == GameState.Menu)
        {
            menuUI.SetActive(true);
            gameCam.SetActive(false);
            RandomChargeBall();
            ball.ResetBowl();
            ball.ResetCam();
        }
        else
        {
            AlleyTheme(GameObject.FindObjectOfType<PinSetter>().alleySong);
            type = GameState.Intro;
            gameUI.SetActive(true);
            menuCam.SetActive(false);
            chooseBallUI.SetActive(false);
            powerUpUI.SetActive(false);
            ball.Reset();
            music.Play();
            camType = CameraType.Anim;
            if (GameManager.isVoice && GameManager.pinMode != GameManager.PinMode.Spare)
            {
                commentator.Intro(commentatorAudio);
                commentatorAudio.Play();
            }
            GameObject.FindObjectOfType<PinSetter>().introAnimations[introAnimIndex].PlayAnimation();
            StartCoroutine(IntroTime());
            if (GameManager.pinMode == GameManager.PinMode.Spare)
            {
                if (GameManager.startBall <= 0)
                {
                    GameManager.startBall = 1;
                }
                playerName1.text = "3";
                playerName2.text = "2";
                playerName3.text = "1";
                playerName4.text = "Go!";
            }
            if (allPlayers == Players.OnePlayer && GameManager.pinMode != GameManager.PinMode.Spare)
            {
                if (GameManager.startBall <= 0)
                {
                    GameManager.startBall = 1;
                }
                playerName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            }
            else if (allPlayers == Players.TwoPlayer && GameManager.pinMode != GameManager.PinMode.Spare)
            {
                if (GameManager.startBall <= 0 || GameManager.startBall <= 1)
                {
                    GameManager.startBall = 2;
                }
                playerName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                playerName2.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
            }
            else if (allPlayers == Players.ThreePlayer && GameManager.pinMode != GameManager.PinMode.Spare)
            {
                if (GameManager.startBall <= 0 || GameManager.startBall <= 1 || GameManager.startBall <= 2)
                {
                    GameManager.startBall = 3;
                }
                playerName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                playerName2.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
                playerName3.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
            }
            else if (allPlayers == Players.FourPlayer && GameManager.pinMode != GameManager.PinMode.Spare)
            {
                if (GameManager.startBall <= 0 || GameManager.startBall <= 1 || GameManager.startBall <= 2 || GameManager.startBall <= 3)
                {
                    GameManager.startBall = 4;
                }
                playerName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                playerName2.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
                playerName3.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
                playerName4.text = gameManager.bowler[GameManager.turnNameIndex4].playerName;
            }
            else if (allPlayers == Players.Computer && GameManager.pinMode != GameManager.PinMode.Spare)
            {
                if (GameManager.startBall <= 0)
                {
                    GameManager.startBall = 1;
                }
                playerName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                playerName2.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName;
            }
        }
    }

    // Update is called once per frame
    void Update ()
	{
        PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
        PlayerPrefs.SetInt("SaveBomb", GameManager.bombBalls);
        PlayerPrefs.SetInt("SaveHyper", GameManager.hyperBalls);
        PlayerPrefs.SetInt("SaveLightning", GameManager.lightningBalls);
        moneyText.text = "$" + GameManager.moneys;
        bombBallText.text = bombShopText.text = GameManager.bombBalls + "x";
        hyperBallText.text = hyperShopText.text = GameManager.hyperBalls + "x";
        lightningBallText.text = lightningShopText.text = GameManager.lightningBalls + "x";
        if (GameManager.bombBalls <= 0)
        {
            bombBallButton.interactable = false;
        }
        else
        {
            bombBallButton.interactable = true;
        }
        if (GameManager.hyperBalls <= 0)
        {
            hyperBallButton.interactable = false;
        }
        else
        {
            hyperBallButton.interactable = true;
        }
        if (GameManager.lightningBalls <= 0)
        {
            lightningBallButton.interactable = false;
        }
        else
        {
            lightningBallButton.interactable = true;
        }
        if (gameManager.bowler.Count == 0)
        {
            bowlerButton[0].interactable = false;
            bowlerButton[1].interactable = false;
            bowlerButton[2].interactable = false;
            bowlerButton[3].interactable = false;
        }
        else if (gameManager.bowler.Count == 1)
        {
            bowlerButton[0].interactable = true;
            bowlerButton[1].interactable = false;
            bowlerButton[2].interactable = false;
            bowlerButton[3].interactable = false;
        }
        else if (gameManager.bowler.Count == 2)
        {
            bowlerButton[0].interactable = true;
            bowlerButton[1].interactable = true;
            bowlerButton[2].interactable = false;
            bowlerButton[3].interactable = false;
        }
        else if (gameManager.bowler.Count == 3)
        {
            bowlerButton[0].interactable = true;
            bowlerButton[1].interactable = true;
            bowlerButton[2].interactable = true;
            bowlerButton[3].interactable = false;
        }
        else if (gameManager.bowler.Count >= 4)
        {
            bowlerButton[0].interactable = true;
            bowlerButton[1].interactable = true;
            bowlerButton[2].interactable = true;
            bowlerButton[3].interactable = true;
        }
        ballDataCustomText.text = customBallLbs.value + "lbs.  speed:" + customBallSpeed.value + "  spin:" + customBallSpin.value;
        replays = GameObject.FindObjectsOfType<ActionReplay>();
        if (spareBalls < 0)
        {
            ballsText.text = "no balls";
        }
        else if (spareBalls == 0)
        {
            ballsText.text = "last balls";
        }
        else if (spareBalls > 0)
        {
            ballsText.text = "balls: " + spareBalls;
        }
        stagesText.text = "stage: " + stage;
        stagesWinText.text = "stages cleared: " + stage;
        winBallRender1.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
        winBallRender2.material = gameManager.chooseBalls[GameManager.turnBalls2].ballMat;
        winBallRender3.material = gameManager.chooseBalls[GameManager.turnBalls3].ballMat;
        winBallRender4.material = gameManager.chooseBalls[GameManager.turnBalls4].ballMat;
        winBallRenderCPU.material = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballMat;
        AlleyRegister();
        if (GameManager.isOpening && opening.time >= 14)
        {
            music.Play();
            GameManager.isOpening = false;
        }
        sounds = GameObject.FindGameObjectsWithTag("Sound");
        music.mute = !GameManager.isMusic;
        foreach (GameObject sound in sounds)
        {
            if (type != GameState.Menu)
            {
                sound.GetComponent<AudioSource>().mute = !GameManager.isSound;
            }
            else if (type == GameState.Menu)
            {
                sound.GetComponent<AudioSource>().mute = true;
            }
        }
        if (gameManager.bowler.Count == 1)
        {
            foreach (Text winName1 in winGameName1)
            {
                winName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            }
        }
        else if (gameManager.bowler.Count == 2)
        {
            foreach (Text winName1 in winGameName1)
            {
                winName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            }
            foreach (Text winName2 in winGameName2)
            {
                winName2.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
            }
        }
        else if (gameManager.bowler.Count == 3)
        {
            foreach (Text winName1 in winGameName1)
            {
                winName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            }
            foreach (Text winName2 in winGameName2)
            {
                winName2.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
            }
            foreach (Text winName3 in winGameName3)
            {
                winName3.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
            }
        }
        else if (gameManager.bowler.Count >= 4)
        {
            foreach (Text winName1 in winGameName1)
            {
                winName1.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            }
            foreach (Text winName2 in winGameName2)
            {
                winName2.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
            }
            foreach (Text winName3 in winGameName3)
            {
                winName3.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
            }
            foreach (Text winName4 in winGameName4)
            {
                winName4.text = gameManager.bowler[GameManager.turnNameIndex4].playerName;
            }
        }
        foreach (Text winNameCPU in winGameNameCPU)
        {
            winNameCPU.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName;
        }
        foreach (Text winScore1 in winGameScore1)
        {
            winScore1.text = "score: " + score1;
        }
        foreach (Text winScore2 in winGameScore2)
        {
            winScore2.text = "score: " + score2;
        }
        foreach (Text winScore3 in winGameScore3)
        {
            winScore3.text = "score: " + score3;
        }
        foreach (Text winScore4 in winGameScore4)
        {
            winScore4.text = "score: " + score4;
        }
        qualityText.text = QualitySettings.names[GameManager.qualityIndex];
        resolutionText.text = GameManager.resolutions[GameManager.resolutionIndex].width + " x " + GameManager.resolutions[GameManager.resolutionIndex].height;
        alleyText.text = gameManager.nameAlleys[PlayerPrefs.GetInt("ChooseAlleys")];
        selectAlleysUI.sprite = gameManager.spriteAlleys[PlayerPrefs.GetInt("ChooseAlleys")];
        if (Input.GetMouseButtonDown(0) && type == GameState.Intro && isIntro)
        {
            StopCoroutine(IntroTime());
            if (isIntro)
            {
                chooseBallUI.SetActive(true);
                powerUpUI.SetActive(true);
                for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().introAnimations.Length; i++)
                {
                    GameObject.FindObjectOfType<PinSetter>().introAnimations[i].SkipAnimation();
                }
                type = GameState.Game;
                camType = CameraType.MoveX;
                ball.ResetCam();
                isIntro = false;
            }
        }
        if (scores1.Count <= 0)
        {
            score1 = 0;
        }
        else if (scores1.Count > 0)
        {
            score1 = scores1[scores1.Count - 1];
        }
        if (scores2.Count <= 0)
        {
            score2 = 0;
        }
        else if (scores2.Count > 0)
        {
            score2 = scores2[scores2.Count - 1];
        }
        if (scores3.Count <= 0)
        {
            score3 = 0;
        }
        else if (scores3.Count > 0)
        {
            score3 = scores3[scores3.Count - 1];
        }
        if (scores4.Count <= 0)
        {
            score4 = 0;
        }
        else if (scores4.Count > 0)
        {
            score4 = scores4[scores4.Count - 1];
        }
        if (turns == 0)
        {
            scoreTextBall2.text = "score:" + score1;
        }
        else if (turns == 1)
        {
            scoreTextBall2.text = "score:" + score2;
        }
        else if (turns == 2)
        {
            scoreTextBall2.text = "score:" + score3;
        }
        else if (turns == 3)
        {
            scoreTextBall2.text = "score:" + score4;
        }
        if (type != GameState.Menu)
        {
            if (playerTurn == 0)
            {
                GameManager.chooseBallIndex = GameManager.turnBalls1;
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            }
            else if (playerTurn == 1)
            {
                GameManager.chooseBallIndex = GameManager.turnBalls2;
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
            }
            else if (playerTurn == 2)
            {
                GameManager.chooseBallIndex = GameManager.turnBalls3;
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
            }
            else if (playerTurn == 3)
            {
                GameManager.chooseBallIndex = GameManager.turnBalls4;
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex4].playerName;
            }
            else if (playerTurn == 4)
            {
                GameManager.chooseBallIndex = gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex;
                playerNameText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName;
            }
            if (GameManager.chooseBallIndex < gameManager.chooseBalls.Length)
            {
                ball.ChargeBall(gameManager.chooseBalls[GameManager.chooseBallIndex].ballMat, gameManager.chooseBalls[GameManager.chooseBallIndex].lbs, gameManager.chooseBalls[GameManager.chooseBallIndex].speed, gameManager.chooseBalls[GameManager.chooseBallIndex].spin);
            }
        }
        for (int i = 0; i < regCount; i++)
        {
            if (playerTurn == 0)
            {
                if (gameManager.bowler.Count != 1)
                {
                    arrowBowler.SetActive(true);
                }
                else
                {
                    arrowBowler.SetActive(false);
                }
                if (gameManager.bowler.Count >= 1)
                {
                    bowlerText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                }
                if (gameManager.chooseBalls[GameManager.turnBalls1].isLock != 1)
                {
                    ballLocked.SetActive(false);
                    ballNeed.SetActive(false);
                    ballUnlock.SetActive(false);
                    if (gameManager.chooseBalls[GameManager.turnBalls1].lockType != ChooseBall.LockType.None && gameManager.chooseBalls[GameManager.turnBalls1].lockType != ChooseBall.LockType.Custom)
                    {
                        ballUnlocked.SetActive(true);
                    }
                    else
                    {
                        ballUnlocked.SetActive(false);
                    }
                    if (gameManager.chooseBalls[GameManager.turnBalls1].lockType != ChooseBall.LockType.Custom)
                    {
                        customBallButton.SetActive(false);
                    }
                    else
                    {
                        customBallButton.SetActive(true);
                    }
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
                    SetAlwaysBowl(true);
                }
                else
                {
                    customBallButton.SetActive(false);
                    ballLocked.SetActive(true);
                    ballNeed.SetActive(true);
                    ballUnlock.SetActive(true);
                    ballUnlocked.SetActive(false);
                    ballRender.material = gameManager.lockBallMat;
                    SetAlwaysBowl(false);
                }
                if (gameManager.chooseBalls[GameManager.turnBalls1].lockType == ChooseBall.LockType.Score)
                {
                    ballUnlock.GetComponent<Text>().text = "Score " + gameManager.chooseBalls[GameManager.turnBalls1].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls1].lockType == ChooseBall.LockType.Spare)
                {
                    ballUnlock.GetComponent<Text>().text = "Spare " + gameManager.chooseBalls[GameManager.turnBalls1].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls1].lockType == ChooseBall.LockType.Beat)
                {
                    ballUnlock.GetComponent<Text>().text = "Beat " + gameManager.chooseBalls[GameManager.turnBalls1].CPUName + " to use";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls1].lockType == ChooseBall.LockType.Earn)
                {
                    ballUnlock.GetComponent<Text>().text = "Earn $" + gameManager.chooseBalls[GameManager.turnBalls1].totalLock * 0.001f + ",000 to unlock";
                }
                if (GameManager.turnBalls1 >= 5 && GameManager.unlockRegister == 1)
                {
                    ballRender.material = gameManager.lockBallMat;
                }
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
                ballNameMenuText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
                ballDataMenuText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
            }
            else if (playerTurn == 1)
            {
                if (gameManager.bowler.Count != 1)
                {
                    arrowBowler.SetActive(true);
                }
                else
                {
                    arrowBowler.SetActive(false);
                }
                if (gameManager.bowler.Count >= 2)
                {
                    bowlerText.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
                }
                if (gameManager.chooseBalls[GameManager.turnBalls2].isLock != 1)
                {
                    ballLocked.SetActive(false);
                    ballNeed.SetActive(false);
                    ballUnlock.SetActive(false);
                    if (gameManager.chooseBalls[GameManager.turnBalls2].lockType != ChooseBall.LockType.None && gameManager.chooseBalls[GameManager.turnBalls2].lockType != ChooseBall.LockType.Custom)
                    {
                        ballUnlocked.SetActive(true);
                    }
                    else
                    {
                        ballUnlocked.SetActive(false);
                    }
                    if (gameManager.chooseBalls[GameManager.turnBalls2].lockType != ChooseBall.LockType.Custom)
                    {
                        customBallButton.SetActive(false);
                    }
                    else
                    {
                        customBallButton.SetActive(true);
                    }
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls2].ballMat;
                    SetAlwaysBowl(true);
                }
                else
                {
                    customBallButton.SetActive(false);
                    ballLocked.SetActive(true);
                    ballNeed.SetActive(true);
                    ballUnlock.SetActive(true);
                    ballUnlocked.SetActive(false);
                    ballRender.material = gameManager.lockBallMat;
                    SetAlwaysBowl(false);
                }
                if (gameManager.chooseBalls[GameManager.turnBalls2].lockType == ChooseBall.LockType.Score)
                {
                    ballUnlock.GetComponent<Text>().text = "Score " + gameManager.chooseBalls[GameManager.turnBalls2].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls2].lockType == ChooseBall.LockType.Spare)
                {
                    ballUnlock.GetComponent<Text>().text = "Spare " + gameManager.chooseBalls[GameManager.turnBalls2].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls2].lockType == ChooseBall.LockType.Beat)
                {
                    ballUnlock.GetComponent<Text>().text = "Beat " + gameManager.chooseBalls[GameManager.turnBalls2].CPUName + " to use";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls2].lockType == ChooseBall.LockType.Earn)
                {
                    ballUnlock.GetComponent<Text>().text = "Earn $" + gameManager.chooseBalls[GameManager.turnBalls2].totalLock * 0.001f + ",000 to unlock";
                }
                if (GameManager.turnBalls2 >= 5 && GameManager.unlockRegister == 1)
                {
                    ballRender.material = gameManager.lockBallMat;
                }
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls2].ballName;
                ballNameMenuText.text = gameManager.chooseBalls[GameManager.turnBalls2].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls2].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls2].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls2].spin;
                ballDataMenuText.text = gameManager.chooseBalls[GameManager.turnBalls2].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls2].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls2].spin;
            }
            else if (playerTurn == 2)
            {
                if (gameManager.bowler.Count != 1)
                {
                    arrowBowler.SetActive(true);
                }
                else
                {
                    arrowBowler.SetActive(false);
                }
                if (gameManager.bowler.Count >= 3)
                {
                    bowlerText.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
                }
                if (gameManager.chooseBalls[GameManager.turnBalls3].isLock != 1)
                {
                    ballLocked.SetActive(false);
                    ballNeed.SetActive(false);
                    ballUnlock.SetActive(false);
                    if (gameManager.chooseBalls[GameManager.turnBalls3].lockType != ChooseBall.LockType.None && gameManager.chooseBalls[GameManager.turnBalls3].lockType != ChooseBall.LockType.Custom)
                    {
                        ballUnlocked.SetActive(true);
                    }
                    else
                    {
                        ballUnlocked.SetActive(false);
                    }
                    if (gameManager.chooseBalls[GameManager.turnBalls3].lockType != ChooseBall.LockType.Custom)
                    {
                        customBallButton.SetActive(false);
                    }
                    else
                    {
                        customBallButton.SetActive(true);
                    }
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls3].ballMat;
                    SetAlwaysBowl(true);
                }
                else
                {
                    customBallButton.SetActive(false);
                    ballLocked.SetActive(true);
                    ballNeed.SetActive(true);
                    ballUnlock.SetActive(true);
                    ballUnlocked.SetActive(false);
                    ballRender.material = gameManager.lockBallMat;
                    SetAlwaysBowl(false);
                }
                if (gameManager.chooseBalls[GameManager.turnBalls3].lockType == ChooseBall.LockType.Score)
                {
                    ballUnlock.GetComponent<Text>().text = "Score " + gameManager.chooseBalls[GameManager.turnBalls3].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls3].lockType == ChooseBall.LockType.Spare)
                {
                    ballUnlock.GetComponent<Text>().text = "Spare " + gameManager.chooseBalls[GameManager.turnBalls3].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls3].lockType == ChooseBall.LockType.Beat)
                {
                    ballUnlock.GetComponent<Text>().text = "Beat " + gameManager.chooseBalls[GameManager.turnBalls3].CPUName + " to use";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls3].lockType == ChooseBall.LockType.Earn)
                {
                    ballUnlock.GetComponent<Text>().text = "Earn $" + gameManager.chooseBalls[GameManager.turnBalls3].totalLock * 0.001f + ",000 to unlock";
                }
                if (GameManager.turnBalls3 >= 5 && GameManager.unlockRegister == 1)
                {
                    ballRender.material = gameManager.lockBallMat;
                }
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls3].ballName;
                ballNameMenuText.text = gameManager.chooseBalls[GameManager.turnBalls3].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls3].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls3].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls3].spin;
                ballDataMenuText.text = gameManager.chooseBalls[GameManager.turnBalls3].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls3].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls3].spin;
            }
            else if (playerTurn == 3)
            {
                if (gameManager.bowler.Count != 1)
                {
                    arrowBowler.SetActive(true);
                }
                else
                {
                    arrowBowler.SetActive(false);
                }
                if (gameManager.bowler.Count >= 4)
                {
                    bowlerText.text = gameManager.bowler[GameManager.turnNameIndex4].playerName;
                }
                if (gameManager.chooseBalls[GameManager.turnBalls4].isLock != 1)
                {
                    ballLocked.SetActive(false);
                    ballNeed.SetActive(false);
                    ballUnlock.SetActive(false);
                    if (gameManager.chooseBalls[GameManager.turnBalls4].lockType != ChooseBall.LockType.None && gameManager.chooseBalls[GameManager.turnBalls4].lockType != ChooseBall.LockType.Custom)
                    {
                        ballUnlocked.SetActive(true);
                    }
                    else
                    {
                        ballUnlocked.SetActive(false);
                    }
                    if (gameManager.chooseBalls[GameManager.turnBalls4].lockType != ChooseBall.LockType.Custom)
                    {
                        customBallButton.SetActive(false);
                    }
                    else
                    {
                        customBallButton.SetActive(true);
                    }
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls4].ballMat;
                    SetAlwaysBowl(true);
                }
                else
                {
                    customBallButton.SetActive(false);
                    ballLocked.SetActive(true);
                    ballNeed.SetActive(true);
                    ballUnlock.SetActive(true);
                    ballUnlocked.SetActive(false);
                    ballRender.material = gameManager.lockBallMat;
                    SetAlwaysBowl(false);
                }
                if (gameManager.chooseBalls[GameManager.turnBalls4].lockType == ChooseBall.LockType.Score)
                {
                    ballUnlock.GetComponent<Text>().text = "Score " + gameManager.chooseBalls[GameManager.turnBalls4].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls4].lockType == ChooseBall.LockType.Spare)
                {
                    ballUnlock.GetComponent<Text>().text = "Spare " + gameManager.chooseBalls[GameManager.turnBalls4].totalLock + " to unlock";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls4].lockType == ChooseBall.LockType.Beat)
                {
                    ballUnlock.GetComponent<Text>().text = "Beat " + gameManager.chooseBalls[GameManager.turnBalls4].CPUName + " to use";
                }
                else if (gameManager.chooseBalls[GameManager.turnBalls4].lockType == ChooseBall.LockType.Earn)
                {
                    ballUnlock.GetComponent<Text>().text = "Earn $" + gameManager.chooseBalls[GameManager.turnBalls4].totalLock * 0.001f + ",000 to unlock";
                }
                if (GameManager.turnBalls4 >= 5 && GameManager.unlockRegister == 1)
                {
                    ballRender.material = gameManager.lockBallMat;
                }
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls4].ballName;
                ballNameMenuText.text = gameManager.chooseBalls[GameManager.turnBalls4].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls4].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls4].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls4].spin;
                ballDataMenuText.text = gameManager.chooseBalls[GameManager.turnBalls4].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls4].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls4].spin;
            }
            else if (playerTurn == 4)
            {
                arrowBowler.SetActive(false);
                bowlerText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName;
                ballLocked.SetActive(false);
                ballNeed.SetActive(false);
                ballUnlock.SetActive(false);
                ballUnlocked.SetActive(false);
                if (GameManager.unlockRegister == 0 || GameManager.turnBallsCPU < 2 && GameManager.unlockRegister == 1)
                {
                    ballRender.material = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballMat;
                    if (gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lockType != ChooseBall.LockType.Custom)
                    {
                        customBallButton.SetActive(false);
                    }
                    else
                    {
                        customBallButton.SetActive(true);
                    }
                }
                if (GameManager.turnBallsCPU >= 2 && GameManager.unlockRegister == 1)
                {
                    customBallButton.SetActive(false);
                    ballRender.material = gameManager.lockBallMat;
                }
                SetAlwaysBowl(true);
                ballNameText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName;
                ballNameMenuText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName;
                ballDataText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs + "lbs.  speed:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed + "  spin:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin;
                ballDataMenuText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs + "lbs.  speed:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed + "  spin:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin;
            }
            if (GameManager.unlockRegister == 0 || GameManager.turnBalls1 < 5 && GameManager.unlockRegister == 1 && playerTurn == 0 || GameManager.turnBalls2 < 5 && GameManager.unlockRegister == 1 && playerTurn == 1 || GameManager.turnBalls3 < 5 && GameManager.unlockRegister == 1 && playerTurn == 2 || GameManager.turnBalls4 < 5 && GameManager.unlockRegister == 1 && playerTurn == 3 || GameManager.turnBallsCPU < 2 && GameManager.unlockRegister == 1 && playerTurn == 4)
            {
                ballRegistered.SetActive(false);
                ballsRegisterButton[i].SetActive(false);
                SetAlwaysBowlReg(true);
            }
            else if (GameManager.turnBalls1 >= 5 && GameManager.unlockRegister == 1 && playerTurn == 0 || GameManager.turnBalls2 >= 5 && GameManager.unlockRegister == 1 && playerTurn == 1 || GameManager.turnBalls3 >= 5 && GameManager.unlockRegister == 1 && playerTurn == 2 || GameManager.turnBalls4 >= 5 && GameManager.unlockRegister == 1 && playerTurn == 3 || GameManager.turnBallsCPU >= 2 && GameManager.unlockRegister == 1 && playerTurn == 4)
            {
                customBallButton.SetActive(false);
                ballLocked.SetActive(false);
                ballNeed.SetActive(false);
                ballUnlock.SetActive(false);
                ballUnlocked.SetActive(false);
                ballRegistered.SetActive(true);
                ballsRegisterButton[i].SetActive(true);
                SetAlwaysBowlReg(false);
            }
        }
        replayIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().replays.Length);
        reactIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().reacts.Length);
        pin7.isHitOne = true;
        pin10.isHitOne = true;
        if (pin7.IsStanding() == false)
        {
            pin8.isHitOne = true;
        }
        if (pin10.IsStanding() == false)
        {
            pin9.isHitOne = true;
        }
        if (pin7.IsStanding() == false && pin8.IsStanding() == false && pin9.IsStanding() == false && pin10.IsStanding() == false)
        {
            pin4.isHitOne = true;
        }
        if (pin7.IsStanding() == false && pin8.IsStanding() == false && pin9.IsStanding() == false && pin10.IsStanding() == false)
        {
            pin5.isHitOne = true;
        }
        if (pin7.IsStanding() == false && pin8.IsStanding() == false && pin9.IsStanding() == false && pin10.IsStanding() == false)
        {
            pin6.isHitOne = true;
        }
        if (pin4.IsStanding() == false && pin5.IsStanding() == false && pin7.IsStanding() == false && pin8.IsStanding() == false && pin10.IsStanding() == false)
        {
            pin2.isHitOne = true;
        }
        if (pin5.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin9.IsStanding() == false && pin10.IsStanding() == false)
        {
            pin3.isHitOne = true;
        }
        if (pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin5.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin8.IsStanding() == false && pin9.IsStanding() == false && pin10.IsStanding() == false)
        {
            pin1.isHitOne = true;
        }
        if (rainIndex == 8)
        {
            rainIndex = 0;
        }
        if (!commentatorAudio.isPlaying && commentatorIndex == 1)
        {
            commentatorAudio.clip = Resources.Load<AudioClip>("Sound/silence_sm");
            commentatorAudio.Play();
            commentatorIndex = 2;
        }
        if (!commentatorAudio.isPlaying && commentatorIndex == 2)
        {
            commentator.SecondVoice(commentatorAudio);
            commentatorAudio.Play();
            commentatorIndex = 3;
        }
        if (!commentatorAudio.isPlaying && commentatorIndex == 3)
        {
            if (crowdType == Crowds.CheerBig)
            {
                CheerBig();
            }
            if (crowdType == Crowds.CheerMed)
            {
                CheerMed();
            }
            if (crowdType == Crowds.CrowdOk)
            {
                CrowdOk();
            }
            if (crowdType == Crowds.CrowdHohum)
            {
                CrowdHohum();
            }
            if (crowdType == Crowds.CrowdCrap)
            {
                CrowdCrap();
            }
            if (crowdType == Crowds.Laugh)
            {
                Laugh();
            }
            if (crowdType == Crowds.Oooh)
            {
                Oooh();
            }
            if (crowdType == Crowds.Firework)
            {
                winCrowd.Play();
                fireworksMulti.Play();
            }
            commentatorIndex = 0;
        }
        pinCounter.UpdateStandingCountAndSettle();
        if (replayTime < 10 && isReplayRecord)
        {
            replayTime += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isReplayRecord)
        {
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].Add();
            }
        }
        float nextIndex;
        if (isCurrentReplay)
        {
            nextIndex = currentReplayIndex + 0.5f;

            for (int i = 0; i < replays.Length; i++)
            {
                if (nextIndex >= 0 && nextIndex < replays[i].actionReplayRecords.Count)
                {
                    replays[i].SetTransform(nextIndex);
                }
            }
        }
        else
        {
            nextIndex = 0;
        }
        if (!isReplayRecord && !isCurrentReplay)
        {
            currentReplayIndex = 0;
            replayTime = 0;
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].Clear();
            }
        }
    }

    public IEnumerator IntroTime()
    {
        yield return new WaitForSeconds(GameObject.FindObjectOfType<PinSetter>().introAnimations[introAnimIndex].time);
        if (isIntro)
        {
            chooseBallUI.SetActive(true);
            powerUpUI.SetActive(true);
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().introAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().introAnimations[i].StopAnimation();
            }
            type = GameState.Game;
            camType = CameraType.MoveX;
            isIntro = false;
        }
    }

    public IEnumerator PinTimeA(float time)
	{
        if (b != null)
        {
            StopCoroutine(b);
        }
        if (type == GameState.Replay)
        {
            type = GameState.Game;
        }
        yield return new WaitForSeconds(time);
        b = StartCoroutine(PinTimeB());

        yield return null;
    }

    public IEnumerator PinTimeB()
    {
        ball.rollAudio.Stop();
        ball.gutterAudio.Stop();
        ball.pinAudio.Stop();
        ball.electricAudio.Stop();
        if (throwBall == 1)
        {
            if (pin1.IsStanding() == false || pin2.IsStanding() == false || pin3.IsStanding() == false || pin4.IsStanding() == false || pin5.IsStanding() == false || pin6.IsStanding() == false || pin7.IsStanding() == false || pin8.IsStanding() == false || pin9.IsStanding() == false || pin10.IsStanding() == false)
            {
                isSplit = false;
                is710 = false;
            }
        }
        for (int i = 0; i < pinSplits.Length; i++)
        {
            if (pin1.IsStanding() == pinSplits[i].isPin1 && pin2.IsStanding() == pinSplits[i].isPin2 && pin3.IsStanding() == pinSplits[i].isPin3 && pin4.IsStanding() == pinSplits[i].isPin4 && pin5.IsStanding() == pinSplits[i].isPin5 && pin6.IsStanding() == pinSplits[i].isPin6 && pin7.IsStanding() == pinSplits[i].isPin7 && pin8.IsStanding() == pinSplits[i].isPin8 && pin9.IsStanding() == pinSplits[i].isPin9 && pin10.IsStanding() == pinSplits[i].isPin10 && throwBall == 1)
            {
                isSplit = true;
                is710 = false;
            }
        }
        if (pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin5.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin8.IsStanding() == false && pin9.IsStanding() == false && pin10.IsStanding() == true && throwBall == 1)
        {
            isSplit = true;
            is710 = true;
        }
        if (type == GameState.Game)
        {
            type = GameState.Replay;
        }
        GameObject.FindObjectOfType<PinSetter>().DownPins();
        pinCounter.PinsHaveSettled();
        if (maxBalls == 2)
        {
            if (PinCounter.pinCount == 0 && throwBall == 1)
            {
                if (scoreDisplay[turns].strikes == ScoreDisplay.Strikes.Strike)
                {
                    animations = AnimationScenes.Strike;
                    if (type == GameState.Replay)
                    {
                        VoiceStrike();
                    }
                }
                if (scoreDisplay[turns].strikes == ScoreDisplay.Strikes.Double)
                {
                    animations = AnimationScenes.Double;
                    if (type == GameState.Replay)
                    {
                        VoiceDouble();
                    }
                    RainCountdown();
                }
                if (scoreDisplay[turns].strikes == ScoreDisplay.Strikes.Turkey)
                {
                    animations = AnimationScenes.Turkey;
                    if (type == GameState.Replay)
                    {
                        VoiceTurkey();
                    }
                    RainCountdown();
                }
                if (pinCounts == 10)
                {
                    if (playerTurn == 0)
                    {
                        strikes1++;
                        addCash += 10 * allStrikes1;
                        allStrikes1++;
                    }
                    else if (playerTurn == 1 || playerTurn == 4)
                    {
                        strikes2++;
                        if (!isComputer)
                        {
                            addCash += 10 * allStrikes2;
                            allStrikes2++;
                        }
                    }
                    else if (playerTurn == 2)
                    {
                        strikes3++;
                        addCash += 10 * allStrikes3;
                        allStrikes3++;
                    }
                    else if (playerTurn == 3)
                    {
                        strikes4++;
                        addCash += 10 * allStrikes4;
                        allStrikes4++;
                    }
                    scoreDisplay[turns].AllStrike();
                }
            }
        }
        if (PinCounter.pinCount == 0 && throwBall == 2)
        {
            animations = AnimationScenes.Spare;
            if (isSplit)
            {
                if (type == GameState.Replay)
                {
                    VoiceSplitPick();
                }
            }
            else
            {
                if (type == GameState.Replay)
                {
                    VoiceSpare();
                }
            }
            if (playerTurn == 0)
            {
                spares1++;
                addCash += 5 * allStrikes1;
            }
            else if (playerTurn == 1 || playerTurn == 4)
            {
                spares2++;
                if (!isComputer)
                {
                    addCash += 5 * allStrikes2;
                }
            }
            else if (playerTurn == 2)
            {
                spares3++;
                addCash += 5 * allStrikes3;
            }
            else if (playerTurn == 3)
            {
                spares4++;
                addCash += 5 * allStrikes4;
            }
        }
        if (throwBall == 2)
        {
            if (playerTurn == 0)
            {
                allStrikes1 = 1;
            }
            else if (playerTurn == 1 || playerTurn == 4)
            {
                allStrikes2 = 1;
            }
            else if (playerTurn == 2)
            {
                allStrikes3 = 1;
            }
            else if (playerTurn == 3)
            {
                allStrikes4 = 1;
            }
        }
        if (gutterAnimation == 0 && isReplay)
        {
            if (type == GameState.Replay)
            {
                replayText.SetActive(true);
            }
            isReplay = true;
            isReplayRecord = false;
            isCurrentReplay = true;
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].RigidBodyFreeze(true);
                replays[i].RigidBodyCollider(false);
                replays[i].SetTransform(0);
            }
            cameraFollow.Replay(replayIndex);
            yield return new WaitForSeconds(replayTime * 2.0f);
            replayText.SetActive(false);
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].RigidBodyFreeze(false);
                replays[i].RigidBodyCollider(true);
                replays[i].SetTransform(replays[i].actionReplayRecords.Count - 1);
                replays[i].Clear();
                replays[i].SetVelocity();
            }
            isPin = false;
            isReplayRecord = true;
        }
        replayText.SetActive(false);
        isCurrentReplay = false;
        currentReplayIndex = 0;
        replayTime = 0;
        if (gutterAnimation == 0 && animations == AnimationScenes.Off)
        {
            cameraFollow.React(reactIndex);
        }
        else if (animations == AnimationScenes.Strike)
        {
            strikeAnimIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length);
            isAnim = true;
            camType = CameraType.Anim;
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
            }
            GameObject.FindObjectOfType<PinSetter>().strikeAnimations[strikeAnimIndex].PlayAnimation();
        }
        else if (animations == AnimationScenes.Spare)
        {
            spareAnimIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length);
            isAnim = true;
            camType = CameraType.Anim;
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
            }
            GameObject.FindObjectOfType<PinSetter>().spareAnimations[spareAnimIndex].PlayAnimation();
        }
        else if (animations == AnimationScenes.Double)
        {
            doubleAnimIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length);
            isAnim = true;
            camType = CameraType.Anim;
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
            }
            GameObject.FindObjectOfType<PinSetter>().doubleAnimations[doubleAnimIndex].PlayAnimation();
        }
        else if (animations == AnimationScenes.Turkey)
        {
            turkeyAnimIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length);
            isAnim = true;
            camType = CameraType.Anim;
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
            }
            GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[turkeyAnimIndex].PlayAnimation();
        }
        else if (gutterAnimation == 1)
        {
            gbAnimIndex = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length);
            isReplay = true;
            isAnim = true;
            camType = CameraType.Anim;
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
            }
            GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[gbAnimIndex].PlayAnimation();
        }
        else if (gutterAnimation == 2)
        {
            gbAnimIndex2X = Random.Range(0, GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length);
            isReplay = true;
            isAnim = true;
            camType = CameraType.Anim;
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
            }
            for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
            {
                GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
            }
            GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[gbAnimIndex2X].PlayAnimation();
        }
        powerUps = BallPowerUps.Off;
        GameObject.FindObjectOfType<PinSetter>().ScooperPins();
        if (throwBall < maxBalls)
        {
            GameObject.FindObjectOfType<PinSetter>().DownPins();
            foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
            {
                pin.PinLight();
            }
        }
        if (gutterAnimation == 1)
        {
            yield return new WaitForSeconds(GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[gbAnimIndex].time);
            isAnim = false;
            if (!isReplay || gutterAnimation != 0)
            {
                if (GameObject.FindObjectOfType<PinSetter>().returnPoint != null)
                {
                    cameraFollow.transform.position = new Vector3(GameObject.FindObjectOfType<PinSetter>().returnPoint.position.x + 100, GameObject.FindObjectOfType<PinSetter>().returnPoint.position.y + 100, GameObject.FindObjectOfType<PinSetter>().returnPoint.position.z + 400);
                    cameraFollow.transform.rotation = Quaternion.LookRotation(ball.transform.position - cameraFollow.transform.position);
                    camType = CameraType.ReturnBall;
                }
                ball.BallReturn();
            }
            if (GameObject.FindObjectOfType<PinSetter>().returnPoint != null)
            {
                yield return new WaitForSeconds(15);
            }
        } 
        else if(gutterAnimation == 2)
        {
            yield return new WaitForSeconds(GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[gbAnimIndex2X].time);
            isAnim = false;
            if (!isReplay || gutterAnimation != 0)
            {
                if (GameObject.FindObjectOfType<PinSetter>().returnPoint != null)
                {
                    cameraFollow.transform.position = new Vector3(GameObject.FindObjectOfType<PinSetter>().returnPoint.position.x + 100, GameObject.FindObjectOfType<PinSetter>().returnPoint.position.y + 100, GameObject.FindObjectOfType<PinSetter>().returnPoint.position.z + 400);
                    cameraFollow.transform.rotation = Quaternion.LookRotation(ball.transform.position - cameraFollow.transform.position);
                    camType = CameraType.ReturnBall;
                }
                ball.BallReturn();
            }
            if (GameObject.FindObjectOfType<PinSetter>().returnPoint != null)
            {
                yield return new WaitForSeconds(15);
            }
        }
        else if (gutterAnimation == 0)
        {
            yield return new WaitForSeconds(3);
            if (!isReplay || gutterAnimation != 0)
            {
                ball.BallReturn();
            }
            if (animations == AnimationScenes.Off)
            {
                yield return new WaitForSeconds(12);
            }
            else if (animations == AnimationScenes.Strike)
            {
                yield return new WaitForSeconds(FindObjectOfType<PinSetter>().strikeAnimations[strikeAnimIndex].time);
            }
            else if (animations == AnimationScenes.Spare)
            {
                yield return new WaitForSeconds(FindObjectOfType<PinSetter>().spareAnimations[spareAnimIndex].time);
            }
            else if (animations == AnimationScenes.Double)
            {
                yield return new WaitForSeconds(FindObjectOfType<PinSetter>().doubleAnimations[doubleAnimIndex].time);
            }
            else if (animations == AnimationScenes.Turkey)
            {
                yield return new WaitForSeconds(FindObjectOfType<PinSetter>().turkeyAnimations[turkeyAnimIndex].time);
            }
        }
        SkipReplay();
        isReplayRecord = false;
    }

    public void AlleyTheme(string theme)
    {
        music.clip = Resources.Load<AudioClip>("Music/" + theme);
    }

    public void Bowl(int pinFall)
    {
        if (maxBalls == 1 && type == GameState.Replay)
        {
            if (PinCounter.pinCount != 0)
            {
                CrowdCrap();
                spareBalls--;
            }
            else
            {
                CheerMed();
                stage++;
            }
            if (spareBalls < 0)
            {
                wins = 0;
                isEndGame = true;
            }
        }
        else if (maxBalls == 2 && type == GameState.Replay)
        {
            if (allPlayers == Players.OnePlayer)
            {
                try
                {
                    rolls1.Add(pinFall);
                    GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls1));
                }
                catch
                {
                    isEndGame = true;
                    UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                }

                try
                {
                    scoreDisplay[0].FillRolls(rolls1);
                    scoreDisplay[0].FillFrames(ScoreMaster.ScoreCumulative(rolls1));
                    scores1 = ScoreMaster.ScoreCumulative(rolls1);

                }
                catch
                {
                    UnityEngine.Debug.LogWarning("FillRollCard failed");
                }
            }
            else if (allPlayers == Players.TwoPlayer)
            {
                if (nextTurn == 0)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames1 != 10)
                            {
                                nextTurn = 1;
                                frames1++;
                            }
                            else
                            {
                                nextTurn = 0;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames1 != 10)
                        {
                            nextTurn = 1;
                            ballTurn = 0;
                            frames1++;
                        }
                        else
                        {
                            nextTurn = 0;
                        }
                    }
                    try
                    {
                        rolls1.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls1));
                    }
                    catch
                    {
                        nextTurn = 1;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[0].FillRolls(rolls1);
                        scoreDisplay[0].FillFrames(ScoreMaster.ScoreCumulative(rolls1));
                        scores1 = ScoreMaster.ScoreCumulative(rolls1);

                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames2 != 10)
                            {
                                nextTurn = 0;
                                frames2++;
                            }
                            else
                            {
                                nextTurn = 1;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames2 != 10)
                        {
                            nextTurn = 0;
                            ballTurn = 0;
                            frames2++;
                        }
                        else
                        {
                            nextTurn = 1;
                        }
                    }
                    try
                    {
                        rolls2.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls2));
                    }
                    catch
                    {
                        isEndGame = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[1].FillRolls(rolls2);
                        scoreDisplay[1].FillFrames(ScoreMaster.ScoreCumulative(rolls2));
                        scores2 = ScoreMaster.ScoreCumulative(rolls2);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
            }
            else if (allPlayers == Players.ThreePlayer)
            {
                if (nextTurn == 0)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames1 != 10)
                            {
                                nextTurn = 1;
                                frames1++;
                            }
                            else
                            {
                                nextTurn = 0;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames1 != 10)
                        {
                            nextTurn = 1;
                            ballTurn = 0;
                            frames1++;
                        }
                        else
                        {
                            nextTurn = 0;
                        }
                    }
                    try
                    {
                        rolls1.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls1));
                    }
                    catch
                    {
                        nextTurn = 1;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[0].FillRolls(rolls1);
                        scoreDisplay[0].FillFrames(ScoreMaster.ScoreCumulative(rolls1));
                        scores1 = ScoreMaster.ScoreCumulative(rolls1);

                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else if (nextTurn == 1)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames2 != 10)
                            {
                                nextTurn = 2;
                                frames2++;
                            }
                            else
                            {
                                nextTurn = 1;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames2 != 10)
                        {
                            nextTurn = 2;
                            ballTurn = 0;
                            frames2++;
                        }
                        else
                        {
                            nextTurn = 1;
                        }
                    }
                    try
                    {
                        rolls2.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls2));
                    }
                    catch
                    {
                        nextTurn = 2;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[1].FillRolls(rolls2);
                        scoreDisplay[1].FillFrames(ScoreMaster.ScoreCumulative(rolls2));
                        scores2 = ScoreMaster.ScoreCumulative(rolls2);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames3 != 10)
                            {
                                nextTurn = 0;
                                frames3++;
                            }
                            else
                            {
                                nextTurn = 2;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames3 != 10)
                        {
                            nextTurn = 0;
                            ballTurn = 0;
                            frames3++;
                        }
                        else
                        {
                            nextTurn = 2;
                        }
                    }
                    try
                    {
                        rolls3.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls3));
                    }
                    catch
                    {
                        isEndGame = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[2].FillRolls(rolls3);
                        scoreDisplay[2].FillFrames(ScoreMaster.ScoreCumulative(rolls3));
                        scores3 = ScoreMaster.ScoreCumulative(rolls3);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
            }
            else if (allPlayers == Players.FourPlayer)
            {
                if (nextTurn == 0)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames1 != 10)
                            {
                                nextTurn = 1;
                                frames1++;
                            }
                            else
                            {
                                nextTurn = 0;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames1 != 10)
                        {
                            nextTurn = 1;
                            ballTurn = 0;
                            frames1++;
                        }
                        else
                        {
                            nextTurn = 0;
                        }
                    }
                    try
                    {
                        rolls1.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls1));
                    }
                    catch
                    {
                        nextTurn = 1;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[0].FillRolls(rolls1);
                        scoreDisplay[0].FillFrames(ScoreMaster.ScoreCumulative(rolls1));
                        scores1 = ScoreMaster.ScoreCumulative(rolls1);

                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else if (nextTurn == 1)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames2 != 10)
                            {
                                nextTurn = 2;
                                frames2++;
                            }
                            else
                            {
                                nextTurn = 1;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames2 != 10)
                        {
                            nextTurn = 2;
                            ballTurn = 0;
                            frames2++;
                        }
                        else
                        {
                            nextTurn = 1;
                        }
                    }
                    try
                    {
                        rolls2.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls2));
                    }
                    catch
                    {
                        nextTurn = 2;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[1].FillRolls(rolls2);
                        scoreDisplay[1].FillFrames(ScoreMaster.ScoreCumulative(rolls2));
                        scores2 = ScoreMaster.ScoreCumulative(rolls2);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else if (nextTurn == 2)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames3 != 10)
                            {
                                nextTurn = 3;
                                frames3++;
                            }
                            else
                            {
                                nextTurn = 2;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames3 != 10)
                        {
                            nextTurn = 3;
                            ballTurn = 0;
                            frames3++;
                        }
                        else
                        {
                            nextTurn = 2;
                        }
                    }
                    try
                    {
                        rolls3.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls3));
                    }
                    catch
                    {
                        nextTurn = 3;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[2].FillRolls(rolls3);
                        scoreDisplay[2].FillFrames(ScoreMaster.ScoreCumulative(rolls3));
                        scores3 = ScoreMaster.ScoreCumulative(rolls3);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames4 != 10)
                            {
                                nextTurn = 0;
                                frames4++;
                            }
                            else
                            {
                                nextTurn = 3;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames4 != 10)
                        {
                            nextTurn = 0;
                            ballTurn = 0;
                            frames4++;
                        }
                        else
                        {
                            nextTurn = 3;
                        }
                    }
                    try
                    {
                        rolls4.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls4));
                    }
                    catch
                    {
                        isEndGame = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[3].FillRolls(rolls4);
                        scoreDisplay[3].FillFrames(ScoreMaster.ScoreCumulative(rolls4));
                        scores4 = ScoreMaster.ScoreCumulative(rolls4);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
            }
            else if (allPlayers == Players.Computer)
            {
                if (nextTurn == 0)
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames1 != 10)
                            {
                                nextTurn = 4;
                                frames1++;
                            }
                            else
                            {
                                nextTurn = 0;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames1 != 10)
                        {
                            nextTurn = 4;
                            ballTurn = 0;
                            frames1++;
                        }
                        else
                        {
                            nextTurn = 0;
                        }
                    }
                    try
                    {
                        rolls1.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls1));
                    }
                    catch
                    {
                        nextTurn = 4;
                        ballTurn = 0;
                        isResetPins = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[0].FillRolls(rolls1);
                        scoreDisplay[0].FillFrames(ScoreMaster.ScoreCumulative(rolls1));
                        scores1 = ScoreMaster.ScoreCumulative(rolls1);

                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
                else
                {
                    if (ballTurn == 0)
                    {
                        if (pinFall == 10)
                        {
                            if (frames2 != 10)
                            {
                                nextTurn = 0;
                                frames2++;
                            }
                            else
                            {
                                nextTurn = 4;
                            }
                        }
                        else
                        {
                            ballTurn = 1;
                        }
                    }
                    else
                    {
                        if (frames2 != 10)
                        {
                            nextTurn = 0;
                            ballTurn = 0;
                            frames2++;
                        }
                        else
                        {
                            nextTurn = 4;
                        }
                    }
                    try
                    {
                        rolls2.Add(pinFall);
                        GameObject.FindObjectOfType<PinSetter>().PerformAction(ActionMasterOld.NextAction(rolls2));
                    }
                    catch
                    {
                        isEndGame = true;
                        UnityEngine.Debug.LogWarning("Something went wrong in Bowl()");
                    }

                    try
                    {
                        scoreDisplay[1].FillRolls(rolls2);
                        scoreDisplay[1].FillFrames(ScoreMaster.ScoreCumulative(rolls2));
                        scores2 = ScoreMaster.ScoreCumulative(rolls2);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("FillRollCard failed");
                    }
                }
            }
        }
        if (pinFall == 0 && !isComputer)
        {
            hintCount++;
            if (hintCount == 2 || hintCount == 3 || hintCount == 5)
            {
                gutterHintUI.SetActive(true);
                foreach (GameObject scoreCardUI in falseScoreCardsUI)
                {
                    scoreCardUI.SetActive(false);
                }
            }
            else if (hintCount == 6)
            {
                hintCount = 4;
            }
        }
        else if (pinFall != 0)
        {
            hintCount = 0;
        }
        pinCounts = pinFall;
        if (throwBall == 1)
        {
            if (isSplit)
            {
                if (is710 && type == GameState.Replay)
                {
                    VoiceSplit710();
                }
                else if (!is710 && type == GameState.Replay)
                {
                    VoiceSplit();
                }
            }
            else
            {
                if (pinCounts == 5 || pinCounts == 6)
                {
                    CrowdCrap();
                }
                if (pinCounts == 7 || pinCounts == 8 || pinCounts == 9)
                {
                    CrowdOk();
                }
                if (pinCounts == 0 && PinCounter.pinCount > 0 && type == GameState.Replay)
                {
                    if (playerTurn == 0)
                    {
                        gutters1++;
                    }
                    else if (playerTurn == 1 || playerTurn == 4)
                    {
                        gutters2++;
                    }
                    else if (playerTurn == 2)
                    {
                        gutters3++;
                    }
                    else if (playerTurn == 3)
                    {
                        gutters4++;
                    }
                    if (gutterAnimation == 0)
                    {
                        VoiceMiss();
                    }
                }
                if (pinCounts == 1 && PinCounter.pinCount > 0 && type == GameState.Replay)
                {
                    VoiceOne();
                }
                if (pinCounts == 2 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 3 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 4 && PinCounter.pinCount > 0 && type == GameState.Replay)
                {
                    VoiceFew();
                }
                if (pinCounts == 5 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 6 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 7 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 8 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 9 && PinCounter.pinCount > 0 && type == GameState.Replay)
                {
                    VoiceMost();
                    crowdType = Crowds.NoCrowd;
                }
            }
        }
        if (throwBall >= 2)
        {
            if (pinCounts == 0 && PinCounter.pinCount > 0 && type == GameState.Replay)
            {
                if (playerTurn == 0)
                {
                    gutters1++;
                }
                else if (playerTurn == 1 || playerTurn == 4)
                {
                    gutters2++;
                }
                else if (playerTurn == 2)
                {
                    gutters3++;
                }
                else if (playerTurn == 3)
                {
                    gutters4++;
                }
                if (gutterAnimation == 0)
                {
                    VoiceMiss();
                }
            }
            if (pinCounts == 1 && PinCounter.pinCount > 0 && type == GameState.Replay)
            {
                VoiceOne();
            }
            if (pinCounts == 2 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 3 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 4 && PinCounter.pinCount > 0 && type == GameState.Replay)
            {
                VoiceFew();
            }
            if (pinCounts == 5 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 6 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 7 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 8 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 9 && PinCounter.pinCount > 0 && type == GameState.Replay || pinCounts == 10 && PinCounter.pinCount > 0 && type == GameState.Replay)
            {
                VoiceMost();
                if (pinCounts == 5 || pinCounts == 6)
                {
                    if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
                    {
                        crowdType = Crowds.CrowdCrap;
                    }
                }
                if (pinCounts == 7 || pinCounts == 8 || pinCounts == 9 || pinCounts == 10)
                {
                    if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
                    {
                        crowdType = Crowds.CrowdOk;
                    }
                }
            }
        }
        if (PinCounter.pinCount > 0)
        {
            scoreDisplay[turns].ResetStrike();
        }
    }

    public void VoiceStrike()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Strike(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.CheerMed;
        }
    }

    public void VoiceSpare()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Spare(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.CrowdOk;
        }
    }

    public void VoiceGutterball()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Gutterball(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Laugh;
        }
    }

    public void VoiceDouble()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Double(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.CheerBig;
        }
    }

    public void VoiceTurkey()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Turkey(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.CheerBig;
        }
    }

    public void VoiceSplit()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Split(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Oooh;
        }
    }

    public void VoiceSplit710()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Split710(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Oooh;
        }
    }

    public void VoiceSplitPick()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.SplitPick(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.CheerMed;
        }
    }

    public void VoiceMost()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Most(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
    }

    public void VoiceFew()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Few(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.CrowdCrap;
        }
    }

    public void VoiceOne()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.One(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Laugh;
        }
    }

    public void VoiceMiss()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.Miss(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Laugh;
        }
    }

    public void VoiceEndBad()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.EndBad(commentatorAudio);
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Firework;
        }
    }

    public void VoiceEndOk()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.EndOk(commentatorAudio);
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Firework;
        }
    }

    public void VoiceEndGood()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.EndGood(commentatorAudio);
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Firework;
        }
    }

    public void VoiceEndGreat()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentator.EndGreat(commentatorAudio);
            commentatorIndex = 1;
            commentatorAudio.Play();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Firework;
        }
    }

    public void PerfectGame()
    {
        if (GameManager.isVoice && type != GameState.Menu && maxBalls != 1)
        {
            commentatorAudio.clip = Resources.Load<AudioClip>("Sound/crowd-perfect");
            commentatorAudio.Play();
            commentatorIndex = 3;
        }
        else
        {
            crowdAudio.Stop();
            commentatorAudio.Stop();
        }
        if (GameManager.isVoice || crowdType > Crowds.NoCrowd)
        {
            crowdType = Crowds.Firework;
        }
    }

    public void VoiceStop()
    {
        commentator.commentators1[(int)voices1].Strike.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Strike.voices.Length);
        commentator.commentators1[(int)voices1].Spare.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Spare.voices.Length);
        commentator.commentators1[(int)voices1].Gutterball.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Gutterball.voices.Length);
        commentator.commentators1[(int)voices1].Double.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Double.voices.Length);
        commentator.commentators1[(int)voices1].Turkey.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Turkey.voices.Length);
        commentator.commentators1[(int)voices1].Split.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Split.voices.Length);
        commentator.commentators1[(int)voices1].Split710.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Split710.voices.Length);
        commentator.commentators1[(int)voices1].SplitPick.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].SplitPick.voices.Length);
        commentator.commentators1[(int)voices1].Most.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Most.voices.Length);
        commentator.commentators1[(int)voices1].Few.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Few.voices.Length);
        commentator.commentators1[(int)voices1].One.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].One.voices.Length);
        commentator.commentators1[(int)voices1].Miss.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].Miss.voices.Length);
        commentator.commentators1[(int)voices1].EndBad.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].EndBad.voices.Length);
        commentator.commentators1[(int)voices1].EndOk.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].EndOk.voices.Length);
        commentator.commentators1[(int)voices1].EndGood.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].EndGood.voices.Length);
        commentator.commentators1[(int)voices1].EndGreat.voiceIndex = Random.Range(0, commentator.commentators1[(int)voices1].EndGreat.voices.Length);
        commentator.commentators2[(int)voices2].Strike.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Strike.voices.Length);
        commentator.commentators2[(int)voices2].Spare.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Spare.voices.Length);
        commentator.commentators2[(int)voices2].Gutterball.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Gutterball.voices.Length);
        commentator.commentators2[(int)voices2].Double.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Double.voices.Length);
        commentator.commentators2[(int)voices2].Turkey.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Turkey.voices.Length);
        commentator.commentators2[(int)voices2].Split.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Split.voices.Length);
        commentator.commentators2[(int)voices2].Split710.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Split710.voices.Length);
        commentator.commentators2[(int)voices2].SplitPick.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].SplitPick.voices.Length);
        commentator.commentators2[(int)voices2].Most.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Most.voices.Length);
        commentator.commentators2[(int)voices2].Few.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Few.voices.Length);
        commentator.commentators2[(int)voices2].One.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].One.voices.Length);
        commentator.commentators2[(int)voices2].Miss.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].Miss.voices.Length);
        commentator.commentators2[(int)voices2].EndBad.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].EndBad.voices.Length);
        commentator.commentators2[(int)voices2].EndOk.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].EndOk.voices.Length);
        commentator.commentators2[(int)voices2].EndGood.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].EndGood.voices.Length);
        commentator.commentators2[(int)voices2].EndGreat.voiceIndex = Random.Range(0, commentator.commentators2[(int)voices2].EndGreat.voices.Length);
        commentatorIndex = 0;
        commentatorAudio.Stop();
    }

    public void CrowdStop()
    {
        if (crowdType != Crowds.NoCrowd)
        {
            crowdAudio.Stop();
        }
    }

    public void CheerBig()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.CheerBig(crowdAudio);
        }
    }

    public void CheerMed()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.CheerMed(crowdAudio);
        }
    }

    public void CrowdOk()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.CrowdOk(crowdAudio);
        }
    }

    public void CrowdHohum()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.CrowdHohum(crowdAudio);
        }
    }

    public void CrowdCrap()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.CrowdCrap(crowdAudio);
        }
    }

    public void Laugh()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.Laugh(crowdAudio);
        }
    }

    public void Roll()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.Roll(rollCrowd);
        }
        if (!isComputer && crowd.roll.Length >= 1 && crowdType == Crowds.NoCrowd && type != GameState.Menu)
        {
            crowdAudio.Stop();
        }
    }

    public void Oooh()
    {
        if (GameManager.isCrowd && type != GameState.Menu)
        {
            crowd.Oooh(crowdAudio);
        }
    }

    public void GutterLaugh()
    {
        if (GameManager.isCrowd && !GameManager.isVoice && type != GameState.Menu)
        {
            crowd.Laugh(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 3;
            commentatorAudio.Play();
        }
        crowdType = Crowds.Laugh;
    }

    public void StopScooper()
    {
        if (b != null)
        {
            StopCoroutine(b);
        }
        GameObject.FindObjectOfType<PinSetter>().SkipScooper();
        GameObject.FindObjectOfType<PinSetter>().StopScooper();
    }

    public void StopAllAnimations()
    {
        animations = AnimationScenes.Off;
        for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
        {
            GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].StopAnimation();
        }
        for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
        {
            GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].StopAnimation();
        }
        for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
        {
            GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].StopAnimation();
        }
        for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes.Length; i++)
        {
            GameObject.FindObjectOfType<PinSetter>().gutterballAnimationTwoTimes[i].StopAnimation();
        }
        for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
        {
            GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].StopAnimation();
        }
        for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
        {
            GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].StopAnimation();
        }
    }

    void SkipReplay()
    {
        replayText.SetActive(false);
        if (isAnim)
        {
            StopAllAnimations();
        }
        isAnim = false;
        animations = AnimationScenes.Off;
        isReplay = false;
        isReplayRecord = false;
        isCurrentReplay = false;
        ball.isGutter = false;
        gutterAnimation = 0;
        isPin = false;
        for (int i = 0; i < replays.Length; i++)
        {
            replays[i].RigidBodyCollider(true);
            replays[i].Clear();
        }
        currentReplayIndex = 0;
        replayTime = 0;
        GameObject.FindObjectOfType<PinSetter>().SkipScooper();
        GameObject.FindObjectOfType<PinSetter>().StopScooper();
        if (PinCounter.pinCount != 0 && throwBall < maxBalls && !isResetPins)
        {
            GameObject.FindObjectOfType<PinSetter>().OutOfPins();
        }
        else if (PinCounter.pinCount == 0 || throwBall == maxBalls || isResetPins)
        {
            if (type == GameState.Menu)
            {
                RandomChargeBall();
            }
            if (GameManager.pinMode != GameManager.PinMode.Spare)
            {
                pinCounter.Reset();
                GameObject.FindObjectOfType<PinSetter>().ResetPins();
            }
            else
            {
                GameObject.FindObjectOfType<PinSetter>().ResetPinsFall();
            }
            throwBall = 0;
            isResetPins = false;
            isSplit = false;
            is710 = false;
        }
        foreach (var bounce in GameObject.FindObjectsOfType<TriggerSound>())
        {
            bounce.Reset();
        }
        if (isEndGame)
        {
            if (GameManager.isParticle)
            {
                StartCoroutine(FireworksPop());
            }
            foreach (GameObject falseObject in falseObjects)
            {
                falseObject.SetActive(false);
            }
            foreach (GameObject trueObject in trueObjects)
            {
                trueObject.SetActive(true);
            }
            if (isReplay)
            {
                ballObject.SetActive(false);
            }
            cameraFollow.EndCam();
            type = GameState.EndGame;
            CrowdStop();
            if (maxBalls == 1)
            {
                winGameBalls[17].SetActive(true);
                wins = 0;
            }
            else if (allPlayers == Players.OnePlayer && maxBalls != 1)
            {
                gameManager.bowler[GameManager.turnNameIndex1].playerStrikes += strikes1;
                gameManager.bowler[GameManager.turnNameIndex1].playerSpares += spares1;
                gameManager.bowler[GameManager.turnNameIndex1].playerGutters += gutters1;
                winGameBalls[0].SetActive(true);
                wins = 1;
                if (score1 <= 99)
                {
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 0;
                }
                else if (score1 >= 100 && score1 <= 149)
                {
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 50;
                }
                else if (score1 >= 150 && score1 <= 199)
                {
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 100;
                }
                else if (score1 >= 200 && score1 <= 249)
                {
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 500;
                }
                else if (score1 >= 250 && score1 <= 299)
                {
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 1000;
                }
                else if (score1 >= 300)
                {
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                }
                foreach (Text winPrize1 in winGamePrize1)
                {
                    if (score1 <= 99)
                    {
                        winPrize1.text = "prize: 0";
                    }
                    else if (score1 >= 100 && score1 <= 149)
                    {
                        winPrize1.text = "prize: 50";
                    }
                    else if (score1 >= 150 && score1 <= 199)
                    {
                        winPrize1.text = "prize: 100";
                    }
                    else if (score1 >= 200 && score1 <= 249)
                    {
                        winPrize1.text = "prize: 500";
                    }
                    else if (score1 >= 250 && score1 <= 299)
                    {
                        winPrize1.text = "prize: 1,000";
                    }
                    else if (score1 >= 300)
                    {
                        winPrize1.text = "prize: 5,000";
                    }
                }
            }
            else if (allPlayers == Players.TwoPlayer && maxBalls != 1)
            {
                gameManager.bowler[GameManager.turnNameIndex1].playerStrikes += strikes1;
                gameManager.bowler[GameManager.turnNameIndex1].playerSpares += spares1;
                gameManager.bowler[GameManager.turnNameIndex1].playerGutters += gutters1;
                gameManager.bowler[GameManager.turnNameIndex2].playerStrikes += strikes2;
                gameManager.bowler[GameManager.turnNameIndex2].playerSpares += spares2;
                gameManager.bowler[GameManager.turnNameIndex2].playerGutters += gutters2;
                if (score2 < score1)
                {
                    winGameBalls[0].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 1000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                }
                else if (score1 < score2)
                {
                    winGameBalls[1].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 1000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                }
                else if (score1 == score2)
                {
                    winGameBalls[5].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 1000;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 1000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                }
                foreach (Text winPrize1 in winGamePrize1)
                {
                    winPrize1.text = "prize: 1,000";
                }
                foreach (Text winPrize2 in winGamePrize2)
                {
                    winPrize2.text = "prize: 1,000";
                }
            }
            else if (allPlayers == Players.ThreePlayer && maxBalls != 1)
            {
                gameManager.bowler[GameManager.turnNameIndex1].playerStrikes += strikes1;
                gameManager.bowler[GameManager.turnNameIndex1].playerSpares += spares1;
                gameManager.bowler[GameManager.turnNameIndex1].playerGutters += gutters1;
                gameManager.bowler[GameManager.turnNameIndex2].playerStrikes += strikes2;
                gameManager.bowler[GameManager.turnNameIndex2].playerSpares += spares2;
                gameManager.bowler[GameManager.turnNameIndex2].playerGutters += gutters2;
                gameManager.bowler[GameManager.turnNameIndex3].playerStrikes += strikes3;
                gameManager.bowler[GameManager.turnNameIndex3].playerSpares += spares3;
                gameManager.bowler[GameManager.turnNameIndex3].playerGutters += gutters3;
                if (score2 < score1 && score3 < score1)
                {
                    winGameBalls[0].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                }
                else if (score1 < score2 && score3 < score2)
                {
                    winGameBalls[1].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                }
                else if (score1 < score3 && score2 < score3)
                {
                    winGameBalls[2].SetActive(true);
                    wins = 3;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                }
                else if (score1 == score2 && score1 < score3)
                {
                    winGameBalls[5].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                }
                else if (score1 < score2 && score1 == score3)
                {
                    winGameBalls[6].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                }
                else if (score1 < score2 && score2 == score3)
                {
                    winGameBalls[9].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                }
                else if (score1 == score2 && score1 == score3)
                {
                    winGameBalls[12].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 2500;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                }
                foreach (Text winPrize1 in winGamePrize1)
                {
                    winPrize1.text = "prize: 2,500";
                }
                foreach (Text winPrize2 in winGamePrize2)
                {
                    winPrize2.text = "prize: 2,500";
                }
                foreach (Text winPrize3 in winGamePrize3)
                {
                    winPrize3.text = "prize: 2,500";
                }
            }
            else if (allPlayers == Players.FourPlayer && maxBalls != 1)
            {
                gameManager.bowler[GameManager.turnNameIndex1].playerStrikes += strikes1;
                gameManager.bowler[GameManager.turnNameIndex1].playerSpares += spares1;
                gameManager.bowler[GameManager.turnNameIndex1].playerGutters += gutters1;
                gameManager.bowler[GameManager.turnNameIndex2].playerStrikes += strikes2;
                gameManager.bowler[GameManager.turnNameIndex2].playerSpares += spares2;
                gameManager.bowler[GameManager.turnNameIndex2].playerGutters += gutters2;
                gameManager.bowler[GameManager.turnNameIndex3].playerStrikes += strikes3;
                gameManager.bowler[GameManager.turnNameIndex3].playerSpares += spares3;
                gameManager.bowler[GameManager.turnNameIndex3].playerGutters += gutters3;
                gameManager.bowler[GameManager.turnNameIndex4].playerStrikes += strikes4;
                gameManager.bowler[GameManager.turnNameIndex4].playerSpares += spares4;
                gameManager.bowler[GameManager.turnNameIndex4].playerGutters += gutters4;
                if (score2 < score1 && score3 < score1 && score4 < score1)
                {
                    winGameBalls[0].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score1 < score2 && score3 < score2 && score4 < score2)
                {
                    winGameBalls[1].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score1 < score3 && score2 < score3 && score4 < score3)
                {
                    winGameBalls[2].SetActive(true);
                    wins = 3;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score1 < score4 && score4 < score2 && score4 < score3)
                {
                    winGameBalls[3].SetActive(true);
                    wins = 4;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score1 == score2 && score3 < score1 && score4 < score1)
                {
                    winGameBalls[5].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score2 < score1 && score1 == score3 && score4 < score1)
                {
                    winGameBalls[6].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score2 < score1 && score3 < score1 && score1 == score4)
                {
                    winGameBalls[7].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score1 < score2 && score2 == score3 && score1 < score2)
                {
                    winGameBalls[9].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score1 < score2 && score3 < score2 && score2 == score4)
                {
                    winGameBalls[10].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score1 < score3 && score2 < score3 && score3 == score4)
                {
                    winGameBalls[11].SetActive(true);
                    wins = 3;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score1 == score2 && score1 == score3 && score4 < score1)
                {
                    winGameBalls[12].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerLoss++;
                }
                else if (score1 == score2 && score3 < score1 && score1 == score4)
                {
                    winGameBalls[13].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score2 < score1 && score1 == score3 && score1 == score4)
                {
                    winGameBalls[14].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score1 < score2 && score2 == score3 && score2 == score4)
                {
                    winGameBalls[15].SetActive(true);
                    wins = 2;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                else if (score1 == score2 && score1 == score3 && score1 == score4)
                {
                    winGameBalls[16].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex2].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex3].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex4].playerMoney += 5000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex2].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex3].playerWin++;
                    gameManager.bowler[GameManager.turnNameIndex4].playerWin++;
                }
                foreach (Text winPrize1 in winGamePrize1)
                {
                    winPrize1.text = "prize: 5,000";
                }
                foreach (Text winPrize2 in winGamePrize2)
                {
                    winPrize2.text = "prize: 5,000";
                }
                foreach (Text winPrize3 in winGamePrize3)
                {
                    winPrize3.text = "prize: 5,000";
                }
                foreach (Text winPrize4 in winGamePrize4)
                {
                    winPrize4.text = "prize: 5,000";
                }
            }
            else if (allPlayers == Players.Computer && maxBalls != 1)
            {
                gameManager.bowler[GameManager.turnNameIndex1].playerStrikes += strikes1;
                gameManager.bowler[GameManager.turnNameIndex1].playerSpares += spares1;
                gameManager.bowler[GameManager.turnNameIndex1].playerGutters += gutters1;
                if (score2 < score1)
                {
                    winGameBalls[0].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 1000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                }
                else if (score1 < score2)
                {
                    winGameBalls[4].SetActive(true);
                    wins = 5;
                    gameManager.bowler[GameManager.turnNameIndex1].playerLoss++;
                }
                else if (score1 == score2)
                {
                    winGameBalls[8].SetActive(true);
                    wins = 1;
                    gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 1000;
                    gameManager.bowler[GameManager.turnNameIndex1].playerWin++;
                }
                foreach (Text winPrize1 in winGamePrize1)
                {
                    winPrize1.text = "prize: 1,000";
                }
                foreach (Text winPrize2 in winGamePrize2)
                {
                    winPrize2.text = "prize: 1,000";
                }
            }
            if (wins == 0)
            {
                if (stage >= gameManager.chooseBalls[GameManager.unlockBallSpare].totalLock && gameManager.chooseBalls[GameManager.unlockBallSpare].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallSpare].lockType == ChooseBall.LockType.Spare)
                {
                    unlockedText.SetActive(true);
                    unlockedBallSpare.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallSpare, 0);
                    PlayerPrefs.SetInt("SaveBallSpare", GameManager.unlockBallSpare += 1);
                }
            }
            else if (wins == 1)
            {
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
                ballRender.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
                playerTurn = 0;
                turns = 0;
                playersBallTwo[0].SetActive(true);
                playersBallTwo[1].SetActive(false);
                playersBallTwo[2].SetActive(false);
                playersBallTwo[3].SetActive(false);
                if (score1 <= 99)
                {
                    VoiceEndBad();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Foo05");
                }
                else if(score1 >= 100 && score1 <= 149)
                {
                    VoiceEndOk();
                }
                else if(score1 >= 150 && score1 <= 249)
                {
                    VoiceEndGood();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                }
                else if(score1 >= 250 && score1 <= 299)
                {
                    VoiceEndGreat();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/App01");
                }
                else if(score1 >= 300)
                {
                    PerfectGame();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/end-perfect");
                }
                if (score1 >= 200 && gameManager.isLockAlleys[(int)alleyLockType] == 1)
                {
                    unlockedText.SetActive(true);
                    unlockedAlley.SetActive(true);
                    PlayerPrefs.SetInt("SaveAlleys" + (int)alleyLockType, 2);
                }
                if (allPlayers == Players.Computer && gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].isLock == 1)
                {
                    unlockedText.SetActive(true);
                    unlockedBallBeat.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, 0);
                }
                if (score1 >= gameManager.chooseBalls[GameManager.unlockBallScore].totalLock && gameManager.chooseBalls[GameManager.unlockBallScore].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallScore].lockType == ChooseBall.LockType.Score)
                {
                    unlockedText.SetActive(true);
                    unlockedBallScore.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallScore, 0);
                    PlayerPrefs.SetInt("SaveBallScore", GameManager.unlockBallScore += 1);
                }
            }
            else if (wins == 2)
            {
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls2].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls2].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls2].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls2].spin;
                ballRender.material = gameManager.chooseBalls[GameManager.turnBalls2].ballMat;
                playerTurn = 1;
                turns = 1;
                playersBallTwo[0].SetActive(false);
                playersBallTwo[1].SetActive(true);
                playersBallTwo[2].SetActive(false);
                playersBallTwo[3].SetActive(false);
                if (score2 <= 99)
                {
                    VoiceEndBad();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Foo05");
                }
                else if(score2 >= 100 && score2 <= 149)
                {
                    VoiceEndOk();
                }
                else if(score2 >= 150 && score2 <= 249)
                {
                    VoiceEndGood();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                }
                else if(score2 >= 250 && score2 <= 299)
                {
                    VoiceEndGreat();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/App01");
                }
                else if(score2 >= 300)
                {
                    PerfectGame();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/end-perfect");
                }
                if (score2 >= 200 && gameManager.isLockAlleys[(int)alleyLockType] == 1)
                {
                    unlockedText.SetActive(true);
                    unlockedAlley.SetActive(true);
                    PlayerPrefs.SetInt("SaveAlleys" + (int)alleyLockType, 2);
                }
                if (score2 >= gameManager.chooseBalls[GameManager.unlockBallScore].totalLock && gameManager.chooseBalls[GameManager.unlockBallScore].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallScore].lockType == ChooseBall.LockType.Score)
                {
                    unlockedText.SetActive(true);
                    unlockedBallScore.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallScore, 0);
                    PlayerPrefs.SetInt("SaveBallScore", GameManager.unlockBallScore += 1);
                }
            }
            else if (wins == 3)
            {
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls3].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls3].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls3].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls3].spin;
                ballRender.material = gameManager.chooseBalls[GameManager.turnBalls3].ballMat;
                playerTurn = 2;
                turns = 2;
                playersBallTwo[0].SetActive(false);
                playersBallTwo[1].SetActive(false);
                playersBallTwo[2].SetActive(true);
                playersBallTwo[3].SetActive(false);
                if (score3 <= 99)
                {
                    VoiceEndBad();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Foo05");
                }
                else if (score3 >= 100 && score3 <= 149)
                {
                    VoiceEndOk();
                }
                else if (score3 >= 150 && score3 <= 249)
                {
                    VoiceEndGood();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                }
                else if (score3 >= 250 && score3 <= 299)
                {
                    VoiceEndGreat();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/App01");
                }
                else if (score3 >= 300)
                {
                    PerfectGame();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/end-perfect");
                }
                if (score3 >= 200 && gameManager.isLockAlleys[(int)alleyLockType] == 1)
                {
                    unlockedText.SetActive(true);
                    unlockedAlley.SetActive(true);
                    PlayerPrefs.SetInt("SaveAlleys" + (int)alleyLockType, 2);
                }
                if (score3 >= gameManager.chooseBalls[GameManager.unlockBallScore].totalLock && gameManager.chooseBalls[GameManager.unlockBallScore].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallScore].lockType == ChooseBall.LockType.Score)
                {
                    unlockedText.SetActive(true);
                    unlockedBallScore.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallScore, 0);
                    PlayerPrefs.SetInt("SaveBallScore", GameManager.unlockBallScore += 1);
                }
            }
            else if (wins == 4)
            {
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex4].playerName;
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls4].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls4].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls4].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls4].spin;
                ballRender.material = gameManager.chooseBalls[GameManager.turnBalls4].ballMat;
                playerTurn = 3;
                turns = 3;
                playersBallTwo[0].SetActive(false);
                playersBallTwo[1].SetActive(false);
                playersBallTwo[2].SetActive(false);
                playersBallTwo[3].SetActive(true);
                if (score4 <= 99)
                {
                    VoiceEndBad();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Foo05");
                }
                else if(score4 >= 100 && score4 <= 149)
                {
                    VoiceEndOk();
                }
                else if(score4 >= 150 && score4 <= 249)
                {
                    VoiceEndGood();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                }
                else if(score4 >= 250 && score4 <= 299)
                {
                    VoiceEndGreat();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/App01");
                }
                else if(score4 >= 300)
                {
                    PerfectGame();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/end-perfect");
                }
                if (score4 >= 200 && gameManager.isLockAlleys[(int)alleyLockType] == 1)
                {
                    unlockedText.SetActive(true);
                    unlockedAlley.SetActive(true);
                    PlayerPrefs.SetInt("SaveAlleys" + (int)alleyLockType, 2);
                }
                if (score4 >= gameManager.chooseBalls[GameManager.unlockBallScore].totalLock && gameManager.chooseBalls[GameManager.unlockBallScore].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallScore].lockType == ChooseBall.LockType.Score)
                {
                    unlockedText.SetActive(true);
                    unlockedBallScore.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallScore, 0);
                    PlayerPrefs.SetInt("SaveBallScore", GameManager.unlockBallScore += 1);
                }
            }
            else if (wins == 5)
            {
                playerNameText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName;
                ballNameText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName;
                ballDataText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs + "lbs.  speed:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed + "  spin:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin;
                ballRender.material = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballMat;
                playerTurn = 4;
                turns = 1;
                playersBallTwo[0].SetActive(false);
                playersBallTwo[1].SetActive(true);
                playersBallTwo[2].SetActive(false);
                playersBallTwo[3].SetActive(false);
                if (score2 <= 99)
                {
                    VoiceEndBad();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Foo05");
                }
                else if(score2 >= 100 && score2 <= 149)
                {
                    VoiceEndOk();
                }
                else if(score2 >= 150 && score2 <= 249)
                {
                    VoiceEndGood();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                }
                else if(score2 >= 250 && score2 <= 299)
                {
                    VoiceEndGreat();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/App01");
                }
                else if(score2 >= 300)
                {
                    PerfectGame();
                    winCrowd.clip = Resources.Load<AudioClip>("Sound/end-perfect");
                }
            }
            foreach (PlayerObj playerEarn in gameManager.bowler)
            {
                if (playerEarn.playerMoney >= gameManager.chooseBalls[GameManager.unlockBallEarn].totalLock && gameManager.chooseBalls[GameManager.unlockBallEarn].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallEarn].lockType == ChooseBall.LockType.Earn)
                {
                    unlockedText.SetActive(true);
                    unlockedBallEarn.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallEarn, 0);
                    if (GameManager.unlockBallEarn < 39)
                    {
                        PlayerPrefs.SetInt("SaveBallEarn", GameManager.unlockBallEarn += 5);
                    }
                    else if (GameManager.unlockBallEarn == 39)
                    {
                        PlayerPrefs.SetInt("SaveBallEarn", 84);
                    }
                }
            }
            FileData.SaveToSAV<PlayerObj>(gameManager.bowler, "SaveBowler");
            GameManager.moneys += addCash;
        }
        else
        {
            if (type != GameState.Menu)
            {
                if (nextTurn == 0)
                {
                    playerNameText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                    ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
                    ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
                    ball.ChargeBall(gameManager.chooseBalls[GameManager.turnBalls1].ballMat, gameManager.chooseBalls[GameManager.turnBalls1].lbs, gameManager.chooseBalls[GameManager.turnBalls1].speed, gameManager.chooseBalls[GameManager.turnBalls1].spin);
                    chooseBallUI.SetActive(true);
                    powerUpUI.SetActive(true);
                    playerTurn = 0;
                    turns = 0;
                    isComputer = false;
                    playersBallTwo[0].SetActive(true);
                    playersBallTwo[1].SetActive(false);
                    playersBallTwo[2].SetActive(false);
                    playersBallTwo[3].SetActive(false);
                }
                else if (nextTurn == 1)
                {
                    playerNameText.text = gameManager.bowler[GameManager.turnNameIndex2].playerName;
                    ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls2].ballName;
                    ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls2].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls2].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls2].spin;
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls2].ballMat;
                    ball.ChargeBall(gameManager.chooseBalls[GameManager.turnBalls2].ballMat, gameManager.chooseBalls[GameManager.turnBalls2].lbs, gameManager.chooseBalls[GameManager.turnBalls2].speed, gameManager.chooseBalls[GameManager.turnBalls2].spin);
                    chooseBallUI.SetActive(true);
                    powerUpUI.SetActive(true);
                    playerTurn = 1;
                    turns = 1;
                    isComputer = false;
                    playersBallTwo[0].SetActive(false);
                    playersBallTwo[1].SetActive(true);
                    playersBallTwo[2].SetActive(false);
                    playersBallTwo[3].SetActive(false);
                }
                else if (nextTurn == 2)
                {
                    playerNameText.text = gameManager.bowler[GameManager.turnNameIndex3].playerName;
                    ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls3].ballName;
                    ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls3].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls3].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls3].spin;
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls3].ballMat;
                    ball.ChargeBall(gameManager.chooseBalls[GameManager.turnBalls3].ballMat, gameManager.chooseBalls[GameManager.turnBalls3].lbs, gameManager.chooseBalls[GameManager.turnBalls3].speed, gameManager.chooseBalls[GameManager.turnBalls3].spin);
                    chooseBallUI.SetActive(true);
                    powerUpUI.SetActive(true);
                    playerTurn = 2;
                    turns = 2;
                    isComputer = false;
                    playersBallTwo[0].SetActive(false);
                    playersBallTwo[1].SetActive(false);
                    playersBallTwo[2].SetActive(true);
                    playersBallTwo[3].SetActive(false);
                }
                else if (nextTurn == 3)
                {
                    playerNameText.text = gameManager.bowler[GameManager.turnNameIndex4].playerName;
                    ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls4].ballName;
                    ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls4].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls4].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls4].spin;
                    ballRender.material = gameManager.chooseBalls[GameManager.turnBalls4].ballMat;
                    ball.ChargeBall(gameManager.chooseBalls[GameManager.turnBalls4].ballMat, gameManager.chooseBalls[GameManager.turnBalls4].lbs, gameManager.chooseBalls[GameManager.turnBalls4].speed, gameManager.chooseBalls[GameManager.turnBalls4].spin);
                    chooseBallUI.SetActive(true);
                    powerUpUI.SetActive(true);
                    playerTurn = 3;
                    turns = 3;
                    isComputer = false;
                    playersBallTwo[0].SetActive(false);
                    playersBallTwo[1].SetActive(false);
                    playersBallTwo[2].SetActive(false);
                    playersBallTwo[3].SetActive(true);
                }
                else if (nextTurn == 4)
                {
                    playerNameText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName;
                    ballNameText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName;
                    ballDataText.text = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs + "lbs.  speed:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed + "  spin:" + gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin;
                    ballRender.material = gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballMat;
                    ball.ChargeBall(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballMat, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin);
                    chooseBallUI.SetActive(false);
                    powerUpUI.SetActive(false);
                    playerTurn = 4;
                    turns = 1;
                    isComputer = true;
                    playersBallTwo[0].SetActive(false);
                    playersBallTwo[1].SetActive(true);
                    playersBallTwo[2].SetActive(false);
                    playersBallTwo[3].SetActive(false);
                }
            }
            else
            {
                ball.ResetBowl();
                ball.ResetCam();
            }
            if (type == GameState.Replay)
            {
                if (isComputer)
                {
                    ball.ResetBowl();
                    ball.ResetCam();
                }
                else
                {
                    ball.Reset();
                    ball.ResetCam();
                }
                type = GameState.Game;
            }
        }
    }

    public void RainCountdown()
    {
        if (rainIndex == 0 && GameObject.FindObjectOfType<PinSetter>().isRain)
        {
            if (GameObject.FindObjectOfType<PinSetter>().psDrop != null && GameManager.isWeather)
            {
                GameObject.FindObjectOfType<PinSetter>().psDrop.Play();
            }
            if (GameObject.FindObjectOfType<PinSetter>().isThunder && GameManager.isWeather)
            {
                if (sfx != null && gameManager != null && GameManager.isSound && type != GameState.Menu)
                {
                    PlayClip("thunder");
                }
                if (rainPorch != null && gameManager != null && GameManager.isSound && type != GameState.Menu)
                {
                    rainPorch.Play();
                }
                thunderAnimation.Play();
            }
        }
        else if (rainIndex == 5)
        {
            if (GameObject.FindObjectOfType<PinSetter>().psDrop != null)
            {
                GameObject.FindObjectOfType<PinSetter>().psDrop.Stop();
            }
            if (GameObject.FindObjectOfType<PinSetter>().isThunder)
            {
                if (rainPorch != null)
                {
                    rainPorch.Stop();
                }
            }
        }
        rainIndex++;
    }

    public void PlayClip(string clipName)
    {
        if (GameManager.isSound && type != GameState.Menu)
        {
            sfx.PlayOneShot(Resources.Load<AudioClip>("Sound/" + clipName));
        }
    }

    public void MouseDown()
    {
        if (Input.GetMouseButtonDown(0) && type == GameState.Replay)
        {
            if (isAnim)
            {
                for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().strikeAnimations.Length; i++)
                {
                    GameObject.FindObjectOfType<PinSetter>().strikeAnimations[i].SkipAnimation();
                }
                for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().spareAnimations.Length; i++)
                {
                    GameObject.FindObjectOfType<PinSetter>().spareAnimations[i].SkipAnimation();
                }
                for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().gutterballAnimations.Length; i++)
                {
                    GameObject.FindObjectOfType<PinSetter>().gutterballAnimations[i].SkipAnimation();
                }
                for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().doubleAnimations.Length; i++)
                {
                    GameObject.FindObjectOfType<PinSetter>().doubleAnimations[i].SkipAnimation();
                }
                for (int i = 0; i < GameObject.FindObjectOfType<PinSetter>().turkeyAnimations.Length; i++)
                {
                    GameObject.FindObjectOfType<PinSetter>().turkeyAnimations[i].SkipAnimation();
                }
                StopAllAnimations();
            }
            isAnim = false;
            if (b != null)
            {
                StopCoroutine(b);
            }
            if (isCurrentReplay)
            {
                for (int i = 0; i < replays.Length; i++)
                {
                    replays[i].RigidBodyFreeze(false);
                    replays[i].RigidBodyCollider(true);
                    replays[i].SetTransform(replays[i].actionReplayRecords.Count - 1);
                    replays[i].Clear();
                    if (throwBall < maxBalls)
                    {
                        replays[i].SetVelocity();
                    }
                }
            }
            SkipReplay();
        }
    }

    public void PlayGame()
    {
        GameManager.isHighScore = false;
        VoiceStop();
        AudioStop();
        loadingUI.SetActive(true);
        SceneManager.LoadScene("Main");
        type = GameState.Intro;
        PlayerPrefs.SetInt("PinModes", (int)GameManager.pinMode);
    }

    public void Sending()
    {
        StartCoroutine(EmailSend());
    }

    public void QuitMenu()
    {
        VoiceStop();
        CrowdStop();
        AudioStop();
        SceneManager.LoadScene("Main");
        type = GameState.Menu;
    }

    public void EndGame()
    {
        if (GameManager.pinMode != GameManager.PinMode.Spare)
        {
            switch (GameManager.chooseAlleys)
            {
                case GameManager.Alley.Retro:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Retro");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Retro");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Retro");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Retro");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Retro");
                        AddBowlerScore(gameManager.r_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Retro");
                    }
                    break;
                case GameManager.Alley.Wacky:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Wacky");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Wacky");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Wacky");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Wacky");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Wacky");
                        AddBowlerScore(gameManager.w_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Wacky");
                    }
                    break;
                case GameManager.Alley.Iceberg:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Iceberg");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Iceberg");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Iceberg");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Iceberg");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Iceberg");
                        AddBowlerScore(gameManager.i_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Iceberg");
                    }
                    break;
                case GameManager.Alley.Jungle:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Jungle");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Jungle");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Jungle");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Jungle");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Jungle");
                        AddBowlerScore(gameManager.j_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Jungle");
                    }
                    break;
                case GameManager.Alley.Zen:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Zen");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Zen");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Zen");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Zen");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Zen");
                        AddBowlerScore(gameManager.z_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Zen");
                    }
                    break;
                case GameManager.Alley.Cosmic:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Cosmic");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Cosmic");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Cosmic");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Cosmic");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Cosmic");
                        AddBowlerScore(gameManager.c_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Cosmic");
                    }
                    break;
                case GameManager.Alley.Barnyard:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Barnyard");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Barnyard");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Barnyard");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Barnyard");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Barnyard");
                        AddBowlerScore(gameManager.b_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Barnyard");
                    }
                    break;
                case GameManager.Alley.Mineshaft:
                    if (allPlayers == Players.OnePlayer)
                    {
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Mineshaft");
                    }
                    if (allPlayers == Players.TwoPlayer)
                    {
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Mineshaft");
                    }
                    if (allPlayers == Players.ThreePlayer)
                    {
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Mineshaft");
                    }
                    if (allPlayers == Players.FourPlayer)
                    {
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex2].playerName, score2, strikes2, spares2, gutters2), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex3].playerName, score3, strikes3, spares3, gutters3), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex4].playerName, score4, strikes4, spares4, gutters4), "HS_Mineshaft");
                    }
                    if (allPlayers == Players.Computer)
                    {
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.bowler[GameManager.turnNameIndex1].playerName, score1, strikes1, spares1, gutters1), "HS_Mineshaft");
                        AddBowlerScore(gameManager.m_hs, new ScoreBowler(gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].CPUName, score2, strikes2, spares2, gutters2), "HS_Mineshaft");
                    }
                    break;
            }
            GameManager.isHighScore = true;
        }
        VoiceStop();
        CrowdStop();
        crowdAudio.Stop();
        rollCrowd.Stop();
        winCrowd.Stop();
        fireworksMulti.Stop();
        sfx.Stop();
        rainPorch.Stop();
        SceneManager.LoadScene("Main");
        type = GameState.Menu;
    }

    void AddBowlerScore(List<ScoreBowler> bowlerList, ScoreBowler element, string filename)
    {
        for (int i = 0; i < bowlerList.Count; i++)
        {
            if (i > bowlerList.Count || element.playerScore > bowlerList[i].playerScore)
            {
                bowlerList.Insert(i, element);

                FileData.SaveToSAV<ScoreBowler>(bowlerList, filename);

                if (onHighscoreListChanged != null)
                {
                    onHighscoreListChanged.Invoke(bowlerList);
                }

                break;
            }
        }
        if (bowlerList.Count == 0 || bowlerList.Count >= 1 && element.playerScore <= bowlerList[bowlerList.Count - 1].playerScore)
        {
            bowlerList.Add(element);

            FileData.SaveToSAV<ScoreBowler>(bowlerList, filename);

            if (onHighscoreListChanged != null)
            {
                onHighscoreListChanged.Invoke(bowlerList);
            }
        }
    }

    IEnumerator EmailSend()
    {
        sendingEmail.SetActive(true);
        yield return new WaitForSeconds(9);
        sendingEmail.SetActive(false);
        emailSent.SetActive(true);
        yield return new WaitForSeconds(6);
        emailSent.SetActive(false);
        menuAlleyUI.SetActive(true);
    }

    public void ChargeBall(int chargeBalls)
    {
        ball.ChargeBall(gameManager.chooseBalls[chargeBalls].ballMat, gameManager.chooseBalls[chargeBalls].lbs, gameManager.chooseBalls[chargeBalls].speed, gameManager.chooseBalls[chargeBalls].spin);
    }

    public void RandomChargeBall()
    {
        chargeBallIndex = Random.Range(0, gameManager.chooseBalls.Length);
        ball.ChargeBall(gameManager.chooseBalls[chargeBallIndex].ballMat, gameManager.chooseBalls[chargeBallIndex].lbs, gameManager.chooseBalls[chargeBallIndex].speed, gameManager.chooseBalls[chargeBallIndex].spin);
    }

    public void PrevGutterball1()
    {
        voices1--;
        if (voices1 < Commentators.Baxter)
        {
            voices1 = Commentators.Master;
        }
        if (GameManager.isVoice)
        {
            OpeningStop();
            commentator.PlayGutterball1(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 0;
            commentatorAudio.Play();
        }
    }

    public void NextGutterball1()
    {
        voices1++;
        if (voices1 > Commentators.Master)
        {
            voices1 = Commentators.Baxter;
        }
        if (GameManager.isVoice)
        {
            OpeningStop();
            commentator.PlayGutterball1(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 0;
            commentatorAudio.Play();
        }
    }

    public void PrevGutterball2()
    {
        voices2--;
        if (voices2 < Commentators.Baxter)
        {
            voices2 = Commentators.Master;
        }
        if (GameManager.isVoice)
        {
            OpeningStop();
            commentator.PlayGutterball2(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 0;
            commentatorAudio.Play();
        }
    }

    public void NextGutterball2()
    {
        voices2++;
        if (voices2 > Commentators.Master)
        {
            voices2 = Commentators.Baxter;
        }
        if (GameManager.isVoice)
        {
            OpeningStop();
            commentator.PlayGutterball2(commentatorAudio);
            crowdAudio.Stop();
            commentatorIndex = 0;
            commentatorAudio.Play();
        }
    }

    public void PrevQuality()
    {
        GameManager.qualityIndex--;
        if (GameManager.qualityIndex < 0)
        {
            GameManager.qualityIndex = 5;
        }
        QualitySettings.SetQualityLevel(GameManager.qualityIndex);
        PlayerPrefs.SetInt("SaveQuality", GameManager.qualityIndex);
    }

    public void NextQuality()
    {
        GameManager.qualityIndex++;
        if (GameManager.qualityIndex > 5)
        {
            GameManager.qualityIndex = 0;
        }
        QualitySettings.SetQualityLevel(GameManager.qualityIndex);
        PlayerPrefs.SetInt("SaveQuality", GameManager.qualityIndex);
    }

    public void PrevResolution()
    {
        GameManager.resolutionIndex--;
        if (GameManager.resolutionIndex < 0)
        {
            GameManager.resolutionIndex = GameManager.resolutions.Length - 1;
        }
        Screen.SetResolution(GameManager.resolutions[GameManager.resolutionIndex].width, GameManager.resolutions[GameManager.resolutionIndex].height, Screen.fullScreen);
        PlayerPrefs.SetInt("SaveResolution", GameManager.resolutionIndex);
    }

    public void NextResolution()
    {
        GameManager.resolutionIndex++;
        if (GameManager.resolutionIndex > GameManager.resolutions.Length - 1)
        {
            GameManager.resolutionIndex = 0;
        }
        Screen.SetResolution(GameManager.resolutions[GameManager.resolutionIndex].width, GameManager.resolutions[GameManager.resolutionIndex].height, Screen.fullScreen);
        PlayerPrefs.SetInt("SaveResolution", GameManager.resolutionIndex);
    }

    public void TenPin()
    {
        GameManager.pinMode = GameManager.PinMode.Tenpin;
    }

    public void Spare(GameObject setObject)
    {
        if (GameManager.unlockRegister == 0)
        {
            setObject.SetActive(true);
            GameManager.pinMode = GameManager.PinMode.Spare;
        }
        else if (GameManager.unlockRegister == 1)
        {
            registerUI.SetActive(true);
        }
    }

    public void PrevAlleys()
    {
        GameManager.chooseAlleys--;
        if (GameManager.chooseAlleys < GameManager.Alley.Retro)
        {
            GameManager.chooseAlleys = GameManager.Alley.Cosmic;
        }
        ChargeAlleys();
        StartChargeBalls();
        AlleyRegister();
        PlayerPrefs.SetInt("ChooseAlleys", (int)GameManager.chooseAlleys);
    }

    public void NextAlleys()
    {
        GameManager.chooseAlleys++;
        if (GameManager.chooseAlleys > GameManager.Alley.Cosmic)
        {
            GameManager.chooseAlleys = GameManager.Alley.Retro;
        }
        ChargeAlleys();
        StartChargeBalls();
        AlleyRegister();
        PlayerPrefs.SetInt("ChooseAlleys", (int)GameManager.chooseAlleys);
    }

    public void PrevBowler()
    {
        if (playerTurn == 0)
        {
            GameManager.turnNameIndex1--;
        }
        else if (playerTurn == 1)
        {
            GameManager.turnNameIndex2--;
        }
        else if (playerTurn == 2)
        {
            GameManager.turnNameIndex3--;
        }
        else if (playerTurn == 3)
        {
            GameManager.turnNameIndex4--;
        }
        if (GameManager.turnNameIndex1 < 0)
        {
            GameManager.turnNameIndex1 = gameManager.bowler.Count - 1;
        }
        else if (GameManager.turnNameIndex2 < 0)
        {
            GameManager.turnNameIndex2 = gameManager.bowler.Count - 1;
        }
        else if (GameManager.turnNameIndex3 < 0)
        {
            GameManager.turnNameIndex3 = gameManager.bowler.Count - 1;
        }
        else if (GameManager.turnNameIndex4 < 0)
        {
            GameManager.turnNameIndex4 = gameManager.bowler.Count - 1;
        }
        PlayerPrefs.SetInt("SavePlayer1", GameManager.turnNameIndex1);
        PlayerPrefs.SetInt("SavePlayer2", GameManager.turnNameIndex2);
        PlayerPrefs.SetInt("SavePlayer3", GameManager.turnNameIndex3);
        PlayerPrefs.SetInt("SavePlayer4", GameManager.turnNameIndex4);
    }

    public void NextBowler()
    {
        if (playerTurn == 0)
        {
            GameManager.turnNameIndex1++;
        }
        else if (playerTurn == 1)
        {
            GameManager.turnNameIndex2++;
        }
        else if (playerTurn == 2)
        {
            GameManager.turnNameIndex3++;
        }
        else if (playerTurn == 3)
        {
            GameManager.turnNameIndex4++;
        }
        if (GameManager.turnNameIndex1 >= gameManager.bowler.Count)
        {
            GameManager.turnNameIndex1 = 0;
        }
        else if (GameManager.turnNameIndex2 >= gameManager.bowler.Count)
        {
            GameManager.turnNameIndex2 = 0;
        }
        else if (GameManager.turnNameIndex3 >= gameManager.bowler.Count)
        {
            GameManager.turnNameIndex3 = 0;
        }
        else if (GameManager.turnNameIndex4 >= gameManager.bowler.Count)
        {
            GameManager.turnNameIndex4 = 0;
        }
        PlayerPrefs.SetInt("SavePlayer1", GameManager.turnNameIndex1);
        PlayerPrefs.SetInt("SavePlayer2", GameManager.turnNameIndex2);
        PlayerPrefs.SetInt("SavePlayer3", GameManager.turnNameIndex3);
        PlayerPrefs.SetInt("SavePlayer4", GameManager.turnNameIndex4);
    }

    public void PrevBalls()
    {
        if (playerTurn == 0)
        {
            GameManager.turnBalls1--;
        }
        else if (playerTurn == 1)
        {
            GameManager.turnBalls2--;
        }
        else if (playerTurn == 2)
        {
            GameManager.turnBalls3--;
        }
        else if (playerTurn == 3)
        {
            GameManager.turnBalls4--;
        }
        else if (playerTurn == 4)
        {
            GameManager.turnBallsCPU--;
        }
        if (type != GameState.Menu)
        {
            if (GameManager.turnBalls1 < 0)
            {
                if (GameManager.unlockRegister == 0)
                {
                    GameManager.turnBalls1 = gameManager.chooseBalls.Length - 1;
                }
                else if (GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls1 = 4;
                }
            }
            else if (GameManager.turnBalls2 < 0)
            {
                if (GameManager.unlockRegister == 0)
                {
                    GameManager.turnBalls2 = gameManager.chooseBalls.Length - 1;
                }
                else if (GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls2 = 4;
                }
            }
            else if (GameManager.turnBalls3 < 0)
            {
                if (GameManager.unlockRegister == 0)
                {
                    GameManager.turnBalls3 = gameManager.chooseBalls.Length - 1;
                }
                else if (GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls3 = 4;
                }
            }
            else if (GameManager.turnBalls4 < 0)
            {
                if (GameManager.unlockRegister == 0)
                {
                    GameManager.turnBalls4 = gameManager.chooseBalls.Length - 1;
                }
                else if (GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls4 = 4;
                }
            }
            for (int i = 0; i < gameManager.chooseBalls.Length; i++)
            {
                if (playerTurn == 0 && GameManager.unlockRegister == 0 && gameManager.chooseBalls[GameManager.turnBalls1].isLock == 1)
                {
                    GameManager.turnBalls1 -= gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 1 && GameManager.unlockRegister == 0 && gameManager.chooseBalls[GameManager.turnBalls2].isLock == 1)
                {
                    GameManager.turnBalls2 -= gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 2 && GameManager.unlockRegister == 0 && gameManager.chooseBalls[GameManager.turnBalls3].isLock == 1)
                {
                    GameManager.turnBalls3 -= gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 3 && GameManager.unlockRegister == 0 && gameManager.chooseBalls[GameManager.turnBalls4].isLock == 1)
                {
                    GameManager.turnBalls4 -= gameManager.chooseBalls[i].isLock;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (playerTurn == 0 && GameManager.unlockRegister == 1 && gameManager.chooseBalls[GameManager.turnBalls1].isLock == 1)
                {
                    GameManager.turnBalls1 -= gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 1 && GameManager.unlockRegister == 1 && gameManager.chooseBalls[GameManager.turnBalls2].isLock == 1)
                {
                    GameManager.turnBalls2 -= gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 2 && GameManager.unlockRegister == 1 && gameManager.chooseBalls[GameManager.turnBalls3].isLock == 1)
                {
                    GameManager.turnBalls3 -= gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 3 && GameManager.unlockRegister == 1 && gameManager.chooseBalls[GameManager.turnBalls4].isLock == 1)
                {
                    GameManager.turnBalls4 -= gameManager.chooseBalls[i].isLock;
                }
            }
        }
        else
        {
            if (GameManager.turnBalls1 < 0)
            {
                GameManager.turnBalls1 = gameManager.chooseBalls.Length - 1;
            }
            else if (GameManager.turnBalls2 < 0)
            {
                GameManager.turnBalls2 = gameManager.chooseBalls.Length - 1;
            }
            else if (GameManager.turnBalls3 < 0)
            {
                GameManager.turnBalls3 = gameManager.chooseBalls.Length - 1;
            }
            else if (GameManager.turnBalls4 < 0)
            {
                GameManager.turnBalls4 = gameManager.chooseBalls.Length - 1;
            }
            else if (GameManager.turnBallsCPU < 0)
            {
                GameManager.turnBallsCPU = gameManager.compuObj.Length - 1;
            }
        }
    }

    public void NextBalls()
    {
        if (playerTurn == 0)
        {
            GameManager.turnBalls1++;
        }
        else if (playerTurn == 1)
        {
            GameManager.turnBalls2++;
        }
        else if (playerTurn == 2)
        {
            GameManager.turnBalls3++;
        }
        else if (playerTurn == 3)
        {
            GameManager.turnBalls4++;
        }
        else if (playerTurn == 4)
        {
            GameManager.turnBallsCPU++;
        }
        if (type != GameState.Menu)
        {
            if (GameManager.turnBalls1 >= gameManager.chooseBalls.Length || GameManager.turnBalls1 >= 5 && GameManager.unlockRegister == 1)
            {
                GameManager.turnBalls1 = 0;
            }
            else if (GameManager.turnBalls2 >= gameManager.chooseBalls.Length || GameManager.turnBalls2 >= 5 && GameManager.unlockRegister == 1)
            {
                GameManager.turnBalls2 = 0;
            }
            else if (GameManager.turnBalls3 >= gameManager.chooseBalls.Length || GameManager.turnBalls3 >= 5 && GameManager.unlockRegister == 1)
            {
                GameManager.turnBalls3 = 0;
            }
            else if (GameManager.turnBalls4 >= gameManager.chooseBalls.Length || GameManager.turnBalls4 >= 5 && GameManager.unlockRegister == 1)
            {
                GameManager.turnBalls4 = 0;
            }
            for (int i = 0; i < gameManager.chooseBalls.Length; i++)
            {
                if (playerTurn == 0 && gameManager.chooseBalls[GameManager.turnBalls1].isLock == 1)
                {
                    GameManager.turnBalls1 += gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 1 && gameManager.chooseBalls[GameManager.turnBalls2].isLock == 1)
                {
                    GameManager.turnBalls2 += gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 2 && gameManager.chooseBalls[GameManager.turnBalls3].isLock == 1)
                {
                    GameManager.turnBalls3 += gameManager.chooseBalls[i].isLock;
                }
                else if (playerTurn == 3 && gameManager.chooseBalls[GameManager.turnBalls4].isLock == 1)
                {
                    GameManager.turnBalls4 += gameManager.chooseBalls[i].isLock;
                }
                if (GameManager.turnBalls1 >= gameManager.chooseBalls.Length || GameManager.turnBalls1 >= 5 && GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls1 = 0;
                }
                else if (GameManager.turnBalls2 >= gameManager.chooseBalls.Length || GameManager.turnBalls2 >= 5 && GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls2 = 0;
                }
                else if (GameManager.turnBalls3 >= gameManager.chooseBalls.Length || GameManager.turnBalls3 >= 5 && GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls3 = 0;
                }
                else if (GameManager.turnBalls4 >= gameManager.chooseBalls.Length || GameManager.turnBalls4 >= 5 && GameManager.unlockRegister == 1)
                {
                    GameManager.turnBalls4 = 0;
                }
            }
        }
        else
        {
            if (GameManager.turnBalls1 >= gameManager.chooseBalls.Length)
            {
                GameManager.turnBalls1 = 0;
            }
            else if (GameManager.turnBalls2 >= gameManager.chooseBalls.Length)
            {
                GameManager.turnBalls2 = 0;
            }
            else if (GameManager.turnBalls3 >= gameManager.chooseBalls.Length)
            {
                GameManager.turnBalls3 = 0;
            }
            else if (GameManager.turnBalls4 >= gameManager.chooseBalls.Length)
            {
                GameManager.turnBalls4 = 0;
            }
            else if (GameManager.turnBallsCPU >= gameManager.compuObj.Length)
            {
                GameManager.turnBallsCPU = 0;
            }
        }
    }

    public void SetPlayer(int playerBalls)
    {
        playerTurn = playerBalls;
    }

    public void SetMusic(bool isMusic)
    {
        GameManager.isMusic = isMusic;
        PlayerPrefs.SetInt("SaveMusic", (isMusic ? 0 : 1));
    }

    public void SetSound(bool isSound)
    {
        GameManager.isSound = isSound;
        PlayerPrefs.SetInt("SaveSound", (isSound ? 0 : 1));
    }

    public void SetCrowd(bool isCrowd)
    {
        GameManager.isCrowd = isCrowd;
        PlayerPrefs.SetInt("SaveCrowd", (isCrowd ? 0 : 1));
    }

    public void SetVoice(bool isVoice)
    {
        GameManager.isVoice = isVoice;
        PlayerPrefs.SetInt("SaveVoice", (isVoice ? 0 : 1));
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetChars(bool isChars)
    {
        GameManager.isChars = isChars;
        PlayerPrefs.SetInt("SaveChars", (isChars ? 0 : 1));
    }

    public void SetParticle(bool isParticle)
    {
        GameManager.isParticle = isParticle;
        PlayerPrefs.SetInt("SaveParticle", (isParticle ? 0 : 1));
    }

    public void SetReflect(bool isReflect)
    {
        GameManager.isReflect = isReflect;
        PlayerPrefs.SetInt("SaveReflect", (isReflect ? 0 : 1));
    }

    public void SetWeather(bool isWeather)
    {
        GameManager.isWeather = isWeather;
        PlayerPrefs.SetInt("SaveWeather", (isWeather ? 0 : 1));
    }

    public void SetShake(bool isShake)
    {
        GameManager.isShake = isShake;
        PlayerPrefs.SetInt("SaveShake", (isShake ? 0 : 1));
    }

    public void SetAlwaysBowl(bool isAlwaysBowl)
    {
        for (int i = 0; i < nextButton.Length; i++)
        {
            nextButton[i].GetComponent<Button>().interactable = isAlwaysBowl;
        }
        for (int i = 0; i < bowlButton.Length; i++)
        {
            bowlButton[i].GetComponent<Button>().interactable = isAlwaysBowl;
        }
    }

    public void SetAlwaysBowlReg(bool isAlwaysReg)
    {
        for (int i = 0; i < nextButton.Length; i++)
        {
            nextButton[i].SetActive(isAlwaysReg);
        }
        for (int i = 0; i < bowlButton.Length; i++)
        {
            bowlButton[i].SetActive(isAlwaysReg);
        }
    }

    public void PlaySong()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Process.Start("song.mp4");
        }
        else
        {
            Application.OpenURL("https://archive.org/download/Jxqryv9-068/Jxqryv9-068.mp4");
        }
    }

    public void WebURL()
    {
        Application.OpenURL("www.skunkstudios.com");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpeningStop()
    {
        if (GameManager.isOpening)
        {
            opening.Stop();
            music.Play();
            GameManager.isOpening = false;
        }
    }

    public void AudioStop()
    {
        opening.Stop();
        music.Stop();
        GameManager.isOpening = false;
        foreach (GameObject sound in sounds)
        {
            sound.GetComponent<AudioSource>().Stop();
        }
    }

    public void HideAlleyScores()
    {
        alleyScores[0].SetActive(false);
        alleyScores[1].SetActive(false);
        alleyScores[2].SetActive(false);
        alleyScores[3].SetActive(false);
        alleyScores[4].SetActive(false);
        alleyScores[5].SetActive(false);
        alleyScores[6].SetActive(false);
        alleyScores[7].SetActive(false);
    }

    public void ShowAlleyScores()
    {
        switch (GameManager.chooseAlleys)
        {
            case GameManager.Alley.Retro:
                alleyScores[0].SetActive(true);
                break;
            case GameManager.Alley.Wacky:
                alleyScores[1].SetActive(true);
                break;
            case GameManager.Alley.Iceberg:
                alleyScores[2].SetActive(true);
                break;
            case GameManager.Alley.Jungle:
                alleyScores[3].SetActive(true);
                break;
            case GameManager.Alley.Zen:
                alleyScores[4].SetActive(true);
                break;
            case GameManager.Alley.Cosmic:
                alleyScores[5].SetActive(true);
                break;
            case GameManager.Alley.Barnyard:
                alleyScores[6].SetActive(true);
                break;
            case GameManager.Alley.Mineshaft:
                alleyScores[7].SetActive(true);
                break;
        }
    }

    public void PlayerOne()
    {
        playerTurn = 0;
        allPlayers = Players.OnePlayer;
    }

    public void PlayerTwo()
    {
        playerTurn = 0;
        allPlayers = Players.TwoPlayer;
    }

    public void PlayerThree()
    {
        playerTurn = 0;
        allPlayers = Players.ThreePlayer;
    }

    public void PlayerFour()
    {
        playerTurn = 0;
        allPlayers = Players.FourPlayer;
    }

    public void PlayerComputer()
    {
        playerTurn = 0;
        allPlayers = Players.Computer;
    }

    public void ChargeAlleys()
    {
        switch (GameManager.chooseAlleys)
        {
            case GameManager.Alley.Retro:
                voices1 = Commentators.Maria;
                voices2 = Commentators.Jensen;
                alleyLockType = GameManager.Alley.Retro;
                break;
            case GameManager.Alley.Wacky:
                voices1 = Commentators.Baxter;
                voices2 = Commentators.Maria;
                alleyLockType = GameManager.Alley.Cosmic;
                break;
            case GameManager.Alley.Iceberg:
                voices1 = Commentators.Natasha;
                voices2 = Commentators.Jensen;
                alleyLockType = GameManager.Alley.Barnyard;
                break;
            case GameManager.Alley.Jungle:
                voices1 = Commentators.Jensen;
                voices2 = Commentators.Master;
                alleyLockType = GameManager.Alley.Mineshaft;
                break;
            case GameManager.Alley.Zen:
                voices1 = Commentators.Master;
                voices2 = Commentators.Natasha;
                alleyLockType = GameManager.Alley.Retro;
                break;
            case GameManager.Alley.Cosmic:
                voices1 = Commentators.Master;
                voices2 = Commentators.Baxter;
                lockAlleyText.text = "Score 200 in Wacky to Unlock";
                break;
            case GameManager.Alley.Barnyard:
                voices1 = Commentators.Jensen;
                voices2 = Commentators.Natasha;
                lockAlleyText.text = "Score 200 in Iceberg to Unlock";
                break;
            case GameManager.Alley.Mineshaft:
                voices1 = Commentators.Baxter;
                voices2 = Commentators.Jensen;
                lockAlleyText.text = "Score 200 in Jungle to Unlock";
                break;
        }
    }

    public void StartChargeBalls()
    {
        switch (GameManager.chooseAlleys)
        {
            case GameManager.Alley.Retro:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 0;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 1;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 2;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 3;
                }
                GameManager.turnBallsCPU = 0;
                break;
            case GameManager.Alley.Wacky:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 5;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 6;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 7;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 8;
                }
                GameManager.turnBallsCPU = 2;
                break;
            case GameManager.Alley.Iceberg:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 10;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 11;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 12;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 13;
                }
                GameManager.turnBallsCPU = 4;
                break;
            case GameManager.Alley.Jungle:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 15;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 16;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 17;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 18;
                }
                GameManager.turnBallsCPU = 6;
                break;
            case GameManager.Alley.Zen:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 20;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 21;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 22;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 23;
                }
                GameManager.turnBallsCPU = 8;
                break;
            case GameManager.Alley.Cosmic:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 25;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 26;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 27;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 28;
                }
                GameManager.turnBallsCPU = 10;
                break;
            case GameManager.Alley.Barnyard:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 30;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 31;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 32;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 33;
                }
                GameManager.turnBallsCPU = 12;
                break;
            case GameManager.Alley.Mineshaft:
                if (GameManager.startBall <= 0)
                {
                    GameManager.turnBalls1 = 35;
                }
                if (GameManager.startBall <= 1)
                {
                    GameManager.turnBalls2 = 36;
                }
                if (GameManager.startBall <= 2)
                {
                    GameManager.turnBalls3 = 37;
                }
                if (GameManager.startBall <= 3)
                {
                    GameManager.turnBalls4 = 38;
                }
                GameManager.turnBallsCPU = 14;
                break;
        }
    }

    public void AlleyRegister()
    {
        if (gameManager.isLockAlleys[(int)GameManager.chooseAlleys] == 0 && GameManager.unlockRegister == 0)
        {
            alleyRegistered.SetActive(false);
            startButton.SetActive(true);
            menuRegisterButton.SetActive(false);
            startButton.GetComponent<Button>().interactable = true;
            lockAlleyUI.SetActive(false);
            unlockAlleyUI.SetActive(false);
        }
        else if (gameManager.isLockAlleys[(int)GameManager.chooseAlleys] == 1 && GameManager.unlockRegister == 0)
        {
            alleyRegistered.SetActive(false);
            startButton.SetActive(true);
            menuRegisterButton.SetActive(false);
            startButton.GetComponent<Button>().interactable = false;
            unlockAlleyUI.SetActive(false);
            lockAlleyUI.SetActive(true);
        }
        else if (gameManager.isLockAlleys[(int)GameManager.chooseAlleys] == 2 && GameManager.unlockRegister == 0)
        {
            alleyRegistered.SetActive(false);
            startButton.SetActive(true);
            menuRegisterButton.SetActive(false);
            startButton.GetComponent<Button>().interactable = true;
            lockAlleyUI.SetActive(false);
            unlockAlleyUI.SetActive(true);
        }
        if ((int)GameManager.chooseAlleys == 0 && GameManager.unlockRegister == 1)
        {
            alleyRegistered.SetActive(false);
            startButton.SetActive(true);
            menuRegisterButton.SetActive(false);
        }
        else if ((int)GameManager.chooseAlleys >= 1 && GameManager.unlockRegister == 1)
        {
            alleyRegistered.SetActive(true);
            startButton.SetActive(false);
            menuRegisterButton.SetActive(true);
        }
    }

    public void UnlockRegister()
    {
        if (keyField.text != "DCQ6HT9PJYGPB8RJCCR3")
        {
            registerComplete.SetActive(false);
            registerFail.SetActive(true);
        }
        else
        {
            GameManager.unlockRegister = 0;
            PlayerPrefs.SetInt("UnlockRegister", GameManager.unlockRegister);
            registerFail.SetActive(false);
            registerComplete.SetActive(true);
        }
        AlleyRegister();
    }

    public void GoBackMenu()
    {
        if (GameManager.isHighScore)
        {
            highScoreUI.SetActive(true);
            ShowAlleyScores();
        }
        else
        {
            menuAlleyUI.SetActive(true);
        }
        GameManager.isHighScore = false;
    }

    public void CancelBowlerToList()
    {
        bowlerField.text = "";
    }

    public void AddBowlerToList()
    {
        if (bowlerField.text != "")
        {
            gameManager.bowler.Add(new PlayerObj(bowlerField.text, 0, 0, 0, 0, 0, 0));
            GameObject element = Instantiate(bowlerUIElement, bowlerWrapper) as GameObject;
            element.GetComponent<BowlerUI>().BowlerUpdate(bowlerField.text, 0, 0, 0, 0, 0, 0);
        }
        bowlerField.text = "";
        FileData.SaveToSAV<PlayerObj>(gameManager.bowler, "SaveBowler");
    }

    public void CustomBall()
    {
        if (playerTurn == 0)
        {
            customBallNameField.text = PlayerPrefs.GetString("CustomBallName" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].ballName);
            customBallLbs.value = PlayerPrefs.GetInt("CustomBallLbs" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].lbs);
            customBallSpeed.value = PlayerPrefs.GetInt("CustomBallSpeed" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].speed);
            customBallSpin.value = PlayerPrefs.GetInt("CustomBallSpin" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].spin);
            customBallFileField.text = PlayerPrefs.GetString("CustomBallURL" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].urlTextureBall);
        }
        else if (playerTurn == 1)
        {
            customBallNameField.text = PlayerPrefs.GetString("CustomBallName" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].ballName);
            customBallLbs.value = PlayerPrefs.GetInt("CustomBallLbs" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].lbs);
            customBallSpeed.value = PlayerPrefs.GetInt("CustomBallSpeed" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].speed);
            customBallSpin.value = PlayerPrefs.GetInt("CustomBallSpin" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].spin);
            customBallFileField.text = PlayerPrefs.GetString("CustomBallURL" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].urlTextureBall);
        }
        else if (playerTurn == 2)
        {
            customBallNameField.text = PlayerPrefs.GetString("CustomBallName" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].ballName);
            customBallLbs.value = PlayerPrefs.GetInt("CustomBallLbs" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].lbs);
            customBallSpeed.value = PlayerPrefs.GetInt("CustomBallSpeed" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].speed);
            customBallSpin.value = PlayerPrefs.GetInt("CustomBallSpin" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].spin);
            customBallFileField.text = PlayerPrefs.GetString("CustomBallURL" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].urlTextureBall);
        }
        else if (playerTurn == 3)
        {
            customBallNameField.text = PlayerPrefs.GetString("CustomBallName" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].ballName);
            customBallLbs.value = PlayerPrefs.GetInt("CustomBallLbs" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].lbs);
            customBallSpeed.value = PlayerPrefs.GetInt("CustomBallSpeed" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].speed);
            customBallSpin.value = PlayerPrefs.GetInt("CustomBallSpin" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].spin);
            customBallFileField.text = PlayerPrefs.GetString("CustomBallURL" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].urlTextureBall);
        }
        else if (playerTurn == 4)
        {
            customBallNameField.text = PlayerPrefs.GetString("CustomBallName" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName);
            customBallLbs.value = PlayerPrefs.GetInt("CustomBallLbs" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs);
            customBallSpeed.value = PlayerPrefs.GetInt("CustomBallSpeed" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed);
            customBallSpin.value = PlayerPrefs.GetInt("CustomBallSpin" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin);
            customBallFileField.text = PlayerPrefs.GetString("CustomBallURL" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].urlTextureBall);
        }
    }

    public void ApplyCustomBall()
    {
        if (playerTurn == 0)
        {
            gameManager.chooseBalls[GameManager.turnBalls1].ballName = customBallNameField.text;
            gameManager.chooseBalls[GameManager.turnBalls1].lbs = (int)customBallLbs.value;
            gameManager.chooseBalls[GameManager.turnBalls1].speed = (int)customBallSpeed.value;
            gameManager.chooseBalls[GameManager.turnBalls1].spin = (int)customBallSpin.value;
            gameManager.chooseBalls[GameManager.turnBalls1].urlTextureBall = customBallFileField.text;
            PlayerPrefs.SetString("CustomBallName" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].ballName);
            PlayerPrefs.SetInt("CustomBallLbs" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].lbs);
            PlayerPrefs.SetInt("CustomBallSpeed" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].speed);
            PlayerPrefs.SetInt("CustomBallSpin" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].spin);
            PlayerPrefs.SetString("CustomBallURL" + GameManager.turnBalls1, gameManager.chooseBalls[GameManager.turnBalls1].urlTextureBall);
            StartCoroutine(gameManager.DownloadTexture(customBallFileField.text, gameManager.chooseBalls[GameManager.turnBalls1].ballMat));
        }
        else if (playerTurn == 1)
        {
            gameManager.chooseBalls[GameManager.turnBalls2].ballName = customBallNameField.text;
            gameManager.chooseBalls[GameManager.turnBalls2].lbs = (int)customBallLbs.value;
            gameManager.chooseBalls[GameManager.turnBalls2].speed = (int)customBallSpeed.value;
            gameManager.chooseBalls[GameManager.turnBalls2].spin = (int)customBallSpin.value;
            gameManager.chooseBalls[GameManager.turnBalls2].urlTextureBall = customBallFileField.text;
            PlayerPrefs.SetString("CustomBallName" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].ballName);
            PlayerPrefs.SetInt("CustomBallLbs" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].lbs);
            PlayerPrefs.SetInt("CustomBallSpeed" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].speed);
            PlayerPrefs.SetInt("CustomBallSpin" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].spin);
            PlayerPrefs.SetString("CustomBallURL" + GameManager.turnBalls2, gameManager.chooseBalls[GameManager.turnBalls2].urlTextureBall);
            StartCoroutine(gameManager.DownloadTexture(customBallFileField.text, gameManager.chooseBalls[GameManager.turnBalls2].ballMat));
        }
        else if (playerTurn == 2)
        {
            gameManager.chooseBalls[GameManager.turnBalls3].ballName = customBallNameField.text;
            gameManager.chooseBalls[GameManager.turnBalls3].lbs = (int)customBallLbs.value;
            gameManager.chooseBalls[GameManager.turnBalls3].speed = (int)customBallSpeed.value;
            gameManager.chooseBalls[GameManager.turnBalls3].spin = (int)customBallSpin.value;
            gameManager.chooseBalls[GameManager.turnBalls3].urlTextureBall = customBallFileField.text;
            PlayerPrefs.SetString("CustomBallName" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].ballName);
            PlayerPrefs.SetInt("CustomBallLbs" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].lbs);
            PlayerPrefs.SetInt("CustomBallSpeed" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].speed);
            PlayerPrefs.SetInt("CustomBallSpin" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].spin);
            PlayerPrefs.SetString("CustomBallURL" + GameManager.turnBalls3, gameManager.chooseBalls[GameManager.turnBalls3].urlTextureBall);
            StartCoroutine(gameManager.DownloadTexture(customBallFileField.text, gameManager.chooseBalls[GameManager.turnBalls3].ballMat));
        }
        else if (playerTurn == 3)
        {
            gameManager.chooseBalls[GameManager.turnBalls4].ballName = customBallNameField.text;
            gameManager.chooseBalls[GameManager.turnBalls4].lbs = (int)customBallLbs.value;
            gameManager.chooseBalls[GameManager.turnBalls4].speed = (int)customBallSpeed.value;
            gameManager.chooseBalls[GameManager.turnBalls4].spin = (int)customBallSpin.value;
            gameManager.chooseBalls[GameManager.turnBalls4].urlTextureBall = customBallFileField.text;
            PlayerPrefs.SetString("CustomBallName" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].ballName);
            PlayerPrefs.SetInt("CustomBallLbs" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].lbs);
            PlayerPrefs.SetInt("CustomBallSpeed" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].speed);
            PlayerPrefs.SetInt("CustomBallSpin" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].spin);
            PlayerPrefs.SetString("CustomBallURL" + GameManager.turnBalls4, gameManager.chooseBalls[GameManager.turnBalls4].urlTextureBall);
            StartCoroutine(gameManager.DownloadTexture(customBallFileField.text, gameManager.chooseBalls[GameManager.turnBalls4].ballMat));
        }
        else if (playerTurn == 4)
        {
            gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName = customBallNameField.text;
            gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs = (int)customBallLbs.value;
            gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed = (int)customBallSpeed.value;
            gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin = (int)customBallSpin.value;
            gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].urlTextureBall = customBallFileField.text;
            PlayerPrefs.SetString("CustomBallName" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballName);
            PlayerPrefs.SetInt("CustomBallLbs" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].lbs);
            PlayerPrefs.SetInt("CustomBallSpeed" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].speed);
            PlayerPrefs.SetInt("CustomBallSpin" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].spin);
            PlayerPrefs.SetString("CustomBallURL" + gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].urlTextureBall);
            StartCoroutine(gameManager.DownloadTexture(customBallFileField.text, gameManager.chooseBalls[gameManager.compuObj[GameManager.turnBallsCPU].cpuIndex].ballMat));
        }
    }

    public void CancelFileInfo()
    {
        infoFileField.text = "";
    }

    public void AddFileInfo()
    {
        if (infoFileField.text != "")
        {
            gameManager.urlInfoScreen.Add(infoFileField.text);
        }
        infoFileField.text = "";
        FileData.SaveToSAV<string>(gameManager.urlInfoScreen, "InfoURL");
    }

    public void BuyBomb()
    {
        if (GameManager.moneys >= 1000)
        {
            GameManager.bombBalls++;
            GameManager.moneys -= 1000;
            PlayClip("buy_powerups");
        }
        else
        {
            PlayClip("not_powerups");
        }
    }

    public void BuyHyper()
    {
        if (GameManager.moneys >= 500)
        {
            GameManager.hyperBalls++;
            GameManager.moneys -= 500;
            PlayClip("buy_powerups");
        }
        else
        {
            PlayClip("not_powerups");
        }
    }

    public void BuyLightning()
    {
        if (GameManager.moneys >= 250)
        {
            GameManager.lightningBalls++;
            GameManager.moneys -= 250;
            PlayClip("buy_powerups");
        }
        else
        {
            PlayClip("not_powerups");
        }
    }

    public void ClickAudio()
    {
        if (GameManager.isSound)
        {
            GameObject.Find("UIClick").GetComponent<AudioSource>().Play();
        }
    }

    public void CodeAudio()
    {
        if (GameManager.isSound)
        {
            GameObject.Find("UICode").GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator FireworksPop()
    {
        yield return new WaitForSeconds(Random.Range(0.3f, 3f));
        while (true)
        {
            GameObject pop = Instantiate(fireworks, new Vector3(Random.Range(-512, 512), Random.Range(-256, 256), 5000), Quaternion.identity) as GameObject;
            var main = pop.GetComponent<ParticleSystem>().main;
            main.startColor = pop.GetComponent<Fireworks>().colorFireworks[Random.Range(0, pop.GetComponent<Fireworks>().colorFireworks.Length)];
            yield return new WaitForSeconds(Random.Range(0.3f, 3f));
        }
    }
}
