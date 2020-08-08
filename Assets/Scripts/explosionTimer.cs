using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionTimer : MonoBehaviour {
	// Use this for initialization
	void Start () {
        
        playSound();
        StartCoroutine(timer());
        
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
        Destroy(this);
    }

    void playSound ()
    {
        AudioSource speaker = this.gameObject.AddComponent<AudioSource>();
        AudioClip   clip = variables.explosionSounds[Random.Range(0, variables.explosionSounds.Count - 1)];
        speaker.clip = clip;
        speaker.Play(0);
    }
}
