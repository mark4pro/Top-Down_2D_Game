using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Player set up.
    public Camera CameraComponent;
    public Transform PlayerCamera;
    public Rigidbody2D LowerBody;
    public Transform UpperBody;
    public float RotationOffset;
    public Vector2 CameraOffset;
    public float CameraZoom;

    //Player variables.
    public Boolean MainPlayer = false;
    public Boolean FollowMainPlayer = false;
    public Boolean DockedAtHideout = true;
    public float NormalSpeed;
    public float RunSpeed;

    //Sets up health and shit...
    void Start()
    {

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

        //Upper Body Rotation.
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //MousePos.z = LowerBody.transform.position.z;
        float Angle = ((180 / Mathf.PI) * (Mathf.Atan2(MousePos.y - LowerBody.transform.position.y, MousePos.x - LowerBody.transform.position.x))) + RotationOffset;
        UpperBody.transform.rotation = Quaternion.Euler(0, 0, Angle);

        //Player Controls.
        if (MainPlayer == true)
        {
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
    }
}
