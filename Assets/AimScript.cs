using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimScript : MonoBehaviour {
  public Transform slashIndicator;
  public Transform aimIndicator;
  public Sprite[] swordSprites;

  public float radius = 4;
  public float angleRange = 120;
  public float deadZone = 0.3f;
  public float slashDuration = .5f;

  public float swordAngleStart = -75;

  float minAngle;
  float maxAngle;
  float angle;

  float ellipseOffsetY = 2;
  float ellipseRadiusX = 1;
  float ellipseRadiusY = 2;

  float slashTimeRemaining = 0;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    if (slashTimeRemaining <= 0) {
      slashIndicator.gameObject.SetActive(false);

      Vector2 inputAim = gamepad.leftStick.ReadValue();
      if (inputAim.magnitude >= deadZone) {
        angle = Vector2.SignedAngle(Vector2.up, inputAim);
        float minAngle = (180 - angleRange) / 2;
        float maxAngle = 180 - minAngle;
        angle = Mathf.Sign(angle) * Mathf.Clamp(Mathf.Abs(angle), minAngle, maxAngle);
        aimIndicator.position = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.up * radius;
      }

      if (gamepad.rightTrigger.wasPressedThisFrame) {
        slashTimeRemaining = slashDuration;
        slashIndicator.gameObject.SetActive(true);
      }
    }

    if (slashTimeRemaining > 0) {
      float lerpValue = (slashDuration - slashTimeRemaining) / slashDuration;
      slashTimeRemaining -= Time.deltaTime;

      Vector2 ellipseCenter = Vector2.down * ellipseOffsetY;

      float swordAngle = Mathf.Lerp(-swordAngleStart, swordAngleStart, lerpValue);
      float swordAngleRad = (swordAngle + 90) * Mathf.Deg2Rad;
      Vector2 handlePoint = ellipseCenter + new Vector2(ellipseRadiusY * Mathf.Cos(swordAngleRad), ellipseRadiusX * Mathf.Sin(swordAngleRad));
      float swordHeight = -handlePoint.y;
      float swordWidth = swordHeight * Mathf.Tan(-swordAngle * Mathf.Deg2Rad);
      Vector2 swordPoint = handlePoint + new Vector2(swordWidth, swordHeight);

      Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
      handlePoint = rotation * handlePoint;
      swordPoint = rotation * swordPoint;

      // slashIndicator.position = Vector2.Lerp(aimIndicator.position, -aimIndicator.position, lerpValue);

      // Debug.DrawLine(handlePoint, swordPoint);

      slashIndicator.position = handlePoint;
      slashIndicator.localRotation = rotation;

      SpriteRenderer slashSpriter = slashIndicator.GetComponent<SpriteRenderer>();
      slashSpriter.sprite = swordSprites[(int)(lerpValue * swordSprites.Length)];
      float spriteHeight = slashSpriter.sprite.rect.height / slashSpriter.sprite.pixelsPerUnit;
      slashIndicator.localScale = Vector2.one * swordHeight / spriteHeight;
    }
  }
}
