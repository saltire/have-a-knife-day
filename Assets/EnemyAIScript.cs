using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour {
  public SwordScript sword;
  public Transform aimIndicator;

  public float tickLength = 1;
  public float attackProbability = 0.25f;
  public float switchProbability = 0.25f;
  float nextTick = 0;

  public float moveAmount = 1;
  public float moveDuration = 2;
  Vector2 moveTarget;
  Vector2 moveVelocity = Vector2.zero;

  public float radius = 3;
  public float angleRange = 120;
  float aimAngle = 90;
  float angleTarget;
  float angleVelocity = 0;

  void Update() {
    if (!sword.IsSlashing()) {
      nextTick -= Time.deltaTime;

      if (nextTick <= 0) {
        nextTick = tickLength;

        if (Random.Range(0f, 1f) < attackProbability) {
          sword.StartSlashing(aimAngle);
        }
        else {
          moveTarget = Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.forward) * Vector2.up;

          angleTarget = (Random.Range(0, angleRange) + (180 - angleRange) / 2) * Mathf.Sign(angleTarget);
          if (Random.Range(0f, 1f) < switchProbability) {
            aimAngle *= -1;
            angleTarget *= -1;
          }
        }
      }

      aimAngle = Mathf.SmoothDampAngle(aimAngle, angleTarget, ref angleVelocity, tickLength);
      aimIndicator.position = Quaternion.AngleAxis(aimAngle, Vector3.forward) * Vector2.up * radius;

      sword.PositionSword(0, aimAngle);
    }

    transform.position = Vector2.SmoothDamp(transform.position, moveTarget, ref moveVelocity, moveDuration);
  }
}
