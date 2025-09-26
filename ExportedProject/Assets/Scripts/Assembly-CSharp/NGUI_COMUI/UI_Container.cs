using System.Collections.Generic;
using MessageID;
using UnityEngine;

namespace NGUI_COMUI
{
	public class UI_Container : UIEntity
	{
		public enum EBoxSelType
		{
			Single = 0,
			Multi = 1
		}

		[SerializeField]
		protected EBoxSelType _boxSelType;

		[SerializeField]
		protected UI_Box _curSelBox;

		[SerializeField]
		protected UI_Box _preSelBox;

		[SerializeField]
		protected List<UI_Box> _preSelBoxLst = new List<UI_Box>();

		[SerializeField]
		protected GameObject _objBoxPrefab;

		[SerializeField]
		protected UIGrid _cmpUIGrid;

		[SerializeField]
		protected List<UI_Box> _lstBoxs = new List<UI_Box>();

		public EBoxSelType BoxSelType
		{
			get
			{
				return _boxSelType;
			}
			set
			{
				_boxSelType = value;
			}
		}

		public UI_Box CurSelBox
		{
			get
			{
				return _curSelBox;
			}
		}

		public List<UI_Box> LstBoxs
		{
			get
			{
				return _lstBoxs;
			}
		}

		protected override void Load()
		{
			RegisterMessage(EUIMessageID.UIContainer_BoxOnClick, this, BoxOnClick);
			RegisterMessage(EUIMessageID.UIContainer_BoxOnDelete, this, BoxOnDelete);
		}

		protected override void UnLoad()
		{
			UnregisterMessage(EUIMessageID.UIContainer_BoxOnClick, this);
			UnregisterMessage(EUIMessageID.UIContainer_BoxOnDelete, this);
		}

		private bool BoxOnClick(TUITelegram msg)
		{
			UI_Box component = _objBoxPrefab.GetComponent<UI_Box>();
			UI_Box uI_Box = msg._pExtraInfo as UI_Box;
			if (component.GetType() != uI_Box.GetType())
			{
				return true;
			}
			UI_Box loseSel = null;
			if (IsCanSelBox(uI_Box, out loseSel))
			{
				_preSelBox = _curSelBox;
				_curSelBox = uI_Box;
				if (BoxSelType == EBoxSelType.Multi)
				{
					if (loseSel != null)
					{
						_preSelBoxLst.Remove(loseSel);
					}
					_preSelBoxLst.Add(_curSelBox);
				}
				else if (BoxSelType == EBoxSelType.Single)
				{
					_preSelBoxLst.Clear();
				}
				ProcessBoxLoseSelected(loseSel);
				ProcessBoxSelected(_curSelBox);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIContainer_BoxSelChanged, null, _preSelBoxLst);
			}
			else
			{
				_preSelBox = null;
				ProcessBoxCanntSelected(uI_Box);
				if (BoxSelType == EBoxSelType.Multi)
				{
					if (loseSel != null)
					{
						_preSelBoxLst.Remove(loseSel);
					}
				}
				else if (BoxSelType == EBoxSelType.Single)
				{
					_preSelBoxLst.Clear();
				}
				ProcessBoxLoseSelected(loseSel);
				UIMessageDispatch.Instance.SendMessage(EUIMessageID.UIContainer_BoxSelChanged, null, _preSelBoxLst);
				if (loseSel == _curSelBox)
				{
					_curSelBox = null;
				}
			}
			return true;
		}

		private bool BoxOnDelete(TUITelegram msg)
		{
			UI_Box component = _objBoxPrefab.GetComponent<UI_Box>();
			UI_Box uI_Box = msg._pExtraInfo as UI_Box;
			if (component.GetType() != uI_Box.GetType())
			{
				return true;
			}
			if (!LstBoxs.Remove(uI_Box))
			{
				return true;
			}
			ProcessBoxBeDeleted(uI_Box);
			uI_Box.transform.parent = null;
			Object.Destroy(uI_Box.gameObject);
			RefreshContainer();
			return true;
		}

		protected virtual bool IsCanSelBox(UI_Box box, out UI_Box loseSel)
		{
			Debug.LogWarning("IsCanSelBox No Realize!!!");
			loseSel = null;
			return false;
		}

		protected virtual void ProcessBoxSelected(UI_Box box)
		{
			box.SetSelected();
		}

		protected virtual void ProcessBoxSelectedGameMode(UI_Box box)
		{
			box.SetSelectedGameMode();
		}

		protected virtual void ProcessBoxLoseSelected(UI_Box box)
		{
			if (box != null)
			{
				box.SetLoseSelected();
			}
		}

		protected virtual void ProcessBoxCanntSelected(UI_Box box)
		{
			Debug.LogWarning("ProcessBoxCanntSelected No Realize!!!<" + base.name + ">");
		}

		protected virtual void ProcessBoxBeDeleted(UI_Box box)
		{
		}

		public void InitContainer(EBoxSelType type)
		{
			BoxSelType = type;
			ProcessBoxLoseSelected(_curSelBox);
			_curSelBox = null;
			_preSelBox = null;
			_preSelBoxLst = new List<UI_Box>();
		}

