using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    private Vector3 topDownOffset = new Vector3(0, 10, 0); // Offset relative to the player
    private Quaternion topDownFixedRotation = Quaternion.Euler(90, 0, 0); // Looking straight down

    private bool isMouseDisabled = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Start in the Topdown style if required
        SwitchCameraStyle(CameraStyle.Topdown);
    }

    private void Update()
    {
        if (currentStyle == CameraStyle.Topdown)
        {
            // Lock camera position and orientation relative to the player
            transform.position = player.position + topDownOffset;
            transform.rotation = topDownFixedRotation;

            // Disable mouse input
            if (!isMouseDisabled)
            {
                DisableMouseInput();
            }

            return; // Skip other logic
        }
        else
        {
            // Re-enable mouse input if switching away from Topdown
            if (isMouseDisabled)
            {
                EnableMouseInput();
            }

            // Allow switching styles
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);
        }

        // Rotate orientation for other styles
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate player object
        if (currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown)
        {
            topDownCam.SetActive(true);
            // Immediately fix the camera position and rotation
            transform.position = player.position + topDownOffset;
            transform.rotation = topDownFixedRotation;
        }

        currentStyle = newStyle;
    }

    private void DisableMouseInput()
    {
        Cursor.lockState = CursorLockMode.Confined; // Mouse confined to screen
        Cursor.visible = true;                     // Make the cursor visible
        isMouseDisabled = true;
    }

    private void EnableMouseInput()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse to the screen center
        Cursor.visible = false;                  // Hide the cursor
        isMouseDisabled = false;
    }
}
