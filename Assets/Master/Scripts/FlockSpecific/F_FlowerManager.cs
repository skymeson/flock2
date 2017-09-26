﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class F_FlowerManager : MonoBehaviour {

	public string[] trackedObjectNames;
	GameObject[] trackedObjects;
	GameObject[] networkedObjects;
	public float searchFrequency;
	public GameObject networkObject;

//	void OnConnectedToServer() {
//		
//	}
		
	void Start () {
		trackedObjects = new GameObject[trackedObjectNames.Length];
		networkedObjects = new GameObject[trackedObjectNames.Length];
		StartCoroutine (search ());
	}


	IEnumerator search(){

		for (int i = 0; i < trackedObjectNames.Length; i++) {
			float timer = Time.timeSinceLevelLoad;
			GameObject found = GameObject.Find (trackedObjectNames [i]);
			if (found == null && trackedObjects [i] != null) {
				if (networkedObjects [i] != null)
					NetworkServer.Destroy (networkedObjects [i]);
			} else if (found != null && trackedObjects [i] == null) {
				trackedObjects [i] = found;
			}
			if (found!=null && trackedObjects [i] != null && networkedObjects [i] == null) {
				if (NetworkServer.active) {
					networkedObjects [i] = (GameObject)Instantiate (networkObject, found.transform.position, found.transform.rotation);
					NetworkServer.Spawn (networkedObjects [i]);
					networkedObjects [i].GetComponent<F_CopyXForms> ().target = found.transform;
				}
			}
			timer -= Time.timeSinceLevelLoad;
			Debug.Log (timer);
		}
		yield return new WaitForSeconds (searchFrequency);
		StartCoroutine (search ());
	}

}