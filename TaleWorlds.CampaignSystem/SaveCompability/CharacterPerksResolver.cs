using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D6 RID: 214
	public class CharacterPerksResolver : IConflictResolver
	{
		// Token: 0x060014F1 RID: 5361 RVA: 0x00062345 File Offset: 0x00060545
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00062368 File Offset: 0x00060568
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00062392 File Offset: 0x00060592
		public Type GetNewType()
		{
			return typeof(PropertyOwner<PerkObject>);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0006239E File Offset: 0x0006059E
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
