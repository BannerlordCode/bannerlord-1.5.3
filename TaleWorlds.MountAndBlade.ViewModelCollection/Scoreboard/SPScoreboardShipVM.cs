using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000011 RID: 17
	public class SPScoreboardShipVM : ViewModel
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00005544 File Offset: 0x00003744
		public SPScoreboardShipVM(IShipOrigin ship, string shipType, IBattleCombatant owner, TeamSideEnum teamSideEnum, int formationIndex)
		{
			this.Ship = ship;
			this.ShipType = "Ship_" + shipType;
			this.Owner = owner;
			this.IsPlayerTeam = teamSideEnum == TeamSideEnum.PlayerTeam;
			this.IsPlayerAllyTeam = teamSideEnum == TeamSideEnum.PlayerAllyTeam;
			this.IsEnemyTeam = teamSideEnum == TeamSideEnum.EnemyTeam;
			this.MaxHealth = this.Ship.MaxHitPoints;
			this.CurrentHealth = this.Ship.HitPoints;
			this.FormationIndex = formationIndex;
			this.Tooltip = new BasicTooltipViewModel(delegate
			{
				Func<SPScoreboardShipVM, List<TooltipProperty>> getTooltip = SPScoreboardShipVM.GetTooltip;
				if (getTooltip == null)
				{
					return null;
				}
				return getTooltip(this);
			});
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000055D8 File Offset: 0x000037D8
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000055E0 File Offset: 0x000037E0
		[DataSourceProperty]
		public string ShipType
		{
			get
			{
				return this._shipType;
			}
			set
			{
				if (value != this._shipType)
				{
					this._shipType = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipType");
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00005603 File Offset: 0x00003803
		// (set) Token: 0x06000140 RID: 320 RVA: 0x0000560B File Offset: 0x0000380B
		[DataSourceProperty]
		public bool IsPlayerTeam
		{
			get
			{
				return this._isPlayerTeam;
			}
			set
			{
				if (value != this._isPlayerTeam)
				{
					this._isPlayerTeam = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerTeam");
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00005629 File Offset: 0x00003829
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00005631 File Offset: 0x00003831
		[DataSourceProperty]
		public bool IsPlayerAllyTeam
		{
			get
			{
				return this._isPlayerAllyTeam;
			}
			set
			{
				if (value != this._isPlayerAllyTeam)
				{
					this._isPlayerAllyTeam = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerAllyTeam");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000564F File Offset: 0x0000384F
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00005657 File Offset: 0x00003857
		[DataSourceProperty]
		public bool IsEnemyTeam
		{
			get
			{
				return this._isEnemyTeam;
			}
			set
			{
				if (value != this._isEnemyTeam)
				{
					this._isEnemyTeam = value;
					base.OnPropertyChangedWithValue(value, "IsEnemyTeam");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00005675 File Offset: 0x00003875
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000567D File Offset: 0x0000387D
		[DataSourceProperty]
		public float CurrentHealth
		{
			get
			{
				return this._currentHealth;
			}
			set
			{
				if (value != this._currentHealth)
				{
					this._currentHealth = value;
					base.OnPropertyChangedWithValue(value, "CurrentHealth");
					this.IsDestroyed = this._currentHealth == 0f;
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000056AE File Offset: 0x000038AE
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000056B6 File Offset: 0x000038B6
		[DataSourceProperty]
		public float MaxHealth
		{
			get
			{
				return this._maxHealth;
			}
			set
			{
				if (value != this._maxHealth)
				{
					this._maxHealth = value;
					base.OnPropertyChangedWithValue(value, "MaxHealth");
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000056D4 File Offset: 0x000038D4
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000056DC File Offset: 0x000038DC
		[DataSourceProperty]
		public bool IsDestroyed
		{
			get
			{
				return this._isDestroyed;
			}
			set
			{
				if (value != this._isDestroyed)
				{
					this._isDestroyed = value;
					base.OnPropertyChangedWithValue(value, "IsDestroyed");
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000056FA File Offset: 0x000038FA
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00005702 File Offset: 0x00003902
		[DataSourceProperty]
		public bool IsInactive
		{
			get
			{
				return this._isInactive;
			}
			set
			{
				if (value != this._isInactive)
				{
					this._isInactive = value;
					base.OnPropertyChangedWithValue(value, "IsInactive");
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00005720 File Offset: 0x00003920
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00005728 File Offset: 0x00003928
		[DataSourceProperty]
		public bool IsRetreated
		{
			get
			{
				return this._isRetreated;
			}
			set
			{
				if (value != this._isRetreated)
				{
					this._isRetreated = value;
					base.OnPropertyChangedWithValue(value, "IsRetreated");
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00005746 File Offset: 0x00003946
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000574E File Offset: 0x0000394E
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x04000091 RID: 145
		public int FormationIndex;

		// Token: 0x04000092 RID: 146
		public readonly IShipOrigin Ship;

		// Token: 0x04000093 RID: 147
		public readonly IBattleCombatant Owner;

		// Token: 0x04000094 RID: 148
		public static Func<SPScoreboardShipVM, List<TooltipProperty>> GetTooltip;

		// Token: 0x04000095 RID: 149
		private string _shipType;

		// Token: 0x04000096 RID: 150
		private bool _isPlayerTeam;

		// Token: 0x04000097 RID: 151
		private bool _isPlayerAllyTeam;

		// Token: 0x04000098 RID: 152
		private bool _isEnemyTeam;

		// Token: 0x04000099 RID: 153
		private float _currentHealth;

		// Token: 0x0400009A RID: 154
		private float _maxHealth;

		// Token: 0x0400009B RID: 155
		private bool _isDestroyed;

		// Token: 0x0400009C RID: 156
		private bool _isInactive;

		// Token: 0x0400009D RID: 157
		private bool _isRetreated;

		// Token: 0x0400009E RID: 158
		private BasicTooltipViewModel _tooltip;
	}
}
