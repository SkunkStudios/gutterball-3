using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRigidbody : MonoBehaviour
{
    public float forceX;
    public float forceZ;
    public bool isStartForce = false;
    public bool isSleep = false;
    public bool isGravity = true;

    private Rigidbody rigidBody;
    private bool isPortal;
    private float portalGravity;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (isSleep)
        {
            rigidBody.Sleep();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isStartForce)
        {
            rigidBody.AddForce(forceX, 0, -forceZ);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPortal)
        {
            portalGravity -= Time.deltaTime * rigidBody.mass * 100;
            GetComponent<ConstantForce>().force = new Vector3(0, 0, portalGravity);
        }
        else
        {
            portalGravity = 0;
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(rigidBody.angularVelocity / 1.25f, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && rigidBody.useGravity || collision.gameObject.tag == "Pin" && rigidBody.useGravity)
        {
            if (!isGravity)
            {
                isPortal = true;
                rigidBody.useGravity = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gravity"))
        {
            isPortal = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<ConstantForce>().force = new Vector3(0, -5 * rigidBody.mass, 0);
        }
    }

    public void BowlForce()
    {
        rigidBody.isKinematic = false;
        rigidBody.AddForce(forceX, 0, -forceZ);
    }
}
