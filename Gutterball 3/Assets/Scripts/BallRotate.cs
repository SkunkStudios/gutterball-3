using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotate : MonoBehaviour
{
    public Vector3 axis;

    private Vector3 currentAxis;
    private bool isNegativeRollX;
    private bool isNegativeRollY;
    private bool isRollBall;

    // Start is called before the first frame update
    void Start()
    {
        ResetAxis();
        ResetRotate();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRollBall)
        {
            currentAxis.x -= Input.GetAxis("Mouse Y") * 15;
            currentAxis.y -= Input.GetAxis("Mouse X") * 15;
            if (currentAxis.x < 0)
            {
                isNegativeRollX = true;
            }
            else if (currentAxis.x > 0)
            {
                isNegativeRollX = false;
            }
            if (currentAxis.y < -10)
            {
                isNegativeRollY = true;
            }
            else if (currentAxis.y > -10)
            {
                isNegativeRollY = false;
            }
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * 100 * Time.deltaTime, Space.World);
        }
        else
        {
            if (isNegativeRollX)
            {
                if (currentAxis.x < 0)
                {
                    currentAxis.x += Time.deltaTime * 100 / 3;
                }
                else if (currentAxis.x > 0)
                {
                    currentAxis.x = 0;
                }
            }
            else
            {
                if (currentAxis.x > 0)
                {
                    currentAxis.x -= Time.deltaTime * 100 / 3;
                }
                else if (currentAxis.x < 0)
                {
                    currentAxis.x = 0;
                }
            }
            if (isNegativeRollY)
            {
                if (currentAxis.y < -10)
                {
                    currentAxis.y += Time.deltaTime * 100 / 3;
                }
                else if (currentAxis.y > -10)
                {
                    currentAxis.y = -10;
                }
            }
            else
            {
                if (currentAxis.y > -10)
                {
                    currentAxis.y -= Time.deltaTime * 100 / 3;
                }
                else if (currentAxis.y < -10)
                {
                    currentAxis.y = -10;
                }
            }
            transform.Rotate(currentAxis * Time.deltaTime, Space.World);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isRollBall = false;
        }
    }

    public void RollBall()
    {
        isRollBall = true;
    }

    public void ResetAxis()
    {
        currentAxis = axis;
    }

    public void ResetRotate()
    {
        transform.eulerAngles = new Vector3(0, 90, 0);
    }
}
