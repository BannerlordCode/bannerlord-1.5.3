using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000006 RID: 6
	public class BannerTableauWidget : TextureWidget
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000217C File Offset: 0x0000037C
		public BannerTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "BannerTableauTextureProvider";
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002190 File Offset: 0x00000390
		protected override void OnMousePressed()
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002192 File Offset: 0x00000392
		protected override void OnMouseReleased(bool isFromInput)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002194 File Offset: 0x00000394
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021A0 File Offset: 0x000003A0
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			this._isRenderRequestedPreviousFrame = true;
			if (base.TextureProvider == null)
			{
				return;
			}
			base.Texture = base.TextureProvider.GetTextureForRender(twoDimensionContext, null);
			Texture texture = base.Texture;
			if (texture == null || !texture.IsValid)
			{
				return;
			}
			SimpleMaterial simpleMaterial = drawContext.CreateSimpleMaterial();
			Brush readOnlyBrush = base.ReadOnlyBrush;
			StyleLayer styleLayer;
			if (readOnlyBrush == null)
			{
				styleLayer = null;
			}
			else
			{
				StyleLayer[] layers = readOnlyBrush.GetStyleOrDefault(base.CurrentState).GetLayers();
				styleLayer = ((layers != null) ? layers.FirstOrDefault<StyleLayer>() : null);
			}
			StyleLayer styleLayer2 = styleLayer ?? null;
			simpleMaterial.OverlayEnabled = false;
			simpleMaterial.CircularMaskingEnabled = false;
			simpleMaterial.Texture = base.Texture;
			simpleMaterial.NinePatchParameters = SpriteNinePatchParameters.Empty;
			simpleMaterial.AlphaFactor = ((styleLayer2 != null) ? styleLayer2.AlphaFactor : 1f) * base.ReadOnlyBrush.GlobalAlphaFactor * base.Context.ContextAlpha;
			simpleMaterial.ColorFactor = ((styleLayer2 != null) ? styleLayer2.ColorFactor : 1f) * base.ReadOnlyBrush.GlobalColorFactor;
			simpleMaterial.HueFactor = ((styleLayer2 != null) ? styleLayer2.HueFactor : 0f);
			simpleMaterial.SaturationFactor = ((styleLayer2 != null) ? styleLayer2.SaturationFactor : 0f);
			simpleMaterial.ValueFactor = ((styleLayer2 != null) ? styleLayer2.ValueFactor : 0f);
			simpleMaterial.Color = ((styleLayer2 != null) ? styleLayer2.Color : Color.White) * base.ReadOnlyBrush.GlobalColor;
			ImageDrawObject imageDrawObject = ImageDrawObject.Create(in this.AreaRect, in Vec2.Zero, in Vec2.One);
			imageDrawObject.Scale = base._scaleToUse;
			if (drawContext.CircularMaskEnabled)
			{
				simpleMaterial.CircularMaskingEnabled = true;
				simpleMaterial.CircularMaskingCenter = drawContext.CircularMaskCenter;
				simpleMaterial.CircularMaskingRadius = drawContext.CircularMaskRadius;
				simpleMaterial.CircularMaskingSmoothingRadius = drawContext.CircularMaskSmoothingRadius;
			}
			drawContext.Draw(simpleMaterial, in imageDrawObject);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000235A File Offset: 0x0000055A
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002362 File Offset: 0x00000562
		[Editor(false)]
		public string BannerCodeText
		{
			get
			{
				return this._bannerCode;
			}
			set
			{
				if (value != this._bannerCode)
				{
					this._bannerCode = value;
					base.OnPropertyChanged<string>(value, "BannerCodeText");
					base.SetTextureProviderProperty("BannerCodeText", value);
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002391 File Offset: 0x00000591
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002399 File Offset: 0x00000599
		[Editor(false)]
		public float CustomRenderScale
		{
			get
			{
				return this._customRenderScale;
			}
			set
			{
				if (value != this._customRenderScale)
				{
					this._customRenderScale = value;
					base.OnPropertyChanged(value, "CustomRenderScale");
					base.SetTextureProviderProperty("CustomRenderScale", value);
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000023C8 File Offset: 0x000005C8
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000023D0 File Offset: 0x000005D0
		[Editor(false)]
		public bool IsNineGrid
		{
			get
			{
				return this._isNineGrid;
			}
			set
			{
				if (value != this._isNineGrid)
				{
					this._isNineGrid = value;
					base.OnPropertyChanged(value, "IsNineGrid");
					base.SetTextureProviderProperty("IsNineGrid", value);
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000023FF File Offset: 0x000005FF
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002407 File Offset: 0x00000607
		[Editor(false)]
		public Vec2 UpdatePositionValueManual
		{
			get
			{
				return this._updatePositionRef;
			}
			set
			{
				if (value != this._updatePositionRef)
				{
					this._updatePositionRef = value;
					base.OnPropertyChanged(value, "UpdatePositionValueManual");
					base.SetTextureProviderProperty("UpdatePositionValueManual", value);
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000243B File Offset: 0x0000063B
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002443 File Offset: 0x00000643
		[Editor(false)]
		public Vec2 UpdateSizeValueManual
		{
			get
			{
				return this._updateSizeRef;
			}
			set
			{
				if (value != this._updateSizeRef)
				{
					this._updateSizeRef = value;
					base.OnPropertyChanged(value, "UpdateSizeValueManual");
					base.SetTextureProviderProperty("UpdateSizeValueManual", value);
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002477 File Offset: 0x00000677
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002480 File Offset: 0x00000680
		[Editor(false)]
		public ValueTuple<float, bool> UpdateRotationValueManualWithMirror
		{
			get
			{
				return this._updateRotationWithMirrorRef;
			}
			set
			{
				if (value.Item1 != this._updateRotationWithMirrorRef.Item1 || value.Item2 != this._updateRotationWithMirrorRef.Item2)
				{
					this._updateRotationWithMirrorRef = value;
					base.OnPropertyChanged<string>("UpdateRotationValueManualWithMirror", "UpdateRotationValueManualWithMirror");
					base.SetTextureProviderProperty("UpdateRotationValueManualWithMirror", value);
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024DB File Offset: 0x000006DB
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000024E3 File Offset: 0x000006E3
		[Editor(false)]
		public int MeshIndexToUpdate
		{
			get
			{
				return this._meshIndexToUpdate;
			}
			set
			{
				if (value != this._meshIndexToUpdate)
				{
					this._meshIndexToUpdate = value;
					base.OnPropertyChanged(value, "MeshIndexToUpdate");
					base.SetTextureProviderProperty("MeshIndexToUpdate", value);
				}
			}
		}

		// Token: 0x04000003 RID: 3
		private string _bannerCode;

		// Token: 0x04000004 RID: 4
		private float _customRenderScale;

		// Token: 0x04000005 RID: 5
		private bool _isNineGrid;

		// Token: 0x04000006 RID: 6
		private Vec2 _updatePositionRef;

		// Token: 0x04000007 RID: 7
		private Vec2 _updateSizeRef;

		// Token: 0x04000008 RID: 8
		private ValueTuple<float, bool> _updateRotationWithMirrorRef;

		// Token: 0x04000009 RID: 9
		private int _meshIndexToUpdate;
	}
}
