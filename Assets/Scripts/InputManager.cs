
using UnityEngine;
using UnityEngine.Events;
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
	private UnityEvent<Vector2> m_onSwipeRegistered = new();
	
	private Vector2 m_touchStartPos = Vector2.zero;
	private float m_touchStartTime = 0.0f;

#if UNITY_EDITOR

	private void Update()
	{
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
			m_onSwipeRegistered.Invoke(swipe);
		}
	}

#endif
	
	public void PressCallback(InputAction.CallbackContext context)
	{
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

			m_onSwipeRegistered.Invoke(direction);
		}
	}
}
