using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains balancing information to be edited
/// </summary>
[CreateAssetMenu(fileName = "Database", menuName = "Snake/Database")]
public class Database : ScriptableObject
{
	[Header("Player")]
	[SerializeField]
	private int m_defaultPlayerLength = 3;
	public int DefaultPlayerLength => m_defaultPlayerLength;

	[Header("Grid")]
	[SerializeField]
	private Vector2Int m_gridSize = Vector2Int.zero;
	public Vector2Int GridSize => m_gridSize;
	
	[SerializeField]
	private List<Occupant> m_occupantDisplayPriority = new();
	[SerializeField]
	private SerializableDictionary<Occupant, Color> m_cellColours = new();
	
	[Header("Tick Frequency")]
	[SerializeField]
	private float m_baseTickFrequency = 1.0f;
	[SerializeField]
	private float m_minTickFrequency = 0.05f;
	[SerializeField, Tooltip("How much faster (in seconds) the tick frequency gets for every fruit eaten")]
	private float m_tickFreqPerScoreModifier = 0.05f;

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

	public float GetTickFrequency(float score)
	{
		float modifier = m_tickFreqPerScoreModifier * score;
		float tickFreq = m_baseTickFrequency - modifier;
		
		// No lower than the min
		tickFreq = Mathf.Max(tickFreq, m_minTickFrequency);

		return tickFreq;
	}
}
