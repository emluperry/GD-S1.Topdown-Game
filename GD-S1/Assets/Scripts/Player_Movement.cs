using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Entity_Movement
{
    [Header("Player Variables")]
    [SerializeField] private Camera m_Camera;

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    protected void FixedUpdate()
    {
        ApplyMovement();

        m_Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
