using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	[HideInInspector] public WerewolfController controller;

	public GameObject background;
	GameObject previouslyHighlightedCard;
	GameObject clickedCard;

	void Start() {
		controller = this.GetComponent<WerewolfController>();
	}

    void Update() {

		if (EventSystem.current)
		{
			if (EventSystem.current.IsPointerOverGameObject()) {
				return;
			}
		}

		Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		RaycastHit2D hitInfo = Physics2D.Raycast(ray, Vector2.zero, 0f);

		if (hitInfo) {
			GameObject ourHitObject = hitInfo.collider.transform.gameObject;
			if (ourHitObject.GetComponent<WerewolfPlayer>() != null && ourHitObject != clickedCard) {
				CardHover(ourHitObject);
				CardClick(ourHitObject);
			} else if (ourHitObject == background && previouslyHighlightedCard != null) {
				CardUnhover(ourHitObject);
			}
		}
    }

	void CardHover(GameObject ourHitObject) {
		controller.HighlightCard(ourHitObject, true);
		previouslyHighlightedCard = ourHitObject;
	}

	void CardUnhover(GameObject ourHitObject) {
		if (previouslyHighlightedCard != clickedCard) {
			controller.HighlightCard(ourHitObject, false);
		}
		previouslyHighlightedCard = null;
	}

    void CardClick(GameObject ourHitObject) {
		if (Input.GetMouseButtonDown(0)) {
			controller.ClickCard(ourHitObject);
			clickedCard = ourHitObject;
		}
    }
}