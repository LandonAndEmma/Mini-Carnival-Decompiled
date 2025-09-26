using System.Collections.Generic;
using UnityEngine;

public class TBaseAddressBook
{
	protected Dictionary<int, int> _mapAddrBook = new Dictionary<int, int>();

	protected Dictionary<int, List<int>> _mapChannel = new Dictionary<int, List<int>>();

	public virtual int RegisterAddr(int name, int id)
	{
		if (_mapAddrBook.ContainsKey(name))
		{
			Debug.LogWarning("TBaseAddressBook-RegisterAddr:Exist<" + name + ">");
			return -1;
		}
		_mapAddrBook.Add(name, id);
		return 0;
	}

	public virtual int GetIDByName(int name)
	{
		if (_mapAddrBook.ContainsKey(name))
		{
			return _mapAddrBook[name];
		}
		return -1;
	}
}
