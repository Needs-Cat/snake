using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains balancing information to be edited
/// </summary>
[CreateAssetMenu(fileName = "Database", menuName = "Snake/Database")]
public class Database : ScriptableObject
{
	public Vector2Int GridSize => m_gridSize;
	[SerializeField]
	private Vector2Int m_gridSize = Vector2Int.zero;

	public int DefaultPlayerLength => m_defaultPlayerLength;
	[SerializeField]
	private int m_defaultPlayerLength = 3;

	[SerializeField]
	private List<Occupant> m_occupantDisplayPriority = new();
	
	[SerializeField]
	private SerializableDictionary<Occupant, Color> m_cellColours = new();

	public Color GetCellColour(Occupant cellOccupant)
	{
		foreach (Occupant occupantPriority in m_occupantDisplayPriority)
		{
			if (cellOccupant.HasFlag(occupantPriority))
			{
				return m_cellColours[occupantPriority];
			}
		}

		// Priority order, in case the cell has multiple occupants
		return m_cellColours[Occupant.Empty];
	}
}
