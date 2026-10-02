using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000078 RID: 120
	internal class AdminPanelOptionGroup : IAdminPanelOptionGroup, IAdminPanelTickable
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00010412 File Offset: 0x0000E612
		string IAdminPanelOptionGroup.UniqueId
		{
			get
			{
				return this._uniqueId;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0001041A File Offset: 0x0000E61A
		TextObject IAdminPanelOptionGroup.Name
		{
			get
			{
				return this._nameTextObj;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00010422 File Offset: 0x0000E622
		MBReadOnlyList<IAdminPanelOption> IAdminPanelOptionGroup.Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0001042A File Offset: 0x0000E62A
		MBReadOnlyList<IAdminPanelAction> IAdminPanelOptionGroup.Actions
		{
			get
			{
				return this._actions;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00010432 File Offset: 0x0000E632
		bool IAdminPanelOptionGroup.RequiresRestart
		{
			get
			{
				return this._requiresRestart;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001043A File Offset: 0x0000E63A
		public AdminPanelOptionGroup(string uniqueId, TextObject name, bool requiresRestart = false)
		{
			this._uniqueId = uniqueId;
			this._nameTextObj = name;
			this._requiresRestart = requiresRestart;
			this._options = new MBList<IAdminPanelOption>();
			this._actions = new MBList<IAdminPanelAction>();
			this._tickableOptions = new MBList<IAdminPanelTickable>();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00010478 File Offset: 0x0000E678
		public void AddOption(IAdminPanelOption option)
		{
			this._options.Add(option);
			IAdminPanelTickable adminPanelTickable;
			if ((adminPanelTickable = option as IAdminPanelTickable) != null)
			{
				this._tickableOptions.Add(adminPanelTickable);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000104A7 File Offset: 0x0000E6A7
		public void AddAction(IAdminPanelAction action)
		{
			this._actions.Add(action);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000104B8 File Offset: 0x0000E6B8
		void IAdminPanelTickable.OnTick(float dt)
		{
			for (int i = 0; i < this._tickableOptions.Count; i++)
			{
				this._tickableOptions[i].OnTick(dt);
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000104F0 File Offset: 0x0000E6F0
		void IAdminPanelOptionGroup.OnFinalize()
		{
			for (int i = 0; i < this._options.Count; i++)
			{
				IAdminPanelOptionInternal adminPanelOptionInternal;
				if ((adminPanelOptionInternal = this._options[i] as IAdminPanelOptionInternal) != null)
				{
					adminPanelOptionInternal.OnFinalize();
				}
			}
			for (int j = 0; j < this._actions.Count; j++)
			{
				IAdminPanelActionInternal adminPanelActionInternal;
				if ((adminPanelActionInternal = this._actions[j] as IAdminPanelActionInternal) != null)
				{
					adminPanelActionInternal.OnFinalize();
				}
			}
		}

		// Token: 0x04000114 RID: 276
		private readonly bool _requiresRestart;

		// Token: 0x04000115 RID: 277
		private readonly string _uniqueId;

		// Token: 0x04000116 RID: 278
		private readonly TextObject _nameTextObj;

		// Token: 0x04000117 RID: 279
		private readonly MBList<IAdminPanelOption> _options;

		// Token: 0x04000118 RID: 280
		private readonly MBList<IAdminPanelAction> _actions;

		// Token: 0x04000119 RID: 281
		private readonly MBList<IAdminPanelTickable> _tickableOptions;
	}
}
