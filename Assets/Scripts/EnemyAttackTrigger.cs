using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour {

	private bool damaging = false;

	void OnTriggerEnter2D (Collider2D col) {
		if(col.tag == "Player" && damaging) {
			Vector2 recoilForce = new Vector2(
				transform.position.x - col.transform.position.x,
				transform.position.y - col.transform.position.y + 1);
			col.gameObject.GetComponent<PlayerController> ().GetDamaged (recoilForce);
		}
	}
}
