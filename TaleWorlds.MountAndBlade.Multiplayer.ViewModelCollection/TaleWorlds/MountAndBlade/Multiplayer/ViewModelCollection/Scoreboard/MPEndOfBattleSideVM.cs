using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x02000023 RID: 35
	public class MPEndOfBattleSideVM : ViewModel
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00009D0D File Offset: 0x00007F0D
		// (set) Token: 0x06000269 RID: 617 RVA: 0x00009D15 File Offset: 0x00007F15
		public MissionScoreboardComponent.MissionScoreboardSide Side { get; private set; }

		// Token: 0x0600026A RID: 618 RVA: 0x00009D20 File Offset: 0x00007F20
		public MPEndOfBattleSideVM(MissionScoreboardComponent missionScoreboardComponent, MissionScoreboardComponent.MissionScoreboardSide side, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColorInfo)
		{
			this._missionScoreboardComponent = missionScoreboardComponent;
			this.Side = side;
			this._culture = cultureColorInfo.Culture;
			if (this.Side != null)
			{
				this.CultureId = this._culture.StringId;
				this.Score = this.Side.SideScore;
				this.IsRoundWinner = this._missionScoreboardComponent.RoundWinner == side.Side || this._missionScoreboardComponent.RoundWinner == BattleSideEnum.None;
			}
			this.CultureColor1 = cultureColorInfo.Color1;
			this.CultureColor2 = cultureColorInfo.Color2;
			this.RefreshValues();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009DBF File Offset: 0x00007FBF
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Side != null)
			{
				this.CultureId = this._culture.StringId;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009DE0 File Offset: 0x00007FE0
		// (set) Token: 0x0600026D RID: 621 RVA: 0x00009DE8 File Offset: 0x00007FE8
		[DataSourceProperty]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (value != this._factionName)
				{
					this._factionName = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionName");
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00009E0B File Offset: 0x0000800B
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00009E13 File Offset: 0x00008013
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00009E36 File Offset: 0x00008036
		// (set) Token: 0x06000271 RID: 625 RVA: 0x00009E3E File Offset: 0x0000803E
		[DataSourceProperty]
		public int Score
		{
			get
			{
				return this._score;
			}
			set
			{
				if (value != this._score)
				{
					this._score = value;
					base.OnPropertyChangedWithValue(value, "Score");
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00009E5C File Offset: 0x0000805C
		// (set) Token: 0x06000273 RID: 627 RVA: 0x00009E64 File Offset: 0x00008064
		[DataSourceProperty]
		public bool IsRoundWinner
		{
			get
			{
				return this._isRoundWinner;
			}
			set
			{
				if (value != this._isRoundWinner)
				{
					this._isRoundWinner = value;
					base.OnPropertyChangedWithValue(value, "IsRoundWinner");
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00009E82 File Offset: 0x00008082
		// (set) Token: 0x06000275 RID: 629 RVA: 0x00009E8A File Offset: 0x0000808A
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00009EAD File Offset: 0x000080AD
		// (set) Token: 0x06000277 RID: 631 RVA: 0x00009EB5 File Offset: 0x000080B5
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x04000144 RID: 324
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x04000145 RID: 325
		private BasicCultureObject _culture;

		// Token: 0x04000146 RID: 326
		private string _factionName;

		// Token: 0x04000147 RID: 327
		private string _cultureId;

		// Token: 0x04000148 RID: 328
		private int _score;

		// Token: 0x04000149 RID: 329
		private bool _isRoundWinner;

		// Token: 0x0400014A RID: 330
		private Color _cultureColor1;

		// Token: 0x0400014B RID: 331
		private Color _cultureColor2;
	}
}
