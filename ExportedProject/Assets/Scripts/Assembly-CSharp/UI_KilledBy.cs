using UnityEngine;

public class UI_KilledBy : MonoBehaviour
{
	[SerializeField]
	private TUILabel _label;

	[SerializeField]
	private TUIMeshSprite _icon;

	[SerializeField]
	private GameObject[] countDownObjs;

	private int countIndex;

	private void SetKilledByName(string str)
	{
		string text = TUITool.StringFormat(TUITextManager.Instance().GetString("moshitanchukuang_desc18"), str);
		_label.Text = text;
	}

	private void SetPlayerIcon(RenderTexture tex)
	{
		_icon.UseCustomize = true;
		_icon.CustomizeTexture = tex;
		_icon.CustomizeRect = new Rect(0f, 0f, tex.width, tex.height);
	}

	public void SetKilledInfo(string strName, RenderTexture tex)
	{
		SetKilledByName(strName);
		SetPlayerIcon(tex);
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		SceneTimerInstance.Instance.Remove(CountDown);
	}

	private void OnEnable()
	{
		if (countDownObjs != null && countDownObjs.Length > 0)
		{
			GameObject[] array = countDownObjs;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(false);
			}
			countIndex = countDownObjs.Length - 1;
			countDownObjs[countIndex].SetActive(true);
			SceneTimerInstance.Instance.Add(1f, CountDown);
		}
	}

	private void OnDisable()
	{
	}

	public bool CountDown()
	{
		countDownObjs[countIndex].SetActive(false);
		countIndex--;
		if (countIndex >= 0)
		{
			countDownObjs[countIndex].SetActive(true);
			return true;
		}
		countIndex = countDownObjs.Length - 1;
		return false;
	}

	private void Update()
	{
	}
}
