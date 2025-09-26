using System.Collections.Generic;
using UnityEngine;

public class UIRPG_MyFingerEvent_NGUI : MonoBehaviour
{
	private class FingerInformation
	{
		public float startTime;

		public float endTime;

		public float startDragTime;

		public float endDragTime;

		public Vector2 startPosition;

		public Vector2 endPosition;

		public Vector2 startDragPosition;

		public Vector2 endDragPosition;
	}

	private Dictionary<int, FingerInformation> m_fingers = new Dictionary<int, FingerInformation>();

	public void OnPress(bool isDown)
	{
		Debug.Log("isDown " + isDown);
		if (isDown)
		{
			Debug.Log(UICamera.currentTouchID);
			Debug.Log(UICamera.currentTouch.pos);
			if (m_fingers.ContainsKey(UICamera.currentTouchID))
			{
				m_fingers[UICamera.currentTouchID].startPosition = UICamera.currentTouch.pos;
				m_fingers[UICamera.currentTouchID].startDragTime = Time.time;
				m_fingers[UICamera.currentTouchID].startDragPosition = UICamera.currentTouch.pos;
			}
			else
			{
				m_fingers.Add(UICamera.currentTouchID, new FingerInformation());
				m_fingers[UICamera.currentTouchID].startPosition = UICamera.currentTouch.pos;
				m_fingers[UICamera.currentTouchID].startDragTime = Time.time;
				m_fingers[UICamera.currentTouchID].startDragPosition = UICamera.currentTouch.pos;
			}
		}
		else if (m_fingers.ContainsKey(UICamera.currentTouchID))
		{
			m_fingers.Remove(UICamera.currentTouchID);
		}
	}

	public void OnDrag(Vector2 delta)
	{
		if (m_fingers.ContainsKey(UICamera.currentTouchID))
		{
			m_fingers[UICamera.currentTouchID].endDragPosition = UICamera.currentTouch.pos;
		}
		if (m_fingers.Count == 2)
		{
			int[] array = new int[2];
			int num = 0;
			foreach (int key in m_fingers.Keys)
			{
				array[num] = key;
				num++;
			}
			float magnitude = (m_fingers[array[0]].startDragPosition - m_fingers[array[1]].startDragPosition).magnitude;
			float magnitude2 = (m_fingers[array[0]].endDragPosition - m_fingers[array[1]].endDragPosition).magnitude;
			float num2 = magnitude2 - magnitude;
			SendMessage("FingerScale", num2);
		}
		else if (m_fingers.Count == 1)
		{
			SendMessage("FingerMove", delta);
		}
		if (m_fingers.ContainsKey(UICamera.currentTouchID))
		{
			m_fingers[UICamera.currentTouchID].startDragPosition = m_fingers[UICamera.currentTouchID].endDragPosition;
		}
	}
}
