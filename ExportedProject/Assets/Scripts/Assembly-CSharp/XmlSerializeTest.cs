using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlSerializeTest : MonoBehaviour
{
	public class Test
	{
		public class Feel
		{
			public string aa = "a12";

			public string bb = "b12";
		}

		public string Name = "djzhu";

		public Feel _feel = new Feel();
	}

	private void Start()
	{
		Test pObject = new Test();
		string message = SerializeObject<Test>(pObject);
		Debug.Log(message);
		string pXmlizedString = "\ufeff<?xml version=\"1.0\" encoding=\"utf-8\"?><Test><Name>d123</Name><_feel aa=\"a34\" bb=\"b34\"/></Test>";
		Test test = DeserializeObject<Test>(pXmlizedString) as Test;
		Debug.Log(test.Name + " " + test._feel.aa + " " + test._feel.bb);
	}

	public string SerializeObject<T>(object pObject)
	{
		string empty = string.Empty;
		MemoryStream stream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		xmlSerializer.Serialize(xmlTextWriter, pObject);
		stream = (MemoryStream)xmlTextWriter.BaseStream;
		return Encoding.UTF8.GetString(stream.ToArray());
	}

	public object DeserializeObject<T>(string pXmlizedString)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(pXmlizedString));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		return xmlSerializer.Deserialize(stream);
	}
}
