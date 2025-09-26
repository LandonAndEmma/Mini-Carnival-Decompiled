using System;
using UnityEngine;

public class COMA_Fishing_PlayerController : TBaseEntityCenterController
{
	public enum EState
	{
		Idle = 0,
		CastPole = 1,
		CancelPole = 2,
		SuccessPole = 3,
		Fetching = 4,
		Fishing = 5,
		ReceivePole = 6,
		PullPole = 7,
		PullPole02 = 8,
		ShowItem = 9,
		MaxCount = 10
	}

	public enum EChildTag
	{
		Buff = 0
	}

	private StateMachine<COMA_Fishing_PlayerController> stateMachine;

	private TStateFactory<COMA_Fishing_PlayerController>[] stateFactory;

	private TState<COMA_Fishing_PlayerController>[] objState;

	public EState _fetchingNextState = EState.CancelPole;

	public int nFetchParam;

	public bool bNeedOffBoat;

	public COMA_Fishing_PlayerController(TBaseEntity own)
		: base(own)
	{
	}

	protected override int InitController()
	{
		stateMachine = new StateMachine<COMA_Fishing_PlayerController>(this);
		stateFactory = new TStateFactory<COMA_Fishing_PlayerController>[10];
		stateFactory[0] = new COMA_Fishing_PlayerStateCreator_Idle();
		stateFactory[1] = new COMA_Fishing_PlayerStateCreator_CastPole();
		stateFactory[2] = new COMA_Fishing_PlayerStateCreator_CancelPole();
		stateFactory[3] = new COMA_Fishing_PlayerStateCreator_SuccessPole();
		stateFactory[4] = new COMA_Fishing_PlayerStateCreator_FetchingPole();
		stateFactory[5] = new COMA_Fishing_PlayerStateCreator_Fishing();
		stateFactory[6] = new COMA_Fishing_PlayerStateCreator_ReceivePole();
		stateFactory[7] = new COMA_Fishing_PlayerStateCreator_PullPole();
		stateFactory[8] = new COMA_Fishing_PlayerStateCreator_PullPole02();
		stateFactory[9] = new COMA_Fishing_PlayerStateCreator_ShowItem();
		objState = new TState<COMA_Fishing_PlayerController>[10];
		for (int i = 0; i < 10; i++)
		{
			objState[i] = stateFactory[i].CreateState();
		}
		stateMachine.Init(objState[0], new COMA_Fishing_PlayerStateCreator_Global().CreateState());
		return 0;
	}

	public void ChangeState(EState curtate)
	{
		Debug.Log(string.Concat("########Change STATE=", stateMachine.CurState, "   TO STATE=", objState[(int)curtate]));
		if (curtate != EState.MaxCount)
		{
			stateMachine.ChangeState(objState[(int)curtate]);
		}
	}

	public StateMachine<COMA_Fishing_PlayerController> GetStateMachine()
	{
		return stateMachine;
	}

	public override bool CanHandleMessage(TTelegram msg)
	{
		return true;
	}

	public override bool HandleMessage(TTelegram msg)
	{
		base.HandleMessage(msg);
		return stateMachine.HandleMessage(msg);
	}

	public override void Tick()
	{
		stateMachine.Update();
	}

	public override void FixedTick()
	{
	}

	private TBaseEntityBuffController GetBuffCtrl()
	{
		int childSysTypeByTag = GetChildSysTypeByTag(0);
		TBaseEntityBuffController tBaseEntityBuffController = GetOwner().GetSystemCtrlByType(childSysTypeByTag) as TBaseEntityBuffController;
		if (tBaseEntityBuffController == null)
		{
			Debug.LogError("------------>Cann't find buffCtrl!!! ChildSysType=" + childSysTypeByTag);
		}
		return tBaseEntityBuffController;
	}

	public bool IsIdleState()
	{
		return (GetStateMachine().CurState == objState[0]) ? true : false;
	}

