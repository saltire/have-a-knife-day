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
  public float slashDuration = .5f;
  public float blockDuration = 1f;

  public float swingAngleStart = -75;

  SpriteRenderer swordSpriter;

  float aimAngle;
  float blockAngleModifier = 0.6f;

  Vector2 ellipseCenter = new Vector2(0, -2);
  float ellipseRadiusX = 1;
  float ellipseRadiusY = 2;

  float slashTimeRemaining = 0;
  float blockTimeRemaining = 0;

  void Start() {
    swordSpriter = sword.GetComponent<SpriteRenderer>();
  }

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    if (slashTimeRemaining <= 0) {
      Vector2 inputAim = gamepad.leftStick.ReadValue();
      if (inputAim.magnitude >= deadZone) {
        aimAngle = Vector2.SignedAngle(Vector2.up, inputAim);
        float minAngle = (180 - angleRange) / 2;
        float maxAngle = 180 - minAngle;
        aimAngle = Mathf.Sign(aimAngle) * Mathf.Clamp(Mathf.Abs(aimAngle), minAngle, maxAngle);
        aimIndicator.position = Quaternion.AngleAxis(aimAngle, Vector3.forward) * Vector2.up * radius;
        PositionSword(0);
      }

      if (blockTimeRemaining <= 0) {
        if (gamepad.rightTrigger.wasPressedThisFrame) {
          slashTimeRemaining = slashDuration;
        }
        else if (gamepad.leftTrigger.wasPressedThisFrame) {
          blockTimeRemaining = blockDuration;
        }
      }
    }

    if (slashTimeRemaining > 0) {
      float lerpValue = (slashDuration - slashTimeRemaining) / slashDuration;
      PositionSword(lerpValue);

      slashTimeRemaining -= Time.deltaTime;
    }

    if (blockTimeRemaining > 0) {
      float blockAngle = aimAngle * blockAngleModifier + Mathf.Sign(aimAngle) * angleRange * (1 - blockAngleModifier);
      PositionSword(0, blockAngle);
      sword.position -= new Vector3(sword.position.x, 0, 0);
      sword.localScale *= new Vector2(-1, 1);
      sword.localRotation = Quaternion.Euler(0, 0, -sword.localRotation.eulerAngles.z);

      blockTimeRemaining -= Time.deltaTime;
    }
  }

  void PositionSword(float lerpValue) {
    PositionSword(lerpValue, aimAngle);
  }

  void PositionSword(float lerpValue, float angle) {
    if (angle < 0) {
      lerpValue = 1 - lerpValue;
    }

    float swingAngle = Mathf.Lerp(-swingAngleStart, swingAngleStart, lerpValue);
    float swingAngleRad = (swingAngle + 90) * Mathf.Deg2Rad;

    Vector2 handlePoint = ellipseCenter + new Vector2(ellipseRadiusY * Mathf.Cos(swingAngleRad), ellipseRadiusX * Mathf.Sin(swingAngleRad));
    float swordHeight = -handlePoint.y;
    float swordWidth = swordHeight * Mathf.Tan(-swingAngle * Mathf.Deg2Rad);
    Vector2 swordPoint = handlePoint + new Vector2(swordWidth, swordHeight);

    Quaternion rotation = Quaternion.AngleAxis(angle - 90 * Mathf.Sign(aimAngle), Vector3.forward);
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
