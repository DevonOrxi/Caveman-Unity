using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenerator : MonoBehaviour {

	private GameObject currentInstance;
	public GameObject generatorPrefab;
	public Transform entityGroupRef;

	void Awake () {
		currentInstance = Instantiate (generatorPrefab, transform.position, transform.rotation, entityGroupRef);
		currentInstance.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "AliveArea") {
			if (currentInstance == null)
				currentInstance = Instantiate (generatorPrefab, transform.position, transform.rotation, entityGroupRef);
			else
				currentInstance.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "AliveArea") {
			if (currentInstance != null)
				currentInstance.SetActive (false);
		}
	}
}
