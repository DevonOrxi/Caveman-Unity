using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Animator animator;
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private bool isGrounded = false;

	public GameObject[] feetRef;

	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}
	
	void Update () {
		
	}

	void FixedUpdate() {
		HorizontalMovement ();
		CheckGrounded ();
		Jump ();
		CheckFacing ();
	}

	void Jump() {
		if (Input.GetKeyDown (KeyCode.Z) && isGrounded) {
			rb.AddForce (Globals.jumpForce * Vector2.up);
		}

		animator.SetFloat ("SpeedY", rb.velocity.y);
	}

	void HorizontalMovement() {
		float resultantX = 0;

		if (Input.GetKey (KeyCode.RightArrow))
			resultantX += Globals.playerSpeedX;
		if (Input.GetKey (KeyCode.LeftArrow))
			resultantX -= Globals.playerSpeedX;

		resultantX *= Time.deltaTime;

		rb.velocity = new Vector2 (resultantX, rb.velocity.y);
		
		animator.SetFloat ("SpeedX", Mathf.Abs(resultantX));
	}

	void CheckGrounded() {
		isGrounded = false;

		for (int i = 0; i < feetRef.Length; i++) {
			GameObject g = feetRef [i];

			if (Physics2D.Raycast (g.transform.position, Vector2.down).distance < 0.05)
				isGrounded = true;
		}

		if (isGrounded)
			animator.SetBool ("IsGrounded", true);
		else {
			animator.SetBool ("IsGrounded", false);
		}
	}

	void CheckFacing() {
		if (rb.velocity.x > 0)
			sr.flipX = false;
		else if (rb.velocity.x < 0)
			sr.flipX = true;
	}
}
