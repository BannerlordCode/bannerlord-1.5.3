using System;
using System.Collections.Generic;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000C RID: 12
	public class BrushAnimation
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003920 File Offset: 0x00001B20
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003928 File Offset: 0x00001B28
		public string Name { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003931 File Offset: 0x00001B31
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003939 File Offset: 0x00001B39
		public float Duration { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003942 File Offset: 0x00001B42
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000394A File Offset: 0x00001B4A
		public bool Loop { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003953 File Offset: 0x00001B53
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000395B File Offset: 0x00001B5B
		public AnimationInterpolation.Type InterpolationType { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003964 File Offset: 0x00001B64
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000396C File Offset: 0x00001B6C
		public AnimationInterpolation.Function InterpolationFunction { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003975 File Offset: 0x00001B75
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000397D File Offset: 0x00001B7D
		public BrushLayerAnimation StyleAnimation { get; set; }

		// Token: 0x060000C9 RID: 201 RVA: 0x00003986 File Offset: 0x00001B86
		public BrushAnimation()
		{
			this._data = new Dictionary<string, BrushLayerAnimation>();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000399C File Offset: 0x00001B9C
		public void AddAnimationProperty(BrushAnimationProperty property)
		{
			BrushLayerAnimation brushLayerAnimation = null;
			if (string.IsNullOrEmpty(property.LayerName))
			{
				if (this.StyleAnimation == null)
				{
					this.StyleAnimation = new BrushLayerAnimation();
				}
				brushLayerAnimation = this.StyleAnimation;
			}
			else if (!this._data.TryGetValue(property.LayerName, out brushLayerAnimation))
			{
				brushLayerAnimation = new BrushLayerAnimation();
				brushLayerAnimation.LayerName = property.LayerName;
				this._data.Add(property.LayerName, brushLayerAnimation);
			}
			brushLayerAnimation.AddAnimationProperty(property);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003A14 File Offset: 0x00001C14
		public void RemoveAnimationProperty(BrushAnimationProperty property)
		{
			BrushLayerAnimation brushLayerAnimation;
			if (string.IsNullOrEmpty(property.LayerName))
			{
				if (this.StyleAnimation == null)
				{
					this.StyleAnimation = new BrushLayerAnimation();
				}
				brushLayerAnimation = this.StyleAnimation;
			}
			else
			{
				if (!this._data.ContainsKey(property.LayerName))
				{
					return;
				}
				brushLayerAnimation = this._data[property.LayerName];
			}
			brushLayerAnimation.RemoveAnimationProperty(property);
			if (brushLayerAnimation.Collections.Count == 0)
			{
				this._data.Remove(property.LayerName);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003A98 File Offset: 0x00001C98
		public void FillFrom(BrushAnimation animation)
		{
			this.Name = animation.Name;
			this.Duration = animation.Duration;
			this.Loop = animation.Loop;
			this.InterpolationType = animation.InterpolationType;
			this.InterpolationFunction = animation.InterpolationFunction;
			if (animation.StyleAnimation != null)
			{
				this.StyleAnimation = animation.StyleAnimation.Clone();
			}
			this._data = new Dictionary<string, BrushLayerAnimation>();
			foreach (KeyValuePair<string, BrushLayerAnimation> keyValuePair in animation._data)
			{
				string key = keyValuePair.Key;
				BrushLayerAnimation brushLayerAnimation = keyValuePair.Value.Clone();
				this._data.Add(key, brushLayerAnimation);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003B68 File Offset: 0x00001D68
		public BrushLayerAnimation GetLayerAnimation(string name)
		{
			if (this._data.ContainsKey(name))
			{
				return this._data[name];
			}
			return null;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003B86 File Offset: 0x00001D86
		public IEnumerable<BrushLayerAnimation> GetLayerAnimations()
		{
			return this._data.Values;
		}

		// Token: 0x04000035 RID: 53
		private Dictionary<string, BrushLayerAnimation> _data;
	}
}
