using UnityEngine;

public class ShowFPS : MonoBehaviour
{
	private float m_fps;

	public void Awake()
	{
		m_fps = 0f;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Update()
	{
		float num = 1f / Time.deltaTime;
		m_fps = m_fps * 0.4f + num * 0.6f;
	}

	public void OnGUI()
	{
		GUI.Label(new Rect(10f, 5f, 60f, 30f), "fps:" + (int)m_fps);
	}
}
