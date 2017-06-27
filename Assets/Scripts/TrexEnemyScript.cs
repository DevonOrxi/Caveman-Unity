using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexEnemyScript : BaseEnemyScript {

	void Awake () {
		hp = 3;
		animator = GetComponent<Animator> ();
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

	public override void StopHurting ()
	{
		base.StopHurting ();
	}
}
