using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefs
{
	[MenuItem("Player Prefs/Clear All", false, 21)]
	private static void ClearAllPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
	
	[MenuItem("Player Prefs/Clear High Score", false)]
	private static void ClearHighScore()
	{
		PlayerPrefs.DeleteKey(PlayerPrefKeys.HIGH_SCORE);
		PlayerPrefs.DeleteKey(PlayerPrefKeys.NEW_HIGH_SCORE);
	}
}
