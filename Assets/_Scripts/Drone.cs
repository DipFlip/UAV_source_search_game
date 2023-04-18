using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(100)]
public class Drone : MonoBehaviour
{
    PlayerControls controls;
    public float moveFactor = 10;
    Vector2 move;
    float altitudeStick;
    private Vector2 cameraLook;
    // public GameObject source;
    public SourceDetector sourceDetector;
    private GameObject uavGameobject;
    private GameObject followGameobject;
    private Rigidbody rb;
    public AudioSource audioSource;
    private float tweenPitch = 1f;
    [SerializeField] private Light detectionLight;
    [SerializeField] private float droneSpeed = 100f;
    [SerializeField] private float camSensitivity = 0.5f;
    [SerializeField] private PrefabSpawner spawner;
    
    void Awake()
    {
        // controls = new PlayerControls();
        controls = GameManager.Instance.controls;
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.AltitudeUp.performed += ctx => altitudeStick = 1f;
        controls.Gameplay.AltitudeDown.performed += ctx => altitudeStick = -1f;

        controls.Gameplay.CameraLook.performed += ctx => SetCameraLookWithConstraints(ctx.ReadValue<Vector2>());
        controls.Gameplay.CameraLook.canceled += ctx => cameraLook = Vector2.zero;

        controls.Gameplay.AltitudeUp.canceled += ctx => altitudeStick = 0;
        controls.Gameplay.AltitudeDown.canceled += ctx => altitudeStick = 0;
        controls.Gameplay.Flag.performed += ctx => spawner.SpawnPrefabClosest();
    }

    private void SetCameraLookWithConstraints(Vector2 cameraStick)
    {
        float x = followGameobject.transform.rotation.eulerAngles.x;
        if (x > 180) x = x - 360;
        if (x < -65 && cameraStick.y > 0) cameraStick.y = 0;
        if (x > 20 && cameraStick.y < 0) cameraStick.y = 0;
        cameraLook = cameraStick;
    }
   void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        uavGameobject = GameObject.Find("UAV");
        followGameobject = GameObject.Find("Follow Target");
        Cursor.visible = false;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    void FixedUpdate()
    {
        Movement();
    }
    void Update()
    {
        UpdateLightColor();

        followGameobject.transform.Rotate(0, cameraLook.x*camSensitivity, 0, Space.World);
        followGameobject.transform.Rotate(-cameraLook.y*camSensitivity, 0, 0, Space.Self);
    }

    private void UpdateLightColor()
    {
        GameObject closestSource = sourceDetector.GetClosestSource();

        if (closestSource != null)
        {
            float distanceToSource = Vector3.Distance(uavGameobject.transform.position, closestSource.transform.position);
            // create a color between red and green based on distance to source

            Color c = Color.Lerp(Color.red, Color.green, distanceToSource / 25);
            // set the color of the light
            detectionLight.color = c;
        }
    }


    private void Movement()
    {
        if (move.magnitude < 0.1f)
        {
            move = Vector2.zero;
        }
        Vector3 horizontalMovement = new Vector3(move.x, 0, move.y) * Time.deltaTime * moveFactor;
        Vector3 speedVector = new Vector3(move.x, altitudeStick, move.y) * Time.deltaTime * moveFactor;
        float speed = speedVector.magnitude;
        // transform relative to the camera x and y
        Vector3 mcam = Camera.main.transform.TransformDirection(horizontalMovement);
        // scale the magnitude of the movement to the speed
        mcam.y = altitudeStick * Time.deltaTime * moveFactor;
        mcam = mcam.normalized * droneSpeed;
        // transform.Translate(mcam, Space.World);
        rb.AddForce(mcam * speed);
        if (move.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(mcam.x, mcam.y, mcam.z), Vector3.up);
            newRotation = Quaternion.Euler(newRotation.eulerAngles.x + 25, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
            uavGameobject.transform.rotation = Quaternion.Lerp(uavGameobject.transform.rotation, newRotation, Time.deltaTime * 10);
        }
        else
        {
            // flatten the drone
            Quaternion newRotation = Quaternion.Euler(0, uavGameobject.transform.rotation.eulerAngles.y, 0);
            uavGameobject.transform.rotation = Quaternion.Lerp(uavGameobject.transform.rotation, newRotation, Time.deltaTime * 10);
        }
        // change sound pitch based on speed
        tweenPitch = Mathf.Lerp(tweenPitch, 1f + speed * 1.2f, 0.05f);
        audioSource.pitch = tweenPitch;
    }
}
