using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Test : SpellScript {

	private void Start() {
		onEmpty = true;
		onAlly = true;
		onEnnemy = true;
		onObstacle = true;
		onWater = true;
		needVision = true;
		minRange = 1;
		maxRange = 7;
	}

	public override void Launch(CellScript target)
	{
		Debug.Log("Do something.");
		return;
	}
}
