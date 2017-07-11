using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		if (col.gameObject.tag == "Enemy") {

			//	Meter esto en otra función
			//	Usar Transform del collider
			Vector2 recoilForce = new Vector2(
				transform.position.x - col.transform.position.x,
				transform.position.y - col.transform.position.y + 1);
			GetDamaged (recoilForce);
		}
	}

	public void GetDamaged(Vector2 rf) {
		if (hp > 0) {
			hp--;
			HurtLogic (rf);
		}
	}

	public void GetDamaged(Vector2 rf, int damage) {
		if (hp >= 0) {
			hp -= damage;
			HurtLogic (rf);
		}
	}

	private void HurtLogic(Vector2 rf) {
		if (hp >= 0 && !isDamaged) {
			hp--;
			isDamaged = true;
			animator.SetBool ("IsDamaged", true);

			blinkCoroutine = Blink (Globals.hurtTime);
			StartCoroutine(blinkCoroutine);

			rb.AddForce(rf.normalized * Globals.recoilForce);

			Invoke ("StopHurting", Globals.hurtTime);
			animator.Play ("Hurt", -1, 0f);
			animator.SetBool ("Hurt", true);
			animator.SetInteger ("Health", hp);
			rb.velocity = Vector2.zero;
			animator.SetFloat ("SpeedAbsX", 0);

			if (hp == 0)
				status = Status.Dying;
			else
				status = Status.Hurting;
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
		if (hp > 0) {
			isDamaged = false;
			animator.SetBool ("IsDamaged", false);
		} else {
			animator.SetBool ("IsDying", true);
		}
	}

	void LoseLife() {
		SceneManager.LoadScene ("GameOver");
	}
}
