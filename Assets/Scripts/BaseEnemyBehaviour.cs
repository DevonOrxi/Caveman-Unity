using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status {
	Roaming,
	Idle,
	Attacking,
	Jumping,
	Falling,
	Hurting,
	Dying,
	Dead
}

public class BaseEnemyScript : MonoBehaviour {

	protected int hp = 1;
	protected Status status = Status.Idle;
	protected Status prevStatus = Status.Idle;
	protected Animator animator;
	protected BoxCollider2D bc;
	protected Rigidbody2D rb;

	void Awake () {
		if (animator == null)
			animator = GetComponent<Animator> ();
		if (bc == null)
			bc = GetComponent<BoxCollider2D> ();
		if (rb == null)
			rb = GetComponent<Rigidbody2D> ();
		
		name = "Base";
	}

	public void GetDamaged() {
		if (hp > 0) {
			hp--;
			Debug.Log (hp);
			HurtLogic ();
		}
	}

	public void GetDamaged(int damage) {
		if (hp >= 0) {
			hp -= damage;
			Debug.Log (hp);
			HurtLogic ();
		}
	}

	public void HurtLogic() {
		if (hp >= 0) {
			status = Status.Hurting;
			animator.Play ("Hurt", -1, 0f);
			animator.SetBool ("Hurt", true);
			animator.SetInteger ("Health", hp);

			if (hp == 0)
				status = Status.Dying;
		}
	}

	public void StopBehaviour () {
		rb.simulated = false;
		bc.enabled = false;
		status = Status.Dead;
		this.enabled = false;
	}

	void Update() {
		prevStatus = status;
	}
}
