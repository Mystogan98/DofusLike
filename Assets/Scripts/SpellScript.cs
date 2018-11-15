using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellScript {
	public bool onEmpty, onAlly, onEnnemy, onObstacle;

	private bool range;

	public abstract void launch(CellScript cell);

	public bool IsInRange(CellScript cell)
	{
		// Traitement
		return false;
	}
}
