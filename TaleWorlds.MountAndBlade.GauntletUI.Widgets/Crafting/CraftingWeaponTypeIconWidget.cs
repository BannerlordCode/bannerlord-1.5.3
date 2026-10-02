using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000170 RID: 368
	public class CraftingWeaponTypeIconWidget : Widget
	{
		// Token: 0x0600137B RID: 4987 RVA: 0x0003542B File Offset: 0x0003362B
		public CraftingWeaponTypeIconWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00035434 File Offset: 0x00033634
		private void UpdateIconVisual()
		{
			base.Sprite = base.Context.SpriteData.GetSprite("Crafting\\WeaponTypes\\" + this.WeaponType);
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x0003545C File Offset: 0x0003365C
		// (set) Token: 0x0600137E RID: 4990 RVA: 0x00035464 File Offset: 0x00033664
		[Editor(false)]
		public string WeaponType
		{
			get
			{
				return this._weaponType;
			}
			set
			{
				if (value != this._weaponType)
				{
					this._weaponType = value;
					this.UpdateIconVisual();
					base.OnPropertyChanged<string>(value, "WeaponType");
				}
			}
		}

		// Token: 0x040008E0 RID: 2272
		private string _weaponType;
	}
}
