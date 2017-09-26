﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class F_FlowerManager : NetworkBehaviour {

	public string[] trackedObjectNames;
	GameObject[] trackedObjects;
	GameObject[] networkedObjects;
	public float searchFrequency;
	public GameObject networkObject;
	public SettingsManager2 settings;

	void Start () {
		trackedObjects = new GameObject[trackedObjectNames.Length];
		networkedObjects = new GameObject[trackedObjectNames.Length];
		StartCoroutine (search ());
	}

	IEnumerator search(){
		for (int i = 0; i < trackedObjectNames.Length; i++) {
			GameObject found = GameObject.Find (trackedObjectNames [i]);
			if (found == null && trackedObjects [i] != null) {
				if (networkedObjects [i] != null)
					NetworkServer.Destroy (networkedObjects [i]);
			} else if (found != null && trackedObjects [i] == null) {
				trackedObjects [i] = found;
			}
//			if (found!=null && trackedObjects [i] != null && networkedObjects [i] == null) {
//				if (NetworkServer.active) {// || NetworkClient.active) {
//                    InstanceNetworkObject(i, 
//                        networkObject,
//                        found);
//                    //Debug.Log(g);
//                    Debug.Log("networrrrkkkk");
//
//                    //NetworkServer.SpawnWithClientAuthority( networkedObjects [i],g);
//
//                    //NetworkServer.Spawn(networkedObjects[i]);
//                    //networkedObjects [i].GetComponent<F_CopyXForms> ().target = found.transform;
//				}
//			}
			if (found!=null && trackedObjects [i] != null && networkedObjects [i] == null) {
				if (NetworkClient.active) {// || NetworkClient.active) {
					CmdInstanceNetworkObject(i, 
						networkObject,
						found);
					//Debug.Log(g);
					Debug.Log("networrrrkkkk");

					//NetworkServer.SpawnWithClientAuthority( networkedObjects [i],g);

					//NetworkServer.Spawn(networkedObjects[i]);
					//networkedObjects [i].GetComponent<F_CopyXForms> ().target = found.transform;
				}
			}
		}
		yield return new WaitForSeconds (searchFrequency);
		StartCoroutine (search ());
	}

    [Command]
    public void CmdInstanceNetworkObject(int which, GameObject toInstance, GameObject found) {
//        GameObject g = GameObject.FindObjectOfType<F_IsLocalPlayer>().gameObject;
        networkedObjects[which] = (GameObject)Instantiate(networkObject, found.transform.position, found.transform.rotation);
		NetworkServer.SpawnWithClientAuthority( networkedObjects [which],this.connectionToClient);
//        NetworkServer.Spawn(networkedObjects[which]);
        networkedObjects[which].GetComponent<F_CopyXForms>().target = found.transform;
//        Debug.Log("spawned " + networkedObjects[which]);
    }
}
