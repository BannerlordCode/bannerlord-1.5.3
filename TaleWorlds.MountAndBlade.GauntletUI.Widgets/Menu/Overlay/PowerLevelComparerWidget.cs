using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000115 RID: 277
	public class PowerLevelComparerWidget : Widget
	{
		// Token: 0x06000ED7 RID: 3799 RVA: 0x00028EE5 File Offset: 0x000270E5
		public PowerLevelComparerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00028EF0 File Offset: 0x000270F0
		protected override void OnLateUpdate(float dt)
		{
			if (this.AttackerPowerWidget != null)
			{
				this.AttackerPowerWidget.AlphaFactor = 0.7f;
				this.AttackerPowerWidget.ValueFactor = -70f;
			}
			if (this.DefenderPowerWidget != null)
			{
				this.DefenderPowerWidget.AlphaFactor = 0.7f;
				this.DefenderPowerWidget.ValueFactor = -70f;
			}
			if (this._powerListPanel != null)
			{
				if (this._defenderSideInitialPowerLevelDescription == null)
				{
					this._defenderSideInitialPowerLevelDescription = new ContainerItemDescription();
					this._defenderSideInitialPowerLevelDescription.WidgetId = "DefenderSideInitialPowerLevel";
					this._powerListPanel.AddItemDescription(this._defenderSideInitialPowerLevelDescription);
				}
				if (this._attackerSideInitialPowerLevelDescription == null)
				{
					this._attackerSideInitialPowerLevelDescription = new ContainerItemDescription();
					this._attackerSideInitialPowerLevelDescription.WidgetId = "AttackerSideInitialPowerLevel";
					this._powerListPanel.AddItemDescription(this._attackerSideInitialPowerLevelDescription);
				}
			}
			if (this._defenderPowerListPanel != null)
			{
				if (this._defenderSidePowerLevelDescription == null)
				{
					this._defenderSidePowerLevelDescription = new ContainerItemDescription();
					this._defenderSidePowerLevelDescription.WidgetId = "DefenderSidePowerLevel";
					this._defenderPowerListPanel.AddItemDescription(this._defenderSidePowerLevelDescription);
				}
				if (this._defenderSideEmptyPowerLevelDescription == null)
				{
					this._defenderSideEmptyPowerLevelDescription = new ContainerItemDescription();
					this._defenderSideEmptyPowerLevelDescription.WidgetId = "DefenderSideEmptyPowerLevel";
					this._defenderPowerListPanel.AddItemDescription(this._defenderSideEmptyPowerLevelDescription);
				}
			}
			if (this._attackerPowerListPanel != null)
			{
				if (this._attackerSidePowerLevelDescription == null)
				{
					this._attackerSidePowerLevelDescription = new ContainerItemDescription();
					this._attackerSidePowerLevelDescription.WidgetId = "AttackerSidePowerLevel";
					this._attackerPowerListPanel.AddItemDescription(this._attackerSidePowerLevelDescription);
				}
				if (this._attackerSideEmptyPowerLevelDescription == null)
				{
					this._attackerSideEmptyPowerLevelDescription = new ContainerItemDescription();
					this._attackerSideEmptyPowerLevelDescription.WidgetId = "AttackerSideEmptyPowerLevel";
					this._attackerPowerListPanel.AddItemDescription(this._attackerSideEmptyPowerLevelDescription);
				}
			}
			if (this._defenderSideInitialPowerLevelDescription != null && this._attackerSideInitialPowerLevelDescription != null)
			{
				float num = (float)this.InitialDefenderBattlePower / (float)(this.InitialAttackerBattlePower + this.InitialDefenderBattlePower);
				float num2 = (float)this.InitialAttackerBattlePower / (float)(this.InitialAttackerBattlePower + this.InitialDefenderBattlePower);
				if (this._defenderSideInitialPowerLevelDescription.WidthStretchRatio != num || this._attackerSideInitialPowerLevelDescription.WidthStretchRatio != num2)
				{
					this._defenderSideInitialPowerLevelDescription.WidthStretchRatio = num;
					this._attackerSideInitialPowerLevelDescription.WidthStretchRatio = num2;
					base.SetMeasureAndLayoutDirty();
				}
			}
			if (this._defenderSidePowerLevelDescription != null && this._defenderSideEmptyPowerLevelDescription != null)
			{
				float num3 = 1f - (float)this.DefenderPower / (float)this.InitialDefenderBattlePower;
				float num4 = (float)this.DefenderPower / (float)this.InitialDefenderBattlePower;
				if (this._defenderSideEmptyPowerLevelDescription.WidthStretchRatio != num3 || this._defenderSidePowerLevelDescription.WidthStretchRatio != num4)
				{
					this._defenderSidePowerLevelDescription.WidthStretchRatio = num4;
					this._defenderSideEmptyPowerLevelDescription.WidthStretchRatio = num3;
					base.SetMeasureAndLayoutDirty();
				}
			}
			if (this._attackerSidePowerLevelDescription != null && this._attackerSideEmptyPowerLevelDescription != null)
			{
				float num5 = 1f - (float)this.AttackerPower / (float)this.InitialAttackerBattlePower;
				float num6 = (float)this.AttackerPower / (float)this.InitialAttackerBattlePower;
				if (this._attackerSidePowerLevelDescription.WidthStretchRatio != num6 || this._attackerSideEmptyPowerLevelDescription.WidthStretchRatio != num5)
				{
					this._attackerSidePowerLevelDescription.WidthStretchRatio = num6;
					this._attackerSideEmptyPowerLevelDescription.WidthStretchRatio = num5;
					base.SetMeasureAndLayoutDirty();
				}
			}
			if (this.IsCenterSeperatorEnabled && this.CenterSeperatorWidget != null)
			{
				this.CenterSeperatorWidget.ScaledPositionXOffset = this.AttackerPowerWidget.Size.X - (this.CenterSeperatorWidget.Size.X - this.CenterSpace) / 2f;
			}
			base.OnLateUpdate(dt);
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0002924B File Offset: 0x0002744B
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x00029253 File Offset: 0x00027453
		[Editor(false)]
		public bool IsCenterSeperatorEnabled
		{
			get
			{
				return this._isCenterSeperatorEnabled;
			}
			set
			{
				if (this._isCenterSeperatorEnabled != value)
				{
					this._isCenterSeperatorEnabled = value;
					base.OnPropertyChanged(value, "IsCenterSeperatorEnabled");
				}
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00029271 File Offset: 0x00027471
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x00029279 File Offset: 0x00027479
		[Editor(false)]
		public float CenterSpace
		{
			get
			{
				return this._centerSpace;
			}
			set
			{
				if (this._centerSpace != value)
				{
					this._centerSpace = value;
					base.OnPropertyChanged(value, "CenterSpace");
				}
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00029297 File Offset: 0x00027497
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x0002929F File Offset: 0x0002749F
		[Editor(false)]
		public double DefenderPower
		{
			get
			{
				return this._defenderPower;
			}
			set
			{
				if (this._defenderPower != value && !double.IsNaN(value))
				{
					this._defenderPower = value;
					base.OnPropertyChanged(value, "DefenderPower");
				}
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x000292C5 File Offset: 0x000274C5
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x000292CD File Offset: 0x000274CD
		[Editor(false)]
		public double AttackerPower
		{
			get
			{
				return this._attackerPower;
			}
			set
			{
				if (this._attackerPower != value && !double.IsNaN(value))
				{
					this._attackerPower = value;
					base.OnPropertyChanged(value, "AttackerPower");
				}
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x000292F3 File Offset: 0x000274F3
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x000292FB File Offset: 0x000274FB
		[Editor(false)]
		public double InitialAttackerBattlePower
		{
			get
			{
				return this._initialAttackerBattlePower;
			}
			set
			{
				if (this._initialAttackerBattlePower != value && !double.IsNaN(value))
				{
					this._initialAttackerBattlePower = value;
					base.OnPropertyChanged(value, "InitialAttackerBattlePower");
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00029321 File Offset: 0x00027521
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x00029329 File Offset: 0x00027529
		[Editor(false)]
		public double InitialDefenderBattlePower
		{
			get
			{
				return this._initialDefenderBattlePower;
			}
			set
			{
				if (this._initialDefenderBattlePower != value && !double.IsNaN(value))
				{
					this._initialDefenderBattlePower = value;
					base.OnPropertyChanged(value, "InitialDefenderBattlePower");
				}
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0002934F File Offset: 0x0002754F
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00029357 File Offset: 0x00027557
		[Editor(false)]
		public Widget AttackerPowerWidget
		{
			get
			{
				return this._attackerPowerWidget;
			}
			set
			{
				if (this._attackerPowerWidget != value)
				{
					this._attackerPowerWidget = value;
					base.OnPropertyChanged<Widget>(value, "AttackerPowerWidget");
				}
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00029375 File Offset: 0x00027575
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x0002937D File Offset: 0x0002757D
		[Editor(false)]
		public Widget DefenderPowerWidget
		{
			get
			{
				return this._defenderPowerWidget;
			}
			set
			{
				if (this._defenderPowerWidget != value)
				{
					this._defenderPowerWidget = value;
					base.OnPropertyChanged<Widget>(value, "DefenderPowerWidget");
				}
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0002939B File Offset: 0x0002759B
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x000293A3 File Offset: 0x000275A3
		[Editor(false)]
		public ListPanel PowerListPanel
		{
			get
			{
				return this._powerListPanel;
			}
			set
			{
				if (this._powerListPanel != value)
				{
					this._powerListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "PowerListPanel");
				}
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x000293C1 File Offset: 0x000275C1
		// (set) Token: 0x06000EEC RID: 3820 RVA: 0x000293C9 File Offset: 0x000275C9
		[Editor(false)]
		public ListPanel AttackerPowerListPanel
		{
			get
			{
				return this._attackerPowerListPanel;
			}
			set
			{
				if (this._attackerPowerListPanel != value)
				{
					this._attackerPowerListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "AttackerPowerListPanel");
				}
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x000293E7 File Offset: 0x000275E7
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x000293EF File Offset: 0x000275EF
		[Editor(false)]
		public ListPanel DefenderPowerListPanel
		{
			get
			{
				return this._defenderPowerListPanel;
			}
			set
			{
				if (this._defenderPowerListPanel != value)
				{
					this._defenderPowerListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "DefenderPowerListPanel");
				}
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0002940D File Offset: 0x0002760D
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x00029415 File Offset: 0x00027615
		[Editor(false)]
		public Widget CenterSeperatorWidget
		{
			get
			{
				return this._centerSeperatorWidget;
			}
			set
			{
				if (this._centerSeperatorWidget != value)
				{
					this._centerSeperatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "CenterSeperatorWidget");
				}
			}
		}

		// Token: 0x040006C0 RID: 1728
		private Widget _centerSeperatorWidget;

		// Token: 0x040006C1 RID: 1729
		private bool _isCenterSeperatorEnabled;

		// Token: 0x040006C2 RID: 1730
		private float _centerSpace;

		// Token: 0x040006C3 RID: 1731
		private double _defenderPower;

		// Token: 0x040006C4 RID: 1732
		private double _attackerPower;

		// Token: 0x040006C5 RID: 1733
		private double _initialAttackerBattlePower;

		// Token: 0x040006C6 RID: 1734
		private double _initialDefenderBattlePower;

		// Token: 0x040006C7 RID: 1735
		private Widget _defenderPowerWidget;

		// Token: 0x040006C8 RID: 1736
		private Widget _attackerPowerWidget;

		// Token: 0x040006C9 RID: 1737
		private ListPanel _powerListPanel;

		// Token: 0x040006CA RID: 1738
		private ListPanel _defenderPowerListPanel;

		// Token: 0x040006CB RID: 1739
		private ListPanel _attackerPowerListPanel;

		// Token: 0x040006CC RID: 1740
		private ContainerItemDescription _defenderSideInitialPowerLevelDescription;

		// Token: 0x040006CD RID: 1741
		private ContainerItemDescription _attackerSideInitialPowerLevelDescription;

		// Token: 0x040006CE RID: 1742
		private ContainerItemDescription _defenderSidePowerLevelDescription;

		// Token: 0x040006CF RID: 1743
		private ContainerItemDescription _defenderSideEmptyPowerLevelDescription;

		// Token: 0x040006D0 RID: 1744
		private ContainerItemDescription _attackerSidePowerLevelDescription;

		// Token: 0x040006D1 RID: 1745
		private ContainerItemDescription _attackerSideEmptyPowerLevelDescription;
	}
}
