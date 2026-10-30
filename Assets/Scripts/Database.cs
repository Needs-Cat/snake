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
}
