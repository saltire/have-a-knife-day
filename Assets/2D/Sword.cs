using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour {
  public float scale = 5;
  public float unitsPerMetre = 5;
  public float moveDuration = .2f;
  public float maxMoveSpeed = 10;
  Vector3 moveVelocity;

  public Collider2D[] stabAreas;

  Vector3 rotateVelocity = Vector3.zero;
  public float rotateDuration = .1f;
  public float maxRotateSpeed = 1000;

  public Transform target;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    Vector2 rightStick = gamepad.rightStick.ReadValue();

    // Move sword around the circle.
    transform.position = Vector3.SmoothDamp(
      transform.position, rightStick * scale, ref moveVelocity, moveDuration, maxMoveSpeed);

    Vector3 currentAimDir = transform.rotation * Vector3.up;

    // Aim the sword differently depending if the arm is at a slash or stab angle.
    bool aimForStab = stabAreas.Any(coll => coll.OverlapPoint(transform.position));
    Vector3 targetAimDir = aimForStab ? target.position - transform.position
      : transform.position - target.position;

    Vector3 aimDir = Vector3.SmoothDamp(currentAimDir, targetAimDir, ref rotateVelocity,
      rotateDuration, maxRotateSpeed);
    transform.rotation = Quaternion.FromToRotation(Vector3.up, aimDir);
  }
}
