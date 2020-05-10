using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour {
  public Transform shoulder;
  public Transform hand;

  void Update() {
    GetComponent<LineRenderer>().SetPositions(new Vector3[] { shoulder.position, hand.position });
  }
}
