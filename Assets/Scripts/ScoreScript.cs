using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour {
  public GameObject score;

  List<GameObject> fingers;

  void Start() {
    fingers = new List<GameObject>();
    foreach (Transform finger in score.transform) {
      fingers.Add(finger.gameObject);
    }
  }

  public void LoseFinger() {
    if (fingers.Count > 0) {
      GameObject finger = fingers[Random.Range(0, fingers.Count)];
      fingers.Remove(finger);
      finger.SetActive(false);
    }
  }
}
