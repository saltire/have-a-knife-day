using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input2DScript : MonoBehaviour {
  public float scale = 5;
  public float smoothTime = .2f;
  public float unitsPerMetre = 5;
  public float maxSpeed = 10;

  Vector3 velocity;

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    Vector2 rightStick = gamepad.rightStick.ReadValue();
    // Get the current polar coordinates. Angle is clockwise from the top.
    float radius = rightStick.magnitude;
    float angle = Vector2.SignedAngle(rightStick, Vector2.down) + 180;

    // Vector3 gravity = 9.8f * unitsPerMetre * Time.deltaTime * Vector3.down;

    // velocity += gravity;

    transform.position = Vector3.SmoothDamp(
      transform.position, rightStick * scale, ref velocity, smoothTime, maxSpeed);

    // Util.Log(velocity.y, gravity.y);
  }
}
