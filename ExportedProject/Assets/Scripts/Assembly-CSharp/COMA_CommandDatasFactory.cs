using UnityEngine;

public class COMA_CommandDatasFactory
{
	public static COMA_CommandDatas CreateCommandDatas(COMA_Command command)
	{
		COMA_CommandDatas cOMA_CommandDatas = null;
		switch (command)
		{
		case COMA_Command.PLAYER_CREATE:
			cOMA_CommandDatas = new COMA_CD_PlayerCreate();
			break;
		case COMA_Command.PLAYER_TEXTUREINIT:
			cOMA_CommandDatas = new COMA_CD_PlayerTextureInit();
			break;
		case COMA_Command.PLAYER_TRANSFORM:
			cOMA_CommandDatas = new COMA_CD_PlayerTransform();
			break;
		case COMA_Command.PLAYER_ANIMATION:
			cOMA_CommandDatas = new COMA_CD_PlayerAnimation();
			break;
		case COMA_Command.PLAYER_BUFF:
			cOMA_CommandDatas = new COMA_CD_PlayerBuff();
			break;
		case COMA_Command.PLAYER_SWITCHWEAPON:
			cOMA_CommandDatas = new COMA_CD_PlayerSwitchWeapon();
			break;
		case COMA_Command.PLAYER_HIT:
			cOMA_CommandDatas = new COMA_CD_PlayerHit();
			break;
		case COMA_Command.PLAYER_SCOREGET:
			cOMA_CommandDatas = new COMA_CD_PlayerScoreGet();
			break;
		case COMA_Command.PLAYER_HPSET:
			cOMA_CommandDatas = new COMA_CD_PlayerHPSet();
			break;
		case COMA_Command.ENEMY_TRANSFORM:
			cOMA_CommandDatas = new COMA_CD_EnemyTransform();
			break;
		case COMA_Command.ENEMY_ANIMATION:
			cOMA_CommandDatas = new COMA_CD_EnemyAnimation();
			break;
		case COMA_Command.ENEMY_HIT:
			cOMA_CommandDatas = new COMA_CD_EnemyHit();
			break;
		case COMA_Command.ITEMCREATE:
			cOMA_CommandDatas = new COMA_CD_CreateItem();
			break;
		case COMA_Command.ITEMDELETE:
			cOMA_CommandDatas = new COMA_CD_DeleteItem();
			break;
		case COMA_Command.BLOCKDELETE:
			cOMA_CommandDatas = new COMA_CD_DeleteBlock();
			break;
		case COMA_Command.RUN_CREATEROAD:
			cOMA_CommandDatas = new COMA_CD_CreateRoad();
			break;
		case COMA_Command.RUN_USEITEMINFO:
			cOMA_CommandDatas = new COMA_CD_UseItemInfo();
			break;
		case COMA_Command.PUTMINE:
			cOMA_CommandDatas = new COMA_CD_PutMine();
			break;
		case COMA_Command.DELMINE:
			cOMA_CommandDatas = new COMA_CD_DelMine();
			break;
		case COMA_Command.POUNCE:
			cOMA_CommandDatas = new COMA_CD_Pounce();
			break;
		case COMA_Command.EVIL:
			cOMA_CommandDatas = new COMA_CD_BeEvil();
			break;
		case COMA_Command.CREATEEVILITEM:
			cOMA_CommandDatas = new COMA_CD_CreateEvilItem();
			break;
		case COMA_Command.DELETEEVILITEM:
			cOMA_CommandDatas = new COMA_CD_DeleteEvilItem();
			break;
		case COMA_Command.GAME_TIME:
			cOMA_CommandDatas = new COMA_CD_GameTime();
			break;
		case COMA_Command.GAME_START:
			cOMA_CommandDatas = new COMA_CD_GameStart();
			break;
		case COMA_Command.CHAT:
			cOMA_CommandDatas = new COMA_CD_Chatting();
			break;
		case COMA_Command.FISHING_INFO:
			cOMA_CommandDatas = new COMA_CD_FishingInfo();
			break;
		case COMA_Command.FISHING_PRAISE:
			cOMA_CommandDatas = new COMA_CD_FishingPraise();
			break;
		case COMA_Command.FISHING_ONBOAT:
			cOMA_CommandDatas = new COMA_CD_FishingOnBoat();
			break;
		case COMA_Command.FISHING_OFFBOAT:
			cOMA_CommandDatas = new COMA_CD_FishingOffBoat();
			break;
		case COMA_Command.FISHING_STATE:
			cOMA_CommandDatas = new COMA_CD_FishingState();
			break;
		case COMA_Command.FISHING_UPDATE_FISHROT:
			cOMA_CommandDatas = new COMA_CD_FishingUpdateFishrot();
			break;
		default:
			Debug.LogError("No Command Defined!!");
			break;
		}
		cOMA_CommandDatas.Command = command;
		return cOMA_CommandDatas;
	}
}
