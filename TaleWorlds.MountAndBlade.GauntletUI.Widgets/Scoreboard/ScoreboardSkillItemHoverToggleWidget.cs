using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x0200005A RID: 90
	public class ScoreboardSkillItemHoverToggleWidget : HoverToggleWidget
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0000F7BB File Offset: 0x0000D9BB
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x0000F7C3 File Offset: 0x0000D9C3
		public ScoreboardGainedSkillsListPanel SkillsShowWidget { get; set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		public ListPanel GainedSkillsList { get; set; }

		// Token: 0x060004FB RID: 1275 RVA: 0x0000F7DD File Offset: 0x0000D9DD
		public ScoreboardSkillItemHoverToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000F7E6 File Offset: 0x0000D9E6
		public List<Widget> GetAllSkillWidgets()
		{
			return this.GainedSkillsList.Children;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000F7F4 File Offset: 0x0000D9F4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsOverWidget && !this._isHoverBeginHandled)
			{
				this.SkillsShowWidget.SetCurrentUnit(this);
				this._isHoverBeginHandled = true;
				this._isHoverEndHandled = true;
				return;
			}
			if (!base.IsOverWidget && this._isHoverEndHandled)
			{
				this.SkillsShowWidget.SetCurrentUnit(null);
				this._isHoverEndHandled = false;
				this._isHoverBeginHandled = false;
			}
		}

		// Token: 0x0400021F RID: 543
		private bool _isHoverEndHandled;

		// Token: 0x04000220 RID: 544
		private bool _isHoverBeginHandled;
	}
}
