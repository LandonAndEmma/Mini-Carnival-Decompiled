public class UIArmory_Box : UI_Box
{
	protected UIArmory_Container.EEleType _type;

	public void Init(UIArmory_Container.EEleType t)
	{
		_type = t;
	}

	public UIArmory_Container.EEleType GetEleType()
	{
		return _type;
	}

	public bool IsType(UIArmory_Container.EEleType t)
	{
		return (t == _type) ? true : false;
	}
}
