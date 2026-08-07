using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ObjectMove
{
    public Monkey monkey;
    public Vector3 fireworksPosition;
    public Vector3 move;
    public float jump;
    public float waitTime;
    public int animIndex;
    public bool isFireworksPop;
    public bool isPop;
    public bool isPlayAudio;
    public bool isJump;
    public bool isAnim;
    public string clipName;
}
public class ObjectAnimation : MonoBehaviour
{
    public string clipName;
    public AnimList animList;
    public Monkey monkey;
    public Transform reobject;
    public Transform objectTarget;
    public Transform camTarget;
    public GameObject fireworks;
    public Vector3 fireworksStartPosition;
    public Vector3 position;
    public Vector3 rotation;
	public ObjectMove[] objectMoves;
    public bool startFireworksPop;
    public bool startPlayAudio;
    public bool startJump;
    public bool isPlayers;
    public bool isStop = true;
    public int playIndex;
    public Vector3 popPosition;
    public Vector2 fallMove;
    public float jump;
    private float time;
	private int nextIndex;
	public bool isMove;
	private bool isTime;
    private Vector3 startPosition;
    private Vector3 movePosition;
    private Vector3 fall;
    private bool isFall;
    private Game game;

    void Start()
    {
        game = GameObject.FindObjectOfType<Game>();
        if (objectTarget != null)
        {
            startPosition = objectTarget.position;
        }
    }

    void Update ()
    {
        if (isFall)
        {
            fall.x -= fallMove.x * Time.deltaTime;
            fall.y -= fallMove.y * Time.deltaTime;
        }
        if (objectMoves.Length >= 1 && nextIndex < objectMoves.Length && isTime)
        {
            time += 10 * Time.deltaTime;
        }
        if (objectMoves.Length >= 1 && nextIndex < objectMoves.Length && isMove)
        {
            if (objectTarget != null)
            {
                objectTarget.Translate(fall.x * 10 * Time.deltaTime, fall.y * 10 * Time.deltaTime, objectMoves[nextIndex].move.z * 10 * Time.deltaTime, Space.World);
            }
            if (camTarget != null && game.camType == Game.CameraType.Anim)
            {
                camTarget.Translate(fall.x * 10 * Time.deltaTime, fall.y * 10 * Time.deltaTime, objectMoves[nextIndex].move.z * 10 * Time.deltaTime, Space.World);
            }
        }

        if (time >= objectMoves[nextIndex].waitTime && isTime)
        {
            if (monkey != null)
            {
                monkey.Stop();
            }
            if (isStop && nextIndex < objectMoves.Length)
            {
                if (objectTarget != null)
                {
                    objectTarget.position = movePosition;
                }
                if (camTarget != null && game.camType == Game.CameraType.Anim)
                {
                    camTarget.position = movePosition;
                }
            }
            if (animList != null && objectMoves[nextIndex].isAnim)
            {
                animList.PlayAnim(objectMoves[nextIndex].animIndex, fallMove.y);
            }
            if (fireworks != null && objectMoves[nextIndex].isFireworksPop && GameManager.isParticle)
            {
                GameObject pop = Instantiate(fireworks, objectMoves[nextIndex].fireworksPosition, Quaternion.identity) as GameObject;
                var main = pop.GetComponent<ParticleSystem>().main;
                main.startColor = pop.GetComponent<Fireworks>().colorFireworks[Random.Range(0, pop.GetComponent<Fireworks>().colorFireworks.Length)];
            }
            if (objectMoves[nextIndex].monkey != null)
            {
                objectMoves[nextIndex].monkey.Jump(objectMoves[nextIndex].jump);
            }
            if (objectTarget != null && objectMoves[nextIndex].isPop)
            {
                if (fireworks != null && animList != null && GameManager.isChars && GameManager.isParticle)
                {
                    GameObject pop = Instantiate(fireworks, objectTarget.position, Quaternion.identity) as GameObject;
                    var main = pop.GetComponent<ParticleSystem>().main;
                    main.startColor = pop.GetComponent<Fireworks>().colorFireworks[Random.Range(0, pop.GetComponent<Fireworks>().colorFireworks.Length)];
                }
                else if (fireworks != null && animList == null && GameManager.isParticle)
                {
                    GameObject pop = Instantiate(fireworks, objectTarget.position, Quaternion.identity) as GameObject;
                    var main = pop.GetComponent<ParticleSystem>().main;
                    main.startColor = pop.GetComponent<Fireworks>().colorFireworks[Random.Range(0, pop.GetComponent<Fireworks>().colorFireworks.Length)];
                }
                objectTarget.position = popPosition;
            }
            if (animList != null && GameManager.isChars)
            {
                animList.PlaySFX(objectMoves[nextIndex].clipName);
            }
            else if (animList == null)
            {
                GameObject.FindObjectOfType<Game>().PlayClip(objectMoves[nextIndex].clipName);
            }
            isFall = objectMoves[nextIndex].isJump;
            time = 0;
            nextIndex++;
            isMove = true;
            if (nextIndex < objectMoves.Length)
            {
                if (objectTarget != null)
                {
                    movePosition = objectTarget.position + objectMoves[nextIndex].move * objectMoves[nextIndex].waitTime;
                }
                if (camTarget != null && game.camType == Game.CameraType.Anim)
                {
                    movePosition = camTarget.position + objectMoves[nextIndex].move * objectMoves[nextIndex].waitTime;
                }
                fall = objectMoves[nextIndex].move;
            }
        }

        if (nextIndex >= objectMoves.Length)
        {
            StopObject();
            if (objectTarget != null && animList != null && GameManager.isChars)
            {
                animList.PlayAnim(0, 0);
            }
        }
    }

