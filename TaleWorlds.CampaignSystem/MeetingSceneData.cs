using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000091 RID: 145
	public struct MeetingSceneData
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x00056771 File Offset: 0x00054971
		// (set) Token: 0x060012B4 RID: 4788 RVA: 0x00056779 File Offset: 0x00054979
		public string SceneID { get; private set; }

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00056782 File Offset: 0x00054982
		// (set) Token: 0x060012B6 RID: 4790 RVA: 0x0005678A File Offset: 0x0005498A
		public string CultureString { get; private set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00056793 File Offset: 0x00054993
		public CultureObject Culture
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CultureObject>(this.CultureString);
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000567A5 File Offset: 0x000549A5
		public MeetingSceneData(string sceneID, string cultureString)
		{
			this.SceneID = sceneID;
			this.CultureString = cultureString;
		}
	}
}
