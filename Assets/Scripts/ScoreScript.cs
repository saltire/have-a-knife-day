using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour {
  public GameObject score;
  public GameObject loseText;
  public GameObject continueText;
  public bool isEnemy;
  public float gameOverCooldown = .5f;

  List<GameObject> fingers;
  bool gameOver;

  float showContinueTime;

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

    if (fingers.Count == 0) {
      loseText.SetActive(true);

      Time.timeScale = 0;
      gameOver = true;

      FindObjectOfType<InputScript>().allowInput = false;
      FindObjectOfType<EnemyFaceScript>().OnWin(!isEnemy);

      showContinueTime = Time.realtimeSinceStartup + gameOverCooldown;
    }
  }

  void Update() {
    if (gameOver) {
      if (!continueText.activeSelf && Time.realtimeSinceStartup >= showContinueTime) {
        continueText.SetActive(true);
      }

      if (continueText.activeSelf) {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null && (
          gamepad.buttonNorth.wasPressedThisFrame ||
          gamepad.buttonSouth.wasPressedThisFrame ||
          gamepad.buttonEast.wasPressedThisFrame ||
          gamepad.buttonWest.wasPressedThisFrame ||
          gamepad.leftTrigger.wasPressedThisFrame ||
          gamepad.rightTrigger.wasPressedThisFrame ||
          gamepad.leftShoulder.wasPressedThisFrame ||
          gamepad.rightShoulder.wasPressedThisFrame ||
          gamepad.selectButton.wasPressedThisFrame ||
          gamepad.startButton.wasPressedThisFrame
        )) {
          SceneManager.LoadScene("game");
        }
      }
    }
  }
}
