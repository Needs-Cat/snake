using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private const string GAME_SCENE = "Game";
	
	public void PlayGame()
	{
		SceneManager.LoadScene(GAME_SCENE);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
