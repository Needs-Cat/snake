using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public void PauseToggled(bool isPaused)
	{
		gameObject.SetActive(isPaused);
	}

	public void QuitPressed()
	{
		SceneManager.LoadScene(SceneNames.MAIN_MENU);
	}
}
