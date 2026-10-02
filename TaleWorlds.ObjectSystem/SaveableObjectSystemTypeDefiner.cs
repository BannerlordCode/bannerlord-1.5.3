using System;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x02000011 RID: 17
	public class SaveableObjectSystemTypeDefiner : SaveableTypeDefiner
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00004F66 File Offset: 0x00003166
		public SaveableObjectSystemTypeDefiner()
			: base(10000)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004F73 File Offset: 0x00003173
		protected override void DefineBasicTypes()
		{
			base.DefineBasicTypes();
			base.AddBasicTypeDefinition(typeof(MBGUID), 1005, new MBGUIDBasicTypeSerializer());
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004F95 File Offset: 0x00003195
		protected override void DefineClassTypes()
		{
			base.AddClassDefinition(typeof(MBObjectBase), 34, null);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004FAA File Offset: 0x000031AA
		protected override void DefineStructTypes()
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004FAC File Offset: 0x000031AC
		protected override void DefineEnumTypes()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004FAE File Offset: 0x000031AE
		protected override void DefineInterfaceTypes()
		{
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004FB0 File Offset: 0x000031B0
		protected override void DefineRootClassTypes()
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004FB2 File Offset: 0x000031B2
		protected override void DefineGenericClassDefinitions()
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004FB4 File Offset: 0x000031B4
		protected override void DefineGenericStructDefinitions()
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004FB6 File Offset: 0x000031B6
		protected override void DefineContainerDefinitions()
		{
		}
	}
}
