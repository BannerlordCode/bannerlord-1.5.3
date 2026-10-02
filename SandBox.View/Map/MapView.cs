using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.View.Map
{
	// Token: 0x0200005D RID: 93
	public abstract class MapView : SandboxView
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0001CA59 File Offset: 0x0001AC59
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0001CA61 File Offset: 0x0001AC61
		public MapScreen MapScreen { get; internal set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0001CA6A File Offset: 0x0001AC6A
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0001CA72 File Offset: 0x0001AC72
		public MapState MapState { get; internal set; }

		// Token: 0x06000388 RID: 904 RVA: 0x0001CA7B File Offset: 0x0001AC7B
		protected internal virtual void CreateLayout()
		{
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001CA7D File Offset: 0x0001AC7D
		protected internal virtual void OnResume()
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001CA7F File Offset: 0x0001AC7F
		protected internal virtual void OnHourlyTick()
		{
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001CA81 File Offset: 0x0001AC81
		protected internal virtual void OnStartWait(string waitMenuId)
		{
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001CA83 File Offset: 0x0001AC83
		protected internal virtual void OnMainPartyEncounter()
		{
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001CA85 File Offset: 0x0001AC85
		protected internal virtual void OnDispersePlayerLeadedArmy()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001CA87 File Offset: 0x0001AC87
		protected internal virtual void OnArmyLeft()
		{
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001CA89 File Offset: 0x0001AC89
		protected internal virtual bool IsEscaped()
		{
			return false;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001CA8C File Offset: 0x0001AC8C
		protected internal virtual bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return true;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001CA8F File Offset: 0x0001AC8F
		protected internal virtual void OnOverlayCreated()
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001CA91 File Offset: 0x0001AC91
		protected internal virtual void OnOverlayClosed()
		{
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001CA93 File Offset: 0x0001AC93
		protected internal virtual void OnMenuModeTick(float dt)
		{
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001CA95 File Offset: 0x0001AC95
		protected internal virtual void OnMapScreenUpdate(float dt)
		{
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001CA97 File Offset: 0x0001AC97
		protected internal virtual void OnIdleTick(float dt)
		{
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001CA99 File Offset: 0x0001AC99
		protected internal virtual void OnMapTerrainClick()
		{
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001CA9B File Offset: 0x0001AC9B
		protected internal virtual void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001CA9D File Offset: 0x0001AC9D
		protected internal virtual void OnMapConversationStart()
		{
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001CA9F File Offset: 0x0001AC9F
		protected internal virtual void OnMapConversationOver()
		{
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001CAA1 File Offset: 0x0001ACA1
		protected internal virtual TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x040001D8 RID: 472
		protected const float ContextAlphaModifier = 8.5f;
	}
}
