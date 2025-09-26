using System.Collections.Generic;
using UnityEngine;

public class SS_Harmonious : MonoBehaviour
{
	private static SS_Harmonious m_instance;

	public TextAsset m_words;

	private Dictionary<char, List<string>> m_dictionaryLib = new Dictionary<char, List<string>>();

	public static SS_Harmonious Instance
	{
		get
		{
			return m_instance;
		}
	}

	private void Awake()
	{
		m_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		string[] array = m_words.text.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i] == string.Empty))
			{
				array[i] = array[i].ToUpper();
				if (m_dictionaryLib.ContainsKey(array[i][0]))
				{
					m_dictionaryLib[array[i][0]].Add(array[i]);
					continue;
				}
				List<string> list = new List<string>();
				list.Add(array[i]);
				m_dictionaryLib.Add(array[i][0], list);
			}
		}
	}

	private bool NeedHarmonize(string word)
	{
		if (word == null || word == string.Empty)
		{
			return false;
		}
		if (!m_dictionaryLib.ContainsKey(word[0]))
		{
			return false;
		}
		List<string> list = m_dictionaryLib[word[0]];
		for (int i = 0; i < list.Count; i++)
		{
			if (word.Contains(list[i]))
			{
				return true;
			}
		}
		return false;
	}

	private string GetHarmoniousWord(string word)
	{
		if (word == null || word == string.Empty)
		{
			return word;
		}
		string word2 = word.ToUpper();
		if (!NeedHarmonize(word2))
		{
			return word;
		}
		string text = "*";
		for (int i = 1; i < word.Length; i += 2)
		{
			text += "*";
		}
		return text;
	}

	public string GetHarmoniousSentence(string sentence)
	{
		if (sentence == null || sentence == string.Empty)
		{
			return sentence;
		}
		string[] array = sentence.Split(' ');
		string text = GetHarmoniousWord(array[0]);
		for (int i = 1; i < array.Length; i++)
		{
			text = text + " " + GetHarmoniousWord(array[i]);
		}
		return text;
	}
}
