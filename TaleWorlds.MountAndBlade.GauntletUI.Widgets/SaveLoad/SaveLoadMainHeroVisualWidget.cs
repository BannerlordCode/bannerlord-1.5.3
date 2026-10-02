using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.SaveLoad
{
	// Token: 0x0200005C RID: 92
	public class SaveLoadMainHeroVisualWidget : Widget
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0000F93C File Offset: 0x0000DB3C
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x0000F944 File Offset: 0x0000DB44
		public Widget DefaultVisualWidget { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0000F94D File Offset: 0x0000DB4D
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x0000F955 File Offset: 0x0000DB55
		public SaveLoadHeroTableauWidget SaveLoadHeroTableau { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000F95E File Offset: 0x0000DB5E
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x0000F966 File Offset: 0x0000DB66
		public bool IsVisualDisabledForMemoryPurposes { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x0000F96F File Offset: 0x0000DB6F
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x0000F977 File Offset: 0x0000DB77
		public bool IsLoadingSaves { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0000F980 File Offset: 0x0000DB80
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x0000F988 File Offset: 0x0000DB88
		public bool IsRefreshingSaves { get; set; }

		// Token: 0x06000510 RID: 1296 RVA: 0x0000F991 File Offset: 0x0000DB91
		public SaveLoadMainHeroVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000F99C File Offset: 0x0000DB9C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.DefaultVisualWidget != null && !this.IsLoadingSaves && !this.IsRefreshingSaves)
			{
				if (this.IsVisualDisabledForMemoryPurposes)
				{
					this.DefaultVisualWidget.IsVisible = true;
					this.SaveLoadHeroTableau.IsVisible = false;
					return;
				}
				this.DefaultVisualWidget.IsVisible = string.IsNullOrEmpty(this.SaveLoadHeroTableau.HeroVisualCode) || !this.SaveLoadHeroTableau.IsVersionCompatible;
				this.SaveLoadHeroTableau.IsVisible = !this.DefaultVisualWidget.IsVisible;
			}
		}
	}
}
