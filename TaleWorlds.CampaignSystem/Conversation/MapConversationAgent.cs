using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x0200023F RID: 575
	public class MapConversationAgent : IAgent
	{
		// Token: 0x060022F7 RID: 8951 RVA: 0x0009AB61 File Offset: 0x00098D61
		public MapConversationAgent(CharacterObject characterObject)
		{
			this._characterObject = characterObject;
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0009AB70 File Offset: 0x00098D70
		public BasicCharacterObject Character
		{
			get
			{
				return this._characterObject;
			}
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0009AB78 File Offset: 0x00098D78
		public bool IsEnemyOf(IAgent agent)
		{
			return false;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0009AB7B File Offset: 0x00098D7B
		public bool IsFriendOf(IAgent agent)
		{
			return true;
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x0009AB7E File Offset: 0x00098D7E
		public AgentState State
		{
			get
			{
				return AgentState.Active;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x0009AB81 File Offset: 0x00098D81
		public IMissionTeam Team
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x0009AB84 File Offset: 0x00098D84
		public IAgentOriginBase Origin
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x0009AB87 File Offset: 0x00098D87
		public float Age
		{
			get
			{
				return this.Character.Age;
			}
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0009AB94 File Offset: 0x00098D94
		public bool IsActive()
		{
			return true;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0009AB97 File Offset: 0x00098D97
		public void SetAsConversationAgent(bool set)
		{
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0009AB99 File Offset: 0x00098D99
		public void OnConversationStarted()
		{
		}

		// Token: 0x04000A2F RID: 2607
		private CharacterObject _characterObject;

		// Token: 0x04000A30 RID: 2608
		public bool DeliveredLine;
	}
}
