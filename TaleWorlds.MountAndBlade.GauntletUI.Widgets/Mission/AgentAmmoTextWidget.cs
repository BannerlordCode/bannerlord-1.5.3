using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D6 RID: 214
	public class AgentAmmoTextWidget : TextWidget
	{
		// Token: 0x06000B02 RID: 2818 RVA: 0x0001EFC4 File Offset: 0x0001D1C4
		public AgentAmmoTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0001EFCD File Offset: 0x0001D1CD
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsAlertEnabled)
			{
				this.SetState("Alert");
				return;
			}
			this.SetState("Default");
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x0001EFF5 File Offset: 0x0001D1F5
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x0001EFFD File Offset: 0x0001D1FD
		public bool IsAlertEnabled
		{
			get
			{
				return this._isAlertEnabled;
			}
			set
			{
				if (this._isAlertEnabled != value)
				{
					this._isAlertEnabled = value;
					base.OnPropertyChanged(value, "IsAlertEnabled");
				}
			}
		}

		// Token: 0x040004FC RID: 1276
		private bool _isAlertEnabled;
	}
}
