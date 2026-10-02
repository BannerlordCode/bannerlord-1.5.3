using System;
using SandBox.AdvancedStartOptions;
using SandBox.View;
using SandBox.ViewModelCollection.CampaignStartingOptions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI
{
	// Token: 0x02000006 RID: 6
	[OverrideView(typeof(CampaignAdvancedStartingOptionsView))]
	public class GauntletCampaignStartingOptionsView : GlobalLayer
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0000263C File Offset: 0x0000083C
		public GauntletCampaignStartingOptionsView(AdvancedStartOptions startOptions, Action<AdvancedStartOptions> onConfirm, Action onClose)
		{
			MBTextManager.SetTextVariable("newline", "\n", false);
			this._dataSource = new CampaignStartingOptionsVM(startOptions, new Action<AdvancedStartOptions>(this.OnConfirm), new Action(this.Close));
			this._onConfirm = onConfirm;
			this._onClose = onClose;
			GauntletLayer gauntletLayer = new GauntletLayer("CampaignStartingOptions", 11, false);
			gauntletLayer.LoadMovie("CampaignStartingOptionsScreen", this._dataSource);
			base.Layer = gauntletLayer;
			base.Layer.IsFocusLayer = true;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetRandomizeInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Randomize"));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002748 File Offset: 0x00000948
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._dataSource != null)
			{
				if (!(ScreenManager.TopScreen is GauntletInitialScreen))
				{
					this._dataSource.ExecuteCancel();
					return;
				}
				ScreenManager.TrySetFocus(base.Layer);
				if (base.Layer.Input.IsHotKeyReleased("Confirm"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteConfirm();
					return;
				}
				if (base.Layer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteCancel();
					return;
				}
				if (base.Layer.Input.IsHotKeyReleased("Randomize"))
				{
					StartingOptionVM focusedOption = this._dataSource.FocusedOption;
					if (focusedOption != null && focusedOption.AllowRandomization && !this._dataSource.FocusedOption.IsDisabled)
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.FocusedOption.ExecuteRandomize();
					}
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002840 File Offset: 0x00000A40
		private void OnConfirm(AdvancedStartOptions options)
		{
			Action<AdvancedStartOptions> onConfirm = this._onConfirm;
			if (onConfirm != null)
			{
				onConfirm(options);
			}
			ScreenManager.RemoveGlobalLayer(this, true);
			CampaignStartingOptionsVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			MBGameManager.StartNewGame(new SandBoxGameManager(() => new Campaign(CampaignGameMode.Campaign, options.GetChangedOptions())));
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000289F File Offset: 0x00000A9F
		private void Close()
		{
			Action onClose = this._onClose;
			if (onClose != null)
			{
				onClose();
			}
			ScreenManager.RemoveGlobalLayer(this, true);
			CampaignStartingOptionsVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnFinalize();
		}

		// Token: 0x0400000A RID: 10
		private readonly CampaignStartingOptionsVM _dataSource;

		// Token: 0x0400000B RID: 11
		private readonly Action<AdvancedStartOptions> _onConfirm;

		// Token: 0x0400000C RID: 12
		private readonly Action _onClose;
	}
}
