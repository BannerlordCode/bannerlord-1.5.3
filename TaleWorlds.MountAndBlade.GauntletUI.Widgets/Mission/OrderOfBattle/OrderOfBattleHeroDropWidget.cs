using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F3 RID: 243
	public class OrderOfBattleHeroDropWidget : ButtonWidget
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00022624 File Offset: 0x00020824
		public OrderOfBattleHeroDropWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0002262D File Offset: 0x0002082D
		protected override bool OnPreviewDrop()
		{
			this.HandleSoundEvent();
			return true;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00022636 File Offset: 0x00020836
		protected override void HandleClick()
		{
			this.HandleSoundEvent();
			base.HandleClick();
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00022644 File Offset: 0x00020844
		private void HandleSoundEvent()
		{
			switch (this.FormationClass)
			{
			case 0:
				break;
			case 1:
				base.EventFired("Infantry", Array.Empty<object>());
				return;
			case 2:
				base.EventFired("Archers", Array.Empty<object>());
				return;
			case 3:
				base.EventFired("Cavalry", Array.Empty<object>());
				return;
			case 4:
				base.EventFired("HorseArchers", Array.Empty<object>());
				return;
			case 5:
				base.EventFired("InfantryArchers", Array.Empty<object>());
				return;
			case 6:
				base.EventFired("CavalryHorseArchers", Array.Empty<object>());
				break;
			default:
				return;
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000226E0 File Offset: 0x000208E0
		protected override bool OnPreviewDragHover()
		{
			return true;
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x000226E3 File Offset: 0x000208E3
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x000226EB File Offset: 0x000208EB
		[DataSourceProperty]
		public int FormationClass
		{
			get
			{
				return this._formationClass;
			}
			set
			{
				if (value != this._formationClass)
				{
					this._formationClass = value;
					base.OnPropertyChanged(value, "FormationClass");
				}
			}
		}

		// Token: 0x040005AC RID: 1452
		private int _formationClass;
	}
}
