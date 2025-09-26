using UnityEngine;

public class GUI_Menu_Paint : MonoBehaviour
{
	private BodyPaint bodyPaint;

	private int celWidth = 20;

	private int celHeight = 20;

	private Texture2D[] texs;

	private void Start()
	{
		bodyPaint = GetComponent<BodyPaint>();
		texs = new Texture2D[COMA_Color.colors.Length];
		for (int i = 0; i < texs.Length; i++)
		{
			texs[i] = new Texture2D(celWidth, celHeight, TextureFormat.RGB24, false);
			Color[] array = new Color[celWidth * celHeight];
			for (int j = 0; j < celWidth * celHeight; j++)
			{
				array[j] = COMA_Color.colors[i];
			}
			texs[i].SetPixels(array);
			texs[i].Apply(false);
		}
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10 * GUI_Menu_Show.retina, 10 * GUI_Menu_Show.retina, 50 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "Back"))
		{
		}
		if (bodyPaint.GetBackStepCount() > 1 && GUI.Button(new Rect(Screen.width - 80 * GUI_Menu_Show.retina, 20 * GUI_Menu_Show.retina, 70 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "Last Step"))
		{
			Debug.Log("Last Step");
			bodyPaint.Undo();
		}
		if (GUI.Button(new Rect(Screen.width - 80 * GUI_Menu_Show.retina, 80 * GUI_Menu_Show.retina, 70 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "Full"))
		{
			Debug.Log("Full");
			bodyPaint.Full();
		}
		if (GUI.Button(new Rect(10 * GUI_Menu_Show.retina, Screen.height - 220 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "B1"))
		{
			Debug.Log("Brush1");
			bodyPaint.paintRadius = 1;
		}
		else if (GUI.Button(new Rect(10 * GUI_Menu_Show.retina, Screen.height - 180 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "B2"))
		{
			Debug.Log("Brush2");
			bodyPaint.paintRadius = 2;
		}
		else if (GUI.Button(new Rect(10 * GUI_Menu_Show.retina, Screen.height - 140 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "B3"))
		{
			Debug.Log("Brush3");
			bodyPaint.paintRadius = 3;
		}
		else if (GUI.Button(new Rect(10 * GUI_Menu_Show.retina, Screen.height - 100 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina, 30 * GUI_Menu_Show.retina), "B4"))
		{
			Debug.Log("Brush4");
			bodyPaint.paintRadius = 4;
		}
	}
}
