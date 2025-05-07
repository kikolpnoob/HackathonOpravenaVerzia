using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticlesOnDestroy : MonoBehaviour
{
    public ParticleSystem deathBlood;
    void OnDestroy()
    {
        if (SceneManager.GetActiveScene().name == "Main")
            Instantiate(deathBlood, transform.position, Quaternion.identity, null).gameObject.SetActive(true);
    }
}
