using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class COMA_Scene_NPCController : MonoBehaviour
{
	private enum NPCState
	{
		Idle = 0,
		Walk = 1,
		Count = 2
	}

	private class StateCtl
	{
		public NPCState state;

		public float lastTime = 3f;

		public Vector3 forward = new Vector3(1f, 0f, 0f);

		public float speed = 1f;
	}

	public GameObject[] npcs;

	private List<StateCtl> lst_fsm = new List<StateCtl>();

	private StateCtl CreateState()
	{
		StateCtl stateCtl = new StateCtl();
		stateCtl.state = (NPCState)(0 + Random.Range(0, 2));
		stateCtl.lastTime = Random.Range(2f, 5f);
		float num = Random.Range(-1f, 1f);
		float num2 = Random.Range(-1f, 1f);
		if (Mathf.Abs(num) + Mathf.Abs(num2) < 0.1f)
		{
			num2 = -1f;
		}
		stateCtl.forward = new Vector3(num, 0f, num2);
		stateCtl.speed = Random.Range(0.6f, 1f);
		return stateCtl;
	}

	private void Start()
	{
		int i = 0;
		for (int num = npcs.Length; i < num; i++)
		{
			StateCtl stateCtl = CreateState();
			lst_fsm.Add(stateCtl);
			if (stateCtl.state == NPCState.Walk)
			{
				npcs[i].animation.CrossFade("Walk00", 0.2f);
				npcs[i].animation["Walk00"].speed = lst_fsm[i].speed;
			}
		}
		List<string> list = COMA_FileIO.SearchFriendFiles("Friends", npcs.Length);
		for (int j = 0; j < list.Count; j++)
		{
			string json = COMA_FileIO.LoadFile("Friends/" + list[j]);
			JsonData jsonData = JsonMapper.ToObject<JsonData>(json);
			string text = (string)jsonData["Name"];
			string text2 = (string)jsonData["Package"];
			string text3 = (string)jsonData["Data"];
			string[] array = text3.Split('^');
			int num2 = 1;
			int[] array2 = new int[3]
			{
				int.Parse(array[num2++]),
				int.Parse(array[num2++]),
				int.Parse(array[num2++])
			};
			int[] array3 = new int[7]
			{
				int.Parse(array[num2++]),
				int.Parse(array[num2++]),
				int.Parse(array[num2++]),
				int.Parse(array[num2++]),
				int.Parse(array[num2++]),
				int.Parse(array[num2++]),
				int.Parse(array[num2++])
			};
			string[] array4 = new string[7];
			text2 = text2.Substring(text2.IndexOf(';') + 1);
			string[] array5 = text2.Split(';');
			for (int k = 0; k < array3.Length; k++)
			{
				if (array3[k] < 0)
				{
					array4[k] = string.Empty;
					continue;
				}
				array4[k] = array5[array3[k]];
				if (array4[k] != string.Empty)
				{
					string[] array6 = array4[k].Split('^');
					array4[k] = array6[0];
				}
			}
			COMA_PlayerCharacter component = npcs[j].GetComponent<COMA_PlayerCharacter>();
			component.RemoveAllAccounterment();
			for (int l = 0; l < array4.Length; l++)
			{
				component.CreateAccouterment(array4[l]);
			}
			string[] array7 = new string[3]
			{
				"Friends/" + list[j] + "_0",
				"Friends/" + list[j] + "_1",
				"Friends/" + list[j] + "_2"
			};
			Texture2D[] array8 = new Texture2D[3];
			for (int m = 0; m < array7.Length; m++)
			{
				byte[] array9 = COMA_FileIO.ReadPngData(array7[m]);
				if (array9 != null)
				{
					array8[m] = new Texture2D(COMA_TexBase.Instance.width, COMA_TexBase.Instance.height, TextureFormat.RGB24, false);
					array8[m].LoadImage(array9);
					array8[m].filterMode = FilterMode.Point;
				}
			}
			if (array8[0] != null)
			{
				npcs[j].transform.FindChild("head").renderer.material.mainTexture = array8[0];
			}
			if (array8[1] != null)
			{
				npcs[j].transform.FindChild("body").renderer.material.mainTexture = array8[1];
			}
			if (array8[2] != null)
			{
				npcs[j].transform.FindChild("breeches").renderer.material.mainTexture = array8[2];
			}
		}
	}

	private void Update()
	{
		int i = 0;
		for (int count = lst_fsm.Count; i < count; i++)
		{
			lst_fsm[i].lastTime -= Time.deltaTime;
			if (lst_fsm[i].lastTime <= 0f)
			{
				lst_fsm[i] = CreateState();
				if (lst_fsm[i].state == NPCState.Idle)
				{
					npcs[i].animation.CrossFade("Idle00", 0.2f);
				}
				else if (lst_fsm[i].state == NPCState.Walk)
				{
					npcs[i].animation.CrossFade("Walk00", 0.2f);
					npcs[i].animation["Walk00"].speed = lst_fsm[i].speed;
				}
			}
			else if (lst_fsm[i].state == NPCState.Walk)
			{
				npcs[i].transform.forward = Vector3.Lerp(npcs[i].transform.forward, lst_fsm[i].forward, 0.1f);
				Vector3 position = npcs[i].transform.position + npcs[i].transform.forward * lst_fsm[i].speed * Time.deltaTime;
				if ((position.x > -6f && position.x < 6f && position.z > -6f && position.z < 6f) || position.x < -12f || position.x > 12f || position.z < -12f || position.z > 12f)
				{
					npcs[i].transform.forward = -npcs[i].transform.forward;
					lst_fsm[i].forward = npcs[i].transform.forward;
				}
				else
				{
					npcs[i].transform.position = position;
				}
			}
		}
	}
}
