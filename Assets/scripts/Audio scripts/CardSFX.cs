using UnityEngine;
using System.Collections;

public class CardSFX : MonoBehaviour {

	bool initialized;

	public AudioClip DRAWCARDSFX;
	public AudioClip PLAYCARDSFX;

	AudioSource audioSource;

	void Initialize() {
		if (initialized) return;
		initialized = true;

		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void PlayDrawCardSFX() {
		Initialize();
		audioSource.clip = DRAWCARDSFX;
		audioSource.Play();
	}

	public void PlayPlayCardSFX() {
		Initialize();
		audioSource.clip = PLAYCARDSFX;
		audioSource.Play();
	}
}
