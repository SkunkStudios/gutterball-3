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

    // Use this for initialization
    void Start ()
	{
        pinStartPos = transform.position;
        transform.position = pinStartPos;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().Sleep();
        ball = GameObject.FindObjectOfType<Ball>();
        isSplash = GameObject.FindObjectOfType<PinSetter>().isSplash;
        if (GameManager.pinMode != GameManager.PinMode.Spare)
        {
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
        }
    }

    // Update is called once per frame
    void Update ()
	{
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && !GameObject.FindObjectOfType<PinSetter>().isGravity || collision.gameObject.tag == "Pin" && !GameObject.FindObjectOfType<PinSetter>().isGravity)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<ConstantForce>().force = new Vector3(0, -15, -150);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fall") && !GameObject.FindObjectOfType<PinSetter>().isGravity)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<ConstantForce>().force = new Vector3(0, -15, -150);
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
        if (transform.position.y > pinStartPos.y - 0.5f && transform.position.y < pinStartPos.y + 0.5f)
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
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
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
        }
        type = PinType.PinGravity;
    }

    public void OutOfPin()
    {
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
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
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
        pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
    }

    public void ResetFall(int isPinFall)
    {
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
        if (isPinFall != 0)
        {
            gameObject.SetActive(true);
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOn;
        }
        else
        {
            gameObject.SetActive(false);
            pinLight.material = GameObject.FindObjectOfType<PinSetter>().pinOff;
        }
    }
}
