using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HundredPin : MonoBehaviour
{

    public GameHundredPin game;
    public GameObject splash;

    private BallHundredPin ball;
    private Vector3 pinStartPos;
    private bool pinRaise;
    private bool isSplash = true;
    private bool isFall;

    void Awake()
    {
        pinStartPos = transform.position;
        GetComponent<Rigidbody>().Sleep();
        ball = GameObject.FindObjectOfType<BallHundredPin>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isFall)
        {
            transform.position = pinStartPos;
            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().Sleep();
        }
    }

    void FixedUpdate()
    {
        if (isFall)
        {
            transform.Rotate(GetComponent<Rigidbody>().angularVelocity / 1.25f, Space.World);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pin" && collision.relativeVelocity.magnitude > 600 && game.isPin)
        {
            game.PinTimeA(6);
        }
        if (collision.gameObject.tag != "Lane" && !GetComponent<Rigidbody>().isKinematic && !isFall)
        {
            isFall = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Scooper" && !GetComponent<Rigidbody>().isKinematic)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, collision.contacts[0].point.z - 16.4f);
            GetComponent<Rigidbody>().AddForce(0, 0, -300);
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
        pinRaise = IsStanding();
    }

    public void OutOfPin()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        isSplash = true;
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
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        isSplash = true;
        transform.position = pinStartPos;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().Sleep();
        isFall = false;
    }

    public void FallPinDown()
    {
        if (game.powerUps == GameHundredPin.BallPowerUps.Bomb && Vector3.Distance(ball.transform.position, transform.position) <= 160 || game.powerUps == GameHundredPin.BallPowerUps.ForcePulse && Vector3.Distance(ball.transform.position, transform.position) <= 80 || game.powerUps == GameHundredPin.BallPowerUps.Hyper && Vector3.Distance(ball.transform.position, transform.position) <= 32 || game.powerUps == GameHundredPin.BallPowerUps.Lightning && Vector3.Distance(ball.transform.position, transform.position) <= 48)
        {
            isFall = true;
        }
    }
}
