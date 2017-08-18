using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour { 
    public AudioSource FxSource;
    public AudioSource MusicSource;
    public static SoundManager Instance = null;
    public float LowPitchRange = 0.95f;
    public float HighPitchRange = 1.05f;

    public void PlaySingle(AudioClip clip)
    {
        FxSource.clip = clip;
        FxSource.Play();
    }

    public void RandomFx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        FxSource.pitch = randomPitch;
        FxSource.clip = clips[randomIndex];
        FxSource.Play();
    }
    
	void Awake () {
		if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}
}
