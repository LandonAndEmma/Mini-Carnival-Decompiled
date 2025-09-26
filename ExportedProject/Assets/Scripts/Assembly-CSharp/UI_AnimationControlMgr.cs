using UnityEngine;

public class UI_AnimationControlMgr : MonoBehaviour
{
	[SerializeField]
	private TUIBlock _block;

	public string _strEnterSceneName;

	public TUIBlock SceneBlock
	{
		get
		{
			if (_block == null)
			{
				_block = base.gameObject.AddComponent<TUIBlock>();
				_block.size = new Vector2(480f, 320f);
			}
			return _block;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public virtual void EnterStart()
	{
		SceneBlock.m_bEnable = true;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_In);
	}

	public virtual void EnterEnd()
	{
		SceneBlock.m_bEnable = false;
	}

	public virtual void ExitStart()
	{
		SceneBlock.m_bEnable = true;
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Out);
	}

	public virtual void ExitEnd()
	{
		SceneBlock.m_bEnable = false;
		COMA_Loading.LoadLevel(_strEnterSceneName);
	}
}
