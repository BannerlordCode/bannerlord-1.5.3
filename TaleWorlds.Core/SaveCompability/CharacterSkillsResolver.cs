using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.Core.SaveCompability
{
	// Token: 0x020000E3 RID: 227
	public class CharacterSkillsResolver : IConflictResolver
	{
		// Token: 0x06000B8E RID: 2958 RVA: 0x00025672 File Offset: 0x00023872
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00025695 File Offset: 0x00023895
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x000256BF File Offset: 0x000238BF
		public Type GetNewType()
		{
			return typeof(PropertyOwner<SkillObject>);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x000256CB File Offset: 0x000238CB
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
