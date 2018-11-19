using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResetMode {
	soft, normal, hard
}

public class CellManager : MonoBehaviour {

	[HideInInspector]
	public static List<CellScript> grid = new List<CellScript>();

	public static void ShowMoveRange(CellScript center)
	{
		// Djiskstra here

		// FOR NOW just show all cells around him
		int moveRange = center.target.GetComponent<CharacterScript>().moveRange;
		foreach(CellScript cell in grid)
		{
			if(center.Distance(cell) <= moveRange)
			{
				cell.isInMoveRange = true;
			}
		}
	}

	// Resets isInMoveRange, isInSpellRange and isInPath
	// soft = Do not reset isInMoveRange and isInSpellRange
	// hard = Also reset Selected
	public static void ResetGrid(ResetMode mode)
	{
		// if "soft", reset only isInPath
		// if "hard", reset selected too
		foreach(CellScript cell in grid)
		{
			cell.isInPath = false;
			if(mode != ResetMode.soft)
			{
				cell.isInMoveRange = false;
				cell.isInSpellRange = false;
			} else if (mode == ResetMode.hard) {
				cell.selected = false;
			}
		}
	}

	public static void ShowSpellRange(CellScript center, SpellScript spell)
	{
		// Djistra here
		// Nope, we need a vision system.
	}

	// Center is assumed to be PlayerScript.selectedCell
	public static void ShowPath(CellScript target)
	{
		// A* here
	}

	public static void AddCell(CellScript cell, int x, int y)
	{
		cell.x = x;
		cell.y = y;
		grid.Add(cell);
	}
}


// DISJKTRA
// Stack case retenus
// liste case non utile
// case de depart
// case d'arrivé
// case = {x + y} ; valeur = x+y ; distance = |valeur_depart - valeur_arrivé|

// On part de la case de départ en direction de la case d'arrivé
// on met la première case dans la stack, puis on prend la case suivante
// Si la case ne mene a rien, on reviens en arrière dans la stack et on met cette case dans la liste
// si case pas dans la liste, alors on met dans la stack et on continue

// si distance > portée FALSE
// TRUE