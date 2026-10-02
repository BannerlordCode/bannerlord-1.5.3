using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000106 RID: 262
	public class BasicBattleAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00017DF5 File Offset: 0x00015FF5
		bool IAgentOriginBase.IsUnderPlayersCommand
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x00017DF8 File Offset: 0x00015FF8
		bool IAgentOriginBase.IsInSameArmyAsPlayer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00017DFB File Offset: 0x00015FFB
		uint IAgentOriginBase.FactionColor
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x00017DFE File Offset: 0x00015FFE
		uint IAgentOriginBase.FactionColor2
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00017E01 File Offset: 0x00016001
		IBattleCombatant IAgentOriginBase.BattleCombatant
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00017E04 File Offset: 0x00016004
		int IAgentOriginBase.UniqueSeed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00017E07 File Offset: 0x00016007
		int IAgentOriginBase.Seed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00017E0A File Offset: 0x0001600A
		Banner IAgentOriginBase.Banner
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00017E0D File Offset: 0x0001600D
		BasicCharacterObject IAgentOriginBase.Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x00017E15 File Offset: 0x00016015
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00017E1D File Offset: 0x0001601D
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x00017E25 File Offset: 0x00016025
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00017E2D File Offset: 0x0001602D
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00017E35 File Offset: 0x00016035
		public BasicBattleAgentOrigin(BasicCharacterObject troop)
		{
			this._troop = troop;
			AgentOriginUtilities.GetDefaultTroopTraits(this._troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00017E67 File Offset: 0x00016067
		void IAgentOriginBase.SetWounded()
		{
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00017E69 File Offset: 0x00016069
		void IAgentOriginBase.SetKilled()
		{
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00017E6B File Offset: 0x0001606B
		void IAgentOriginBase.SetRouted(bool isOrderRetreat)
		{
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00017E6D File Offset: 0x0001606D
		void IAgentOriginBase.OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00017E6F File Offset: 0x0001606F
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00017E71 File Offset: 0x00016071
		void IAgentOriginBase.SetBanner(Banner banner)
		{
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00017E73 File Offset: 0x00016073
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x040002D5 RID: 725
		private BasicCharacterObject _troop;

		// Token: 0x040002D6 RID: 726
		private bool _hasThrownWeapon;

		// Token: 0x040002D7 RID: 727
		private bool _hasHeavyArmor;

		// Token: 0x040002D8 RID: 728
		private bool _hasShield;

		// Token: 0x040002D9 RID: 729
		private bool _hasSpear;
	}
}
