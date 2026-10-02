using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000076 RID: 118
	internal class AdminPanelAction : IAdminPanelActionInternal, IAdminPanelAction
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000FFF7 File Offset: 0x0000E1F7
		public string UniqueId
		{
			get
			{
				return this._uniqueId;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000FFFF File Offset: 0x0000E1FF
		public string Name
		{
			get
			{
				TextObject nameTextObj = this._nameTextObj;
				return ((nameTextObj != null) ? nameTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0001001C File Offset: 0x0000E21C
		public string Description
		{
			get
			{
				TextObject descriptionTextObj = this._descriptionTextObj;
				return ((descriptionTextObj != null) ? descriptionTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00010039 File Offset: 0x0000E239
		public AdminPanelAction(string uniqueId)
		{
			this._uniqueId = uniqueId;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00010048 File Offset: 0x0000E248
		public virtual void OnFinalize()
		{
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001004A File Offset: 0x0000E24A
		void IAdminPanelAction.OnActionExecuted()
		{
			Action onActionExecuted = this._onActionExecuted;
			if (onActionExecuted == null)
			{
				return;
			}
			onActionExecuted();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001005C File Offset: 0x0000E25C
		public virtual bool GetIsAvailable()
		{
			return true;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001005F File Offset: 0x0000E25F
		public virtual bool GetIsDisabled(out string reason)
		{
			reason = string.Empty;
			return false;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00010069 File Offset: 0x0000E269
		public AdminPanelAction BuildName(TextObject name)
		{
			this._nameTextObj = name;
			return this;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00010073 File Offset: 0x0000E273
		public AdminPanelAction BuildDescription(TextObject description)
		{
			this._descriptionTextObj = description;
			return this;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001007D File Offset: 0x0000E27D
		public AdminPanelAction BuildOnActionExecutedCallback(Action onActionExecuted)
		{
			this._onActionExecuted = onActionExecuted;
			return this;
		}

		// Token: 0x04000103 RID: 259
		private readonly string _uniqueId;

		// Token: 0x04000104 RID: 260
		private TextObject _nameTextObj;

		// Token: 0x04000105 RID: 261
		private TextObject _descriptionTextObj;

		// Token: 0x04000106 RID: 262
		private Action _onActionExecuted;
	}
}
