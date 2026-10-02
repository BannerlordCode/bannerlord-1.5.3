using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x020004AD RID: 1197
	public class SafePassageBarterable : Barterable
	{
		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x001838B3 File Offset: 0x00181AB3
		public override string StringID
		{
			get
			{
				return "safe_passage_barterable";
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x001838BC File Offset: 0x00181ABC
		public override TextObject Name
		{
			get
			{
				TextObject textObject;
				if (this._otherHero != null)
				{
					StringHelpers.SetCharacterProperties("HERO", this._otherHero.CharacterObject, null, false);
					textObject = new TextObject("{=BJbbahYe}Let {HERO.NAME} Go", null);
				}
				else
				{
					textObject = new TextObject("{=QKNWsJRb}Let {PARTY} Go", null);
					textObject.SetTextVariable("PARTY", this._otherParty.Name);
				}
				return textObject;
			}
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x0018391B File Offset: 0x00181B1B
		public SafePassageBarterable(Hero originalOwner, Hero otherHero, PartyBase ownerParty, PartyBase otherParty)
			: base(originalOwner, ownerParty)
		{
			this._otherHero = otherHero;
			this._otherParty = otherParty;
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x00183934 File Offset: 0x00181B34
		public override int GetUnitValueForFaction(IFaction faction)
		{
			float num = MathF.Clamp(this.GetPlayerStrengthRatioInEncounter(), 0f, 1f);
			int num2 = (int)MathF.Clamp((float)(Hero.MainHero.Gold + PartyBase.MainParty.ItemRoster.Sum<ItemRosterElement>((ItemRosterElement t) => t.EquipmentElement.Item.Value * t.Amount)), 0f, 2.1474836E+09f);
			float num3 = ((num < 1f) ? (0.05f + (1f - num) * 0.2f) : 0.1f);
			float num4 = ((faction.Leader == null) ? 1f : MathF.Clamp((50f + (float)faction.Leader.GetRelation(this._otherHero)) / 50f, 0.05f, 1.1f));
			if (!PlayerEncounter.EncounteredParty.IsMobile || !PlayerEncounter.EncounteredParty.MobileParty.IsBandit)
			{
				num2 += 3000 + (int)(Hero.MainHero.Clan.Renown * 50f);
				num3 *= 1.5f;
			}
			if (MobileParty.MainParty.MapEvent != null || MobileParty.MainParty.SiegeEvent != null)
			{
				num3 *= 1.2f;
			}
			int num5 = (int)((float)num2 * num3 + 1000f);
			MobileParty mobileParty = PlayerEncounter.EncounteredParty.MobileParty;
			if (mobileParty != null && mobileParty.IsBandit)
			{
				num5 /= 8;
				if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.SweetTalker) && !MobileParty.MainParty.IsCurrentlyAtSea)
				{
					num5 += MathF.Round((float)num5 * DefaultPerks.Roguery.SweetTalker.PrimaryBonus);
				}
			}
			else
			{
				num5 /= 2;
				num5 += (int)(0.3f * num3 * Campaign.Current.Models.ValuationModel.GetMilitaryValueOfParty(this._otherParty.MobileParty));
				num5 += (int)(0.3f * num3 * Campaign.Current.Models.ValuationModel.GetValueOfHero(this._otherHero));
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Trade.MarketDealer))
			{
				num5 += MathF.Round((float)num5 * DefaultPerks.Trade.MarketDealer.PrimaryBonus);
			}
			Hero originalOwner = base.OriginalOwner;
			if (faction != ((originalOwner != null) ? originalOwner.Clan : null))
			{
				Hero originalOwner2 = base.OriginalOwner;
				if (faction != ((originalOwner2 != null) ? originalOwner2.MapFaction : null) && faction != base.OriginalParty.MapFaction)
				{
					Hero otherHero = this._otherHero;
					if (faction != ((otherHero != null) ? otherHero.Clan : null))
					{
						Hero otherHero2 = this._otherHero;
						if (faction != ((otherHero2 != null) ? otherHero2.MapFaction : null) && faction != this._otherParty.MapFaction)
						{
							return num5;
						}
					}
					return (int)(0.9f * (float)num5);
				}
			}
			return -(int)((float)num5 / (num4 * num4));
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x00183BD8 File Offset: 0x00181DD8
		public float GetPlayerStrengthRatioInEncounter()
		{
			List<MobileParty> list = new List<MobileParty>();
			List<MobileParty> list2 = new List<MobileParty>();
			PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list, list2);
			if (!list.Contains(MobileParty.MainParty))
			{
				list.Add(MobileParty.MainParty);
			}
			if (!list2.Contains(base.OriginalParty.MobileParty))
			{
				list2.Add(base.OriginalParty.MobileParty);
			}
			float num = 0f;
			float num2 = 0f;
			MapEvent.PowerCalculationContext powerCalculationContext = (PlayerEncounter.IsNavalEncounter() ? MapEvent.PowerCalculationContext.SeaBattle : MapEvent.PowerCalculationContext.PlainBattle);
			foreach (MobileParty mobileParty in list)
			{
				if (mobileParty != null)
				{
					num += mobileParty.Party.GetCustomStrength(BattleSideEnum.Defender, powerCalculationContext);
				}
				else
				{
					Debug.FailedAssert("Player side party null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BarterSystem\\Barterables\\SafePassageBarterable.cs", "GetPlayerStrengthRatioInEncounter", 156);
				}
			}
			foreach (MobileParty mobileParty2 in list2)
			{
				if (mobileParty2 != null)
				{
					num2 += mobileParty2.Party.GetCustomStrength(BattleSideEnum.Attacker, powerCalculationContext);
				}
				else
				{
					Debug.FailedAssert("Opponent side party null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BarterSystem\\Barterables\\SafePassageBarterable.cs", "GetPlayerStrengthRatioInEncounter", 168);
				}
			}
			if (num2 <= 0f)
			{
				num2 = 1E-05f;
			}
			return num / num2;
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x00183D3C File Offset: 0x00181F3C
		public override bool IsCompatible(Barterable barterable)
		{
			return true;
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x00183D3F File Offset: 0x00181F3F
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x00183D44 File Offset: 0x00181F44
		public override void Apply()
		{
			if (PlayerEncounter.Current != null)
			{
				List<MobileParty> list = new List<MobileParty>();
				List<MobileParty> list2 = new List<MobileParty>();
				PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list, list2);
				if (!list2.Contains(base.OriginalParty.MobileParty))
				{
					list2.Add(base.OriginalParty.MobileParty);
				}
				PartyBase originalParty = base.OriginalParty;
				if (((originalParty != null) ? originalParty.SiegeEvent : null) != null && base.OriginalParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(base.OriginalParty, MapEvent.BattleTypes.Siege) && this._otherParty != null && base.OriginalParty.SiegeEvent.BesiegedSettlement.HasInvolvedPartyForEventType(this._otherParty, MapEvent.BattleTypes.Siege))
				{
					if (base.OriginalParty.SiegeEvent.BesiegedSettlement.MapFaction == Hero.MainHero.MapFaction)
					{
						GainKingdomInfluenceAction.ApplyForSiegeSafePassageBarter(MobileParty.MainParty, -10f);
					}
					Campaign.Current.GameMenuManager.SetNextMenu("menu_siege_safe_passage_accepted");
					PlayerSiege.FinalizePlayerSiege();
					using (List<MobileParty>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MobileParty mobileParty = enumerator.Current;
							mobileParty.Ai.SetDoNotAttackMainParty(32);
						}
						return;
					}
				}
				foreach (MobileParty mobileParty2 in list2)
				{
					mobileParty2.Ai.SetDoNotAttackMainParty(32);
					mobileParty2.SetMoveModeHold();
					mobileParty2.Ai.ForceDefaultBehaviorUpdate();
					mobileParty2.Ai.SetInitiative(0f, 0.8f, 8f);
				}
				PlayerEncounter.LeaveEncounter = true;
				if (MobileParty.MainParty.SiegeEvent != null && MobileParty.MainParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(PartyBase.MainParty, MapEvent.BattleTypes.Siege))
				{
					MobileParty.MainParty.BesiegerCamp = null;
				}
				PartyBase originalParty2 = base.OriginalParty;
				bool flag;
				if (originalParty2 == null)
				{
					flag = null != null;
				}
				else
				{
					MobileParty mobileParty3 = originalParty2.MobileParty;
					flag = ((mobileParty3 != null) ? mobileParty3.Ai.AiBehaviorPartyBase : null) != null;
				}
				if (flag && base.OriginalParty != PartyBase.MainParty)
				{
					base.OriginalParty.MobileParty.SetMoveModeHold();
					if (base.OriginalParty.MobileParty.Army != null && MobileParty.MainParty.Army != base.OriginalParty.MobileParty.Army)
					{
						base.OriginalParty.MobileParty.Army.LeaderParty.SetMoveModeHold();
						return;
					}
				}
			}
			else
			{
				Debug.FailedAssert("Can not find player encounter for safe passage barterable", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BarterSystem\\Barterables\\SafePassageBarterable.cs", "Apply", 256);
			}
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00183FD8 File Offset: 0x001821D8
		internal static void AutoGeneratedStaticCollectObjectsSafePassageBarterable(object o, List<object> collectedObjects)
		{
			((SafePassageBarterable)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x00183FE6 File Offset: 0x001821E6
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x04001541 RID: 5441
		private readonly Hero _otherHero;

		// Token: 0x04001542 RID: 5442
		private readonly PartyBase _otherParty;
	}
}
