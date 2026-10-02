using System;
using SandBox.View.Map;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200002F RID: 47
	[OverrideView(typeof(MapBasicView))]
	public class GauntletMapBasicView : MapView
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000E837 File Offset: 0x0000CA37
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000E83F File Offset: 0x0000CA3F
		public GauntletLayer GauntletLayer { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000E848 File Offset: 0x0000CA48
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000E850 File Offset: 0x0000CA50
		public GauntletLayer GauntletNameplateLayer { get; private set; }

		// Token: 0x06000248 RID: 584 RVA: 0x0000E85C File Offset: 0x0000CA5C
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this.GauntletLayer = new GauntletLayer("MapMenuView", 100, false);
			this.GauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			base.MapScreen.AddLayer(this.GauntletLayer);
			this.GauntletNameplateLayer = new GauntletLayer("MapNameplateLayer", 90, false);
			this.GauntletNameplateLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.MouseButtons | InputUsageMask.Keyboardkeys);
			base.MapScreen.AddLayer(this.GauntletNameplateLayer);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E8DB File Offset: 0x0000CADB
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			ScreenManager.SetSuspendLayer(this.GauntletLayer, true);
			ScreenManager.SetSuspendLayer(this.GauntletNameplateLayer, true);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E8FB File Offset: 0x0000CAFB
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			ScreenManager.SetSuspendLayer(this.GauntletLayer, false);
			ScreenManager.SetSuspendLayer(this.GauntletNameplateLayer, false);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000E91B File Offset: 0x0000CB1B
		protected override void OnFinalize()
		{
			base.MapScreen.RemoveLayer(this.GauntletLayer);
			this.GauntletLayer = null;
			base.OnFinalize();
		}
	}
}
