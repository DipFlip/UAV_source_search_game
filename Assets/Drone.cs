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
    public GameObject source;
    Light detectionLight;
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Altitude.performed += ctx => altitudeStick = ctx.ReadValue<Vector2>().y;
        controls.Gameplay.Altitude.canceled += ctx => altitudeStick = 0;
    }

    void Start()
    {
        detectionLight = transform.Find("Point Light").GetComponent<Light>();

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
        if (move.magnitude < 0.1f)
        {
            move = Vector2.zero;
        }
        Vector3 m = new Vector3(-move.x, altitudeStick, -move.y) * Time.deltaTime * moveFactor;
        transform.Translate(m, Space.World);
        if (move.magnitude > 0.1f)
        {
            // rotate the drone to face the direction of movement
            Quaternion newRotation = Quaternion.LookRotation(m);
            newRotation *= Quaternion.Euler(20, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
        else
        {
            // flatten the drone
            Quaternion newRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }

        float distanceToSource = Vector3.Distance(transform.position, source.transform.position);
        // create a color between red and green based on distance to source
        Color c = Color.Lerp(Color.red, Color.green, distanceToSource / 10);
        // set the color of the light
        detectionLight.color = c;
    }
}
