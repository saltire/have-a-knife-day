using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimScript : MonoBehaviour {
  public Transform sword;
  public Transform aimIndicator;
  public Sprite[] swordSprites;

  public float radius = 4;
  public float angleRange = 120;
  public float deadZone = 0.3f;
  // public Vector2 startPosition = new Vector2(-20, -10);
  // public float raiseDuration = .5f;
  public float slashDuration = .5f;
  // public float dropDuration = .5f;
  // public Vector2 stopPosition = new Vector2(20, -10);

  public float swingAngleStart = -75;

  SpriteRenderer swordSpriter;

  bool aimEnabled = true;
  float aimAngle;
  Quaternion rotation;

  Vector2 ellipseCenter = new Vector2(0, -2);
  float ellipseRadiusX = 1;
  float ellipseRadiusY = 2;

  float raiseTimeRemaining = 0;
  float slashTimeRemaining = 0;
  float dropTimeRemaining = 0;

  void Start() {
    swordSpriter = sword.GetComponent<SpriteRenderer>();
  }

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    if (aimEnabled) {
      Vector2 inputAim = gamepad.leftStick.ReadValue();
      if (inputAim.magnitude >= deadZone) {
        aimAngle = Vector2.SignedAngle(Vector2.up, inputAim);
        float minAngle = (180 - angleRange) / 2;
        float maxAngle = 180 - minAngle;
        aimAngle = Mathf.Sign(aimAngle) * Mathf.Clamp(Mathf.Abs(aimAngle), minAngle, maxAngle);
        rotation = Quaternion.AngleAxis(aimAngle - 90 * Mathf.Sign(aimAngle), Vector3.forward);
        aimIndicator.position = Quaternion.AngleAxis(aimAngle, Vector3.forward) * Vector2.up * radius;
        PositionSword(0);
      }

      if (gamepad.rightTrigger.wasPressedThisFrame) {
        aimEnabled = false;
        slashTimeRemaining = slashDuration;
        swordSpriter.sprite = swordSprites[aimAngle < 0 ? swordSprites.Length - 1 : 0];
      }
    }
    else {
      // if (raiseTimeRemaining > 0) {
      //   float lerpValue = (raiseDuration - raiseTimeRemaining) / raiseDuration;
      //   PositionSword(0);
      //   slashIndicator.position = Vector2.Lerp(startPosition, slashIndicator.position, lerpValue);

      //   raiseTimeRemaining -= Time.deltaTime;
      //   if (raiseTimeRemaining <= 0) {
      //     slashTimeRemaining = slashDuration;
      //   }
      // }
      if (slashTimeRemaining > 0) {
        float lerpValue = (slashDuration - slashTimeRemaining) / slashDuration;
        PositionSword(lerpValue);

        slashTimeRemaining -= Time.deltaTime;
        if (slashTimeRemaining <= 0) {
          // dropTimeRemaining = dropDuration;
          aimEnabled = true;
        }
      }
      // else if (dropTimeRemaining > 0) {
      //   float lerpValue = (dropDuration - dropTimeRemaining) / dropDuration;
      //   PositionSword(1);
      //   slashIndicator.position = Vector2.Lerp(slashIndicator.position, stopPosition, lerpValue);

      //   dropTimeRemaining -= Time.deltaTime;
      //   if (dropTimeRemaining <= 0) {
      //     aimEnabled = true;
      //   }
      // }
    }
  }

  void PositionSword(float lerpValue) {
    if (aimAngle < 0) {
      lerpValue = 1 - lerpValue;
    }

    float swingAngle = Mathf.Lerp(-swingAngleStart, swingAngleStart, lerpValue);
    float swingAngleRad = (swingAngle + 90) * Mathf.Deg2Rad;

    Vector2 handlePoint = ellipseCenter + new Vector2(ellipseRadiusY * Mathf.Cos(swingAngleRad), ellipseRadiusX * Mathf.Sin(swingAngleRad));
    float swordHeight = -handlePoint.y;
    float swordWidth = swordHeight * Mathf.Tan(-swingAngle * Mathf.Deg2Rad);
    Vector2 swordPoint = handlePoint + new Vector2(swordWidth, swordHeight);

    handlePoint = rotation * handlePoint;
    swordPoint = rotation * swordPoint;
    // Debug.DrawLine(handlePoint, swordPoint);

    // slashIndicator.position = Vector2.Lerp(aimIndicator.position, -aimIndicator.position, lerpValue);
    sword.position = swordPoint;
    sword.localRotation = rotation;

    swordSpriter.sprite = swordSprites[Mathf.Min((int)(lerpValue * swordSprites.Length), swordSprites.Length - 1)];
    float spriteHeight = swordSpriter.sprite.rect.height / swordSpriter.sprite.pixelsPerUnit;
    sword.localScale = Vector2.one * swordHeight / spriteHeight;
  }
}
