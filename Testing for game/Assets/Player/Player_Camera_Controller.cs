using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player_Camera_Controller : MonoBehaviour
{
    //Hidden set up variables
    private Camera CameraComponent;
    private Transform PlayerCamera;
    private Player_Controller Character1Controller;
    private Player_Controller Character2Controller;
    private Rigidbody2D Character1LowerBody;
    private Rigidbody2D Character2LowerBody;

    //Public camera variables
    [Tooltip("Makes the camera stop following both players (For panning the camera)")]
    public Boolean FollowOverride;
    [Tooltip("Camera zoom from player")]
    public float CameraZoom;
    [Tooltip("Character 1 gameobject")]
    public GameObject Character1;
    [Tooltip("Character 2 gameobject")]
    public GameObject Character2;
    [Tooltip("Offset of camera from the player")]
    public Vector2 CameraOffset;

    void Start()
    {
        foreach (Transform child in Character1.transform)
        {
            if (child.name == "Lower Body")
            {
                Character1LowerBody = child.GetComponent<Rigidbody2D>();
            }
        }
        foreach (Transform child in Character2.transform)
        {
            if (child.name == "Lower Body")
            {
                Character2LowerBody = child.GetComponent<Rigidbody2D>();
            }
        }
        CameraComponent = GetComponent<Camera>();
        PlayerCamera = GetComponent<Transform>();
        Character1Controller = Character1.GetComponent<Player_Controller>();
        Character2Controller = Character2.GetComponent<Player_Controller>();
    }

    void Update()
    {
        //Set Player Camera Size.
        CameraComponent.orthographicSize = CameraZoom;

        //Player 1 Camera Follow.
        if (Character1Controller.MainPlayer == true && FollowOverride == false)
        {
            PlayerCamera.transform.position = (new Vector3(Character1LowerBody.transform.position.x + CameraOffset.x, Character1LowerBody.transform.position.y + CameraOffset.y, -10));
        }

        //Player 1 Camera Follow.
        if (Character2Controller.MainPlayer == true && FollowOverride == false)
        {
            PlayerCamera.transform.position = (new Vector3(Character2LowerBody.transform.position.x + CameraOffset.x, Character2LowerBody.transform.position.y + CameraOffset.y, -10));
        }
    }
}
