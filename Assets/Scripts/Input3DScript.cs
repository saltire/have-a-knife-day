using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input3DScript : MonoBehaviour {
  public Transform inputSphere;
  public Transform outputSphere;

  public float radius = 4;

  public float maxAngle = 60;

  public float moveSmoothTime = 0.1f;
  public float moveMaxSpeed = 100;

  public float tiltSmoothTime = 0.25f;

  float currentAngle;
  float tiltVelocity;
  Vector3 cameraAxis;
  public bool useCameraAxis;

  Vector3 inputVelocity;
  Vector3 outputVelocity;

  float currentX;
  float xVelocity;


  void Start() {
    cameraAxis = FindObjectOfType<Camera>().transform.rotation * Vector3.forward;
  }

  void Update() {
    Gamepad gamepad = Gamepad.current;
    if (gamepad == null) {
      return;
    }

    Vector2 rightStick = gamepad.rightStick.ReadValue();
    float side = Mathf.Sign(rightStick.x);

    // Tilt the aim plane.

    float angle = Vector3.SignedAngle(Vector3.right * side, rightStick, Vector3.forward);
    float targetAngle = Mathf.SmoothStep(-maxAngle, maxAngle, Mathf.InverseLerp(-90, 90, angle));

    currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle,
      ref tiltVelocity, tiltSmoothTime);
    transform.localRotation = Quaternion.AngleAxis(currentAngle,
      useCameraAxis ? cameraAxis : Vector3.forward);

    // Move the input indicator.

    Vector3 inputPosition = new Vector3(rightStick.x, 0, rightStick.y) * radius;
    inputSphere.localPosition = Vector3.SmoothDamp(inputSphere.localPosition, inputPosition,
      ref inputVelocity, moveSmoothTime, moveMaxSpeed);

    // Move the output indicator.

    currentX = Mathf.SmoothDamp(currentX, rightStick.magnitude * side, ref xVelocity,
      moveSmoothTime);
    // Set the Z position based on input speed, clamped to the circular plane edge.
    float currentZ = Mathf.Clamp(Mathf.Abs(inputVelocity.magnitude) / moveMaxSpeed,
      0, Mathf.Cos(currentX * Mathf.PI / 2));
    outputSphere.localPosition = new Vector3(currentX, 0, currentZ) * radius;
  }
}
