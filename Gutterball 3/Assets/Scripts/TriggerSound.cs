using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public string clipName;
    public GameObject hitParticles;
    public GameObject objectShake;
    public float maxShake;
    public bool isHitPins;

    private float shakeTime;
    private Vector2 shake;
    private AudioSource audioSource;
    private bool isBounce;

    // Use this for initialization
    void Start()
    {
        audioSource = GameObject.Find("SFXSource").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        shake.x = Random.Range(-shakeTime, shakeTime);
        shake.y = Random.Range(-shakeTime, shakeTime);
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime * 0.5f;
        }
        if (shakeTime <= 0)
        {
            shakeTime = 0;
        }
        if (objectShake != null)
        {
            objectShake.transform.position = new Vector3(shake.x, shake.y, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && !isBounce)
        {
            if (audioSource != null && GameManager.isSound && Game.type != Game.GameState.Menu)
            {
                audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/" + clipName));
            }
            isBounce = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") || other.CompareTag("Pin") && isHitPins)
        {
            if (hitParticles != null && GameManager.isParticle)
            {
                Vector3 splashPosition = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
                Instantiate(hitParticles, splashPosition, Quaternion.identity);
            }
            shakeTime = maxShake;
            if (audioSource != null && GameManager.isSound && Game.type != Game.GameState.Menu)
            {
                audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/" + clipName));
            }
        }
    }

    public void Reset()
    {
        isBounce = false;
    }
}
