using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x020004B0 RID: 1200
	public class PartyAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06004C93 RID: 19603 RVA: 0x0018432C File Offset: 0x0018252C
		// (set) Token: 0x06004C94 RID: 19604 RVA: 0x0018438D File Offset: 0x0018258D
		public PartyBase Party
		{
			get
			{
				PartyBase partyBase = this._party;
				if (this._troop.IsHero && this._troop.HeroObject.PartyBelongedTo != null && this._troop.HeroObject.PartyBelongedTo.Party != null)
				{
					partyBase = this._troop.HeroObject.PartyBelongedTo.Party;
				}
				return partyBase;
			}
			set
			{
				this._party = value;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x00184396 File Offset: 0x00182596
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x001843A0 File Offset: 0x001825A0
		public Banner Banner
		{
			get
			{
				Banner banner;
				if ((banner = this._banner) == null)
				{
					if (this.Party == null)
					{
						if (!this._troop.IsHero)
						{
							return null;
						}
						return this._troop.HeroObject.MapFaction.Banner;
					}
					else
					{
						if (this.Party.LeaderHero == null)
						{
							return this.Party.MapFaction.Banner;
						}
						banner = this.Party.LeaderHero.ClanBanner;
					}
				}
				return banner;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x00184412 File Offset: 0x00182612
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x0018441A File Offset: 0x0018261A
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06004C99 RID: 19609 RVA: 0x00184422 File Offset: 0x00182622
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x0018442A File Offset: 0x0018262A
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x00184432 File Offset: 0x00182632
		public BasicCharacterObject Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x0018443A File Offset: 0x0018263A
		// (set) Token: 0x06004C9D RID: 19613 RVA: 0x00184442 File Offset: 0x00182642
		public int Rank { get; private set; }

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x0018444C File Offset: 0x0018264C
		public bool IsUnderPlayersCommand
		{
			get
			{
				PartyBase party = this.Party;
				return (party != null && party == PartyBase.MainParty) || party.Owner == Hero.MainHero || party.MapFaction.Leader == Hero.MainHero;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x0018448C File Offset: 0x0018268C
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

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x001844FE File Offset: 0x001826FE
		public uint FactionColor
		{
			get
			{
				if (this.Party == null)
				{
					return this._troop.HeroObject.MapFaction.Color;
				}
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06004CA1 RID: 19617 RVA: 0x0018452E File Offset: 0x0018272E
		public uint FactionColor2
		{
			get
			{
				if (this.Party == null)
				{
					return this._troop.HeroObject.MapFaction.Color2;
				}
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x0018455E File Offset: 0x0018275E
		public int Seed
		{
			get
			{
				if (this.Party == null)
				{
					return 0;
				}
				return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this._troop, this.Rank);
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x00184584 File Offset: 0x00182784
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x001845A0 File Offset: 0x001827A0
		public PartyAgentOrigin(PartyBase partyBase, CharacterObject characterObject, int rank = -1, UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor), bool alwaysWounded = false, bool isInvincible = false)
		{
			this.Party = partyBase;
			this._troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._alwaysWounded = alwaysWounded;
			this._isInvincible = isInvincible;
			AgentOriginUtilities.GetDefaultTroopTraits(this.Troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x00184630 File Offset: 0x00182830
		public void SetWounded()
		{
			if (!this._isInvincible)
			{
				if (this._troop.IsHero)
				{
					this._troop.HeroObject.MakeWounded(null, KillCharacterAction.KillCharacterActionDetail.None);
				}
				if (this.Party != null)
				{
					this.Party.MemberRoster.AddToCounts(this._troop, 0, false, 1, 0, true, -1);
				}
			}
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x0018468C File Offset: 0x0018288C
		public void SetKilled()
		{
			if (!this._isInvincible)
			{
				if (this._alwaysWounded)
				{
					this.SetWounded();
					return;
				}
				if (this._troop.IsHero)
				{
					KillCharacterAction.ApplyByBattle(this._troop.HeroObject, null, true);
					return;
				}
				if (!this._troop.IsHero)
				{
					PartyBase party = this.Party;
					if (party == null)
					{
						return;
					}
					party.MemberRoster.AddToCounts(this._troop, -1, false, 0, 0, true, -1);
				}
			}
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x001846FF File Offset: 0x001828FF
		public void SetRouted(bool isOrderRetreat)
		{
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x00184704 File Offset: 0x00182904
		public void OnAgentRemoved(float agentHealth)
		{
			if (this._troop.IsHero && this._troop.HeroObject.HeroState != Hero.CharacterStates.Dead && !this._isInvincible)
			{
				this._troop.HeroObject.HitPoints = MathF.Max(1, MathF.Round(agentHealth));
			}
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x00184755 File Offset: 0x00182955
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x00184757 File Offset: 0x00182957
		public void SetBanner(Banner banner)
		{
			this._banner = banner;
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x00184760 File Offset: 0x00182960
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x04001548 RID: 5448
		private PartyBase _party;

		// Token: 0x04001549 RID: 5449
		private Banner _banner;

		// Token: 0x0400154A RID: 5450
		private CharacterObject _troop;

		// Token: 0x0400154B RID: 5451
		private bool _hasThrownWeapon;

		// Token: 0x0400154C RID: 5452
		private bool _hasHeavyArmor;

		// Token: 0x0400154D RID: 5453
		private bool _hasShield;

		// Token: 0x0400154E RID: 5454
		private bool _hasSpear;

		// Token: 0x04001550 RID: 5456
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x04001551 RID: 5457
		private readonly bool _alwaysWounded;

		// Token: 0x04001552 RID: 5458
		private readonly bool _isInvincible;
	}
}
