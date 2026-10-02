using System;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace SandBox.View.Menu
{
	// Token: 0x0200003C RID: 60
	public abstract class MenuView : SandboxView
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00013108 File Offset: 0x00011308
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00013110 File Offset: 0x00011310
		internal bool Removed { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00013119 File Offset: 0x00011319
		public virtual bool ShouldUpdateMenuAfterRemoved
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0001311C File Offset: 0x0001131C
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00013124 File Offset: 0x00011324
		public MenuViewContext MenuViewContext { get; internal set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0001312D File Offset: 0x0001132D
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00013135 File Offset: 0x00011335
		public MenuContext MenuContext { get; internal set; }

		// Token: 0x060001BD RID: 445 RVA: 0x0001313E File Offset: 0x0001133E
		protected internal virtual void OnMenuContextUpdated(MenuContext newMenuContext)
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00013140 File Offset: 0x00011340
		protected internal virtual void OnMenuContextRefreshed()
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00013142 File Offset: 0x00011342
		protected internal virtual void OnOverlayTypeChange(GameMenu.MenuOverlayType newType)
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00013144 File Offset: 0x00011344
		protected internal virtual void OnCharacterDeveloperOpened()
		{
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00013146 File Offset: 0x00011346
		protected internal virtual void OnCharacterDeveloperClosed()
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00013148 File Offset: 0x00011348
		protected internal virtual void OnBackgroundMeshNameSet(string name)
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0001314A File Offset: 0x0001134A
		protected internal virtual void OnHourlyTick()
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0001314C File Offset: 0x0001134C
		protected internal virtual void OnResume()
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0001314E File Offset: 0x0001134E
		protected internal virtual void OnMapConversationActivated()
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00013150 File Offset: 0x00011350
		protected internal virtual void OnMapConversationDeactivated()
		{
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00013152 File Offset: 0x00011352
		protected internal virtual TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x040000F9 RID: 249
		protected const float ContextAlphaModifier = 8.5f;
	}
}
