using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drone : MonoBehaviour
{
    PlayerControls controls;
    public float moveFactor = 10;
    Vector2 move;
    float altitudeStick;
    private Vector2 cameraLook;
    public GameObject source;
    private GameObject uavGameobject;
    private GameObject followGameobject;

    [SerializeField] private Light detectionLight;
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.AltitudeUp.performed += ctx => altitudeStick = 1f;
        controls.Gameplay.AltitudeDown.performed += ctx => altitudeStick = -1f;

        controls.Gameplay.CameraLook.performed += ctx => cameraLook = ctx.ReadValue<Vector2>();
        controls.Gameplay.CameraLook.canceled += ctx => cameraLook = Vector2.zero;

        controls.Gameplay.AltitudeUp.canceled += ctx => altitudeStick = 0;
        controls.Gameplay.AltitudeDown.canceled += ctx => altitudeStick = 0;
    }

    void Start()
    {
        uavGameobject = GameObject.Find("UAV");
        followGameobject = GameObject.Find("Follow Target");
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        Movement();

        UpdateLightColor();

        followGameobject.transform.Rotate(-cameraLook.y, cameraLook.x, 0);

    }

    private void UpdateLightColor()
    {
        float distanceToSource = Vector3.Distance(transform.position, source.transform.position);
        // create a color between red and green based on distance to source
        Color c = Color.Lerp(Color.red, Color.green, distanceToSource / 10);
        // set the color of the light
        detectionLight.color = c;
    }

    private void Movement()
    {
        if (move.magnitude < 0.1f)
        {
            move = Vector2.zero;
        }
        Vector3 m = new Vector3(move.x, altitudeStick, move.y) * Time.deltaTime * moveFactor;
        float speed = m.magnitude;
        // transform relative to the camera x and y
        Vector3 mcam = Camera.main.transform.TransformDirection(m);
        // scale the magnitude of the movement to the speed
        mcam.y = m.y;
        mcam = mcam.normalized * speed;
        transform.Translate(mcam, Space.World);
        if (move.magnitude > 0.1f)
        {
            // rotate the drone to face the direction of movement
            // Quaternion newRotation = Quaternion.LookRotation(new Vector3(0, mcam.y, 0), Vector3.up);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(mcam.x, mcam.y, mcam.z), Vector3.up);
            newRotation = Quaternion.Euler(newRotation.eulerAngles.x + 20, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
            uavGameobject.transform.rotation = Quaternion.Lerp(uavGameobject.transform.rotation, newRotation, Time.deltaTime * 10);
        }
        else
        {
            // flatten the drone
            Quaternion newRotation = Quaternion.Euler(0, uavGameobject.transform.rotation.eulerAngles.y, 0);
            uavGameobject.transform.rotation = Quaternion.Lerp(uavGameobject.transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }
}
