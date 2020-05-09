using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimScript : MonoBehaviour {
  public SwordScript sword;
  public Transform aimIndicator;

  public float radius = 4;
  public float angleRange = 120;
  public float deadZone = 0.3f;

  float aimAngle;
  float blockAngleModifier = 0.6f;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    if (!sword.IsSlashing()) {
      Vector2 leftAim = gamepad.leftStick.ReadValue();
      Vector2 rightAim = gamepad.rightStick.ReadValue();
      if (leftAim.magnitude >= deadZone) {
        SetAimAngle(leftAim);
      }
      else if (rightAim.magnitude >= deadZone) {
        SetAimAngle(rightAim);
      }

      if (gamepad.leftShoulder.isPressed || gamepad.rightShoulder.isPressed) {
        float blockAngle = (Mathf.Abs(aimAngle) * blockAngleModifier + angleRange * (1 - blockAngleModifier)) *
          (gamepad.leftShoulder.isPressed ? 1 : -1);
        sword.Block(blockAngle);
      }
      else if (gamepad.leftTrigger.wasPressedThisFrame || gamepad.rightTrigger.wasPressedThisFrame) {
        sword.StartSlashing(aimAngle);
      }
      else {
        sword.PositionSword(0, aimAngle);
      }
    }
  }

  void SetAimAngle(Vector2 inputAim) {
    aimAngle = Vector2.SignedAngle(Vector2.up, inputAim);
    float minAngle = (180 - angleRange) / 2;
    float maxAngle = 180 - minAngle;
    aimAngle = Mathf.Sign(aimAngle) * Mathf.Clamp(Mathf.Abs(aimAngle), minAngle, maxAngle);
    aimIndicator.position = Quaternion.AngleAxis(aimAngle, Vector3.forward) * Vector2.up * radius;
  }
}
