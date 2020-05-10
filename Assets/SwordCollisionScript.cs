using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollisionScript : MonoBehaviour {
  void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "sword") {
      transform.parent.GetComponent<SwordScript>().HandleSwordCollision(collision);
    }
    else if (collision.gameObject.tag == "body") {
      transform.parent.GetComponent<SwordScript>().HandleBodyCollision(collision);
    }
  }
}
