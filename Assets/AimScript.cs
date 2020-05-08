using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimScript : MonoBehaviour {
  float deadZone = 0.3f;

  void FixedUpdate() {
    Gamepad gamepad = Gamepad.current;

    if (gamepad != null) {
      Vector2 aim = gamepad.leftStick.ReadValue();
      if (aim.magnitude >= deadZone) {
        float angle = Vector2.SignedAngle(Vector2.up, aim);
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      }
    }
  }
}
