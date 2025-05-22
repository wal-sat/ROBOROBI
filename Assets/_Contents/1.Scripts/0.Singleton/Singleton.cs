using UnityEngine;

[DefaultExecutionOrder(-101)] // ... (1)
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T instance = default;

	public virtual void Awake()
	{
		if (instance)
		{
			Destroy(this.gameObject);
			return;
		}

		instance = this as T;

		// 親要素があるとDontDestroyOnLoadが機能しないため、親要素を外す
		this.gameObject.transform.parent = null;
		DontDestroyOnLoad(this.gameObject);
	}

	protected virtual void OnDestroy()
	{
		if (ReferenceEquals(this, instance)) instance = null;
	}
}

// (1)
// PlayerInputのExecutionOrderは-100である。
// PlayerInputのAwake()よりも早く実行するために、ExecutionOrderを-101に設定している。