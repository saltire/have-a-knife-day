using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {
  public GameObject[] slashObjs;
  public GameObject blockObj;
  public Transform hand;
  public GameObject blockBurstPrefab;
  public GameObject hitBurstPrefab;

  public float handPosition = 0.9f;

  public float slashDuration = .5f;
  float slashTimeRemaining = 0;
  float slashAngle = 0;

  public float swingAngleStart = -75;

  public float ellipseRadiusX = 3;
  public float ellipseRadiusY = 1;
  public float ellipseOffsetY = -2;

  public Vector2 blockOffset = new Vector2(-4, 0);

  GameObject swordObj;
  TrailRenderer swordTrail;

  ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();

  void Start() {
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

  public void StopSlashing() {
    slashTimeRemaining = 0;
    swordTrail.emitting = false;
  }

  public void Block(float angle) {
    Quaternion rotation = Quaternion.AngleAxis(angle - 90 * Mathf.Sign(angle), Vector3.forward);

    transform.localPosition = rotation * new Vector3(blockOffset.x * Mathf.Sign(angle), blockOffset.y, transform.localPosition.z);
    transform.localScale = new Vector2(Mathf.Sign(angle) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    transform.localRotation = Quaternion.Euler(-rotation.eulerAngles);

    foreach (GameObject obj in slashObjs) obj.SetActive(false);
    swordObj = blockObj;
    swordObj.SetActive(true);

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

    foreach (GameObject obj in slashObjs) obj.SetActive(false);
    blockObj.SetActive(false);
    swordObj = slashObjs[Mathf.Min((int)(lerpValue * slashObjs.Length), slashObjs.Length - 1)];
    swordObj.SetActive(true);
    Sprite swordSprite = swordObj.GetComponent<SpriteRenderer>().sprite;
    float spriteHeight = swordSprite.rect.height / swordSprite.pixelsPerUnit;
    transform.localScale = Vector2.one * swordHeight / spriteHeight;
  }

  public void HandleSwordCollision(Collision2D collision) {
    if (IsSlashing()) {
      StopSlashing();
      Instantiate<GameObject>(blockBurstPrefab,
        collision.contactCount > 0 ? collision.GetContact(0).point : (Vector2)transform.position,
        Quaternion.identity);
    }
  }

  public void HandleBodyCollision(Collision2D collision) {
    if (IsSlashing() && collision.gameObject != gameObject) {
      StopSlashing();
      Instantiate<GameObject>(hitBurstPrefab,
        collision.contactCount > 0 ? collision.GetContact(0).point : (Vector2)transform.position,
        Quaternion.identity);
      collision.gameObject.GetComponent<ScoreScript>().LoseFinger();
    }
  }
}
