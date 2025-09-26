using UnityEngine;

public class RPGFixInnerPanelClip : MonoBehaviour
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
		if (draggablePanel != null && _container_box != null && _container_box.OwnerBox != null)
		{
			Vector4 clipRange = GetComponent<UIPanel>().clipRange;
			if (_clipX)
			{
				float x = draggablePanel.transform.localPosition.x;
				clipRange.x = 0f - _container_box.OwnerBox.transform.localPosition.x - x;
			}
			else
			{
				float y = draggablePanel.transform.localPosition.y;
				clipRange.y = 0f - _container_box.OwnerBox.transform.localPosition.y - y;
			}
			GetComponent<UIPanel>().clipRange = clipRange;
		}
	}
}
