using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Visualiser of a single cell on the grid
/// </summary>
public class Cell : MonoBehaviour
{
	public Occupant Occupant { get; private set; } = Occupant.Empty;

	[SerializeField]
	private Image m_image = null;
	[SerializeField]
	private TextMeshProUGUI m_debugCoordsText = null;

	public void Initialise(int x, int y)
	{
		m_debugCoordsText.text = $"({x}, {y})";
		m_debugCoordsText.gameObject.SetActive(false);
	}
	
	public void AddOccupant(Occupant occupant)
	{
		Occupant |= occupant;

		Refresh();
	}
	
#if UNITY_EDITOR
	
	private void Update()
	{
		// Debug - toggle coordinates
		if (Keyboard.current.tKey.wasPressedThisFrame)
		{
			m_debugCoordsText.gameObject.SetActive(!m_debugCoordsText.gameObject.activeSelf);
		}
	}
	
#endif

	private void Refresh()
	{
		// Update colour to represent what's in the cell
		m_image.color = GameManager.Database.GetCellColour(Occupant);
	}
}
