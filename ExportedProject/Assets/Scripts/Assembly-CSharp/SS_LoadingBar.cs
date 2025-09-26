using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UISlider))]
public class SS_LoadingBar : MonoBehaviour
{
	private UISlider m_slider;

	[SerializeField]
	public string m_tarSceneName = string.Empty;

	private AsyncOperation m_async;

	private float m_curValue;

	[SerializeField]
	private UILabel _labelTxt;

	private void Start()
	{
		m_slider = GetComponent<UISlider>();
		StartCoroutine(LoadSceneAsync());
		m_async.allowSceneActivation = false;
	}

	private void Update()
	{
		m_curValue = Mathf.Clamp(m_curValue + Time.deltaTime, 0f, m_async.progress + 0.1f);
		if (m_async.progress > 0.89f && m_curValue >= m_async.progress + 0.099f)
		{
			Resources.UnloadUnusedAssets();
			m_async.allowSceneActivation = true;
		}
		if (m_slider != null)
		{
			m_slider.sliderValue = m_curValue;
		}
	}

	private IEnumerator LoadSceneAsync()
	{
		if (m_tarSceneName != string.Empty)
		{
			m_async = Application.LoadLevelAsync(m_tarSceneName);
		}
		else
		{
			Debug.LogError("No scene to Load!!");
		}
		yield return new WaitForEndOfFrame();
	}

	public void OnSliderChange(float val)
	{
		if (_labelTxt != null)
		{
			_labelTxt.text = Mathf.CeilToInt(val * 100f) + "%";
		}
	}
}
