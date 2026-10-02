using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x0200013A RID: 314
	public class KingdomTabControlListPanel : ListPanel
	{
		// Token: 0x0600106C RID: 4204 RVA: 0x0002D40C File Offset: 0x0002B60C
		public KingdomTabControlListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0002D418 File Offset: 0x0002B618
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.FiefsButton.IsSelected = this.FiefsPanel.IsVisible;
			this.PoliciesButton.IsSelected = this.PoliciesPanel.IsVisible;
			this.ClansButton.IsSelected = this.ClansPanel.IsVisible;
			this.ArmiesButton.IsSelected = this.ArmiesPanel.IsVisible;
			this.DiplomacyButton.IsSelected = this.DiplomacyPanel.IsVisible;
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x0002D49A File Offset: 0x0002B69A
		// (set) Token: 0x0600106F RID: 4207 RVA: 0x0002D4A2 File Offset: 0x0002B6A2
		[Editor(false)]
		public Widget DiplomacyPanel
		{
			get
			{
				return this._diplomacyPanel;
			}
			set
			{
				if (this._diplomacyPanel != value)
				{
					this._diplomacyPanel = value;
					base.OnPropertyChanged<Widget>(value, "DiplomacyPanel");
				}
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0002D4C0 File Offset: 0x0002B6C0
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x0002D4C8 File Offset: 0x0002B6C8
		[Editor(false)]
		public Widget ArmiesPanel
		{
			get
			{
				return this._armiesPanel;
			}
			set
			{
				if (this._armiesPanel != value)
				{
					this._armiesPanel = value;
					base.OnPropertyChanged<Widget>(value, "ArmiesPanel");
				}
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0002D4E6 File Offset: 0x0002B6E6
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x0002D4EE File Offset: 0x0002B6EE
		[Editor(false)]
		public Widget ClansPanel
		{
			get
			{
				return this._clansPanel;
			}
			set
			{
				if (this._clansPanel != value)
				{
					this._clansPanel = value;
					base.OnPropertyChanged<Widget>(value, "ClansPanel");
				}
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0002D50C File Offset: 0x0002B70C
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x0002D514 File Offset: 0x0002B714
		[Editor(false)]
		public Widget PoliciesPanel
		{
			get
			{
				return this._policiesPanel;
			}
			set
			{
				if (this._policiesPanel != value)
				{
					this._policiesPanel = value;
					base.OnPropertyChanged<Widget>(value, "PoliciesPanel");
				}
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0002D532 File Offset: 0x0002B732
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x0002D53A File Offset: 0x0002B73A
		[Editor(false)]
		public Widget FiefsPanel
		{
			get
			{
				return this._fiefsPanel;
			}
			set
			{
				if (this._fiefsPanel != value)
				{
					this._fiefsPanel = value;
					base.OnPropertyChanged<Widget>(value, "FiefsPanel");
				}
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0002D558 File Offset: 0x0002B758
		// (set) Token: 0x06001079 RID: 4217 RVA: 0x0002D560 File Offset: 0x0002B760
		[Editor(false)]
		public ButtonWidget FiefsButton
		{
			get
			{
				return this._fiefsButton;
			}
			set
			{
				if (this._fiefsButton != value)
				{
					this._fiefsButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FiefsButton");
				}
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0002D57E File Offset: 0x0002B77E
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x0002D586 File Offset: 0x0002B786
		[Editor(false)]
		public ButtonWidget PoliciesButton
		{
			get
			{
				return this._policiesButton;
			}
			set
			{
				if (this._policiesButton != value)
				{
					this._policiesButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "PoliciesButton");
				}
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0002D5A4 File Offset: 0x0002B7A4
		// (set) Token: 0x0600107D RID: 4221 RVA: 0x0002D5AC File Offset: 0x0002B7AC
		[Editor(false)]
		public ButtonWidget ClansButton
		{
			get
			{
				return this._clansButton;
			}
			set
			{
				if (this._clansButton != value)
				{
					this._clansButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ClansButton");
				}
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0002D5CA File Offset: 0x0002B7CA
		// (set) Token: 0x0600107F RID: 4223 RVA: 0x0002D5D2 File Offset: 0x0002B7D2
		[Editor(false)]
		public ButtonWidget ArmiesButton
		{
			get
			{
				return this._armiesButton;
			}
			set
			{
				if (this._armiesButton != value)
				{
					this._armiesButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ArmiesButton");
				}
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0002D5F0 File Offset: 0x0002B7F0
		// (set) Token: 0x06001081 RID: 4225 RVA: 0x0002D5F8 File Offset: 0x0002B7F8
		[Editor(false)]
		public ButtonWidget DiplomacyButton
		{
			get
			{
				return this._diplomacyButton;
			}
			set
			{
				if (this._diplomacyButton != value)
				{
					this._diplomacyButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DiplomacyButton");
				}
			}
		}

		// Token: 0x0400077C RID: 1916
		private Widget _armiesPanel;

		// Token: 0x0400077D RID: 1917
		private Widget _clansPanel;

		// Token: 0x0400077E RID: 1918
		private Widget _policiesPanel;

		// Token: 0x0400077F RID: 1919
		private Widget _fiefsPanel;

		// Token: 0x04000780 RID: 1920
		private Widget _diplomacyPanel;

		// Token: 0x04000781 RID: 1921
		private ButtonWidget _fiefsButton;

		// Token: 0x04000782 RID: 1922
		private ButtonWidget _clansButton;

		// Token: 0x04000783 RID: 1923
		private ButtonWidget _policiesButton;

		// Token: 0x04000784 RID: 1924
		private ButtonWidget _armiesButton;

		// Token: 0x04000785 RID: 1925
		private ButtonWidget _diplomacyButton;
	}
}
