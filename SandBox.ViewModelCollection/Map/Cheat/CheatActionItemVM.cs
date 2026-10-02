using System;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x0200004E RID: 78
	public class CheatActionItemVM : CheatItemBaseVM
	{
		// Token: 0x060004F5 RID: 1269 RVA: 0x000133DE File Offset: 0x000115DE
		public CheatActionItemVM(GameplayCheatItem cheat, Action<CheatActionItemVM> onCheatExecuted)
		{
			this._onCheatExecuted = onCheatExecuted;
			this.Cheat = cheat;
			this.RefreshValues();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000133FA File Offset: 0x000115FA
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameplayCheatItem cheat = this.Cheat;
			base.Name = ((cheat != null) ? cheat.GetName().ToString() : null);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001341F File Offset: 0x0001161F
		public override void ExecuteAction()
		{
			GameplayCheatItem cheat = this.Cheat;
			if (cheat != null)
			{
				cheat.ExecuteCheat();
			}
			Action<CheatActionItemVM> onCheatExecuted = this._onCheatExecuted;
			if (onCheatExecuted == null)
			{
				return;
			}
			onCheatExecuted(this);
		}

		// Token: 0x0400027E RID: 638
		public readonly GameplayCheatItem Cheat;

		// Token: 0x0400027F RID: 639
		private readonly Action<CheatActionItemVM> _onCheatExecuted;
	}
}