    public void PlayIdle()
    {
        time = 0;
        nextIndex = 0;
        isMove = true;
        isTime = true;
    }

    public void PlayObject()
    {
        if (animList != null && GameManager.isChars)
        {
            animList.PlaySFX(clipName);
            animList.PlayAnim(playIndex, 0);
        }
        else if (animList == null)
        {
            GameObject.FindObjectOfType<Game>().PlayClip(clipName);
        }
        if (monkey != null)
        {
            monkey.Stop();
        }
        if (objectTarget != null)
        {
            objectTarget.position = position;
        }
        if (camTarget != null)
        {
            camTarget.position = position;
            camTarget.rotation = Quaternion.Euler(rotation.x, 180 + rotation.y, rotation.z);
        }
        time = 0;
        nextIndex = 0;
        if (monkey == null)
        {
            movePosition = position + objectMoves[0].move * objectMoves[0].waitTime;
        }
        if (startJump)
        {
            monkey.Jump(jump);
        }
        if (fireworks != null && startFireworksPop && GameManager.isParticle)
        {
            GameObject pop = Instantiate(fireworks, fireworksStartPosition, Quaternion.identity) as GameObject;
            var main = pop.GetComponent<ParticleSystem>().main;
            main.startColor = pop.GetComponent<Fireworks>().colorFireworks[Random.Range(0, pop.GetComponent<Fireworks>().colorFireworks.Length)];
        }
        fall = objectMoves[nextIndex].move;
        if (objectMoves.Length >= 1)
        {
            isMove = true;
            isTime = true;
        }
    }

    public void SkipObject()
    {
        if (monkey != null && GameManager.isChars)
        {
            monkey.Play();
        }
        if (animList != null && GameManager.isChars)
        {
            animList.PlayAnim(0, 0);
        }
        if (isPlayers)
        {
            objectTarget.position = new Vector3(0, 5000, -5000);
        }
        else
        {
            if (objectTarget != null)
            {
                objectTarget.position = startPosition;
            }
        }
        time = 0;
        nextIndex = 0;
        isMove = false;
        isTime = false;
        isFall = false;
    }

    public void StopObject()
    {
        if (objectTarget != null && monkey != null && GameManager.isChars)
        {
            monkey.Play();
        }
        if (objectTarget != null && animList != null && monkey == null && GameManager.isChars)
        {
            animList.PlayAnim(0, 0);
        }
        if (reobject != null)
        {
            if (isPlayers)
            {
                reobject.position = new Vector3(0, 5000, -5000);
            }
            else
            {
                reobject.position = startPosition;
            }
        }
        time = 0;
        nextIndex = 0;
        isMove = false;
        isTime = false;
        isFall = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (objectMoves.Length == 1)
        {
            Gizmos.DrawLine(position, position + objectMoves[0].move * objectMoves[0].waitTime);
        }
        else if (objectMoves.Length == 2)
        {
            Gizmos.DrawLine(position, position + objectMoves[0].move * objectMoves[0].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime);
        }
        else if (objectMoves.Length == 3)
        {
            Gizmos.DrawLine(position, position + objectMoves[0].move * objectMoves[0].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime);
        }
        else if (objectMoves.Length == 4)
        {
            Gizmos.DrawLine(position, position + objectMoves[0].move * objectMoves[0].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime + objectMoves[3].move * objectMoves[3].waitTime);
        }
        else if (objectMoves.Length == 5)
        {
            Gizmos.DrawLine(position, position + objectMoves[0].move * objectMoves[0].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime + objectMoves[3].move * objectMoves[3].waitTime);
            Gizmos.DrawLine(position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime + objectMoves[3].move * objectMoves[3].waitTime, position + objectMoves[0].move * objectMoves[0].waitTime + objectMoves[1].move * objectMoves[1].waitTime + objectMoves[2].move * objectMoves[2].waitTime + objectMoves[3].move * objectMoves[3].waitTime + objectMoves[4].move * objectMoves[4].waitTime);
        }
    }
}
