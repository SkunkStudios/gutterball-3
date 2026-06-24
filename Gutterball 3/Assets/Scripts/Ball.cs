using UnityEngine;

public class Ball : MonoBehaviour
{
    [Range(8, 16)]
    public int lbs = 12;
    [Range(10, 100)]
    public int speed = 55;
    [Range(0, 100)]
    public int spin = 50;
    public AudioSource rollAudio;
    public AudioSource gutterAudio;
    public AudioSource pinAudio;
    public AudioSource electricAudio;
    public AudioSource splashAudio;
    public AudioClip[] tubSplashs;
    public Game game;
    public GameObject hit;
    public GameObject splash;
    public CameraFollow cameraFollow;
    public BoxCollider roll;
    public BoxCollider replay;
    public Camera spinUI;
    public bool isGutter = false;
    public bool isGutterAnimation = false;
    public bool isGutterAnimation2X = false;
    public GameObject controlArrow;
    public GameObject saturnRingBall;
    public GameObject uranusRingBall;
    public GameObject sunBall;
    public GameObject bombBall;
    public GameObject forcePulseBall;
    public GameObject hyperBall;
    public GameObject lightningBall;
    public GameObject sunParticle;
    public GameObject bombParticle;
    public ParticleSystem forcePulseParticle;
    public GameObject hyperParticle;
    public GameObject lightningParticle;
    public Renderer[] meshBalls;

    private Vector2 moveMouse;
    private Vector2 direction;
    private float fastSpeed;
    private float maxSpeed;
    private float moveZThrow;
    private bool isMoveY = false;
    private bool isThrow = false;
    private bool isNet = false;
    private bool isBackWall = false;
    private bool isSplash;
    private Vector3 spinStart;
    private Vector3 spinEnd;
    private Rigidbody rigidBody;
    private ConstantForce force;
    private Pin pin1;
    private Pin pin2;
    private Pin pin3;
    private Pin pin4;
    private Pin pin6;
    private Pin pin7;
    private Pin pin10;
    private bool isPinTarget = false;
    private int randomTarget;

    void Awake()
    {
        splashAudio = GameObject.Find("Splash").GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
    }

