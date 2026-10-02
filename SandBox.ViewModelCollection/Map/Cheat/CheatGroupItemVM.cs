using System;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x0200004F RID: 79
	public class CheatGroupItemVM : CheatItemBaseVM
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x00013443 File Offset: 0x00011643
		public CheatGroupItemVM(GameplayCheatGroup cheatGroup, Action<CheatGroupItemVM> onSelectCheatGroup)
		{
			this.CheatGroup = cheatGroup;
			this._onSelectCheatGroup = onSelectCheatGroup;
			this.RefreshValues();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001345F File Offset: 0x0001165F
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject name = this.CheatGroup.GetName();
			base.Name = ((name != null) ? name.ToString() : null);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00013484 File Offset: 0x00011684
		public override void ExecuteAction()
		{
			Action<CheatGroupItemVM> onSelectCheatGroup = this._onSelectCheatGroup;
			if (onSelectCheatGroup == null)
			{
				return;
			}
			onSelectCheatGroup(this);
		}

		// Token: 0x04000280 RID: 640
		public readonly GameplayCheatGroup CheatGroup;

		// Token: 0x04000281 RID: 641
		private readonly Action<CheatGroupItemVM> _onSelectCheatGroup;
	}
}
