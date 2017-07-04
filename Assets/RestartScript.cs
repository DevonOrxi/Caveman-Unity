using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown (KeyCode.X))
			SceneManager.LoadScene ("Game");
	}
}
