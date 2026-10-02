using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000054 RID: 84
	public class ScoreboardBattleResultTitleBackgroundWidget : Widget
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x0000E9AF File Offset: 0x0000CBAF
		public ScoreboardBattleResultTitleBackgroundWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
		private void BattleResultUpdated()
		{
			if (this.DefeatWidget != null)
			{
				this.DefeatWidget.IsVisible = this.BattleResult == 0;
			}
			if (this.VictoryWidget != null)
			{
				this.VictoryWidget.IsVisible = this.BattleResult == 1;
			}
			if (this.RetreatWidget != null)
			{
				this.RetreatWidget.IsVisible = this.BattleResult == 2;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0000EA19 File Offset: 0x0000CC19
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0000EA21 File Offset: 0x0000CC21
		[Editor(false)]
		public int BattleResult
		{
			get
			{
				return this._battleResult;
			}
			set
			{
				if (this._battleResult != value)
				{
					this._battleResult = value;
					base.OnPropertyChanged(value, "BattleResult");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0000EA45 File Offset: 0x0000CC45
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0000EA4D File Offset: 0x0000CC4D
		[Editor(false)]
		public Widget VictoryWidget
		{
			get
			{
				return this._victoryWidget;
			}
			set
			{
				if (this._victoryWidget != value)
				{
					this._victoryWidget = value;
					base.OnPropertyChanged<Widget>(value, "VictoryWidget");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0000EA71 File Offset: 0x0000CC71
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0000EA79 File Offset: 0x0000CC79
		[Editor(false)]
		public Widget DefeatWidget
		{
			get
			{
				return this._defeatWidget;
			}
			set
			{
				if (this._defeatWidget != value)
				{
					this._defeatWidget = value;
					base.OnPropertyChanged<Widget>(value, "DefeatWidget");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0000EA9D File Offset: 0x0000CC9D
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0000EAA5 File Offset: 0x0000CCA5
		[Editor(false)]
		public Widget RetreatWidget
		{
			get
			{
				return this._retreatWidget;
			}
			set
			{
				if (this._retreatWidget != value)
				{
					this._retreatWidget = value;
					base.OnPropertyChanged<Widget>(value, "RetreatWidget");
					this.BattleResultUpdated();
				}
			}
		}

		// Token: 0x040001F6 RID: 502
		private int _battleResult;

		// Token: 0x040001F7 RID: 503
		private Widget _victoryWidget;

		// Token: 0x040001F8 RID: 504
		private Widget _defeatWidget;

		// Token: 0x040001F9 RID: 505
		private Widget _retreatWidget;
	}
}
