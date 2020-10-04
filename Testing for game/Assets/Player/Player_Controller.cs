﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Player_Controller : MonoBehaviour
{
    //Player set up.
    public Transform MouseCollider;
    private Camera CameraComponent;
    public Transform PlayerCamera;
    private Rigidbody2D LowerBody;
    private Transform UpperBody;
    [Tooltip("Difference in rotation of sprite relative to up direction")]
    public float RotationOffset;
    [Tooltip("Offset of camera from the player")]
    public Vector2 CameraOffset;
    [Tooltip("Camera zoom from player")]
    public float CameraZoom;

    //Player variables.
    [Tooltip("Whether this is the controlled player")]
    public Boolean MainPlayer = false;
    [Tooltip("Whether this character is following the main player")]
    public Boolean FollowMainPlayer = false;
    [Tooltip("Whether this character is docked at hideout")]
    public Boolean DockedAtHideout = true;
    [Tooltip("Normal movement speed of player")]
    public float WalkSpeed = 1;
    [Tooltip("Run speed of the player")]
    public float RunSpeed = 2;
    [Tooltip("Slow walk speed as a percentage of walk speed")]
    public float SlowWalkSpeed = 0.8f;
    [Tooltip("Run/Walk distance threshold for secondary character to swap from run to walk speed and vice versa")]
    public float RunWalkSwap = 2;
    [Tooltip("Walk/Slow Walk distance threshold for secondary character to swap from walk to slow walk speed and vice versa")]
    public float WalkSlowSwap = 0.5f;
    [Tooltip("Global modifier for secondary player's speed")]
    public float SecPlayerSpeedModifier = 0.8f;

    private AILerp AIController;
    private AIDestinationSetter AIDestSet;

    //Sets up references to components
    void Start()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Lower Body")
            {
                LowerBody = child.GetComponent<Rigidbody2D>();
                foreach (Transform grandchild in LowerBody.transform)
                {
                    if (grandchild.name == "Upper Body")
                        UpperBody = grandchild;
                }
            }
        }
        CameraComponent = PlayerCamera.GetComponent<Camera>();
        AIController = GetComponent<AILerp>();
        AIDestSet = GetComponent<AIDestinationSetter>();
    }

    private float moveSpeed = 0;
    void Update()
    {
        //Set Player Camera Size.
        CameraComponent.orthographicSize = CameraZoom;

        //Set Player Rigid Body To Static.
        if (MainPlayer == true)
        {
            LowerBody.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            LowerBody.bodyType = RigidbodyType2D.Static;
        }

        //Player Camera Follow.
        if (MainPlayer == true)
        {
            PlayerCamera.transform.position = (new Vector3(LowerBody.transform.position.x + CameraOffset.x, LowerBody.transform.position.y + CameraOffset.y, -10));
        }

        //Upper Body Rotation/Mouse Collider Position.
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (MainPlayer == true)
        {
            MouseCollider.transform.position = (new Vector3(MousePos.x, MousePos.y, 1));
        }
        //MousePos.z = LowerBody.transform.position.z;
        float Angle = ((180 / Mathf.PI) * (Mathf.Atan2(MousePos.y - LowerBody.transform.position.y, MousePos.x - LowerBody.transform.position.x))) + RotationOffset;
        UpperBody.transform.rotation = Quaternion.Euler(0, 0, Angle);

        //Set AI
        AIController.canMove = FollowMainPlayer;

        //Player Controls.
        if (MainPlayer == true)
        {

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = RunSpeed;
            }
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = WalkSpeed;
            }
            if (Input.GetMouseButton(0))
            {
                //Mouse displacement with relation to player body
                Vector2 MouseDisplacement = new Vector2(MousePos.x - LowerBody.transform.position.x, MousePos.y - LowerBody.transform.position.y);
                LowerBody.transform.rotation = Quaternion.Euler(0, 0, Angle);
                LowerBody.velocity = MouseDisplacement.normalized * moveSpeed;
            }
            else
            {
                //Slow the character to a stop
                LowerBody.velocity = Vector2.MoveTowards(LowerBody.velocity,Vector2.zero,.1f);
            }
        }
        else //Secondary Character 
        {
            float distanceFromTarget = Vector3.Distance(transform.position, AIDestSet.target.transform.position);

            //Slow/Speed character based on distance from target
            if (distanceFromTarget > RunWalkSwap)
                AIController.speed = SecPlayerSpeedModifier * RunSpeed;
            else if (distanceFromTarget < WalkSlowSwap)
                AIController.speed = SecPlayerSpeedModifier * SlowWalkSpeed;
            else
                AIController.speed = SecPlayerSpeedModifier * WalkSpeed;
        }
    }
}
