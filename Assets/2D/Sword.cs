using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
  // Angles are clockwise from the top.
  public float minStabAngle = 90;
  public float maxStabAngle = 225;

  float velocity = 0;
  public float aimDuration = .05f;
  public float maxSpeed = 1000;

  void Update() {
    float armAngle = Vector2.SignedAngle(transform.position, Vector3.down) + 180;

    float currentAimAngle = transform.eulerAngles.z;

    // Aim the sword differently depending if the arm is at a slash or stab angle.

    // TODO: During a slash, continue using slash aim until slash is over.
    // Then go back to using either slash or stab aim depending on the arm angle.

    float targetAimAngle = (armAngle > minStabAngle && armAngle < maxStabAngle)
      ? -(armAngle + 180) : -armAngle;

    float aimAngle = Mathf.SmoothDampAngle(
      currentAimAngle, targetAimAngle,
      ref velocity, aimDuration, maxSpeed);

    transform.rotation = Quaternion.Euler(0, 0, aimAngle);
  }
}
