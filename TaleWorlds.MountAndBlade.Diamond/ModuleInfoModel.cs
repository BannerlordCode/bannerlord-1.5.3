using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200013B RID: 315
	[Serializable]
	public class ModuleInfoModel
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0000C8B3 File Offset: 0x0000AAB3
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x0000C8BB File Offset: 0x0000AABB
		public string Id { get; private set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x0000C8CC File Offset: 0x0000AACC
		public string Name { get; private set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0000C8D5 File Offset: 0x0000AAD5
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0000C8DD File Offset: 0x0000AADD
		public ModuleCategory Category { get; private set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0000C8E6 File Offset: 0x0000AAE6
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x0000C8EE File Offset: 0x0000AAEE
		public string Version { get; private set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0000C8F7 File Offset: 0x0000AAF7
		[JsonIgnore]
		public bool IsOptional
		{
			get
			{
				return this.Category == ModuleCategory.MultiplayerOptional;
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0000C902 File Offset: 0x0000AB02
		[JsonConstructor]
		private ModuleInfoModel(string id, string name, string version, ModuleCategory category)
		{
			this.Id = id;
			this.Name = name;
			this.Version = version;
			this.Category = category;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0000C928 File Offset: 0x0000AB28
		internal ModuleInfoModel(ModuleInfo moduleInfo)
			: this(moduleInfo.Id, moduleInfo.Name, moduleInfo.Version.ToString(), moduleInfo.Category)
		{
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0000C961 File Offset: 0x0000AB61
		public static bool ShouldIncludeInSession(ModuleInfo moduleInfo)
		{
			return !moduleInfo.IsOfficial && moduleInfo.HasMultiplayerCategory;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0000C973 File Offset: 0x0000AB73
		public static bool TryCreateForSession(ModuleInfo moduleInfo, out ModuleInfoModel moduleInfoModel)
		{
			if (ModuleInfoModel.ShouldIncludeInSession(moduleInfo))
			{
				moduleInfoModel = new ModuleInfoModel(moduleInfo);
				return true;
			}
			moduleInfoModel = null;
			return false;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0000C98C File Offset: 0x0000AB8C
		public override bool Equals(object obj)
		{
			ModuleInfoModel moduleInfoModel;
			return (moduleInfoModel = obj as ModuleInfoModel) != null && this.Id == moduleInfoModel.Id && this.Version == moduleInfoModel.Version;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0000C9C9 File Offset: 0x0000ABC9
		public override int GetHashCode()
		{
			return (-612338121 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Version);
		}
	}
}
