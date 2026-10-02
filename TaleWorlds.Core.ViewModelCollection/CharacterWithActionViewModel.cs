using System;

namespace TaleWorlds.Core.ViewModelCollection
{
	// Token: 0x02000009 RID: 9
	public class CharacterWithActionViewModel : CharacterViewModel
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000029BC File Offset: 0x00000BBC
		public CharacterWithActionViewModel(Action onAction)
		{
			this._onAction = onAction;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000029CB File Offset: 0x00000BCB
		private void ExecuteAction()
		{
			Action onAction = this._onAction;
			if (onAction == null)
			{
				return;
			}
			onAction();
		}

		// Token: 0x04000021 RID: 33
		private Action _onAction;
	}
}
