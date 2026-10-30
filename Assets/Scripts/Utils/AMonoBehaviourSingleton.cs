
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

public abstract partial class AMonoBehaviourSingleton<T> : MonoBehaviour
	where T : AMonoBehaviourSingleton<T>
{
	public static T Instance => m_instance;
	public static bool Exists => m_instance != null;
	
	[AutoStaticsCleanup]
	private static T m_instance = null;

	protected void Awake()
	{
		if (m_instance != null && m_instance != this)
		{
			Destroy(this);
			return;
		}

		m_instance = (T) this;
	}
}
