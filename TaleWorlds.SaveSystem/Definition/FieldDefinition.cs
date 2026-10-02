using System;
using System.Reflection;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000061 RID: 97
	public class FieldDefinition : MemberDefinition
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000EB24 File Offset: 0x0000CD24
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000EB2C File Offset: 0x0000CD2C
		public FieldInfo FieldInfo { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000EB35 File Offset: 0x0000CD35
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000EB3D File Offset: 0x0000CD3D
		public SaveableFieldAttribute SaveableFieldAttribute { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000EB46 File Offset: 0x0000CD46
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000EB4E File Offset: 0x0000CD4E
		public GetFieldValueDelegate GetFieldValueMethod { get; private set; }

		// Token: 0x0600034B RID: 843 RVA: 0x0000EB57 File Offset: 0x0000CD57
		public FieldDefinition(FieldInfo fieldInfo, MemberTypeId id)
			: base(fieldInfo, id)
		{
			this.FieldInfo = fieldInfo;
			this.SaveableFieldAttribute = fieldInfo.GetCustomAttribute<SaveableFieldAttribute>();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000EB74 File Offset: 0x0000CD74
		public override Type GetMemberType()
		{
			return this.FieldInfo.FieldType;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000EB84 File Offset: 0x0000CD84
		public override object GetValue(object target)
		{
			object obj;
			if (this.GetFieldValueMethod != null)
			{
				obj = this.GetFieldValueMethod(target);
			}
			else
			{
				obj = this.FieldInfo.GetValue(target);
			}
			return obj;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000EBB6 File Offset: 0x0000CDB6
		public void InitializeForAutoGeneration(GetFieldValueDelegate getFieldValueMethod)
		{
			this.GetFieldValueMethod = getFieldValueMethod;
		}
	}
}
