using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{

    public enum PinType { PinGravity, PinRaise, PinStop, PinLower }
    public Renderer pinLight;
    public Game game;
    public GameObject splash;
    public float distToRaise = 40f;
    public bool isHitOne;

    private PinType type;
    private Ball ball;
    private Vector3 pinStartPos;
    private bool pinRaise;
    private bool isSplash;
    private bool isSpare;
    private bool isFall;
    private bool isPortal;
    private float portalGravity;

    void Awake()
    {
        pinStartPos = transform.position;
        GetComponent<Rigidbody>().Sleep();
        ball = GameObject.FindObjectOfType<Ball>();
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
    }

    // Use this for initialization
    void Start ()
	{
        if (GameManager.pinMode != GameManager.PinMode.Spare)
        {
            if (DateTime.Now.Month == 10 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas || DateTime.Now.Month == 12 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas)
            {
                pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOnHalloweenXmas;
            }
            else
            {
                pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
            }
        }
    }

    // Update is called once per frame
    void Update ()
	{
        if (isPortal)
        {
            portalGravity -= Time.deltaTime * 30f;
            GetComponent<ConstantForce>().force = new Vector3(0, 0, portalGravity);
        }
        else
        {
            portalGravity = 0;
        }
        if (GetComponent<Rigidbody>().isKinematic && pinRaise && !game.isCurrentReplay)
        {
            if (type == PinType.PinRaise)
            {
                transform.Translate(new Vector3(0, distToRaise * Time.deltaTime, 0), Space.World);
            }
            if (type == PinType.PinLower)
            {
                transform.Translate(new Vector3(0, -distToRaise * Time.deltaTime, 0), Space.World);
            }
        }
        if (!isFall)
        {
            transform.position = pinStartPos;
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().Sleep();
        }
    }

    private void FixedUpdate()
    {
        if (isFall)
        {
            transform.Rotate(GetComponent<Rigidbody>().angularVelocity / 1.25f, Space.World);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Scooper" && !GetComponent<Rigidbody>().isKinematic)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, collision.contacts[0].point.z - 16);
            GetComponent<Rigidbody>().AddForce(0, 0, -1500);
        }
        if (collision.gameObject.tag != "Lane" && GetComponent<Rigidbody>().useGravity)
        {
            isFall = true;
            if (!GameObject.FindObjectOfType<PinSetter>().isGravity)
            {
                isPortal = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<ConstantForce>().force = new Vector3(0, 0, -10);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Scooper" && !GetComponent<Rigidbody>().isKinematic)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, collision.contacts[0].point.z - 16);
            GetComponent<Rigidbody>().AddForce(0, 0, -175);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gravity") && !GameObject.FindObjectOfType<PinSetter>().isGravity)
        {
            isFall = true;
            isPortal = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<ConstantForce>().force = new Vector3(0, -5 * 0.3f, 0);
        }
        Vector3 splashPosition = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        if (other.CompareTag("Fall") && isSplash || other.CompareTag("Gutter") && isSplash || other.CompareTag("Water") && isSplash)
        {
            if (GameManager.isParticle)
            {
                Instantiate(splash, splashPosition, Quaternion.identity);
            }
            isSplash = false;
        }
    }

    public bool IsStanding()
    {
        if (transform.position.y > pinStartPos.y - 0.5f && transform.position.y < pinStartPos.y + 0.5f && transform.position.z > -3500)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PinDown()
    {
        if (GameManager.pinMode == GameManager.PinMode.Spare)
        {
            isSpare = true;
        }
        pinRaise = IsStanding();
        PinLight();
    }

    public void PinLight()
    {
        if (pinRaise)
        {
            if (DateTime.Now.Month == 10 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas || DateTime.Now.Month == 12 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas)
            {
                pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOnHalloweenXmas;
            }
            else
            {
                pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
            }
        }
        else
        {
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOff;
        }
    }

    public void Raise()
    {
        if (game.throwBall < game.maxBalls)
        {
            isFall = true;
            GetComponent<Rigidbody>().isKinematic = pinRaise;
        }
        type = PinType.PinRaise;
    }

    public void Stop()
    {
        if (pinRaise)
        {
            transform.position = new Vector3(pinStartPos.x, transform.position.y, pinStartPos.z);
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        type = PinType.PinStop;
    }

    public void Lower()
    {
        type = PinType.PinLower;
    }

    public void Land()
    {
        if (GameObject.FindObjectOfType<PinSetter>().isGravity)
        {
            isPortal = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        }
        GetComponent<Rigidbody>().isKinematic = false;
        if (pinRaise && game.throwBall < game.maxBalls)
        {
            transform.position = pinStartPos;
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().Sleep();
            isFall = false;
        }
        type = PinType.PinGravity;
    }

    public void OutOfPin()
    {
        isPortal = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().isKinematic = false;
        isHitOne = false;
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
        gameObject.SetActive(pinRaise);
        if (pinRaise)
        {
            transform.position = pinStartPos;
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().Sleep();
            isFall = false;
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        isPortal = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().isKinematic = false;
        isHitOne = false;
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
        transform.position = pinStartPos;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().Sleep();
        isFall = false;
        if (DateTime.Now.Month == 10 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas || DateTime.Now.Month == 12 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas)
        {
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOnHalloweenXmas;
        }
        else
        {
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
        }
    }

    public void ResetFall(int isPinFall)
    {
        isPortal = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        isHitOne = false;
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
        if (isSpare)
        {
            transform.position = pinStartPos;
        }
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().Sleep();
        isFall = false;
        if (isPinFall != 0)
        {
            gameObject.SetActive(true);
            if (DateTime.Now.Month == 10 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas || DateTime.Now.Month == 12 && GameObject.FindObjectOfType<PinSetter>().isHalloweenXmas)
            {
                pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOnHalloweenXmas;
            }
            else
            {
                pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
            }
        }
        else
        {
            gameObject.SetActive(false);
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOff;
        }
    }

    public void FallPinDown()
    {
        if (game.powerUps == Game.BallPowerUps.Bomb && Vector3.Distance(ball.transform.position, transform.position) <= 160 || game.powerUps == Game.BallPowerUps.ForcePulse && Vector3.Distance(ball.transform.position, transform.position) <= 80 || game.powerUps == Game.BallPowerUps.Hyper && Vector3.Distance(ball.transform.position, transform.position) <= 32 || game.powerUps == Game.BallPowerUps.Lightning && Vector3.Distance(ball.transform.position, transform.position) <= 48)
        {
            isFall = true;
            if (!GameObject.FindObjectOfType<PinSetter>().isGravity)
            {
                isPortal = false;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<ConstantForce>().force = new Vector3(0, 0, -7.5f);
            }
        }
    }
}
