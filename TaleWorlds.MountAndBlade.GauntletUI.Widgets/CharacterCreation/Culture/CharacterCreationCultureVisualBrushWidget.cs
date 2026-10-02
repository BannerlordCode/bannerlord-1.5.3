using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Culture
{
	// Token: 0x0200018D RID: 397
	public class CharacterCreationCultureVisualBrushWidget : BrushWidget
	{
		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x000387F2 File Offset: 0x000369F2
		// (set) Token: 0x060014B2 RID: 5298 RVA: 0x000387FA File Offset: 0x000369FA
		public bool UseSmallVisuals { get; set; } = true;

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x00038803 File Offset: 0x00036A03
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x0003880B File Offset: 0x00036A0B
		public ParallaxItemBrushWidget Layer1Widget { get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00038814 File Offset: 0x00036A14
		// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0003881C File Offset: 0x00036A1C
		public ParallaxItemBrushWidget Layer2Widget { get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x00038825 File Offset: 0x00036A25
		// (set) Token: 0x060014B8 RID: 5304 RVA: 0x0003882D File Offset: 0x00036A2D
		public ParallaxItemBrushWidget Layer3Widget { get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x00038836 File Offset: 0x00036A36
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x0003883E File Offset: 0x00036A3E
		public ParallaxItemBrushWidget Layer4Widget { get; set; }

		// Token: 0x060014BB RID: 5307 RVA: 0x00038847 File Offset: 0x00036A47
		public CharacterCreationCultureVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00038860 File Offset: 0x00036A60
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isFirstFrame)
			{
				this._alphaTarget = (float)(string.IsNullOrEmpty(this.CurrentCultureId) ? 0 : 1);
				this.SetGlobalAlphaRecursively(this._alphaTarget);
				ParallaxItemBrushWidget layer1Widget = this.Layer1Widget;
				if (layer1Widget != null)
				{
					layer1Widget.RegisterBrushStatesOfWidget();
				}
				ParallaxItemBrushWidget layer2Widget = this.Layer2Widget;
				if (layer2Widget != null)
				{
					layer2Widget.RegisterBrushStatesOfWidget();
				}
				ParallaxItemBrushWidget layer3Widget = this.Layer3Widget;
				if (layer3Widget != null)
				{
					layer3Widget.RegisterBrushStatesOfWidget();
				}
				ParallaxItemBrushWidget layer4Widget = this.Layer4Widget;
				if (layer4Widget != null)
				{
					layer4Widget.RegisterBrushStatesOfWidget();
				}
				this._isFirstFrame = false;
			}
			this.SetGlobalAlphaRecursively(Mathf.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, this._alphaTarget, dt * 10f));
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00038910 File Offset: 0x00036B10
		private void SetCultureVisual(string newCultureId)
		{
			if (string.IsNullOrEmpty(newCultureId))
			{
				this._alphaTarget = 0f;
				return;
			}
			if (this.UseSmallVisuals)
			{
				Sprite sprite = base.Context.SpriteData.GetSprite("CharacterCreation\\Culture\\" + newCultureId);
				if (sprite == null)
				{
					sprite = base.Context.SpriteData.GetSprite("CharacterCreation\\Culture\\blank_culture");
				}
				using (Dictionary<string, Style>.ValueCollection.Enumerator enumerator = base.Brush.Styles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Style style = enumerator.Current;
						StyleLayer[] layers = style.GetLayers();
						for (int i = 0; i < layers.Length; i++)
						{
							layers[i].Sprite = sprite;
						}
					}
					goto IL_00EC;
				}
			}
			ParallaxItemBrushWidget layer1Widget = this.Layer1Widget;
			if (layer1Widget != null)
			{
				layer1Widget.SetState(newCultureId);
			}
			ParallaxItemBrushWidget layer2Widget = this.Layer2Widget;
			if (layer2Widget != null)
			{
				layer2Widget.SetState(newCultureId);
			}
			ParallaxItemBrushWidget layer3Widget = this.Layer3Widget;
			if (layer3Widget != null)
			{
				layer3Widget.SetState(newCultureId);
			}
			ParallaxItemBrushWidget layer4Widget = this.Layer4Widget;
			if (layer4Widget != null)
			{
				layer4Widget.SetState(newCultureId);
			}
			IL_00EC:
			this._alphaTarget = 1f;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x00038A24 File Offset: 0x00036C24
		// (set) Token: 0x060014BF RID: 5311 RVA: 0x00038A2C File Offset: 0x00036C2C
		[Editor(false)]
		public string CurrentCultureId
		{
			get
			{
				return this._currentCultureId;
			}
			set
			{
				if (this._currentCultureId != value)
				{
					this._currentCultureId = value;
					base.OnPropertyChanged<string>(value, "CurrentCultureId");
					this.SetCultureVisual(value);
					this.SetGlobalAlphaRecursively(1f);
				}
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x00038A61 File Offset: 0x00036C61
		// (set) Token: 0x060014C1 RID: 5313 RVA: 0x00038A69 File Offset: 0x00036C69
		[Editor(false)]
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					this._isBig = value;
					base.OnPropertyChanged(value, "IsBig");
				}
			}
		}

		// Token: 0x04000971 RID: 2417
		private float _alphaTarget;

		// Token: 0x04000972 RID: 2418
		private bool _isFirstFrame = true;

		// Token: 0x04000973 RID: 2419
		private string _currentCultureId;

		// Token: 0x04000974 RID: 2420
		private bool _isBig;
	}
}
