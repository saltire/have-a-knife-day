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

  public static float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value) {
    Vector3 AB = b - a;
    Vector3 AV = value - a;
    return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
  }

  public static Quaternion QuaternionSmoothDamp(
    Quaternion current, Quaternion target, ref Quaternion deriv, float smoothTime, float maxSpeed
  ) {
		if (Time.deltaTime < Mathf.Epsilon) return current;
		// account for double-cover
		float sign = Quaternion.Dot(current, target) > 0f ? 1f : -1f;
		// smooth damp (nlerp approx)
		var Result = new Vector4(
			Mathf.SmoothDamp(current.x, target.x * sign, ref deriv.x, smoothTime, maxSpeed),
			Mathf.SmoothDamp(current.y, target.y * sign, ref deriv.y, smoothTime, maxSpeed),
			Mathf.SmoothDamp(current.z, target.z * sign, ref deriv.z, smoothTime, maxSpeed),
			Mathf.SmoothDamp(current.w, target.w * sign, ref deriv.w, smoothTime, maxSpeed)
		).normalized;

		// ensure deriv is tangent
		var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
		deriv.x -= derivError.x;
		deriv.y -= derivError.y;
		deriv.z -= derivError.z;
		deriv.w -= derivError.w;

		return new Quaternion(Result.x, Result.y, Result.z, Result.w);
	}
}
