using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : TargetScript {

	[Header("Debug variables")]
	public bool alwaysPlayable = false;
	[Space(20)]

	// TODO : See if you can hide "obstacle"
	public Type typeOfCharacter = Type.ally;
	public int moveRange = 3;

	private SpellScript spell; 	// I don't know how many yet
	private CellScript parent;

	private void Start() {
		_canPlay = true;
		type = typeOfCharacter;
		UpdateParent();
	}

	public void Move(CellScript target)
	{
		if(target.isInMoveRange)
		{
			// remove from parent
			parent.target = null;
			// move to target
			target.target = this;
			transform.SetParent(target.transform,false);
			UpdateParent();
			if(!alwaysPlayable)
				canPlay = false;
			CellManager.ResetGrid(ResetMode.normal);
		}
	}

	private void LateUpdate() {
		if(!canPlay)
			GetComponent<SpriteRenderer>().color = Color.gray;
		else
			GetComponent<SpriteRenderer>().color = Color.white;
	}

	public bool InMoveRange(CellScript target)
	{
		// Traitement
		return true;
	}

	private void UpdateParent()
	{
		parent = transform.parent.GetComponent<CellScript>();
	}
}