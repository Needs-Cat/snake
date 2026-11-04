using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds and manages a 2D array that the game is played on
/// </summary>
public class Grid : AMonoBehaviourSingleton<Grid>
{
	[SerializeField]
	private GridLayoutGroup m_gridLayoutGroup = null;
	[SerializeField]
	private Cell m_cellPrefab = null;
	
	private Cell[,] m_grid = null;
	private List<Vector2Int> m_emptyCells = new();
	
	public void Initialise()
	{
		BuildGrid();
		
		// Force GridLayout to set position for cells
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) m_gridLayoutGroup.transform);
	}

	public bool AddOccupant(Vector2Int coordinate, Occupant occupant)
	{
		return AddOccupant(coordinate.x, coordinate.y, occupant);
	}
	
	public bool AddOccupant(int x, int y, Occupant occupant)
	{
		if (!IsWithinBounds(x, y))
		{
			return false;
		}
		
		m_grid[x, y].AddOccupant(occupant);

		// Remove cell from cache as it's no longer empty
		if (m_grid[x, y].Occupant != Occupant.Empty)
		{
			m_emptyCells.Remove(new Vector2Int(x, y));
		}

		return true;
	}

	public bool RemoveOccupant(Vector2Int coordinate, Occupant occupant)
	{
		return RemoveOccupant(coordinate.x, coordinate.y, occupant);
	}

	public bool RemoveOccupant(int x, int y, Occupant occupant)
	{
		if (!IsWithinBounds(x, y))
		{
			return false;
		}

		if (!m_grid[x, y].Occupant.HasFlag(occupant))
		{
			return false;
		}

		m_grid[x, y].RemoveOccupant(occupant);

		// Add cell back to cache if it is now empty
		Vector2Int coords = new(x, y);
		if (m_grid[x, y].Occupant == Occupant.Empty && !m_emptyCells.Contains(coords))
		{
			m_emptyCells.Add(coords);
		}

		return true;
	}

	public bool MoveOccupant(Vector2Int originalCoordinate, Vector2Int newCoordinate, Occupant occupant)
	{
		return MoveOccupant(originalCoordinate.x, originalCoordinate.y, newCoordinate.x, newCoordinate.y, occupant);
	}

	public bool MoveOccupant(int originalX, int originalY, int newX, int newY, Occupant occupant)
	{
		// Start by trying to remove the current occupant
		if (!RemoveOccupant(originalX, originalY, occupant))
		{
			return false;
		}
		
		// Now try to add the new occupant
		if (!AddOccupant(newX, newY, occupant))
		{
			// Add the original occupant back - so the state is the same as before the method call
			AddOccupant(originalX, originalY, occupant);
			
			return false;
		}

		return true;
	}

	public bool ContainsOccupant(Vector2Int coordinate, Occupant occupant)
	{
		return ContainsOccupant(coordinate.x, coordinate.y, occupant);
	}
	
	public bool ContainsOccupant(int x, int y, Occupant occupant)
	{
		if (!IsWithinBounds(x, y))
		{
			return false;
		}

		return m_grid[x, y].Occupant.HasFlag(occupant);
	}

	public Vector2Int GetCentre()
	{
		int width = m_grid.GetLength(0);
		int height = m_grid.GetLength(1);

		return new(
			Mathf.RoundToInt(width * 0.5f),
			Mathf.RoundToInt(height * 0.5f));
	}

	public Vector2Int GetRandomEmptyCell()
	{
		int randomIndex = Random.Range(0, m_emptyCells.Count);
		return m_emptyCells[randomIndex];
	}

	public Vector3 GetPositionForCell(Vector2Int cell)
	{
		if (!IsWithinBounds(cell))
		{
			return Vector3.zero;
		}

		return m_grid[cell.x, cell.y].transform.position;
	}

	private void BuildGrid()
	{
		// Destroy any existing cells
		ClearCells();
		
		// Create the new cells
		Database db = GameManager.Database;
		m_grid = new Cell[db.GridSize.x, db.GridSize.y];
		int width = m_grid.GetLength(0);
		int height = m_grid.GetLength(1);

		m_gridLayoutGroup.constraintCount = width;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				m_grid[x, y] = Instantiate(m_cellPrefab, transform);
				m_grid[x, y].Initialise(x, y);

				m_emptyCells.Add(new Vector2Int(x, y));
			}
		}
	}

	private void ClearCells()
	{
		if (m_grid == null)
		{
			return;
		}
		
		int width = m_grid.GetLength(0);
		int height = m_grid.GetLength(1);

		// Destroy the cell GameObjects
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Destroy(m_grid[x, y].gameObject);
			}
		}

		m_grid = null;
		m_emptyCells.Clear();
	}

	private bool IsWithinBounds(Vector2Int coordinate)
	{
		return IsWithinBounds(coordinate.x, coordinate.y);
	}
	
	private bool IsWithinBounds(int x, int y)
	{
		return x >= 0 &&
			y >= 0 &&
			x < m_grid.GetLength(0) &&
			y < m_grid.GetLength(1);
	}
}
