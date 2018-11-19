using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResetMode {
	soft, normal, hard
}

public enum RangeType {
	movement, spell
}

public class CellManager {

	public static List<CellScript> grid = new List<CellScript>();

	private static int nbX, nbY;

	public static void ShowMoveRange(CellScript center)
	{
		Dijkstra(center, ((CharacterScript) center.target).moveRange, 0, RangeType.movement);
	}

	private static void Dijkstra (CellScript center, int max, int min, RangeType type = RangeType.movement)
	{
		List<CellScript> current = new List<CellScript>(), next = new List<CellScript>();
		int distance = 0, i = 0;

		// Reset cells before doing Djikstra
		CellManager.ResetGrid();

		// Add all cells around the center
		current = AddToList(current, center);

		// While we have cells to check and distance < to moveRange
		while(distance < max && i != current.Count) {
			#region movement
			if(type == RangeType.movement)
			{
				// If cell hasn't been treated yet and is empty
				if (!current[i].isInMoveRange && current[i].target == null)
				{
					// We add its neighboors to the list and put it in MoveRange
					current[i].isInMoveRange = true;
					next = AddToList(next, current[i]);
				}
			}
			#endregion
			#region spell
			else if (type == RangeType.spell)
			{
				// If cell hasn't been treated yet and is empty
				if (!current[i].isInSpellRange && current[i].target == null)
				{
					// We add its neighboors to the list and put it in MoveRange
					if(distance >= min)
						current[i].isInSpellRange = true;
					next = AddToList(next, current[i]);
				}
			}
			#endregion
			#region both
			// Then we take next cell in the list
			i++;
			// If "current" is done, copy next into current and clear next 
			if(i == current.Count)
			{
				distance++;
				i = 0;
				current = new List<CellScript>(next);
				next.Clear();
			}
			#endregion
		}
	}

	private static List<CellScript> AddToList(List<CellScript> cells, CellScript center) {
		
		int next, max = nbX*nbY;
		for (int i = -1 ; i < 2 ; i += 2)
		{
			// TODO : duplicate code.

			// Index in the list = x + y*nbX;
			next = GridIndex(center.x + i, center.y);
			// If it exist (index < max) and is not already in the list
			if(next < max && next > -1 && !cells.Contains(grid[next]))
			{
				// Add it.
				cells.Add(grid[next]);
			}

			// Index in the list = x + y*nbX;
			next = GridIndex(center.x, center.y + i);
			// If it exist (index < max) and is not already in the list
			if(next < max && next > -1 && !cells.Contains(grid[next]))
			{
				// Add it.
				cells.Add(grid[next]);
			}
		}
		return cells;
	}

	private static int GridIndex (int x, int y)
	{
		if(x == nbX || x == -1) { return -1; }
		return x + y*nbX;
	}

	// Resets isInMoveRange, isInSpellRange and isInPath
	// soft = Do not reset isInMoveRange and isInSpellRange
	// hard = Also reset Selected
	public static void ResetGrid(ResetMode mode = ResetMode.normal)
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
		// Dijkstra here
		// Nope, we need a vision system.
		Dijkstra(center, spell.maxRange, spell.minRange, RangeType.spell);
	}

	// Center is assumed to be PlayerScript.selectedCell
	public static void ShowPath(CellScript target)
	{
		// Cells are set in moveRange already, so we can use that
		CellScript current = PlayerScript.instance.selectedCell;
		List<CellScript> path = new List<CellScript>(), wrong = new List<CellScript>();
		int next;

		path.Add(current);

		// TODO : GROSSE DUPLICATION DE CODE, franchement tu peut faire mieux...

		while (current != target)
		{
			if (target.y > current.y)
			{
				next = GridIndex(current.x, current.y+1);
				if (next != -1 && next < grid.Count && grid[next].isInMoveRange && !wrong.Contains(grid[next]))
				{
					current = grid[next];
                    current.isInPath = true;
					path.Add(current);
                    continue;
				}
			}	// We use "continue;" because we can't use "else if", as if "next" isn't good we need to check other cells
            if (target.y < current.y)
            {
                next = GridIndex(current.x, current.y-1);
				if (next != -1 && next < grid.Count && grid[next].isInMoveRange && !wrong.Contains(grid[next]))
				{
					current = grid[next];
                    current.isInPath = true;
					path.Add(current);
                    continue;
				}
            } 
			if (target.x > current.x)
			{
				next = GridIndex(current.x+1, current.y);
				if (next != -1 && next < grid.Count && grid[next].isInMoveRange && !wrong.Contains(grid[next]))
				{
					current = grid[next];
                    current.isInPath = true;
					path.Add(current);
                    continue;
				}
			}	// We use "continue;" because we can't use "else if", as if "next" isn't good we need to check other cells
            if (target.x < current.x)
            {
                next = GridIndex(current.x-1, current.y);
				if (next != -1 && next < grid.Count && grid[next].isInMoveRange && !wrong.Contains(grid[next]))
				{
					current = grid[next];
                    current.isInPath = true;
					path.Add(current);
                    continue;
				}
            } 
			// If none was find, add current to wrong and get back to last cell
			wrong.Add(current);
			current.isInPath = false;
    		path.Remove(current);
    		current = path[path.Count-1];
		}
	}

	public static void AddCell(CellScript cell, int x, int y)
	{
		cell.x = x;
		cell.y = y;
		grid.Add(cell);
	}

	public static void setGridSize(int x, int y)
	{
		nbX = x; nbY = y;
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