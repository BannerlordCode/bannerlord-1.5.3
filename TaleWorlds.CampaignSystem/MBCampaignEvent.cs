using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003E RID: 62
	public class MBCampaignEvent
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001F6F6 File Offset: 0x0001D8F6
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0001F6FE File Offset: 0x0001D8FE
		public CampaignTime TriggerPeriod { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001F707 File Offset: 0x0001D907
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0001F70F File Offset: 0x0001D90F
		public CampaignTime InitialWait { get; private set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0001F718 File Offset: 0x0001D918
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x0001F720 File Offset: 0x0001D920
		public bool isEventDeleted { get; set; }

		// Token: 0x06000402 RID: 1026 RVA: 0x0001F729 File Offset: 0x0001D929
		public MBCampaignEvent(string eventName)
		{
			this.description = eventName;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001F743 File Offset: 0x0001D943
		public MBCampaignEvent(CampaignTime triggerPeriod, CampaignTime initialWait)
		{
			this.TriggerPeriod = triggerPeriod;
			this.InitialWait = initialWait;
			this.NextTriggerTime = CampaignTime.Now + this.InitialWait;
			this.isEventDeleted = false;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001F781 File Offset: 0x0001D981
		public void AddHandler(MBCampaignEvent.CampaignEventDelegate gameEventDelegate)
		{
			this.handlers.Add(gameEventDelegate);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001F790 File Offset: 0x0001D990
		public void RunHandlers(params object[] delegateParams)
		{
			for (int i = 0; i < this.handlers.Count; i++)
			{
				this.handlers[i](this, delegateParams);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001F7C8 File Offset: 0x0001D9C8
		public void Unregister(object instance)
		{
			for (int i = 0; i < this.handlers.Count; i++)
			{
				if (this.handlers[i].Target == instance)
				{
					this.handlers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001F810 File Offset: 0x0001DA10
		public void CheckUpdate()
		{
			while (this.NextTriggerTime.IsPast && !this.isEventDeleted)
			{
				this.RunHandlers(new object[] { CampaignTime.Now });
				this.NextTriggerTime += this.TriggerPeriod;
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001F864 File Offset: 0x0001DA64
		public void DeletePeriodicEvent()
		{
			this.isEventDeleted = true;
		}

		// Token: 0x04000189 RID: 393
		public string description;

		// Token: 0x0400018A RID: 394
		protected List<MBCampaignEvent.CampaignEventDelegate> handlers = new List<MBCampaignEvent.CampaignEventDelegate>();

		// Token: 0x0400018B RID: 395
		[CachedData]
		protected CampaignTime NextTriggerTime;

		// Token: 0x02000531 RID: 1329
		// (Invoke) Token: 0x06004EE7 RID: 20199
		public delegate void CampaignEventDelegate(MBCampaignEvent campaignEvent, params object[] delegateParams);
	}
}
