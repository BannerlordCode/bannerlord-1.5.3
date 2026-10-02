using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F1 RID: 241
	public class OrderOfBattleHeroButtonWidget : ButtonWidget
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x0002227E File Offset: 0x0002047E
		public OrderOfBattleHeroButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00022290 File Offset: 0x00020490
		private void UpdateMainHeroHueFactor()
		{
			foreach (BrushLayer brushLayer in base.Brush.Layers)
			{
				brushLayer.HueFactor = (float)(this.IsMainHero ? this.MainHeroHueFactor : 0);
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x000222F8 File Offset: 0x000204F8
		private void UpdateMainHeroAcceptEvents()
		{
			base.DoNotAcceptEvents = this.IsMainHero && !this.CanMainHeroAcceptEvents;
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00022314 File Offset: 0x00020514
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x0002231C File Offset: 0x0002051C
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChanged(value, "IsMainHero");
					this.UpdateMainHeroHueFactor();
					this.UpdateMainHeroAcceptEvents();
				}
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00022346 File Offset: 0x00020546
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x0002234E File Offset: 0x0002054E
		public int MainHeroHueFactor
		{
			get
			{
				return this._mainHeroHueFactor;
			}
			set
			{
				if (value != this._mainHeroHueFactor)
				{
					this._mainHeroHueFactor = value;
					base.OnPropertyChanged(value, "MainHeroHueFactor");
					this.UpdateMainHeroHueFactor();
				}
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00022372 File Offset: 0x00020572
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x0002237A File Offset: 0x0002057A
		public bool CanMainHeroAcceptEvents
		{
			get
			{
				return this._canMainHeroAcceptEvents;
			}
			set
			{
				if (value != this._canMainHeroAcceptEvents)
				{
					this._canMainHeroAcceptEvents = value;
					base.OnPropertyChanged(value, "CanMainHeroAcceptEvents");
					this.UpdateMainHeroAcceptEvents();
				}
			}
		}

		// Token: 0x040005A4 RID: 1444
		private bool _isMainHero;

		// Token: 0x040005A5 RID: 1445
		private int _mainHeroHueFactor;

		// Token: 0x040005A6 RID: 1446
		private bool _canMainHeroAcceptEvents = true;
	}
}
