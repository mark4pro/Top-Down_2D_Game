﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    //Player set up.
    PlayerControls controls;
    public Transform MouseCollider;
    private Transform LowerBody;
    private Transform UpperBody;
    private Rigidbody2D rb;
    private GameObject[] Hideouts;
    private GameObject[] Inventory;
    [Tooltip("Difference in rotation of sprite relative to up direction")]
    public float RotationOffset;

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
    [Tooltip("Max inventory space")]
    public float MaxInventory = 10;

    private AILerp AIController;
    private AIDestinationSetter AIDestSet;

    private Vector2 look;
    private float moving;
    private float running;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += cntxt => look = cntxt.ReadValue<Vector2>();
        controls.Player.Move.performed += cntxt => moving = cntxt.ReadValue<float>();
        controls.Player.Move.canceled += cntxt => moving = 0;
        controls.Player.Run.performed += cntxt => running = cntxt.ReadValue<float>();
        controls.Player.Run.canceled += cntxt => running = 0;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    //Sets up references to components
    void Start()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Lower Body")
            {
                LowerBody = child.GetComponent<Transform>();
                foreach (Transform grandchild in LowerBody.transform)
                {
                    if (grandchild.name == "Upper Body")
                        UpperBody = grandchild;
                }
            }
        }
        Hideouts = GameObject.FindGameObjectsWithTag("Hideout");
        rb = GetComponent<Rigidbody2D>();
        AIController = GetComponent<AILerp>();
        AIDestSet = GetComponent<AIDestinationSetter>();
    }

    private float moveSpeed = 0;
    void Update()
    {
        //Set AI
        AIController.canMove = FollowMainPlayer;

        //Upper Body Rotation/Mouse Collider Position. (Delete When we work on the AI fighting mechanic)
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(look);
        MouseCollider.transform.position = (new Vector3(MousePos.x, MousePos.y, 1));
        float Angle = (Mathf.Rad2Deg * (Mathf.Atan2(MousePos.y - LowerBody.transform.position.y, MousePos.x - LowerBody.transform.position.x))) + RotationOffset;
        UpperBody.transform.rotation = Quaternion.Euler(0, 0, Angle);

        //Player Controls.
        if (MainPlayer == true)
        {
            //Upper Body Rotation/Mouse Collider Position. (Delete // on this portion when the AI fighting mechanic is being worked on)
            //Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //MouseCollider.transform.position = (new Vector3(MousePos.x, MousePos.y, 1));
            //float Angle = (Mathf.Rad2Deg * (Mathf.Atan2(MousePos.y - LowerBody.transform.position.y, MousePos.x - LowerBody.transform.position.x))) + RotationOffset;
            //UpperBody.transform.rotation = Quaternion.Euler(0, 0, Angle);

            //Turn off AI when main character
            ToggleAI(false);

            if (running == 1)
            {
                moveSpeed = RunSpeed;
            }
            if (running == 0)
            {
                moveSpeed = WalkSpeed;
            }
            if (moving == 1)
            {
                //Mouse displacement with relation to player body
                Vector2 MouseDisplacement = new Vector2(MousePos.x - LowerBody.transform.position.x, MousePos.y - LowerBody.transform.position.y);
                LowerBody.transform.rotation = Quaternion.Euler(0, 0, Angle);
                rb.velocity = MouseDisplacement.normalized * moveSpeed;
            }
            else
            {
                //Slow the character to a stop
                rb.velocity = Vector2.MoveTowards(rb.velocity,Vector2.zero,.1f);
            }
        }
        else //Secondary Character 
        {
            //Enable AI
            ToggleAI(true);

            //Distance from followed target
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

    /// <summary>
    /// Enable/Disable AI controller
    /// </summary>
    /// <param name="state">State to set the AI to.</param>
    void ToggleAI(bool state)
    {
        AIDestSet.enabled = state;
        AIController.enabled = state;

        //rb.bodyType = state ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
    }
}