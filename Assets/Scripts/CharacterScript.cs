using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : TargetScript {

	// TODO : See if you can hide "obstacle"
	public new Type type;

	private SpellScript spell; 	// I don't know how many yet
	private CellScript parent;

	private void Start() {
		UpdateParent();
	}

	public void Move(CellScript target)
	{
		Debug.Log("Move");
		// remove from parent
		parent.target = null;
		// move to target
		target.target = this;
		transform.SetParent(target.transform,false);
		UpdateParent();
		canPlay = false;
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