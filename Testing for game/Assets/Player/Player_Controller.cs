using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Player_Controller : MonoBehaviour
{
    //Player set up.
    private Camera CameraComponent;
    public Transform PlayerCamera;
    private Rigidbody2D LowerBody;
    private Transform UpperBody;
    public float RotationOffset;
    public Vector2 CameraOffset;
    public float CameraZoom;

    //Player variables.
    public Boolean MainPlayer = false;
    public Boolean FollowMainPlayer = false;
    public Boolean DockedAtHideout = true;
    public float NormalSpeed;
    public float RunSpeed;

    private AILerp AIController;

    //Sets up health and shit...
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
    }

    private float moveSpeed = 0;
    void Update()
    {
        //Set Player Camera Size.
        CameraComponent.orthographicSize = CameraZoom;

        //Player Camera Follow.
        if (MainPlayer == true)
        {
            PlayerCamera.transform.position = (new Vector3(LowerBody.transform.position.x + CameraOffset.x, LowerBody.transform.position.y + CameraOffset.y, -10));
        }

        //Player Controls.
        if (MainPlayer == true)
        {
            //Upper Body Rotation.
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float Angle = ((180 / Mathf.PI) * (Mathf.Atan2(MousePos.y - LowerBody.transform.position.y, MousePos.x - LowerBody.transform.position.x))) + RotationOffset;
            UpperBody.transform.rotation = Quaternion.Euler(0, 0, Angle);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = RunSpeed;
            }
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = NormalSpeed;
            }
            if (Input.GetMouseButton(0))
            {
                //Mouse displacement with relation to player body
                Vector2 MouseDisplacement = new Vector2(MousePos.x - LowerBody.transform.position.x, MousePos.y - LowerBody.transform.position.y);
                LowerBody.transform.rotation = Quaternion.Euler(0, 0, Angle);
                LowerBody.velocity = MouseDisplacement.normalized * moveSpeed;

                //LowerBody.transform.position = Vector3.MoveTowards(LowerBody.transform.position, mousePos, moveSpeed * Time.deltaTime);
            }
            else
            {
                LowerBody.velocity = Vector2.zero;
            }
        }
        else //Secondary Character 
        {
             AIController.canMove = FollowMainPlayer;
        }
    }
}
