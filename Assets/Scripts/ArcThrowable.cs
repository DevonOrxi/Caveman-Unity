using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcThrowable : MonoBehaviour {

	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		rb.AddTorque (-5);
	}
	
	void Update () {
		
	}
}
