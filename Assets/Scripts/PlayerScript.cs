using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public static PlayerScript instance;

	private CellScript selectedCell, aimedCell;
	private SpellScript activeSpell;

	// Use this for initialization
	void Start () {
		if(instance == null)
			instance = this;
		else
			Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectCell(CellScript cell)
	{
		// TargetScript (abstract)
		// |-> Character
			// SpellScript (abstract)
		// |-> Obstacle

		// No cell
			// Select if cell has a target
		// Already have a cell selected
			// Empty (target == null)
				// Spell NOT active & In move range ? => Move & unselect
				// Spell active & spell empty possible & in range ? => Fire spell & unselect
				// ELSE => unselect
			// Ally (target.type == ally)
				// Spell active & spell ally possible & in range => fire spell & unselect
				// ELSE unselect & select
			// Ennemy (target.type == ennemy)
				// Spell active & spell ennemy possible & in range => fire spell & unselect
				// (unselect & select)	// to show ennemy stats ?
			// blocked (target.type == blocked)
				// Spell active & spell blocked possible & in range => fire spell & unselect

		// when "unselect" => set character to "done"

		
		if(selectedCell == null && cell.target != null && cell.target.canPlay)
		{
			Select(cell);
		} else if (selectedCell != null)
		{
			if(cell.target == null)
			{
				CharacterScript target = (CharacterScript) selectedCell.target;
				if (activeSpell == null && target.InMoveRange(cell))
				{
					target.Move(cell);
					Unselect(true);
				} else if (activeSpell != null && activeSpell.onEmpty && activeSpell.IsInRange(cell))
				{
					activeSpell.launch(cell);
					Unselect(true);
				} else {
					Unselect(false);
				}
			} else if (cell.target.GetTypeOfTarget() == Type.ally)
			{
				if (activeSpell != null && activeSpell.onAlly && activeSpell.IsInRange(cell))
				{
					activeSpell.launch(cell);
					Unselect(true);
				} else {
					Unselect(false);
					Select(cell);
				}
			} else if (cell.target.GetTypeOfTarget() == Type.ennemy)
			{
				if (activeSpell != null && activeSpell.onEnnemy && activeSpell.IsInRange(cell))
				{
					activeSpell.launch(cell);
					Unselect(true);
				}
			} else if (cell.target.GetTypeOfTarget() == Type.obstacle)
			{
				if (activeSpell != null && activeSpell.onObstacle && activeSpell.IsInRange(cell))
				{
					activeSpell.launch(cell);
					Unselect(true);
				}
			}
		}
	}

	private void Unselect(bool played){
		if (played && selectedCell.target != null)
			selectedCell.target.canPlay = false;
		selectedCell.Unselect();
		selectedCell = null;
	}

	private void Select(CellScript cell)
	{
		if(cell.target.canPlay)
		{
			selectedCell = cell;
			cell.Select();
		}
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