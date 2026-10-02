using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator
{
	// Token: 0x02000042 RID: 66
	[OverrideView(typeof(FaceGeneratorScreen))]
	public class GauntletBodyGeneratorScreen : ScreenBase, IFaceGeneratorScreen
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000326 RID: 806 RVA: 0x000138DC File Offset: 0x00011ADC
		public IFaceGeneratorHandler Handler
		{
			get
			{
				return this._facegenLayer;
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000138E4 File Offset: 0x00011AE4
		public GauntletBodyGeneratorScreen(BasicCharacterObject character, bool openedFromMultiplayer, IFaceGeneratorCustomFilter filter)
		{
			this._facegenLayer = new BodyGeneratorView(new ControlCharacterCreationStage(this.OnExit), GameTexts.FindText("str_done", null), new ControlCharacterCreationStage(this.OnExit), GameTexts.FindText("str_cancel", null), character, openedFromMultiplayer, filter, null, null, null, null, null, null);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00013939 File Offset: 0x00011B39
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._facegenLayer.OnTick(dt);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001394E File Offset: 0x00011B4E
		public void OnExit()
		{
			ScreenManager.PopScreen();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00013955 File Offset: 0x00011B55
		protected override void OnInitialize()
		{
			base.OnInitialize();
			Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
			base.AddLayer(this._facegenLayer.GauntletLayer);
			InformationManager.HideAllMessages();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00013983 File Offset: 0x00011B83
		protected override void OnFinalize()
		{
			base.OnFinalize();
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000139A7 File Offset: 0x00011BA7
		protected override void OnActivate()
		{
			base.OnActivate();
			base.AddLayer(this._facegenLayer.SceneLayer);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000139C0 File Offset: 0x00011BC0
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._facegenLayer.OnFinalize();
			LoadingWindow.EnableGlobalLoadingWindow();
			MBInformationManager.HideInformations();
			Mission mission = Mission.Current;
			if (mission != null)
			{
				foreach (Agent agent in mission.Agents)
				{
					agent.EquipItemsFromSpawnEquipment(false, false, false, 0);
					agent.UpdateAgentProperties();
				}
			}
		}

		// Token: 0x040001AA RID: 426
		private const int ViewOrderPriority = 15;

		// Token: 0x040001AB RID: 427
		private readonly BodyGeneratorView _facegenLayer;
	}
}
