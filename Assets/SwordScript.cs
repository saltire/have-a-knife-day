using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {
  public Sprite[] swordSprites;
  public Sprite blockSprite;
  public Transform hand;

  public float handPosition = 0.9f;

  public float slashDuration = .5f;
  float slashTimeRemaining = 0;
  float slashAngle = 0;

  public float swingAngleStart = -75;

  public float ellipseRadiusX = 3;
  public float ellipseRadiusY = 1;
  public float ellipseOffsetY = -2;

  public Vector2 blockOffset = new Vector2(-4, 0);

  SpriteRenderer swordSpriter;
  TrailRenderer swordTrail;

  void Start() {
    swordSpriter = GetComponent<SpriteRenderer>();
    swordTrail = GetComponentInChildren<TrailRenderer>();
  }

  void Update() {
    if (slashTimeRemaining > 0) {
      float lerpValue = (slashDuration - slashTimeRemaining) / slashDuration;
      PositionSword(lerpValue, slashAngle);

      if (lerpValue > .9f) {
        swordTrail.emitting = false;
      }

      slashTimeRemaining -= Time.deltaTime;
    }
  }

  public void StartSlashing(float angle) {
    slashAngle = angle;
    slashTimeRemaining = slashDuration;
    swordTrail.emitting = true;
  }

  public bool IsSlashing() {
    return slashTimeRemaining > 0;
  }

  public void Block(float angle) {
    Quaternion rotation = Quaternion.AngleAxis(angle - 90 * Mathf.Sign(angle), Vector3.forward);

    transform.localPosition = rotation * new Vector2(blockOffset.x * Mathf.Sign(angle), blockOffset.y);
    transform.localScale = new Vector2(Mathf.Sign(angle) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    transform.localRotation = Quaternion.Euler(-rotation.eulerAngles);

    swordSpriter.sprite = blockSprite;

    hand.position = transform.position;
  }

  public void PositionSword(float lerpValue, float angle) {
    if (angle < 0) {
      lerpValue = 1 - lerpValue;
    }

    float swingAngle = Util.EaseInOutQuad(-swingAngleStart, swingAngleStart, lerpValue);
    float swingAngleRad = (swingAngle + 90) * Mathf.Deg2Rad;

    Vector2 ellipseCenter = (Vector2)transform.parent.localPosition + new Vector2(0, ellipseOffsetY);
    Vector2 handlePoint = ellipseCenter + new Vector2(ellipseRadiusX * Mathf.Cos(swingAngleRad), ellipseRadiusY * Mathf.Sin(swingAngleRad));
    float swordHeight = transform.parent.localPosition.y - handlePoint.y;
    float swordWidth = swordHeight * Mathf.Tan(-swingAngle * Mathf.Deg2Rad);
    Vector2 swordPoint = handlePoint + new Vector2(swordWidth, swordHeight);

    Quaternion rotation = Quaternion.AngleAxis(angle - 90 * Mathf.Sign(angle), Vector3.forward);
    handlePoint = rotation * handlePoint;
    swordPoint = rotation * swordPoint;
    Debug.DrawLine(handlePoint, swordPoint);

    hand.position = Vector2.Lerp(swordPoint, handlePoint, handPosition);

    transform.position = new Vector3(swordPoint.x, swordPoint.y, transform.position.z);
    transform.localRotation = rotation;

    swordSpriter.sprite = swordSprites[Mathf.Min((int)(lerpValue * swordSprites.Length), swordSprites.Length - 1)];
    float spriteHeight = swordSpriter.sprite.rect.height / swordSpriter.sprite.pixelsPerUnit;
    transform.localScale = Vector2.one * swordHeight / spriteHeight;
  }
}
