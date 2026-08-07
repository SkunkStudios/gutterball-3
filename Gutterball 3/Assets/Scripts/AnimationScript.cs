using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public float time;
    public ObjectAnimation[] objects;

	void Awake ()
	{
        objects = GetComponentsInChildren<ObjectAnimation>();
    }

    public void IdleAnimation()
    {
        foreach (ObjectAnimation objectAnim in objects)
        {
            objectAnim.PlayIdle();
        }
    }

    public void PlayAnimation()
    {
        foreach (ObjectAnimation objectAnim in objects)
        {
            objectAnim.PlayObject();
        }
    }

    public void SkipAnimation()
    {
        foreach (ObjectAnimation objectAnim in objects)
        {
            objectAnim.SkipObject();
        }
    }

    public void StopAnimation()
    {
        foreach (ObjectAnimation objectAnim in objects)
        {
            objectAnim.StopObject();
        }
    }
}
