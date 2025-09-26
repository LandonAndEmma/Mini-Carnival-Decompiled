using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MD5Test : MonoBehaviour
{
	private void OnEnable()
	{
		Debug.Log("OnEnable");
	}

	private void OnDisable()
	{
		Debug.Log("OnDisable");
	}

	private void Start()
	{
		string text = "abc";
		string text2 = "b";
		string strB = "1bc";
		Debug.Log(text2.CompareTo(text));
		Debug.Log(text.CompareTo(strB));
	}

	public string GetFileMD5(string fileContent)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(fileContent);
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(bytes);
		string text = BitConverter.ToString(array);
		return text.Replace("-", string.Empty).ToLower();
	}
}
