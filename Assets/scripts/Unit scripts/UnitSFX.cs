using UnityEngine;
using System.Collections;

public class UnitSFX : MonoBehaviour {

	bool initialized = false;

	public AudioClip AttackSFX;
	AudioClip DieSFX;
	AudioClip StepSFX;
	AudioSource audioSource;

    void Start() {
        useGUILayout = false;
    }
    
	void Initialize() {
		if (initialized) return;
		initialized = true;
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void SetSFX (string attackSFX, string dieSFX, string stepSFX) {
		AttackSFX = Resources.Load<AudioClip>(attackSFX);
		DieSFX = Resources.Load<AudioClip>(dieSFX);
		StepSFX = Resources.Load<AudioClip>(stepSFX);
	}

	public void PlayAttackSFX() {
		Initialize();
		audioSource.clip = AttackSFX;
		audioSource.Play();
	}

	public void PlayDieSFX() {
		Initialize();
		audioSource.clip = DieSFX;
		audioSource.Play();
	}

	public void PlayStepSFX() {
		Initialize();
		audioSource.clip = StepSFX;
		audioSource.Play();
	}
}
