using System;
using System.Collections.Generic;
using SandBox;
using SandBox.View;
using SandBox.View.Map.Managers;
using SandBox.View.Map.Visuals;
using StoryMode.Extensions;
using StoryMode.View.Permissions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AdvancedStartOptions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace StoryMode.View
{
	// Token: 0x02000003 RID: 3
	public class StoryModeViewSubModule : MBSubModuleBase
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002056 File Offset: 0x00000256
		public override void OnGameInitializationFinished(Game game)
		{
			base.OnGameInitializationFinished(game);
			StoryModePermissionsSystem.OnInitialize();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002064 File Offset: 0x00000264
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			StoryModePermissionsSystem.OnUnload();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002074 File Offset: 0x00000274
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			TextObject coreContentDisabledReason = new TextObject("{=V8BXjyYq}Disabled during installation.", null);
			Module.CurrentModule.AddInitialStateOption(new InitialStateOption("StoryModeNewGame", new TextObject("{=sf_menu_storymode_new_game}New Campaign", null), 2, delegate
			{
				this.StartGame();
			}, () => new ValueTuple<bool, TextObject>(Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason), null, null));
			Module.CurrentModule.ImguiProfilerTick += this.OnImguiProfilerTick;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F5 File Offset: 0x000002F5
		protected virtual void FillDataForCampaign()
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F7 File Offset: 0x000002F7
		protected override void OnSubModuleUnloaded()
		{
			Module.CurrentModule.ImguiProfilerTick -= this.OnImguiProfilerTick;
			base.OnSubModuleUnloaded();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002115 File Offset: 0x00000315
		public override void OnSubModuleDeactivated()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002117 File Offset: 0x00000317
		public override void OnSubModuleActivated()
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002119 File Offset: 0x00000319
		private void StartGame()
		{
			this._startedStoryMode = true;
			MBGameManager.StartNewGame(new SandBoxGameManager(() => new CampaignStoryMode(CampaignGameMode.Campaign, new AdvancedStartOptionsData())));
			this._startedStoryMode = false;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002154 File Offset: 0x00000354
		protected override void OnBeforeGameStart(MBGameManager mbGameManager, List<string> disabledModules)
		{
			SandBoxGameManager sandBoxGameManager;
			if ((sandBoxGameManager = mbGameManager as SandBoxGameManager) != null && (sandBoxGameManager.LoadingSavedGame ? (!sandBoxGameManager.MetaData.HasStoryMode()) : (!this._startedStoryMode)))
			{
				disabledModules.Add("StoryMode");
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000219C File Offset: 0x0000039C
		private void OnImguiProfilerTick()
		{
			if (Campaign.Current == null)
			{
				return;
			}
			List<MobileParty> all = MobileParty.All;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			List<EntityVisualManagerBase<PartyBase>> components = SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents<EntityVisualManagerBase<PartyBase>>();
			foreach (MobileParty mobileParty in all)
			{
				if (!mobileParty.IsMilitia && !mobileParty.IsGarrison)
				{
					if (mobileParty.IsVisible)
					{
						num++;
					}
					MapEntityVisual<PartyBase> mapEntityVisual = null;
					foreach (EntityVisualManagerBase<PartyBase> entityVisualManagerBase in components)
					{
						MapEntityVisual<PartyBase> visualOfEntity = entityVisualManagerBase.GetVisualOfEntity(PartyBase.MainParty);
						if (visualOfEntity != null)
						{
							mapEntityVisual = visualOfEntity;
						}
					}
					if (mapEntityVisual != null)
					{
						MobilePartyVisual mobilePartyVisual;
						if ((mobilePartyVisual = mapEntityVisual as MobilePartyVisual) != null)
						{
							if (mobilePartyVisual.HumanAgentVisuals != null)
							{
								num2++;
							}
							if (mobilePartyVisual.MountAgentVisuals != null)
							{
								num2++;
							}
							if (mobilePartyVisual.CaravanMountAgentVisuals != null)
							{
								num2++;
							}
						}
						num3++;
					}
				}
			}
			Imgui.BeginMainThreadScope();
			Imgui.Begin("Bannerlord Campaign Statistics");
			Imgui.Columns(2, "", true);
			Imgui.Text("Name");
			Imgui.NextColumn();
			Imgui.Text("Count");
			Imgui.NextColumn();
			Imgui.Separator();
			Imgui.Text("Total Mobile Party");
			Imgui.NextColumn();
			Imgui.Text(num3.ToString());
			Imgui.NextColumn();
			Imgui.Text("Visible Mobile Party");
			Imgui.NextColumn();
			Imgui.Text(num.ToString());
			Imgui.NextColumn();
			Imgui.Text("Total Agent Visuals");
			Imgui.NextColumn();
			Imgui.Text(num2.ToString());
			Imgui.NextColumn();
			Imgui.End();
			Imgui.EndMainThreadScope();
		}

		// Token: 0x04000001 RID: 1
		private bool _startedStoryMode;
	}
}
