using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcThrowable : MonoBehaviour {

	private Rigidbody2D rb;

	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		
	}

	public void SetArcForce(Vector2 playerSpeed, bool isFlipped) {		

		rb.AddForce (Vector2.up * Globals.boneForceY);

		if (isFlipped) {
			rb.AddTorque (-Globals.boneTorque);
			rb.AddForce (Vector2.left * Globals.boneForceX);
		}
		else {
			rb.AddTorque (Globals.boneTorque);
			rb.AddForce (Vector2.right * Globals.boneForceX);
		}

	}

	void OnTriggerEnter2D(Collider2D col) {

		Destroy (gameObject);
	}
}
