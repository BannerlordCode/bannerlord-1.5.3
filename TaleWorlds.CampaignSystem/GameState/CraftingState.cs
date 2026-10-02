using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A7 RID: 935
	public class CraftingState : GameState
	{
		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000DF251 File Offset: 0x000DD451
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060036BF RID: 14015 RVA: 0x000DF254 File Offset: 0x000DD454
		// (set) Token: 0x060036C0 RID: 14016 RVA: 0x000DF25C File Offset: 0x000DD45C
		public Crafting CraftingLogic { get; private set; }

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x000DF265 File Offset: 0x000DD465
		// (set) Token: 0x060036C2 RID: 14018 RVA: 0x000DF26D File Offset: 0x000DD46D
		public ICraftingStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000DF276 File Offset: 0x000DD476
		public void InitializeLogic(Crafting newCraftingLogic, bool isReplacingWeaponClass = false)
		{
			this.CraftingLogic = newCraftingLogic;
			if (this._handler != null)
			{
				if (isReplacingWeaponClass)
				{
					this._handler.OnCraftingLogicRefreshed();
					return;
				}
				this._handler.OnCraftingLogicInitialized();
			}
		}

		// Token: 0x04000F65 RID: 3941
		private ICraftingStateHandler _handler;
	}
}
