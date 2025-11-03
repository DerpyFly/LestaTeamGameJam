using UnityEngine;
using UnityEngine.Audio;
public class SoundNearToPlayer : MonoBehaviour
{


    
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private AudioMixer mixer;
        

        [Header("Distance Settings")]
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 8f;
        [SerializeField] private AudioSource source;
        //[SerializeField] private string mixerParam = "AmbientVolume";

    [Header("Fade Settings")]
        [SerializeField] private float fadeSpeed = 2f; // скорость плавного перехода

        private float currentVolume01 = 0f;  
        private float targetVolume01 = 0f;   

    private void Start()
    {
        
        source.spatialBlend = 1f;      
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
    }
    private void Update()
        {
            float dist = Vector3.Distance(player.position, transform.position);

            
            if (dist < maxDistance)
            {
                targetVolume01 = Mathf.InverseLerp(maxDistance, minDistance, dist);
            }
            else
            {
                targetVolume01 = 0f;
            
            }

            
            currentVolume01 = Mathf.Lerp(currentVolume01, targetVolume01, Time.deltaTime * fadeSpeed);

        
        //float dB = Mathf.Lerp(-80f, 0f, currentVolume01);
        //mixer.SetFloat(mixerParam, dB);

        source.minDistance = minDistance;
        source.maxDistance = maxDistance;

    }
    

}
