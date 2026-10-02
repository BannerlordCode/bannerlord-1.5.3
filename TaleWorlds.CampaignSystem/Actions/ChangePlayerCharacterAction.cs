using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C6 RID: 1222
	public class ChangePlayerCharacterAction
	{
		// Token: 0x06004D38 RID: 19768 RVA: 0x00186BB8 File Offset: 0x00184DB8
		public static void Apply(Hero hero)
		{
			Hero mainHero = Hero.MainHero;
			MobileParty mainParty = MobileParty.MainParty;
			AnchorPoint anchorPoint = new AnchorPoint(MobileParty.MainParty.Anchor);
			bool isCurrentlyAtSea = MobileParty.MainParty.IsCurrentlyAtSea;
			Game.Current.PlayerTroop = hero.CharacterObject;
			CampaignEventDispatcher.Instance.OnBeforePlayerCharacterChanged(mainHero, hero);
			bool flag;
			Campaign.Current.OnPlayerCharacterChanged(out flag);
			if (mainParty.Ships.Count > 0 && flag)
			{
				Ship ship;
				if (mainParty.MemberRoster.TotalManCount > 1 && isCurrentlyAtSea)
				{
					ship = mainParty.Ships.MinBy<Ship, float>((Ship x) => x.HitPoints);
				}
				else
				{
					ship = null;
				}
				Ship ship2 = ship;
				for (int i = mainParty.Ships.Count - 1; i >= 0; i--)
				{
					if (mainParty.Ships[i] != ship2)
					{
						ChangeShipOwnerAction.ApplyByTransferring(PartyBase.MainParty, mainParty.Ships[i]);
					}
				}
			}
			if (mainParty.IsTransitionInProgress)
			{
				mainParty.CancelNavigationTransition();
			}
			if (MobileParty.MainParty.Ships.Count > 0 && !MobileParty.MainParty.Anchor.IsValid && !MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MobileParty.MainParty.SetAnchor(anchorPoint);
			}
			if (mainParty != MobileParty.MainParty && mainParty.IsActive)
			{
				if (mainParty.MemberRoster.TotalManCount == 0)
				{
					DestroyPartyAction.Apply(null, mainParty);
				}
				else
				{
					mainParty.LordPartyComponent.ChangePartyOwner(Hero.MainHero);
				}
			}
			bool isPrisoner = Hero.MainHero.IsPrisoner;
			if (hero.IsPrisoner)
			{
				PlayerCaptivity.OnPlayerCharacterChanged();
			}
			CampaignEventDispatcher.Instance.OnPlayerCharacterChanged(mainHero, hero, MobileParty.MainParty, flag);
			PartyBase.MainParty.SetVisualAsDirty();
			mainParty.Party.SetVisualAsDirty();
			Campaign.Current.MainHeroIllDays = -1;
		}
	}
}
