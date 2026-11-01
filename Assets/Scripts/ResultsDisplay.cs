
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_scoreText = null;
	[SerializeField]
	private TextMeshProUGUI m_highScoreText = null;
	[SerializeField]
	private GameObject m_newHighScoreParent = null;

	private void Start()
	{
		m_scoreText.text = PlayerPrefs.GetInt(PlayerPrefKeys.SCORE, 0).ToString();
		m_highScoreText.text = PlayerPrefs.GetInt(PlayerPrefKeys.HIGH_SCORE, 0).ToString();

		bool newHighScore = PlayerPrefs.GetInt(PlayerPrefKeys.NEW_HIGH_SCORE, 0) == 1;
		m_newHighScoreParent.SetActive(newHighScore);
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
