using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenerator : MonoBehaviour {

	private GameObject currentInstance;
	private bool canDisableInstance = true;
	public bool CanDisableInstance { get { return canDisableInstance; } }

	public GameObject generatorPrefab;
	public Transform entityGroupRef;

	void Awake () {
		currentInstance = Instantiate (generatorPrefab, transform.position, transform.rotation, entityGroupRef);
		switch (generatorPrefab.name) {
		case "Trex":
			TrexEnemyScript tes = currentInstance.GetComponent<TrexEnemyScript> ();
			tes.eg = this;
			break;
		}
		currentInstance.SetActive (false);
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "AliveArea")
			canDisableInstance = true;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "AliveArea") {
			if (currentInstance == null) {
				currentInstance = Instantiate (generatorPrefab, transform.position, transform.rotation, entityGroupRef);
				switch (generatorPrefab.name) {
				case "Trex":
					TrexEnemyScript tes = currentInstance.GetComponent<TrexEnemyScript> ();
					tes.eg = this;
					break;
				}
			}
			else
				currentInstance.SetActive (true);
			canDisableInstance = false;
		}
	}


}
