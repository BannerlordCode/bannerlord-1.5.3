using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000047 RID: 71
	[Tutorial("AssignRolesTutorial")]
	public class AssignRolesTutorial : TutorialItemBase
	{
		// Token: 0x06000154 RID: 340 RVA: 0x000047F2 File Offset: 0x000029F2
		public AssignRolesTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = "RoleAssignmentWidget";
			base.MouseRequired = true;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00004813 File Offset: 0x00002A13
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.ClanScreen;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00004816 File Offset: 0x00002A16
		public override void OnClanRoleAssignedThroughClanScreen(ClanRoleAssignedThroughClanScreenEvent obj)
		{
			this._playerAssignedRoleToClanMember = true;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000481F File Offset: 0x00002A1F
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.PlayerHasUnassignedRolesAndMember;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004826 File Offset: 0x00002A26
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerAssignedRoleToClanMember;
		}

		// Token: 0x0400005D RID: 93
		private bool _playerAssignedRoleToClanMember;
	}
}
