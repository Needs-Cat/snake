
/// <summary>
/// Flags Enum to represent what is within a cell
/// </summary>
[System.Flags]
public enum Occupant
{
	Empty = 0,
	
	Fruit = 1 << 0,
	
	Head = 1 << 1,
	Tail = 1 << 2,
}
