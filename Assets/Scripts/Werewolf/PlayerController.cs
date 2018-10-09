using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	[Range(3,10)] public int numberOfPlayers = 3;
	int activeCardCount;

	public WerewolfCard[] namedRoles;
	public WerewolfCard extraTownRole;

	[Range(1,2)] public int numberOfWerewolves = 2;
	public WerewolfCard extraWerewolfRole;
	public readonly int[] playerOrder = {4,8,2,10,6,3,9,5,7};
	[HideInInspector] public WerewolfPlayer[] players;

	[HideInInspector] public WerewolfPlayer[] middleCards;
	WerewolfCard[] allRoles;

	public void AssginPlayers() {
		activeCardCount = numberOfPlayers + 3;
		players = new WerewolfPlayer[numberOfPlayers];
		players[0] = this.transform.GetChild(0).gameObject.GetComponent<WerewolfPlayer>();

		for (int i = 1; i <= playerOrder.Length; i++) {
			if (i < numberOfPlayers) {
				WerewolfPlayer player = this.transform.GetChild((playerOrder[i -1])-1).gameObject.GetComponent<WerewolfPlayer>(); 
				players[i] = player;
			}
			else {
				this.transform.GetChild((playerOrder[i -1])-1).gameObject.SetActive(false);
			}
		}
		//Assign cards in the middle
		middleCards = new WerewolfPlayer[3];
		for (int i = 0; i < 3; i++) {
			middleCards[i] = this.transform.GetChild((this.transform.childCount - i) - 1).gameObject.GetComponent<WerewolfPlayer>();
		}
		AssignRoles();
		AddArtwork();
	}

	void AssignRoles() {
		//Create new array adding in the extra roles and werewolves.
		allRoles = new WerewolfCard[activeCardCount];
		int werewolfIndex = numberOfWerewolves;
		for (int i =0; i < (activeCardCount); i ++) {
			if (i < namedRoles.Length)
				allRoles[i] = namedRoles[i];
			else if (werewolfIndex > 0) {
				allRoles[i] = extraWerewolfRole;
				werewolfIndex--;
			}
			else
				allRoles[i] = extraTownRole;
		}
		//Randomize array ready to have lists assigned
		new System.Random().Shuffle(allRoles);
	
		//Cyclethrough players assigning roles and voiding that number
		int middleCardIndex = 0;
		for (int i =0; i < numberOfPlayers + 3; i ++) {
			if (i < numberOfPlayers)
				players[i].role = allRoles[i];
			else {
				middleCards[middleCardIndex].role = allRoles[i];
				middleCardIndex++;
			}
		}
	}

	void AddArtwork() {
		foreach (Transform card in transform) {
			if (card.gameObject.activeSelf) {
				card.GetComponent<SpriteRenderer>().sprite = card.GetComponent<WerewolfPlayer>().role.artwork;
			}
		}
	}
}
static class RandomExtensions
{
    public static void Shuffle<T> (this System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
