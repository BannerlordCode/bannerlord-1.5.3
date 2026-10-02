using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006F RID: 111
	public class PartyUpgradesContainerWidget : Widget
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x00011FAC File Offset: 0x000101AC
		public PartyUpgradesContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00011FBC File Offset: 0x000101BC
		private void OnAnyUpgradeHasRequirementChanged(bool value)
		{
			base.ScaledPositionYOffset = (value ? 0f : 8f);
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x00011FDB File Offset: 0x000101DB
		[Editor(false)]
		public bool AnyUpgradeHasRequirement
		{
			get
			{
				return this._anyUpgradeHasRequirement;
			}
			set
			{
				if (this._anyUpgradeHasRequirement != value)
				{
					this._anyUpgradeHasRequirement = value;
					this.OnAnyUpgradeHasRequirementChanged(value);
					base.OnPropertyChanged(value, "AnyUpgradeHasRequirement");
				}
			}
		}

		// Token: 0x04000298 RID: 664
		private const float _noRequirementOffset = 8f;

		// Token: 0x04000299 RID: 665
		private bool _anyUpgradeHasRequirement = true;
	}
}
