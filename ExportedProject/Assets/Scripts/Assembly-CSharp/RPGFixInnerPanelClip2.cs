using UnityEngine;

public class RPGFixInnerPanelClip2 : MonoBehaviour
{
	[SerializeField]
	private UIDragPanelContents _dragPanelContents;

	[SerializeField]
	private UIContainer_ButtonBox _container_box;

	[SerializeField]
	private bool _clipX = true;

	private void Start()
	{
	}

	private void Update()
	{
		UIDraggablePanel draggablePanel = _dragPanelContents.draggablePanel;
		if (!(draggablePanel != null) || !(_container_box != null) || !(_container_box.OwnerBox != null))
		{
			return;
		}
		Vector4 clipRange = GetComponent<UIPanel>().clipRange;
		if (!(_container_box.transform.parent == null) && !(_container_box.transform.parent.parent == null))
		{
			Vector3 vector = _container_box.transform.parent.transform.localPosition + _container_box.transform.parent.parent.transform.localPosition;
			Vector4 clipRange2 = draggablePanel.GetComponent<UIPanel>().clipRange;
			clipRange.w = clipRange2.w;
			clipRange.z = clipRange2.z;
			if (_clipX)
			{
				clipRange.x = 0f - vector.x - draggablePanel.transform.localPosition.x;
				clipRange.y = 0f - vector.y;
			}
			else
			{
				clipRange.x = 0f - vector.x;
				clipRange.y = 0f - vector.y - draggablePanel.transform.localPosition.y;
			}
			GetComponent<UIPanel>().clipRange = clipRange;
		}
	}
}
