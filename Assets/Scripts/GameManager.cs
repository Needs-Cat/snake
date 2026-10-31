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
	}
}
