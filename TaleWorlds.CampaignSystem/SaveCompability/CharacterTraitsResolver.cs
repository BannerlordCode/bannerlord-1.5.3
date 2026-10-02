using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D7 RID: 215
	public class CharacterTraitsResolver : IConflictResolver
	{
		// Token: 0x060014F6 RID: 5366 RVA: 0x000623AD File Offset: 0x000605AD
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000623D0 File Offset: 0x000605D0
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 10);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x000623FA File Offset: 0x000605FA
		public Type GetNewType()
		{
			return typeof(PropertyOwner<TraitObject>);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00062406 File Offset: 0x00060606
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			return MemberTypeId.Invalid;
		}
	}
}
