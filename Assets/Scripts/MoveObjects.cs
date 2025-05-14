using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    public GameObject player;
    public GameObject objectToMove;
    public Rigidbody objectRb;
    public Vector3 targetPosition;
    public float moveSpeed;
    public bool moveObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!moveObject)
            return;

        Vector3 targetPos = player.transform.position + player.transform.forward * 2f;
        targetPos = new Vector3(targetPos.x, objectRb.position.y, targetPos.z); // Y sabit

        Vector3 toTarget = (targetPos - objectRb.position);
        objectRb.linearVelocity = toTarget * moveSpeed;
    }

    public void ToggleObject(GameObject objectToMove_)
    {

        objectToMove = objectToMove_;
        objectRb = objectToMove.GetComponent<Rigidbody>();
        moveObject = !moveObject;
    }
}
