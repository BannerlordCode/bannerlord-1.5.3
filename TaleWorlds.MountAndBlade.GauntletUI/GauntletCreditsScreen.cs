using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.Credits;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000009 RID: 9
	[OverrideView(typeof(CreditsScreen))]
	public class GauntletCreditsScreen : ScreenBase
	{
		// Token: 0x06000046 RID: 70 RVA: 0x0000419C File Offset: 0x0000239C
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._creditsCategory = UIResourceManager.LoadSpriteCategory("ui_credits");
			this._datasource = new CreditsVM();
			string text = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/" + "Credits.xml";
			this._datasource.FillFromFile(text);
			this._gauntletLayer = new GauntletLayer("CreditsScreen", 100, false);
			this._gauntletLayer.IsFocusLayer = true;
			base.AddLayer(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this._movie = this._gauntletLayer.LoadMovie("CreditsScreen", this._datasource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			InformationManager.HideAllMessages();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004278 File Offset: 0x00002478
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._creditsCategory.Unload();
			this._datasource.OnFinalize();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004296 File Offset: 0x00002496
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._gauntletLayer.Input.IsHotKeyPressed("Exit"))
			{
				ScreenManager.PopScreen();
			}
		}

		// Token: 0x04000039 RID: 57
		private CreditsVM _datasource;

		// Token: 0x0400003A RID: 58
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400003B RID: 59
		private GauntletMovieIdentifier _movie;

		// Token: 0x0400003C RID: 60
		private SpriteCategory _creditsCategory;
	}
}
