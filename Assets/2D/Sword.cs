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

  bool inSlashArea = false;
  public float minSlashVelocity = 20;
  public float slashCooldown = .25f;
  float cooldownRemaining = 0;

  bool slashing;
  Vector3 slashStart;
  Vector3 slashTarget;
  float slashScale;

  Quaternion rotateDeriv = Quaternion.identity;
  public float rotateDuration = .1f;
  public float maxRotateSpeed = 1000;

  public Transform stabTarget;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    Vector2 rightStick = gamepad.rightStick.ReadValue();
    float stickAngle = Vector3.SignedAngle(rightStick, Vector3.up, Vector3.back);

    if (!slashing) {
      // Move sword toward the stick direction.
      transform.position = Vector3.SmoothDamp(
        transform.position, rightStick * scale, ref moveVelocity, moveDuration, maxMoveSpeed);
    }
    else {
      // Move the sword toward the slash target. TODO: different speed values.
      transform.position = Vector3.SmoothDamp(
        transform.position, slashTarget, ref moveVelocity, moveDuration * 5, maxMoveSpeed);

      if (Vector3.Distance(transform.position, slashTarget) < .1f) {
        slashing = false;
        cooldownRemaining = slashCooldown;
      }
    }

    bool aimForStab = stabAreas.Any(coll => coll.OverlapPoint(transform.position));

    Quaternion targetRotation;
    if (aimForStab && !slashing) {
      // If sword is at stab angle, aim it directly at the target point.
      targetRotation = Quaternion.FromToRotation(Vector3.up,
        stabTarget.position - transform.position);
    }
    else {
      if (!slashing) {
        // If the sword is not slashing, aim it on an arc directly through the center.
        targetRotation = SlashRotation(transform.position,
          rightStick.normalized * scale, -rightStick.normalized * scale);
      }
      else {
        // If the sword is slashing, aim it on an arc from the start to the target.
        targetRotation = SlashRotation(transform.position, slashStart, slashTarget);
      }
    }

    transform.rotation = Util.QuaternionSmoothDamp(
      transform.rotation, targetRotation, ref rotateDeriv, rotateDuration, maxRotateSpeed);

    if (cooldownRemaining > 0) {
      cooldownRemaining -= Time.deltaTime;
    }

    if (!slashing && cooldownRemaining <= 0) {
      // If the sword is entering the slash area at a minimum speed, find and set the slash target.
      bool wasInSlashArea = inSlashArea;
      inSlashArea = slashArea.OverlapPoint(transform.position);

      if (
        !aimForStab && inSlashArea && !wasInSlashArea && moveVelocity.magnitude >= minSlashVelocity
      ) {
        // Find the target point on the opposite edge of the circle by passing it and going back.
        RaycastHit2D hit = Physics2D.Raycast(
          transform.position + moveVelocity.normalized * scale * 3, -moveVelocity);

        slashing = true;
        slashStart = transform.position;
        slashTarget = hit.point;
        slashScale = Vector3.Distance(slashStart, slashTarget);
      }
    }
  }

  Quaternion SlashRotation(Vector2 position, Vector2 start, Vector2 target) {
    float totalDistance = Vector2.Distance(start, target);
    float distanceLeft = totalDistance > 0
      ? Vector2.Distance(position, target) / totalDistance : .5f;

    // Arc the sword toward the opponent when approaching the center, then away again.
    float forwardAngle = Util.Map(1, 0, 5, 175, distanceLeft);

    float stickAngle = Vector2.SignedAngle(position - target, Vector2.up);

    // Tilt the sword up a bit when it's held low, to simulate a higher eye level.
    float tiltUpAngle = (transform.position.y / scale - 1) * 22.5f;

    return (
      Quaternion.Euler(tiltUpAngle, 0, 0) *
      Quaternion.Euler(0, 0, -stickAngle) *
      Quaternion.Euler(forwardAngle, 0, 0)
    );
  }
}
