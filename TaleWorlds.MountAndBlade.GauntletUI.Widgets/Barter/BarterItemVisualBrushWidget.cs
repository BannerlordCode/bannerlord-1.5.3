using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x02000193 RID: 403
	public class BarterItemVisualBrushWidget : BrushWidget
	{
		// Token: 0x060014E1 RID: 5345 RVA: 0x00038EC7 File Offset: 0x000370C7
		public BarterItemVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00038EDC File Offset: 0x000370DC
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			if (!this._imageDetermined)
			{
				this.RegisterStatesOfWidgetFromBrush(this.SpriteWidget);
				this.UpdateVisual();
				this._imageDetermined = true;
			}
			if (this._imageDetermined && this.Type == "fief_barterable")
			{
				this.SpriteClipWidget.ClipContents = true;
				this.SpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
				this.SpriteWidget.HeightSizePolicy = SizePolicy.Fixed;
				this.SpriteWidget.ScaledSuggestedHeight = this.SpriteClipWidget.Size.X;
				this.SpriteWidget.ScaledSuggestedWidth = this.SpriteClipWidget.Size.X;
				this.SpriteWidget.PositionYOffset = 18f;
				this.SpriteWidget.VerticalAlignment = VerticalAlignment.Center;
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00038FA4 File Offset: 0x000371A4
		private void RegisterStatesOfWidgetFromBrush(BrushWidget widget)
		{
			if (widget != null)
			{
				foreach (BrushLayer brushLayer in widget.ReadOnlyBrush.Layers)
				{
					widget.AddState(brushLayer.Name);
				}
			}
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00039004 File Offset: 0x00037204
		private void UpdateVisual()
		{
			Sprite sprite = null;
			this.SpriteWidget.IsVisible = false;
			this.MaskedTextureWidget.IsVisible = false;
			this.ImageIdentifierWidget.IsVisible = false;
			string type = this.Type;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(type);
			if (num <= 1654682144U)
			{
				if (num <= 403518212U)
				{
					if (num != 189982571U)
					{
						if (num != 284088421U)
						{
							if (num != 403518212U)
							{
								goto IL_02C1;
							}
							if (!(type == "fief_barterable"))
							{
								goto IL_02C1;
							}
							sprite = base.EventManager.Context.SpriteData.GetSprite(this.FiefImagePath + "_t");
							this.SpriteWidget.Brush = base.EventManager.Context.DefaultBrush;
							this.SpriteWidget.IsVisible = true;
							goto IL_02CD;
						}
						else
						{
							if (!(type == "lift_siege_barterable"))
							{
								goto IL_02C1;
							}
							goto IL_02C1;
						}
					}
					else if (!(type == "join_faction_barterable"))
					{
						goto IL_02C1;
					}
				}
				else if (num <= 806661062U)
				{
					if (num != 535704800U)
					{
						if (num != 806661062U)
						{
							goto IL_02C1;
						}
						if (!(type == "war_barterable"))
						{
							goto IL_02C1;
						}
						goto IL_02C1;
					}
					else
					{
						if (!(type == "no_attack_barterable"))
						{
							goto IL_02C1;
						}
						goto IL_02C1;
					}
				}
				else if (num != 1289251258U)
				{
					if (num != 1654682144U)
					{
						goto IL_02C1;
					}
					if (!(type == "start_siege_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02C1;
				}
				else
				{
					if (!(type == "marriage_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02B3;
				}
			}
			else if (num <= 2639715379U)
			{
				if (num <= 2166136261U)
				{
					if (num != 2080743372U)
					{
						if (num != 2166136261U)
						{
							goto IL_02C1;
						}
						if (type != null && type.Length != 0)
						{
							goto IL_02C1;
						}
						goto IL_02C1;
					}
					else if (!(type == "leave_faction_barterable"))
					{
						goto IL_02C1;
					}
				}
				else if (num != 2342284176U)
				{
					if (num != 2639715379U)
					{
						goto IL_02C1;
					}
					if (!(type == "item_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02B3;
				}
				else
				{
					if (!(type == "set_prisoner_free_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02B3;
				}
			}
			else if (num <= 3787227692U)
			{
				if (num != 3249789840U)
				{
					if (num != 3787227692U)
					{
						goto IL_02C1;
					}
					if (!(type == "safe_passage_barterable"))
					{
						goto IL_02C1;
					}
					goto IL_02C1;
				}
				else if (!(type == "mercenary_join_faction_barterable"))
				{
					goto IL_02C1;
				}
			}
			else if (num != 3835993774U)
			{
				if (num != 3957684540U)
				{
					goto IL_02C1;
				}
				if (!(type == "gold_barterable"))
				{
					goto IL_02C1;
				}
				goto IL_02C1;
			}
			else
			{
				if (!(type == "peace_barterable"))
				{
					goto IL_02C1;
				}
				goto IL_02C1;
			}
			this.MaskedTextureWidget.IsVisible = true;
			goto IL_02CD;
			IL_02B3:
			this.ImageIdentifierWidget.IsVisible = true;
			goto IL_02CD;
			IL_02C1:
			this.SpriteWidget.IsVisible = true;
			IL_02CD:
			if (this.SpriteWidget.ContainsState(this.Type))
			{
				this.SpriteWidget.SetState(this.Type);
			}
			if (sprite != null)
			{
				this.SetWidgetSpriteForAllStyles(this.SpriteWidget, sprite);
			}
			this.SpriteClipWidget.IsVisible = this.SpriteWidget.IsVisible;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00039328 File Offset: 0x00037528
		private void SetWidgetSpriteForAllStyles(BrushWidget widget, Sprite sprite)
		{
			widget.Sprite = sprite;
			foreach (Style style in widget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite;
				}
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0003939C File Offset: 0x0003759C
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x000393A4 File Offset: 0x000375A4
		[Editor(false)]
		public BrushWidget SpriteWidget
		{
			get
			{
				return this._spriteWidget;
			}
			set
			{
				if (this._spriteWidget != value)
				{
					this._spriteWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "SpriteWidget");
				}
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x000393C2 File Offset: 0x000375C2
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x000393CA File Offset: 0x000375CA
		[Editor(false)]
		public Widget SpriteClipWidget
		{
			get
			{
				return this._spriteClipWidget;
			}
			set
			{
				if (this._spriteClipWidget != value)
				{
					this._spriteClipWidget = value;
					base.OnPropertyChanged<Widget>(value, "SpriteClipWidget");
				}
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x000393E8 File Offset: 0x000375E8
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x000393F0 File Offset: 0x000375F0
		[Editor(false)]
		public ImageIdentifierWidget ImageIdentifierWidget
		{
			get
			{
				return this._imageIdentifierWidget;
			}
			set
			{
				if (this._imageIdentifierWidget != value)
				{
					this._imageIdentifierWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "ImageIdentifierWidget");
				}
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0003940E File Offset: 0x0003760E
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x00039416 File Offset: 0x00037616
		[Editor(false)]
		public MaskedTextureWidget MaskedTextureWidget
		{
			get
			{
				return this._maskedTextureWidget;
			}
			set
			{
				if (this._maskedTextureWidget != value)
				{
					this._maskedTextureWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "MaskedTextureWidget");
				}
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x00039434 File Offset: 0x00037634
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x0003943C File Offset: 0x0003763C
		[Editor(false)]
		public bool HasVisualIdentifier
		{
			get
			{
				return this._hasVisualIdentifier;
			}
			set
			{
				if (this._hasVisualIdentifier != value)
				{
					this._hasVisualIdentifier = value;
					base.OnPropertyChanged(value, "HasVisualIdentifier");
				}
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0003945A File Offset: 0x0003765A
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x00039462 File Offset: 0x00037662
		[Editor(false)]
		public string Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged<string>(value, "Type");
				}
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00039485 File Offset: 0x00037685
		// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0003948D File Offset: 0x0003768D
		[Editor(false)]
		public string FiefImagePath
		{
			get
			{
				return this._fiefImagePath;
			}
			set
			{
				if (this._fiefImagePath != value)
				{
					this._fiefImagePath = value;
					base.OnPropertyChanged<string>(value, "FiefImagePath");
				}
			}
		}

		// Token: 0x04000983 RID: 2435
		private bool _imageDetermined;

		// Token: 0x04000984 RID: 2436
		private string _type = "";

		// Token: 0x04000985 RID: 2437
		private string _fiefImagePath;

		// Token: 0x04000986 RID: 2438
		private bool _hasVisualIdentifier;

		// Token: 0x04000987 RID: 2439
		private BrushWidget _spriteWidget;

		// Token: 0x04000988 RID: 2440
		private MaskedTextureWidget _maskedTextureWidget;

		// Token: 0x04000989 RID: 2441
		private ImageIdentifierWidget _imageIdentifierWidget;

		// Token: 0x0400098A RID: 2442
		private Widget _spriteClipWidget;
	}
}
