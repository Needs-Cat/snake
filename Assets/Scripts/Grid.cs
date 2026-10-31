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
	
	public void Initialise()
	{
		BuildGrid();
	}

	public bool AddOccupant(Vector2Int coordinate, Occupant occupant)
	{
		return AddOccupant(coordinate.x, coordinate.y, occupant);
	}
	
	public bool AddOccupant(int x, int y, Occupant occupant)
	{
		if (x >= m_grid.GetLength(0) ||
			y >= m_grid.GetLength(1))
		{
			// Outside range of grid
			return false;
		}
		
		m_grid[x, y].AddOccupant(occupant);

		return true;
	}

	public bool RemoveOccupant(Vector2Int coordinate, Occupant occupant)
	{
		return RemoveOccupant(coordinate.x, coordinate.y, occupant);
	}

	public bool RemoveOccupant(int x, int y, Occupant occupant)
	{
		if (x >= m_grid.GetLength(0) ||
			y >= m_grid.GetLength(1))
		{
			// Outside range of grid
			return false;
		}

		if (!m_grid[x, y].Occupant.HasFlag(occupant))
		{
			return false;
		}

		m_grid[x, y].RemoveOccupant(occupant);

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

	public Vector2Int GetCentre()
	{
		int width = m_grid.GetLength(0);
		int height = m_grid.GetLength(1);

		return new(
			Mathf.RoundToInt(width * 0.5f),
			Mathf.RoundToInt(height * 0.5f));
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
	}
}
