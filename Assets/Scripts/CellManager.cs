using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResetMode {
	soft, normal, hard
}

public class CellManager {

	public static List<CellScript> grid = new List<CellScript>();

	private static int nbX, nbY;

	public static void ShowMoveRange(CellScript center)
	{
		Dijkstra(center, ((CharacterScript) center.target).maxMoveRange, ((CharacterScript) center.target).minMoveRange);
	}

	public static void ShowSpellRange(CellScript center, SpellScript spell)
	{
		// Foreach to set cells to "inSpellRange"
		// If obstacle, ally, ext.. check is spell possible
		// If spell have vision,
		// Then bresenham to check if you can see it
		List<CellScript> cells = new List<CellScript>();
		int distance;

		foreach (CellScript cell in grid)
		{
			distance = center.Distance(cell);
			if(distance <= spell.minRange && distance >= spell.maxRange)
			{	
				// Conditions are inversed so we can factorize the code
				if(cell.target == null && !spell.onEmpty)
					continue;
				switch(cell.target.GetTypeOfTarget())
				{
					case Type.ally : if(!spell.onAlly) { continue; } break;
					case Type.water : if(!spell.onWater) { continue; } break;
					case Type.ennemy : if(!spell.onEnnemy) { continue; } break;
					case Type.obstacle : if(!spell.onObstacle) { continue; } break;
				}
				cell.isInSpellRange = true;
				cells.Add(cell);
			}
		}
		if(spell.needVision)
			BresenhamLine(center,cells);
	}

	private static void BresenhamLine(CellScript center, List<CellScript> cells)
	{
		// For all cell in cells
		// if cells.isinspellrange
		// draw a line
			// if it encounters a obstacle, set all following cells to false
		// end
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

	private static void Dijkstra (CellScript center, int max, int min)
	{
		List<CellScript> current = new List<CellScript>(), next = new List<CellScript>();
		int distance = 0, i = 0;

		// Reset cells before doing Djikstra
		CellManager.ResetGrid();

		// Add all cells around the center
		current = AddToList(current, center);

		// While we have cells to check and distance < to moveRange
		while(distance < max && i != current.Count) {
			// If cell hasn't been treated yet and is empty
			if (!current[i].isInMoveRange && current[i].target == null)
			{
				// We add its neighboors to the list and put it in MoveRange
				if(distance >= min)
					current[i].isInMoveRange = true;
				next = AddToList(next, current[i]);
			}
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
		}
	}

	private static List<CellScript> AddToList(List<CellScript> cells, CellScript center) {
		
		int next, max = nbX*nbY;
		for (int i = -1 ; i < 2 ; i += 2)
		{
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

	// Center is assumed to be PlayerScript.selectedCell
	public static void ShowPath(CellScript target)
	{
		// Cells are set in moveRange already, so we can use that
		CellScript current = PlayerScript.instance.selectedCell;
		List<CellScript> path = new List<CellScript>(), wrong = new List<CellScript>();
		int next;

		path.Add(current);

		while (current != target)
		{
			if (target.y > current.y)
			{
				next = GridIndex(current.x, current.y+1);
				// Check is grid[next] is valid, if not, returns null.
				if (CheckPath(next, wrong, path) != null)
				{
					current = grid[next];
					continue;
				}				
			}	// We use "continue;" because we can't use "else if", as if "next" isn't good we need to check other cells
            if (target.y < current.y)
            {
                next = GridIndex(current.x, current.y-1);
				if (CheckPath(next, wrong, path) != null)
				{
					current = grid[next];
					continue;
				}
            } 
			if (target.x > current.x)
			{
				next = GridIndex(current.x+1, current.y);
				if (CheckPath(next, wrong, path) != null)
				{
					current = grid[next];
					continue;
				}
			}
            if (target.x < current.x)
            {
                next = GridIndex(current.x-1, current.y);
				if (CheckPath(next, wrong, path) != null)
				{
					current = grid[next];
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

	private static CellScript CheckPath(int next, List<CellScript> wrong, List<CellScript> path)
	{
		if (next != -1 && next < grid.Count && grid[next].isInMoveRange && !wrong.Contains(grid[next]))
		{
			grid[next].isInPath = true;
			path.Add(grid[next]);
			return grid[next];
		}
		return null;
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