using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200024B RID: 587
	public class ProfileSelectionState : GameState
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x00077B25 File Offset: 0x00075D25
		// (set) Token: 0x060021FB RID: 8699 RVA: 0x00077B2D File Offset: 0x00075D2D
		public bool IsDirectPlayPossible { get; private set; } = true;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060021FC RID: 8700 RVA: 0x00077B38 File Offset: 0x00075D38
		// (remove) Token: 0x060021FD RID: 8701 RVA: 0x00077B70 File Offset: 0x00075D70
		public event ProfileSelectionState.OnProfileSelectionEvent OnProfileSelection;

		// Token: 0x060021FE RID: 8702 RVA: 0x00077BA5 File Offset: 0x00075DA5
		public void OnProfileSelected()
		{
			NativeOptions.ReadRGLConfigFiles();
			BannerlordConfig.Initialize();
			ProfileSelectionState.OnProfileSelectionEvent onProfileSelection = this.OnProfileSelection;
			if (onProfileSelection != null)
			{
				onProfileSelection();
			}
			this.StartGame();
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00077BC8 File Offset: 0x00075DC8
		public void StartGame()
		{
			Module.CurrentModule.SetInitialModuleScreenAsRootScreen();
		}

		// Token: 0x02000544 RID: 1348
		// (Invoke) Token: 0x06003D3E RID: 15678
		public delegate void OnProfileSelectionEvent();
	}
}
