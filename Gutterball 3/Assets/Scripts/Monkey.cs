using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public SpriteRenderer[] monkeySprites;
    public GameObject monkeyBall;
    public ObjectAnimation[] objectAnimations;

    private Transform ball;
    private bool isJump;
    private bool isJumpOut;
    private bool isAnim;
    private bool isFall;
    private bool isRoll;
    private Vector3 position;
    private Vector3 fall;
    private AnimList animList;

    // Use this for initialization
    void Start ()
	{
        ball = GameObject.Find("Ball").transform;
        animList = GetComponent<AnimList>();
        position = transform.position;
        StartCoroutine(MonkeyAnimTime());
    }

    // Update is called once per frame
    void Update ()
	{
        monkeyBall.transform.localScale = ball.localScale;
        if (isJump)
        {
            transform.Translate(fall.x * Time.deltaTime, fall.y * 10 * Time.deltaTime, 0, Space.World);
            fall.y -= 40 * Time.deltaTime;
        }
        if (isJump && transform.position.y < position.y)
        {
            for (int i = 0; i < objectAnimations.Length; i++)
            {
                if (objectAnimations[i] != null && objectAnimations.Length >= 1)
                {
                    objectAnimations[i].isMove = false;
                }
            }
            animList.PlayAnim(8, 0);
            if (isFall)
            {
                fall.x = 0;
            }
            transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
            isJump = false;
            isFall = false;
            GetComponent<BoxCollider>().enabled = true;
        }

        if (isJumpOut)
        {
            if (isRoll)
            {
                transform.position = ball.position;
                monkeyBall.transform.rotation = ball.rotation;
            }
            else
            {
                transform.position = new Vector3(ball.position.x, ball.position.y + 40, ball.position.z);
            }
        }
        if (isJumpOut && transform.position.z <= -3000)
        {
            JumpOut(50);
        }
    }

    void JumpOut(float jump)
    {
        MonkeyRoll(true);
        monkeyBall.SetActive(false);
        transform.position = new Vector3(ball.position.x, ball.position.y + 40, ball.position.z);
        animList.PlayAnim(6, jump);
        fall = new Vector3(Random.Range(-300, 300), jump, 50);
        isJump = true;
        isJumpOut = false;
    }

    public void Jump(float jump)
    {
        if (!isJumpOut)
        {
            if (animList != null)
            {
                animList.PlayAnim(6, jump);
            }
            fall = new Vector3(0, jump, 0);
            isJump = true;
        }
    }

    public void Play()
    {
        isAnim = true;
    }

    public void Stop()
    {
        transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
        isJump = false;
        isAnim = false;
    }

    public void MonkeyRoll(bool isRollMonkey)
    {
        foreach (SpriteRenderer monkeySprite in monkeySprites)
        {
            monkeySprite.enabled = isRollMonkey;
        }
    }

    void RandomMonkey()
    {
        int randomMonkeyIndex = Random.Range(0, 6);
        if (randomMonkeyIndex == 0)
        {
            if (animList != null)
            {
                animList.PlayAnim(0, 0);
            }
        }
        else if (randomMonkeyIndex == 1)
        {
            Jump(Random.Range(15, 45));
        }
        else if (randomMonkeyIndex == 2)
        {
            if (animList != null)
            {
                animList.PlayAnim(9, 0);
            }
        }
        else if (randomMonkeyIndex == 3)
        {
            if (animList != null)
            {
                animList.PlayAnim(10, 0);
            }
        }
        else if (randomMonkeyIndex == 4)
        {
            if (animList != null)
            {
                animList.PlayAnim(11, 0);
            }
        }
        else if (randomMonkeyIndex == 5)
        {
            if (animList != null)
            {
                animList.PlayAnim(12, 0);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (ball.GetComponent<Rigidbody>().velocity.sqrMagnitude > 320000)
            {
                isRoll = true;
            }
            else
            {
                isRoll = false;
            }
            if (isRoll)
            {
                animList.PlayAnim(3, 0);
                MonkeyRoll(false);
                monkeyBall.SetActive(true);
            }
            else
            {
                animList.PlayAnim(2, 0);
                MonkeyRoll(true);
                monkeyBall.SetActive(false);
            }
            fall.x = transform.position.x;
            fall.x = transform.position.z;
            isJumpOut = true;
            isFall = true;
            GameObject.FindObjectOfType<CameraShake>().Shake(6);
            GameObject.FindObjectOfType<Game>().PlayClip("kick1");
            GameObject.FindObjectOfType<Game>().PlayClip("CHIMP1");
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public IEnumerator MonkeyAnimTime()
    {
        isAnim = true;
        yield return new WaitForSeconds(Random.Range(3, 30));
        while (true)
        {
            if (!isJump && !isJumpOut && isAnim)
            {
                RandomMonkey();
            }
            yield return new WaitForSeconds(Random.Range(3, 30));
        }
    }
}
