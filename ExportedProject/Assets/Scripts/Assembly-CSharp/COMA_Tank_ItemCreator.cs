using System.Collections;
using UnityEngine;

public class COMA_Tank_ItemCreator : MonoBehaviour
{
	public float _fMinTime = 10f;

	public float _fMaxTime = 30f;

	public int _nCreateCount;

	private GameObject _objSubItem;

	private bool _bItemExist;

	private IEnumerator delayCreateItem()
	{
		float fWaitTime = Random.Range(_fMinTime, _fMaxTime);
		yield return new WaitForSeconds(fWaitTime);
		doCreateRandomItem(true);
		_bItemExist = true;
		yield return 0;
	}

	public IEnumerator delayCreateItem(int nIndex)
	{
		Debug.Log("delayCreateItem" + nIndex);
		float fWaitTime = Random.Range(_fMinTime, _fMaxTime);
		yield return new WaitForSeconds(fWaitTime);
		if (doCreateItem(nIndex))
		{
			broadcastCreation(nIndex);
		}
		_bItemExist = true;
		yield return 0;
	}

	private void doCreateRandomItem(bool bBroadcast = false)
	{
		int num = Random.Range(0, 5);
		if (doCreateItem(num) && bBroadcast)
		{
			broadcastCreation(num);
		}
	}

	public void destoryItem(bool bBroadcast = false)
	{
		Object.Destroy(_objSubItem);
		_objSubItem = null;
		GameObject obj = Object.Instantiate(Resources.Load("FBX/Buff/PFB/PFB_S_Buff_ItemGet"), base.transform.position, base.transform.rotation) as GameObject;
		Object.DestroyObject(obj, 2f);
		if (bBroadcast)
		{
			broadcastDelete();
			StartCoroutine(delayCreateItem());
		}
	}

	public bool doCreateItem(int nIndex)
	{
		if (_objSubItem == null)
		{
			string text = "FBX/Scene/Tank/Prefab/Tank_Item" + nIndex;
			Debug.Log(text);
			_objSubItem = Object.Instantiate(Resources.Load(text), base.transform.position, Quaternion.identity) as GameObject;
			_objSubItem.transform.parent = base.transform;
			_nCreateCount++;
			return true;
		}
		return false;
	}

	private void broadcastDelete()
	{
		COMA_CD_DeleteItem cOMA_CD_DeleteItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMDELETE) as COMA_CD_DeleteItem;
		cOMA_CD_DeleteItem.blockIndex = byte.Parse(base.name);
		COMA_CommandHandler.Instance.Send(cOMA_CD_DeleteItem);
	}

	private void broadcastCreation(int nItemIndex)
	{
		COMA_CD_CreateItem cOMA_CD_CreateItem = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.ITEMCREATE) as COMA_CD_CreateItem;
		cOMA_CD_CreateItem.blockIndex = byte.Parse(base.name);
		cOMA_CD_CreateItem.itemIndex = (byte)nItemIndex;
		COMA_CommandHandler.Instance.Send(cOMA_CD_CreateItem);
	}

	private IEnumerator Start()
	{
		while (COMA_PlayerSelf.Instance == null)
		{
			yield return 0;
		}
	}

	private void Update()
	{
	}
}
