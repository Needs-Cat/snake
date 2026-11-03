
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputManager : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField]
	private float m_maxSwipeTime = 0.25f;
	[SerializeField]
	private float m_minSwipeDistance = 1.0f;
	
	[Header("Events")]
	[SerializeField]
	private UnityEvent<Vector2Int> m_onMovementDirectionChanged = new();
	
	private Vector2 m_touchStartPos = Vector2.zero;
	private float m_touchStartTime = 0.0f;

#if UNITY_EDITOR

	private void Update()
	{
		if (!GameManager.Exists)
		{
			return;
		}

		// Don't take in gameplay-related input while paused
		if (GameManager.Instance.IsPaused)
		{
			return;
		}
		
		KeyboardSwipe();
	}

	private void KeyboardSwipe()
	{
		Vector2Int swipe = Vector2Int.zero;

		Keyboard keyboard = Keyboard.current;

		if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
		{
			swipe.y += 1;
		}

		if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
		{
			swipe.y -= 1;
		}

		if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
		{
			swipe.x += 1;
		}

		if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
		{
			swipe.x -= 1;
		}

		if (swipe != Vector2Int.zero)
		{
			m_onMovementDirectionChanged.Invoke(swipe);
		}
	}

#endif
	
	public void PressCallback(InputAction.CallbackContext context)
	{
		if (!GameManager.Exists)
		{
			return;
		}
		
		// Don't take in gameplay-related input while paused
		if (GameManager.Instance.IsPaused)
		{
			return;
		}
		
		TouchState state = context.ReadValue<TouchState>();
		
		if (state.phase == TouchPhase.Began)
		{
			m_touchStartPos = state.position;
			m_touchStartTime = Time.unscaledTime;
			return;
		}
		
		if (state.phase == TouchPhase.Ended)
		{
			// Measure swipe!
			float timeDelta = Time.unscaledTime - m_touchStartTime;
			if (timeDelta > m_maxSwipeTime)
			{
				// Too slow to count as a swipe
				return;
			}

			Vector2 touchEndPos = state.position;
			Vector2 direction = touchEndPos - m_touchStartPos;
			float distance = direction.magnitude;
			if (distance < m_minSwipeDistance)
			{
				// Not far enough to count as a swipe
				return;
			}

			// Set to normalised cardinal
			Vector2Int moveDirection = ToMoveDirection(direction);
			
			m_onMovementDirectionChanged.Invoke(moveDirection);
		}
	}

	public void TapCallback(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Vector2 position = context.ReadValue<Vector2>();

			// Adjust the position so (0, 0) is in the centre of the screen
			float halfWidth = Screen.width * 0.5f;
			float halfHeight = Screen.height * 0.5f;
			position -= new Vector2(halfWidth, halfHeight);

			Vector2Int moveDirection = ToMoveDirection(position);

			m_onMovementDirectionChanged.Invoke(moveDirection);
		}
	}

	private Vector2Int ToMoveDirection(Vector2 direction)
	{
		// Normalise and round to cardinals
		direction.Normalize();

		Vector2Int moveDirection = Vector2Int.zero;
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
		{
			// Horizontal - Don't round to 0!
			moveDirection.x = (direction.x > 0.0f) ? Mathf.CeilToInt(direction.x) : Mathf.FloorToInt(direction.x);
		}
		else
		{
			// Vertical - Don't round to 0!
			moveDirection.y = (direction.y > 0.0f) ? Mathf.CeilToInt(direction.y) : Mathf.FloorToInt(direction.y);
		}

		return moveDirection;
	}
}
