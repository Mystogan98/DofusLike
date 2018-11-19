using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	[HideInInspector]
	public static PlayerScript instance;

	private CellScript selectedCell;
	private SpellScript activeSpell;

	// Use this for initialization
	void Start () {
		if(instance == null)
			instance = this;
		else
			Destroy(this);
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
			} else if (cell.target.GetTypeOfTarget() == Type.water) {
				Unselect(false);
			}
		}
	}

	// TODO : CanPlay = false here or in Character ?
	private void Unselect(bool played){
		// if (played && selectedCell.target != null)
		// 	selectedCell.target.canPlay = false;
		selectedCell.Unselect();
		selectedCell = null;
		CellManager.ResetGrid(ResetMode.normal);
	}

	private void Select(CellScript cell)
	{
		if(cell.target.canPlay)
		{
			selectedCell = cell;
			cell.Select();

			CellManager.ShowMoveRange(selectedCell);
		}
	}

	private void SelectSpell(SpellScript spell)
	{
		activeSpell = spell;
		CellManager.ShowSpellRange(selectedCell,spell);
	}
}