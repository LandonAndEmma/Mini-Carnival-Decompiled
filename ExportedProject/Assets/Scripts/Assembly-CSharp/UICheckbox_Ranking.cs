using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox")]
public class UICheckbox_Ranking : MonoBehaviour
{
	public delegate void OnStateChange(bool state);

	public static UICheckbox_Ranking current;

	public UISprite checkSprite;

	public Animation checkAnimation;

	public bool instantTween;

	public bool startsChecked = true;

	public Transform radioButtonRoot;

	public bool optionCanBeNone;

	public GameObject eventReceiver;

	public string functionName = "OnActivate";

	public OnStateChange onStateChange;

	[SerializeField]
	[HideInInspector]
	private bool option;

	private bool mChecked = true;

	private bool mStarted;

	private Transform mTrans;

	public bool isChecked
	{
		get
		{
			return mChecked;
		}
		set
		{
			if (radioButtonRoot == null || value || optionCanBeNone || !mStarted)
			{
				Set(value);
			}
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		if (checkSprite != null)
		{
			checkSprite.alpha = ((!startsChecked) ? 0f : 1f);
		}
		if (option)
		{
			option = false;
			if (radioButtonRoot == null)
			{
				radioButtonRoot = mTrans.parent;
			}
		}
	}

	private void Start()
	{
		if (eventReceiver == null)
		{
			eventReceiver = base.gameObject;
		}
		mChecked = !startsChecked;
		mStarted = true;
		Set(startsChecked);
	}

	private void OnClick()
	{
		if (base.enabled)
		{
			isChecked = !isChecked;
		}
		if (isChecked)
		{
			COMA_AudioManager.Instance.SoundPlay(AudioCategory.UI_Select);
		}
	}

	private void Set(bool state)
	{
		if (!mStarted)
		{
			mChecked = state;
			startsChecked = state;
			if (checkSprite != null)
			{
				checkSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else
		{
			if (mChecked == state)
			{
				return;
			}
			if (radioButtonRoot != null && state)
			{
				UICheckbox_Ranking[] componentsInChildren = radioButtonRoot.GetComponentsInChildren<UICheckbox_Ranking>(true);
				int i = 0;
				for (int num = componentsInChildren.Length; i < num; i++)
				{
					UICheckbox_Ranking uICheckbox_Ranking = componentsInChildren[i];
					if (uICheckbox_Ranking != this && uICheckbox_Ranking.radioButtonRoot == radioButtonRoot)
					{
						uICheckbox_Ranking.Set(false);
					}
				}
			}
			mChecked = state;
			if (checkSprite != null)
			{
				if (instantTween)
				{
					checkSprite.alpha = ((!mChecked) ? 0f : 1f);
					checkSprite.gameObject.SetActive(mChecked);
				}
				else
				{
					TweenAlpha.Begin(checkSprite.gameObject, 0.15f, (!mChecked) ? 0f : 1f);
				}
			}
			current = this;
			if (onStateChange != null)
			{
				onStateChange(mChecked);
			}
			if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
			{
				eventReceiver.SendMessage(functionName, mChecked, SendMessageOptions.DontRequireReceiver);
			}
			current = null;
			if (checkAnimation != null)
			{
				ActiveAnimation.Play(checkAnimation, state ? Direction.Forward : Direction.Reverse);
			}
		}
	}
}
