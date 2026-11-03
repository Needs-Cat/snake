
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
	private const float TOUCH_DELAY = 0.5f;
	
	[SerializeField]
	private TextMeshProUGUI m_scoreText = null;
	[SerializeField]
	private TextMeshProUGUI m_highScoreText = null;
	[SerializeField]
	private GameObject m_newHighScoreParent = null;

	private bool m_touchBlocked = true;

	private void Start()
	{
		m_scoreText.text = PlayerPrefs.GetInt(PlayerPrefKeys.SCORE, 0).ToString();
		m_highScoreText.text = PlayerPrefs.GetInt(PlayerPrefKeys.HIGH_SCORE, 0).ToString();

		bool newHighScore = PlayerPrefs.GetInt(PlayerPrefKeys.NEW_HIGH_SCORE, 0) == 1;
		m_newHighScoreParent.SetActive(newHighScore);

		// Wait for a small delay before allow buttons to be pressed
		// This is to stop the player accidentally pressing a button
		StartCoroutine(UnblockTouch());
	}

	public void RetryPressed()
	{
		if (m_touchBlocked)
		{
			return;
		}
		
		SceneManager.LoadScene(SceneNames.GAME);
	}

	public void QuitGamePressed()
	{
		if (m_touchBlocked)
		{
			return;
		}
		
		SceneManager.LoadScene(SceneNames.MAIN_MENU);
	}

	private IEnumerator UnblockTouch()
	{
		yield return new WaitForSeconds(TOUCH_DELAY);

		m_touchBlocked = false;
	}
}
