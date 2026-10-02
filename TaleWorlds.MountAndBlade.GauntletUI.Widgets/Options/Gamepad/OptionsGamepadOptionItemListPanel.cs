using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007C RID: 124
	public class OptionsGamepadOptionItemListPanel : ListPanel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006D8 RID: 1752 RVA: 0x00013F9C File Offset: 0x0001219C
		// (remove) Token: 0x060006D9 RID: 1753 RVA: 0x00013FD4 File Offset: 0x000121D4
		public event OptionsGamepadOptionItemListPanel.OnActionTextChangeEvent OnActionTextChanged;

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00014009 File Offset: 0x00012209
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x00014011 File Offset: 0x00012211
		public OptionsGamepadKeyLocationWidget TargetKey { get; private set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001401A File Offset: 0x0001221A
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x00014022 File Offset: 0x00012222
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (this._actionText != value)
				{
					this._actionText = value;
					OptionsGamepadOptionItemListPanel.OnActionTextChangeEvent onActionTextChanged = this.OnActionTextChanged;
					if (onActionTextChanged == null)
					{
						return;
					}
					onActionTextChanged();
				}
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00014049 File Offset: 0x00012249
		public OptionsGamepadOptionItemListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00014052 File Offset: 0x00012252
		public void SetKeyProperties(OptionsGamepadKeyLocationWidget currentTarget, Widget parentAreaWidget)
		{
			this.TargetKey = currentTarget;
			this.TargetKey.SetKeyProperties(this.ActionText, parentAreaWidget);
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001406D File Offset: 0x0001226D
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x00014075 File Offset: 0x00012275
		public int KeyId
		{
			get
			{
				return this._keyId;
			}
			set
			{
				if (value != this._keyId)
				{
					this._keyId = value;
				}
			}
		}

		// Token: 0x040002F1 RID: 753
		private string _actionText;

		// Token: 0x040002F2 RID: 754
		private int _keyId;

		// Token: 0x020001B5 RID: 437
		// (Invoke) Token: 0x06001572 RID: 5490
		public delegate void OnActionTextChangeEvent();
	}
}
