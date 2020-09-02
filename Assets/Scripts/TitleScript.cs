using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScript : MonoBehaviour {
  public GameObject[] toActivate;
  public GameObject[] toDeactivate;

  void Start() {
    Time.timeScale = 0;
  }

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    if (
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
    ) {
      foreach (GameObject obj in toActivate) {
        obj.SetActive(true);
      }
      foreach (GameObject obj in toDeactivate) {
        obj.SetActive(false);
      }

      Time.timeScale = 1;
    }
  }
}
