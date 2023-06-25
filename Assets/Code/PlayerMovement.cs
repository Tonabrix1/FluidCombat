using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody body;
    PlayerStatus player;
    public float speed = 5f, topSpeed = 5f, jumpSpeed = 5f, jumpCost = 5f, dashSpeed = 5f, dashCost = 5f, dashCD = 1.5f, jumpCD = 1f;
    public float mouseSensitivityX = 1f, mouseSensitivityY = 1f, maxAngle = 345f, minAngle = 45f, minZoom = 20f, maxZoom = 100f, zoomSpeed = 250f;
    float dshCD = 0f, jmpCD = 0f;
    bool dashing;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        player = gameObject.GetComponent<PlayerStatus>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Vector3 movement = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward;
        movement = movement * speed;
        body.AddForce(movement, ForceMode.Acceleration);
        JumpPoll();
        if (!dashing) body.velocity = Vector3.ClampMagnitude(body.velocity, topSpeed);
        DashPoll(movement);
        ControlCamera();
    }

    void JumpPoll() {
        if (jmpCD <= 0 && Input.GetAxis("Jump") != 0f && player.CanUse(jumpCost)) {
            jmpCD = jumpCD;
            body.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            player.Use(jumpCost);
        } else jmpCD -= Time.fixedDeltaTime;
    }

    IEnumerator Dash(Vector3 movement) {
        dashing = true;
        body.velocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(.2f);
        body.AddForce(new Vector3(movement.x * dashSpeed, movement.y, movement.z * dashSpeed), ForceMode.Force);
        yield return new WaitForSecondsRealtime(1f);
        body.velocity = Vector3.zero;
        dashing = false;
    }

    void DashPoll(Vector3 movement) {
        if (dshCD <= 0 && Input.GetAxis("Dash") != 0f && player.CanUse(dashCost)) {
            dshCD = dashCD;
            StartCoroutine(Dash(movement));
            player.Use(dashCost);
        } else dshCD -= Time.fixedDeltaTime;
    }

    void ControlCamera() {
        float deltaX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX, deltaY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY, deltaZoom = Input.GetAxis("Mouse ScrollWheel");
        transform.RotateAround(transform.position, Vector3.up, deltaX);
        Transform cam = Camera.main.transform;
        if (cam.eulerAngles.x - deltaY > maxAngle || cam.eulerAngles.x - deltaY < minAngle) cam.RotateAround(cam.position, transform.right, -deltaY);
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + -deltaZoom * zoomSpeed, minZoom, maxZoom);
    }
}
