using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WerewolfPlayer : MonoBehaviour {
	
	public WerewolfCard role;

	[HideInInspector] public bool playerIsActive;
	public bool isMain;

	public bool isMiddle;

	public void FlipCard(bool isFlipped) {
		this.transform.GetChild(0).gameObject.SetActive(!isFlipped);
	}

	void ClickHighlight(bool highlight) {
		if (highlight) {
			this.transform.GetChild(1).gameObject.SetActive(true);
			MeshRenderer mesh = this.transform.GetChild(1).GetComponent<MeshRenderer>();
			mesh.material.color = new Color(255, 128, 0, 255);
		}
	}
}