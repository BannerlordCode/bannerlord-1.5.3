using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Resolvers
{
	// Token: 0x02000032 RID: 50
	public interface IConflictResolver
	{
		// Token: 0x06000213 RID: 531
		bool IsApplicable(ApplicationVersion version);

		// Token: 0x06000214 RID: 532
		Type GetNewType();

		// Token: 0x06000215 RID: 533
		MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId);

		// Token: 0x06000216 RID: 534
		MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId);
	}
}
