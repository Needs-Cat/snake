using System;
using UnityEngine;

/// <summary>
/// Game Runner
/// Tells all the other pieces what to do for the game to function
/// </summary>
public class GameManager : AMonoBehaviourSingleton<GameManager>
{
	public static Database Database => Instance.m_database;
	
	[SerializeField]
	private Database m_database = null;

	private void Start()
	{
		Initialise();
	}

	private void Initialise()
	{
		Grid.Instance.Initialise();
		
		// Create the player
		Vector2Int cellPos = Grid.Instance.GetCentre();
		Grid.Instance.AddOccupant(cellPos, Occupant.Head);
		
		// Just place the tail down until it reaches the bottom
		// If the default player gets longer than the length of the board - this will need to be updated
		for (int i = 1; i < Database.DefaultPlayerLength; i++)
		{
			cellPos.y -= 1;

			Grid.Instance.AddOccupant(cellPos, Occupant.Tail);
		}
	}
}
