using System;
using System.Reflection;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000065 RID: 101
	public abstract class MemberDefinition
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000EDC1 File Offset: 0x0000CFC1
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000EDC9 File Offset: 0x0000CFC9
		public MemberTypeId Id { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000EDD2 File Offset: 0x0000CFD2
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000EDDA File Offset: 0x0000CFDA
		public MemberInfo MemberInfo { get; private set; }

		// Token: 0x06000360 RID: 864 RVA: 0x0000EDE3 File Offset: 0x0000CFE3
		protected MemberDefinition(MemberInfo memberInfo, MemberTypeId id)
		{
			this.MemberInfo = memberInfo;
			this.Id = id;
		}

		// Token: 0x06000361 RID: 865
		public abstract Type GetMemberType();

		// Token: 0x06000362 RID: 866
		public abstract object GetValue(object target);
	}
}
