using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Runner
/// Tells all the other pieces what to do for the game to function
/// </summary>
public class GameManager : AMonoBehaviourSingleton<GameManager>
{
	public static Database Database => Instance.m_database;

	private Vector2Int HeadCoords
	{
		get => m_snakeCoords[0];
		set => m_snakeCoords[0] = value;
	}
	private Vector2Int TailTipCoords => m_snakeCoords[^1];
	
	[SerializeField]
	private Database m_database = null;

	private List<Vector2Int> m_snakeCoords = new();
	
	private Vector2Int m_moveDirection = Vector2Int.up;

	private void Start()
	{
		Initialise();

		StartCoroutine(DoGameLoop());
	}

	private void Initialise()
	{
		Grid.Instance.Initialise();
		
		// Create the player
		m_snakeCoords.Clear();
		
		Vector2Int cellPos = Grid.Instance.GetCentre();
		Grid.Instance.AddOccupant(cellPos, Occupant.Head);
		m_snakeCoords.Add(cellPos);
		
		// Just place the tail down until it reaches the bottom
		// If the default player gets longer than the length of the board - this will need to be updated
		for (int i = 1; i < Database.DefaultPlayerLength; i++)
		{
			cellPos.y -= 1;

			Grid.Instance.AddOccupant(cellPos, Occupant.Tail);
			m_snakeCoords.Add(cellPos);
		}
	}

	public void SetMoveDirection(Vector2 swipeDirection)
	{
		// Set to normalised cardinal
		swipeDirection.Normalize();

		Vector2Int moveDirection = Vector2Int.zero;
		
		if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
		{
			// Horizontal
			moveDirection.x = Mathf.CeilToInt(swipeDirection.x);
		}
		else
		{
			// Vertical
			moveDirection.y = Mathf.CeilToInt(swipeDirection.y);
		}
		
		// Don't allow the snake to move back into itself
		Vector2Int backwardsMoveDirection = m_moveDirection * -1;
		if (moveDirection == backwardsMoveDirection)
		{
			return;
		}

		m_moveDirection = moveDirection;
	}

	private IEnumerator DoGameLoop()
	{
		float tick = Database.TickFrequency;

		while (true)
		{
			// Wait for next tick
			while (tick > 0.0f)
			{
				yield return null;

				tick -= Time.deltaTime;
			}
			tick = Database.TickFrequency;
			
			// Move the player
			if (!MovePlayer())
			{
				// Game over!
				// TODO
				Debug.Log("Game Over");
				yield break;
			}
		}
	}

	private bool MovePlayer()
	{
		// Start with the head - as this will determine if it's successful
		Vector2Int prevHeadCoords = HeadCoords;
		m_snakeCoords.Insert(0, prevHeadCoords + m_moveDirection);  // Add the head to the front
		if (!Grid.Instance.MoveOccupant(prevHeadCoords, HeadCoords, Occupant.Head))
		{
			return false;
		}
		
		// Move the tail tip up to the front to mimic the tail moving
		Vector2Int prevTailTipCoords = PopTail();
		if (!Grid.Instance.MoveOccupant(prevTailTipCoords, prevHeadCoords, Occupant.Tail))
		{
			// If the head move was successful - the tail should always succeed
			Debug.LogError($"Failed to move tail from {TailTipCoords} to {prevHeadCoords}");
			return false;
		}

		return true;
	}

	private Vector2Int PopTail()
	{
		Vector2Int coords = m_snakeCoords[^1];

		m_snakeCoords.RemoveAt(m_snakeCoords.Count - 1);

		return coords;
	}
}
