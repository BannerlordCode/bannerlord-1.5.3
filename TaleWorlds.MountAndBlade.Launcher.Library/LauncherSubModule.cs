using System;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000006 RID: 6
	public class LauncherSubModule : ViewModel
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000025CB File Offset: 0x000007CB
		public LauncherSubModule(SubModuleInfo subModuleInfo)
		{
			this.Info = subModuleInfo;
			this.Name = subModuleInfo.Name;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000025E6 File Offset: 0x000007E6
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000025EE File Offset: 0x000007EE
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x04000012 RID: 18
		public readonly SubModuleInfo Info;

		// Token: 0x04000013 RID: 19
		private string _name;
	}
}
