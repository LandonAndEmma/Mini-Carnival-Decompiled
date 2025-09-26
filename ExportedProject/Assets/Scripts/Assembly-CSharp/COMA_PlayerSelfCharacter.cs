using System.Collections;
using MessageID;
using UnityEngine;

public class COMA_PlayerSelfCharacter : COMA_PlayerCharacter
{
	public bool testDown;

	public bool testLeft;

	public bool testRight;

	[SerializeField]
	private UIMainMenu _mainMenu;

	[SerializeField]
	private bool bUI;

	private bool _bLinkIdle;

	private int _ngAniInIndex;

	public void InitCharacter()
	{
		if (!bUI)
		{
			return;
		}
		if (Application.loadedLevelName != "COMA_Scene_WaitingRoom")
		{
			Debug.Log("bNew_MainMenu=" + COMA_Pref.Instance.bNew_MainMenu);
			if (COMA_Pref.Instance.bNew_MainMenu)
			{
				base.transform.localPosition = new Vector3(-2f, -1f, 2f);
				base.gameObject.SetActive(false);
			}
			else
			{
				base.transform.localPosition = new Vector3(0f, -1f, 2f);
				base.gameObject.SetActive(true);
			}
		}
		else
		{
			base.transform.localPosition = new Vector3(0f, 0f, 0f);
			base.gameObject.SetActive(true);
		}
	}

	public void Start()
	{
		InitCharacter();
	}

	public void Update()
	{
		if (testDown)
		{
			base.animation.Play("Move_down");
			testDown = false;
		}
		if (testLeft)
		{
			base.animation.Play("Move_left");
			testLeft = false;
		}
		if (testRight)
		{
			base.animation.Play("Move_right");
			testRight = false;
		}
	}

	public void AniMoveDownEnd()
	{
		base.animation.Play();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_MoveDownEnd, null, null);
	}

	public void AniNG_AttackEnd()
	{
		base.animation.CrossFade("Farmer_NG_Idle", 0.2f);
	}

	public void Enter()
	{
		base.animation.Play("Move_down");
	}

	public void FarmerAttackNG()
	{
		base.animation.Play("Farmer_Attack_NG");
	}

	public void PointLeftup()
	{
		base.animation.Stop();
		base.animation.Play("Point_leftup");
	}

	public void PointRightdown01()
	{
		base.animation.Play("Point_rightdown01");
	}

	public void PointRightdown02()
	{
		base.animation.Play("Point_rightdown02");
	}

	public void PointDown()
	{
		base.animation.Play("Point_down");
	}

	public void AniPointDownEnd()
	{
		AniPlayIdle(0);
	}

	private IEnumerator OffsetPos()
	{
		yield return 0;
		Vector3 vTmp = base.transform.localPosition;
		vTmp += new Vector3(1.754f, 0f, 0f);
		base.transform.localPosition = vTmp;
	}

	public void AniPlayIdle(int IsIn)
	{
		if (!_bLinkIdle)
		{
			return;
		}
		_bLinkIdle = false;
		if (_ngAniInIndex == 0)
		{
			if (IsIn > 0)
			{
				Vector3 localPosition = base.transform.localPosition;
				localPosition += new Vector3(1.754f, 0f, 0f);
				base.transform.localPosition = localPosition;
				base.animation.CrossFade("Idle00", 0.2f);
			}
			base.animation.Play();
		}
		else if (_ngAniInIndex == 1 || _ngAniInIndex == 2 || _ngAniInIndex == 4 || _ngAniInIndex == 5)
		{
			if (IsIn > 0)
			{
				StartCoroutine(OffsetPos());
			}
			base.animation.Play("Yes");
			_ngAniInIndex = 0;
			_bLinkIdle = true;
		}
		else if (_ngAniInIndex == 3)
		{
			if (IsIn > 0)
			{
				StartCoroutine(OffsetPos());
			}
			base.animation.Play("Point_down");
			_ngAniInIndex = 0;
			_bLinkIdle = true;
		}
	}

	public void MoveRight()
	{
		base.animation.Play("Move_right");
	}

	public void AniMoveRightEnd()
	{
		base.animation.Play("Point_rightup");
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquarePointMails, null, null);
	}

	public void AniPointRightupEnd()
	{
	}

	public void AniPointRightdownEnd()
	{
	}

	public void Ani_Out_to_right()
	{
		base.animation.Stop();
		base.transform.localPosition = new Vector3(2f, -1f, 2f);
		base.animation.Play("Out");
	}

	public void Ani_Out_to_left()
	{
		base.animation.Stop();
		base.transform.localPosition = new Vector3(-4f, -1f, 2f);
		base.animation.Play("In");
	}

	public void Ani_In_from_left(int nIndex)
	{
		base.animation.Stop();
		base.transform.localPosition = new Vector3(-4f, -1f, 2f);
		base.animation.Play("Out");
		_bLinkIdle = true;
		_ngAniInIndex = nIndex;
	}

	public void Ani_In_from_left()
	{
		Ani_In_from_left(0);
	}

	public void Ani_In_from_right(int nIndex)
	{
		base.animation.Stop();
		base.transform.localPosition = new Vector3(2f, -1f, 2f);
		base.animation.Play("In");
		_bLinkIdle = true;
		_ngAniInIndex = nIndex;
	}

	public void Ani_In_from_right()
	{
		Ani_In_from_right(0);
	}

	public void PointRightDown()
	{
		base.animation.Play("Point_rightdown");
	}

	public void Bow()
	{
		base.animation.Play("Bow");
	}

	public void AniBowEnd()
	{
	}

	public void MoveLeft()
	{
		base.animation.Play("Move_left");
	}

	public void Point_down02()
	{
		base.animation.Play("Point_down02");
	}

	public void Point_down02_again()
	{
		base.animation.Play("Point_down02_again");
	}

	public void AniMoveLeftEnd()
	{
		base.animation.Play();
		UIMessageDispatch.Instance.SendMessage(EUIMessageID.UING_SquareMoveLeftEnd, null, null);
	}

	protected override void AnimCallback_PullPole_True()
	{
		COMA_AudioManager.Instance.SoundPlay(AudioCategory.Ani_Fishing_Draw, base.transform.parent);
	}
}
