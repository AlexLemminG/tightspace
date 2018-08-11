using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
	public float acc;
	public float targetSpeed;
	Vector3 prevPos;
	Vector3 prevPos2;

	Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = initialVelocity;
		Debug.Log(rb.velocity);
	}

	public Vector2 initialVelocity;

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 targetDirection = rb.velocity.normalized;
		if (targetDirection == Vector2.zero) {
			targetDirection = Random.insideUnitCircle.normalized;
		}
		var targetVelocity = targetDirection * targetSpeed;
		rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, acc * Time.fixedDeltaTime);
		prevPos = prevPos2;
		prevPos2 = transform.position;
	}
	public void RevertPos(){
		transform.position = prevPos2;// + (Vector3)rb.velocity * Time.fixedDeltaTime;
	}
}