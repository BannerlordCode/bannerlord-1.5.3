using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000171 RID: 369
	public class ConversationItemImageWidget : ImageWidget
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x0003548D File Offset: 0x0003368D
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x00035495 File Offset: 0x00033695
		public Brush NormalBrush { get; set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x0003549E File Offset: 0x0003369E
		// (set) Token: 0x06001382 RID: 4994 RVA: 0x000354A6 File Offset: 0x000336A6
		public Brush SpecialBrush { get; set; }

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x000354AF File Offset: 0x000336AF
		// (set) Token: 0x06001384 RID: 4996 RVA: 0x000354B7 File Offset: 0x000336B7
		public bool IsSpecial { get; set; }

		// Token: 0x06001385 RID: 4997 RVA: 0x000354C0 File Offset: 0x000336C0
		public ConversationItemImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x000354C9 File Offset: 0x000336C9
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				base.Brush = (this.IsSpecial ? this.SpecialBrush : this.NormalBrush);
				this._isInitialized = true;
			}
		}

		// Token: 0x040008E4 RID: 2276
		private bool _isInitialized;
	}
}
