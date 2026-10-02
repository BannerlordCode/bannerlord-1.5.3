using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x020000A1 RID: 161
	public class MissionSiegeEngineMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0003130B File Offset: 0x0002F50B
		public override Vec3 WorldPosition
		{
			get
			{
				if (!(this._siegeEngine != null))
				{
					return Vec3.One;
				}
				return this._siegeEngine.GlobalPosition;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0003132C File Offset: 0x0002F52C
		protected override float HeightOffset
		{
			get
			{
				return 2.5f;
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00031334 File Offset: 0x0002F534
		public MissionSiegeEngineMarkerTargetVM(SiegeWeapon siegeEngine)
			: base(MissionMarkerType.SiegeEngine)
		{
			this._siegeEngine = GameEntity.CreateFromWeakEntity(siegeEngine.GameEntity);
			this.Side = siegeEngine.Side;
			this.SiegeEngineID = siegeEngine.GetSiegeEngineType().StringId;
			uint num = ((this.Side == BattleSideEnum.Attacker) ? Mission.Current.AttackerTeam.Color : Mission.Current.DefenderTeam.Color);
			uint num2 = ((this.Side == BattleSideEnum.Attacker) ? Mission.Current.AttackerTeam.Color2 : Mission.Current.DefenderTeam.Color2);
			base.RefreshColor(num, num2);
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x000313D2 File Offset: 0x0002F5D2
		// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x000313DA File Offset: 0x0002F5DA
		[DataSourceProperty]
		public string SiegeEngineID
		{
			get
			{
				return this._siegeEngineID;
			}
			set
			{
				if (value != this._siegeEngineID)
				{
					this._siegeEngineID = value;
					base.OnPropertyChangedWithValue<string>(value, "SiegeEngineID");
				}
			}
		}

		// Token: 0x04000758 RID: 1880
		private readonly GameEntity _siegeEngine;

		// Token: 0x04000759 RID: 1881
		public readonly BattleSideEnum Side;

		// Token: 0x0400075A RID: 1882
		private string _siegeEngineID;
	}
}
