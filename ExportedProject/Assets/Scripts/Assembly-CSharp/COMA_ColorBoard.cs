using UnityEngine;

public class COMA_ColorBoard : MonoBehaviour
{
	public Color[] colorsInput = new Color[32];

	private void Start()
	{
		string text = string.Empty;
		Color[] array = colorsInput;
		for (int i = 0; i < array.Length; i++)
		{
			Color color = array[i];
			string text2 = text;
			text = text2 + "new Color(" + color.r.ToString("f2") + "f, " + color.g.ToString("f2") + "f, " + color.b.ToString("f2") + "f),\n";
		}
		Debug.Log(text);
	}
}
