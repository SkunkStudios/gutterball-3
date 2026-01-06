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
    public GameObject bombBall;
    public GameObject hyperBall;
    public GameObject lightningBall;
    public Renderer[] meshBalls;

    private Vector2 moveMouse;
    private Vector2 direction;
    private float fastSpeed;
    private float maxSpeed;
    private bool isMoveY = false;
    private bool isThrow = false;
    private bool isNet = false;
    private bool isBackWall = false;
    private bool isSplash;
    private Vector3 spinStart;
    private Vector3 spinEnd;
    private Rigidbody rigidBody;
    private ConstantForce force;

    // Use this for initialization
    void Start ()
    {
        splashAudio = GameObject.Find("Splash").GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = lbs;
        force = GetComponent<ConstantForce>();
    }

    // Update is called once per frame
    void Update ()
    {
        saturnRingBall.transform.eulerAngles = new Vector3(0, 0, 0);
        uranusRingBall.transform.eulerAngles = new Vector3(0, 0, 0);
        bombBall.transform.eulerAngles = Vector3.zero;
        hyperBall.transform.eulerAngles = Vector3.zero;
        lightningBall.transform.eulerAngles = Vector3.zero;
        if (game.powerUps == Game.BallPowerUps.Off)
        {
            bombBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.Bomb)
        {
            bombBall.SetActive(true);
            hyperBall.SetActive(false);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.Hyper)
        {
            bombBall.SetActive(false);
            hyperBall.SetActive(true);
            lightningBall.SetActive(false);
        }
        else if (game.powerUps == Game.BallPowerUps.Lightning)
        {
            bombBall.SetActive(false);
            hyperBall.SetActive(false);
            lightningBall.SetActive(true);
        }
        if (Game.type != Game.GameState.Menu)
        {
            spinStart = Input.mousePosition;
            spinEnd = spinUI.ScreenToWorldPoint(spinStart);

            direction = new Vector2(spinEnd.x, spinEnd.y);

            controlArrow.transform.up = direction;
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
        if (transform.position.z < GameObject.FindObjectOfType<PinSetter>().ballPos.z && game.ballType == Game.BallType.MoveX && Game.type == Game.GameState.Game && !game.isComputer)
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
                Instantiate(hit, collision.transform.position, Quaternion.identity);
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
                GameObject.FindObjectOfType<CameraShake>().Shake(20);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(6400, transform.position, 12800, 256);
                    }
                }
                transform.position = new Vector3(0, -5000, -5000);
                GameManager.bombBalls--;
            }
            else if (game.powerUps == Game.BallPowerUps.Hyper)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(3200, transform.position, 6400, 0);
                    }
                }
                GameManager.hyperBalls--;
            }
            else if (game.powerUps == Game.BallPowerUps.Lightning)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(1600, transform.position, 3200, 0);
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
                force.force = new Vector3(0, -rigidBody.mass * 25, -rigidBody.mass * 250);
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
            if (game.powerUps == Game.BallPowerUps.Hyper)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(3200, transform.position, 6400, 0);
                    }
                }
            }
            else if (game.powerUps == Game.BallPowerUps.Lightning)
            {
                colliders = Physics.OverlapSphere(transform.position, 32);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(1600, transform.position, 3200, 0);
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
        if (other.CompareTag("Fall") && isSplash && game.ballType == Game.BallType.ThrowBall || other.CompareTag("Fall") && isSplash && game.ballType == Game.BallType.SpinBall || other.CompareTag("Fall") && isSplash && game.ballType == Game.BallType.FallBall || other.CompareTag("Gutter") && isSplash && game.ballType == Game.BallType.SpinBall && !game.isPin && !game.isPin || other.CompareTag("Water") && isSplash && game.ballType == Game.BallType.SpinBall || other.CompareTag("Water") && isSplash && game.ballType == Game.BallType.FallBall)
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
                force.force = new Vector3(0, -rigidBody.mass * 25, -rigidBody.mass * 250);
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
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        transform.position = new Vector3(Random.Range(-75, 75), GameObject.FindObjectOfType<PinSetter>().ballPos.y, GameObject.FindObjectOfType<PinSetter>().ballPos.z);
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
        if (speed <= 30)
        {
            rigidBody.AddForce(Vector3.forward * -speed * rigidBody.mass * 1125);
        }
        else if (speed > 30 && speed <= 40)
        {
            rigidBody.AddForce(Vector3.forward * -speed * rigidBody.mass * 875);
        }
        else if (speed > 40 && speed <= 50)
        {
            rigidBody.AddForce(Vector3.forward * -speed * rigidBody.mass * 750);
        }
        else if (speed > 50 && speed <= 60)
        {
            rigidBody.AddForce(Vector3.forward * -speed * rigidBody.mass * 625);
        }
        else if (speed > 60)
        {
            rigidBody.AddForce(Vector3.forward * -speed * 5625);
        }
        if (transform.position.x < GameObject.FindObjectOfType<PinSetter>().ballPos.x)
        {
            if (spin < 25)
            {
                force.force = new Vector3(spin * -rigidBody.mass * 0.75f, 0, 0);
                force.torque = new Vector3(0, 0, spin * rigidBody.mass * 0.375f);
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(25, 112.5f));
            }
            else if (spin >= 25 && spin < 50)
            {
                force.force = new Vector3(spin * -rigidBody.mass * 0.5f, 0, 0);
                force.torque = new Vector3(0, 0, spin * rigidBody.mass * 0.25f);
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(25, 75));
            }
            else if (spin >= 50)
            {
                force.force = new Vector3(spin * -rigidBody.mass * 0.125f, 0, 0);
                force.torque = new Vector3(0, 0, spin * rigidBody.mass * 0.0625f);
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(25, 37.5f));
            }
        }
        else if (transform.position.x == GameObject.FindObjectOfType<PinSetter>().ballPos.x)
        {
            force.force = new Vector3(0, 0, 0);
            if (spin < 25)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-112.5f, 112.5f));
            }
            else if (spin >= 25 && spin < 50)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-75, 75));
            }
            else if (spin >= 50)
            {
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-37.5f, 37.5f));
            }
        }
        else if (transform.position.x > GameObject.FindObjectOfType<PinSetter>().ballPos.x)
        {
            if (spin < 25)
            {
                force.force = new Vector3(spin * rigidBody.mass * 0.75f, 0, 0);
                force.torque = new Vector3(0, 0, spin * -rigidBody.mass * 0.375f);
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-112.5f, -25));
            }
            else if (spin >= 25 && spin < 50)
            {
                force.force = new Vector3(spin * rigidBody.mass * 0.5f, 0, 0);
                force.torque = new Vector3(0, 0, spin * -rigidBody.mass * 0.25f);
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-75, -25));
            }
            else if (spin >= 50)
            {
                force.force = new Vector3(spin * rigidBody.mass * 0.125f, 0, 0);
                force.torque = new Vector3(0, 0, spin * -rigidBody.mass * 0.0625f);
                rigidBody.AddForce(Vector3.right * spin * rigidBody.mass * Random.Range(-37.5f, -25));
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
