using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TUITextManager
{
	private static TUITextManager s_Instance;

	private static bool bParser;

	private Dictionary<string, string> m_enTextCenter = new Dictionary<string, string>();

	private Dictionary<string, string> m_cnTextCenter = new Dictionary<string, string>();

	public static TUITextManager Instance()
	{
		if (s_Instance == null)
		{
			s_Instance = new TUITextManager();
		}
		return s_Instance;
	}

	public void Parser(string enXmlPath, string cnXmlPath)
	{
		if (!bParser)
		{
			TextAsset textAsset = Resources.Load(enXmlPath) as TextAsset;
			ParserXml(textAsset.text, m_enTextCenter);
			TextAsset textAsset2 = Resources.Load(cnXmlPath) as TextAsset;
			ParserXml(textAsset2.text, m_cnTextCenter);
			bParser = true;
		}
	}

	public string GetString(string id)
	{
		Dictionary<string, string> center = GetCenter();
		if (center.ContainsKey(id))
		{
			return center[id];
		}
		return string.Empty;
	}

	private Dictionary<string, string> GetCenter()
	{
		if (Application.systemLanguage == SystemLanguage.Chinese && m_cnTextCenter.Count > 0)
		{
			return m_cnTextCenter;
		}
		return m_enTextCenter;
	}

	private void ParserXml(string xmlContent, Dictionary<string, string> center)
	{
		if (xmlContent.Length == 0)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlContent);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			center.Add(childNode.Name, xmlElement.GetAttribute("value").Trim());
		}
	}
}
