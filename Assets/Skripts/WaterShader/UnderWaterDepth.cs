using UnityEngine;
using UnityEngine.Rendering;

public class UnderWaterDepth : MonoBehaviour
{
    [SerializeField] private Transform MainCamera;
    [SerializeField] private float depth = 0;
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private VolumeProfile surfacePostProcessing;
    [SerializeField] private VolumeProfile underWaterPostProcessing;


    // Update is called once per frame
    void Update()
    {
        if (MainCamera.position.y < depth)
        {
            EnebableEffects(true);
        }
        else
        {
            EnebableEffects(false);
        }
    }

    private void EnebableEffects(bool active)
    {
        if (active)
        {
            RenderSettings.fog = true;
            postProcessingVolume.profile = underWaterPostProcessing;
        }
        else
        {
            RenderSettings.fog = false;
            postProcessingVolume.profile = surfacePostProcessing;
        }
    }
}
