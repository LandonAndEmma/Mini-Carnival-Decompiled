using HuffmanCode;
using UnityEngine;

public class HuffmanTest : MonoBehaviour
{
	private static HuffmanTest _instance;

	public static HuffmanTest Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new HuffmanTest();
			}
			return _instance;
		}
	}

	private void Start()
	{
		string text = "abababababababc";
		TreeList treeList = new TreeList(text);
		for (int i = 0; i < text.Length; i++)
		{
			treeList.AddSign(text[i].ToString());
		}
		treeList.SortTree();
		while (treeList.Lenght() > 1)
		{
			treeList.MergeTree();
		}
		TreeList.MakeKey(treeList.RemoveTree(), string.Empty);
		string text2 = TreeList.Translate(text);
		string[] signTable = treeList.GetSignTable();
		string[] keyTable = treeList.GetKeyTable();
		for (int j = 0; j < signTable.Length; j++)
		{
			Debug.Log(signTable[j] + ": " + keyTable[j]);
		}
		Debug.Log("The original string is " + text.Length * 16 + "bits long . ");
		Debug.Log("the new string is " + text2.Length + "bits long  . ");
		Debug.Log("The coded string looks like this : " + text2);
	}
}
