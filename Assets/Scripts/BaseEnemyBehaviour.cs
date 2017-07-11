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
	protected SpriteRenderer sr;
	public EntityGenerator eg;
	protected bool canDisableInstance = true;
	public bool CanDisableInstance { get { return canDisableInstance; } }

	void Start () {
		if (animator == null)
			animator = GetComponent<Animator> ();
		if (bc == null)
			bc = GetComponent<BoxCollider2D> ();
		if (rb == null)
			rb = GetComponent<Rigidbody2D> ();
		if (sr == null)
			sr = GetComponent<SpriteRenderer> ();
		
		name = "Base";
	}

	public void GetDamaged() {
		if (hp > 0) {
			hp--;
			HurtLogic ();
		}
	}

	public void GetDamaged(int damage) {
		if (hp >= 0) {
			hp -= damage;
			HurtLogic ();
		}
	}

	private void HurtLogic() {
		if (hp >= 0) {
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

	public void StopBehaviour () {
		rb.simulated = false;
		bc.enabled = false;
		status = Status.Dead;
		this.enabled = false;
	}

	public void StopHurting() {
		status = prevStatus;

		if (status == Status.Roaming) {
			rb.velocity = new Vector2 (Globals.trexSpeedX * (sr.flipX ? -1 : 1) * Time.deltaTime, 0);
			animator.SetFloat ("SpeedAbsX", Mathf.Abs (rb.velocity.x));
		}

		animator.SetBool ("Hurt", false);
	}
}
