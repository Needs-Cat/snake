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

	private void BuildGrid()
	{
		// Destroy any existing cells
		ClearCells();
		
		// Create the new cells
		Database db = GameManager.Instance.Database;
		m_grid = new Cell[db.GridSize.x, db.GridSize.y];
		int width = m_grid.GetLength(0);
		int height = m_grid.GetLength(1);

		m_gridLayoutGroup.constraintCount = width;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				m_grid[x, y] = Instantiate(m_cellPrefab, transform);
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
