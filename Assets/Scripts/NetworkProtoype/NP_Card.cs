using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NP_Card : NetworkBehaviour {

	[SyncVar]
	bool isFlipped = false;

	// Update is called once per frame
	void Update () {
		if (!hasAuthority) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			CmdCardFlip();
		}
	}

	[Command]
	void CmdCardFlip() {
		this.transform.GetChild(0).gameObject.SetActive(isFlipped);
		RpcCardFlip();
		isFlipped = !isFlipped;
	}

	[ClientRpc]
	void RpcCardFlip() {
		this.transform.GetChild(0).gameObject.SetActive(isFlipped);
	}
}
