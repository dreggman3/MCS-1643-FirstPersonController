using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float strafeSpeed = 1.5f;
    public float yawSpeed = 260.0f;
    public float pitchSpeed = 260.0f;
    public float jumpForce = 6.0f;
    public Transform groundRef;
    public float maxPitch = 70.0f;
    public float minPitch = -70.0f;

    public GameObject Weapon1;
    public GameObject Weapon2;

    private Rigidbody rb;
    private int currentWeapon;
    private float pitch = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze rotation so physics forces don’t spin the player
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentWeapon = 1;
        Weapon1.SetActive(true);
        Weapon2.SetActive(false);
    }

    void Update()
    {
        // Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        float yaw = Input.GetAxis("Mouse X");
        float pitchInput = Input.GetAxis("Mouse Y");

        // --- Movement ---
        Vector3 move = (transform.right * h * strafeSpeed + transform.forward * v * moveSpeed);
        rb.MovePosition(rb.position + move * Time.deltaTime);

        // --- Rotation (Yaw) ---
        Quaternion deltaRotation = Quaternion.Euler(0f, yaw * yawSpeed * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // --- Camera Pitch ---
        pitch -= pitchInput * pitchSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Camera.main.transform.localEulerAngles = new Vector3(
            pitch,
            0f,
            0f);

        // --- Jump ---
        if (jump && Physics.Raycast(groundRef.position, Vector3.down, 0.1f))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // --- Weapon Switching ---
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != 1)
        {
            currentWeapon = 1;
            Weapon1.SetActive(true);
            Weapon2.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != 2)
        {
            currentWeapon = 2;
            Weapon1.SetActive(false);
            Weapon2.SetActive(true);
        }
    }
}

