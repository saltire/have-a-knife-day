using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour {
  public float scale = 5;
  public float unitsPerMetre = 5;
  public float moveDuration = .2f;
  public float maxMoveSpeed = 10;
  Vector3 moveVelocity;

  public Collider2D slashArea;
  public Collider2D[] stabAreas;

  Vector3 rotateVelocity = Vector3.zero;
  public float rotateDuration = .1f;
  public float maxRotateSpeed = 1000;

  public Transform target;

  bool inSlashArea = false;
  Vector3? slashTarget;
  public float minSlashVelocity = 20;

  public float slashCooldown = .25f;
  float cooldownRemaining = 0;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    Vector2 rightStick = gamepad.rightStick.ReadValue();

    if (!slashTarget.HasValue) {
      // Move sword toward the stick direction.
      transform.position = Vector3.SmoothDamp(
        transform.position, rightStick * scale, ref moveVelocity, moveDuration, maxMoveSpeed);
    }
    else {
      // Move the sword toward the slash target. TODO: different speed values.
      transform.position = Vector3.SmoothDamp(
        transform.position, slashTarget.Value, ref moveVelocity, moveDuration * 5, maxMoveSpeed);

      if (Vector3.Distance(transform.position, slashTarget.Value) < .1f) {
        slashTarget = null;
        cooldownRemaining = slashCooldown;
      }
    }

    Vector3 currentAimDir = transform.rotation * Vector3.up;

    // Aim the sword differently depending if the arm is at a slash or stab angle.
    bool aimForStab = stabAreas.Any(coll => coll.OverlapPoint(transform.position));
    Vector3 targetAimDir = aimForStab ? target.position - transform.position
      : transform.position - target.position;

    Vector3 aimDir = Vector3.SmoothDamp(currentAimDir, targetAimDir, ref rotateVelocity,
      rotateDuration, maxRotateSpeed);
    transform.rotation = Quaternion.FromToRotation(Vector3.up, aimDir);

    if (cooldownRemaining > 0) {
      cooldownRemaining -= Time.deltaTime;
    }

    if (!slashTarget.HasValue && cooldownRemaining <= 0) {
      // If the sword is entering the slash area at a minimum speed, find and set the slash target.
      bool wasInSlashArea = inSlashArea;
      inSlashArea = slashArea.OverlapPoint(transform.position);

      if (
        !aimForStab && inSlashArea && !wasInSlashArea && moveVelocity.magnitude >= minSlashVelocity
      ) {
        // Find the target point on the opposite edge of the circle by passing it and going back.
        RaycastHit2D hit = Physics2D.Raycast(
          transform.position + moveVelocity.normalized * scale * 3, -moveVelocity);
        slashTarget = hit.point;
      }
    }
  }
}
