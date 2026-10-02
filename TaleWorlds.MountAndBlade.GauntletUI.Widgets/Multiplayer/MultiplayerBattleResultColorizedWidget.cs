using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000087 RID: 135
	public class MultiplayerBattleResultColorizedWidget : Widget
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x000168FF File Offset: 0x00014AFF
		public MultiplayerBattleResultColorizedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00016910 File Offset: 0x00014B10
		private void BattleResultUpdated()
		{
			if (this.BattleResult == 2)
			{
				base.Color = this.DrawColor;
				return;
			}
			if (this.BattleResult == 1)
			{
				base.Color = this.VictoryColor;
				return;
			}
			if (this.BattleResult == 0)
			{
				base.Color = this.DefeatColor;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0001695D File Offset: 0x00014B5D
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x00016965 File Offset: 0x00014B65
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

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00016989 File Offset: 0x00014B89
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00016991 File Offset: 0x00014B91
		[Editor(false)]
		public Color DrawColor
		{
			get
			{
				return this._drawColor;
			}
			set
			{
				if (this._drawColor != value)
				{
					this._drawColor = value;
					base.OnPropertyChanged(value, "DrawColor");
				}
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000169B4 File Offset: 0x00014BB4
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x000169BC File Offset: 0x00014BBC
		[Editor(false)]
		public Color VictoryColor
		{
			get
			{
				return this._victoryColor;
			}
			set
			{
				if (this._victoryColor != value)
				{
					this._victoryColor = value;
					base.OnPropertyChanged(value, "VictoryColor");
				}
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000169DF File Offset: 0x00014BDF
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x000169E7 File Offset: 0x00014BE7
		[Editor(false)]
		public Color DefeatColor
		{
			get
			{
				return this._defeatColor;
			}
			set
			{
				if (this._defeatColor != value)
				{
					this._defeatColor = value;
					base.OnPropertyChanged(value, "DefeatColor");
				}
			}
		}

		// Token: 0x04000359 RID: 857
		private int _battleResult = -1;

		// Token: 0x0400035A RID: 858
		private Color _drawColor;

		// Token: 0x0400035B RID: 859
		private Color _victoryColor;

		// Token: 0x0400035C RID: 860
		private Color _defeatColor;
	}
}
