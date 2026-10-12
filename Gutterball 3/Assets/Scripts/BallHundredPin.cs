using UnityEngine;

public class BallHundredPin : MonoBehaviour
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
    public GameHundredPin game;
    public GameObject hit;
    public GameObject splash;
    public CameraFollowHundredPin cameraFollow;
    public BoxCollider roll;
    public BoxCollider replay;
    public Camera spinUI;
    public GameObject controlArrow;
    public GameObject earthCloudBall;
    public GameObject saturnRingBall;
    public GameObject uranusRingBall;
    public GameObject sunBall;
    public GameObject bombBall;
    public GameObject cloneBall;
    public GameObject forcePulseBall;
    public GameObject hyperBall;
    public GameObject lightningBall;
    public GameObject sunParticle;
    public GameObject bombParticle;
    public ParticleSystem forcePulseParticle;
    public GameObject hyperParticle;
    public GameObject lightningParticle;
    public Renderer[] meshBalls;
    public GameObject spawnBall;
    public GameObject spawnBallPoof;

    private Vector2 moveMouse;
    private Vector2 direction;
    private float fastSpeed;
    private float maxSpeed;
    private float moveZThrow;
    private bool isMoveY = false;
    private bool isThrow = false;
    private bool isSplash;
    private Vector3 spinStart;
    private Vector3 spinEnd;
    private Rigidbody rigidBody;
    private ConstantForce force;

    void Awake()
    {
        splashAudio = GameObject.Find("Splash").GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
    }

    // Use this for initialization
    void Start()
    {
        rigidBody.mass = lbs;
        if (!GameManager.isParticle)
        {
            sunParticle.SetActive(false);
            bombParticle.SetActive(false);
            hyperParticle.SetActive(false);
            lightningParticle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (game.powerUps == GameHundredPin.BallPowerUps.Off)
        {
            bombBall.SetActive(false);
            cloneBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == GameHundredPin.BallPowerUps.Bomb)
        {
            bombBall.SetActive(true);
            cloneBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == GameHundredPin.BallPowerUps.Clone)
        {
            bombBall.SetActive(false);
            cloneBall.SetActive(true);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == GameHundredPin.BallPowerUps.ForcePulse)
        {
            bombBall.SetActive(false);
            cloneBall.SetActive(false);
            forcePulseBall.SetActive(true);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == GameHundredPin.BallPowerUps.Hyper)
        {
            bombBall.SetActive(false);
            cloneBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(true);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == GameHundredPin.BallPowerUps.Lightning)
        {
            bombBall.SetActive(false);
            cloneBall.SetActive(false);
            forcePulseBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(true);
        }
        if (GameManager.type != GameManager.GameState.Menu)
        {
            spinStart = Input.mousePosition;
            spinEnd = spinUI.ScreenToWorldPoint(spinStart);

            direction = new Vector2(spinEnd.x, spinEnd.y);

            controlArrow.transform.up = -direction;

            controlArrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(controlArrow.transform.eulerAngles.z, 90, 270));
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && !isMoveY)
        {
            moveMouse = direction;
            fastSpeed = direction.y;
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && isMoveY)
        {
            if (fastSpeed > direction.y - 150)
            {
                fastSpeed = direction.y - 150;
            }
            else
            {
                fastSpeed += Time.deltaTime * 1000 / 3;
            }
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && spinEnd.x <= -75 && spinEnd.x >= -150 && spinEnd.y > -125 && !isMoveY)
        {
            transform.Translate(16 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && spinEnd.x >= 75 && spinEnd.x <= 150 && spinEnd.y > -125 && !isMoveY)
        {
            transform.Translate(-16 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && spinEnd.x <= 150 && spinEnd.y > -125 && !isMoveY)
        {
            transform.Translate(32 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && spinEnd.x >= -150 && spinEnd.y > -125 && !isMoveY)
        {
            transform.Translate(-32 * Time.deltaTime, 0, 0, Space.World);
        }
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -240, 240), transform.position.y, transform.position.z);
        }
        if (transform.position.z > moveZThrow && game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game)
        {
            moveZThrow = transform.position.z;
        }
        else if (transform.position.z < moveZThrow - 100 / lbs && game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game)
        {
            isThrow = true;
        }
        if (isMoveY && game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.FindObjectOfType<HundredPinSetter>().ballPos.z + moveMouse.y - direction.y);
        }
        if (game.ballType == GameHundredPin.BallType.SpinBall && GameManager.type == GameManager.GameState.Game)
        {
            force.force = new Vector3(-spinEnd.x * spin / 3 * Time.deltaTime, 0, 0);
            force.torque = new Vector3(0, 0, spinEnd.x * spin / 6 * Time.deltaTime);
        }
        maxSpeed = fastSpeed - direction.y;
    }

    void FixedUpdate()
    {
        transform.Rotate(rigidBody.angularVelocity / 1.25f, Space.World);
    }

    void LateUpdate()
    {
        saturnRingBall.transform.eulerAngles = Vector3.zero;
        uranusRingBall.transform.eulerAngles = Vector3.zero;
        bombBall.transform.eulerAngles = Vector3.zero;
        cloneBall.transform.eulerAngles = Vector3.zero;
        forcePulseBall.transform.eulerAngles = Vector3.zero;
        hyperBall.transform.eulerAngles = Vector3.zero;
        lightningBall.transform.eulerAngles = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders;
        if (collision.gameObject.tag == "Lane" && game.ballType == GameHundredPin.BallType.ThrowBall && GameManager.type == GameManager.GameState.Game)
        {
            controlArrow.SetActive(true);
            if (game.powerUps == GameHundredPin.BallPowerUps.Clone)
            {
                game.cloneBallTimesText.SetActive(true);
            }
        }
        if (collision.gameObject.tag == "Lane" && game.ballType == GameHundredPin.BallType.ThrowBall)
        {
            rollAudio.Play();
            gutterAudio.Stop();
            game.isPin = false;
            roll.enabled = true;
            replay.enabled = true;
            game.ballType = GameHundredPin.BallType.SpinBall;
            game.camType = GameHundredPin.CameraType.FollowBall;
        }
        if (collision.gameObject.tag == "Pin" && collision.relativeVelocity.magnitude > 2000 / rigidBody.mass && !game.isPin && game.ballType == GameHundredPin.BallType.SpinBall)
        {
            rollAudio.Stop();
            gutterAudio.Stop();
            electricAudio.Stop();
            if (game.powerUps == GameHundredPin.BallPowerUps.Bomb || game.powerUps == GameHundredPin.BallPowerUps.Lightning)
            {
                game.PlayClip("s.explosion_tnt");
            }
            isSplash = true;
            game.isReplay = true;
            controlArrow.SetActive(false);
            game.cloneBallTimesText.SetActive(false);
            if (GameManager.isParticle)
            {
                Instantiate(hit, collision.contacts[0].point, Quaternion.identity);
            }
            foreach (HundredPin pin in GameObject.FindObjectsOfType<HundredPin>())
            {
                pin.FallPinDown();
            }
            if (game.powerUps == GameHundredPin.BallPowerUps.Bomb)
            {
                colliders = Physics.OverlapSphere(transform.position, 160);
                if (GameManager.isParticle)
                {
                    Instantiate(game.explores[Random.Range(0, game.explores.Length)], transform.position, Quaternion.identity);
                    if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
                    {
                        splashAudio.PlayOneShot(game.exploreClips[Random.Range(0, game.exploreClips.Length)]);
                    }
                }
                if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
                {
                    game.rollCrowd.Stop();
                }
                GameObject.FindObjectOfType<CameraShake>().Shake(20);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(5000, transform.position, 10000, 90);
                    }
                }
                transform.position = new Vector3(0, -5000, -5000);
                GameManager.bombBalls--;
                PlayerPrefs.SetInt("SaveBomb", GameManager.bombBalls);
            }
            else if (game.powerUps == GameHundredPin.BallPowerUps.ForcePulse)
            {
                colliders = Physics.OverlapSphere(transform.position, 80);
                if (GameManager.isParticle)
                {
                    forcePulseParticle.Play();
                }
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(3750, transform.position, 7500, 0);
                    }
                }
                GameManager.forcePulseBalls--;
                PlayerPrefs.SetInt("SaveForcePulse", GameManager.forcePulseBalls);
            }
            else if (game.powerUps == GameHundredPin.BallPowerUps.Hyper)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(2500, transform.position, 5000, 0);
                    }
                }
                GameManager.hyperBalls--;
                PlayerPrefs.SetInt("SaveHyper", GameManager.hyperBalls);
            }
            else if (game.powerUps == GameHundredPin.BallPowerUps.Lightning)
            {
                colliders = Physics.OverlapSphere(transform.position, 48);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(1250, transform.position, 2500, 0);
                    }
                }
                GameManager.lightningBalls--;
                PlayerPrefs.SetInt("SaveLightning", GameManager.lightningBalls);
            }
            if (HundredPinCounter.pinCount == 1)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(1);
                pinAudio.clip = game.bowling1;
            }
            else if (HundredPinCounter.pinCount >= 2 && HundredPinCounter.pinCount <= 39)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(2);
                pinAudio.clip = game.bowling2;
            }
            else if (HundredPinCounter.pinCount >= 40 && HundredPinCounter.pinCount <= 59)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(4);
                pinAudio.clip = game.bowling4;
            }
            else if (HundredPinCounter.pinCount >= 60 && HundredPinCounter.pinCount <= 89)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(10);
                pinAudio.clip = game.bowling10;
            }
            else if (HundredPinCounter.pinCount >= 90)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(6);
                pinAudio.clip = game.bowling6;
            }
            pinAudio.Play();
            force.force = new Vector3(0, 0, 0);
            force.torque = new Vector3(0, 0, 0);
            game.isPin = true;
            game.ballType = GameHundredPin.BallType.FallBall;
            cameraFollow.FallMove(transform.position);
            game.camType = GameHundredPin.CameraType.MoveCam;
            game.PinTimeA(6);
        }
        else if (collision.gameObject.tag == "Pin" && collision.relativeVelocity.magnitude > 2000 / rigidBody.mass && game.isPin && game.camType == GameHundredPin.CameraType.MoveCam)
        {
            foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
            {
                pin.FallPinDown();
            }
            if (game.powerUps == GameHundredPin.BallPowerUps.Hyper)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(2500, transform.position, 5000, 0);
                    }
                }
            }
            else if (game.powerUps == GameHundredPin.BallPowerUps.Lightning)
            {
                colliders = Physics.OverlapSphere(transform.position, 48);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(1250, transform.position, 2500, 0);
                    }
                }
            }
            if (HundredPinCounter.pinCount == 1)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(1);
            }
            else if (HundredPinCounter.pinCount >= 2 && HundredPinCounter.pinCount <= 39)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(2);
            }
            else if (HundredPinCounter.pinCount >= 40 && HundredPinCounter.pinCount <= 59)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(4);
            }
            else if (HundredPinCounter.pinCount >= 60 && HundredPinCounter.pinCount <= 89)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(10);
            }
            else if (HundredPinCounter.pinCount >= 90)
            {
                GameObject.FindObjectOfType<CameraShake>().Shake(6);
            }
            game.PinTimeA(6);
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
        if (other.CompareTag("Fall") && isSplash && game.ballType == GameHundredPin.BallType.ThrowBall || other.CompareTag("Fall") && isSplash && game.ballType == GameHundredPin.BallType.SpinBall || other.CompareTag("Fall") && isSplash && game.ballType == GameHundredPin.BallType.FallBall || other.CompareTag("Gutter") && isSplash && game.ballType == GameHundredPin.BallType.SpinBall && !game.isPin || other.CompareTag("Water") && isSplash && game.ballType == GameHundredPin.BallType.SpinBall || other.CompareTag("Water") && isSplash && game.ballType == GameHundredPin.BallType.FallBall)
        {
            if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
            {
                splashAudio.PlayOneShot(tubSplashs[Random.Range(0, tubSplashs.Length)]);
            }
            if (GameManager.isParticle)
            {
                Instantiate(splash, splashPosition, Quaternion.identity);
            }
            isSplash = false;
        }
        if (other.CompareTag("Fall") && game.ballType == GameHundredPin.BallType.ThrowBall || other.CompareTag("Fall") && game.ballType == GameHundredPin.BallType.SpinBall || other.CompareTag("Fall") && game.ballType == GameHundredPin.BallType.FallBall && game.camType != GameHundredPin.CameraType.Anim)
        {
            if (PinCounter.pinCount == 0)
            {
                game.isReplay = true;
            }
            rollAudio.Stop();
            gutterAudio.Stop();
            controlArrow.SetActive(false);
            game.cloneBallTimesText.SetActive(false);
            force.force = new Vector3(0, 0, 0);
            force.torque = new Vector3(0, 0, 0);
            roll.enabled = false;
            game.ballType = GameHundredPin.BallType.FallBall;
            game.PinTimeA(0);
        }
    }

    public void MouseDown()
    {
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && Input.GetMouseButtonDown(0))
        {
            isMoveY = true;
        }
        if (game.ballType == GameHundredPin.BallType.SpinBall && GameManager.type == GameManager.GameState.Game && Input.GetMouseButtonDown(0))
        {
            if (game.powerUps == GameHundredPin.BallPowerUps.Hyper)
            {
                rigidBody.AddForce(0, 0, -rigidBody.mass * 20000 / 3);
            }
            else
            {
                rigidBody.AddForce(0, 0, -rigidBody.mass * 10000 / 3);
            }
        }
        if (Input.GetMouseButtonDown(1) && game.powerUps == GameHundredPin.BallPowerUps.Bomb && !game.isPin && game.ballType == GameHundredPin.BallType.SpinBall)
        {
            Collider[] colliders;
            rollAudio.Stop();
            gutterAudio.Stop();
            electricAudio.Stop();
            game.PlayClip("s.explosion_tnt");
            isSplash = true;
            if (game.isReplayRecord)
            {
                game.isReplay = true;
            }
            controlArrow.SetActive(false);
            foreach (HundredPin pin in GameObject.FindObjectsOfType<HundredPin>())
            {
                pin.FallPinDown();
            }
            colliders = Physics.OverlapSphere(transform.position, 160);
            if (GameManager.isParticle)
            {
                Instantiate(game.explores[Random.Range(0, game.explores.Length)], transform.position, Quaternion.identity);
                if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
                {
                    splashAudio.PlayOneShot(game.exploreClips[Random.Range(0, game.exploreClips.Length)]);
                }
            }
            if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
            {
                game.rollCrowd.Stop();
            }
            GameObject.FindObjectOfType<CameraShake>().Shake(20);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(5000, transform.position, 10000, 90);
                }
            }
            transform.position = new Vector3(0, -5000, -5000);
            GameManager.bombBalls--;
            PlayerPrefs.SetInt("SaveBomb", GameManager.bombBalls);
            game.isPin = true;
            game.ballType = GameHundredPin.BallType.FallBall;
            cameraFollow.FallMove(transform.position);
            game.camType = GameHundredPin.CameraType.MoveCam;
            game.PinTimeA(6);
        }
        if (Input.GetMouseButtonDown(1) && GameManager.cloneBalls >= 1 && game.powerUps == GameHundredPin.BallPowerUps.Clone && game.ballType == GameHundredPin.BallType.SpinBall)
        {
            GameObject miniBall = Instantiate(spawnBall, new Vector3(transform.position.x, transform.position.y + 32, transform.position.z), Quaternion.identity) as GameObject;
            miniBall.GetComponent<MiniBall>().isSplash = true;
            miniBall.GetComponent<MiniBall>().ChargeBall(GameObject.FindObjectOfType<GameManager>().chooseBalls[GameManager.chooseBallIndex].ballMat, lbs, GameManager.chooseBallIndex);
            miniBall.GetComponent<Rigidbody>().velocity = Vector3.forward * -600;
            if (GameManager.isParticle)
            {
                Instantiate(spawnBallPoof, new Vector3(transform.position.x, transform.position.y + 32, transform.position.z), Quaternion.identity);
            }
            GameManager.cloneBalls--;
            PlayerPrefs.SetInt("SaveClone", GameManager.cloneBalls);
        }
    }

    public void MouseUp()
    {
        if (game.ballType == GameHundredPin.BallType.MoveX && GameManager.type == GameManager.GameState.Game && Input.GetMouseButtonUp(0) && isMoveY)
        {
            if (isThrow)
            {
                Bowl();
            }
            else
            {
                isMoveY = false;
                transform.position = GameObject.FindObjectOfType<HundredPinSetter>().ballPos;
                moveZThrow = GameObject.FindObjectOfType<HundredPinSetter>().ballPos.z;
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
        if (game.powerUps == GameHundredPin.BallPowerUps.Hyper)
        {
            rigidBody.AddForce(moveMouse.x - direction.x * spin * rigidBody.mass / 3, 0, maxSpeed * speed * 200 / 3);
        }
        else
        {
            rigidBody.AddForce(moveMouse.x - direction.x * spin * rigidBody.mass / 3, 0, maxSpeed * speed * 100 / 3);
        }
        game.PlayClip("Thumbpop");
        game.ballType = GameHundredPin.BallType.ThrowBall;
        game.camType = GameHundredPin.CameraType.DropBall;
        game.throwBall++;
    }

    public void Reset()
    {
        rigidBody.useGravity = true;
        force.force = new Vector3(0, 0, 0);
        force.torque = new Vector3(0, 0, 0);
        rigidBody.isKinematic = true;
        transform.position = GameObject.FindObjectOfType<HundredPinSetter>().ballPos;
        transform.rotation = Quaternion.Euler(45, 0, 0);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        moveZThrow = GameObject.FindObjectOfType<HundredPinSetter>().ballPos.z;
        isMoveY = false;
        isThrow = false;
        isSplash = true;
        game.isReplayRecord = false;
        game.powerUps = GameHundredPin.BallPowerUps.Off;
        game.ballType = GameHundredPin.BallType.MoveX;
        game.camType = GameHundredPin.CameraType.MoveX;
    }

    public void ResetCam()
    {
        cameraFollow.Reset();
    }

    public void ChargeBall(Material ballMat, int chargeLbs, int chargeSpeed, int chargeSpin)
    {
        foreach (Renderer meshBall in meshBalls)
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
        if (game.powerUps == GameHundredPin.BallPowerUps.Bomb)
        {
            game.powerUps = GameHundredPin.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = GameHundredPin.BallPowerUps.Bomb;
        }
    }

    public void CloneBall()
    {
        if (game.powerUps == GameHundredPin.BallPowerUps.Clone)
        {
            game.powerUps = GameHundredPin.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = GameHundredPin.BallPowerUps.Clone;
        }
    }

    public void ForcePulseBall()
    {
        if (game.powerUps == GameHundredPin.BallPowerUps.ForcePulse)
        {
            game.powerUps = GameHundredPin.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = GameHundredPin.BallPowerUps.ForcePulse;
        }
    }

    public void HyperBall()
    {
        if (game.powerUps == GameHundredPin.BallPowerUps.Hyper)
        {
            game.powerUps = GameHundredPin.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = GameHundredPin.BallPowerUps.Hyper;
        }
    }

    public void LightningBall()
    {
        if (game.powerUps == GameHundredPin.BallPowerUps.Lightning)
        {
            game.powerUps = GameHundredPin.BallPowerUps.Off;
        }
        else
        {
            game.powerUps = GameHundredPin.BallPowerUps.Lightning;
            if (GameManager.isSound && GameManager.type != GameManager.GameState.Menu)
            {
                game.PlayClip("thunder");
            }
            game.thunderAnimation.Play();
        }
    }
}
