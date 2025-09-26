using UnityEngine;

public class UIDoodle_SelOperate : MonoBehaviour
{
	public enum ESelOperate
	{
		Brush = 0,
		Eyedropper = 1,
		Noisy = 2,
		PaintBucket = 3
	}

	public ESelOperate _curSel;

	[SerializeField]
	private UIDoodle1 _msgProce;

	[SerializeField]
	private TUIControl _brushBtn;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void AutoSelectBrushFromBucket()
	{
		TUIInput input = new TUIInput
		{
			fingerId = 1,
			inputType = TUIInputType.Began,
			position = new Vector2(_brushBtn.transform.position.x, _brushBtn.transform.position.y)
		};
		_brushBtn.HandleInput(input);
		input.fingerId = 1;
		input.inputType = TUIInputType.Ended;
		input.position = new Vector2(_brushBtn.transform.position.x, _brushBtn.transform.position.y);
		_brushBtn.HandleInput(input);
	}

	public void HandleEventButtonBrush(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			_curSel = ESelOperate.Brush;
			_msgProce.NotifyOperatorChanged();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButtonEyedropper(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			_curSel = ESelOperate.Eyedropper;
			_msgProce.NotifyOperatorChanged();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButtonNoisy(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			_curSel = ESelOperate.Noisy;
			_msgProce.NotifyOperatorChanged();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}

	public void HandleEventButtonPaintBucket(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 1)
		{
			_curSel = ESelOperate.PaintBucket;
			_msgProce.NotifyOperatorChanged();
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
		}
	}
}
