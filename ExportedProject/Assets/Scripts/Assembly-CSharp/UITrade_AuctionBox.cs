using UnityEngine;

public class UITrade_AuctionBox : MonoBehaviour
{
	public UITrade_EquipedContainer equipedContainer;

	public TUILabel goldLabel;

	private int nPrice;

	private UITrade_EquipedBoxData boxData;

	public int Price
	{
		get
		{
			return int.Parse(goldLabel.Text);
		}
		set
		{
			value = Mathf.Clamp(value, 1, 10000);
			goldLabel.Text = value.ToString();
		}
	}

	public UITrade_EquipedBoxData BoxData
	{
		get
		{
			return boxData;
		}
		set
		{
			boxData = value;
		}
	}

	private void Start()
	{
		Hide();
	}

	private void Update()
	{
	}

	public void Show()
	{
		base.animation.Play();
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = -100f;
		base.transform.localPosition = localPosition;
		Price = 500;
	}

	public void Hide()
	{
		BoxData = null;
		base.animation.Stop();
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 1200f;
		base.transform.localPosition = localPosition;
	}
}
