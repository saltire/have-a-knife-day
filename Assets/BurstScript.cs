using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstScript : MonoBehaviour {
  public float fadeDuration = .2f;

  float fadeRemaining = 0;

  void Start() {
    fadeRemaining = fadeDuration;
  }

  void Update() {
    fadeRemaining -= Time.deltaTime;

    if (fadeRemaining <= 0) {
      Destroy(gameObject);
    }
  }
}
