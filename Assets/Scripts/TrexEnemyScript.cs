using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexEnemyScript : BaseEnemyScript {

	private bool roamLeft;
	public GameObject leftAttackTrigger;
	public GameObject rightAttackTrigger;

	void Start () {
		hp = 3;
		if (animator == null)
			animator = GetComponent<Animator> ();
		if (bc == null)
			bc = GetComponent<BoxCollider2D> ();
		if (rb == null)
			rb = GetComponent<Rigidbody2D> ();
		if (sr == null)
			sr = GetComponent<SpriteRenderer> ();
		
		animator.SetInteger ("Health", hp);
		name = "Trex";
		roamLeft = sr.flipX;
		rb.velocity = new Vector2 (Globals.trexSpeedX * (roamLeft ? -1 : 1) * Time.deltaTime, 0);
		animator.SetFloat ("SpeedAbsX", Mathf.Abs (rb.velocity.x));
		rightAttackTrigger.SetActive (!roamLeft);
		leftAttackTrigger.SetActive (roamLeft);
		status = Status.Roaming;
	}
	
	void Update () {
		if (status != Status.Hurting &&
			status != Status.Dying &&
			status != Status.Attacking &&
			status != Status.Dead) {
			prevStatus = status;
		}

		switch (status) {
		case Status.Attacking:
			break;
		case Status.Dying:
			break;
		case Status.Dead:
			break;
		case Status.Hurting:
			break;
		case Status.Idle:
			break;
		case Status.Roaming:
			Roam ();
			break;
		}

		if (this.canDisableInstance && eg.CanDisableInstance)
			gameObject.SetActive (false);
	}

	void ChangeDirection() {
		sr.flipX = !sr.flipX;
		roamLeft = sr.flipX;
		rightAttackTrigger.SetActive (!roamLeft);
		leftAttackTrigger.SetActive (roamLeft);
	}

	void Roam() {
		rb.velocity = new Vector2 (Globals.trexSpeedX * (roamLeft ? -1 : 1) * Time.deltaTime, 0);
		animator.SetFloat ("SpeedAbsX", Mathf.Abs (rb.velocity.x));
	}

	void StartAttack () {
		status = Status.Attacking;
		animator.SetBool ("FirstAttackFrame", true);

	}

	public void OpenAttackBox (){
		GameObject playerDetector = roamLeft ? leftAttackTrigger : rightAttackTrigger;

	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.tag == "AliveArea")
			this.canDisableInstance = false;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "StopSignal")
			ChangeDirection ();
		if (col.tag == "AliveArea")
			this.canDisableInstance = false;
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "AliveArea")
			this.canDisableInstance = true;
	}
}
