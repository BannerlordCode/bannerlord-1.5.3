using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000064 RID: 100
	public class MissionSiegeEngineMarkerTargetVM : ViewModel
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0001B9EB File Offset: 0x00019BEB
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0001B9F3 File Offset: 0x00019BF3
		public SiegeWeapon Engine { get; private set; }

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001B9FC File Offset: 0x00019BFC
		public MissionSiegeEngineMarkerTargetVM(SiegeWeapon engine, bool isEnemy)
		{
			this.Engine = engine;
			this.EngineType = this.Engine.GetSiegeEngineType().StringId;
			this.IsEnemy = isEnemy;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001BA28 File Offset: 0x00019C28
		public void Refresh()
		{
			this.HitPoints = MathF.Ceiling(this.Engine.DestructionComponent.HitPoint / this.Engine.DestructionComponent.MaxHitPoint * 100f);
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0001BA5C File Offset: 0x00019C5C
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x0001BA64 File Offset: 0x00019C64
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0001BA82 File Offset: 0x00019C82
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0001BA8A File Offset: 0x00019C8A
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (this._isEnemy != value)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0001BAA8 File Offset: 0x00019CA8
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0001BAB0 File Offset: 0x00019CB0
		[DataSourceProperty]
		public string EngineType
		{
			get
			{
				return this._engineType;
			}
			set
			{
				if (this._engineType != value)
				{
					this._engineType = value;
					base.OnPropertyChangedWithValue<string>(value, "EngineType");
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001BAD3 File Offset: 0x00019CD3
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0001BADB File Offset: 0x00019CDB
		[DataSourceProperty]
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (this._isBehind != value)
				{
					this._isBehind = value;
					base.OnPropertyChangedWithValue(value, "IsBehind");
				}
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001BAF9 File Offset: 0x00019CF9
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0001BB01 File Offset: 0x00019D01
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0001BB3C File Offset: 0x00019D3C
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0001BB44 File Offset: 0x00019D44
		[DataSourceProperty]
		public float Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value && !float.IsNaN(value))
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0001BB6A File Offset: 0x00019D6A
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0001BB72 File Offset: 0x00019D72
		[DataSourceProperty]
		public int HitPoints
		{
			get
			{
				return this._hitPoints;
			}
			set
			{
				if (this._hitPoints != value)
				{
					this._hitPoints = value;
					base.OnPropertyChangedWithValue(value, "HitPoints");
				}
			}
		}

		// Token: 0x04000399 RID: 921
		private Vec2 _screenPosition;

		// Token: 0x0400039A RID: 922
		private float _distance;

		// Token: 0x0400039B RID: 923
		private bool _isEnabled;

		// Token: 0x0400039C RID: 924
		private bool _isBehind;

		// Token: 0x0400039D RID: 925
		private bool _isEnemy;

		// Token: 0x0400039E RID: 926
		private string _engineType;

		// Token: 0x0400039F RID: 927
		private int _hitPoints;
	}
}
