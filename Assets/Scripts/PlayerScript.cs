using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	private static PlayerScript instance;

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

	public static void SelectCell(CellScript cell)
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

		
		if(instance.selectedCell == null && cell.target != null)
		{
			Select(cell);
		} else if (instance.selectedCell != null)
		{
			if(cell.target == null)
			{
				CharacterScript target = (CharacterScript) cell.target;
				if (instance.activeSpell == null && target.InMoveRange(cell))
				{
					target.Move(cell);
					Unselect(true);
				} else if (instance.activeSpell != null && instance.activeSpell.onEmpty && instance.activeSpell.IsInRange(cell))
				{
					instance.activeSpell.launch(cell);
					Unselect(true);
				} else {
					Unselect(false);
				}
			} else if (cell.target.GetTypeOfTarget() == Type.ally)
			{
				if (instance.activeSpell != null && instance.activeSpell.onAlly && instance.activeSpell.IsInRange(cell))
				{
					instance.activeSpell.launch(cell);
					Unselect(true);
				} else {
					Unselect(false);
					Select(cell);
				}
			} else if (cell.target.GetTypeOfTarget() == Type.ennemy)
			{
				if (instance.activeSpell != null && instance.activeSpell.onEnnemy && instance.activeSpell.IsInRange(cell))
				{
					instance.activeSpell.launch(cell);
					Unselect(true);
				}
			} else if (cell.target.GetTypeOfTarget() == Type.obstacle)
			{
				if (instance.activeSpell != null && instance.activeSpell.onObstacle && instance.activeSpell.IsInRange(cell))
				{
					instance.activeSpell.launch(cell);
					Unselect(true);
				}
			}
		}
	}

	private static void Unselect(bool played){
		if (played)
			instance.selectedCell.target.canPlay = false;
		instance.selectedCell.Unselect();
		instance.selectedCell = null;
	}

	private static void Select(CellScript cell)
	{
		if(cell.target.canPlay)
		{
			instance.selectedCell = cell;
			cell.Select();
		}
	}
}
