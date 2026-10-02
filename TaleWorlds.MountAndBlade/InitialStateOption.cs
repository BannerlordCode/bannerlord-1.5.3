using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000247 RID: 583
	public class InitialStateOption
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x00077122 File Offset: 0x00075322
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x0007712A File Offset: 0x0007532A
		public int OrderIndex { get; private set; }

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00077133 File Offset: 0x00075333
		// (set) Token: 0x060021C5 RID: 8645 RVA: 0x0007713B File Offset: 0x0007533B
		public TextObject Name { get; private set; }

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060021C6 RID: 8646 RVA: 0x00077144 File Offset: 0x00075344
		// (set) Token: 0x060021C7 RID: 8647 RVA: 0x0007714C File Offset: 0x0007534C
		public string Id { get; private set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x00077155 File Offset: 0x00075355
		// (set) Token: 0x060021C9 RID: 8649 RVA: 0x0007715D File Offset: 0x0007535D
		public Func<bool> IsHidden { get; private set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x00077166 File Offset: 0x00075366
		// (set) Token: 0x060021CB RID: 8651 RVA: 0x0007716E File Offset: 0x0007536E
		public Func<ValueTuple<bool, TextObject>> IsDisabledAndReason { get; private set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x00077177 File Offset: 0x00075377
		// (set) Token: 0x060021CD RID: 8653 RVA: 0x0007717F File Offset: 0x0007537F
		public TextObject EnabledHint { get; private set; }

		// Token: 0x060021CE RID: 8654 RVA: 0x00077188 File Offset: 0x00075388
		public InitialStateOption(string id, TextObject name, int orderIndex, Action action, Func<ValueTuple<bool, TextObject>> isDisabledAndReason, TextObject enabledHint = null, Func<bool> isHidden = null)
		{
			this.Name = name;
			this.Id = id;
			this.OrderIndex = orderIndex;
			this._action = action;
			this.IsHidden = isHidden;
			this.IsDisabledAndReason = isDisabledAndReason;
			this.EnabledHint = enabledHint;
			TextObject item = this.IsDisabledAndReason().Item2;
			string.IsNullOrEmpty((item != null) ? item.ToString() : null);
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000771F2 File Offset: 0x000753F2
		public void DoAction()
		{
			Action action = this._action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04000CF7 RID: 3319
		private Action _action;
	}
}
