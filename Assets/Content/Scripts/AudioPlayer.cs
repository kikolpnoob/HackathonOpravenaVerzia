using System;
using MyBox;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioResource audioResource;
    public float timer = -1f;

    private float _timer;
    private int _currentSequentialSound;
    [ButtonMethod]
    public void Play()
    {
        if (timer > 0) 
            if (_timer < timer)
                return;

        _timer = 0;
        if(audioResource != null)
            AudioManager.SpawnAudio(audioResource);
    }

    private void Update()
    {
        if (timer > 0)
            _timer += Time.deltaTime;
    }
}
