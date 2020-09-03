using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollisionScript : MonoBehaviour {
  void OnCollisionEnter2D(Collision2D collision) {
    // Util.Log("enter", gameObject, collision.gameObject);
    if (collision.gameObject.tag == "sword" && collision.transform.parent != transform) {
      GetComponent<SwordScript>().HandleSwordCollision(collision);
    }
  }

  void OnCollisionExit2D(Collision2D collision) {
    // Util.Log("exit", gameObject, collision.gameObject);
    if (collision.gameObject.tag == "body") {
      GetComponent<SwordScript>().HandleBodyCollision(collision);
    }
  }
}
