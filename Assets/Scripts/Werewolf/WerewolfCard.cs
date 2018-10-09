using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Card", menuName="Werewolf/Card")]
public class WerewolfCard :ScriptableObject {

	public new string name;
	public string description;

	public Sprite artwork;

	public enum Team {Town, Werewolf};
	public Team team;
}