		public void InitContainer()
		{
			ProcessBoxLoseSelected(_curSelBox);
			_curSelBox = null;
			_preSelBox = null;
			_preSelBoxLst = new List<UI_Box>();
		}

		public int InitBoxs(int boxCount, bool cleared)
		{
			if (cleared)
			{
				for (int i = 0; i < LstBoxs.Count; i++)
				{
					LstBoxs[i].gameObject.transform.parent = null;
					Object.Destroy(LstBoxs[i].gameObject);
				}
				LstBoxs.Clear();
			}
			for (int j = 0; j < Mathf.Min(LstBoxs.Count, boxCount); j++)
			{
				LstBoxs[j].transform.parent = _cmpUIGrid.transform;
				LstBoxs[j].FormatBoxName(j);
				LstBoxs[j].SetAvailable(true);
				LstBoxs[j].ClearBoxData();
			}
			if (boxCount > LstBoxs.Count)
			{
				int count = LstBoxs.Count;
				for (int k = count; k < boxCount; k++)
				{
					GameObject gameObject = Object.Instantiate(_objBoxPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
					UI_Box component = gameObject.GetComponent<UI_Box>();
					component.transform.parent = _cmpUIGrid.transform;
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					component.FormatBoxName(k);
					LstBoxs.Add(component);
				}
			}
			else
			{
				for (int l = boxCount; l < LstBoxs.Count; l++)
				{
					LstBoxs[l].gameObject.transform.parent = null;
					LstBoxs[l].SetAvailable(false);
					LstBoxs[l].ClearBoxData();
				}
			}
			RefreshContainer();
			return 0;
		}

		public int SetBoxData(int boxId, UI_BoxData data)
		{
			if (boxId < 0 || boxId >= LstBoxs.Count)
			{
				Debug.LogError("Box Id Error:" + boxId + "  LstBoxs.Count=" + LstBoxs.Count);
				return -1;
			}
			if (!LstBoxs[boxId].IsAvailable)
			{
				Debug.LogWarning("Box Id:" + boxId + " is unavailable!!!");
				return -2;
			}
			LstBoxs[boxId].BoxData = data;
			DataSort();
			return 0;
		}

		public void SetMoveForce(Vector3 v)
		{
			GetComponent<UIDraggablePanel>().scale = v;
		}

		public int AddBox()
		{
			int count = LstBoxs.Count;
			GameObject gameObject = Object.Instantiate(_objBoxPrefab, new Vector3(-1000f, 0f, 0f), Quaternion.identity) as GameObject;
			UI_Box component = gameObject.GetComponent<UI_Box>();
			component.transform.parent = _cmpUIGrid.transform;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			component.FormatBoxName(count);
			LstBoxs.Add(component);
			RefreshContainer();
			return count;
		}

		public int AddBox(int i)
		{
			int count = LstBoxs.Count;
			if (count > i && i >= 0)
			{
				RefreshContainer();
				return i;
			}
			return AddBox();
		}

		public void DelBox(UI_Box box)
		{
			for (int i = 0; i < LstBoxs.Count; i++)
			{
				if (LstBoxs[i] == box)
				{
					Debug.Log("Del : " + i);
					box.gameObject.transform.parent = null;
					LstBoxs.RemoveAt(i);
					Object.Destroy(box.gameObject);
					RefreshContainer();
					break;
				}
			}
		}

		private void Start()
		{
		}

		public void ClearContainer()
		{
			for (int i = 0; i < LstBoxs.Count; i++)
			{
				LstBoxs[i].gameObject.transform.parent = null;
				Object.Destroy(LstBoxs[i].gameObject);
			}
			LstBoxs.Clear();
			if (GetComponent<UIDraggablePanel>() != null && GetComponent<UIDraggablePanel>().horizontalScrollBar != null)
			{
				GetComponent<UIDraggablePanel>().horizontalScrollBar.scrollValue = 0f;
			}
		}

		protected UI_Box IsExistSameTypeInPreList(UI_Box box)
		{
			for (int i = 0; i < _preSelBoxLst.Count; i++)
			{
				if (_preSelBoxLst[i].BoxData.DataType == box.BoxData.DataType)
				{
					return _preSelBoxLst[i];
				}
			}
			return null;
		}

		protected int GetInPreListCount()
		{
			return _preSelBoxLst.Count;
		}

		protected bool IsExistInPreList(UI_Box box)
		{
			for (int i = 0; i < _preSelBoxLst.Count; i++)
			{
				if (_preSelBoxLst[i] == box)
				{
					return true;
				}
			}
			return false;
		}

		private void RefreshContainer()
		{
			_cmpUIGrid.repositionNow = true;
			base.transform.localPosition = Vector3.zero;
			Vector4 clipRange = GetComponent<UIPanel>().clipRange;
			clipRange.x = 0f;
			clipRange.y = 0f;
			GetComponent<UIPanel>().clipRange = clipRange;
		}

		public virtual void DataSort()
		{
		}
	}
}
