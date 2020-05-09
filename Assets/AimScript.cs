using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimScript : MonoBehaviour {
  public Transform slashIndicator;
  public float radius = 4;
  public float deadZone = 0.3f;
  public float slashDuration = .5f;

  float slashTimeRemaining = 0;
  Vector2 slashStart;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    if (slashTimeRemaining <= 0) {
      Vector2 aim = gamepad.leftStick.ReadValue();
      if (aim.magnitude >= deadZone) {
        float angle = Vector2.SignedAngle(Vector2.up, aim);
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      }

      if (gamepad.rightTrigger.wasPressedThisFrame) {
        slashTimeRemaining = slashDuration;
        slashStart = aim.normalized * radius;
        slashIndicator.gameObject.SetActive(true);
      }
    }

    if (slashTimeRemaining > 0) {
      slashIndicator.position = Vector2.Lerp(slashStart, -slashStart, (slashDuration - slashTimeRemaining) / slashDuration);
      slashTimeRemaining -= Time.deltaTime;

      if (slashTimeRemaining <= 0) {
        slashIndicator.gameObject.SetActive(false);
      }
    }
  }
}
