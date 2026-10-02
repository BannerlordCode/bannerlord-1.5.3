using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000181 RID: 385
	public class CharacterDeveloperPerkSelectionItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x00037250 File Offset: 0x00035450
		// (set) Token: 0x06001429 RID: 5161 RVA: 0x00037258 File Offset: 0x00035458
		public Widget PerkSelectionIndicatorWidget { get; set; }

		// Token: 0x0600142A RID: 5162 RVA: 0x00037261 File Offset: 0x00035461
		public CharacterDeveloperPerkSelectionItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0003726C File Offset: 0x0003546C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.PerkSelectionIndicatorWidget != null)
			{
				if (base.ParentWidget.ChildCount == 1)
				{
					this.PerkSelectionIndicatorWidget.VerticalAlignment = VerticalAlignment.Center;
					return;
				}
				this.PerkSelectionIndicatorWidget.VerticalAlignment = ((base.GetSiblingIndex() % 2 == 0) ? VerticalAlignment.Bottom : VerticalAlignment.Top);
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x000372BC File Offset: 0x000354BC
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000372C4 File Offset: 0x000354C4
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
		}
	}
}
