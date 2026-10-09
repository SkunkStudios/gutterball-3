using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHundredPin : MonoBehaviour
{
    public enum BallPowerUps { Off, Bomb, Clone, ForcePulse, Hyper, Lightning }
    public BallPowerUps powerUps;
    public enum BallType { MoveX, ThrowBall, SpinBall, FallBall }
    public BallType ballType;
    public enum CameraType { Intro, MoveX, DropBall, FollowBall, LookBall, Replay, Anim, MoveCam, EndCam }
    public CameraType camType;
    private List<int> rolls = new List<int>();
    private List<int> scores = new List<int>();
    private int score;
    private int strikes;
    private int spares;
    private int gutters;
    private int frames = 1;
    private int allStrikes = 1;
    private int allSpares = 1;
    private int ballTurn = 0;

    public ScoreDisplayHundredPin scoreDisplay;
    public GameObject winGameBall;
    public Text winGameName;
    public Text winGameScore;
    public Text winGamePrize;
    public Text winGameHighScore;
    public int pinsCounter;
    public int throwBall = 0;
    public bool isPin = false;
    public HundredPinCounter pinCounter;
    public AudioSource crowdAudio;
    public AudioSource rollCrowd;
    public AudioSource winCrowd;
    public AudioSource fireworksMulti;
    public AudioSource sfx;
    public CameraFollowHundredPin cameraFollow;
    public bool isReplay;
    public bool isReplayRecord = false;
    public bool isCurrentReplay = false;
    public float currentReplayIndex;
    public Animation thunderAnimation;
    public GameObject replayText;
    public GameObject ballObject;
    public MeshRenderer ballRender;
    public GameObject ballCloudRender;
    public Text playerNameText;
    public Text ballNameText;
    public Text ballDataText;
    public TextMesh playerName;
    public Text scoreTextBall;
    public GameObject chooseBallUI;
    public GameObject powerUpUI;
    public GameObject[] trueObjects;
    public GameObject[] falseObjects;
    public GameObject unlockedText;
    public GameObject unlockedBallEarn;
    public GameObject[] pins;
    public AudioClip bowling1;
    public AudioClip bowling2;
    public AudioClip bowling4;
    public AudioClip bowling6;
    public AudioClip bowling10;
    public GameObject fireworks;
    public Text moneyText;
    public Text bombBallText;
    public Text cloneBallText;
    public Text forcePulseBallText;
    public Text hyperBallText;
    public Text lightningBallText;
    public Text bombShopText;
    public Text cloneShopText;
    public Text forcePulseShopText;
    public Text hyperShopText;
    public Text lightningShopText;
    public Button bombBallButton;
    public Button cloneBallButton;
    public Button forcePulseBallButton;
    public Button hyperBallButton;
    public Button lightningBallButton;
    public GameObject cloneBallTimesText;
    public AudioClip[] exploreClips;
    public GameObject[] explores;
    public GameObject[] bigExplores;

    private AudioSource music;
    private GameObject[] sounds;
    private bool isIntro = true;
    private GameManager gameManager;
    private BallHundredPin ball;
    private Crowd crowd;
    private Coroutine b;
    private ActionReplayHundredPin[] replays;
    private float timer;
    private float maxTimer;
    private bool isWaitPin = false;
    private bool isEndGame;
    private float replayTime = 0;
    private bool isRain = true;
    private int chargeBallIndex;
    private bool isResetPins = false;
    private int pinCounts;
    private int addCash = 0;
    public delegate void OnHighscoreListChanged(List<ScoreBowler> list);
    public static event OnHighscoreListChanged onHighscoreListChanged;

    // Use this for initialization
    void Start()
    {
        pinCounter.Reset();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        crowdAudio = GameObject.Find("Crowd").GetComponent<AudioSource>();
        rollCrowd = GameObject.Find("RollingCrowd").GetComponent<AudioSource>();
        winCrowd = GameObject.Find("WinCrowd").GetComponent<AudioSource>();
        sfx = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.SavePrefs();
        for (int customBalls = 0; customBalls < gameManager.chooseBalls.Length; customBalls++)
        {
            if (customBalls > 89)
            {
                gameManager.chooseBalls[customBalls].ballName = PlayerPrefs.GetString("CustomBallName" + customBalls, gameManager.chooseBalls[customBalls].ballName);
                gameManager.chooseBalls[customBalls].lbs = PlayerPrefs.GetInt("CustomBallLbs" + customBalls, 12);
                gameManager.chooseBalls[customBalls].speed = PlayerPrefs.GetInt("CustomBallSpeed" + customBalls, 55);
                gameManager.chooseBalls[customBalls].spin = PlayerPrefs.GetInt("CustomBallSpin" + customBalls, 50);
                gameManager.chooseBalls[customBalls].urlTextureBall = PlayerPrefs.GetString("CustomBallURL" + customBalls);
                StartCoroutine(gameManager.DownloadTexture(PlayerPrefs.GetString("CustomBallURL" + customBalls), gameManager.chooseBalls[customBalls].ballMat));
            }
        }

        ball = GameObject.FindObjectOfType<BallHundredPin>();
        pins = GameObject.FindGameObjectsWithTag("Pin");
        crowd = GameObject.FindObjectOfType<Crowd>();
        replays = GameObject.FindObjectsOfType<ActionReplayHundredPin>();
        music.clip = Resources.Load<AudioClip>("Music/bonusgame");
        GameManager.type = GameManager.GameState.Intro;
        chooseBallUI.SetActive(false);
        powerUpUI.SetActive(false);
        ball.Reset();
        music.Play();
        cameraFollow.transform.position = new Vector3(0, 506.8f, -1004.7f);
        cameraFollow.transform.eulerAngles = new Vector3(10.2f, 180, 0);
        camType = CameraType.Intro;
        StartCoroutine(IntroTime());
        playerName.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < maxTimer && isWaitPin)
        {
            timer += Time.deltaTime;
        }
        else if (timer >= maxTimer && isWaitPin)
        {
            b = StartCoroutine(PinTimeB());
            isWaitPin = false;
        }
        moneyText.text = "$" + GameManager.moneys;
        bombBallText.text = bombShopText.text = GameManager.bombBalls + "x";
        cloneBallText.text = cloneShopText.text = cloneBallTimesText.GetComponent<Text>().text = GameManager.cloneBalls + "x";
        forcePulseBallText.text = forcePulseShopText.text = GameManager.forcePulseBalls + "x";
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
        if (GameManager.cloneBalls <= 0)
        {
            cloneBallButton.interactable = false;
        }
        else
        {
            cloneBallButton.interactable = true;
        }
        if (GameManager.forcePulseBalls <= 0)
        {
            forcePulseBallButton.interactable = false;
        }
        else
        {
            forcePulseBallButton.interactable = true;
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
        replays = GameObject.FindObjectsOfType<ActionReplayHundredPin>();
        sounds = GameObject.FindGameObjectsWithTag("Sound");
        music.mute = !GameManager.isMusic;
        foreach (GameObject sound in sounds)
        {
            if (GameManager.type != GameManager.GameState.Menu)
            {
                sound.GetComponent<AudioSource>().mute = !GameManager.isSound;
            }
            else if (GameManager.type == GameManager.GameState.Menu)
            {
                sound.GetComponent<AudioSource>().mute = true;
            }
        }
        winGameName.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
        winGameScore.text = "score: " + score;
        winGameHighScore.text = "high score: " + PlayerPrefs.GetInt("100PinHighScore", 0);
        if (Input.GetMouseButtonDown(0) && GameManager.type == GameManager.GameState.Intro && isIntro)
        {
            StopCoroutine(IntroTime());
            if (isIntro)
            {
                chooseBallUI.SetActive(true);
                powerUpUI.SetActive(true);
                playerName.characterSize = 0;
                GameManager.type = GameManager.GameState.Game;
                camType = CameraType.MoveX;
                ball.ResetCam();
                isIntro = false;
            }
        }
        if (scores.Count <= 0)
        {
            score = 0;
        }
        else if (scores.Count > 0)
        {
            score = scores[scores.Count - 1];
        }
        scoreTextBall.text = "score:" + score;
        GameManager.chooseBallIndex = GameManager.turnBalls1;
        playerNameText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
        if (GameManager.chooseBallIndex < gameManager.chooseBalls.Length && !isEndGame)
        {
            ball.ChargeBall(gameManager.chooseBalls[GameManager.chooseBallIndex].ballMat, gameManager.chooseBalls[GameManager.chooseBallIndex].lbs, gameManager.chooseBalls[GameManager.chooseBallIndex].speed, gameManager.chooseBalls[GameManager.chooseBallIndex].spin);
        }
        if (GameManager.chooseBallIndex == 47)
        {
            ballCloudRender.SetActive(true);
            ball.earthCloudBall.SetActive(true);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.chooseBallIndex == 50)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(true);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.chooseBallIndex == 51)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(true);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.chooseBallIndex == 54)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(true);
        }
        else
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        if (gameManager.chooseBalls[GameManager.turnBalls1].isLock != 1)
        {
            ballRender.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
            if (GameManager.turnBalls1 == 47)
            {
                ballCloudRender.SetActive(true);
            }
            else
            {
                ballCloudRender.SetActive(false);
            }
        }
        ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
        ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
        pinCounter.UpdateStandingCountAndSettle();
        if (replayTime < 15f && isReplayRecord)
        {
            replayTime += Time.deltaTime;
        }
        else if (replayTime >= 15f && isReplayRecord)
        {
            replayTime = 15f;
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
        if (!isReplayRecord && !isCurrentReplay && !isWaitPin)
        {
            currentReplayIndex = 0;
            replayTime = 0;
            timer = 0;
            maxTimer = 0;
            for (int i = 0; i < replays.Length; i++)
            {
                replays[i].Clear();
            }
        }
    }

    public IEnumerator IntroTime()
    {
        yield return new WaitForSeconds(6);
        if (isIntro)
        {
            chooseBallUI.SetActive(true);
            powerUpUI.SetActive(true);
            playerName.characterSize = 0;
            GameManager.type = GameManager.GameState.Game;
            camType = CameraType.MoveX;
            isIntro = false;
        }
    }

    public void PinTimeA(float time)
    {
        if (time != 0)
        {
            maxTimer = timer + time;
        }
        isWaitPin = true;
        if (b != null)
        {
            StopCoroutine(b);
        }
        if (GameManager.type == GameManager.GameState.Replay)
        {
            GameManager.type = GameManager.GameState.Game;
        }
    }

    public IEnumerator PinTimeB()
    {
        ball.rollAudio.Stop();
        ball.gutterAudio.Stop();
        ball.pinAudio.Stop();
        ball.electricAudio.Stop();
        if (GameManager.type == GameManager.GameState.Game)
        {
            GameManager.type = GameManager.GameState.Replay;
        }
        GameObject.FindObjectOfType<HundredPinSetter>().DownPins();
        pinCounter.PinsHaveSettled();
        if (HundredPinCounter.pinCount == 0 && throwBall == 1)
        {
            CheerMed();
            if (pinCounts == 100)
            {
                strikes++;
                addCash += 100 * allStrikes;
                allStrikes++;
                allSpares = 1;
            }
        }
        if (HundredPinCounter.pinCount == 0 && throwBall == 2)
        {
            spares++;
            addCash += 50 * allSpares;
            allSpares++;
            allStrikes = 1;
        }
        if (HundredPinCounter.pinCount != 0 && throwBall == 2)
        {
            allStrikes = 1;
            allSpares = 1;
        }
        if (isReplay)
        {
            cameraFollow.transform.position = new Vector3(0, 300, -2000);
            cameraFollow.transform.eulerAngles = new Vector3(10.21f, 180, 0);
            foreach (MiniBall miniBall in GameObject.FindObjectsOfType<MiniBall>())
            {
                miniBall.DestroyBall();
            }
            if (GameManager.type == GameManager.GameState.Replay)
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
        timer = 0;
        maxTimer = 0;
        SkipReplay();
        isReplayRecord = false;
    }

    public void Bowl(int pinFall)
    {
        if (GameManager.type == GameManager.GameState.Replay)
        {
            try
            {
                rolls.Add(pinFall);
                GameObject.FindObjectOfType<HundredPinSetter>().PerformAction(ActionMasterOldHundredPin.NextAction(rolls));
            }
            catch
            {
                isEndGame = true;
            }

            try
            {
                scoreDisplay.FillRolls(rolls);
                scoreDisplay.FillFrames(ScoreMasterHundredPin.ScoreCumulative(rolls));
                scores = ScoreMasterHundredPin.ScoreCumulative(rolls);

            }
            catch
            {
                UnityEngine.Debug.LogWarning("FillRollCard failed");
            }
            pinCounts = pinFall;
        }
        if (pinCounts == 0 || pinCounts == 1 && HundredPinCounter.pinCount > 0)
        {
            Laugh();
        }
        if (pinCounts >= 2 && pinCounts <= 69 && HundredPinCounter.pinCount > 0)
        {
            CrowdCrap();
        }
        if (pinCounts >= 70 && pinCounts <= 99 && HundredPinCounter.pinCount > 0 || throwBall == 2 && HundredPinCounter.pinCount == 0)
        {
            CrowdOk();
        }
        if (pinCounts == 0 && HundredPinCounter.pinCount > 0 && GameManager.type == GameManager.GameState.Replay)
        {
            gutters++;
        }
    }

    public void CheerBig()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.CheerBig(crowdAudio);
        }
    }

    public void CheerMed()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.CheerMed(crowdAudio);
        }
    }

    public void CrowdOk()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.CrowdOk(crowdAudio);
        }
    }

    public void CrowdHohum()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.CrowdHohum(crowdAudio);
        }
    }

    public void CrowdCrap()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.CrowdCrap(crowdAudio);
        }
    }

    public void Laugh()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.Laugh(crowdAudio);
        }
    }

    public void Roll()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.Roll(rollCrowd);
        }
        if (crowd.roll.Length >= 1 && GameManager.type != GameManager.GameState.Menu)
        {
            crowdAudio.Stop();
        }
    }

    public void Oooh()
    {
        if (GameManager.isCrowd && GameManager.type != GameManager.GameState.Menu)
        {
            crowd.Oooh(crowdAudio);
        }
    }

    void SkipReplay()
    {
        replayText.SetActive(false);
        isReplay = false;
        isReplayRecord = false;
        isCurrentReplay = false;
        isPin = false;
        for (int i = 0; i < replays.Length; i++)
        {
            replays[i].RigidBodyCollider(true);
            replays[i].Clear();
        }
        currentReplayIndex = 0;
        replayTime = 0;
        timer = 0;
        maxTimer = 0;
        if (HundredPinCounter.pinCount != 0 && throwBall < 2 && !isResetPins)
        {
            GameObject.FindObjectOfType<HundredPinSetter>().OutOfPins();
        }
        else if (HundredPinCounter.pinCount == 0 || throwBall == 2 || isResetPins)
        {
            pinCounter.Reset();
            GameObject.FindObjectOfType<HundredPinSetter>().ResetPins();
            throwBall = 0;
            isResetPins = false;
        }
        foreach (var bounce in GameObject.FindObjectsOfType<TriggerSound>())
        {
            bounce.Reset();
        }
        foreach (MiniBall miniBall in GameObject.FindObjectsOfType<MiniBall>())
        {
            miniBall.DestroyBall();
        }
        if (isEndGame)
        {
            cameraFollow.transform.position = new Vector3(0, 300, -2000);
            cameraFollow.transform.eulerAngles = new Vector3(10.21f, 180, 0);
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
            GameManager.type = GameManager.GameState.EndGame;
            gameManager.bowler[GameManager.turnNameIndex1].playerStrikes += strikes;
            gameManager.bowler[GameManager.turnNameIndex1].playerSpares += spares;
            gameManager.bowler[GameManager.turnNameIndex1].playerGutters += gutters;
            winGameBall.SetActive(true);
            if (score <= 999)
            {
                winGamePrize.text = "prize: 0";
                gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 0;
                winCrowd.clip = Resources.Load<AudioClip>("Sound/Foo05");
                winCrowd.Play();
                fireworksMulti.Play();
            }
            else if (score >= 1000 && score <= 1499)
            {
                winGamePrize.text = "prize: 50";
                gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 50;
                fireworksMulti.Play();
            }
            else if (score >= 1500 && score <= 1999)
            {
                winGamePrize.text = "prize: 100";
                gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 100;
                winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                winCrowd.Play();
                fireworksMulti.Play();
            }
            else if (score >= 2000 && score <= 2499)
            {
                winGamePrize.text = "prize: 500";
                gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 500;
                winCrowd.clip = Resources.Load<AudioClip>("Sound/Yea05");
                winCrowd.Play();
                fireworksMulti.Play();
            }
            else if (score >= 2500 && score <= 2999)
            {
                winGamePrize.text = "prize: 1,000";
                gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 1000;
                winCrowd.clip = Resources.Load<AudioClip>("Sound/App01");
                winCrowd.Play();
                fireworksMulti.Play();
            }
            else if (score >= 3000)
            {
                winGamePrize.text = "prize: 5,000";
                gameManager.bowler[GameManager.turnNameIndex1].playerMoney += 5000;
                winCrowd.clip = Resources.Load<AudioClip>("Sound/end-perfect");
                winCrowd.Play();
                fireworksMulti.Play();
            }
            playerNameText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
            ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
            ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
            ballRender.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
            foreach (PlayerObj playerEarn in gameManager.bowler)
            {
                if (playerEarn.playerMoney >= gameManager.chooseBalls[GameManager.unlockBallEarn].totalLock && gameManager.chooseBalls[GameManager.unlockBallEarn].isLock == 1 && gameManager.chooseBalls[GameManager.unlockBallEarn].lockType == ChooseBall.LockType.Earn)
                {
                    unlockedText.SetActive(true);
                    unlockedBallEarn.SetActive(true);
                    PlayerPrefs.SetInt("SaveBalls" + GameManager.unlockBallEarn, 0);
                    if (GameManager.unlockBallEarn < 44)
                    {
                        PlayerPrefs.SetInt("SaveBallEarn", GameManager.unlockBallEarn += 5);
                    }
                    else if (GameManager.unlockBallEarn == 44)
                    {
                        PlayerPrefs.SetInt("SaveBallEarn", 89);
                    }
                }
            }
            if (score > PlayerPrefs.GetInt("100PinHighScore", 0))
            {
                PlayerPrefs.SetInt("100PinHighScore", score);
            }
            FileData.SaveToSAV<PlayerObj>(gameManager.bowler, "SaveBowler");
            GameManager.moneys += addCash;
            PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
        }
        else
        {
            if (GameManager.type != GameManager.GameState.Menu)
            {
                playerNameText.text = gameManager.bowler[GameManager.turnNameIndex1].playerName;
                ballNameText.text = gameManager.chooseBalls[GameManager.turnBalls1].ballName;
                ballDataText.text = gameManager.chooseBalls[GameManager.turnBalls1].lbs + "lbs.  speed:" + gameManager.chooseBalls[GameManager.turnBalls1].speed + "  spin:" + gameManager.chooseBalls[GameManager.turnBalls1].spin;
                ballRender.material = gameManager.chooseBalls[GameManager.turnBalls1].ballMat;
                if (GameManager.turnBalls1 == 47)
                {
                    ballCloudRender.SetActive(true);
                    ball.earthCloudBall.SetActive(true);
                    ball.saturnRingBall.SetActive(false);
                    ball.uranusRingBall.SetActive(false);
                    ball.sunBall.SetActive(false);
                }
                else if (GameManager.turnBalls1 == 50)
                {
                    ballCloudRender.SetActive(false);
                    ball.earthCloudBall.SetActive(false);
                    ball.saturnRingBall.SetActive(true);
                    ball.uranusRingBall.SetActive(false);
                    ball.sunBall.SetActive(false);
                }
                else if (GameManager.turnBalls1 == 51)
                {
                    ballCloudRender.SetActive(false);
                    ball.earthCloudBall.SetActive(false);
                    ball.saturnRingBall.SetActive(false);
                    ball.uranusRingBall.SetActive(true);
                    ball.sunBall.SetActive(false);
                }
                else if (GameManager.turnBalls1 == 54)
                {
                    ballCloudRender.SetActive(false);
                    ball.earthCloudBall.SetActive(false);
                    ball.saturnRingBall.SetActive(false);
                    ball.uranusRingBall.SetActive(false);
                    ball.sunBall.SetActive(true);
                }
                else
                {
                    ballCloudRender.SetActive(false);
                    ball.earthCloudBall.SetActive(false);
                    ball.saturnRingBall.SetActive(false);
                    ball.uranusRingBall.SetActive(false);
                    ball.sunBall.SetActive(false);
                }
                ball.ChargeBall(gameManager.chooseBalls[GameManager.turnBalls1].ballMat, gameManager.chooseBalls[GameManager.turnBalls1].lbs, gameManager.chooseBalls[GameManager.turnBalls1].speed, gameManager.chooseBalls[GameManager.turnBalls1].spin);
                chooseBallUI.SetActive(true);
                powerUpUI.SetActive(true);
            }
            if (GameManager.type == GameManager.GameState.Replay)
            {
                ball.Reset();
                ball.ResetCam();
                GameManager.type = GameManager.GameState.Game;
            }
        }
    }

    public void PlayClip(string clipName)
    {
        if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
        {
            sfx.PlayOneShot(Resources.Load<AudioClip>("Sound/" + clipName));
        }
    }

    public void MouseDown()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.type == GameManager.GameState.Replay)
        {
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
                    if (throwBall < 2)
                    {
                        replays[i].SetVelocity();
                    }
                }
            }
            SkipReplay();
        }
    }

    public void QuitMenu()
    {
        AudioStop();
        SceneManager.LoadScene("Main");
        GameManager.type = GameManager.GameState.Menu;
    }

    public void EndGame()
    {
        crowdAudio.Stop();
        rollCrowd.Stop();
        winCrowd.Stop();
        fireworksMulti.Stop();
        sfx.Stop();
        SceneManager.LoadScene("Main");
        GameManager.type = GameManager.GameState.Menu;
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

    public void ChargeBall(int chargeBalls)
    {
        ball.ChargeBall(gameManager.chooseBalls[chargeBalls].ballMat, gameManager.chooseBalls[chargeBalls].lbs, gameManager.chooseBalls[chargeBalls].speed, gameManager.chooseBalls[chargeBalls].spin);
    }

    public void RandomChargeBall()
    {
        chargeBallIndex = Random.Range(0, gameManager.chooseBalls.Length);
        if (chargeBallIndex == 47)
        {
            ball.earthCloudBall.SetActive(true);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (chargeBallIndex == 50)
        {
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(true);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (chargeBallIndex == 51)
        {
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(true);
            ball.sunBall.SetActive(false);
        }
        else if (chargeBallIndex == 54)
        {
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(true);
        }
        else
        {
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        ball.ChargeBall(gameManager.chooseBalls[chargeBallIndex].ballMat, gameManager.chooseBalls[chargeBallIndex].lbs, gameManager.chooseBalls[chargeBallIndex].speed, gameManager.chooseBalls[chargeBallIndex].spin);
    }

    public void PrevBalls()
    {
        GameManager.turnBalls1--;
        if (GameManager.turnBalls1 < 0)
        {
            GameManager.turnBalls1 = gameManager.chooseBalls.Length - 1;
        }
        for (int i = 0; i < gameManager.chooseBalls.Length; i++)
        {
            if (gameManager.chooseBalls[GameManager.turnBalls1].isLock == 1)
            {
                GameManager.turnBalls1 -= gameManager.chooseBalls[i].isLock;
            }
        }
        if (GameManager.turnBalls1 == 47)
        {
            ballCloudRender.SetActive(true);
            ball.earthCloudBall.SetActive(true);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.turnBalls1 == 50)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(true);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.turnBalls1 == 51)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(true);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.turnBalls1 == 54)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(true);
        }
        else
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
    }

    public void NextBalls()
    {
        GameManager.turnBalls1++;
        if (GameManager.turnBalls1 >= gameManager.chooseBalls.Length)
        {
            GameManager.turnBalls1 = 0;
        }
        for (int i = 0; i < gameManager.chooseBalls.Length; i++)
        {
            if (gameManager.chooseBalls[GameManager.turnBalls1].isLock == 1)
            {
                GameManager.turnBalls1 += gameManager.chooseBalls[i].isLock;
            }
        }
        if (GameManager.turnBalls1 == 47)
        {
            ballCloudRender.SetActive(true);
            ball.earthCloudBall.SetActive(true);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.turnBalls1 == 50)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(true);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.turnBalls1 == 51)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(true);
            ball.sunBall.SetActive(false);
        }
        else if (GameManager.turnBalls1 == 54)
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(true);
        }
        else
        {
            ballCloudRender.SetActive(false);
            ball.earthCloudBall.SetActive(false);
            ball.saturnRingBall.SetActive(false);
            ball.uranusRingBall.SetActive(false);
            ball.sunBall.SetActive(false);
        }
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

    public void AudioStop()
    {
        music.Stop();
        GameManager.isOpening = false;
        foreach (GameObject sound in sounds)
        {
            sound.GetComponent<AudioSource>().Stop();
        }
    }

    public void BuyBomb()
    {
        if (GameManager.moneys >= 1250)
        {
            GameManager.bombBalls++;
            PlayerPrefs.SetInt("SaveBomb", GameManager.bombBalls);
            GameManager.moneys -= 1250;
            PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
            PlayClip("buy_powerups");
        }
        else
        {
            PlayClip("not_powerups");
        }
    }

    public void BuyClone()
    {
        if (GameManager.moneys >= 1000)
        {
            GameManager.cloneBalls++;
            PlayerPrefs.SetInt("SaveClone", GameManager.cloneBalls);
            GameManager.moneys -= 1000;
            PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
            PlayClip("buy_powerups");
        }
        else
        {
            PlayClip("not_powerups");
        }
    }

    public void BuyForcePulse()
    {
        if (GameManager.moneys >= 750)
        {
            GameManager.forcePulseBalls++;
            PlayerPrefs.SetInt("SaveForcePulse", GameManager.forcePulseBalls);
            GameManager.moneys -= 750;
            PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
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
            PlayerPrefs.SetInt("SaveHyper", GameManager.hyperBalls);
            GameManager.moneys -= 500;
            PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
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
            PlayerPrefs.SetInt("SaveLightning", GameManager.lightningBalls);
            GameManager.moneys -= 250;
            PlayerPrefs.SetInt("SaveMoney", GameManager.moneys);
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
            GameObject pop = Instantiate(fireworks, new Vector3(Random.Range(-512, 512), Random.Range(-256, 256), 7500), Quaternion.identity) as GameObject;
            var main = pop.GetComponent<ParticleSystem>().main;
            main.startColor = pop.GetComponent<Fireworks>().colorFireworks[Random.Range(0, pop.GetComponent<Fireworks>().colorFireworks.Length)];
            yield return new WaitForSeconds(Random.Range(0.3f, 3f));
        }
    }
}
