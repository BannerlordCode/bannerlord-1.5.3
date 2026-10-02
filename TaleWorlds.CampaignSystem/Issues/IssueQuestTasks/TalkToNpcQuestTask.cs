using System;

namespace TaleWorlds.CampaignSystem.Issues.IssueQuestTasks
{
	// Token: 0x0200039D RID: 925
	public class TalkToNpcQuestTask : QuestTaskBase
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x000DECC7 File Offset: 0x000DCEC7
		public TalkToNpcQuestTask(Hero hero, Action onSucceededAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, null, null)
		{
			this._character = hero.CharacterObject;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000DECDF File Offset: 0x000DCEDF
		public TalkToNpcQuestTask(CharacterObject character, Action onSucceededAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, null, null)
		{
			this._character = character;
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x000DECF2 File Offset: 0x000DCEF2
		public bool IsTaskCharacter()
		{
			return this._character == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x000DED01 File Offset: 0x000DCF01
		protected override void OnFinished()
		{
			this._character = null;
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x000DED0A File Offset: 0x000DCF0A
		public override void SetReferences()
		{
		}

		// Token: 0x04000F50 RID: 3920
		private CharacterObject _character;
	}
}
