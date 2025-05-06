using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static GameObject SpawnAudio(AudioResource resource, bool loop = false, Vector3 position = new Vector3(), float distance = 0)
    {
        GameObject g = new GameObject($"SoundCue: {resource.name}");
        AudioSource a = g.AddComponent<AudioSource>();
        a.resource = resource;
        a.transform.position = position;
        a.loop = loop;
        if (distance > 0)
        {
            a.spatialBlend = 1;
            a.maxDistance = distance;
        }
        a.Play();
        Destroy(g, 3);
        return g;
    }
    public static GameObject SpawnAudio(AudioResource resource, float pitch, bool loop = false, Vector3 position = new Vector3(), float distance = 0)
    {
        GameObject g = new GameObject($"SoundCue: {resource.name}");
        AudioSource a = g.AddComponent<AudioSource>();
        a.resource = resource;
        a.transform.position = position;
        a.loop = loop;
        a.pitch = pitch;
        if (distance > 0)
        {
            a.spatialBlend = 1;
            a.maxDistance = distance;
        }
        a.Play();
        Destroy(g, 3);
        return g;
    }
}
