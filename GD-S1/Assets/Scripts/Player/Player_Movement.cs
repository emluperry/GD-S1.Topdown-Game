using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Entity_Movement
{
    [Header("Player Variables")]
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    private void Update()
    {
        m_InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    protected void FixedUpdate()
    {
        if (m_IsPaused)
            return;

        ApplyMovement();

        m_Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void SetCamera(Camera cam)
    {
        m_Camera = cam;
    }
}
