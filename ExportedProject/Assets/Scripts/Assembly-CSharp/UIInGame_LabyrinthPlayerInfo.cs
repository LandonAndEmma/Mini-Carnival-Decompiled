using UnityEngine;

public class UIInGame_LabyrinthPlayerInfo : MonoBehaviour
{
	public UILabyrinthPlayerInfo _playInfo = new UILabyrinthPlayerInfo();

	[SerializeField]
	private GameObject _objDiedIcon;

	[SerializeField]
	private TUIMeshSprite _texIcon;

	[SerializeField]
	private TUILabel _goldNum;

	public void RefreshUI()
	{
		float hP = _playInfo.HP;
		if (hP <= 0f)
		{
			_objDiedIcon.SetActive(true);
		}
		else
		{
			_objDiedIcon.SetActive(false);
		}
		if (_texIcon != null)
		{
			_texIcon.CustomizeTexture = _playInfo.Tex2D;
		}
		if (_goldNum != null)
		{
			_goldNum.Text = _playInfo.GoldNum.ToString();
		}
	}

	public void RefreshHeadIconUI()
	{
		if (_texIcon != null)
		{
			_texIcon.CustomizeTexture = _playInfo.Tex2D;
		}
	}
}
