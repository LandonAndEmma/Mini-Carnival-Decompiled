using System.Collections;
using UnityEngine;

public class COMA_AudioManager
{
	private static COMA_AudioManager _instance;

	private string defaultFileName = COMA_FileNameManager.Instance.GetFileName("AudioControl");

	private char sepSign = 'X';

	private bool soundOn = true;

	private bool musicOn = true;

	private GameObject musicObj;

	private string musicName = string.Empty;

	private ArrayList envSoundList = new ArrayList();

	private float _fPreRPGCommonAudioTime;

	public static COMA_AudioManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_AudioManager();
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			return ((!bSound) ? "0" : "1") + sepSign + ((!bMusic) ? "0" : "1") + sepSign + COMA_Sys.Instance.sensitivity.ToString();
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				bSound = array[0] == "1";
				bMusic = array[1] == "1";
				if (array.Length >= 3)
				{
					COMA_Sys.Instance.sensitivity = float.Parse(array[2]);
				}
			}
		}
	}

	public bool bSound
	{
		get
		{
			return soundOn;
		}
		set
		{
			soundOn = value;
			TAudioController.bSound = soundOn;
			ResetEnvironmentSound();
		}
	}

	public bool bMusic
	{
		get
		{
			return musicOn;
		}
		set
		{
			musicOn = value;
			if (musicObj != null)
			{
				musicObj.audio.mute = !musicOn;
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}

	public void SetEnvironmentSound(GameObject[] audioObjs)
	{
		envSoundList.Clear();
		foreach (GameObject gameObject in audioObjs)
		{
			gameObject.audio.mute = !soundOn;
			envSoundList.Add(gameObject);
		}
	}

	private void ResetEnvironmentSound()
	{
		foreach (GameObject envSound in envSoundList)
		{
			envSound.audio.mute = !soundOn;
		}
	}

	public void MusicPlay(AudioCategory kind)
	{
		string text = string.Empty;
		switch (kind)
		{
		case AudioCategory.RPG_Lose:
			text = "BGM_Lose";
			break;
		case AudioCategory.RPG_Precombat:
			text = "BGM_PreCombat";
			break;
		case AudioCategory.RPG_Win:
			text = "BGM_Win";
			break;
		case AudioCategory.BGM_Boss_Combat01:
			text = "BGM_Boss_Combat01";
			break;
		case AudioCategory.BGM_Castle01:
			text = "BGM_Castle01";
			break;
		case AudioCategory.BGM_Combat01:
			text = "BGM_Combat01";
			break;
		case AudioCategory.BGM_Menu:
			text = "BGM_Menu";
			break;
		case AudioCategory.BGM_Scene_Castle:
			text = "BGM_Scene_Castle";
			break;
		case AudioCategory.BGM_Scene_Hunger:
			text = "BGM_Scene_Hunger";
			break;
		case AudioCategory.BGM_Scene_Rocket:
			text = "BGM_Scene_Rocket";
			break;
		case AudioCategory.BGM_Scene_Run:
			text = "BGM_Scene_Run";
			break;
		case AudioCategory.BGM_Scene_Treasure:
			text = "BGM_Scene_Treasure";
			break;
		case AudioCategory.BGM_ScoreCheck:
			text = "BGM_ScoreCheck";
			break;
		case AudioCategory.Amb_Island:
			text = "Amb_Island";
			break;
		case AudioCategory.BGM_Scene_Tank:
			text = "BGM_Scene_Tank";
			break;
		case AudioCategory.BGM_Tank_Win:
			text = "BGM_Tank_Win";
			break;
		case AudioCategory.BGM_Tank_Lose:
			text = "BGM_Tank_Lose";
			break;
		default:
			Debug.LogError("不存在");
			break;
		}
		if (!(musicName == text))
		{
			musicName = text;
			if (musicObj != null)
			{
				Object.DestroyObject(musicObj);
			}
			musicObj = Object.Instantiate(Resources.Load("SoundEvent/" + musicName)) as GameObject;
			Object.DontDestroyOnLoad(musicObj);
			TAudioEffectRandom component = musicObj.GetComponent<TAudioEffectRandom>();
			component.Trigger();
			musicObj.audio.mute = !musicOn;
		}
	}

	public void SoundPlay(AudioCategory kind)
	{
		SoundPlay(kind, null, 3f, true);
	}

	public void SoundPlay(AudioCategory kind, Transform belongTrs)
	{
		SoundPlay(kind, belongTrs, 3f, true);
	}

	public void SoundPlay(AudioCategory kind, Transform belongTrs, float lastTime)
	{
		SoundPlay(kind, belongTrs, lastTime, true);
	}

	public void SoundPlay(AudioCategory kind, Transform belongTrs, float lastTime, bool bAutoDestroy)
	{
		if (!bSound)
		{
			return;
		}
		GameObject gameObject = null;
		Transform transform = null;
		if (!bAutoDestroy)
		{
			transform = belongTrs.FindChild(kind.ToString() + "(Clone)");
		}
		if (transform != null)
		{
			gameObject = transform.gameObject;
		}
		else
		{
			switch (kind)
			{
			case AudioCategory.UI_AchievementPop:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_achievement")) as GameObject;
				break;
			case AudioCategory.UI_Back:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_back")) as GameObject;
				break;
			case AudioCategory.UI_Buy_Crystal:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_buy_crastal")) as GameObject;
				break;
			case AudioCategory.UI_Buy_Gold:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_buy_gold")) as GameObject;
				break;
			case AudioCategory.UI_Buy_Fail:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_can't_buy")) as GameObject;
				break;
			case AudioCategory.UI_Start_CarnivalIn:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Carnival_in")) as GameObject;
				break;
			case AudioCategory.UI_Start_CarnivalOut:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Carnival_out")) as GameObject;
				break;
			case AudioCategory.UI_Click:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_click")) as GameObject;
				break;
			case AudioCategory.UI_EnterMarket:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_marketicon")) as GameObject;
				break;
			case AudioCategory.UI_NextPage:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_next")) as GameObject;
				break;
			case AudioCategory.UI_TimeCountDown:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_no_time")) as GameObject;
				break;
			case AudioCategory.UI_Settlement_ScoreRolling:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_score")) as GameObject;
				break;
			case AudioCategory.UI_DiscardItem:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_delete")) as GameObject;
				break;
			case AudioCategory.UI_Close:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_close")) as GameObject;
				break;
			case AudioCategory.UI_Unlock:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_unlock")) as GameObject;
				break;
			case AudioCategory.UI_BuyItem:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_item_buy")) as GameObject;
				break;
			case AudioCategory.UI_SellItem:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_item_sell")) as GameObject;
				break;
			case AudioCategory.UI_SelectMode:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_select_mode")) as GameObject;
				break;
			case AudioCategory.UI_ModeStart:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_intro_start01")) as GameObject;
				break;
			case AudioCategory.UI_EnterWaitingRoom:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_intro_start02")) as GameObject;
				break;
			case AudioCategory.UI_Finish:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_finish")) as GameObject;
				break;
			case AudioCategory.UI_In:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_in")) as GameObject;
				break;
			case AudioCategory.UI_Out:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_out")) as GameObject;
				break;
			case AudioCategory.Game_CountDown3:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_start_3")) as GameObject;
				break;
			case AudioCategory.Game_CountDown2:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_start_2")) as GameObject;
				break;
			case AudioCategory.Game_CountDown1:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_start_1")) as GameObject;
				break;
			case AudioCategory.Game_CountDownStart:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_start_0")) as GameObject;
				break;
			case AudioCategory.Game_Cheer:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_intro_cheer")) as GameObject;
				break;
			case AudioCategory.Game_Camera:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_intro_whoosh")) as GameObject;
				break;
			case AudioCategory.Game_LevelUp:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_level_up")) as GameObject;
				break;
			case AudioCategory.FX_Gold_appear:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Gold_appear")) as GameObject;
				break;
			case AudioCategory.FX_Gold_Jet:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Gold_Jet")) as GameObject;
				break;
			case AudioCategory.FX_Get_Gold:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Gold")) as GameObject;
				break;
			case AudioCategory.FX_Get_Ammunition:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Ammunition")) as GameObject;
				break;
			case AudioCategory.FX_Get_Weapon:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Weapon")) as GameObject;
				break;
			case AudioCategory.FX_Get_Banner:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Banner")) as GameObject;
				break;
			case AudioCategory.FX_Get_Cloak:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Cloak")) as GameObject;
				break;
			case AudioCategory.FX_Get_Ham:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Ham")) as GameObject;
				break;
			case AudioCategory.FX_Get_item:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_item")) as GameObject;
				break;
			case AudioCategory.FX_Get_item_01:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_item_01")) as GameObject;
				break;
			case AudioCategory.FX_Get_Max_ammo:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Max_ammo")) as GameObject;
				break;
			case AudioCategory.FX_Get_Mine:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Mine")) as GameObject;
				break;
			case AudioCategory.FX_Mine_bomb:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Mine_bomb")) as GameObject;
				break;
			case AudioCategory.FX_Get_shandian:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_shandian")) as GameObject;
				break;
			case AudioCategory.FX_Get_Shoe:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Get_Shoe")) as GameObject;
				break;
			case AudioCategory.FX_Transmission:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Transmission_Deliver")) as GameObject;
				break;
			case AudioCategory.FX_JiaXue:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_JiaXue")) as GameObject;
				break;
			case AudioCategory.FX_Shoe_JiaSu:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Shoe_JiaSu")) as GameObject;
				break;
			case AudioCategory.FX_Transfiguration:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Transfiguration")) as GameObject;
				break;
			case AudioCategory.FX_Stealth:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Stealth")) as GameObject;
				break;
			case AudioCategory.FX_Gate_up:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Gate_up")) as GameObject;
				break;
			case AudioCategory.FX_Crystal_Broken:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Crystal_Broken")) as GameObject;
				break;
			case AudioCategory.FX_flash_kill:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_flash_kill")) as GameObject;
				break;
			case AudioCategory.FX_restart:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_restart")) as GameObject;
				break;
			case AudioCategory.FX_Absorb:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Absorb")) as GameObject;
				break;
			case AudioCategory.FX_stun:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_stun")) as GameObject;
				break;
			case AudioCategory.FX_Swoop:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Swoop")) as GameObject;
				break;
			case AudioCategory.FX_Blood:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Blood")) as GameObject;
				break;
			case AudioCategory.FX_Exhaust:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Exhaust")) as GameObject;
				break;
			case AudioCategory.FX_Screen_ink_use:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Screen_ink_use")) as GameObject;
				break;
			case AudioCategory.FX_Screen_ink:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Screen_ink")) as GameObject;
				break;
			case AudioCategory.FX_Mine_use:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Mine_use")) as GameObject;
				break;
			case AudioCategory.FX_Shield:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Shield")) as GameObject;
				break;
			case AudioCategory.FX_Curse_use:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Curse_use")) as GameObject;
				break;
			case AudioCategory.FX_Curse:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Curse")) as GameObject;
				break;
			case AudioCategory.FX_Fire:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Fire")) as GameObject;
				break;
			case AudioCategory.FX_Fire_burst:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Fire_burst")) as GameObject;
				break;
			case AudioCategory.FX_Rocket_Appear:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Rocket_Appear")) as GameObject;
				break;
			case AudioCategory.FX_Rocket_Fly_A:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Rocket_Fly_A")) as GameObject;
				break;
			case AudioCategory.FX_Rocket_Fly_B:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Rocket_Fly_B")) as GameObject;
				break;
			case AudioCategory.FX_Rocket_Fly_C:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Rocket_Fly_C")) as GameObject;
				break;
			case AudioCategory.FX_Spring:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Spring")) as GameObject;
				break;
			case AudioCategory.FX_Sweat:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Sweat")) as GameObject;
				break;
			case AudioCategory.FX_Sweat_use:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Sweat_use")) as GameObject;
				break;
			case AudioCategory.UI_Page_Turn:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Page_Turn")) as GameObject;
				break;
			case AudioCategory.UI_Fishing_Good_Popup:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Fishing_Good_Popup")) as GameObject;
				break;
			case AudioCategory.UI_Fishing_Useless_Popup:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Fishing_Useless_Popup")) as GameObject;
				break;
			case AudioCategory.UI_Type_Normal:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Type_Normal")) as GameObject;
				break;
			case AudioCategory.UI_Type_Other:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Type_Other")) as GameObject;
				break;
			case AudioCategory.UI_Type_Done:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Type_Done")) as GameObject;
				break;
			case AudioCategory.UI_Type_Cancel:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Type_Cancel")) as GameObject;
				break;
			case AudioCategory.UI_Fishing_Powerup:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Fishing_Powerup")) as GameObject;
				break;
			case AudioCategory.Ani_Player_joy_jump:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Player_joy_jump")) as GameObject;
				break;
			case AudioCategory.Ani_Fish_Splash:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fish_Splash")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_Throw:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fishing_Throw")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_Float:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fishing_Float")) as GameObject;
				break;
			case AudioCategory.Ani_Fish_Bite:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fish_Bite")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_Draw:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fishing_Draw")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_Draw_Empty:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fishing_Draw_Empty")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_WOW:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fishing_WOW")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_Inhand:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Fishing_Inhand")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_BePraise:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/UI_Redheart")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_OnSail:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Amb_Boat_move")) as GameObject;
				break;
			case AudioCategory.Ani_Fishing_StopBoat:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Amb_Boat_stop")) as GameObject;
				break;
			case AudioCategory.Amb_Seagull:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Amb_Seagull")) as GameObject;
				break;
			case AudioCategory.Amb_Whale_Appear:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Amb_Whale_Appear")) as GameObject;
				break;
			case AudioCategory.Amb_Whale_Disappear:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Amb_Whale_Disappear")) as GameObject;
				break;
			case AudioCategory.Amb_Tree_In_Wind:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Amb_Tree_In_Wind")) as GameObject;
				break;
			case AudioCategory.Ani_Get_Weapon:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Get_Weapon")) as GameObject;
				break;
			case AudioCategory.Ani_Weapon_Change:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Weapon_Change")) as GameObject;
				break;
			case AudioCategory.FX_Impact_Player:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Impact_Player")) as GameObject;
				break;
			case AudioCategory.FX_Gun_Rico:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Gun_Rico")) as GameObject;
				break;
			case AudioCategory.FX_LightSabre_Impact:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_LightSabre_Impact")) as GameObject;
				break;
			case AudioCategory.FX_Mace_Impact:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Mace_Impact")) as GameObject;
				break;
			case AudioCategory.FX_Axe_Impact:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Axe_Impact")) as GameObject;
				break;
			case AudioCategory.FX_Mace_Stun:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Mace_Stun")) as GameObject;
				break;
			case AudioCategory.FX_Axe_TurnAround:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Axe_TurnAround")) as GameObject;
				break;
			case AudioCategory.FX_Player_Death:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Player_Death")) as GameObject;
				break;
			case AudioCategory.FX_Player_Explode:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Player_Explode")) as GameObject;
				break;
			case AudioCategory.Ani_Tank_Move:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Tank_Move")) as GameObject;
				break;
			case AudioCategory.Ani_Tank01_Fire:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Tank01_Fire")) as GameObject;
				break;
			case AudioCategory.Ani_Tank02_Fire:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Tank02_Fire")) as GameObject;
				break;
			case AudioCategory.Ani_Tank03_Fire:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Tank03_Fire")) as GameObject;
				break;
			case AudioCategory.Ani_Tank04_Fire:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/Ani_Tank04_Fire")) as GameObject;
				break;
			case AudioCategory.FX_energy_gun_BulletFly:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_energy_gun_BulletFly")) as GameObject;
				break;
			case AudioCategory.FX_energy_gun_Smitten:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_energy_gun_Smitten")) as GameObject;
				break;
			case AudioCategory.FX_Tank_Miss:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Tank_Miss")) as GameObject;
				break;
			case AudioCategory.FX_Tank01_Blast:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Tank01_Blast")) as GameObject;
				break;
			case AudioCategory.FX_Tank02_Blast:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Tank02_Blast")) as GameObject;
				break;
			case AudioCategory.FX_Tank03_Blast:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Tank03_Blast")) as GameObject;
				break;
			case AudioCategory.FX_Wall_Destroy:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Wall_Destroy")) as GameObject;
				break;
			case AudioCategory.FX_Can_Explode:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Can_Explode")) as GameObject;
				break;
			case AudioCategory.FX_Rebirth:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Rebirth")) as GameObject;
				break;
			case AudioCategory.FX_Tank_Destroy:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Tank_Destroy")) as GameObject;
				break;
			case AudioCategory.FX_Ice_Crack:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/FX_Ice_Crack")) as GameObject;
				break;
			case AudioCategory.UI_Applaud:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Applaud")) as GameObject;
				break;
			case AudioCategory.UI_Backpack:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Backpack")) as GameObject;
				break;
			case AudioCategory.UI_Bar_Drawback:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Bar_Drawback")) as GameObject;
				break;
			case AudioCategory.UI_Bar_Drawout:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Bar_Drawout")) as GameObject;
				break;
			case AudioCategory.UI_Click_Sort:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Click_Sort")) as GameObject;
				break;
			case AudioCategory.UI_Collect:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Collect")) as GameObject;
				break;
			case AudioCategory.UI_Equipped:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Equipped")) as GameObject;
				break;
			case AudioCategory.UI_Figure:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Figure")) as GameObject;
				break;
			case AudioCategory.UI_Follow:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Follow")) as GameObject;
				break;
			case AudioCategory.UI_Gamezone:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Gamezone")) as GameObject;
				break;
			case AudioCategory.UI_Refresh:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Refresh")) as GameObject;
				break;
			case AudioCategory.UI_Select:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Select")) as GameObject;
				break;
			case AudioCategory.UI_Unequipped:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.0/UI_Unequipped")) as GameObject;
				break;
			case AudioCategory.RPG_Miss:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Miss")) as GameObject;
				break;
			case AudioCategory.RPG_Stun_Debuff:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Stun_Debuff")) as GameObject;
				break;
			case AudioCategory.RPG_Ice_debuff:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Ice_debuff")) as GameObject;
				break;
			case AudioCategory.RPG_BUFF_Rock_Fly:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/RPG_BUFF_Rock_Fly")) as GameObject;
				break;
			case AudioCategory.RPG_Buff_Common:
				if (_fPreRPGCommonAudioTime != Time.time)
				{
					_fPreRPGCommonAudioTime = Time.time;
					gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Buff01")) as GameObject;
				}
				break;
			case AudioCategory.RPG_Card_upgrade:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Card_upgrade")) as GameObject;
				break;
			case AudioCategory.RPG_Click_move:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Click_move")) as GameObject;
				break;
			case AudioCategory.RPG_Click_seating:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Click_seating")) as GameObject;
				break;
			case AudioCategory.RPG_Coin_collect:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Coin_collect")) as GameObject;
				break;
			case AudioCategory.RPG_Coin_collectall:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Coin_collectall")) as GameObject;
				break;
			case AudioCategory.RPG_Gem_upgrade:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Gem_upgrade")) as GameObject;
				break;
			case AudioCategory.RPG_GetCard:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/GetCard")) as GameObject;
				break;
			case AudioCategory.RPG_GetCard_elite:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/GetCard_elite")) as GameObject;
				break;
			case AudioCategory.RPG_GetCard_new:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/GetCard_new")) as GameObject;
				break;
			case AudioCategory.RPG_GetCard_open01:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/GetCard_open01")) as GameObject;
				break;
			case AudioCategory.RPG_GetCard_open02:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/GetCard_open02")) as GameObject;
				break;
			case AudioCategory.RPG_GetCard_open03:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/GetCard_open03")) as GameObject;
				break;
			case AudioCategory.RPG_levelup:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/levelup")) as GameObject;
				break;
			case AudioCategory.RPG_RPG_SnowMonster_Fly:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/RPG_SnowMonster_Fly")) as GameObject;
				break;
			case AudioCategory.RPG_Scrollup:
				gameObject = Object.Instantiate(Resources.Load("SoundEvent/2.1/Scrollup")) as GameObject;
				break;
			default:
				Debug.LogError(kind);
				return;
			}
		}
		if (belongTrs != null)
		{
			gameObject.transform.parent = belongTrs;
			gameObject.transform.localPosition = Vector3.zero;
		}
		if (gameObject != null)
		{
			ITAudioEvent component = gameObject.GetComponent<ITAudioEvent>();
			component.Trigger();
		}
		if (gameObject != null && bAutoDestroy)
		{
			Object.DestroyObject(gameObject, lastTime * Time.timeScale);
		}
	}
}
