using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowHundredPin : MonoBehaviour
{
    public Transform ball;
    public GameHundredPin game;
    public float smoothPosSpeed = 0.25f;
    public float smoothRotSpeed = 10f;

    private Vector3 fallMove;
    private Coroutine replayCoroutine;
    private float reactMoveX;
    private float reactMoveZ;

    void Update()
    {
        if (game.camType == GameHundredPin.CameraType.Intro)
        {
            transform.Translate(0, 0, 300 * Time.deltaTime, Space.World);
        }
        if (transform.position.y < GameObject.FindObjectOfType<HundredPinSetter>().offsetY && game.camType != GameHundredPin.CameraType.Anim && !game.isPin)
        {
            transform.position = new Vector3(transform.position.x, GameObject.FindObjectOfType<HundredPinSetter>().offsetY, transform.position.z);
        }
        if (transform.position.z < -3000 && game.ballType == GameHundredPin.BallType.SpinBall)
        {
            if (game.camType == GameHundredPin.CameraType.FollowBall)
            {
                game.camType = GameHundredPin.CameraType.LookBall;
            }
        }
        if (game.camType == GameHundredPin.CameraType.EndCam)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(-transform.position.x, 120 - transform.position.y, -3298.75f - transform.position.z));
        }
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition;
        Vector3 smoothedPosition;
        if (game.camType == GameHundredPin.CameraType.MoveX)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<HundredPinSetter>().pos.x, GameObject.FindObjectOfType<HundredPinSetter>().pos.y, GameObject.FindObjectOfType<HundredPinSetter>().pos.z);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
            transform.position = smoothedPosition;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<HundredPinSetter>().rot * GameObject.FindObjectOfType<HundredPinSetter>().rotSetX, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == GameHundredPin.CameraType.DropBall)
        {
            desiredPosition = new Vector3(ball.position.x, ball.position.y + 45, ball.position.z + 196);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
            transform.position = smoothedPosition;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<HundredPinSetter>().rot * GameObject.FindObjectOfType<HundredPinSetter>().rotOffset, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == GameHundredPin.CameraType.FollowBall)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<HundredPinSetter>().offset.x, ball.position.y + GameObject.FindObjectOfType<HundredPinSetter>().offset.y, ball.position.z + GameObject.FindObjectOfType<HundredPinSetter>().offset.z);
            if (transform.position.y >= GameObject.FindObjectOfType<HundredPinSetter>().offsetY && !game.isPin)
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
                transform.position = smoothedPosition;
            }
            else if (transform.position.y < GameObject.FindObjectOfType<HundredPinSetter>().offsetY && !game.isPin)
            {
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, GameObject.FindObjectOfType<HundredPinSetter>().offsetY, desiredPosition.z), smoothPosSpeed);
                transform.position = smoothedPosition;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GameObject.FindObjectOfType<HundredPinSetter>().rot * GameObject.FindObjectOfType<HundredPinSetter>().rotOffset, 180, 0), smoothRotSpeed * Time.fixedDeltaTime);
        }
        else if (game.camType == GameHundredPin.CameraType.LookBall)
        {
            desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<HundredPinSetter>().offset.x, transform.position.y, transform.position.z);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothPosSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void Reset()
    {
        if (replayCoroutine != null)
        {
            StopCoroutine(replayCoroutine);
        }
        Vector3 desiredPosition = new Vector3(ball.position.x + GameObject.FindObjectOfType<HundredPinSetter>().pos.x, GameObject.FindObjectOfType<HundredPinSetter>().pos.y, GameObject.FindObjectOfType<HundredPinSetter>().pos.z);
        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(GameObject.FindObjectOfType<HundredPinSetter>().rot * GameObject.FindObjectOfType<HundredPinSetter>().rotSetX, 180, 0);
    }

    public void FallMove(Vector3 camMove)
    {
        fallMove = camMove;
    }
}
