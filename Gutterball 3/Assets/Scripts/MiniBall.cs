using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBall : MonoBehaviour
{
    public GameObject earthCloudBall;
    public GameObject saturnRingBall;
    public GameObject uranusRingBall;
    public GameObject sunBall;
    public Renderer meshBall;
    public GameObject poof;
    public GameObject splash;
    public bool isSplash;

    private bool isPortal;
    private float portalGravity;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        if (isPortal && !GetComponent<Rigidbody>().isKinematic)
        {
            portalGravity -= Time.deltaTime * GetComponent<Rigidbody>().mass * 100;
            GetComponent<ConstantForce>().force = new Vector3(0, 0, portalGravity);
        }
        else if (!isPortal)
        {
            portalGravity = 0;
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(GetComponent<Rigidbody>().angularVelocity / 1.25f, Space.World);
    }

    void LateUpdate()
    {
        saturnRingBall.transform.eulerAngles = Vector3.zero;
        uranusRingBall.transform.eulerAngles = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pin" && GetComponent<Rigidbody>().useGravity && GameObject.FindObjectOfType<PinSetter>() != null && !GameObject.FindObjectOfType<PinSetter>().isGravity)
        {
            isPortal = true;
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 splashPosition = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        if (other.CompareTag("Fall") && isSplash || other.CompareTag("Gutter") && isSplash|| other.CompareTag("Water") && isSplash)
        {
            if (GameManager.isParticle)
            {
                Instantiate(splash, splashPosition, Quaternion.identity);
            }
            isSplash = false;
        }
        if (other.CompareTag("Gravity") && GameObject.FindObjectOfType<PinSetter>() != null && !GameObject.FindObjectOfType<PinSetter>().isGravity)
        {
            isPortal = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<ConstantForce>().force = new Vector3(0, -5 * GetComponent<Rigidbody>().mass, 0);
        }
    }

    public void ChargeBall(Material ballMat, int chargeLbs, int ballFX)
    {
        meshBall.material = ballMat;
        GetComponent<Rigidbody>().mass = chargeLbs;
        if (ballFX == 47)
        {
            earthCloudBall.SetActive(true);
            saturnRingBall.SetActive(false);
            uranusRingBall.SetActive(false);
            sunBall.SetActive(false);
        }
        else if (ballFX == 50)
        {
            earthCloudBall.SetActive(false);
            saturnRingBall.SetActive(true);
            uranusRingBall.SetActive(false);
            sunBall.SetActive(false);
        }
        else if (ballFX == 51)
        {
            earthCloudBall.SetActive(false);
            saturnRingBall.SetActive(false);
            uranusRingBall.SetActive(true);
            sunBall.SetActive(false);
        }
        else if (ballFX == 54)
        {
            earthCloudBall.SetActive(false);
            saturnRingBall.SetActive(false);
            uranusRingBall.SetActive(false);
            sunBall.SetActive(true);
        }
        else
        {
            earthCloudBall.SetActive(false);
            saturnRingBall.SetActive(false);
            uranusRingBall.SetActive(false);
            sunBall.SetActive(false);
        }
    }

    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(15);
        if (GameManager.isParticle)
        {
            Instantiate(poof, transform.position, Quaternion.identity);
        }
        DestroyBall();
    }

    public void DestroyBall()
    {
        Destroy(gameObject);
    }
}
