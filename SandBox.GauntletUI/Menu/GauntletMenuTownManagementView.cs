using System;
using System.Linq;
using SandBox.View.Map;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x0200002A RID: 42
	[OverrideView(typeof(MenuTownManagementView))]
	public class GauntletMenuTownManagementView : MenuView
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0000D46C File Offset: 0x0000B66C
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new TownManagementVM();
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_town_management");
			base.Layer = new GauntletLayer("MapTownManagement", 206, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MenuViewContext.AddLayer(base.Layer);
			if (!base.Layer.Input.IsCategoryRegistered(HotKeyManager.GetCategory("GenericPanelGameKeyCategory")))
			{
				base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			}
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._layerAsGauntletLayer.LoadMovie("TownManagement", this._dataSource);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			this._dataSource.Show = true;
			MapScreen mapScreen;
			if ((mapScreen = ScreenManager.TopScreen as MapScreen) != null)
			{
				mapScreen.SetIsInTownManagement(true);
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000D588 File Offset: 0x0000B788
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._spriteCategory.Unload();
			base.MenuViewContext.RemoveLayer(base.Layer);
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			MapScreen mapScreen;
			if ((mapScreen = ScreenManager.TopScreen as MapScreen) != null)
			{
				mapScreen.SetIsInTownManagement(false);
			}
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			base.OnFinalize();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000D60C File Offset: 0x0000B80C
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("Confirm"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				if (this._dataSource.ReserveControl.IsEnabled)
				{
					this._dataSource.ReserveControl.ExecuteConfirm();
				}
				else
				{
					this._dataSource.ExecuteDone();
				}
			}
			else if (base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				if (this._dataSource.IsSelectingGovernor)
				{
					this._dataSource.IsSelectingGovernor = false;
				}
				else if (this._dataSource.ReserveControl.IsEnabled)
				{
					this._dataSource.ReserveControl.ExecuteCancel();
				}
				else
				{
					SettlementBuildingProjectVM settlementBuildingProjectVM = this._dataSource.ProjectSelection.AvailableProjects.FirstOrDefault<SettlementBuildingProjectVM>((SettlementBuildingProjectVM x) => x.IsSelected);
					if (settlementBuildingProjectVM != null)
					{
						settlementBuildingProjectVM.IsSelected = false;
					}
					else
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.ExecuteDone();
					}
				}
			}
			if (!this._dataSource.Show)
			{
				base.MenuViewContext.CloseTownManagement();
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000D73F File Offset: 0x0000B93F
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000D75B File Offset: 0x0000B95B
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000AF RID: 175
		private SpriteCategory _spriteCategory;

		// Token: 0x040000B0 RID: 176
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000B1 RID: 177
		private TownManagementVM _dataSource;
	}
}
