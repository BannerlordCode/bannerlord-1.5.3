using System;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015A RID: 346
	public class EncyclopediaCharacterTableauWidget : CharacterTableauWidget
	{
		// Token: 0x0600128C RID: 4748 RVA: 0x000335B4 File Offset: 0x000317B4
		public EncyclopediaCharacterTableauWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x000335BD File Offset: 0x000317BD
		private void UpdateVisual(bool isDead)
		{
			base.Brush.SaturationFactor = (float)(isDead ? (-100) : 0);
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x000335D3 File Offset: 0x000317D3
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x000335DB File Offset: 0x000317DB
		[Editor(false)]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (this._isDead != value)
				{
					this._isDead = value;
					base.OnPropertyChanged(value, "IsDead");
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x0400087B RID: 2171
		private bool _isDead;
	}
}