    // Use this for initialization
    void Start ()
    {
        rigidBody.mass = lbs;
        if (!GameManager.isParticle)
        {
            sunParticle.SetActive(false);
            bombParticle.SetActive(false);
            hyperParticle.SetActive(false);
            lightningParticle.SetActive(false);
        }
        if (GameManager.chooseAlleys != GameManager.Alley.Wacky)
        {
            isPinTarget = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (pin1 == null && pin2 == null && pin3 == null && pin4 == null && pin6 == null && pin7 == null && pin10 == null)
        {
            pin1 = GameObject.FindObjectOfType<PinSetter>().pin1.GetComponent<Pin>();
            pin2 = GameObject.FindObjectOfType<PinSetter>().pin2.GetComponent<Pin>();
            pin3 = GameObject.FindObjectOfType<PinSetter>().pin3.GetComponent<Pin>();
            pin4 = GameObject.FindObjectOfType<PinSetter>().pin4.GetComponent<Pin>();
            pin6 = GameObject.FindObjectOfType<PinSetter>().pin6.GetComponent<Pin>();
            pin7 = GameObject.FindObjectOfType<PinSetter>().pin7.GetComponent<Pin>();
            pin10 = GameObject.FindObjectOfType<PinSetter>().pin10.GetComponent<Pin>();
        }
        saturnRingBall.transform.eulerAngles = Vector3.zero;
        uranusRingBall.transform.eulerAngles = Vector3.zero;
        bombBall.transform.eulerAngles = Vector3.zero;
        forcePulseBall.transform.eulerAngles = Vector3.zero;
        hyperBall.transform.eulerAngles = Vector3.zero;
        lightningBall.transform.eulerAngles = Vector3.zero;
        if (game.powerUps == Game.BallPowerUps.Off)
        {
            bombBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.Bomb)
        {
            bombBall.SetActive(true);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.ForcePulse)
        {
            bombBall.SetActive(false);
            forcePulseBall.SetActive(true);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.Hyper)
        {
            bombBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(true);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.Lightning)
        {
            bombBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(true);
        }
        if (Game.type != Game.GameState.Menu)
        {
            spinStart = Input.mousePosition;
            spinEnd = spinUI.ScreenToWorldPoint(spinStart);

            direction = new Vector2(spinEnd.x, spinEnd.y);

            controlArrow.transform.up = -direction;

            controlArrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(controlArrow.transform.eulerAngles.z, 90, 270));
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer && !isMoveY)
        {
            moveMouse = direction;
            fastSpeed = direction.y;
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer && isMoveY)
        {
            if (fastSpeed > direction.y - 100)
            {
                fastSpeed = direction.y - 100;
            }
            else
            {
                fastSpeed += Time.deltaTime * 225;
            }
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && spinEnd.x <= -75 && spinEnd.x >= -150 && spinEnd.y > -125 && !game.isComputer && !isMoveY)
        {
            transform.Translate(16 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && spinEnd.x >= 75 && spinEnd.x <= 150 && spinEnd.y > -125 && !game.isComputer && !isMoveY)
        {
            transform.Translate(-16 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && spinEnd.x <= 150 && spinEnd.y > -125 && !game.isComputer && !isMoveY)
        {
            transform.Translate(32 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && spinEnd.x >= -150 && spinEnd.y > -125 && !game.isComputer && !isMoveY)
        {
            transform.Translate(-32 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -120, 120), transform.position.y, transform.position.z);
        }
        if (transform.position.z > moveZThrow && game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer)
        {
            moveZThrow = transform.position.z;
        }
        else if (transform.position.z < moveZThrow && game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer)
        {
            isThrow = true;
        }
        if (isMoveY && game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.FindObjectOfType<PinSetter>().ballPos.z + moveMouse.y - direction.y);
        }
        if (game.ballType == Game.BallType.SpinBall && Game.type == Game.GameState.Game && !game.isComputer)
        {
            force.force = new Vector3(-spinEnd.x * spin * 0.32f * Time.deltaTime, 0, 0);
            force.torque = new Vector3(0, 0, spinEnd.x * spin * 0.16f * Time.deltaTime);
        }
        if (isGutter)
        {
            rigidBody.AddForce(0, 0, -rigidBody.mass * 320);
        }
        if (game.ballType == Game.BallType.SpinBall && game.isComputer && isPinTarget || game.ballType == Game.BallType.SpinBall && Game.type == Game.GameState.Menu && isPinTarget)
        {
            if (pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == true && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin1.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin1.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin1.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin1.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin1.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin1.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
            else if (pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 0 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true && randomTarget == 0 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false && randomTarget == 0 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 0 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false && randomTarget == 0)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin2.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin2.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin2.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin2.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin2.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin2.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
            else if (pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 1 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true && randomTarget == 1 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false && randomTarget == 1 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 1 || pin1.IsStanding() == false && pin2.IsStanding() == true && pin3.IsStanding() == true && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false && randomTarget == 1)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin3.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin3.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin3.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin3.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin3.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin3.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
            else if (pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 0 || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false && randomTarget == 0)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin4.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin4.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin4.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin4.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin4.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin4.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
            else if (pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 1 || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == true && pin6.IsStanding() == true && pin7.IsStanding() == false && pin10.IsStanding() == false && randomTarget == 1)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin6.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin6.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin6.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin6.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin6.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin6.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
            else if (pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == false || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 0)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin7.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin7.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin7.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin7.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin7.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin7.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
            else if (pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == false && pin10.IsStanding() == true || pin1.IsStanding() == false && pin2.IsStanding() == false && pin3.IsStanding() == false && pin4.IsStanding() == false && pin6.IsStanding() == false && pin7.IsStanding() == true && pin10.IsStanding() == true && randomTarget == 1)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin10.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin10.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.75f * Time.deltaTime);
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin10.transform.position.x - transform.position.x * spin * rigidBody.mass * 1.0f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin10.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime);
                }
                else if (spin >= 50)
                {
                    rigidBody.AddForce(-GameObject.FindObjectOfType<PinSetter>().pin10.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.5f * Time.deltaTime, 0, 0);
                    rigidBody.AddTorque(0, 0, GameObject.FindObjectOfType<PinSetter>().pin10.transform.position.x - transform.position.x * spin * rigidBody.mass * 0.25f * Time.deltaTime);
                }
            }
        }
        maxSpeed = fastSpeed - direction.y;
    }

    private void FixedUpdate()
    {
        transform.Rotate(rigidBody.angularVelocity / 1.25f, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders;
        if (collision.gameObject.tag == "Lane" && game.ballType == Game.BallType.ThrowBall && !game.isComputer && Game.type == Game.GameState.Game)
        {
            controlArrow.SetActive(true);
        }
        if (collision.gameObject.tag == "Lane" && game.ballType == Game.BallType.ThrowBall)
        {
            rollAudio.Play();
            gutterAudio.Stop();
            isGutter = false;
            game.isPin = false;
            roll.enabled = true;
            replay.enabled = true;
            game.ballType = Game.BallType.SpinBall;
            game.camType = Game.CameraType.FollowBall;
            if (GameObject.FindObjectOfType<PinSetter>().gutter != null)
            {
                GameObject.FindObjectOfType<PinSetter>().gutter.enabled = true;
            }
        }
        if (collision.gameObject.tag == "Pin" && collision.relativeVelocity.magnitude > rigidBody.mass * 10 && !game.isPin && !isBackWall && game.ballType == Game.BallType.SpinBall)
        {
            rollAudio.Stop();
            gutterAudio.Stop();
            electricAudio.Stop();
            if (game.powerUps == Game.BallPowerUps.Bomb || game.powerUps == Game.BallPowerUps.Lightning)
            {
                game.PlayClip("s.explosion_tnt");
            }
            isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
            game.isReplay = true;
            controlArrow.SetActive(false);
            if (GameManager.isParticle)
            {
                Instantiate(hit, collision.contacts[0].point, Quaternion.identity);
            }
            foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
            {
                pin.FallPinDown();
            }
            if (game.powerUps == Game.BallPowerUps.Bomb)
            {
                colliders = Physics.OverlapSphere(transform.position, 128);
                if (GameManager.isParticle)
                {
                    Instantiate(game.explores[Random.Range(0, game.explores.Length)], transform.position, Quaternion.identity);
                    if (GameManager.isSound && Game.type != Game.GameState.Menu)
                    {
                        splashAudio.PlayOneShot(game.exploreClips[Random.Range(0, game.exploreClips.Length)]);
                    }
                }
                if (Game.type != Game.GameState.Menu)
                {
                    game.rollCrowd.Stop();
                }
                GameObject.FindObjectOfType<CameraShake>().Shake(20);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(9000, transform.position, 12000, 90);
                    }
                }
                transform.position = new Vector3(0, -5000, -5000);
                GameManager.bombBalls--;
            }
            else if (game.powerUps == Game.BallPowerUps.ForcePulse)
            {
                colliders = Physics.OverlapSphere(transform.position, 64);
                if (GameManager.isParticle)
                {
                    forcePulseParticle.Play();
                }
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(6000, transform.position, 9000, 0);
                    }
                }
                GameManager.forcePulseBalls--;
            }
            else if (game.powerUps == Game.BallPowerUps.Hyper)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(3000, transform.position, 6000, 0);
                    }
                }
                GameManager.hyperBalls--;
            }
            else if (game.powerUps == Game.BallPowerUps.Lightning)
            {
                colliders = Physics.OverlapSphere(transform.position, 48);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(1500, transform.position, 3000, 0);
                    }
                }
                GameManager.lightningBalls--;
            }
            if (PinCounter.pinCount == 1 || collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(1);
                pinAudio.clip = game.bowling1;
            }
            else if (PinCounter.pinCount == 2 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 3 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(2);
                pinAudio.clip = game.bowling2;
            }
            else if (PinCounter.pinCount == 4 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 5 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(4);
                pinAudio.clip = game.bowling4;
            }
            else if (PinCounter.pinCount == 6 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 7 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 8 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(10);
                pinAudio.clip = game.bowling10;
            }
            else if (PinCounter.pinCount == 9 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 10 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(6);
                pinAudio.clip = game.bowling6;
            }
            pinAudio.Play();
            if (GameObject.FindObjectOfType<PinSetter>().isGravity)
            {
                force.force = new Vector3(0, 0, 0);
                force.torque = new Vector3(0, 0, 0);
            }
            else
            {
                rigidBody.useGravity = false;
                force.force = new Vector3(0, 0, -rigidBody.mass * 100);
                force.torque = new Vector3(0, 0, 0);
            }
            game.isPin = true;
            game.ballType = Game.BallType.FallBall;
            cameraFollow.FallMove(transform.position);
            game.camType = Game.CameraType.MoveCam;
            GameObject.FindObjectOfType<PinSetter>().StopScooper();
            game.PinTimeA(6);
        }
        else if (collision.gameObject.tag == "Pin" && collision.relativeVelocity.magnitude > rigidBody.mass * 10 && game.isPin)
        {
            foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
            {
                pin.FallPinDown();
            }
            if (game.powerUps == Game.BallPowerUps.ForcePulse)
            {
                colliders = Physics.OverlapSphere(transform.position, 64);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(6000, transform.position, 9000, 0);
                    }
                }
            }
            else if (game.powerUps == Game.BallPowerUps.Hyper)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(3000, transform.position, 6000, 0);
                    }
                }
            }
            else if (game.powerUps == Game.BallPowerUps.Lightning)
            {
                colliders = Physics.OverlapSphere(transform.position, 48);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(1500, transform.position, 3000, 0);
                    }
                }
            }
            if (PinCounter.pinCount == 1 || collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(1);
            }
            else if (PinCounter.pinCount == 2 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 3 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(2);
            }
            else if (PinCounter.pinCount == 4 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 5 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(4);
            }
            else if (PinCounter.pinCount == 6 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 7 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 8 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(10);
            }
            else if (PinCounter.pinCount == 9 && !collision.gameObject.GetComponent<Pin>().isHitOne || PinCounter.pinCount == 10 && !collision.gameObject.GetComponent<Pin>().isHitOne)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(6);
            }
        }

        if (collision.gameObject.tag == "Backwall" && collision.relativeVelocity.magnitude > rigidBody.mass * 7.5f && !isBackWall)
        {
            rollAudio.Stop();
            gutterAudio.Stop();
            game.PlayClip("Net");
            isBackWall = true;
            if (!isNet)
            {
                if (collision.relativeVelocity.magnitude > rigidBody.mass * 30)
                {
                    game.isReplay = true;
                }
                if (Game.type == Game.GameState.Replay)
                {
                    GameObject.FindObjectOfType<PinSetter>().LandPins();
                    game.isReplay = false;
                    game.isReplayRecord = false;
                }
                if (Game.type != Game.GameState.Menu)
                {
                    game.rollCrowd.Stop();
                }
                controlArrow.SetActive(false);
                isNet = true;
                game.StopScooper();
                game.ballType = Game.BallType.FallBall;
                GameObject.FindObjectOfType<PinSetter>().SkipScooper();
                GameObject.FindObjectOfType<PinSetter>().StopScooper();
                if (!game.isPin)
                {
                    cameraFollow.FallMove(transform.position);
                    game.camType = Game.CameraType.MoveCam;
                    game.PinTimeA(6);
                    game.isPin = true;
                }
                else
                {
                    game.PinTimeA(6);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 splashPosition = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        if (other.CompareTag("Roll"))
        {
            game.Roll();
            roll.enabled = false;
        }
        if (other.CompareTag("Replay"))
        {
            replay.enabled = false;
            game.isReplayRecord = true;
        }
        if (other.CompareTag("Fall") && isSplash && game.ballType == Game.BallType.ThrowBall || other.CompareTag("Fall") && isSplash && game.ballType == Game.BallType.SpinBall || other.CompareTag("Fall") && isSplash && game.ballType == Game.BallType.FallBall || other.CompareTag("Gutter") && isSplash && game.ballType == Game.BallType.SpinBall && !game.isPin || other.CompareTag("Water") && isSplash && game.ballType == Game.BallType.SpinBall || other.CompareTag("Water") && isSplash && game.ballType == Game.BallType.FallBall)
        {
            if (GameManager.isSound && Game.type != Game.GameState.Menu)
            {
                splashAudio.PlayOneShot(tubSplashs[Random.Range(0, tubSplashs.Length)]);
            }
            if (GameManager.isParticle)
            {
                Instantiate(splash, splashPosition, Quaternion.identity);
            }
            isSplash = false;
        }
        if (other.CompareTag("Fall") && game.ballType == Game.BallType.ThrowBall || other.CompareTag("Fall") && game.ballType == Game.BallType.SpinBall || other.CompareTag("Fall") && game.ballType == Game.BallType.FallBall && isGutter)
        {
            if (PinCounter.pinCount == 0)
            {
                game.isReplay = true;
            }
            rollAudio.Stop();
            gutterAudio.Stop();
            if (!game.isComputer && game.gutterAnimation == 0 && Game.type != Game.GameState.Menu)
            {
                game.rollCrowd.Stop();
            }
            controlArrow.SetActive(false);
            if (GameObject.FindObjectOfType<PinSetter>().isGravity)
            {
                force.force = new Vector3(0, 0, 0);
                force.torque = new Vector3(0, 0, 0);
            }
            else
            {
                rigidBody.useGravity = false;
                force.force = new Vector3(0, 0, -rigidBody.mass * 100);
                force.torque = new Vector3(0, 0, 0);
            }
            if (transform.position.z >= -3200 || game.isComputer && game.gutterAnimation == 0 || Game.type == Game.GameState.Menu && game.gutterAnimation == 0)
            {
                if (game.ballType == Game.BallType.SpinBall)
                {
                    game.VoiceGutterball();
                }
                if (game.gutterAnimation == 0)
                {
                    isGutter = false;
                    game.gutterAnimation = 1;
                }
                else
                {
                    isGutter = false;
                    game.gutterAnimation = 2;
                }
                isNet = true;
            }
            roll.enabled = false;
            game.isReplayRecord = false;
            if (GameObject.FindObjectOfType<PinSetter>().gutter != null)
            {
                GameObject.FindObjectOfType<PinSetter>().gutter.enabled = false;
            }
            game.ballType = Game.BallType.FallBall;
            game.PinTimeA(0);
        }
        if (other.CompareTag("Gutter") && !game.isPin)
        {
            rollAudio.Stop();
            controlArrow.SetActive(false);
            force.force = new Vector3(0, 0, 0);
            force.torque = new Vector3(0, 0, 0);
            game.CrowdStop();
            if (game.ballType == Game.BallType.ThrowBall)
            {
                game.GutterLaugh();
            }
            game.VoiceGutterball();
            if (game.gutterAnimation == 0)
            {
                gutterAudio.Play();
                game.rollCrowd.Stop();
                isGutter = true;
                game.gutterAnimation = 1;
                game.isPin = true;
                roll.enabled = false;
                game.isReplayRecord = false;
            }
            else if (game.gutterAnimation == 1)
            {
                gutterAudio.Stop();
                isGutter = false;
                game.gutterAnimation = 2;
                game.isPin = true;
                roll.enabled = false;
                game.isReplayRecord = false;
                game.PinTimeA(0);
            }
            isNet = true;
            cameraFollow.FallMove(transform.position);
            game.camType = Game.CameraType.MoveCam;
            if (GameObject.FindObjectOfType<PinSetter>().gutter != null)
            {
                GameObject.FindObjectOfType<PinSetter>().gutter.enabled = false;
            }
            if (game.ballType == Game.BallType.SpinBall)
            {
                game.ballType = Game.BallType.FallBall;
            }
        }
    }

    public void MouseDown()
    {
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer && Input.GetMouseButtonDown(0))
        {
            isMoveY = true;
        }
        if (game.ballType == Game.BallType.SpinBall && Game.type == Game.GameState.Game && !game.isComputer && Input.GetMouseButtonDown(0))
        {
            if (game.powerUps == Game.BallPowerUps.Hyper)
            {
                rigidBody.AddForce(0, 0, -rigidBody.mass * 5000);
            }
            else
            {
                rigidBody.AddForce(0, 0, -rigidBody.mass * 2500);
            }
        }
    }

    public void MouseUp()
    {
        if (game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer && Input.GetMouseButtonUp(0))
        {
            if (isThrow)
            {
                Bowl();
            }
            else
            {
                isMoveY = false;
                transform.position = GameObject.FindObjectOfType<PinSetter>().ballPos;
                moveZThrow = GameObject.FindObjectOfType<PinSetter>().ballPos.z;
            }
        }
    }

    public void MoveX(float move)
    {
        transform.Translate(-move, 0, 0);
    }

    public void Bowl()
    {
        game.chooseBallUI.SetActive(false);
        game.powerUpUI.SetActive(false);
        rigidBody.isKinematic = false;
        game.VoiceStop();
        game.CrowdStop();
        if (game.powerUps == Game.BallPowerUps.Hyper)
        {
            rigidBody.AddForce(moveMouse.x - direction.x * spin * rigidBody.mass * 0.375f, 0, maxSpeed * speed * 50f);
        }
        else
        {
            rigidBody.AddForce(moveMouse.x - direction.x * spin * rigidBody.mass * 0.375f, 0, maxSpeed * speed * 25f);
        }
        game.PlayClip("Thumbpop");
        game.ballType = Game.BallType.ThrowBall;
        game.camType = Game.CameraType.DropBall;
        game.throwBall++;
    }

    public void BallReturn()
    {
        isGutter = false;
        rigidBody.useGravity = true;
        force.force = new Vector3(0, 0, 0);
        force.torque = new Vector3(0, 0, 0);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        if (GameObject.FindObjectOfType<PinSetter>().returnPoint != null)
        {
            game.powerUps = Game.BallPowerUps.Off;
            transform.position = GameObject.FindObjectOfType<PinSetter>().returnPoint.position;
            rigidBody.velocity = Vector3.forward * 200;
        }
    }

    public void Reset()
    {
        rigidBody.useGravity = true;
        force.force = new Vector3(0, 0, 0);
        force.torque = new Vector3(0, 0, 0);
        rigidBody.isKinematic = true;
        transform.position = GameObject.FindObjectOfType<PinSetter>().ballPos;
        transform.rotation = Quaternion.Euler(45, 0, 0);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        moveZThrow = GameObject.FindObjectOfType<PinSetter>().ballPos.z;
        isMoveY = false;
        isThrow = false;
        isGutter = false;
        isNet = false;
        isBackWall = false;
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
        game.isReplayRecord = false;
        game.powerUps = Game.BallPowerUps.Off;
        game.ballType = Game.BallType.MoveX;
        game.camType = Game.CameraType.MoveX;
        if (GameObject.FindObjectOfType<PinSetter>().gutter != null)
        {
            GameObject.FindObjectOfType<PinSetter>().gutter.enabled = true;
        }
    }

    public void ResetBowl()
    {
        randomTarget = Random.Range(0, 2);
        rigidBody.useGravity = true;
        force.force = new Vector3(0, 0, 0);
        force.torque = new Vector3(0, 0, 0);
        rigidBody.isKinematic = false;
        transform.position = new Vector3(Random.Range(-60, 60), GameObject.FindObjectOfType<PinSetter>().ballPos.y, GameObject.FindObjectOfType<PinSetter>().ballPos.z);
        transform.rotation = Quaternion.Euler(45, 0, 0);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        isMoveY = false;
        isThrow = false;
        isGutter = false;
        isNet = false;
        isBackWall = false;
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
        game.isReplayRecord = false;
        rigidBody.AddForce(-Vector3.forward * 366250);
        if (isPinTarget)
        {
            if (transform.position.x > 0)
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-100f, -12.5f));
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-75f, -12.5f));
                }
                else if (spin >= 50 && spin < 75)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-50f, -12.5f));
                }
                else if (spin >= 75)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-25f, -12.5f));
                }
            }
            else
            {
                if (spin < 25)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(12.5f, 100f));
                }
                else if (spin >= 25 && spin < 50)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(12.5f, 75f));
                }
                else if (spin >= 50 && spin < 75)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(12.5f, 50f));
                }
                else if (spin >= 75)
                {
                    rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(12.5f, 25f));
                }
            }
        }
        else
        {
            if (spin < 25)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-100f, 100f));
            }
            else if (spin >= 25 && spin < 50)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-75f, 75f));
            }
            else if (spin >= 50 && spin < 75)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-50f, 50f));
            }
            else if (spin >= 75)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-25f, 25f));
            }
        }
        game.PlayClip("Thumbpop");
        game.powerUps = Game.BallPowerUps.Off;
        game.ballType = Game.BallType.ThrowBall;
        game.camType = Game.CameraType.DropBall;
        if (GameObject.FindObjectOfType<PinSetter>().gutter != null)
        {
            GameObject.FindObjectOfType<PinSetter>().gutter.enabled = true;
        }
        game.throwBall++;
    }

    public void ResetCam()
    {
        cameraFollow.Reset();
    }

    public void ChargeBall(Material ballMat, int chargeLbs, int chargeSpeed, int chargeSpin)
    {
        foreach(Renderer meshBall in meshBalls)
        {
            meshBall.material = ballMat;
        }
        lbs = chargeLbs;
        speed = chargeSpeed;
        spin = chargeSpin;
        rigidBody.mass = lbs;
    }

    public void BombBall()
    {
        if (game.powerUps == Game.BallPowerUps.Bomb)
        {
            game.powerUps = Game.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = Game.BallPowerUps.Bomb;
        }
    }

    public void ForcePulseBall()
    {
        if (game.powerUps == Game.BallPowerUps.ForcePulse)
        {
            game.powerUps = Game.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = Game.BallPowerUps.ForcePulse;
        }
    }

    public void HyperBall()
    {
        if (game.powerUps == Game.BallPowerUps.Hyper)
        {
            game.powerUps = Game.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = Game.BallPowerUps.Hyper;
        }
    }

    public void LightningBall()
    {
        if (game.powerUps == Game.BallPowerUps.Lightning)
        {
            game.powerUps = Game.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = Game.BallPowerUps.Lightning;
            if (GameManager.isSound && Game.type != Game.GameState.Menu)
            {
                game.PlayClip("thunder");
            }
            game.thunderAnimation.Play();
        }
    }
}
