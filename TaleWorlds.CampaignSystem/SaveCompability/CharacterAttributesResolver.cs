using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D5 RID: 213
	public class CharacterAttributesResolver : IConflictResolver
	{
		// Token: 0x060014EC RID: 5356 RVA: 0x000622DD File Offset: 0x000604DD
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00062300 File Offset: 0x00060500
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0006232A File Offset: 0x0006052A
		public Type GetNewType()
		{
			return typeof(PropertyOwner<CharacterAttribute>);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00062336 File Offset: 0x00060536
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
