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
        PlayerCamera.transform.position = (new Vector3(LowerBody.transform.position.x + CameraOffset.x, LowerBody.transform.position.y + CameraOffset.y, -10));
        //Upper Body Rotation.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = LowerBody.transform.position.z;
        float Angle = ((180 / Mathf.PI) * (Mathf.Atan2(mousePos.y - LowerBody.transform.position.y, mousePos.x - LowerBody.transform.position.x))) + RotationOffset;
        UpperBody.transform.rotation = Quaternion.Euler(0, 0, Angle);
        //Player Controls.
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
            LowerBody.transform.rotation = Quaternion.Euler(0, 0, Angle);
            LowerBody.transform.position = Vector3.MoveTowards(LowerBody.transform.position, mousePos, moveSpeed * Time.deltaTime);
        }
    }
}
