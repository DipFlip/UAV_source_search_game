using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessChanger : MonoBehaviour
{
    public VolumeProfile defaultProfile;
    public VolumeProfile newProfile;

    private Volume volume;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        if (volume == null)
        {
            Debug.LogError("PostProcessChanger requires a Volume component.");
        }
    }

    public void ChangePostProcessData()
    {
        if (volume != null && newProfile != null)
        {
            volume.profile = newProfile;
        }
        else
        {
            Debug.LogWarning("PostProcessChanger: ChangePostProcessData called, but either the Volume component is missing or the newProfile field is not assigned.");
        }
    }

    public void ResetPostProcessData()
    {
        if (volume != null && defaultProfile != null)
        {
            volume.profile = defaultProfile;
        }
        else
        {
            Debug.LogWarning("PostProcessChanger: ResetPostProcessData called, but either the Volume component is missing or the defaultProfile field is not assigned.");
        }
    }
}
