using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexEnemyScript : BaseEnemyScript {

	void Awake () {
		hp = 3;
		if (animator == null)
			animator = GetComponent<Animator> ();
		if (bc == null)
			bc = GetComponent<BoxCollider2D> ();
		if (rb == null)
			rb = GetComponent<Rigidbody2D> ();
		
		animator.SetInteger ("Health", hp);
		name = "Trex";
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	void Roam() {

	}

	void Attack () {
	}
}
