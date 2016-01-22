using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicController : MonoBehaviour {

	public List<AudioClip> MUSICTRACKS;

	void Start() {
		int i = Random.Range(0, MUSICTRACKS.Count);
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = MUSICTRACKS[i];
		audioSource.Play();
	}
}
