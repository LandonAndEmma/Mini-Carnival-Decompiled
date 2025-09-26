using UnityEngine;

public class UIDoodle_ColourBoxSlot : UI_BoxSlot
{
	[SerializeField]
	private TUIMeshSprite[] meshSprites;

	private float fVolume;

	private GameObject preSelColour;

	public float Volume
	{
		get
		{
			return fVolume;
		}
		set
		{
			fVolume = value;
			Transform parent = base.gameObject.transform.parent;
			if (!(parent != null))
			{
				return;
			}
			Transform parent2 = parent.parent;
			if (!(parent2 != null))
			{
				return;
			}
			Transform parent3 = parent2.parent;
			if (parent3 != null)
			{
				UIDoodle_ColourVolume volumeIndicate = parent3.GetComponent<UIDoodle_ColourContainer>().volumeIndicate;
				if (volumeIndicate != null)
				{
					volumeIndicate.RefreshVolume(fVolume);
				}
			}
		}
	}

	private void Start()
	{
		preSelColour = null;
	}

	private new void Update()
	{
	}

	public void SetBoxColorAndVolume(Color color, float f)
	{
		TUIMeshSprite[] array = meshSprites;
		foreach (TUIMeshSprite tUIMeshSprite in array)
		{
			tUIMeshSprite.color = color;
		}
		Volume = f;
		Debug.Log("Set Color-" + color);
	}

	public void HandleEventButton_Colour(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		switch (eventType)
		{
		case 3:
		{
			UIDoodle uIDoodle = (UIDoodle)GetTUIMessageHandler(true);
			if (uIDoodle != null)
			{
				uIDoodle.ProcessColourChanged(control, eventType, wparam, lparam, data, this);
			}
			break;
		}
		case 1:
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Click);
			break;
		}
	}
}
