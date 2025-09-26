using UnityEngine;

[RequireComponent(typeof(TUIDrawSprite))]
[ExecuteInEditMode]
public class UI_InputLabel : TUILabel
{
	public float GetLineWidth()
	{
		if (multiline)
		{
			return -1f;
		}
		float num = 0f;
		float num2 = 0f;
		int length = base.Text.Length;
		for (int i = 0; i < length - 1; i++)
		{
			num += (float)(fontHD.mSpacingX + fontHD.bmFont.GetGlyph(base.Text[i]).advance);
		}
		num2 = ((length != 0) ? (num + (float)fontHD.bmFont.GetGlyph(base.Text[length - 1]).width) : 0f);
		return num2 * 0.5f;
	}
}
