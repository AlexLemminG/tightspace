using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledParticle : MonoBehaviour {

	Vector2 prevPos;
	Queue<Vector2> velocities;
	int velocityCountFrames = 5;

	void Awake(){
		velocities = new Queue<Vector2>();
	}
	void OnEnable(){
		prevPos = transform.position;
		velocities.Clear();
	}

	void OnCollisionEnter2D(Collision2D col){
		GameManager.instance.OnControlledParticleCollision(col.contacts[0].point);
	}

	void Update(){
		var velocity = ((Vector2)transform.position - prevPos)/Time.deltaTime;
		prevPos = transform.position;
		velocities.Enqueue(velocity);
		if (velocities.Count > velocityCountFrames)
		{
			velocities.Dequeue();
		}
		Debug.DrawRay(transform.position, GetAvarageVelocity());
	}
    public Vector2 GetAvarageVelocity()
    {
        var result = Vector2.zero;
        foreach (var v in velocities)
        {
            result += v;
        }
        if (velocities.Count > 0)
        {
            result /= velocities.Count;
        }
        return result;
    }
}
