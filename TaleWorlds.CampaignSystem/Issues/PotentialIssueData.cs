using System;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000397 RID: 919
	public struct PotentialIssueData
	{
		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x000DE590 File Offset: 0x000DC790
		public PotentialIssueData.StartIssueDelegate OnStartIssue { get; }

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x0600363D RID: 13885 RVA: 0x000DE598 File Offset: 0x000DC798
		public string IssueId { get; }

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x000DE5A0 File Offset: 0x000DC7A0
		public Type IssueType { get; }

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x0600363F RID: 13887 RVA: 0x000DE5A8 File Offset: 0x000DC7A8
		public IssueBase.IssueFrequency Frequency { get; }

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x000DE5B0 File Offset: 0x000DC7B0
		public object RelatedObject { get; }

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06003641 RID: 13889 RVA: 0x000DE5B8 File Offset: 0x000DC7B8
		public bool IsValid
		{
			get
			{
				return this.OnStartIssue != null;
			}
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000DE5C3 File Offset: 0x000DC7C3
		public PotentialIssueData(PotentialIssueData.StartIssueDelegate onStartIssue, Type issueType, IssueBase.IssueFrequency frequency, object relatedObject = null)
		{
			this.OnStartIssue = onStartIssue;
			this.IssueId = issueType.Name;
			this.IssueType = issueType;
			this.Frequency = frequency;
			this.RelatedObject = relatedObject;
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000DE5EE File Offset: 0x000DC7EE
		public PotentialIssueData(Type issueType, IssueBase.IssueFrequency frequency)
		{
			this.OnStartIssue = null;
			this.IssueId = issueType.Name;
			this.IssueType = issueType;
			this.Frequency = frequency;
			this.RelatedObject = null;
		}

		// Token: 0x0200079D RID: 1949
		// (Invoke) Token: 0x0600641F RID: 25631
		public delegate IssueBase StartIssueDelegate(in PotentialIssueData pid, Hero issueOwner);
	}
}
