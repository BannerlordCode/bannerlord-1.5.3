using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000107 RID: 263
	public class CustomBattleAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00017E7B File Offset: 0x0001607B
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x00017E83 File Offset: 0x00016083
		public CustomBattleCombatant CustomBattleCombatant { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00017E8C File Offset: 0x0001608C
		IBattleCombatant IAgentOriginBase.BattleCombatant
		{
			get
			{
				return this.CustomBattleCombatant;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00017E94 File Offset: 0x00016094
		// (set) Token: 0x06000D52 RID: 3410 RVA: 0x00017E9C File Offset: 0x0001609C
		public BasicCharacterObject Troop { get; private set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x00017EA5 File Offset: 0x000160A5
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00017EAD File Offset: 0x000160AD
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x00017EB5 File Offset: 0x000160B5
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00017EBD File Offset: 0x000160BD
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x00017EC5 File Offset: 0x000160C5
		// (set) Token: 0x06000D58 RID: 3416 RVA: 0x00017ECD File Offset: 0x000160CD
		public int Rank { get; private set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00017ED6 File Offset: 0x000160D6
		public Banner Banner
		{
			get
			{
				return this.CustomBattleCombatant.Banner;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00017EE3 File Offset: 0x000160E3
		public bool IsUnderPlayersCommand
		{
			get
			{
				return this._isPlayerSide;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x00017EEB File Offset: 0x000160EB
		public bool IsInSameArmyAsPlayer
		{
			get
			{
				return this._isPlayerSide;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00017EF3 File Offset: 0x000160F3
		public uint FactionColor
		{
			get
			{
				return this.CustomBattleCombatant.BasicCulture.Color;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00017F05 File Offset: 0x00016105
		public uint FactionColor2
		{
			get
			{
				return this.CustomBattleCombatant.BasicCulture.Color2;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00017F17 File Offset: 0x00016117
		public int Seed
		{
			get
			{
				return this.Troop.GetDefaultFaceSeed(this.Rank);
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00017F2C File Offset: 0x0001612C
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00017F48 File Offset: 0x00016148
		public CustomBattleAgentOrigin(CustomBattleCombatant customBattleCombatant, BasicCharacterObject characterObject, CustomBattleTroopSupplier troopSupplier, bool isPlayerSide, int rank = -1, UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor))
		{
			this.CustomBattleCombatant = customBattleCombatant;
			this.Troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._troopSupplier = troopSupplier;
			this._isPlayerSide = isPlayerSide;
			AgentOriginUtilities.GetDefaultTroopTraits(this.Troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00017FD6 File Offset: 0x000161D6
		public void SetWounded()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopWounded();
				this._isRemoved = true;
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00017FF2 File Offset: 0x000161F2
		public void SetKilled()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopKilled();
				this._isRemoved = true;
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0001800E File Offset: 0x0001620E
		public void SetRouted(bool isOrderRetreat)
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopRouted();
				this._isRemoved = true;
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0001802A File Offset: 0x0001622A
		public void OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0001802C File Offset: 0x0001622C
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0001802E File Offset: 0x0001622E
		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00018035 File Offset: 0x00016235
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x040002DD RID: 733
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x040002DE RID: 734
		private readonly bool _isPlayerSide;

		// Token: 0x040002DF RID: 735
		private CustomBattleTroopSupplier _troopSupplier;

		// Token: 0x040002E0 RID: 736
		private bool _isRemoved;

		// Token: 0x040002E1 RID: 737
		private bool _hasThrownWeapon;

		// Token: 0x040002E2 RID: 738
		private bool _hasHeavyArmor;

		// Token: 0x040002E3 RID: 739
		private bool _hasShield;

		// Token: 0x040002E4 RID: 740
		private bool _hasSpear;
	}
}
