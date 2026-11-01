using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void PlayGame()
	{
		SceneManager.LoadScene(SceneNames.GAME);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
