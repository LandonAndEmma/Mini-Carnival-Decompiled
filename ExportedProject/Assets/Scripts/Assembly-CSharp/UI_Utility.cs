using System;
using UnityEngine;

[ExecuteInEditMode]
public class UI_Utility : MonoBehaviour
{
	[SerializeField]
	private float _h;

	[SerializeField]
	private float _s;

	[SerializeField]
	private float _l;

	[SerializeField]
	private bool _bTest;

	private void Start()
	{
	}

	private void Update()
	{
		if (_bTest)
		{
			Color color = HSL2RGB(_h, _s, _l);
			Debug.Log(color);
			_bTest = false;
		}
	}

	public static Color HSL2RGB(float h, float sl, float l)
	{
		float r = l;
		float g = l;
		float b = l;
		float num = ((!(l <= 0.5f)) ? (l + sl - l * sl) : (l * (1f + sl)));
		if (num > 0f)
		{
			float num2 = l + l - num;
			float num3 = (num - num2) / num;
			h *= 6f;
			int num4 = (int)h;
			float num5 = h - (float)num4;
			float num6 = num * num3 * num5;
			float num7 = num2 + num6;
			float num8 = num - num6;
			switch (num4)
			{
			case 0:
				r = num;
				g = num7;
				b = num2;
				break;
			case 1:
				r = num8;
				g = num;
				b = num2;
				break;
			case 2:
				r = num2;
				g = num;
				b = num7;
				break;
			case 3:
				r = num2;
				g = num8;
				b = num;
				break;
			case 4:
				r = num7;
				g = num2;
				b = num;
				break;
			case 5:
				r = num;
				g = num2;
				b = num8;
				break;
			}
		}
		return new Color(r, g, b);
	}

	public static void RGB2HSL(Color rgb, out float h, out float s, out float l)
	{
		float r = rgb.r;
		float g = rgb.g;
		float b = rgb.b;
		h = 0f;
		s = 0f;
		l = 0f;
		float val = Math.Max(r, g);
		val = Math.Max(val, b);
		float val2 = Math.Min(r, g);
		val2 = Math.Min(val2, b);
		l = (val2 + val) / 2f;
		if (l <= 0f)
		{
			return;
		}
		float num = (s = val - val2);
		if ((double)s > 0.0)
		{
			s /= ((!(l <= 0.5f)) ? (2f - val - val2) : (val + val2));
			float num2 = (val - r) / num;
			float num3 = (val - g) / num;
			float num4 = (val - b) / num;
			if (r == val)
			{
				h = ((g != val2) ? (1f - num3) : (5f + num4));
			}
			else if (g == val)
			{
				h = ((b != val2) ? (3f - num4) : (1f + num2));
			}
			else
			{
				h = ((r != val2) ? (5f - num2) : (3f + num3));
			}
			h /= 6f;
		}
	}
}
