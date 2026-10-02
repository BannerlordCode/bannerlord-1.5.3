using System;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000011 RID: 17
	public class LauncherDLLData
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000456A File Offset: 0x0000276A
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004572 File Offset: 0x00002772
		public SubModuleInfo SubModule { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000457B File Offset: 0x0000277B
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00004583 File Offset: 0x00002783
		public bool IsDangerous { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000458C File Offset: 0x0000278C
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004594 File Offset: 0x00002794
		public string VerifyInformation { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000459D File Offset: 0x0000279D
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000045A5 File Offset: 0x000027A5
		public uint Size { get; private set; }

		// Token: 0x06000099 RID: 153 RVA: 0x000045AE File Offset: 0x000027AE
		public LauncherDLLData(SubModuleInfo subModule, bool isDangerous, string verifyInformation, uint size)
		{
			this.SubModule = subModule;
			this.IsDangerous = isDangerous;
			this.VerifyInformation = verifyInformation;
			this.Size = size;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000045D3 File Offset: 0x000027D3
		public void SetIsDLLDangerous(bool isDangerous)
		{
			this.IsDangerous = isDangerous;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000045DC File Offset: 0x000027DC
		public void SetDLLSize(uint size)
		{
			this.Size = size;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000045E5 File Offset: 0x000027E5
		public void SetDLLVerifyInformation(string info)
		{
			this.VerifyInformation = info;
		}
	}
}
