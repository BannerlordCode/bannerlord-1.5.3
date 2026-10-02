using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Map.Navigation
{
	// Token: 0x0200006B RID: 107
	public static class MapNavigationHelper
	{
		// Token: 0x06000497 RID: 1175 RVA: 0x000250DC File Offset: 0x000232DC
		public static InquiryData GetUnsavedChangedInquiry(Action openNewScreenAction)
		{
			return new InquiryData(string.Empty, GameTexts.FindText("str_unsaved_changes", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				MapNavigationHelper.ApplyCurrentChanges();
				MapNavigationHelper.SwitchToANewScreen(openNewScreenAction);
			}, delegate
			{
				MapNavigationHelper.SwitchToANewScreen(openNewScreenAction);
			}, "", 0f, null, null, null);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00025158 File Offset: 0x00023358
		public static InquiryData GetUnapplicableChangedInquiry()
		{
			return new InquiryData(string.Empty, GameTexts.FindText("str_unapplicable_changes", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), null, null, "", 0f, null, null, null);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000251B0 File Offset: 0x000233B0
		public static bool IsMapTopScreen()
		{
			return ScreenManager.TopScreen is MapScreen;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000251C0 File Offset: 0x000233C0
		public static bool IsNavigationBarEnabled(MapNavigationHandler handler)
		{
			if (Hero.MainHero != null)
			{
				Hero mainHero = Hero.MainHero;
				if (mainHero == null || !mainHero.IsDead)
				{
					Campaign campaign = Campaign.Current;
					if (campaign == null || !campaign.SaveHandler.IsSaving)
					{
						if (handler != null && handler.IsNavigationLocked)
						{
							return false;
						}
						if (PlayerEncounter.CurrentBattleSimulation != null)
						{
							return false;
						}
						MapScreen mapScreen;
						if ((mapScreen = ScreenManager.TopScreen as MapScreen) != null && (mapScreen.IsInArmyManagement || mapScreen.IsMarriageOfferPopupActive || mapScreen.IsHeirSelectionPopupActive || mapScreen.IsMapCheatsActive || mapScreen.IsMapIncidentActive || mapScreen.EncyclopediaScreenManager.IsEncyclopediaOpen))
						{
							return false;
						}
						if (handler != null && handler.IsEscapeMenuActive)
						{
							return false;
						}
						INavigationElement[] elements = handler.GetElements();
						for (int i = 0; i < elements.Length; i++)
						{
							if (elements[i].IsLockingNavigation)
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0002528C File Offset: 0x0002348C
		private static void ApplyCurrentChanges()
		{
			IChangeableScreen changeableScreen;
			if ((changeableScreen = ScreenManager.TopScreen as IChangeableScreen) != null && changeableScreen.AnyUnsavedChanges())
			{
				if (changeableScreen.CanChangesBeApplied())
				{
					changeableScreen.ApplyChanges();
					return;
				}
				changeableScreen.ResetChanges();
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000252C4 File Offset: 0x000234C4
		public static void SwitchToANewScreen(Action openNewScreenAction)
		{
			if (!MapNavigationHelper.IsMapTopScreen())
			{
				Game.Current.GameStateManager.PopState(0);
			}
			if (openNewScreenAction != null)
			{
				openNewScreenAction();
			}
		}
	}
}
