using UnityEngine;

public class RotateAtRPM : MonoBehaviour
{
    [SerializeField] private float rotationsPerMinute = 150f;

    private void Update()
    {
        float rpmInDegreesPerSecond = rotationsPerMinute * 360f / 60f;
        transform.Rotate(0f, 0f, rpmInDegreesPerSecond * Time.deltaTime);
    }
}
