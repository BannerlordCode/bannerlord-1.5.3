using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x0200014C RID: 332
	public class TooltipPropertyWidget : Widget
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x00030BAD File Offset: 0x0002EDAD
		// (set) Token: 0x060011A5 RID: 4517 RVA: 0x00030BB5 File Offset: 0x0002EDB5
		public bool IsTwoColumn { get; private set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00030BBE File Offset: 0x0002EDBE
		// (set) Token: 0x060011A7 RID: 4519 RVA: 0x00030BC6 File Offset: 0x0002EDC6
		public TooltipPropertyWidget.TooltipPropertyFlags PropertyModifierAsFlag { get; private set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x00030BCF File Offset: 0x0002EDCF
		private bool _allBrushesInitialized
		{
			get
			{
				return this.SubtextBrush != null && this.ValueTextBrush != null && this.DescriptionTextBrush != null && this.ValueNameTextBrush != null && this.RundownSeperatorSprite != null && this.DefaultSeperatorSprite != null && this.TitleBackgroundSprite != null;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00030C0C File Offset: 0x0002EE0C
		public bool IsMultiLine
		{
			get
			{
				return this._isMultiLine;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x00030C14 File Offset: 0x0002EE14
		public bool IsBattleMode
		{
			get
			{
				return this._isBattleMode;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x00030C1C File Offset: 0x0002EE1C
		public bool IsBattleModeOver
		{
			get
			{
				return this._isBattleModeOver;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00030C24 File Offset: 0x0002EE24
		public bool IsCost
		{
			get
			{
				return this._isCost;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x00030C2C File Offset: 0x0002EE2C
		public bool IsRelation
		{
			get
			{
				return this._isRelation;
			}
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00030C34 File Offset: 0x0002EE34
		public TooltipPropertyWidget(UIContext context)
			: base(context)
		{
			this._isMultiLine = false;
			this._isBattleMode = false;
			this._isBattleModeOver = false;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00030C60 File Offset: 0x0002EE60
		public void SetBattleScope(bool battleScope)
		{
			if (battleScope)
			{
				this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Center;
				this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Center;
				return;
			}
			this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Right;
			this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Left;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00030C98 File Offset: 0x0002EE98
		public void RefreshSize(bool inBattleScope, float battleScopeSize, float maxValueLabelSizeX, float maxDefinitionLabelSizeX, Brush definitionRelationBrush = null, Brush valueRelationBrush = null)
		{
			if (this._isMultiLine || this._isSubtext)
			{
				this.DefinitionLabelContainer.IsVisible = false;
				this.DefinitionLabelContainer.ScaledSuggestedWidth = 0f;
				this.ValueLabel.WidthSizePolicy = SizePolicy.Fixed;
				this.ValueLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
				this.ValueLabel.ScaledSuggestedWidth = base.ParentWidget.Size.X - (base.ScaledMarginLeft + base.ScaledMarginRight);
				this.ValueLabelContainer.ScaledSuggestedWidth = base.ParentWidget.Size.X - (base.ScaledMarginLeft + base.ScaledMarginRight);
			}
			else if (inBattleScope)
			{
				this.DefinitionLabelContainer.ScaledSuggestedWidth = battleScopeSize;
				this.DefinitionLabel.Brush = definitionRelationBrush;
				this.ValueLabelContainer.ScaledSuggestedWidth = battleScopeSize;
				this.ValueLabel.Brush = valueRelationBrush;
				this.ValueLabelContainer.HorizontalAlignment = HorizontalAlignment.Left;
				this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Left;
				this.ValueLabel.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
			}
			else if (!this.IsTwoColumn)
			{
				if (!string.IsNullOrEmpty(this.DefinitionLabel.Text))
				{
					float num = ((this.DefinitionLabel.Size.X > this.ValueLabel.Size.X) ? this.DefinitionLabel.Size.X : this.ValueLabel.Size.X);
					this.DefinitionLabelContainer.ScaledSuggestedWidth = num;
					this.ValueLabelContainer.ScaledSuggestedWidth = num;
				}
				else
				{
					this.DefinitionLabelContainer.ScaledSuggestedWidth = 0f;
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabelContainer.ScaledSuggestedWidth = this.ValueLabel.Size.X;
				}
			}
			if (this.IsTwoColumn && !this._isMultiLine && (!this._isTitle || (this._isTitle && this.IsTwoColumn)))
			{
				this.ValueLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
				this.ValueLabelContainer.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxValueLabelSizeX);
				if (this.ItemModifierLabel != null && this.ItemModifierLabel.IsVisible)
				{
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.MaxWidth = MathF.Max(53f, maxValueLabelSizeX * base._inverseScaleToUse) - this.ItemModifierLabel.Size.X * base._inverseScaleToUse - (this.ItemModifierLabel.MarginLeft + this.ItemModifierLabel.MarginRight);
				}
				else
				{
					this.ValueLabel.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueLabel.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxValueLabelSizeX);
				}
			}
			if (this.IsTwoColumn && !this._isMultiLine && this._isTitle)
			{
				this.DefinitionLabel.WidthSizePolicy = SizePolicy.Fixed;
				this.DefinitionLabel.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxDefinitionLabelSizeX);
				this.DefinitionLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
				this.DefinitionLabelContainer.ScaledSuggestedWidth = MathF.Max(53f * base._scaleToUse, maxDefinitionLabelSizeX);
			}
			Widget parentWidget = base.ParentWidget;
			this.SetGlobalAlphaRecursively((parentWidget != null) ? parentWidget.AlphaFactor : 0f);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00030FD7 File Offset: 0x0002F1D7
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._firstFrame)
			{
				this.RefreshText();
				this._firstFrame = false;
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00030FF8 File Offset: 0x0002F1F8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.CoverChildren;
			if (this._currentSprite != null)
			{
				if (this.DefinitionLabelContainer.Size.X + this.ValueLabelContainer.Size.X > base.ParentWidget.Size.X)
				{
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.ScaledSuggestedWidth = this.DefinitionLabelContainer.Size.X + this.ValueLabelContainer.Size.X;
					base.MarginLeft = 0f;
					base.MarginRight = 0f;
				}
				else
				{
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.ScaledSuggestedWidth = base.ParentWidget.Size.X - (base.MarginLeft + base.MarginRight) * base._scaleToUse;
				}
				this.ValueBackgroundSpriteWidget.MinHeight = (float)this._currentSprite.Height;
				if (this._isTitle)
				{
					base.PositionXOffset = -base.MarginLeft;
					this.ValueLabelContainer.PositionYOffset = 0f;
					if (!this.IsTwoColumn)
					{
						this.ValueLabelContainer.MarginLeft = base.MarginLeft;
						this.ValueBackgroundSpriteWidget.ScaledSuggestedHeight = this.ValueLabel.Size.Y;
						return;
					}
					this.DefinitionLabelContainer.MarginLeft = base.MarginLeft;
					this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Left;
					return;
				}
			}
			else
			{
				this.ValueBackgroundSpriteWidget.SuggestedWidth = 0f;
			}
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00031184 File Offset: 0x0002F384
		private void RefreshText()
		{
			this.DefinitionLabel.Text = this._definitionText;
			this.ValueLabel.Text = this._valueText;
			this.DetermineTypeOfTooltipProperty();
			this.ValueLabelContainer.IsVisible = true;
			this.DefinitionLabelContainer.IsVisible = true;
			this.DefinitionLabel.IsVisible = true;
			this.ValueLabel.IsVisible = true;
			this._currentSprite = null;
			if (this._allBrushesInitialized)
			{
				if (this._isRelation)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = "";
				}
				else if (this._isBattleMode)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = "";
				}
				else if (this._isBattleModeOver)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = "";
				}
				else if (this._isMultiLine)
				{
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabel.Text = this._valueText;
					this.ValueLabel.Brush = ((this.TextHeight < 0) ? this.SubtextBrush : this.DescriptionTextBrush);
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.Fixed;
					this.ValueLabelContainer.SuggestedWidth = 0f;
				}
				else if (this._isCost)
				{
					this.DefinitionLabel.Text = "";
					this.ValueLabel.Text = this._valueText;
					base.HorizontalAlignment = HorizontalAlignment.Center;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				else if (this._isRundownSeperator)
				{
					this.ValueLabel.IsVisible = false;
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueBackgroundSpriteWidget.IsVisible = true;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this._currentSprite = this.RundownSeperatorSprite;
					this.ValueBackgroundSpriteWidget.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueBackgroundSpriteWidget.PositionXOffset = base.Right * base._inverseScaleToUse;
					this.ValueBackgroundSpriteWidget.Sprite = this._currentSprite;
					this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
				}
				else if (this._isDefaultSeperator)
				{
					this.ValueLabel.IsVisible = false;
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueBackgroundSpriteWidget.IsVisible = true;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this._currentSprite = this.DefaultSeperatorSprite;
					this.ValueBackgroundSpriteWidget.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueBackgroundSpriteWidget.PositionXOffset = base.Right * base._inverseScaleToUse;
					this.ValueBackgroundSpriteWidget.Sprite = this._currentSprite;
					this.ValueBackgroundSpriteWidget.AlphaFactor = 0.5f;
					this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.Fixed;
					this.ValueBackgroundSpriteWidget.WidthSizePolicy = SizePolicy.Fixed;
				}
				else if (this._isTitle)
				{
					this.DefinitionLabel.Brush = this.TitleTextBrush;
					this.ValueLabel.Brush = this.TitleTextBrush;
					this.DefinitionLabel.HeightSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.HeightSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabelContainer.HeightSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.HeightSizePolicy = SizePolicy.CoverChildren;
					if (this.IsTwoColumn)
					{
						this.DefinitionLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
						this.DefinitionLabelContainer.HorizontalAlignment = HorizontalAlignment.Left;
						this.DefinitionLabel.WidthSizePolicy = SizePolicy.CoverChildren;
						this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Left;
						this.DefinitionLabel.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
						this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
						this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
						this.ValueLabel.MarginLeft = base.MarginLeft;
					}
					else
					{
						this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
						this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					}
					this._currentSprite = this.TitleBackgroundSprite;
					this.ValueBackgroundSpriteWidget.HeightSizePolicy = SizePolicy.CoverChildren;
					this.ValueBackgroundSpriteWidget.Sprite = this._currentSprite;
					this.ValueBackgroundSpriteWidget.IsVisible = true;
				}
				else if (this.IsTwoColumn)
				{
					this.DefinitionLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabelContainer.HorizontalAlignment = HorizontalAlignment.Right;
					this.DefinitionLabel.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					base.HorizontalAlignment = HorizontalAlignment.Right;
					this.ValueLabel.MarginLeft = base.MarginLeft;
					this.DefinitionLabel.Brush = this.ValueNameTextBrush;
					this.ValueLabel.Brush = this.ValueTextBrush;
				}
				else if (this._isSubtext)
				{
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabel.Brush = this.SubtextBrush;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				else if (this._isEmptySpace)
				{
					this.DefinitionLabel.IsVisible = false;
					this.ValueLabel.Text = " ";
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					if (this.TextHeight > 0)
					{
						this.ValueLabel.Brush.FontSize = 30;
					}
					else if (this.TextHeight < 0)
					{
						this.ValueLabel.Brush.FontSize = 10;
					}
					else
					{
						this.ValueLabel.Brush.FontSize = 15;
					}
				}
				else if (this.DefinitionLabel.Text == string.Empty && this.ValueLabel.Text != string.Empty)
				{
					this.DefinitionLabelContainer.IsVisible = false;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabel.Brush = this.DescriptionTextBrush;
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.ValueLabelContainer.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				else
				{
					this.ValueLabel.WidthSizePolicy = SizePolicy.CoverChildren;
					this.DefinitionLabel.WidthSizePolicy = SizePolicy.CoverChildren;
				}
				if (this._useCustomColor)
				{
					this.ValueLabel.Brush.FontColor = this.TextColor;
					this.ValueLabel.Brush.TextAlphaFactor = this.TextColor.Alpha;
				}
				if (this._isRundownResult)
				{
					this.ValueLabel.Brush.FontSize = (int)((float)this.ValueLabel.ReadOnlyBrush.FontSize * 1.3f);
					this.DefinitionLabel.Brush.FontSize = (int)((float)this.DefinitionLabel.ReadOnlyBrush.FontSize * 1.3f);
				}
			}
			Widget parentWidget = base.ParentWidget;
			this.SetGlobalAlphaRecursively((parentWidget != null) ? parentWidget.AlphaFactor : 0f);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00031854 File Offset: 0x0002FA54
		private void DetermineTypeOfTooltipProperty()
		{
			this.PropertyModifierAsFlag = (TooltipPropertyWidget.TooltipPropertyFlags)this.PropertyModifier;
			this._isMultiLine = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.MultiLine) == TooltipPropertyWidget.TooltipPropertyFlags.MultiLine;
			this._isBattleMode = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.BattleMode) == TooltipPropertyWidget.TooltipPropertyFlags.BattleMode;
			this._isBattleModeOver = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.BattleModeOver) == TooltipPropertyWidget.TooltipPropertyFlags.BattleModeOver;
			this._isCost = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.Cost) == TooltipPropertyWidget.TooltipPropertyFlags.Cost;
			this._isTitle = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.Title) == TooltipPropertyWidget.TooltipPropertyFlags.Title;
			this._isRelation = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstNeutral) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstNeutral || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly || (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondNeutral) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondNeutral;
			this._isRundownSeperator = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.RundownSeperator) == TooltipPropertyWidget.TooltipPropertyFlags.RundownSeperator;
			this._isDefaultSeperator = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.DefaultSeperator) == TooltipPropertyWidget.TooltipPropertyFlags.DefaultSeperator;
			this._isRundownResult = (this.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.RundownResult) == TooltipPropertyWidget.TooltipPropertyFlags.RundownResult;
			this.IsTwoColumn = false;
			this._isSubtext = false;
			this._isEmptySpace = false;
			if (!this._isMultiLine && !this._isBattleMode && !this._isBattleModeOver && !this._isCost && !this._isRundownSeperator && !this._isDefaultSeperator)
			{
				this._isEmptySpace = this.DefinitionText == string.Empty && this.ValueText == string.Empty;
				this.IsTwoColumn = this.DefinitionText != string.Empty && this.ValueText != string.Empty && this.TextHeight == 0;
				this._isSubtext = this.DefinitionText == string.Empty && this.ValueText != string.Empty && this.TextHeight < 0;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00031A6C File Offset: 0x0002FC6C
		// (set) Token: 0x060011B6 RID: 4534 RVA: 0x00031A74 File Offset: 0x0002FC74
		[Editor(false)]
		public string RundownSeperatorSpriteName
		{
			get
			{
				return this._rundownSeperatorSpriteName;
			}
			set
			{
				if (this._rundownSeperatorSpriteName != value)
				{
					this._rundownSeperatorSpriteName = value;
					base.OnPropertyChanged<string>(value, "RundownSeperatorSpriteName");
					this.RundownSeperatorSprite = base.Context.SpriteData.GetSprite(value);
				}
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00031AAE File Offset: 0x0002FCAE
		// (set) Token: 0x060011B8 RID: 4536 RVA: 0x00031AB6 File Offset: 0x0002FCB6
		[Editor(false)]
		public string DefaultSeperatorSpriteName
		{
			get
			{
				return this._defaultSeperatorSpriteName;
			}
			set
			{
				if (this._defaultSeperatorSpriteName != value)
				{
					this._defaultSeperatorSpriteName = value;
					base.OnPropertyChanged<string>(value, "DefaultSeperatorSpriteName");
					this.DefaultSeperatorSprite = base.Context.SpriteData.GetSprite(value);
				}
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00031AF0 File Offset: 0x0002FCF0
		// (set) Token: 0x060011BA RID: 4538 RVA: 0x00031AF8 File Offset: 0x0002FCF8
		[Editor(false)]
		public string TitleBackgroundSpriteName
		{
			get
			{
				return this._titleBackgroundSpriteName;
			}
			set
			{
				if (this._titleBackgroundSpriteName != value)
				{
					this._titleBackgroundSpriteName = value;
					base.OnPropertyChanged<string>(value, "TitleBackgroundSpriteName");
					this.TitleBackgroundSprite = base.Context.SpriteData.GetSprite(value);
				}
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00031B32 File Offset: 0x0002FD32
		// (set) Token: 0x060011BC RID: 4540 RVA: 0x00031B3A File Offset: 0x0002FD3A
		[Editor(false)]
		public Brush ValueNameTextBrush
		{
			get
			{
				return this._valueNameTextBrush;
			}
			set
			{
				if (this._valueNameTextBrush != value)
				{
					this._valueNameTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "ValueNameTextBrush");
				}
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x00031B58 File Offset: 0x0002FD58
		// (set) Token: 0x060011BE RID: 4542 RVA: 0x00031B60 File Offset: 0x0002FD60
		[Editor(false)]
		public Brush TitleTextBrush
		{
			get
			{
				return this._titleTextBrush;
			}
			set
			{
				if (this._titleTextBrush != value)
				{
					this._titleTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "TitleTextBrush");
				}
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x00031B7E File Offset: 0x0002FD7E
		// (set) Token: 0x060011C0 RID: 4544 RVA: 0x00031B86 File Offset: 0x0002FD86
		[Editor(false)]
		public Brush SubtextBrush
		{
			get
			{
				return this._subtextBrush;
			}
			set
			{
				if (this._subtextBrush != value)
				{
					this._subtextBrush = value;
					base.OnPropertyChanged<Brush>(value, "SubtextBrush");
				}
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00031BA4 File Offset: 0x0002FDA4
		// (set) Token: 0x060011C2 RID: 4546 RVA: 0x00031BAC File Offset: 0x0002FDAC
		[Editor(false)]
		public Brush ValueTextBrush
		{
			get
			{
				return this._valueTextBrush;
			}
			set
			{
				if (this._valueTextBrush != value)
				{
					this._valueTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "ValueTextBrush");
				}
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00031BCA File Offset: 0x0002FDCA
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x00031BD2 File Offset: 0x0002FDD2
		[Editor(false)]
		public Brush DescriptionTextBrush
		{
			get
			{
				return this._descriptionTextBrush;
			}
			set
			{
				if (this._descriptionTextBrush != value)
				{
					this._descriptionTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "DescriptionTextBrush");
				}
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00031BF0 File Offset: 0x0002FDF0
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00031BF8 File Offset: 0x0002FDF8
		[Editor(false)]
		public bool ModifyDefinitionColor
		{
			get
			{
				return this._modifyDefinitionColor;
			}
			set
			{
				if (this._modifyDefinitionColor != value)
				{
					this._modifyDefinitionColor = value;
					base.OnPropertyChanged(value, "ModifyDefinitionColor");
				}
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00031C16 File Offset: 0x0002FE16
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x00031C1E File Offset: 0x0002FE1E
		[Editor(false)]
		public RichTextWidget DefinitionLabel
		{
			get
			{
				return this._definitionLabel;
			}
			set
			{
				if (this._definitionLabel != value)
				{
					this._definitionLabel = value;
					base.OnPropertyChanged<RichTextWidget>(value, "DefinitionLabel");
				}
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00031C3C File Offset: 0x0002FE3C
		// (set) Token: 0x060011CA RID: 4554 RVA: 0x00031C44 File Offset: 0x0002FE44
		[Editor(false)]
		public RichTextWidget ValueLabel
		{
			get
			{
				return this._valueLabel;
			}
			set
			{
				if (this._valueLabel != value)
				{
					this._valueLabel = value;
					base.OnPropertyChanged<RichTextWidget>(value, "ValueLabel");
				}
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x00031C62 File Offset: 0x0002FE62
		// (set) Token: 0x060011CC RID: 4556 RVA: 0x00031C6A File Offset: 0x0002FE6A
		[Editor(false)]
		public TextWidget ItemModifierLabel
		{
			get
			{
				return this._itemModifierLabel;
			}
			set
			{
				if (this._itemModifierLabel != value)
				{
					this._itemModifierLabel = value;
					base.OnPropertyChanged<TextWidget>(value, "ItemModifierLabel");
				}
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00031C88 File Offset: 0x0002FE88
		// (set) Token: 0x060011CE RID: 4558 RVA: 0x00031C90 File Offset: 0x0002FE90
		[Editor(false)]
		public ListPanel ValueBackgroundSpriteWidget
		{
			get
			{
				return this._valueBackgroundSpriteWidget;
			}
			set
			{
				if (this._valueBackgroundSpriteWidget != value)
				{
					this._valueBackgroundSpriteWidget = value;
					base.OnPropertyChanged<ListPanel>(value, "ValueBackgroundSpriteWidget");
				}
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00031CAE File Offset: 0x0002FEAE
		// (set) Token: 0x060011D0 RID: 4560 RVA: 0x00031CB6 File Offset: 0x0002FEB6
		[Editor(false)]
		public Widget DefinitionLabelContainer
		{
			get
			{
				return this._definitionLabelContainer;
			}
			set
			{
				if (this._definitionLabelContainer != value)
				{
					this._definitionLabelContainer = value;
					base.OnPropertyChanged<Widget>(value, "DefinitionLabelContainer");
				}
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00031CD4 File Offset: 0x0002FED4
		// (set) Token: 0x060011D2 RID: 4562 RVA: 0x00031CDC File Offset: 0x0002FEDC
		[Editor(false)]
		public Widget ValueLabelContainer
		{
			get
			{
				return this._valueLabelContainer;
			}
			set
			{
				if (this._valueLabelContainer != value)
				{
					this._valueLabelContainer = value;
					base.OnPropertyChanged<Widget>(value, "ValueLabelContainer");
				}
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00031CFA File Offset: 0x0002FEFA
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00031D02 File Offset: 0x0002FF02
		[Editor(false)]
		public Color TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				if (this._textColor != value)
				{
					this._textColor = value;
					base.OnPropertyChanged(value, "TextColor");
					this._useCustomColor = true;
				}
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00031D2C File Offset: 0x0002FF2C
		// (set) Token: 0x060011D6 RID: 4566 RVA: 0x00031D34 File Offset: 0x0002FF34
		[Editor(false)]
		public int TextHeight
		{
			get
			{
				return this._textHeight;
			}
			set
			{
				if (this._textHeight != value)
				{
					this._textHeight = value;
					base.OnPropertyChanged(value, "TextHeight");
				}
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00031D52 File Offset: 0x0002FF52
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x00031D5A File Offset: 0x0002FF5A
		[Editor(false)]
		public string DefinitionText
		{
			get
			{
				return this._definitionText;
			}
			set
			{
				if (this._definitionText != value)
				{
					this._definitionText = value;
					base.OnPropertyChanged<string>(value, "DefinitionText");
					this._firstFrame = true;
				}
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00031D84 File Offset: 0x0002FF84
		// (set) Token: 0x060011DA RID: 4570 RVA: 0x00031D8C File Offset: 0x0002FF8C
		[Editor(false)]
		public string ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (this._valueText != value)
				{
					this._valueText = value;
					base.OnPropertyChanged<string>(value, "ValueText");
					this._firstFrame = true;
				}
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00031DB6 File Offset: 0x0002FFB6
		// (set) Token: 0x060011DC RID: 4572 RVA: 0x00031DBE File Offset: 0x0002FFBE
		[Editor(false)]
		public int PropertyModifier
		{
			get
			{
				return this._propertyModifier;
			}
			set
			{
				if (this._propertyModifier != value)
				{
					this._propertyModifier = value;
					base.OnPropertyChanged(value, "PropertyModifier");
				}
			}
		}

		// Token: 0x04000809 RID: 2057
		private const int HeaderSize = 30;

		// Token: 0x0400080A RID: 2058
		private const int DefaultSize = 15;

		// Token: 0x0400080B RID: 2059
		private const int SubTextSize = 10;

		// Token: 0x0400080D RID: 2061
		private bool _isMultiLine;

		// Token: 0x0400080E RID: 2062
		private bool _isBattleMode;

		// Token: 0x0400080F RID: 2063
		private bool _isBattleModeOver;

		// Token: 0x04000810 RID: 2064
		private bool _isCost;

		// Token: 0x04000811 RID: 2065
		private bool _isRundownSeperator;

		// Token: 0x04000812 RID: 2066
		private bool _isDefaultSeperator;

		// Token: 0x04000813 RID: 2067
		private bool _isRundownResult;

		// Token: 0x04000814 RID: 2068
		private bool _isTitle;

		// Token: 0x04000815 RID: 2069
		private bool _isSubtext;

		// Token: 0x04000816 RID: 2070
		private bool _isEmptySpace;

		// Token: 0x04000817 RID: 2071
		private bool _isRelation;

		// Token: 0x04000819 RID: 2073
		private bool _useCustomColor;

		// Token: 0x0400081A RID: 2074
		private Sprite RundownSeperatorSprite;

		// Token: 0x0400081B RID: 2075
		private Sprite DefaultSeperatorSprite;

		// Token: 0x0400081C RID: 2076
		private Sprite TitleBackgroundSprite;

		// Token: 0x0400081D RID: 2077
		private Sprite _currentSprite;

		// Token: 0x0400081E RID: 2078
		private bool _firstFrame = true;

		// Token: 0x0400081F RID: 2079
		private bool _modifyDefinitionColor = true;

		// Token: 0x04000820 RID: 2080
		private Color _textColor;

		// Token: 0x04000821 RID: 2081
		private RichTextWidget _definitionLabel;

		// Token: 0x04000822 RID: 2082
		private RichTextWidget _valueLabel;

		// Token: 0x04000823 RID: 2083
		private TextWidget _itemModifierLabel;

		// Token: 0x04000824 RID: 2084
		private Widget _definitionLabelContainer;

		// Token: 0x04000825 RID: 2085
		private Widget _valueLabelContainer;

		// Token: 0x04000826 RID: 2086
		private ListPanel _valueBackgroundSpriteWidget;

		// Token: 0x04000827 RID: 2087
		private int _textHeight;

		// Token: 0x04000828 RID: 2088
		private Brush _titleTextBrush;

		// Token: 0x04000829 RID: 2089
		private Brush _subtextBrush;

		// Token: 0x0400082A RID: 2090
		private Brush _valueTextBrush;

		// Token: 0x0400082B RID: 2091
		private Brush _descriptionTextBrush;

		// Token: 0x0400082C RID: 2092
		private Brush _valueNameTextBrush;

		// Token: 0x0400082D RID: 2093
		private string _rundownSeperatorSpriteName;

		// Token: 0x0400082E RID: 2094
		private string _defaultSeperatorSpriteName;

		// Token: 0x0400082F RID: 2095
		private string _titleBackgroundSpriteName;

		// Token: 0x04000830 RID: 2096
		private string _definitionText;

		// Token: 0x04000831 RID: 2097
		private string _valueText;

		// Token: 0x04000832 RID: 2098
		private int _propertyModifier;

		// Token: 0x020001D5 RID: 469
		[Flags]
		public enum TooltipPropertyFlags
		{
			// Token: 0x04000A6E RID: 2670
			None = 0,
			// Token: 0x04000A6F RID: 2671
			MultiLine = 1,
			// Token: 0x04000A70 RID: 2672
			BattleMode = 2,
			// Token: 0x04000A71 RID: 2673
			BattleModeOver = 4,
			// Token: 0x04000A72 RID: 2674
			WarFirstEnemy = 8,
			// Token: 0x04000A73 RID: 2675
			WarFirstAlly = 16,
			// Token: 0x04000A74 RID: 2676
			WarFirstNeutral = 32,
			// Token: 0x04000A75 RID: 2677
			WarSecondEnemy = 64,
			// Token: 0x04000A76 RID: 2678
			WarSecondAlly = 128,
			// Token: 0x04000A77 RID: 2679
			WarSecondNeutral = 256,
			// Token: 0x04000A78 RID: 2680
			RundownSeperator = 512,
			// Token: 0x04000A79 RID: 2681
			DefaultSeperator = 1024,
			// Token: 0x04000A7A RID: 2682
			Cost = 2048,
			// Token: 0x04000A7B RID: 2683
			Title = 4096,
			// Token: 0x04000A7C RID: 2684
			RundownResult = 8192
		}
	}
}
