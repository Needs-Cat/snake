using UnityEngine;

public class SnakeFace : AMonoBehaviourSingleton<SnakeFace>
{
	public void Initialise()
	{
		GameManager.Instance.OnMoveDirectionChanged += MoveDirectionChanged;
		GameManager.Instance.OnHeadCellChanged += HeadCellChanged;
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnMoveDirectionChanged -= MoveDirectionChanged;
		GameManager.Instance.OnHeadCellChanged -= HeadCellChanged;
	}

	private void MoveDirectionChanged(Vector2Int moveDirection)
	{
		// Rotate face to match the movement direction
		transform.up = new Vector3(moveDirection.x, moveDirection.y, 0.0f);
	}
	
	private void HeadCellChanged(Vector2Int cell)
	{
		// Move position to match Head Cell's position
		transform.position = Grid.Instance.GetPositionForCell(cell);
	}
}
