using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000C RID: 12
	public class GauntletDefaultLoadingWindowManager : GlobalLayer, ILoadingWindowManager
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00004384 File Offset: 0x00002584
		void ILoadingWindowManager.Initialize()
		{
			string spriteCategoryName = this.GetSpriteCategoryName();
			UIResourceManager.SpriteData.SpriteCategories.ContainsKey(spriteCategoryName);
			this._sploadingCategory = UIResourceManager.GetSpriteCategory(spriteCategoryName);
			this._sploadingCategory.InitializePartialLoad();
			this._loadingWindowViewModel = new LoadingWindowViewModel(new LoadingWindowViewModel.LoadImageDelegate(this.LoadImage), new LoadingWindowViewModel.UnloadImageDelegate(this.UnloadImage));
			this._loadingWindowViewModel.Enabled = false;
			this._loadingWindowViewModel.SetTotalGenericImageCount(this._sploadingCategory.SpriteSheetCount);
			this._loadingWindowViewModel.IsNavalDLCEnabled = ModuleHelper.IsModuleActive("NavalDLC");
			bool flag = false;
			this._gauntletLayer = new GauntletLayer("LoadingWindow", 115003, flag);
			this._gauntletLayer.LoadMovie("LoadingWindow", this._loadingWindowViewModel);
			base.Layer = this._gauntletLayer;
			ScreenManager.AddGlobalLayer(this, false);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000445C File Offset: 0x0000265C
		void ILoadingWindowManager.Destroy()
		{
			if (this._gauntletLayer == null)
			{
				Debug.FailedAssert("Trying to destroy loading window but it was not initialized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletDefaultLoadingWindowManager.cs", "Destroy", 63);
				return;
			}
			LoadingWindowViewModel loadingWindowViewModel = this._loadingWindowViewModel;
			if (loadingWindowViewModel != null)
			{
				loadingWindowViewModel.OnFinalize();
			}
			ScreenManager.RemoveGlobalLayer(this, true);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004498 File Offset: 0x00002698
		void ILoadingWindowManager.EnableLoadingWindow()
		{
			this._loadingWindowViewModel.Enabled = true;
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			base.Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			Utilities.StartLoadingStuckCheckState(720f);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000044E4 File Offset: 0x000026E4
		void ILoadingWindowManager.DisableLoadingWindow()
		{
			this._loadingWindowViewModel.Enabled = false;
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			base.Layer.InputRestrictions.ResetInputRestrictions();
			GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
			if (instance != null)
			{
				instance.SetAllDirty();
			}
			Utilities.EndLoadingStuckCheckState();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004539 File Offset: 0x00002739
		protected virtual string GetSpriteCategoryName()
		{
			return "ui_loading";
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004540 File Offset: 0x00002740
		protected override void OnLateTick(float dt)
		{
			base.OnLateTick(dt);
			this._loadingWindowViewModel.Update();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004554 File Offset: 0x00002754
		public void SetCurrentModeIsMultiplayer(bool isMultiplayer)
		{
			if (this._isMultiplayer != isMultiplayer)
			{
				this._isMultiplayer = isMultiplayer;
				this._loadingWindowViewModel.IsMultiplayer = isMultiplayer;
				if (isMultiplayer)
				{
					this._mpLoadingCategory = UIResourceManager.LoadSpriteCategory("ui_mploading");
					this._mpBackgroundCategory = UIResourceManager.LoadSpriteCategory("ui_mpbackgrounds");
					return;
				}
				this._mpLoadingCategory.Unload();
				this._mpBackgroundCategory.Unload();
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000045B8 File Offset: 0x000027B8
		private void LoadImage(int index, out string imageName)
		{
			if (this._sploadingCategory != null)
			{
				this._sploadingCategory.PartialLoadAtIndex(UIResourceManager.ResourceContext, UIResourceManager.ResourceDepot, index);
				imageName = this._sploadingCategory.SpriteParts[index - 1].Name;
				return;
			}
			imageName = string.Empty;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004605 File Offset: 0x00002805
		private void UnloadImage(int index)
		{
			if (this._sploadingCategory != null)
			{
				this._sploadingCategory.PartialUnloadAtIndex(index);
			}
		}

		// Token: 0x0400003F RID: 63
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000040 RID: 64
		private LoadingWindowViewModel _loadingWindowViewModel;

		// Token: 0x04000041 RID: 65
		private SpriteCategory _sploadingCategory;

		// Token: 0x04000042 RID: 66
		private SpriteCategory _mpLoadingCategory;

		// Token: 0x04000043 RID: 67
		private SpriteCategory _mpBackgroundCategory;

		// Token: 0x04000044 RID: 68
		private bool _isMultiplayer;
	}
}
