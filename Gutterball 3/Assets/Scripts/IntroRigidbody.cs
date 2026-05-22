using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRigidbody : MonoBehaviour
{
    public float forceX;
    public float forceZ;
    public float spin;
    public bool isStartForce = false;
    public bool isSleep = false;
    public bool isGravity = true;

    private Rigidbody rigidBody;

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
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(rigidBody.angularVelocity / 1.25f, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" || collision.gameObject.tag == "Pin")
        {
            if (!isGravity)
            {
                rigidBody.useGravity = false;
                GetComponent<ConstantForce>().force = new Vector3(0, 0, -rigidBody.mass * 100);
            }
        }
    }

    public void BowlForce()
    {
        rigidBody.isKinematic = false;
        rigidBody.AddForce(forceX, 0, -forceZ);
    }
}
