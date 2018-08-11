using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour {
	public AnimationCurve scale;
	public float duration;
	IEnumerator Start () {
		float t = 0f;
		SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		Color clearWhite = Color.white;
		clearWhite.a = 0f;
		while(t < 1f){
			transform.localScale = Vector3.one * scale.Evaluate(t);
			foreach(var sr in spriteRenderers)
			    sr.color = Color.Lerp(Color.white, clearWhite, t);
			t += Time.deltaTime / duration;
			yield return null;
		}
		Destroy(gameObject);
	}
}
