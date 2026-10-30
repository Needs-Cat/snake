using UnityEngine;

/// <summary>
/// Visualiser of a single cell on the grid
/// </summary>
public class Cell : MonoBehaviour
{
	public Occupant Occupant { get; set; } = Occupant.Empty;
}
