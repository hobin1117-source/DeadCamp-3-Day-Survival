using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [Header("아이템 움직임")]
    public float UpDown = 0.5f;
    public float UpDownSpeed = 2f;
    public float rotationSpeed = 30f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + MathF.Sin(Time.time * UpDownSpeed) * UpDown;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
