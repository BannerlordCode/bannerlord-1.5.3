using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.TroopSuppliers;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x020004B1 RID: 1201
	public class PartyGroupAgentOrigin : IAgentOriginBase
	{
		// Token: 0x06004CAC RID: 19628 RVA: 0x00184768 File Offset: 0x00182968
		internal PartyGroupAgentOrigin(PartyGroupTroopSupplier supplier, UniqueTroopDescriptor descriptor, int rank)
		{
			this._supplier = supplier;
			this._descriptor = descriptor;
			this._rank = rank;
			AgentOriginUtilities.GetDefaultTroopTraits(this.Troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x001847A8 File Offset: 0x001829A8
		public PartyBase Party
		{
			get
			{
				return this._supplier.GetParty(this._descriptor);
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06004CAE RID: 19630 RVA: 0x001847BB File Offset: 0x001829BB
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06004CAF RID: 19631 RVA: 0x001847C3 File Offset: 0x001829C3
		public Banner Banner
		{
			get
			{
				if (this.Party.LeaderHero == null)
				{
					return this.Party.MapFaction.Banner;
				}
				return this.Party.LeaderHero.ClanBanner;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06004CB0 RID: 19632 RVA: 0x001847F4 File Offset: 0x001829F4
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x0018480F File Offset: 0x00182A0F
		public CharacterObject Troop
		{
			get
			{
				return this._supplier.GetTroop(this._descriptor);
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x00184822 File Offset: 0x00182A22
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x0018482A File Offset: 0x00182A2A
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x00184832 File Offset: 0x00182A32
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x0018483A File Offset: 0x00182A3A
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x00184842 File Offset: 0x00182A42
		BasicCharacterObject IAgentOriginBase.Troop
		{
			get
			{
				return this.Troop;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0018484A File Offset: 0x00182A4A
		public UniqueTroopDescriptor TroopDesc
		{
			get
			{
				return this._descriptor;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00184852 File Offset: 0x00182A52
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0018485A File Offset: 0x00182A5A
		public bool IsUnderPlayersCommand
		{
			get
			{
				return this.Troop == Hero.MainHero.CharacterObject || PartyBase.IsPartyUnderPlayerCommand(this.Party);
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x0018487C File Offset: 0x00182A7C
		public bool IsInSameArmyAsPlayer
		{
			get
			{
				PartyBase party = this.Party;
				MobileParty mobileParty;
				Army army;
				return party != null && (mobileParty = party.MobileParty) != null && (army = mobileParty.Army) != null && army == MobileParty.MainParty.Army && (army.LeaderParty == mobileParty || mobileParty.AttachedTo == army.LeaderParty) && (army.LeaderParty == MobileParty.MainParty || MobileParty.MainParty.AttachedTo == army.LeaderParty);
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x001848EE File Offset: 0x00182AEE
		public uint FactionColor
		{
			get
			{
				return this.Party.MapFaction.Color;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x00184900 File Offset: 0x00182B00
		public uint FactionColor2
		{
			get
			{
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x00184912 File Offset: 0x00182B12
		public int Seed
		{
			get
			{
				return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this.Troop, this.Rank);
			}
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0018492B File Offset: 0x00182B2B
		public void SetWounded()
		{
			if (!this._isRemoved)
			{
				this._supplier.OnTroopWounded(this._descriptor);
				this._isRemoved = true;
			}
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x00184950 File Offset: 0x00182B50
		public void SetKilled()
		{
			if (!this._isRemoved)
			{
				this._supplier.OnTroopKilled(this._descriptor);
				if (this.Troop.IsHero)
				{
					KillCharacterAction.ApplyByBattle(this.Troop.HeroObject, null, true);
				}
				this._isRemoved = true;
			}
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0018499C File Offset: 0x00182B9C
		public void SetRouted(bool isOrderRetreat)
		{
			if (!this._isRemoved)
			{
				this._supplier.OnTroopRouted(this._descriptor, isOrderRetreat);
				this._isRemoved = true;
			}
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x001849BF File Offset: 0x00182BBF
		public void OnAgentRemoved(float agentHealth)
		{
			if (this.Troop.IsHero)
			{
				this.Troop.HeroObject.HitPoints = MathF.Max(1, MathF.Round(agentHealth));
			}
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x001849EA File Offset: 0x00182BEA
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			this._supplier.OnTroopScoreHit(this._descriptor, victim, damage, isFatal, isTeamKill, attackerWeapon);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x00184A05 File Offset: 0x00182C05
		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x00184A0C File Offset: 0x00182C0C
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x04001553 RID: 5459
		private readonly PartyGroupTroopSupplier _supplier;

		// Token: 0x04001554 RID: 5460
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x04001555 RID: 5461
		private readonly int _rank;

		// Token: 0x04001556 RID: 5462
		private bool _isRemoved;

		// Token: 0x04001557 RID: 5463
		private bool _hasThrownWeapon;

		// Token: 0x04001558 RID: 5464
		private bool _hasHeavyArmor;

		// Token: 0x04001559 RID: 5465
		private bool _hasShield;

		// Token: 0x0400155A RID: 5466
		private bool _hasSpear;
	}
}
