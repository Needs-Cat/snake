
using UnityEngine;

public class MoveDirectionDisplay : MonoBehaviour
{
	private void Start()
	{
		GameManager.Instance.OnMoveDirectionChanged += MoveDirectionChanged;
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnMoveDirectionChanged -= MoveDirectionChanged;
	}

	private void MoveDirectionChanged(Vector2Int moveDirection)
	{
		transform.up = new Vector3(moveDirection.x, moveDirection.y, 0.0f);
	}
}
