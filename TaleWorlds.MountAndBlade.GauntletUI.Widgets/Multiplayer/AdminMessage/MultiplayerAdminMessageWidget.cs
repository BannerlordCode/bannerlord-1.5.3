using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.AdminMessage
{
	// Token: 0x020000D4 RID: 212
	public class MultiplayerAdminMessageWidget : Widget
	{
		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0001EACB File Offset: 0x0001CCCB
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x0001EAD3 File Offset: 0x0001CCD3
		public TextWidget MessageTextWidget { get; set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0001EADC File Offset: 0x0001CCDC
		public float MessageOnScreenStayTime
		{
			get
			{
				return 5f;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0001EAE3 File Offset: 0x0001CCE3
		public float MessageFadeInTime
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0001EAEA File Offset: 0x0001CCEA
		public float MessageFadeOutTime
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0001EAF1 File Offset: 0x0001CCF1
		public MultiplayerAdminMessageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0001EAFC File Offset: 0x0001CCFC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.ChildCount <= 0)
			{
				this._currentTextOnScreenTime = 0f;
				return;
			}
			this._currentTextOnScreenTime += dt;
			if (this._currentTextOnScreenTime < this.MessageFadeInTime)
			{
				float num = MathF.Lerp(0f, 1f, this._currentTextOnScreenTime / this.MessageFadeInTime, 1E-05f);
				base.Children[0].SetGlobalAlphaRecursively(num);
				base.Children[0].IsVisible = true;
				return;
			}
			if (this._currentTextOnScreenTime > this.MessageFadeInTime && this._currentTextOnScreenTime < this.MessageOnScreenStayTime + this.MessageFadeInTime)
			{
				base.Children[0].SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this._currentTextOnScreenTime < this.MessageFadeInTime + this.MessageOnScreenStayTime + this.MessageFadeOutTime)
			{
				float num2 = MathF.Lerp(1f, 0f, (this._currentTextOnScreenTime - (this.MessageFadeInTime + this.MessageOnScreenStayTime)) / this.MessageFadeOutTime, 1E-05f);
				base.Children[0].SetGlobalAlphaRecursively(num2);
				return;
			}
			MultiplayerAdminMessageItemWidget multiplayerAdminMessageItemWidget = base.Children[0] as MultiplayerAdminMessageItemWidget;
			if (multiplayerAdminMessageItemWidget != null)
			{
				multiplayerAdminMessageItemWidget.Remove();
			}
			this._currentTextOnScreenTime = 0f;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0001EC4B File Offset: 0x0001CE4B
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
		}

		// Token: 0x040004F8 RID: 1272
		private float _currentTextOnScreenTime;
	}
}
