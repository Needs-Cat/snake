
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Runner
/// Tells all the other pieces what to do for the game to function
/// </summary>
public class GameManager : AMonoBehaviourSingleton<GameManager>
{
	public static Database Database => Instance.m_database;
	public bool IsPaused => m_isPaused;

	private Vector2Int HeadCoords => m_snakeCoords[0];
	private Vector2Int TailTipCoords => m_snakeCoords[^1];
	
	[SerializeField]
	private Database m_database = null;

	[SerializeField]
	private UnityEvent<bool> m_onPaused = new();

	private List<Vector2Int> m_snakeCoords = new();
	
	private Vector2Int m_moveDirection = Vector2Int.up;
	private Vector2Int m_prevMoveDirection = Vector2Int.up;
	private Vector2Int m_prevTailCoords = Vector2Int.zero;

	private bool m_isPaused = false;
	
	private int m_score = 0;

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
		
		// Create the head
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
		
		// Spawn the first fruit
		SpawnFruit();

		// Ensure the game is properly un-paused
		PauseGame(false);
	}

	public void SetMoveDirection(Vector2 swipeDirection)
	{
		// Set to normalised cardinal
		swipeDirection.Normalize();

		Vector2Int moveDirection = Vector2Int.zero;
		
		if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
		{
			// Horizontal - Don't round to 0!
			moveDirection.x = (swipeDirection.x > 0.0f) ? Mathf.CeilToInt(swipeDirection.x) : Mathf.FloorToInt(swipeDirection.x);
		}
		else
		{
			// Vertical - Don't round to 0!
			moveDirection.y = (swipeDirection.y > 0.0f) ? Mathf.CeilToInt(swipeDirection.y) : Mathf.FloorToInt(swipeDirection.y);
		}
		
		// Don't allow the snake to move back into itself
		Vector2Int backwardsMoveDirection = m_prevMoveDirection * -1;
		if (moveDirection == backwardsMoveDirection)
		{
			return;
		}

		m_moveDirection = moveDirection;
	}

	public void PauseGame(bool paused)
	{
		m_isPaused = paused;

		m_onPaused.Invoke(m_isPaused);
	}

	private IEnumerator DoGameLoop()
	{
		float tick = Database.GetTickFrequency(m_score);

		while (true)
		{
			// Wait for next tick
			while (tick > 0.0f)
			{
				yield return null;

				tick -= Time.deltaTime;

				// Stop the game loop while we're paused
				while (m_isPaused)
				{
					yield return null;
				}
			}
			
			// Move the player
			if (!MovePlayer())
			{
				// Game over!
				DoGameOver();
				yield break;
			}
			
			// Try to collect, and respawn, fruit
			TryCollectFruit();
			
			// Update the tick timer after collecting fruit - so we get the latest balancing
			tick = Database.GetTickFrequency(m_score);
		}
	}
	
	private void DoGameOver()
	{
		// Compare and cache high score
		int highScore = PlayerPrefs.GetInt(PlayerPrefKeys.HIGH_SCORE, 0);
		if (m_score > highScore)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.HIGH_SCORE, m_score);
			PlayerPrefs.SetInt(PlayerPrefKeys.NEW_HIGH_SCORE, 1);
		}
		else
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.NEW_HIGH_SCORE, 0);
		}
		
		// Cache score for next scene
		PlayerPrefs.SetInt(PlayerPrefKeys.SCORE, m_score);

		SceneManager.LoadScene(SceneNames.RESULTS);
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
		m_prevTailCoords = PopTail();
		if (!Grid.Instance.MoveOccupant(m_prevTailCoords, prevHeadCoords, Occupant.Tail))
		{
			// If the head move was successful - the tail should always succeed
			Debug.LogError($"Failed to move tail from {TailTipCoords} to {prevHeadCoords}");
			return false;
		}

		m_prevMoveDirection = m_moveDirection;
		
		return true;
	}

	private void TryCollectFruit()
	{
		if (!Grid.Instance.ContainsOccupant(HeadCoords, Occupant.Fruit))
		{
			return;
		}
		
		// Collect the fruit!
		Grid.Instance.RemoveOccupant(HeadCoords, Occupant.Fruit);

		// Grow tail
		PushTail();
		
		// Increase score - this will also increase tick rate
		m_score++;

		SpawnFruit();
	}

	private void PushTail()
	{
		// Spawn the tail just behind the last one
		if (!Grid.Instance.AddOccupant(m_prevTailCoords, Occupant.Tail))
		{
			Debug.LogError($"Failed to spawn tail at {m_prevTailCoords}");
			return;
		}

		m_snakeCoords.Add(m_prevTailCoords);
	}

	private Vector2Int PopTail()
	{
		Vector2Int coords = m_snakeCoords[^1];

		m_snakeCoords.RemoveAt(m_snakeCoords.Count - 1);

		return coords;
	}

	private void SpawnFruit()
	{
		Vector2Int coords = Grid.Instance.GetRandomEmptyCell();

		Grid.Instance.AddOccupant(coords, Occupant.Fruit);
	}
}
