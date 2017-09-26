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
	bool doSearch = true;
	GameObject found;

	void Start () {
		trackedObjects = new GameObject[trackedObjectNames.Length];
		networkedObjects = new GameObject[trackedObjectNames.Length];
		StartCoroutine (search ());
	}

	void Update(){
		if (doSearch) {
			finder ();
			doSearch = false;
			StartCoroutine (search ());
		}
	}

	void finder(){
		found = null;
		for (int i = 0; i < trackedObjectNames.Length; i++) {
			found = GameObject.Find (trackedObjectNames [i]);
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
					Debug.Log("networkclient" + NetworkClient.active);
					CmdInstanceNetworkObject(i ,
//						networkObject,
//						found,
						GameObject.FindObjectOfType<F_IsLocalPlayer>().gameObject.GetComponent<NetworkIdentity>()
//						connectionToClient
					);
					//Debug.Log(g);
					Debug.Log("networrrrkkkk");
//					Debug.Log(this.GetComponent<NetworkIdentity>().connectionToClient);


					//NetworkServer.SpawnWithClientAuthority( networkedObjects [i],g);

					//NetworkServer.Spawn(networkedObjects[i]);
					//networkedObjects [i].GetComponent<F_CopyXForms> ().target = found.transform;
				}
			}
		}
	}

	IEnumerator search(){
		
		yield return new WaitForSeconds (searchFrequency);
		doSearch = true;
	}

    [Command]
	public void CmdInstanceNetworkObject(int which, NetworkIdentity id){//int which, GameObject toInstance, GameObject found, NetworkConnection conn) {
//		Debug.Log(conn);

		Debug.Log (GameObject.FindObjectOfType<F_IsLocalPlayer>().gameObject.GetComponent<NetworkIdentity>().connectionToClient);
//        GameObject g = GameObject.FindObjectOfType<F_IsLocalPlayer>().gameObject;
        networkedObjects[which] = (GameObject)Instantiate(networkObject, this.transform.position, this.transform.rotation);
//		GameObject g = (GameObject)Instantiate(networkObject,null);
		NetworkServer.SpawnWithClientAuthority( networkObject,id.connectionToClient);
//        NetworkServer.Spawn(networkedObjects[which]);
        networkedObjects[which].GetComponent<F_CopyXForms>().target = found.transform;
//        Debug.Log("spawned " + networkedObjects[which]);
    }
}
