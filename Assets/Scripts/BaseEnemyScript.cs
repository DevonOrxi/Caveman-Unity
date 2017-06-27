using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status {
	Roaming,
	Idle,
	Attacking,
	Hurting,
	Dying
}

public class BaseEnemyScript : MonoBehaviour {

	protected int hp = 1;
	protected Status status = Status.Idle;
	protected Status prevStatus = Status.Idle;
	protected Animator animator;

	void Awake () {
		animator = GetComponent<Animator> ();
		name = "Base";
	}

	public void GetDamaged() {
		hp--;

		status = Status.Hurting;
		animator.SetBool ("Hurt", true);
	}

	public void GetDamaged(int damage) {
		hp -= damage;

		status = Status.Hurting;
		animator.SetBool ("Hurt", true);
	}

	virtual public void StopHurting() {
		switch (prevStatus) {
		case Status.Idle:
			animator.SetBool ("ForceIdle", true);
			break;
		case Status.Roaming:
			animator.SetBool ("ForceRun", true);
			break;
		}
	}

	void Update() {
		prevStatus = status;
	}

	public void ResetForcingBooleans() {
		animator.SetBool ("ForceRun", false);
		animator.SetBool ("ForceIdle", false);
		animator.SetBool ("Hurt", false);
	}
}
