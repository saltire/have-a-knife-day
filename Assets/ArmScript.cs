using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour {
  public Transform shoulder;
  public Transform handle;

  void Update() {
    GetComponent<LineRenderer>().SetPositions(new Vector3[] { shoulder.position, handle.position });
  }
}
