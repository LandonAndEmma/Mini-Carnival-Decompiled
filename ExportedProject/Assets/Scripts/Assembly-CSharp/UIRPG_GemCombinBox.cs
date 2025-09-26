using UnityEngine;

public class UIRPG_GemCombinBox : MonoBehaviour
{
	[SerializeField]
	private UILabel _priceLabel;

	private UIRPG_GemCombinBoxData _data;

	public UIRPG_GemCombinBoxData BOXData
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			RefreshUI();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void RefreshUI()
	{
		if (_data != null)
		{
			_priceLabel.text = _data.Price.ToString();
		}
	}
}
