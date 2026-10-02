using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000DA RID: 218
	public class HeroTraitDeveloperResolver : IConflictResolver
	{
		// Token: 0x06001502 RID: 5378 RVA: 0x0006256F File Offset: 0x0006076F
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00062592 File Offset: 0x00060792
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000625BC File Offset: 0x000607BC
		public Type GetNewType()
		{
			return typeof(PropertyOwner<PropertyObject>);
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000625C8 File Offset: 0x000607C8
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
