using System.Collections.Generic;
using UnityEngine;

public class COMA_PlayerCharacter : MonoBehaviour
{
	public delegate void onPlayAnim(string strAnimName, float fSpeed);

	public COMA_Player playerCom;

	public GameObject[] bodyObjs;

	public Transform boneTrs_Waist;

	public Transform viewTrs;

	public Transform handTrs;

	public Transform head_top;

	public Transform head_front;

	public Transform head_back;

	public Transform head_left;

	public Transform head_right;

	public Transform chest_front;

	public Transform chest_back;

	public Transform fishing_item;

	protected Dictionary<byte, string> animIndexToName = new Dictionary<byte, string>();

	protected Dictionary<string, byte> animNameToIndex = new Dictionary<string, byte>();

	private string _lastTankDeadAnimName;

	public onPlayAnim _onPlayAnim;

	protected virtual void Awake()
	{
		if (base.animation["Attack01_W002"] != null)
		{
			base.animation["Attack01_W002"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W003"] != null)
		{
			base.animation["Attack01_W003"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W004"] != null)
		{
			base.animation["Attack01_W004"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W005"] != null)
		{
			base.animation["Attack01_W005"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W006"] != null)
		{
			base.animation["Attack01_W006"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W007"] != null)
		{
			base.animation["Attack01_W007"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W002"] != null)
		{
			base.animation["Reload01_W002"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W003"] != null)
		{
			base.animation["Reload01_W003"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W004"] != null)
		{
			base.animation["Reload01_W004"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W005"] != null)
		{
			base.animation["Reload01_W005"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W006"] != null)
		{
			base.animation["Reload01_W006"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W007"] != null)
		{
			base.animation["Reload01_W007"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack11_W001"] != null)
		{
			base.animation["Attack11_W001"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack11_W008"] != null)
		{
			base.animation["Attack11_W008"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload11_W001"] != null)
		{
			base.animation["Reload11_W001"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload11_W008"] != null)
		{
			base.animation["Reload11_W008"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_Run"] != null)
		{
			base.animation["Attack01_Run"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W020"] != null)
		{
			base.animation["Attack01_W020"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W021"] != null)
		{
			base.animation["Attack01_W021"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Attack01_W022"] != null)
		{
			base.animation["Attack01_W022"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W020"] != null)
		{
			base.animation["Reload01_W020"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W021"] != null)
		{
			base.animation["Reload01_W021"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Reload01_W022"] != null)
		{
			base.animation["Reload01_W022"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["ColdArms_Attack01_Axe"] != null)
		{
			base.animation["ColdArms_Attack01_Axe"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["ColdArms_Attack01_LightSabre"] != null)
		{
			base.animation["ColdArms_Attack01_LightSabre"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["ColdArms_Attack01_Mace"] != null)
		{
			base.animation["ColdArms_Attack01_Mace"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["Hello"] != null)
		{
			base.animation["Hello"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["ColdArms_Take01"] != null)
		{
			base.animation["ColdArms_Take01"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["ColdArms_Take01"] != null)
		{
			base.animation["ColdArms_Take01"].layer = 2;
		}
		if (base.animation["TwoHand_Take01"] != null)
		{
			base.animation["TwoHand_Take01"].AddMixingTransform(boneTrs_Waist);
		}
		if (base.animation["TwoHand_Take01"] != null)
		{
			base.animation["TwoHand_Take01"].layer = 2;
		}
		if (base.animation["Attack11_W001"] != null)
		{
			base.animation["Attack11_W001"].layer = 2;
		}
		if (base.animation["Attack01_W002"] != null)
		{
			base.animation["Attack01_W002"].layer = 2;
		}
		if (base.animation["Attack01_W003"] != null)
		{
			base.animation["Attack01_W003"].layer = 2;
		}
		if (base.animation["Attack01_W004"] != null)
		{
			base.animation["Attack01_W004"].layer = 2;
		}
		if (base.animation["Attack01_W005"] != null)
		{
			base.animation["Attack01_W005"].layer = 2;
		}
		if (base.animation["Attack01_W006"] != null)
		{
			base.animation["Attack01_W006"].layer = 2;
		}
		if (base.animation["Attack01_W007"] != null)
		{
			base.animation["Attack01_W007"].layer = 2;
		}
		if (base.animation["Attack11_W008"] != null)
		{
			base.animation["Attack11_W008"].layer = 2;
		}
		if (base.animation["Reload11_W001"] != null)
		{
			base.animation["Reload11_W001"].layer = 2;
		}
		if (base.animation["Reload01_W002"] != null)
		{
			base.animation["Reload01_W002"].layer = 2;
		}
		if (base.animation["Reload01_W003"] != null)
		{
			base.animation["Reload01_W003"].layer = 2;
		}
		if (base.animation["Reload01_W004"] != null)
		{
			base.animation["Reload01_W004"].layer = 2;
		}
		if (base.animation["Reload01_W005"] != null)
		{
			base.animation["Reload01_W005"].layer = 2;
		}
		if (base.animation["Reload01_W006"] != null)
		{
			base.animation["Reload01_W006"].layer = 2;
		}
		if (base.animation["Reload01_W007"] != null)
		{
			base.animation["Reload01_W007"].layer = 2;
		}
		if (base.animation["Reload11_W008"] != null)
		{
			base.animation["Reload11_W008"].layer = 2;
		}
		if (base.animation["Attack01_Run"] != null)
		{
			base.animation["Attack01_Run"].layer = 1;
		}
		if (base.animation["Attack01_W020"] != null)
		{
			base.animation["Attack01_W020"].layer = 2;
		}
		if (base.animation["Attack01_W021"] != null)
		{
			base.animation["Attack01_W021"].layer = 2;
		}
		if (base.animation["Attack01_W022"] != null)
		{
			base.animation["Attack01_W022"].layer = 2;
		}
		if (base.animation["Reload01_W020"] != null)
		{
			base.animation["Reload01_W020"].layer = 2;
		}
		if (base.animation["Reload01_W021"] != null)
		{
			base.animation["Reload01_W021"].layer = 2;
		}
		if (base.animation["Reload01_W022"] != null)
		{
			base.animation["Reload01_W022"].layer = 2;
		}
		if (base.animation["ColdArms_Attack01_Axe"] != null)
		{
			base.animation["ColdArms_Attack01_Axe"].layer = 2;
		}
		if (base.animation["ColdArms_Attack01_LightSabre"] != null)
		{
			base.animation["ColdArms_Attack01_LightSabre"].layer = 2;
		}
		if (base.animation["ColdArms_Attack01_Mace"] != null)
		{
			base.animation["ColdArms_Attack01_Mace"].layer = 2;
		}
		if (base.animation["Getdown"] != null)
		{
			base.animation["Getdown"].layer = 1;
		}
		if (base.animation["Falldown"] != null)
		{
			base.animation["Falldown"].layer = 1;
		}
		if (base.animation["Makeup"] != null)
		{
			base.animation["Makeup"].layer = 1;
		}
		if (base.animation["Makeup01"] != null)
		{
			base.animation["Makeup01"].layer = 1;
		}
		if (base.animation["Makeup02"] != null)
		{
			base.animation["Makeup02"].layer = 1;
		}
		if (base.animation["Slide"] != null)
		{
			base.animation["Slide"].layer = 1;
		}
		if (base.animation["Show"] != null)
		{
			base.animation["Show"].layer = 2;
		}
		if (base.animation["Rockets"] != null)
		{
			base.animation["Rockets"].layer = 1;
		}
		if (base.animation["Dizzy"] != null)
		{
			base.animation["Dizzy"].layer = 1;
		}
		if (base.animation["Death00"] != null)
		{
			base.animation["Death00"].layer = 7;
		}
		if (base.animation["Death01"] != null)
		{
			base.animation["Death01"].layer = 7;
		}
		if (base.animation["Death02"] != null)
		{
			base.animation["Death02"].layer = 7;
		}
		if (base.animation["Death11"] != null)
		{
			base.animation["Death11"].layer = 7;
		}
		if (base.animation["Death12"] != null)
		{
			base.animation["Death12"].layer = 7;
		}
		if (base.animation["ColdArms_Death01"] != null)
		{
			base.animation["ColdArms_Death01"].layer = 7;
		}
		if (base.animation["ColdArms_Death02"] != null)
		{
			base.animation["ColdArms_Death02"].layer = 7;
		}
		if (base.animation["TwoHand_Death01"] != null)
		{
			base.animation["TwoHand_Death01"].layer = 7;
		}
		if (base.animation["TwoHand_Death02"] != null)
		{
			base.animation["TwoHand_Death02"].layer = 7;
		}
		if (base.animation["Rockets03_huang"] != null)
		{
			base.animation["Rockets03_huang"].layer = 0;
		}
		if (base.animation["Lookback_huang"] != null)
		{
			base.animation["Lookback_huang"].layer = 1;
		}
		if (base.animation["Point_down"] != null)
		{
			base.animation["Point_down"].layer = 0;
		}
		if (base.animation["Fishing_unsuccess"] != null)
		{
			base.animation["Fishing_unsuccess"].wrapMode = WrapMode.Once;
		}
		if (base.animation["Fishing_success"] != null)
		{
			base.animation["Fishing_success"].wrapMode = WrapMode.Once;
		}
		if (base.animation["Fishing_idle"] != null)
		{
			base.animation["Fishing_idle"].wrapMode = WrapMode.Loop;
		}
		if (base.animation["Fishing_castpole"] != null)
		{
			base.animation["Fishing_castpole"].wrapMode = WrapMode.ClampForever;
		}
		if (base.animation["Fishing_cancelpole"] != null)
		{
			base.animation["Fishing_cancelpole"].wrapMode = WrapMode.Once;
		}
		if (base.animation["Fishing_pullpole"] != null)
		{
			base.animation["Fishing_pullpole"].wrapMode = WrapMode.Loop;
		}
		if (base.animation["Tank_Fire"] != null)
		{
			base.animation["Tank_Fire"].layer = 2;
		}
		if (base.animation["Tank_Death01"] != null)
		{
			base.animation["Tank_Death01"].layer = 7;
			base.animation["Tank_Death01"].wrapMode = WrapMode.ClampForever;
		}
		if (base.animation["Tank_Death02"] != null)
		{
			base.animation["Tank_Death02"].layer = 7;
			base.animation["Tank_Death02"].wrapMode = WrapMode.ClampForever;
		}
		if (base.animation["Tank_Death03"] != null)
		{
			base.animation["Tank_Death03"].layer = 7;
			base.animation["Tank_Death03"].wrapMode = WrapMode.ClampForever;
		}
		if (base.animation["Tank_Damage_B"] != null)
		{
			base.animation["Tank_Damage_B"].layer = 4;
		}
		if (base.animation["Tank_Damage_F"] != null)
		{
			base.animation["Tank_Damage_F"].layer = 4;
		}
		if (base.animation["Tank_Damage_L"] != null)
		{
			base.animation["Tank_Damage_L"].layer = 4;
		}
		if (base.animation["Tank_Damage_R"] != null)
		{
			base.animation["Tank_Damage_R"].layer = 4;
		}
		if (base.animation["Hello"] != null)
		{
			base.animation["Hello"].layer = 2;
		}
		if (base.animation["Fly_up"] != null)
		{
			base.animation["Fly_up"].layer = 1;
		}
		if (base.animation["Sit_Down"] != null)
		{
			base.animation["Sit_Down"].layer = 1;
		}
		if (base.animation["Stand_Idle"] != null)
		{
			base.animation["Stand_Idle"].layer = 1;
		}
		if (base.animation["Stand_Up"] != null)
		{
			base.animation["Stand_Up"].layer = 1;
		}
		byte b = 0;
		foreach (AnimationState item in base.animation)
		{
			animIndexToName.Add(b, item.name);
			animNameToIndex.Add(item.name, b);
			b++;
		}
	}

	protected void OnEnable()
	{
	}

	protected void OnDisable()
	{
	}

	public void CreateAccouterment(string accoutermentName)
	{
		CreateAccouterment(accoutermentName, string.Empty);
	}

	public void CreateAccouterment(string accoutermentName, string layerName)
	{
		if (accoutermentName == null || accoutermentName == string.Empty)
		{
			return;
		}
		Transform transform = null;
		if (accoutermentName.StartsWith("HT"))
		{
			transform = head_top;
		}
		else if (accoutermentName.StartsWith("HF"))
		{
			transform = head_front;
		}
		else if (accoutermentName.StartsWith("HB"))
		{
			transform = head_back;
		}
		else if (accoutermentName.StartsWith("HL"))
		{
			transform = head_left;
		}
		else if (accoutermentName.StartsWith("HR"))
		{
			transform = head_right;
		}
		else if (accoutermentName.StartsWith("CF"))
		{
			transform = chest_front;
		}
		else if (accoutermentName.StartsWith("CB"))
		{
			transform = chest_back;
		}
		else if (accoutermentName.StartsWith("HDR"))
		{
			transform = handTrs;
		}
		else if (accoutermentName.StartsWith("AA08") || accoutermentName.StartsWith("AA16") || accoutermentName.StartsWith("AA18") || accoutermentName.StartsWith("AA20") || accoutermentName.StartsWith("AA23") || accoutermentName.StartsWith("AA25") || accoutermentName.StartsWith("AA26") || accoutermentName.StartsWith("AA28") || accoutermentName.StartsWith("AA37") || accoutermentName.StartsWith("AA39") || accoutermentName.StartsWith("AA40") || accoutermentName.StartsWith("AA42") || accoutermentName.StartsWith("AA46"))
		{
			transform = head_top;
		}
		else if (accoutermentName.StartsWith("AA09") || accoutermentName.StartsWith("AA22") || accoutermentName.StartsWith("AA36") || accoutermentName.StartsWith("AA38") || accoutermentName.StartsWith("AA43") || accoutermentName.StartsWith("AA44") || accoutermentName.StartsWith("AA45") || accoutermentName.StartsWith("AA47") || accoutermentName.StartsWith("AA32"))
		{
			transform = chest_back;
		}
		else if (accoutermentName.StartsWith("AA10") || accoutermentName.StartsWith("AA17"))
		{
			transform = head_front;
		}
		if (!(transform != null))
		{
			return;
		}
		if (transform.childCount > 0)
		{
			if (transform.GetChild(0).name == accoutermentName)
			{
				return;
			}
			Object.DestroyObject(transform.GetChild(0).gameObject);
		}
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/Player/Part/PFB/" + accoutermentName)) as GameObject;
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		if (layerName != string.Empty)
		{
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array = componentsInChildren;
			foreach (MeshRenderer meshRenderer in array)
			{
				meshRenderer.gameObject.layer = LayerMask.NameToLayer(layerName);
			}
		}
	}

	public void DestroyAccouterment(string accoutermentName)
	{
		if (accoutermentName != null && !(accoutermentName == string.Empty))
		{
			Transform transform = null;
			if (accoutermentName.StartsWith("HT"))
			{
				transform = head_top;
			}
			else if (accoutermentName.StartsWith("HF"))
			{
				transform = head_front;
			}
			else if (accoutermentName.StartsWith("HB"))
			{
				transform = head_back;
			}
			else if (accoutermentName.StartsWith("HL"))
			{
				transform = head_left;
			}
			else if (accoutermentName.StartsWith("HR"))
			{
				transform = head_right;
			}
			else if (accoutermentName.StartsWith("CF"))
			{
				transform = chest_front;
			}
			else if (accoutermentName.StartsWith("CB"))
			{
				transform = chest_back;
			}
			if (transform != null && transform.childCount > 0)
			{
				Object.DestroyObject(transform.GetChild(0).gameObject);
			}
		}
	}

	public void DestroyAccoutermentExact(string accoutermentName)
	{
		if (accoutermentName != null && !(accoutermentName == string.Empty))
		{
			Transform transform = null;
			if (accoutermentName.StartsWith("HT"))
			{
				transform = head_top;
			}
			else if (accoutermentName.StartsWith("HF"))
			{
				transform = head_front;
			}
			else if (accoutermentName.StartsWith("HB"))
			{
				transform = head_back;
			}
			else if (accoutermentName.StartsWith("HL"))
			{
				transform = head_left;
			}
			else if (accoutermentName.StartsWith("HR"))
			{
				transform = head_right;
			}
			else if (accoutermentName.StartsWith("CF"))
			{
				transform = chest_front;
			}
			else if (accoutermentName.StartsWith("CB"))
			{
				transform = chest_back;
			}
			if (transform != null && transform.childCount > 0 && transform.GetChild(0).name == accoutermentName)
			{
				Object.DestroyObject(transform.GetChild(0).gameObject);
			}
		}
	}

	public void RemoveAllAccounterment()
	{
		for (int num = head_top.childCount - 1; num >= 0; num--)
		{
			Transform child = head_top.GetChild(num);
			child.parent = null;
			Object.DestroyObject(child.gameObject);
		}
		for (int num2 = head_front.childCount - 1; num2 >= 0; num2--)
		{
			Transform child2 = head_front.GetChild(num2);
			child2.parent = null;
			Object.DestroyObject(child2.gameObject);
		}
		for (int num3 = head_back.childCount - 1; num3 >= 0; num3--)
		{
			Transform child3 = head_back.GetChild(num3);
			child3.parent = null;
			Object.DestroyObject(child3.gameObject);
		}
		for (int num4 = head_left.childCount - 1; num4 >= 0; num4--)
		{
			Transform child4 = head_left.GetChild(num4);
			child4.parent = null;
			Object.DestroyObject(child4.gameObject);
		}
		for (int num5 = head_right.childCount - 1; num5 >= 0; num5--)
		{
			Transform child5 = head_right.GetChild(num5);
			child5.parent = null;
			Object.DestroyObject(child5.gameObject);
		}
		for (int num6 = chest_front.childCount - 1; num6 >= 0; num6--)
		{
			Transform child6 = chest_front.GetChild(num6);
			child6.parent = null;
			Object.DestroyObject(child6.gameObject);
		}
		for (int num7 = chest_back.childCount - 1; num7 >= 0; num7--)
		{
			Transform child7 = chest_back.GetChild(num7);
			child7.parent = null;
			Object.DestroyObject(child7.gameObject);
		}
	}

	public void FreezeAnimation()
	{
		base.animation.Sample();
		base.animation.Stop();
	}

	public void PlayMyAnimation(string key, string weaponSerial)
	{
		PlayMyAnimation(key, weaponSerial, string.Empty);
	}

	public void PlayMyAnimation(string key, string weaponSerial, Vector3 extra)
	{
		PlayMyAnimation(key, weaponSerial, string.Empty, extra);
	}

	public void PlayMyAnimation(string key, string weaponSerial, string param)
	{
		PlayMyAnimation(key, weaponSerial, param, Vector3.zero);
	}

	private bool isTankMode(string weaponSerial)
	{
		int result;
		switch (weaponSerial)
		{
		default:
			result = ((weaponSerial == "W019") ? 1 : 0);
			break;
		case "W011":
		case "W013":
		case "W014":
		case "W016":
			result = 1;
			break;
		}
		return (byte)result != 0;
	}

	public void PlayMyAnimation(string key, string weaponSerial, string param, Vector3 extra)
	{
		if ((playerCom != null && playerCom.IsFrozen) || (playerCom != null && playerCom._bRideRocket && !(key == "Point_down") && !(key == "Lookback_huang")))
		{
			return;
		}
		string[] array = weaponSerial.Split('_');
		weaponSerial = array[0];
		switch (key)
		{
		case "Fly_up":
			if (!base.animation.IsPlaying("Fly_up"))
			{
				PlayMyAnimation("Fly_up", 0.1f);
			}
			break;
		case "TakeOut_LongWeapon":
			if (!base.animation.IsPlaying("TwoHand_Take01"))
			{
				PlayMyAnimation("TwoHand_Take01", 0.1f);
			}
			if (base.animation.IsPlaying("ColdArms_Jump01") || base.animation.IsPlaying("ColdArms_Jump_air01"))
			{
				PlayMyAnimation("TwoHand_Jump_air01", 0.1f);
			}
			break;
		case "TakeOut_ShortWeapon":
			if (!base.animation.IsPlaying("ColdArms_Take01"))
			{
				PlayMyAnimation("ColdArms_Take01", 0.1f);
			}
			if (base.animation.IsPlaying("TwoHand_Jump01") || base.animation.IsPlaying("TwoHand_Jump_air01"))
			{
				PlayMyAnimation("ColdArms_Jump_air01", 0.1f);
			}
			break;
		case "Slide":
			if (!base.animation.IsPlaying("Slide"))
			{
				PlayMyAnimation("Slide", 0.1f);
			}
			break;
		case "Dizzy":
			if (!base.animation.IsPlaying("Dizzy"))
			{
				PlayMyAnimation("Dizzy", 0.1f);
			}
			break;
		case "Run00_fast":
			if (param == "0")
			{
				PlayMyAnimation("Run00_fast", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Run00_fast"))
			{
				PlayMyAnimation("Run00_fast", 0.2f);
			}
			break;
		case "Sit_Down":
			if (param == "0")
			{
				PlayMyAnimation("Sit_Down", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Sit_Down"))
			{
				PlayMyAnimation("Sit_Down", 0.2f);
			}
			break;
		case "Stand_Idle":
			if (param == "0")
			{
				PlayMyAnimation("Stand_Idle", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Stand_Idle"))
			{
				PlayMyAnimation("Stand_Idle", 0.2f);
			}
			break;
		case "Stand_Up":
			if (param == "0")
			{
				PlayMyAnimation("Stand_Up", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Stand_Up"))
			{
				PlayMyAnimation("Stand_Up", 0.2f);
			}
			break;
		case "Jump00_fly01":
			if (param == "0")
			{
				PlayMyAnimation("Jump00_fly01", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Jump00_fly01"))
			{
				PlayMyAnimation("Jump00_fly01", 0.2f);
			}
			break;
		case "Rockets":
			if (param == "0")
			{
				PlayMyAnimation("Rockets", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Rockets"))
			{
				PlayMyAnimation("Rockets", 0.2f);
			}
			break;
		case "Rockets03_huang":
			if (param == "0")
			{
				PlayMyAnimation("Rockets03_huang", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Rockets03_huang"))
			{
				PlayMyAnimation("Rockets03_huang", 0.2f);
			}
			break;
		case "Lookback_huang":
			if (param == "0")
			{
				PlayMyAnimation("Lookback_huang", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Lookback_huang"))
			{
				PlayMyAnimation("Lookback_huang", 0.2f);
			}
			break;
		case "Point_down":
			if (param == "0")
			{
				PlayMyAnimation("Point_down", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Point_down"))
			{
				PlayMyAnimation("Point_down", 0.2f);
			}
			break;
		case "Weightlessness":
			Debug.Log("Weightlessness");
			if (param == "0")
			{
				PlayMyAnimation("Weightlessness", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Weightlessness"))
			{
				PlayMyAnimation("Weightlessness", 0.2f);
			}
			break;
		case "Getdown":
			if (!base.animation.IsPlaying("Getdown"))
			{
				PlayMyAnimation("Getdown", 0.2f);
			}
			break;
		case "Falldown":
			if (!base.animation.IsPlaying("Falldown"))
			{
				PlayMyAnimation("Falldown", 0.2f);
			}
			break;
		case "Makeup":
			if (!base.animation.IsPlaying("Makeup"))
			{
				PlayMyAnimation("Makeup", 0.2f);
			}
			break;
		case "Makeup01":
			if (!base.animation.IsPlaying("Makeup01"))
			{
				PlayMyAnimation("Makeup01", 0.1f);
			}
			break;
		case "Makeup02":
			if (!base.animation.IsPlaying("Makeup02"))
			{
				PlayMyAnimation("Makeup02", 0.2f);
			}
			break;
		case "Show":
			if (param == "0")
			{
				PlayMyAnimation("Show", 0f, 0f);
			}
			else if (!base.animation.IsPlaying("Show"))
			{
				PlayMyAnimation("Show", 0.2f);
			}
			break;
		case "Win":
			if (!base.animation.IsPlaying("Win"))
			{
				PlayMyAnimation("Win", 0.2f);
			}
			break;
		case "Fishing_cancelpole":
			if (!base.animation.IsPlaying("Fishing_cancelpole"))
			{
				PlayMyAnimation("Fishing_cancelpole", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_cancelpole02":
			if (!base.animation.IsPlaying("Fishing_cancelpole02"))
			{
				PlayMyAnimation("Fishing_cancelpole02", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_castpole":
			if (!base.animation.IsPlaying("Fishing_castpole"))
			{
				PlayMyAnimation("Fishing_castpole", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_idle":
			if (!base.animation.IsPlaying("Fishing_idle"))
			{
				PlayMyAnimation("Fishing_idle", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_pullpole":
			if (!base.animation.IsPlaying("Fishing_pullpole"))
			{
				PlayMyAnimation("Fishing_pullpole", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_pullpole02":
			if (!base.animation.IsPlaying("Fishing_pullpole02"))
			{
				PlayMyAnimation("Fishing_pullpole02", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_pullpole_once":
			if (!base.animation.IsPlaying("Fishing_pullpole_once"))
			{
				PlayMyAnimation("Fishing_pullpole_once", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_success":
			if (!base.animation.IsPlaying("Fishing_success"))
			{
				PlayMyAnimation("Fishing_success", 0.2f, 1f, extra);
			}
			break;
		case "Fishing_unsuccess":
			if (!base.animation.IsPlaying("Fishing_unsuccess"))
			{
				PlayMyAnimation("Fishing_unsuccess", 0.2f, 1f, extra);
			}
			break;
		case "Ship_TurnLeft01":
			if (!base.animation.IsPlaying("Ship_TurnLeft01"))
			{
				PlayMyAnimation("Ship_TurnLeft01", 0.2f, 1f, extra);
			}
			break;
		case "Ship_Idle01":
			if (!base.animation.IsPlaying("Ship_Idle01"))
			{
				PlayMyAnimation("Ship_Idle01", 0.2f, 1f, extra);
			}
			break;
		case "Ship_Forward01":
			if (!base.animation.IsPlaying("Ship_Forward01"))
			{
				PlayMyAnimation("Ship_Forward01", 0.2f, 1f, extra);
			}
			break;
		case "Ship_TurnRight01":
			if (!base.animation.IsPlaying("Ship_TurnRight01"))
			{
				PlayMyAnimation("Ship_TurnRight01", 0.2f, 1f, extra);
			}
			break;
		case "Hello":
			if (!base.animation.IsPlaying("Hello"))
			{
				PlayMyAnimation("Hello", 0.2f, 1f, extra);
			}
			break;
		case "Lose":
			if (!base.animation.IsPlaying("Lose"))
			{
				PlayMyAnimation("Lose", 0.2f);
			}
			break;
		case "Idle":
			if (base.animation.IsPlaying("Attack01_Run"))
			{
				PlayMyAnimation("Attack01_Run", 0f, 0f);
			}
			switch (weaponSerial)
			{
			case "W000":
				if (!base.animation.IsPlaying("Idle00"))
				{
					PlayMyAnimation("Idle00", 0.2f);
				}
				break;
			case "W001":
			case "W008":
				if (!base.animation.IsPlaying("Idle11"))
				{
					PlayMyAnimation("Idle11", 0.2f);
				}
				break;
			default:
				if (isTankMode(weaponSerial))
				{
					if (!base.animation.IsPlaying("Tank_Idle"))
					{
						PlayMyAnimation("Tank_Idle", 0.2f);
					}
					break;
				}
				switch (weaponSerial)
				{
				case "W025":
				case "W026":
				case "W027":
					if (!base.animation.IsPlaying("ColdArms_Idle01"))
					{
						PlayMyAnimation("ColdArms_Idle01", 0.2f);
					}
					break;
				case "W020":
				case "W021":
				case "W022":
					if (!base.animation.IsPlaying("TwoHand_Idle01"))
					{
						PlayMyAnimation("TwoHand_Idle01", 0.2f);
					}
					break;
				default:
					if (!base.animation.IsPlaying("Idle01"))
					{
						PlayMyAnimation("Idle01", 0.2f);
					}
					break;
				}
				break;
			}
			break;
		case "Run":
		{
			string empty2 = string.Empty;
			switch (weaponSerial)
			{
			case "W000":
				empty2 = "Run00_" + param;
				break;
			case "W001":
			case "W008":
				empty2 = "Run11_" + param;
				break;
			default:
				if (isTankMode(weaponSerial))
				{
					empty2 = "Tank_Forward";
					break;
				}
				switch (weaponSerial)
				{
				case "W025":
				case "W026":
				case "W027":
					empty2 = "ColdArms_Run01_" + param;
					break;
				case "W020":
				case "W021":
				case "W022":
					empty2 = "TwoHand_Run01_" + param;
					break;
				default:
					empty2 = "Run01_" + param;
					break;
				}
				break;
			}
			if (!base.animation.IsPlaying(empty2))
			{
				PlayMyAnimation(empty2, 0.2f);
			}
			break;
		}
		case "Jump":
			switch (weaponSerial)
			{
			case "W001":
			case "W008":
				if (!base.animation.IsPlaying("Jump11") && !base.animation.IsPlaying("Jumpair11"))
				{
					string animName5 = ((Random.Range(0, 2) != 0) ? "Jumpair11" : "Jump11");
					PlayMyAnimation(animName5);
				}
				break;
			case "W000":
				if (!base.animation.IsPlaying("Jump00") && !base.animation.IsPlaying("Jumpair00"))
				{
					string animName8 = ((Random.Range(0, 2) != 0) ? "Jumpair00" : "Jump00");
					PlayMyAnimation(animName8);
				}
				break;
			case "W025":
			case "W026":
			case "W027":
				if (!base.animation.IsPlaying("ColdArms_Jump01") && !base.animation.IsPlaying("ColdArms_Jump_air01"))
				{
					string animName7 = ((Random.Range(0, 2) != 0) ? "ColdArms_Jump_air01" : "ColdArms_Jump01");
					PlayMyAnimation(animName7);
				}
				break;
			case "W020":
			case "W021":
			case "W022":
				if (!base.animation.IsPlaying("TwoHand_Jump01") && !base.animation.IsPlaying("TwoHand_Jump_air01"))
				{
					string animName6 = ((Random.Range(0, 2) != 0) ? "TwoHand_Jump_air01" : "TwoHand_Jump01");
					PlayMyAnimation(animName6);
				}
				break;
			default:
				if (!base.animation.IsPlaying("Jump01") && !base.animation.IsPlaying("Jumpair01"))
				{
					string animName4 = ((Random.Range(0, 2) != 0) ? "Jumpair01" : "Jump01");
					PlayMyAnimation(animName4);
				}
				break;
			}
			break;
		case "Attack":
			if (param == "Hold")
			{
				break;
			}
			switch (weaponSerial)
			{
			case "W001":
				if (param == "0")
				{
					PlayMyAnimation("Attack11_W001", 0f, 0f);
				}
				else
				{
					PlayMyAnimation("Attack11_W001", 0f);
				}
				break;
			case "W008":
				if (param == "0")
				{
					PlayMyAnimation("Attack11_W008", 0f, 0f);
				}
				else
				{
					PlayMyAnimation("Attack11_W008", 0f);
				}
				break;
			default:
				if (isTankMode(weaponSerial))
				{
					if (param == "0")
					{
						PlayMyAnimation("Tank_Fire", 0f, 0f);
						break;
					}
					Transform parent = base.transform.parent;
					Vector3 extra2 = Vector3.forward;
					if (parent != null && parent.name == "Mount")
					{
						Transform parent2 = parent.parent.parent;
						extra2 = parent2.eulerAngles;
					}
					PlayMyAnimation("Tank_Fire", 0f, 1f, extra2);
					break;
				}
				switch (weaponSerial)
				{
				case "W025":
					PlayMyAnimation("Attack01_Run", 0f, 0f);
					if (param == "0")
					{
						PlayMyAnimation("ColdArms_Attack01_LightSabre", 0f, 0f);
					}
					else
					{
						PlayMyAnimation("ColdArms_Attack01_LightSabre", 0.1f);
					}
					break;
				case "W026":
					PlayMyAnimation("Attack01_Run", 0f, 0f);
					if (param == "0")
					{
						PlayMyAnimation("ColdArms_Attack01_Mace", 0f, 0f);
					}
					else
					{
						PlayMyAnimation("ColdArms_Attack01_Mace", 0.1f);
					}
					break;
				case "W027":
					PlayMyAnimation("Attack01_Run", 0f, 0f);
					if (param == "0")
					{
						PlayMyAnimation("ColdArms_Attack01_Axe", 0f, 0f);
					}
					else
					{
						PlayMyAnimation("ColdArms_Attack01_Axe", 0.1f);
					}
					break;
				case "W020":
				case "W021":
				case "W022":
					if (param == "0")
					{
						PlayMyAnimation("Attack01_" + weaponSerial, 0f, 0f);
					}
					else
					{
						PlayMyAnimation("Attack01_" + weaponSerial, 0f);
					}
					break;
				default:
					PlayMyAnimation("Attack01_Run");
					if (param == "0")
					{
						PlayMyAnimation("Attack01_" + weaponSerial, 0f, 0f);
					}
					else
					{
						PlayMyAnimation("Attack01_" + weaponSerial, 0f);
					}
					break;
				}
				break;
			}
			break;
		case "Reload":
			if (!(weaponSerial == "W000") && !isTankMode(weaponSerial))
			{
				if (weaponSerial == "W001")
				{
					PlayMyAnimation("Reload11_W001", 0f);
				}
				else if (weaponSerial == "W008")
				{
					PlayMyAnimation("Reload11_W008", 0f);
				}
				else
				{
					PlayMyAnimation("Reload01_" + weaponSerial, 0f);
				}
			}
			break;
		case "Die":
		{
			string empty = string.Empty;
			switch (weaponSerial)
			{
			case "W000":
				empty = "Death00";
				if (param == "0")
				{
					if (base.animation.IsPlaying(empty))
					{
						PlayMyAnimation(empty, 0f, 0f);
					}
				}
				else if (!base.animation.IsPlaying(empty))
				{
					PlayMyAnimation(empty);
				}
				return;
			case "W001":
			case "W008":
				empty = "Death1";
				break;
			default:
				if (isTankMode(weaponSerial))
				{
					int num = Random.Range(1, 4);
					if (param == "0" && _lastTankDeadAnimName != string.Empty)
					{
						Debug.Log("------------------_lastTankDeadAnimName:" + _lastTankDeadAnimName);
						PlayMyAnimation(_lastTankDeadAnimName, 0f, 0f);
						return;
					}
					string text = "Tank_Death0" + num;
					Debug.Log("play dead anim:" + text);
					PlayMyAnimation(text);
					_lastTankDeadAnimName = text;
					return;
				}
				switch (weaponSerial)
				{
				case "W025":
				case "W026":
				case "W027":
					empty = "ColdArms_Death0";
					break;
				case "W020":
				case "W021":
				case "W022":
					empty = "TwoHand_Death0";
					break;
				default:
					empty = "Death0";
					break;
				}
				break;
			}
			if (param == "0")
			{
				string animName2 = empty + "1";
				string animName3 = empty + "2";
				if (base.animation.IsPlaying(animName2))
				{
					PlayMyAnimation(animName2, 0f, 0f);
				}
				else if (base.animation.IsPlaying(animName3))
				{
					PlayMyAnimation(animName3, 0f, 0f);
				}
			}
			else if (!base.animation.IsPlaying(empty + "1") && !base.animation.IsPlaying(empty + "2"))
			{
				empty += ((Random.Range(0, 2) != 1) ? "2" : "1");
				PlayMyAnimation(empty);
			}
			break;
		}
		case "TankHurt":
		{
			string animName = "Tank_Damage_" + param;
			if (!base.animation.IsPlaying(animName))
			{
				PlayMyAnimation(animName, 0.05f);
			}
			break;
		}
		default:
			Debug.LogError("Not defined key!!");
			break;
		}
	}

	private void PlayMyAnimation(string animName)
	{
		PlayMyAnimation(animName, 0.3f, 1f);
	}

	private void PlayMyAnimation(string animName, float fadeTime)
	{
		PlayMyAnimation(animName, fadeTime, 1f);
	}

	private void PlayMyAnimation(string animName, float fadeTime, float playSpeed)
	{
		PlayMyAnimation(animName, fadeTime, playSpeed, Vector3.zero);
	}

	private void PlayMyAnimation(string animName, float fadeTime, float playSpeed, Vector3 extra)
	{
		if (base.animation[animName] == null)
		{
			Debug.LogError(animName);
			return;
		}
		if (playSpeed == 0f)
		{
			base.animation.Stop(animName);
		}
		else
		{
			base.animation[animName].speed = playSpeed;
			if (fadeTime == 0f)
			{
				base.animation.Play(animName);
			}
			else
			{
				base.animation.CrossFade(animName, fadeTime);
			}
			if (_onPlayAnim != null)
			{
				_onPlayAnim(animName, playSpeed);
			}
		}
		if (COMA_Network.Instance.TNetInstance.IsContected())
		{
			COMA_CD_PlayerAnimation cOMA_CD_PlayerAnimation = COMA_CommandDatasFactory.CreateCommandDatas(COMA_Command.PLAYER_ANIMATION) as COMA_CD_PlayerAnimation;
			cOMA_CD_PlayerAnimation.btAnimName = animNameToIndex[animName];
			cOMA_CD_PlayerAnimation.btFadeTime = (byte)(fadeTime * 100f);
			cOMA_CD_PlayerAnimation.btPlaySeed = (byte)(playSpeed * 10f);
			cOMA_CD_PlayerAnimation.extra = extra;
			COMA_CommandHandler.Instance.Send(cOMA_CD_PlayerAnimation);
		}
	}

	private void AnimCallback_Fire()
	{
		if (playerCom != null)
		{
			playerCom.CharacterCall_Fire();
		}
	}

	private void AnimCallback_Fire2()
	{
		if (playerCom != null)
		{
			playerCom.CharacterCall_Fire2();
		}
	}

	protected void AnimCallback_PullPole()
	{
		AnimCallback_PullPole_True();
	}

	protected virtual void AnimCallback_PullPole_True()
	{
	}

	public void PlayRandomAnimationWithLoop()
	{
		byte key = (byte)Random.Range(0, animNameToIndex.Count);
		string text = animIndexToName[key];
		base.animation[text].wrapMode = WrapMode.Loop;
		base.animation.Play(text);
	}
}
