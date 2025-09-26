using UnityEngine;

public class LogCallBack : MonoBehaviour
{
	private string[] output = new string[10];

	private int index;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnGUI()
	{
		if (Screen.height != 768 || Screen.width != 1024)
		{
			GUILayout.BeginArea(new Rect(1f, 10f, Screen.width, (float)Screen.height * 0.9f));
			GUI.contentColor = Color.red;
			for (int i = 0; i < output.Length; i++)
			{
				GUILayout.Label(output[i]);
			}
			GUILayout.EndArea();
		}
	}

	private void OnEnable()
	{
		if (Screen.height != 768 || Screen.width != 1024)
		{
			Application.RegisterLogCallback(HandleLog);
		}
	}

	private void OnDisable()
	{
		if (Screen.height != 768 || Screen.width != 1024)
		{
			Application.RegisterLogCallback(null);
		}
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		logString = logString.Replace('\n', ' ');
		if (index >= output.Length)
		{
			for (int i = 1; i < output.Length; i++)
			{
				output[i - 1] = output[i];
			}
			output[output.Length - 1] = logString;
		}
		else
		{
			output[index] = logString;
			index++;
		}
	}
}