	public bool IsFishingPoleValid(out int reason)
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing.CurFishPole == null || cOMA_PlayerSelf_Fishing.CurFishPole.CurUseCount <= 0)
		{
			reason = -1;
			return false;
		}
		reason = 0;
		return true;
	}

	public bool IsInShakeState()
	{
		TBaseEntityBuffController buffCtrl = GetBuffCtrl();
		if (buffCtrl == null)
		{
			return false;
		}
		return buffCtrl.IsExitBuff(0);
	}

	public bool IsNeedEnterPullState()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		COMA_Fishing_FishableObj fishingItem = cOMA_PlayerSelf_Fishing.GetFishingItem();
		if (fishingItem == null)
		{
			Debug.LogError("------>FishItem is NULL!!!");
			return false;
		}
		Debug.Log("==========Get ==============>" + fishingItem.name);
		if (fishingItem.GetEntityType() == 101 || (fishingItem.GetEntityType() == 100 && ((COMA_Fishing_Fish)fishingItem).IsBigFish()))
		{
			return true;
		}
		return false;
	}

	public void CancelBuff_Shake()
	{
		TBaseEntityBuffController buffCtrl = GetBuffCtrl();
		if (buffCtrl != null)
		{
			int num = buffCtrl.RemoveBuffByType(0);
			Debug.Log("#####-> Remove buff SHAKE=" + num);
		}
	}

	public void ReduceFishingPoleUseCount()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing.CurFishPole != null)
		{
			cOMA_PlayerSelf_Fishing.CurFishPole.CurUseCount--;
			Debug.Log("========>reduce fish pole count! cur count=" + cOMA_PlayerSelf_Fishing.CurFishPole.CurUseCount);
		}
	}

	public void ClearCurFishItem()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing.CurFishItem != null)
		{
			UnityEngine.Object.DestroyObject(cOMA_PlayerSelf_Fishing.CurFishItem.gameObject);
			cOMA_PlayerSelf_Fishing.CurFishItem = null;
			Debug.Log("Clear fish item!");
		}
	}

	public void DestoryFishPole()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing.CurFishPole != null)
		{
			UnityEngine.Object.DestroyObject(cOMA_PlayerSelf_Fishing.CurFishPole.gameObject);
			cOMA_PlayerSelf_Fishing.CurFishPole = null;
		}
	}

	public int CalFishFloatPos()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		Vector3 normalized = cOMA_PlayerSelf_Fishing.transform.forward.normalized;
		float num = UnityEngine.Random.Range(-(float)Math.PI / 4f, (float)Math.PI / 4f);
		if (((COMA_PlayerSelf_Fishing)COMA_PlayerSelf.Instance).IsOnBoat())
		{
			num = ((UnityEngine.Random.Range(0, 2) != 0) ? UnityEngine.Random.Range((float)Math.PI / 8f, (float)Math.PI / 4f) : UnityEngine.Random.Range(-(float)Math.PI / 4f, -(float)Math.PI / 8f));
		}
		float w = Mathf.Cos(num / 2f);
		float y = Mathf.Sin(num / 2f);
		normalized = (new Quaternion(0f, y, 0f, w) * normalized).normalized;
		float fFishPoleLen = cOMA_PlayerSelf_Fishing.CurFishPole._fFishPoleLen;
		float num2 = UnityEngine.Random.Range(cOMA_PlayerSelf_Fishing._fMinFloatDis, cOMA_PlayerSelf_Fishing._fMaxFloatDis);
		Vector3 origin = cOMA_PlayerSelf_Fishing.CurFishPole.transform.position + normalized * (fFishPoleLen + num2);
		origin.y = 20f;
		Ray ray = new Ray(origin, -cOMA_PlayerSelf_Fishing.transform.up);
		int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Default"));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 50f, layerMask))
		{
			if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				TMessageDispatcher.Instance.DispatchMsg(-1, cOMA_PlayerSelf_Fishing.GetInstanceID(), 5, TTelegram.SEND_MSG_IMMEDIATELY, hitInfo.point);
			}
			else
			{
				TMessageDispatcher.Instance.DispatchMsg(-1, cOMA_PlayerSelf_Fishing.GetInstanceID(), 4, TTelegram.SEND_MSG_IMMEDIATELY, hitInfo.point);
			}
		}
		else
		{
			TMessageDispatcher.Instance.DispatchMsg(-1, cOMA_PlayerSelf_Fishing.GetInstanceID(), 5, TTelegram.SEND_MSG_IMMEDIATELY, null);
		}
		return 0;
	}

	public void DrawFishingLine()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		DestoryFishingLine(false);
		cOMA_PlayerSelf_Fishing._objFishingLine = new GameObject();
		LineRenderer lineRenderer = cOMA_PlayerSelf_Fishing._objFishingLine.AddComponent<LineRenderer>();
		lineRenderer.SetWidth(0.01f, 0.01f);
		Vector3 fishingPoleEndPos = cOMA_PlayerSelf_Fishing.CurFishPole.GetFishingPoleEndPos();
		Vector3 position = cOMA_PlayerSelf_Fishing.CurFishFloat.gameObject.transform.position;
		Debug.Log(string.Concat("Line:p1=", fishingPoleEndPos, "  p2=", position));
		lineRenderer.SetVertexCount(12);
		int num = 0;
		lineRenderer.SetPosition(num, fishingPoleEndPos);
		num++;
		Vector3 v = (fishingPoleEndPos + position) * 0.5f;
		v.y = Mathf.Min(fishingPoleEndPos.y, position.y);
		Bezier3 bezier = new Bezier3(fishingPoleEndPos, v, position);
		for (float num2 = 0f; num2 < 1f; num2 += 0.1f)
		{
			Vector3 pointAtTime = bezier.GetPointAtTime(num2);
			lineRenderer.SetPosition(num, pointAtTime);
			num++;
		}
		Debug.Log(num);
		lineRenderer.SetPosition(num, position);
		num++;
		lineRenderer.material = cOMA_PlayerSelf_Fishing._fishingMaterial;
	}

	public void DrawFishingLine_Straight()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		DestoryFishingLine(false);
		cOMA_PlayerSelf_Fishing._objFishingLine = new GameObject();
		LineRenderer lineRenderer = cOMA_PlayerSelf_Fishing._objFishingLine.AddComponent<LineRenderer>();
		lineRenderer.SetWidth(0.01f, 0.01f);
		Vector3 fishingPoleEndPos = cOMA_PlayerSelf_Fishing.CurFishPole.GetFishingPoleEndPos();
		Vector3 position = cOMA_PlayerSelf_Fishing.CurFishFloat.gameObject.transform.position;
		lineRenderer.SetVertexCount(2);
		int num = 0;
		lineRenderer.SetPosition(num, fishingPoleEndPos);
		num++;
		lineRenderer.SetPosition(num, position);
		num++;
		lineRenderer.material = cOMA_PlayerSelf_Fishing._fishingMaterial;
	}

	public void DestoryFishingLine()
	{
		DestoryFishingLine(true);
	}

	public void DestoryFishingLine(bool bNotify)
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		if (bNotify)
		{
			NotifyFishFloatDisable();
		}
		if (cOMA_PlayerSelf_Fishing._objFishingLine != null)
		{
			UnityEngine.Object.DestroyObject(cOMA_PlayerSelf_Fishing._objFishingLine);
			cOMA_PlayerSelf_Fishing._objFishingLine = null;
		}
	}

	public void NotifyFishFloatDisable()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		int instanceID = cOMA_PlayerSelf_Fishing.CurFishFloat.GetInstanceID();
		TMessageDispatcher.Instance.DispatchMsg(-1, instanceID, 8, TTelegram.SEND_MSG_IMMEDIATELY, null);
	}

	public void HandleRefreshCharacPos()
	{
		COMA_PlayerSelf_Fishing cOMA_PlayerSelf_Fishing = GetOwner() as COMA_PlayerSelf_Fishing;
		if (cOMA_PlayerSelf_Fishing.IsOnBoat())
		{
			cOMA_PlayerSelf_Fishing.characterCom.transform.position = cOMA_PlayerSelf_Fishing._curUseBoat._transPlayerNode.position;
			cOMA_PlayerSelf_Fishing.shadowTrs.localPosition = cOMA_PlayerSelf_Fishing.characterCom.transform.localPosition + new Vector3(0f, 0.01f, 0f);
			cOMA_PlayerSelf_Fishing.dialogBoxCom.ChangeLocPos(new Vector3(0f, 2.2f, 0f), true);
		}
	}
}
