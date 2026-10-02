using System;
using System.Collections.Generic;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000035 RID: 53
	[OverrideView(typeof(MapEscapeMenuView))]
	public class GauntletMapEscapeMenuView : MapView
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000FB7B File Offset: 0x0000DD7B
		public GauntletMapEscapeMenuView(List<EscapeMenuItemVM> items)
		{
			this._menuItems = items;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000FB8C File Offset: 0x0000DD8C
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._escapeMenuDatasource = new EscapeMenuVM(this._menuItems, null);
			this.InitializeCampaignStartingOptionsInfo();
			base.Layer = new GauntletLayer("MapEscapeMenu", 4400, false)
			{
				IsFocusLayer = true
			};
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._escapeMenuMovie = this._layerAsGauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MapScreen.AddLayer(base.Layer);
			base.MapScreen.PauseAmbientSounds();
			ScreenManager.TrySetFocus(base.Layer);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000FC54 File Offset: 0x0000DE54
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.HandleTick(dt);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000FC64 File Offset: 0x0000DE64
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			this.HandleTick(dt);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000FC74 File Offset: 0x0000DE74
		private void HandleTick(float dt)
		{
			this._escapeMenuDatasource.Tick(dt);
			if (base.Layer.Input.IsHotKeyReleased("ToggleEscapeMenu") || base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				MapScreen.Instance.CloseEscapeMenu();
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000FCC5 File Offset: 0x0000DEC5
		protected override bool IsEscaped()
		{
			return base.Layer.Input.IsHotKeyReleased("ToggleEscapeMenu");
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		protected override void OnFinalize()
		{
			base.OnFinalize();
			base.Layer.InputRestrictions.ResetInputRestrictions();
			base.MapScreen.RemoveLayer(base.Layer);
			base.MapScreen.RestartAmbientSounds();
			ScreenManager.TryLoseFocus(base.Layer);
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000FD42 File Offset: 0x0000DF42
		protected override TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.EscapeMenu;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000FD48 File Offset: 0x0000DF48
		private void InitializeCampaignStartingOptionsInfo()
		{
			Campaign campaign = Campaign.Current;
			bool flag;
			if (campaign == null)
			{
				flag = null != null;
			}
			else
			{
				CampaignOptions options = campaign.Options;
				flag = ((options != null) ? options.AdvancedStartOptionsData : null) != null;
			}
			if (!flag)
			{
				return;
			}
			if (campaign.GameTypeStringId == "CampaignStoryMode")
			{
				return;
			}
			AdvancedStartOptionsData advancedStartData = campaign.AdvancedStartData;
			List<TextObject> scenarioParameters = GauntletMapEscapeMenuView.GetScenarioParameters(advancedStartData);
			string text;
			if (scenarioParameters.Count > 0)
			{
				scenarioParameters.Insert(0, advancedStartData.GetSelectedScenarioName());
				text = GameTexts.GameTextHelper.MergeTextObjectsWithComma(scenarioParameters, false).ToString();
			}
			else
			{
				text = advancedStartData.GetSelectedScenarioName().ToString();
			}
			this._escapeMenuDatasource.InitializeCampaignStartingOptionsInfo(text, this.GetRawSeed(advancedStartData));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000FDDB File Offset: 0x0000DFDB
		private uint GetRawSeed(AdvancedStartOptionsData startOptions)
		{
			if (startOptions.HasValue("Seed"))
			{
				return startOptions.GetValue<uint>("Seed");
			}
			return Campaign.Current.Options.Seed;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000FE08 File Offset: 0x0000E008
		private static List<TextObject> GetScenarioParameters(AdvancedStartOptionsData startOptions)
		{
			string scenario = startOptions.GetScenario();
			List<TextObject> list = new List<TextObject>();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(scenario);
			if (num <= 830264547U)
			{
				if (num != 217951011U)
				{
					if (num != 801936231U)
					{
						if (num == 830264547U)
						{
							if (scenario == "InvasionId")
							{
								GauntletMapEscapeMenuView.AddOptionValueTo(startOptions, list, "InvasionScenarioFactionId");
							}
						}
					}
					else if (!(scenario == "nordinvasion"))
					{
					}
				}
				else if (scenario == "twofactionwar")
				{
					GauntletMapEscapeMenuView.AddOptionValueTo(startOptions, list, "TwoFactionWarFaction1Id");
					GauntletMapEscapeMenuView.AddOptionValueTo(startOptions, list, "TwoFactionWarFaction2Id");
				}
			}
			else if (num <= 2830372097U)
			{
				if (num != 1399573479U)
				{
					if (num == 2830372097U)
					{
						if (scenario == "alternativecalradia")
						{
							GauntletMapEscapeMenuView.AddOptionValueTo(startOptions, list, "AlternativeCalradiaVariantId");
						}
					}
				}
				else if (scenario == "LastStand")
				{
					GauntletMapEscapeMenuView.AddOptionValueTo(startOptions, list, "LastStandKingdomId");
				}
			}
			else if (num != 2913447899U)
			{
				if (num == 3855596206U)
				{
					if (scenario == "unitedempire")
					{
						GauntletMapEscapeMenuView.AddOptionValueTo(startOptions, list, "UnitedEmpireUnifierKingdomId");
					}
				}
			}
			else if (!(scenario == "none"))
			{
			}
			return list;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000FF49 File Offset: 0x0000E149
		private static void AddOptionValueTo(AdvancedStartOptionsData startOptions, List<TextObject> list, string optionId)
		{
			list.Add(startOptions.GetDisplayName(optionId));
		}

		// Token: 0x040000E7 RID: 231
		private const string StoryModeGameTypeStringId = "CampaignStoryMode";

		// Token: 0x040000E8 RID: 232
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000E9 RID: 233
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x040000EA RID: 234
		private GauntletMovieIdentifier _escapeMenuMovie;

		// Token: 0x040000EB RID: 235
		private readonly List<EscapeMenuItemVM> _menuItems;
	}
}
