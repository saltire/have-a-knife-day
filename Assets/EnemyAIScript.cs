using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour {
  public SwordScript sword;
  public Transform aimIndicator;

  public float radius = 3;
  public float angleRange = 120;

  public float tickLength = 1;
  public float attackProbability = 0.25f;
  public float switchProbability = 0.25f;

  float nextTick = 0;

  float aimAngle = 90;
  float angleTarget;
  float angleVelocity = 0;

  void Start() {

  }

  void Update() {
    if (!sword.IsSlashing()) {
      nextTick -= Time.deltaTime;

      if (nextTick <= 0) {
        nextTick = tickLength;

        if (Random.Range(0f, 1f) < attackProbability) {
          sword.StartSlashing(aimAngle);
        }
        else {
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
  }
}
