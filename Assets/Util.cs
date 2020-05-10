using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
	public static void Log(params object[] items) {
		string str = "";
		foreach (object item in items) {
			str += item;
			str += " ";
		}
		Debug.Log(str);
	}

  public static float Map(float fromA, float fromB, float toA, float toB, float value) {
    float lerpValue = Mathf.InverseLerp(fromA, fromB, value);
    return Mathf.Lerp(toA, toB, lerpValue);
  }

	public static float EaseInOutQuad(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value + start;
		value--;
		return -end * 0.5f * (value * (value - 2) - 1) + start;
	}

  public static float EaseInOutCubic(float start, float end, float value) {
    value /= .5f;
    end -= start;
    if (value < 1) return end * 0.5f * value * value * value + start;
    value -= 2;
    return end * 0.5f * (value * value * value + 2) + start;
  }

  public static float EaseInOutQuart(float start, float end, float value) {
    value /= .5f;
    end -= start;
    if (value < 1) return end * 0.5f * value * value * value * value + start;
    value -= 2;
    return -end * 0.5f * (value * value * value * value - 2) + start;
  }

  public static float EaseInOutQuint(float start, float end, float value) {
    value /= .5f;
    end -= start;
    if (value < 1) return end * 0.5f * value * value * value * value * value + start;
    value -= 2;
    return end * 0.5f * (value * value * value * value * value + 2) + start;
  }
}
