using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x020004B2 RID: 1202
	public class SimpleAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x00184A14 File Offset: 0x00182C14
		public BasicCharacterObject Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06004CC6 RID: 19654 RVA: 0x00184A1C File Offset: 0x00182C1C
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06004CC7 RID: 19655 RVA: 0x00184A24 File Offset: 0x00182C24
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x00184A2C File Offset: 0x00182C2C
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06004CC9 RID: 19657 RVA: 0x00184A34 File Offset: 0x00182C34
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06004CCA RID: 19658 RVA: 0x00184A3C File Offset: 0x00182C3C
		public bool IsUnderPlayersCommand
		{
			get
			{
				PartyBase party = this.Party;
				return party != null && (party == PartyBase.MainParty || party.Owner == Hero.MainHero || party.MapFaction.Leader == Hero.MainHero);
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06004CCB RID: 19659 RVA: 0x00184A80 File Offset: 0x00182C80
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

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06004CCC RID: 19660 RVA: 0x00184AF2 File Offset: 0x00182CF2
		public uint FactionColor
		{
			get
			{
				if (this.Party != null)
				{
					return this.Party.MapFaction.Color;
				}
				if (this._troop.IsHero)
				{
					return this._troop.HeroObject.MapFaction.Color;
				}
				return 0U;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06004CCD RID: 19661 RVA: 0x00184B31 File Offset: 0x00182D31
		public uint FactionColor2
		{
			get
			{
				if (this.Party != null)
				{
					return this.Party.MapFaction.Color2;
				}
				if (this._troop.IsHero)
				{
					return this._troop.HeroObject.MapFaction.Color2;
				}
				return 0U;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06004CCE RID: 19662 RVA: 0x00184B70 File Offset: 0x00182D70
		public int Seed
		{
			get
			{
				if (this.Party != null)
				{
					return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this._troop, this.Rank);
				}
				return CharacterHelper.GetDefaultFaceSeed(this._troop, this.Rank);
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06004CCF RID: 19663 RVA: 0x00184BA3 File Offset: 0x00182DA3
		public PartyBase Party
		{
			get
			{
				if (!this._troop.IsHero || this._troop.HeroObject.PartyBelongedTo == null)
				{
					return null;
				}
				return this._troop.HeroObject.PartyBelongedTo.Party;
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x00184BDB File Offset: 0x00182DDB
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06004CD1 RID: 19665 RVA: 0x00184BE3 File Offset: 0x00182DE3
		public Banner Banner
		{
			get
			{
				return this._banner;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06004CD2 RID: 19666 RVA: 0x00184BEB File Offset: 0x00182DEB
		// (set) Token: 0x06004CD3 RID: 19667 RVA: 0x00184BF3 File Offset: 0x00182DF3
		public int Rank { get; private set; }

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06004CD4 RID: 19668 RVA: 0x00184BFC File Offset: 0x00182DFC
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x00184C0C File Offset: 0x00182E0C
		public SimpleAgentOrigin(BasicCharacterObject troop, int rank = -1, Banner banner = null, UniqueTroopDescriptor descriptor = default(UniqueTroopDescriptor))
		{
			this._troop = (CharacterObject)troop;
			this._descriptor = descriptor;
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._banner = banner;
			AgentOriginUtilities.GetDefaultTroopTraits(this._troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x00184C74 File Offset: 0x00182E74
		public void SetWounded()
		{
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x00184C76 File Offset: 0x00182E76
		public void SetKilled()
		{
			if (this._troop.IsHero)
			{
				KillCharacterAction.ApplyByBattle(this._troop.HeroObject, null, true);
			}
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x00184C97 File Offset: 0x00182E97
		public void SetRouted(bool isOrderRetreat)
		{
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x00184C99 File Offset: 0x00182E99
		public void OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00184C9C File Offset: 0x00182E9C
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject formationCaptain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			if (isTeamKill)
			{
				CharacterObject troop = this._troop;
				ExplainedNumber xpFromHit = Campaign.Current.Models.CombatXpModel.GetXpFromHit(troop, (CharacterObject)formationCaptain, (CharacterObject)victim, this.Party, damage, isFatal, CombatXpModel.MissionTypeEnum.Battle);
				if (troop.IsHero && attackerWeapon != null)
				{
					SkillObject skillForWeapon = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(attackerWeapon, false);
					troop.HeroObject.AddSkillXp(skillForWeapon, (float)xpFromHit.RoundedResultNumber);
				}
			}
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x00184D18 File Offset: 0x00182F18
		public void SetBanner(Banner banner)
		{
			this._banner = banner;
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x00184D21 File Offset: 0x00182F21
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x0400155B RID: 5467
		private CharacterObject _troop;

		// Token: 0x0400155C RID: 5468
		private bool _hasThrownWeapon;

		// Token: 0x0400155D RID: 5469
		private bool _hasHeavyArmor;

		// Token: 0x0400155E RID: 5470
		private bool _hasShield;

		// Token: 0x0400155F RID: 5471
		private bool _hasSpear;

		// Token: 0x04001560 RID: 5472
		private Banner _banner;

		// Token: 0x04001562 RID: 5474
		private UniqueTroopDescriptor _descriptor;
	}
}
