
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_scoreText = null;

	private void Start()
	{
		m_scoreText.text = PlayerPrefs.GetInt(PlayerPrefKeys.SCORE, 0).ToString();
	}

	public void RetryPressed()
	{
		SceneManager.LoadScene(SceneNames.GAME);
	}

	public void QuitGamePressed()
	{
		SceneManager.LoadScene(SceneNames.MAIN_MENU);
	}
}
