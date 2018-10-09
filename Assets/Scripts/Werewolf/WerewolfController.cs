using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WerewolfController : MonoBehaviour {

	public GameObject playerCard;
	PlayerController playerController;
	float timer;

	public Text timerUI;
	public Text phaseUI;

	private GameObject previouslyHighlightedCard;
//	private GameObject highlightedCard;
	private GameObject previouslyclickedCard;
	private GameObject clickedCard;
	private WerewolfPlayer votedCard;

	bool canClick;
	bool canHighlight;
	int middleCardClickCount;

	WerewolfPlayer mainPlayer;

	void Start () {
		mainPlayer = playerCard.GetComponent<WerewolfPlayer>();
		playerController = this.GetComponent<PlayerController>();

		playerController.AssginPlayers();

		canClick = false;
		canHighlight = false;
		middleCardClickCount = 0;
	}

	void Update() {
		timer +=Time.deltaTime;
		timerUI.text = Mathf.RoundToInt(timer).ToString();
		if (Mathf.RoundToInt(timer) < 6 ) {
			InitialFlip();
		}
		//Start nighttime events
		else if (Mathf.RoundToInt(timer) < 30) {
			Nighttime();
		}
		else if (Mathf.RoundToInt(timer) >= 30) {
			Day();
		}
	}

	void InitialFlip() {
		phaseUI.text = "InitialFlip";
		mainPlayer.FlipCard(true);
		if (Mathf.RoundToInt(timer) == 5) {
			mainPlayer.FlipCard(false);
		}
	}
	
	void Nighttime() {

		//Werewolf phase
		if (Mathf.RoundToInt(timer) < 10) {
			phaseUI.text = "WerewolfPhase";
			if (mainPlayer.role.name == "Werewolf") {
				WerewolfPhase();
			}
		}
		//Seer Phase
		else if (Mathf.RoundToInt(timer) < 15) {
			phaseUI.text = "SeerPhase";
			if (mainPlayer.role.name == "Seer") {
				canHighlight = true;
				canClick = true;
				SeerPhase();
			}
		}
		//RobberPhase
		else if (Mathf.RoundToInt(timer) < 20) {
			phaseUI.text = "RobberPhase";
			if (mainPlayer.role.name == "Robber") {
				canHighlight = true;
				canClick = true;
				RobberPhase();
			}
			
		}
		//TroublemakerPhase
		else if (Mathf.RoundToInt(timer) < 25) {
			phaseUI.text = "TroubleMakerPhase";
			if (mainPlayer.role.name == "Troublemaker") {
				canHighlight = true;
				canClick = true;
				TroubleMakerPhase();
			}
		}
	}

	void Day() {
		if (Mathf.RoundToInt(timer) < 59) {
			phaseUI.text = "Discuss and Vote";
			canClick = true;
			canHighlight = true;
			//Audio Queue

			//People vote for who to kill
			if (canClick && clickedCard != null && !clickedCard.GetComponent<WerewolfPlayer>().isMain 
				&& !clickedCard.GetComponent<WerewolfPlayer>().isMiddle) {
				votedCard = clickedCard.GetComponent<WerewolfPlayer>();
			}
		}
		
		
		//Winner is revealed
		if (Mathf.RoundToInt(timer) > 60) {
			mainPlayer.FlipCard(true);
			foreach (WerewolfPlayer player in playerController.players) {
				player.FlipCard(true);
			}
			foreach(WerewolfPlayer middlecard in playerController.middleCards) {
				middlecard.FlipCard(true);
			}
			if (votedCard.role.team == WerewolfCard.Team.Werewolf) {
				phaseUI.text = "The Town Wins";
			} else {
				phaseUI.text = "The Werewolves Win";
			}
			
		}
		//Score Added
	}
	
	void WerewolfPhase() {
		if (Mathf.RoundToInt(timer) < 9) {
			//Flip werewolves
			if (mainPlayer.role.name == "Werewolf") {
				mainPlayer.FlipCard(true);
			}
			
			foreach (WerewolfPlayer player in playerController.players) {
				if (player.role.name == "Werewolf" && !player.isMiddle) {
					player.FlipCard(true);
				}
			}
		}

		//Flipback werewolves
		if (Mathf.RoundToInt(timer) == 9) {
			if (mainPlayer.role.name == "Werewolf")
				mainPlayer.FlipCard(false);
			
			foreach (WerewolfPlayer player in playerController.players) {
				if (player.role.name == "Werewolf") {
					player.FlipCard(false);
				}
			}
		}
	}

	void SeerPhase() {
		if (Mathf.RoundToInt(timer) < 14) {
			if (canClick && clickedCard != null && !clickedCard.GetComponent<WerewolfPlayer>().isMain) {
				if (clickedCard.GetComponent<WerewolfPlayer>() != null) {
					if (clickedCard.GetComponent<WerewolfPlayer>().isMiddle && clickedCard != previouslyclickedCard && middleCardClickCount < 2) {
						clickedCard.GetComponent<WerewolfPlayer>().FlipCard(true);
						previouslyclickedCard = clickedCard;
						middleCardClickCount++;
					} else if (middleCardClickCount == 2) {
						canClick = false;
						canHighlight = false;
					}  else if (!clickedCard.GetComponent<WerewolfPlayer>().isMiddle && middleCardClickCount == 0) {
						clickedCard.GetComponent<WerewolfPlayer>().FlipCard(true);
						canClick = false;
						canHighlight = false;
					}
				}
			}
		}

		if (Mathf.RoundToInt(timer) == 14) {
			canClick = false;
			canHighlight = false;
			foreach (WerewolfPlayer player in playerController.players) {
				player.FlipCard(false);
			}
			foreach (WerewolfPlayer player in playerController.middleCards) {
				player.FlipCard(false);
			}
			ResetCards();	
		}
	}

	void RobberPhase() {
		if (Mathf.RoundToInt(timer) < 19) {
			if (canClick && clickedCard != null && clickedCard != previouslyclickedCard && !clickedCard.GetComponent<WerewolfPlayer>().isMain && !clickedCard.GetComponent<WerewolfPlayer>().isMiddle) {
				WerewolfCard robberRole = clickedCard.GetComponent<WerewolfPlayer>().role;
				clickedCard.GetComponent<WerewolfPlayer>().role = mainPlayer.role;
				mainPlayer.role = robberRole;
				previouslyclickedCard = clickedCard;
				canClick = false;
				canHighlight = false;
				mainPlayer.FlipCard(true);
			}
		}
		if (Mathf.RoundToInt(timer) == 19) {
			mainPlayer.FlipCard(false);
			ResetCards();
		}
	}

	void TroubleMakerPhase() {
		if (Mathf.RoundToInt(timer) < 24) {
			if (canClick && clickedCard != null && clickedCard != previouslyclickedCard && !clickedCard.GetComponent<WerewolfPlayer>().isMain && !clickedCard.GetComponent<WerewolfPlayer>().isMiddle) {
				if (previouslyclickedCard != null) {
					WerewolfCard newRole = previouslyclickedCard.GetComponent<WerewolfPlayer>().role;
					previouslyclickedCard.GetComponent<WerewolfPlayer>().role = clickedCard.GetComponent<WerewolfPlayer>().role;
					clickedCard.GetComponent<WerewolfPlayer>().role = newRole;
					canClick = false;
					canHighlight = false;
					previouslyclickedCard = clickedCard;
				} else {
					previouslyclickedCard = clickedCard;					
				}
			}
		}

		if (Mathf.RoundToInt(timer) == 25) {
			ResetCards();
			canClick = false;
			canHighlight = false;
		}
	}

	public void HighlightCard(GameObject hitObject, bool hover) {
		if (canHighlight) {
			if (hover) {
				hitObject.transform.GetChild(1).gameObject.SetActive(true);
				MeshRenderer mesh = hitObject.transform.GetChild(1).GetComponent<MeshRenderer>();
				mesh.material.color = new Color(255, 128, 0, 255);
				previouslyHighlightedCard = hitObject;
			} else {
				if (previouslyHighlightedCard != clickedCard){
					previouslyHighlightedCard.transform.GetChild(1).gameObject.SetActive(false);
				}
				previouslyHighlightedCard = null;
			}
		}
	}

	public void ClickCard(GameObject hitObject) {
		if (canClick) {
			if (clickedCard != null) {
				clickedCard.transform.GetChild(1).gameObject.SetActive(false);
			}
			hitObject.transform.GetChild(1).gameObject.SetActive(true);
			MeshRenderer mesh = hitObject.transform.GetChild(1).GetComponent<MeshRenderer>();
			mesh.material.color = Color.red;
			clickedCard = hitObject;
		}
	}

	void ResetCards() {
		previouslyHighlightedCard = null;
//		highlightedCard = null;
		previouslyclickedCard = null;
		clickedCard = null;
	}
}
