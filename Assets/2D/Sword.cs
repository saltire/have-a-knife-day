using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
  // Angles are clockwise from the top.
  public float minStabAngle = 140;
  public float maxStabAngle = 220;

  public Vector2 stabOrigin;
  public float stabRadius;

  // float velocity = 0;
  Vector3 velocity = Vector3.zero;
  public float aimDuration = .1f;
  public float maxSpeed = 1000;

  public Transform target;

  void Update() {
    // 2D angle of the hand's position around the centre Z-axis, clockwise from the top.
    float armAngle = Vector2.SignedAngle(transform.position, Vector3.down) + 180;

    // Aim the sword differently depending if the arm is at a slash or stab angle.
    // bool aimForStab = armAngle > minStabAngle && armAngle < maxStabAngle;
    bool aimForStab = Vector2.Distance(transform.position, stabOrigin) <= stabRadius;

    // float currentAimAngle = transform.eulerAngles.z;

    // // TODO: During a slash, continue using slash aim until slash is over.
    // // Then go back to using either slash or stab aim depending on the arm angle.

    // float targetAimAngle = stabAngle ? -(armAngle + 180) : -armAngle;

    // float aimAngle = Mathf.SmoothDampAngle(
    //   currentAimAngle, targetAimAngle,
    //   ref velocity, aimDuration, maxSpeed);

    // transform.rotation = Quaternion.Euler(0, 0, aimAngle);


    Vector3 currentAimDir = transform.rotation * Vector3.up;
    Vector3 targetAimDir = aimForStab ? target.position - transform.position
      : transform.position - target.position;

    // Vector3 axis = Vector3.Cross(currentAimDir, targetAimDir);
    // float angle = Vector3.Angle(currentAimDir, targetAimDir);

    // float moveAngle = Mathf.SmoothDampAngle(0, angle, ref velocity, aimDuration, maxSpeed);
    // transform.RotateAround(transform.position, axis, moveAngle);

    Vector3 aimDir = Vector3.SmoothDamp(currentAimDir, targetAimDir, ref velocity,
      aimDuration, maxSpeed);
    transform.rotation = Quaternion.FromToRotation(Vector3.up, aimDir);
  }
}
