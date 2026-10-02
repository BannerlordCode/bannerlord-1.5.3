using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D9 RID: 217
	public class HeroDeveloperResolver : IConflictResolver
	{
		// Token: 0x060014FD RID: 5373 RVA: 0x000624B3 File Offset: 0x000606B3
		public bool IsApplicable(ApplicationVersion version)
		{
			return version != ApplicationVersion.Empty && version.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0));
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x000624D8 File Offset: 0x000606D8
		public MemberTypeId GetFieldMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId == new MemberTypeId(3, 10))
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(this.GetNewType()), 0);
			}
			if (memberTypeId.TypeLevel >= 4)
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(typeof(HeroDeveloper)), memberTypeId.LocalSaveId);
			}
			return MemberTypeId.Invalid;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00062530 File Offset: 0x00060730
		public Type GetNewType()
		{
			return typeof(HeroDeveloper);
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0006253C File Offset: 0x0006073C
		public MemberTypeId GetPropertyMemberWithId(MemberTypeId memberTypeId)
		{
			if (memberTypeId.TypeLevel >= 4)
			{
				return new MemberTypeId(TypeDefinitionBase.GetClassLevel(typeof(HeroDeveloper)), memberTypeId.LocalSaveId);
			}
			return MemberTypeId.Invalid;
		}
	}
}
