using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x0200014B RID: 331
	public class PropertyBasedTooltipWidget : TooltipWidget
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x000306F8 File Offset: 0x0002E8F8
		// (set) Token: 0x0600118C RID: 4492 RVA: 0x00030700 File Offset: 0x0002E900
		public Color AllyColor { get; set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00030709 File Offset: 0x0002E909
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00030711 File Offset: 0x0002E911
		public Color EnemyColor { get; set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x0003071A File Offset: 0x0002E91A
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x00030722 File Offset: 0x0002E922
		public Color NeutralColor { get; set; }

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x0003072B File Offset: 0x0002E92B
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x00030733 File Offset: 0x0002E933
		public Widget PropertyListBackground { get; set; }

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x0003073C File Offset: 0x0002E93C
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x00030744 File Offset: 0x0002E944
		public ListPanel PropertyList { get; set; }

		// Token: 0x06001195 RID: 4501 RVA: 0x0003074D File Offset: 0x0002E94D
		public PropertyBasedTooltipWidget(UIContext context)
			: base(context)
		{
			this._animationDelayInFrames = 2;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00030764 File Offset: 0x0002E964
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateBattleScopes();
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00030774 File Offset: 0x0002E974
		private void UpdateBattleScopes()
		{
			bool flag = false;
			foreach (TooltipPropertyWidget tooltipPropertyWidget in this.PropertyWidgets)
			{
				if (tooltipPropertyWidget.IsBattleMode)
				{
					flag = true;
				}
				else if (tooltipPropertyWidget.IsBattleModeOver)
				{
					flag = false;
				}
				tooltipPropertyWidget.SetBattleScope(flag);
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000307DC File Offset: 0x0002E9DC
		private float GetBattleScopeSize()
		{
			bool flag = false;
			float num = 0f;
			if (this.PropertyList != null)
			{
				for (int i = 0; i < this.PropertyList.ChildCount; i++)
				{
					TooltipPropertyWidget tooltipPropertyWidget;
					if ((tooltipPropertyWidget = this.PropertyList.GetChild(i) as TooltipPropertyWidget) != null)
					{
						if (tooltipPropertyWidget.IsBattleMode)
						{
							flag = true;
						}
						else if (tooltipPropertyWidget.IsBattleModeOver)
						{
							flag = false;
						}
						if (flag)
						{
							float num2 = ((tooltipPropertyWidget.ValueLabel.Size.X > tooltipPropertyWidget.DefinitionLabel.Size.X) ? tooltipPropertyWidget.ValueLabel.Size.X : tooltipPropertyWidget.DefinitionLabel.Size.X);
							if (num2 > num)
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00030898 File Offset: 0x0002EA98
		private void UpdateRelationBrushes()
		{
			TooltipPropertyWidget tooltipPropertyWidget = this.PropertyWidgets.SingleOrDefault<TooltipPropertyWidget>((TooltipPropertyWidget p) => p.IsRelation);
			if (tooltipPropertyWidget != null)
			{
				if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly)
				{
					this._definitionRelationBrush = this.AllyTroopsTextBrush;
				}
				else if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy)
				{
					this._definitionRelationBrush = this.EnemyTroopsTextBrush;
				}
				else
				{
					this._definitionRelationBrush = this.NeutralTroopsTextBrush;
				}
				if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly)
				{
					this._valueRelationBrush = this.AllyTroopsTextBrush;
					return;
				}
				if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy)
				{
					this._valueRelationBrush = this.EnemyTroopsTextBrush;
					return;
				}
				this._valueRelationBrush = this.NeutralTroopsTextBrush;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x0003095C File Offset: 0x0002EB5C
		private IEnumerable<TooltipPropertyWidget> PropertyWidgets
		{
			get
			{
				if (this.PropertyList != null)
				{
					int num;
					for (int i = 0; i < this.PropertyList.ChildCount; i = num + 1)
					{
						TooltipPropertyWidget tooltipPropertyWidget;
						if ((tooltipPropertyWidget = this.PropertyList.GetChild(i) as TooltipPropertyWidget) != null)
						{
							yield return tooltipPropertyWidget;
						}
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0003096C File Offset: 0x0002EB6C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = false;
			float battleScopeSize = this.GetBattleScopeSize();
			float num = -1f;
			float num2 = -1f;
			this._definitionRelationBrush = null;
			this._valueRelationBrush = null;
			if (this.PropertyList != null)
			{
				if (this._firstFrame)
				{
					this._firstFrame = false;
					return;
				}
				for (int i = 0; i < this.PropertyList.ChildCount; i++)
				{
					TooltipPropertyWidget tooltipPropertyWidget;
					if ((tooltipPropertyWidget = this.PropertyList.GetChild(i) as TooltipPropertyWidget) != null && tooltipPropertyWidget.IsTwoColumn && !tooltipPropertyWidget.IsMultiLine)
					{
						if (num < tooltipPropertyWidget.ValueLabelContainer.Size.X)
						{
							num = tooltipPropertyWidget.ValueLabelContainer.Size.X;
						}
						if (num2 < tooltipPropertyWidget.DefinitionLabelContainer.Size.X)
						{
							num2 = tooltipPropertyWidget.DefinitionLabelContainer.Size.X;
						}
					}
				}
				for (int j = 0; j < this.PropertyList.ChildCount; j++)
				{
					TooltipPropertyWidget tooltipPropertyWidget2;
					if ((tooltipPropertyWidget2 = this.PropertyList.GetChild(j) as TooltipPropertyWidget) != null)
					{
						if (tooltipPropertyWidget2.IsBattleMode)
						{
							flag = true;
						}
						else if (tooltipPropertyWidget2.IsBattleModeOver)
						{
							flag = false;
						}
						if (flag && (this._definitionRelationBrush == null || this._valueRelationBrush == null))
						{
							this.UpdateRelationBrushes();
						}
						tooltipPropertyWidget2.RefreshSize(flag, battleScopeSize, num, num2, this._definitionRelationBrush, this._valueRelationBrush);
					}
				}
			}
			else
			{
				this._firstFrame = true;
			}
			if (this.PropertyListBackground != null)
			{
				if (this.Mode == 2)
				{
					this.PropertyListBackground.Color = this.AllyColor;
					return;
				}
				if (this.Mode == 3)
				{
					this.PropertyListBackground.Color = this.EnemyColor;
					return;
				}
				this.PropertyListBackground.Color = this.NeutralColor;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00030B21 File Offset: 0x0002ED21
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x00030B29 File Offset: 0x0002ED29
		[Editor(false)]
		public int Mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (this._mode != value)
				{
					this._mode = value;
				}
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x00030B3B File Offset: 0x0002ED3B
		// (set) Token: 0x0600119F RID: 4511 RVA: 0x00030B43 File Offset: 0x0002ED43
		[Editor(false)]
		public Brush NeutralTroopsTextBrush
		{
			get
			{
				return this._neutralTroopsTextBrush;
			}
			set
			{
				if (this._neutralTroopsTextBrush != value)
				{
					this._neutralTroopsTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "NeutralTroopsTextBrush");
				}
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x00030B61 File Offset: 0x0002ED61
		// (set) Token: 0x060011A1 RID: 4513 RVA: 0x00030B69 File Offset: 0x0002ED69
		[Editor(false)]
		public Brush EnemyTroopsTextBrush
		{
			get
			{
				return this._enemyTroopsTextBrush;
			}
			set
			{
				if (this._enemyTroopsTextBrush != value)
				{
					this._enemyTroopsTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "EnemyTroopsTextBrush");
				}
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00030B87 File Offset: 0x0002ED87
		// (set) Token: 0x060011A3 RID: 4515 RVA: 0x00030B8F File Offset: 0x0002ED8F
		[Editor(false)]
		public Brush AllyTroopsTextBrush
		{
			get
			{
				return this._allyTroopsTextBrush;
			}
			set
			{
				if (this._allyTroopsTextBrush != value)
				{
					this._allyTroopsTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "AllyTroopsTextBrush");
				}
			}
		}

		// Token: 0x04000802 RID: 2050
		private Brush _definitionRelationBrush;

		// Token: 0x04000803 RID: 2051
		private Brush _valueRelationBrush;

		// Token: 0x04000804 RID: 2052
		private bool _firstFrame = true;

		// Token: 0x04000805 RID: 2053
		private int _mode;

		// Token: 0x04000806 RID: 2054
		private Brush _neutralTroopsTextBrush;

		// Token: 0x04000807 RID: 2055
		private Brush _allyTroopsTextBrush;

		// Token: 0x04000808 RID: 2056
		private Brush _enemyTroopsTextBrush;
	}
}
