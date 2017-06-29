using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private IEnumerator blinkCoroutine;
	private Animator animator;
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private bool isGrounded = false;
	private bool isFirstAttackFrame = false;
	private bool isDamaged = false;
	private Status status = Status.Idle;
	private int hp = 3;

	public GameObject[] feetRef;
	public GameObject bonePrefab;
	public GameObject weaponProjGroup;

	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		blinkCoroutine = Blink (Globals.hurtTime);
	}
	
	void Update () {
		animator.SetBool ("FirstAttackFrame", false);
		if (!isDamaged) {
			Jump ();
			Attack ();
		}
	}

	void FixedUpdate() {
		if (!isDamaged) {
			HorizontalMovement ();
			CheckGrounded ();
		}
		CheckFacing ();
	}

	void GenerateProjectile() {
		GameObject p = Instantiate(bonePrefab, transform.position, transform.rotation, weaponProjGroup.transform);
		ArcThrowable at = p.GetComponent<ArcThrowable> ();
		at.SetArcForce (rb.velocity, sr.flipX);
	}

	void Attack() {
		if (Input.GetKeyDown (KeyCode.X)) {
			isFirstAttackFrame = true;
			animator.SetBool ("FirstAttackFrame", true);
		}
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
		
		animator.SetFloat ("SpeedAbsX", Mathf.Abs(resultantX));
	}

	void CheckGrounded() {
		isGrounded = false;

		for (int i = 0; i < feetRef.Length; i++) {
			RaycastHit2D rh = Physics2D.Raycast (feetRef [i].transform.position, Vector2.down, 0.05f, 1 << 10);

			if (rh.collider != null) {
				isGrounded = true;
			}
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

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Enemy" && !isDamaged) {
			isDamaged = true;
			animator.SetBool ("IsDamaged", true);
			StartCoroutine(blinkCoroutine);

			rb.AddForce((new Vector2(
				transform.position.x - col.transform.position.x,
				transform.position.y - col.transform.position.y + 1
			).normalized * Globals.recoilForce));

			Invoke ("StopHurting", Globals.hurtTime);
		}
	}

	private IEnumerator Blink(float waitTime) {
		float endTime = Time.time + waitTime;
		while (Time.time < endTime) {
			sr.enabled = false;
			yield return new WaitForSeconds (0.08f);
			sr.enabled = true;
			yield return new WaitForSeconds (0.08f);
		}
	}

	void StopHurting() {
		isDamaged = false;
		animator.SetBool ("IsDamaged", false);
	}
}
