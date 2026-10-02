using System;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000D RID: 13
	public class BrushAnimationKeyFrame
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003B93 File Offset: 0x00001D93
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00003B9B File Offset: 0x00001D9B
		public float Time { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003BA4 File Offset: 0x00001DA4
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003BAC File Offset: 0x00001DAC
		public int Index { get; private set; }

		// Token: 0x060000D4 RID: 212 RVA: 0x00003BBD File Offset: 0x00001DBD
		public void InitializeAsFloat(float time, float value)
		{
			this.Time = time;
			this._valueType = BrushAnimationKeyFrame.ValueType.Float;
			this._valueAsFloat = value;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public void InitializeAsColor(float time, Color value)
		{
			this.Time = time;
			this._valueType = BrushAnimationKeyFrame.ValueType.Color;
			this._valueAsColor = value;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003BEB File Offset: 0x00001DEB
		public void InitializeAsSprite(float time, Sprite value)
		{
			this.Time = time;
			this._valueType = BrushAnimationKeyFrame.ValueType.Sprite;
			this._valueAsSprite = value;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003C02 File Offset: 0x00001E02
		public void InitializeIndex(int index)
		{
			this.Index = index;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003C0B File Offset: 0x00001E0B
		public float GetValueAsFloat()
		{
			return this._valueAsFloat;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003C13 File Offset: 0x00001E13
		public Color GetValueAsColor()
		{
			return this._valueAsColor;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003C1B File Offset: 0x00001E1B
		public Sprite GetValueAsSprite()
		{
			return this._valueAsSprite;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003C24 File Offset: 0x00001E24
		public object GetValueAsObject()
		{
			switch (this._valueType)
			{
			case BrushAnimationKeyFrame.ValueType.Float:
				return this._valueAsFloat;
			case BrushAnimationKeyFrame.ValueType.Color:
				return this._valueAsColor;
			case BrushAnimationKeyFrame.ValueType.Sprite:
				return this._valueAsSprite;
			default:
				return null;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003C6C File Offset: 0x00001E6C
		public BrushAnimationKeyFrame Clone()
		{
			return new BrushAnimationKeyFrame
			{
				_valueType = this._valueType,
				_valueAsFloat = this._valueAsFloat,
				_valueAsColor = this._valueAsColor,
				_valueAsSprite = this._valueAsSprite,
				Time = this.Time,
				Index = this.Index
			};
		}

		// Token: 0x04000037 RID: 55
		private BrushAnimationKeyFrame.ValueType _valueType;

		// Token: 0x04000038 RID: 56
		private float _valueAsFloat;

		// Token: 0x04000039 RID: 57
		private Color _valueAsColor;

		// Token: 0x0400003A RID: 58
		private Sprite _valueAsSprite;

		// Token: 0x02000076 RID: 118
		public enum ValueType
		{
			// Token: 0x040003FC RID: 1020
			Float,
			// Token: 0x040003FD RID: 1021
			Color,
			// Token: 0x040003FE RID: 1022
			Sprite
		}
	}
}
