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
    private float reactMove;

    void Update()
    {
        if (transform.position.y < GameObject.FindObjectOfType<PinSetter>().offsetY && game.camType != Game.CameraType.Anim && !game.isPin)
        {
            transform.position = new Vector3(transform.position.x, GameObject.FindObjectOfType<PinSetter>().offsetY, transform.position.z);
        }
        if (transform.position.x > -GameObject.FindObjectOfType<PinSetter>().offsetX && transform.position.x < GameObject.FindObjectOfType<PinSetter>().offsetX && transform.position.z < -GameObject.FindObjectOfType<PinSetter>().offsetZ && game.ballType == Game.BallType.SpinBall)
        {
            if (game.camType == Game.CameraType.FollowBall)
            {
                game.camType = Game.CameraType.LookBall;
            }
            else if (game.camType == Game.CameraType.ComputerFollow)
            {
                game.camType = Game.CameraType.ComputerLook;
            }
        }
    }

    void FixedUpdate ()
	{
        Vector3 desiredPosition;
        Vector3 smoothedPosition;
        if (game.camType == Game.CameraType.MoveX)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<PinSetter>().pos.x, GameObject.FindObjectOfType<PinSetter>().pos.y, GameObject.FindObjectOfType<PinSetter>().pos.z);
            if (game.isPinTarget)
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
                transform.position = smoothedPosition;
            }
            else
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed / 2);
                transform.position = smoothedPosition;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotSetX, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == Game.CameraType.DropBall)
        {
            desiredPosition = new Vector3(ball.position.x, ball.position.y + 45, ball.position.z + 196);
            if (game.isPinTarget)
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
                transform.position = smoothedPosition;
            }
            else
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed / 2);
                transform.position = smoothedPosition;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<PinSetter>().rot * GameObject.FindObjectOfType<PinSetter>().rotOffset, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == Game.CameraType.FollowBall)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<PinSetter>().offset.x, ball.position.y + GameObject.FindObjectOfType<PinSetter>().offset.y, ball.position.z + GameObject.FindObjectOfType<PinSetter>().offset.z);
            if (transform.position.y >= GameObject.FindObjectOfType<PinSetter>().offsetY && !game.isPin)
            {
                if (game.isPinTarget)
                {
                    smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
                    transform.position = smoothedPosition;
                }
                else
                {
                    smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed / 2);
                    transform.position = smoothedPosition;
                }
            }
            else if (transform.position.y < GameObject.FindObjectOfType<PinSetter>().offsetY && !game.isPin)
            {
                if (game.isPinTarget)
                {
                    smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, GameObject.FindObjectOfType<PinSetter>().offsetY, desiredPosition.z), smoothPosSpeed);
                    transform.position = smoothedPosition;
                }
                else
                {
                    smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, GameObject.FindObjectOfType<PinSetter>().offsetY, desiredPosition.z), smoothPosSpeed / 2);
                    transform.position = smoothedPosition;
                }
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
            transform.Translate(-fallMove.x * 0.1f / 3 * Time.fixedDeltaTime, 10 / 3 * Time.fixedDeltaTime, 50 / 3 * Time.fixedDeltaTime, Space.World);
        }
        else if (game.camType == Game.CameraType.ReactCam)
        {
            transform.Translate(Vector3.right * reactMove * Time.fixedDeltaTime, Space.World);
        }
        else if (game.camType == Game.CameraType.Replay2)
        {
            transform.Translate(Vector3.forward * 50 / 3 * Time.fixedDeltaTime, Space.World);
        }
        else if (game.camType == Game.CameraType.ReturnBall)
        {
            transform.Translate(-10 / 3 * Time.fixedDeltaTime, -10 / 3 * Time.fixedDeltaTime, 500 / 3 * Time.fixedDeltaTime, Space.World);
            desiredPosition = new Vector3(ball.position.x - transform.position.x, ball.position.y - transform.position.y + 32, ball.position.z - transform.position.z);
            Quaternion lookRotation = Quaternion.LookRotation(desiredPosition);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, smoothRotSpeed * 2 * Time.fixedDeltaTime);
        }
        else if (game.camType == Game.CameraType.ComputerCam)
        {
            transform.Translate(Vector3.forward * -50 / 3 * Time.fixedDeltaTime, Space.World);
        }
        else if (game.camType == Game.CameraType.ComputerFollow)
        {
            transform.position = new Vector3(ball.position.x - 59.5f, ball.position.y + 61.25f, ball.position.z + 287.5f);
            transform.rotation = Quaternion.Euler(5, 170, 0);
        }
        else if (game.camType == Game.CameraType.ComputerLook)
        {
            transform.position = new Vector3(ball.position.x - 59.5f, ball.position.y + 61.25f, transform.position.z);
            transform.rotation = Quaternion.Euler(5, 170, 0);
        }
    }

    public void Replay(int index)
    {
        game.camType = Game.CameraType.Replay;
        replayCoroutine = StartCoroutine(GameObject.FindObjectOfType<PinSetter>().replays[index].ReplayMove());
    }

    public void React(int index)
    {
        reactMove = Random.Range(-10, 10) / 3;
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

    public void ComputerCam()
    {
        if (replayCoroutine != null)
        {
            StopCoroutine(replayCoroutine);
        }
        game.camType = Game.CameraType.ComputerCam;
        transform.position = new Vector3(-GameObject.FindObjectOfType<PinSetter>().compuPos.x, GameObject.FindObjectOfType<PinSetter>().compuPos.y, GameObject.FindObjectOfType<PinSetter>().compuPos.z);
        transform.rotation = Quaternion.Euler(-GameObject.FindObjectOfType<PinSetter>().compuRot.x, 180 - GameObject.FindObjectOfType<PinSetter>().compuRot.y, 0);
        StartCoroutine(ComputerFollowThrow());
    }

    public void EndCam()
    {
        if (replayCoroutine != null)
        {
            StopCoroutine(replayCoroutine);
        }
        game.camType = Game.CameraType.EndCam;
        transform.position = new Vector3(-GameObject.FindObjectOfType<PinSetter>().winPos.x, GameObject.FindObjectOfType<PinSetter>().winPos.y, GameObject.FindObjectOfType<PinSetter>().winPos.z);
        transform.rotation = Quaternion.Euler(-GameObject.FindObjectOfType<PinSetter>().winRot.x, 180 - GameObject.FindObjectOfType<PinSetter>().winRot.y, 0);
    }

    public void FallMove(Vector3 camMove)
    {
        fallMove = camMove;
    }

    IEnumerator ComputerFollowThrow()
    {
        yield return new WaitForSeconds(1.5f);
        if (game.camType == Game.CameraType.ComputerCam)
        {
            if (GameManager.chooseAlleys == GameManager.Alley.Wacky)
            {
                game.camType = Game.CameraType.FollowBall;
            }
            else
            {
                game.camType = Game.CameraType.ComputerFollow;
            }
        }
    }
}
