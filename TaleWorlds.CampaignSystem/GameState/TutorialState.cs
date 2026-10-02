using System;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003BC RID: 956
	public class TutorialState : GameState
	{
		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x000DFCC4 File Offset: 0x000DDEC4
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000DFCEA File Offset: 0x000DDEEA
		protected override void OnActivate()
		{
			base.OnActivate();
			this.MenuContext.Refresh();
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000DFCFD File Offset: 0x000DDEFD
		protected override void OnFinalize()
		{
			this.MenuContext.Destroy();
			this._objectManager.UnregisterObject(this.MenuContext);
			this.MenuContext = null;
			base.OnFinalize();
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000DFD28 File Offset: 0x000DDF28
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.MenuContext.OnTick(dt);
		}

		// Token: 0x04000F90 RID: 3984
		private MBObjectManager _objectManager = MBObjectManager.Instance;

		// Token: 0x04000F91 RID: 3985
		public MenuContext MenuContext = MBObjectManager.Instance.CreateObject<MenuContext>();
	}
}
