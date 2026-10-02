using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CE RID: 206
	public class MultiplayerClassLoadoutItemTabListPanel : ListPanel
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000AC8 RID: 2760 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
		// (remove) Token: 0x06000AC9 RID: 2761 RVA: 0x0001E5FC File Offset: 0x0001C7FC
		public event Action OnInitialized;

		// Token: 0x06000ACA RID: 2762 RVA: 0x0001E631 File Offset: 0x0001C831
		public MultiplayerClassLoadoutItemTabListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0001E63A File Offset: 0x0001C83A
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				this._isInitialized = true;
				Action onInitialized = this.OnInitialized;
				if (onInitialized == null)
				{
					return;
				}
				onInitialized();
			}
		}

		// Token: 0x040004E9 RID: 1257
		private bool _isInitialized;
	}
}
