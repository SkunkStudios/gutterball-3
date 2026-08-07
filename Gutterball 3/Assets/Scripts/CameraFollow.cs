using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ball;
    public Game game;
    public float smoothPosSpeed = 0.25f;
    public float smoothRotSpeed = 10f;

    private Vector3 fallMove;
    private Coroutine replayCoroutine;

    void Update()
    {
        if (transform.position.y < GameObject.FindObjectOfType<PinSetter>().offsetY && game.camType != Game.CameraType.Anim && !game.isPin)
        {
            transform.position = new Vector3(transform.position.x, GameObject.FindObjectOfType<PinSetter>().offsetY, transform.position.z);
        }
        if (transform.position.z < -GameObject.FindObjectOfType<PinSetter>().offsetZ && game.ballType == Game.BallType.SpinBall)
        {
            game.camType = Game.CameraType.LookBall;
        }
    }

    void FixedUpdate ()
	{
        Vector3 desiredPosition;
        Vector3 smoothedPosition;
        if (game.camType == Game.CameraType.MoveX)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<PinSetter>().pos.x, GameObject.FindObjectOfType<PinSetter>().pos.y, GameObject.FindObjectOfType<PinSetter>().pos.z);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
            transform.position = smoothedPosition;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotSetX, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == Game.CameraType.DropBall)
        {
            desiredPosition = new Vector3(ball.position.x, ball.position.y + 45, ball.position.z + 196);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
            transform.position = smoothedPosition;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotOffset, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == Game.CameraType.FollowBall)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<PinSetter>().offset.x, ball.position.y + GameObject.FindObjectOfType<PinSetter>().offset.y, ball.position.z + GameObject.FindObjectOfType<PinSetter>().offset.z);
            if (transform.position.y >= GameObject.FindObjectOfType<PinSetter>().offsetY && !game.isPin)
            {
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, desiredPosition.y, desiredPosition.z), smoothPosSpeed);
                transform.position = smoothedPosition;
            }
            else if (transform.position.y < GameObject.FindObjectOfType<PinSetter>().offsetY && !game.isPin)
            {
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, transform.position.y, desiredPosition.z), smoothPosSpeed);
                transform.position = smoothedPosition;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotOffset, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == Game.CameraType.LookBall)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<PinSetter>().offset.x, transform.position.y, transform.position.z);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
            transform.position = smoothedPosition;
        }
        else if (game.camType == Game.CameraType.MoveCam)
        {
            transform.Translate(-fallMove.x * 0.02f * Time.fixedDeltaTime, 3.2f * Time.fixedDeltaTime, 16 * Time.fixedDeltaTime, Space.World);
        }
        else if (game.camType == Game.CameraType.Replay2)
        {
            transform.Translate(Vector3.forward * 16 * Time.fixedDeltaTime, Space.World);
        }
        else if (game.camType == Game.CameraType.ReturnBall)
        {
            transform.Translate(-3.0f * Time.fixedDeltaTime, -3.0f * Time.fixedDeltaTime, 180 * Time.fixedDeltaTime, Space.World);
            desiredPosition = new Vector3(ball.position.x - transform.position.x, ball.position.y - transform.position.y + 32, ball.position.z - transform.position.z);
            Quaternion lookRotation = Quaternion.LookRotation(desiredPosition);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, smoothRotSpeed * 2 * Time.fixedDeltaTime);
        }
    }

    public void Replay(int index)
    {
        game.camType = Game.CameraType.Replay;
        replayCoroutine = StartCoroutine(GameObject.FindObjectOfType<PinSetter>().replays[index].ReplayMove());
    }

    public void React(int index)
    {
        game.camType = Game.CameraType.ReactCam;
        transform.position = GameObject.FindObjectOfType<PinSetter>().reacts[index].position;
        transform.rotation = GameObject.FindObjectOfType<PinSetter>().reacts[index].rotation;
    }

    public void Reset()
    {
        if (replayCoroutine != null)
        {
            StopCoroutine(replayCoroutine);
        }
        Vector3 desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<PinSetter>().pos.x, GameObject.FindObjectOfType<PinSetter>().pos.y, GameObject.FindObjectOfType<PinSetter>().pos.z);
        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotSetX, 180, 0);
    }

    public void EndCam()
    {
        if (replayCoroutine != null)
        {
            StopCoroutine(replayCoroutine);
        }
        game.camType = Game.CameraType.ReactCam;
        transform.position = new Vector3(-GameObject.FindObjectOfType<PinSetter>().winPos.x, GameObject.FindObjectOfType<PinSetter>().winPos.y, GameObject.FindObjectOfType<PinSetter>().winPos.z);
        transform.rotation = Quaternion.Euler(-GameObject.FindObjectOfType<PinSetter>().winRot.x, 180 - GameObject.FindObjectOfType<PinSetter>().winRot.y, 0);
    }

    public void FallMove(Vector3 camMove)
    {
        fallMove = camMove;
    }
}
