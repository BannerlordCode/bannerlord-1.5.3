using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.MapSiege;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000022 RID: 34
	[Tutorial("BombardmentStep1")]
	public class BombardmentStep1Tutorial : TutorialItemBase
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x0000339E File Offset: 0x0000159E
		public BombardmentStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = true;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000033BF File Offset: 0x000015BF
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerSelectedSiegeEngine || this._isGameMenuChangedAfterActivation;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000033D1 File Offset: 0x000015D1
		public override void OnPlayerStartEngineConstruction(PlayerStartEngineConstructionEvent obj)
		{
			this._playerSelectedSiegeEngine = true;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000033DA File Offset: 0x000015DA
		public override void OnGameMenuOptionSelected(GameMenuOption obj)
		{
			base.OnGameMenuOptionSelected(obj);
			if (this._isActivated)
			{
				this._isGameMenuChangedAfterActivation = true;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000033F2 File Offset: 0x000015F2
		public override void OnGameMenuOpened(MenuCallbackArgs obj)
		{
			base.OnGameMenuOpened(obj);
			if (this._isActivated)
			{
				this._isGameMenuChangedAfterActivation = true;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000340A File Offset: 0x0000160A
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003410 File Offset: 0x00001610
		public override bool IsConditionsMetForActivation()
		{
			MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
			bool flag5;
			if (((currentMenuContext != null) ? currentMenuContext.GameMenu.StringId : null) == "menu_siege_strategies")
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				bool flag;
				if (playerSiegeEvent == null)
				{
					flag = false;
				}
				else
				{
					SiegeEvent.SiegeEnginesContainer siegeEngines = playerSiegeEvent.GetSiegeEventSide(PlayerSiege.PlayerSide).SiegeEngines;
					bool? flag2;
					if (siegeEngines == null)
					{
						flag2 = null;
					}
					else
					{
						SiegeEvent.SiegeEngineConstructionProgress siegePreparations = siegeEngines.SiegePreparations;
						flag2 = ((siegePreparations != null) ? new bool?(siegePreparations.IsActive) : null);
					}
					bool? flag3 = flag2;
					bool flag4 = true;
					flag = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
				}
				if (flag)
				{
					flag5 = TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
					goto IL_0092;
				}
			}
			flag5 = false;
			IL_0092:
			this._isActivated = flag5;
			return this._isActivated;
		}

		// Token: 0x0400002B RID: 43
		private bool _playerSelectedSiegeEngine;

		// Token: 0x0400002C RID: 44
		private bool _isGameMenuChangedAfterActivation;

		// Token: 0x0400002D RID: 45
		private bool _isActivated;
	}
}
