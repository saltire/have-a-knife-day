using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour {
  public float fadeDuration = .5f;

  LineRenderer line;

  Queue<Vector3> positions = new Queue<Vector3>();
  Queue<float> times = new Queue<float>();
  Queue<float> velocities = new Queue<float>();

  Vector3 lastPosition = Vector3.zero;

  void Start() {
    line = GetComponent<LineRenderer>();
  }

  void Update() {
    float velocity = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
    lastPosition = transform.position;

    positions.Enqueue(transform.position);
    times.Enqueue(Time.time);
    velocities.Enqueue(velocity);

    float expireTime = Time.time - fadeDuration;
    while (times.Count > 0 && times.Peek() < expireTime) {
      times.Dequeue();
      positions.Dequeue();
      velocities.Dequeue();
    }

    line.positionCount = positions.Count;
    line.SetPositions(positions.ToArray());
  }
}
