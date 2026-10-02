using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000A RID: 10
	public class GauntletDebugStats : GlobalLayer
	{
		// Token: 0x0600004B RID: 75 RVA: 0x000042CC File Offset: 0x000024CC
		public void Initialize()
		{
			this._dataSource = new DebugStatsVM();
			GauntletLayer gauntletLayer = new GauntletLayer("DebugStats", 30000, false);
			gauntletLayer.LoadMovie("DebugStats", this._dataSource);
			gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			base.Layer = gauntletLayer;
			ScreenManager.AddGlobalLayer(this, true);
		}

		// Token: 0x0400003D RID: 61
		private DebugStatsVM _dataSource;
	}
}
