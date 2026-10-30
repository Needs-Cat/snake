
using Unity.Scripting.LifecycleManagement;

public abstract partial class ASingleton<T>
	where T : ASingleton<T>, new()
{
	public static T Instance
	{
		get
		{
			m_instance ??= new T();

			return m_instance;
		}
	}

	public static bool Exists => m_instance != null;
	
	[AutoStaticsCleanup]
	private static T m_instance = null;
	
	protected ASingleton()
	{
		// Stub so it can't be called
	}
}
