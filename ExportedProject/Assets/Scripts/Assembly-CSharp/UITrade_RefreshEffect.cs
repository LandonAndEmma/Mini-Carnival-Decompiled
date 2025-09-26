using UnityEngine;

public class UITrade_RefreshEffect : MonoBehaviour
{
	private bool _bRefreshing;

	public bool bTestStop;

	private void Start()
	{
	}

	private void Update()
	{
		if (bTestStop)
		{
			EndRefresh();
			bTestStop = false;
		}
		if (_bRefreshing && !base.animation.IsPlaying("UITrade_Rotation"))
		{
			base.animation.Play("UITrade_Rotation");
		}
	}

	private void FixedUpdate()
	{
	}

	public void StartRefresh()
	{
		_bRefreshing = true;
	}

	public void EndRefresh()
	{
		_bRefreshing = false;
		base.transform.localRotation = default(Quaternion);
	}
}
