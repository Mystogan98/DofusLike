using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellScript : MonoBehaviour {
	public bool onEmpty, onAlly, onEnnemy, onObstacle, onWater, needVision = true;

	public int minRange, maxRange;

	public abstract void Launch(CellScript cell);
}
