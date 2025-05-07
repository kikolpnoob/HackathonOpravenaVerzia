using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticlesAfterFinished : MonoBehaviour
{
    ParticleSystem particleSystem;
    bool startedPlaying;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem.isPlaying)
            startedPlaying = true;
        else if (startedPlaying)
            Destroy(gameObject);
    }
}
