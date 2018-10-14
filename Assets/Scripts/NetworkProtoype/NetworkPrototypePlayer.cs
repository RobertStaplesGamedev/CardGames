using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPrototypePlayer : NetworkBehaviour {

	public GameObject playerCard;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			return;
		}
		CmdSpawnCard();
	}

	// Update is called once per frame
	void Update () {
		
	}
	[Command]
	void CmdSpawnCard() {
		GameObject go = Instantiate(playerCard);
		NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	}
}
