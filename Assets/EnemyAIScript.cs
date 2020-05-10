using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour {
  public SwordScript sword;
  public SwordScript playerSword;
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
  public float angleRange = 75;
  public float blockAngleRange = 40;
  float aimAngle = 90;
  float angleTarget;
  float angleVelocity = 0;
  float minAimAngle;
  float maxAimAngle;
  float minBlockAngle;
  float maxBlockAngle;

  public float blockDuration = .4f;
  float blockTimeRemaining = 0;

  void Start() {
    minAimAngle = 90 - angleRange / 2;
    maxAimAngle = 90 + angleRange / 2;
    minBlockAngle = 90 - blockAngleRange / 2;
    maxBlockAngle = 90 + blockAngleRange / 2;
  }

  void Update() {
    if (blockTimeRemaining > 0) {
      blockTimeRemaining -= Time.deltaTime;

      float blockAngle = Util.Map(minAimAngle, maxAimAngle, minBlockAngle, maxBlockAngle, aimAngle);
      sword.Block(blockAngle);
    }
    else if (!sword.IsSlashing()) {
      nextTick -= Time.deltaTime;

      if (nextTick <= 0) {
        nextTick = tickLength;

        if (playerSword.IsSlashing()) {
          blockTimeRemaining = blockDuration;
        }
        else if (Random.Range(0f, 1f) < attackProbability) {
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
      aimIndicator.localPosition = Quaternion.AngleAxis(aimAngle, Vector3.forward) * Vector2.up * radius;

      sword.PositionSword(0, aimAngle);
    }

    transform.localPosition = Vector2.SmoothDamp(transform.localPosition, moveTarget, ref moveVelocity, moveDuration);
  }
}
