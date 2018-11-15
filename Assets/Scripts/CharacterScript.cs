using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : TargetScript {

	public new Type type;

	private SpellScript spell; 	// I don't know how many yet

	public void Move(CellScript target)
	{

	}

	public bool InMoveRange(CellScript target)
	{
		// Traitement
		return false;
	}
}
