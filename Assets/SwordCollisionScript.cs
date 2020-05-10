using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollisionScript : MonoBehaviour {
  void OnCollisionEnter2D(Collision2D collision) {
    transform.parent.GetComponent<SwordScript>().HandleCollision(collision);
  }
}
