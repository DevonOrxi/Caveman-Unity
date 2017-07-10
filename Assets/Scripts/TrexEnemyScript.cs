using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexEnemyScript : BaseEnemyScript {

	private bool roamLeft;
	private bool canDisableInstance = true;
	public bool CanDisableInstance { get { return canDisableInstance; } }
	public GameObject leftAttackTrigger;
	public GameObject rightAttackTrigger;
	public EntityGenerator eg;

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

	//	POR QUÉ MIERDAS NECESITO PONERLE VELOCIDAD CADA FRAME?
	void Roam() {
		rb.velocity = new Vector2 (Globals.trexSpeedX * (roamLeft ? -1 : 1) * Time.deltaTime, 0);
		animator.SetFloat ("SpeedAbsX", Mathf.Abs (rb.velocity.x));
	}

	void Attack () {
		status = Status.Attacking;
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.tag == "AliveArea")
			this.canDisableInstance = false;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "StopSignal")
			ChangeDirection ();
		else if (col.tag == "AliveArea")
			this.canDisableInstance = false;
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "AliveArea")
			this.canDisableInstance = true;
	}
}
