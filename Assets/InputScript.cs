using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour {
  public SwordScript sword;
  public Transform aimIndicator;
  public Transform leftShoulder;
  public Transform rightShoulder;

  public float moveAmount = 1;
  public float moveDuration = 2;
  Vector2 moveTarget;
  Vector2 moveVelocity = Vector2.zero;

  public float radius = 4;
  public float angleRange = 120;
  public float deadZone = 0.3f;
  float aimAngle;
  float blockAngleModifier = 0.6f;

  ArmScript arm;

  void Start() {
    arm = GetComponent<ArmScript>();
  }

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    Vector2? stickInput = null;
    Vector2 leftStick = gamepad.leftStick.ReadValue();
    if (leftStick.magnitude >= deadZone) {
      stickInput = leftStick;
      arm.shoulder = leftShoulder;
    }
    else {
      Vector2 rightStick = gamepad.rightStick.ReadValue();
      if (rightStick.magnitude >= deadZone) {
        stickInput = rightStick;
        arm.shoulder = rightShoulder;
      }
    }

    moveTarget = stickInput != null ? stickInput.Value * moveAmount : Vector2.zero;
    transform.position = Vector2.SmoothDamp(transform.position, moveTarget, ref moveVelocity, moveDuration);

    if (!sword.IsSlashing()) {
      if (stickInput != null) {
        SetAimAngle(stickInput.Value);
      }

      if ((arm.shoulder == leftShoulder && gamepad.leftShoulder.isPressed) ||
        (arm.shoulder == rightShoulder && gamepad.rightShoulder.isPressed)) {
        float blockAngle = (Mathf.Abs(aimAngle) * blockAngleModifier + angleRange * (1 - blockAngleModifier)) *
          (gamepad.leftShoulder.isPressed ? 1 : -1);
        sword.Block(blockAngle);
      }
      else if ((arm.shoulder == leftShoulder && gamepad.leftTrigger.wasPressedThisFrame) ||
        (arm.shoulder == rightShoulder && gamepad.rightTrigger.wasPressedThisFrame)) {
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
    aimIndicator.localPosition = Quaternion.AngleAxis(aimAngle, Vector3.forward) * Vector2.up * radius;
  }
}
