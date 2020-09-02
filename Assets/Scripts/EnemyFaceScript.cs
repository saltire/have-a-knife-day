using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFaceScript : MonoBehaviour {
  SpriteRenderer spriter;
  Sprite defaultSprite;
  public Sprite enemyHitSprite;
  public Sprite enemyHurtSprite;
  public Sprite enemyWinSprite;
  public Sprite enemyLoseSprite;

  public float faceDuration = 0.5f;
  float resetTime;

  void Start() {
    spriter = GetComponent<SpriteRenderer>();
    defaultSprite = spriter.sprite;
  }

  void Update() {
    if (spriter.sprite != defaultSprite && Time.time >= resetTime) {
      spriter.sprite = defaultSprite;
    }
  }

  public void OnHit(bool enemyHit) {
    spriter.sprite = enemyHit ? enemyHitSprite : enemyHurtSprite;
    resetTime = Time.time + faceDuration;
  }

  public void OnWin(bool enemyWin) {
    spriter.sprite = enemyWin ? enemyWinSprite : enemyLoseSprite;
  }
}
