using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPaint : MonoBehaviour
{
	[NonSerialized]
	public Collider paintTarget;

	[NonSerialized]
	public Texture2D tex2D;

	private Color[] color2D;

	private int texWidth = 1;

	private int texHeight = 1;

	private List<Color[]> colorList = new List<Color[]>();

	private int clrSteps = 16;

	[NonSerialized]
	public int paintRadius = 1;

	[NonSerialized]
	public Color paintColor = Color.white;

	[NonSerialized]
	public bool isNoise;

	private Vector2 lastTexPos = Vector2.zero;

	public void StartInit()
	{
		texWidth = tex2D.width;
		texHeight = tex2D.height;
		color2D = tex2D.GetPixels();
		colorList.Add(color2D);
	}

	public void Undo()
	{
		Debug.Log(colorList.Count);
		if (colorList.Count > 1)
		{
			colorList.RemoveAt(colorList.Count - 1);
			color2D = colorList[colorList.Count - 1].Clone() as Color[];
			tex2D.SetPixels(color2D);
			tex2D.Apply(false);
		}
	}

	public int GetBackStepCount()
	{
		return colorList.Count;
	}

	public void Full()
	{
		color2D = colorList[colorList.Count - 1].Clone() as Color[];
		DrawARect(color2D, texWidth, texHeight, paintColor, Vector2.zero, texWidth + texHeight);
		tex2D.SetPixels(color2D);
		tex2D.Apply(false);
		PaintEnd();
	}

	public Color GetPixColor(Vector2 textureCoord)
	{
		Vector2 vector = new Vector2(textureCoord.x * (float)texWidth, textureCoord.y * (float)texHeight);
		int num = Mathf.FloorToInt(vector.x);
		int num2 = Mathf.FloorToInt(vector.y);
		return color2D[num2 * texWidth + num];
	}

	public void PaintStart(Vector2 textureCoord)
	{
		Vector2 centerPos = new Vector2(textureCoord.x * (float)texWidth, textureCoord.y * (float)texHeight);
		color2D = colorList[colorList.Count - 1].Clone() as Color[];
		DrawARect(color2D, texWidth, texHeight, paintColor, centerPos, paintRadius);
		tex2D.SetPixels(color2D);
		tex2D.Apply(false);
		lastTexPos = centerPos;
	}

	public void Paint(Vector2 textureCoord)
	{
		Vector2 vector = new Vector2(textureCoord.x * (float)texWidth, textureCoord.y * (float)texHeight);
		float magnitude = (vector - lastTexPos).magnitude;
		if (magnitude > 0.01f && magnitude < (float)texWidth * 0.125f)
		{
			for (int i = 0; (float)i <= magnitude; i += paintRadius)
			{
				Vector2 centerPos = Vector2.Lerp(lastTexPos, vector, (float)i / magnitude);
				DrawARect(color2D, texWidth, texHeight, paintColor, centerPos, paintRadius);
			}
			tex2D.SetPixels(color2D);
			tex2D.Apply(false);
		}
		lastTexPos = vector;
	}

	public void PaintEnd()
	{
		colorList.Add(color2D);
		if (colorList.Count > clrSteps)
		{
			colorList.RemoveAt(0);
		}
	}

	private void DrawARect(Color[] color2D, int texWidth, int texHeight, Color curClr, Vector2 centerPos, int paintRadius)
	{
		DrawARect(color2D, texWidth, texHeight, curClr, centerPos.x, centerPos.y, paintRadius);
	}

	private void DrawARect(Color[] color2D, int texWidth, int texHeight, Color curClr, float centerPosX, float centerPosY, int paintRadius)
	{
		int centerPosX2 = Mathf.FloorToInt(centerPosX);
		int centerPosY2 = Mathf.FloorToInt(centerPosY);
		DrawARect(color2D, texWidth, texHeight, curClr, centerPosX2, centerPosY2, paintRadius);
	}

	private void DrawARect(Color[] color2D, int texWidth, int texHeight, Color curClr, int centerPosX, int centerPosY, int paintRadius)
	{
		int num = centerPosX - paintRadius + 1;
		int num2 = centerPosY - paintRadius + 1;
		int num3 = centerPosX + paintRadius;
		int num4 = centerPosY + paintRadius;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num3 > texWidth)
		{
			num3 = texWidth;
		}
		if (num4 > texHeight)
		{
			num4 = texHeight;
		}
		if (isNoise)
		{
			for (int i = num2; i < num4; i++)
			{
				for (int j = num; j < num3; j++)
				{
					float num5 = UnityEngine.Random.Range(-0.015f, 0.015f);
					color2D[i * texWidth + j] += new Color(num5, num5, num5, 0f);
					color2D[i * texWidth + j].a = 1f;
				}
			}
			return;
		}
		for (int k = num2; k < num4; k++)
		{
			for (int l = num; l < num3; l++)
			{
				color2D[k * texWidth + l] = curClr;
			}
		}
	}
}
