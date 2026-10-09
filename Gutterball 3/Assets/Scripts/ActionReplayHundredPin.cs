using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplayHundredPin : MonoBehaviour
{
    public List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();
    private GameHundredPin game;
    private Rigidbody rigidBody;
    private Vector3 velocity;
    private Vector3 angularVelocity;

    void Start()
    {
        game = GameObject.FindObjectOfType<GameHundredPin>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!rigidBody.isKinematic)
        {
            velocity = rigidBody.velocity;
            angularVelocity = rigidBody.angularVelocity;
        }
    }

    public void RigidBodyCollider(bool isCollider)
    {
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = isCollider;
        }
    }

    public void RigidBodyFreeze(bool isFreeze)
    {
        rigidBody.isKinematic = isFreeze;
    }

    public void Add()
    {
        actionReplayRecords.Add(new ActionReplayRecord { position = transform.position, rotation = transform.rotation });
    }

    public void Clear()
    {
        actionReplayRecords.Clear();
    }

    public void SetTransform(float index)
    {
        game.currentReplayIndex = index;

        ActionReplayRecord record = actionReplayRecords[(int)index];

        transform.position = record.position;
        transform.rotation = record.rotation;
    }

    public void SetVelocity()
    {
        rigidBody.velocity = velocity;
        rigidBody.angularVelocity = angularVelocity;
    }
}
