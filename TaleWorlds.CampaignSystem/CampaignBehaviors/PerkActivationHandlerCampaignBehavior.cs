using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044F RID: 1103
	public class PerkActivationHandlerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060046C2 RID: 18114 RVA: 0x00159836 File Offset: 0x00157A36
		public override void RegisterEvents()
		{
			CampaignEvents.PerkOpenedEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerkOpened));
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x00159850 File Offset: 0x00157A50
		private void OnPerkOpened(Hero hero, PerkObject perk)
		{
			if (hero != null)
			{
				if (perk == DefaultPerks.OneHanded.Trainer || perk == DefaultPerks.OneHanded.UnwaveringDefense || perk == DefaultPerks.TwoHanded.ThickHides || perk == DefaultPerks.Athletics.WellBuilt || perk == DefaultPerks.Medicine.PreventiveMedicine)
				{
					hero.HitPoints += (int)perk.PrimaryBonus;
				}
				else if (perk == DefaultPerks.Crafting.VigorousSmith)
				{
					int num = 1;
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, num, false);
				}
				else if (perk == DefaultPerks.Crafting.StrongSmith)
				{
					int num2 = 1;
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, num2, false);
				}
				else if (perk == DefaultPerks.Crafting.EnduringSmith)
				{
					int num3 = 1;
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, num3, false);
				}
				else if (perk == DefaultPerks.Crafting.WeaponMasterSmith)
				{
					int num4 = 1;
					int focus = hero.HeroDeveloper.GetFocus(DefaultSkills.OneHanded);
					int focus2 = hero.HeroDeveloper.GetFocus(DefaultSkills.TwoHanded);
					if (focus < Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill)
					{
						hero.HeroDeveloper.AddFocus(DefaultSkills.OneHanded, num4, false);
					}
					if (focus2 < Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill)
					{
						hero.HeroDeveloper.AddFocus(DefaultSkills.TwoHanded, num4, false);
					}
				}
				else if (perk == DefaultPerks.Athletics.Durable)
				{
					int num5 = 1;
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, num5, false);
				}
				else if (perk == DefaultPerks.Athletics.Steady)
				{
					int num6 = 1;
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, num6, false);
				}
				else if (perk == DefaultPerks.Athletics.Strong)
				{
					int num7 = 1;
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, num7, false);
				}
				if (hero == Hero.MainHero && (perk == DefaultPerks.OneHanded.Prestige || perk == DefaultPerks.TwoHanded.Hope || perk == DefaultPerks.Athletics.ImposingStature || perk == DefaultPerks.Bow.MerryMen || perk == DefaultPerks.Tactics.HordeLeader || perk == DefaultPerks.Scouting.MountedScouts || perk == DefaultPerks.Leadership.Authority || perk == DefaultPerks.Leadership.LeaderOfMasses || perk == DefaultPerks.Leadership.UltimateLeader))
				{
					PartyBase.MainParty.MemberRoster.UpdateVersion();
				}
				if (perk.PrimaryRole == PartyRole.Captain)
				{
					hero.UpdatePowerModifier();
				}
			}
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x00159A54 File Offset: 0x00157C54
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
