using UnityEngine;

public class UI_WeaponSelect : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _objGameWeapons;

	private void Start()
	{
		for (int i = 0; i < _objGameWeapons.Length; i++)
		{
			_objGameWeapons[i].SetActive(false);
		}
	}

	private void Update()
	{
	}

	public void SetWeapon(int id)
	{
		for (int i = 0; i < _objGameWeapons.Length; i++)
		{
			if (i == id)
			{
				_objGameWeapons[i].SetActive(true);
			}
			else
			{
				_objGameWeapons[i].SetActive(false);
			}
		}
	}

	public void SetWeapon(string name)
	{
		for (int i = 0; i < _objGameWeapons.Length; i++)
		{
			if (_objGameWeapons[i].name == name)
			{
				_objGameWeapons[i].SetActive(true);
			}
			else
			{
				_objGameWeapons[i].SetActive(false);
			}
		}
	}
}
