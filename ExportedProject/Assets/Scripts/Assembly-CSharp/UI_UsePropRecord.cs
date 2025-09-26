using UnityEngine;

public class UI_UsePropRecord : MonoBehaviour
{
	[SerializeField]
	private UI_InputLabel _aName;

	[SerializeField]
	private TUILabel _bName;

	[SerializeField]
	private TUIMeshSprite _propSprite;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RefreshUIByData(UI_UsePropInfoData data)
	{
		if (data == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		_aName.Text = data.AName;
		_bName.Text = data.BName;
		if (COMA_PlayerSelf.Instance != null)
		{
			if (_aName.Text == COMA_PlayerSelf.Instance.nickname)
			{
				_aName.color = new Color(1f, 0.73f, 0f);
			}
			else
			{
				_aName.color = new Color(1f, 1f, 1f);
			}
			if (_bName.Text == COMA_PlayerSelf.Instance.nickname)
			{
				_bName.color = new Color(1f, 0.73f, 0f);
			}
			else
			{
				_bName.color = new Color(1f, 1f, 1f);
			}
		}
		_propSprite.UseCustomize = true;
		_propSprite.CustomizeTexture = data.PropTexture;
		if (data.PropTexture != null)
		{
			_propSprite.CustomizeRect = new Rect(0f, 0f, data.PropTexture.width, data.PropTexture.height);
		}
		if (data.PropTexture != null)
		{
			float num = (float)data.PropTexture.width * _propSprite.gameObject.transform.localRotation.x / 4f + 10f;
			float lineWidth = _aName.GetLineWidth();
			Debug.Log(string.Concat("--------", _aName, ":", lineWidth));
			Vector3 localPosition = _propSprite.gameObject.transform.localPosition;
			localPosition.x = _aName.gameObject.transform.localPosition.x + lineWidth + num;
			_propSprite.gameObject.transform.localPosition = localPosition;
			Vector3 localPosition2 = _bName.gameObject.transform.localPosition;
			localPosition2.x = localPosition.x + num;
			_bName.gameObject.transform.localPosition = localPosition2;
		}
	}
}
