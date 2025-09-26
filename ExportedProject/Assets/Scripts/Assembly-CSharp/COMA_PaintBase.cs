using UnityEngine;

public class COMA_PaintBase
{
	private static COMA_PaintBase _instance;

	public string defaultFileName = COMA_FileNameManager.Instance.GetFileName("PaintColor");

	public Color curPaint = Color.black;

	public Color[] bakPaint = new Color[6]
	{
		Color.red,
		Color.green,
		Color.blue,
		Color.red,
		Color.green,
		Color.blue
	};

	private char sepSign = 'X';

	public static COMA_PaintBase Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new COMA_PaintBase();
			}
			return _instance;
		}
	}

	public string content
	{
		get
		{
			string text = curPaint.r.ToString() + sepSign + curPaint.g.ToString() + sepSign + curPaint.b.ToString();
			for (int i = 0; i < bakPaint.Length; i++)
			{
				string text2 = text;
				text = text2 + sepSign + bakPaint[i].r.ToString() + sepSign + bakPaint[i].g.ToString() + sepSign + bakPaint[i].b.ToString();
			}
			return text;
		}
		set
		{
			if (value != null && !(value == string.Empty))
			{
				string[] array = value.Split(sepSign);
				int num = 0;
				curPaint.r = float.Parse(array[num++]);
				curPaint.g = float.Parse(array[num++]);
				curPaint.b = float.Parse(array[num++]);
				for (int i = 0; i < bakPaint.Length; i++)
				{
					bakPaint[i].r = float.Parse(array[num++]);
					bakPaint[i].g = float.Parse(array[num++]);
					bakPaint[i].b = float.Parse(array[num++]);
				}
			}
		}
	}

	public static void ResetInstance()
	{
		_instance = null;
	}
}
