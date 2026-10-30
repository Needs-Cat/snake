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
	
	private Occupant[,] m_data = null;
	private Cell[,] m_visuals = null;
	
	public void Initialise()
	{
		BuildGrid();
	}

	private void BuildGrid()
	{
		Database db = GameManager.Instance.Database;
		
		// Create the data
		m_data = new Occupant[db.GridSize.x, db.GridSize.y];
		
		// Destroy any existing cell visuals
		ClearCellVisuals();
		
		// Create the new cell visuals
		int width = m_data.GetLength(0);
		int height = m_data.GetLength(1);

		m_gridLayoutGroup.constraintCount = width;

		m_visuals = new Cell[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				m_visuals[x, y] = Instantiate(m_cellPrefab, transform);
			}
		}
	}

	private void ClearCellVisuals()
	{
		if (m_visuals == null)
		{
			return;
		}
		
		int width = m_visuals.GetLength(0);
		int height = m_visuals.GetLength(1);

		// Destroy the cell GameObjects
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Destroy(m_visuals[x, y].gameObject);
			}
		}

		m_visuals = null;
	}
}
