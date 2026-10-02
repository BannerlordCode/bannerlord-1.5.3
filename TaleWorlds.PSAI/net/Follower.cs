using System;

namespace psai.net
{
	// Token: 0x0200001E RID: 30
	public struct Follower
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000210 RID: 528 RVA: 0x000097CD File Offset: 0x000079CD
		// (set) Token: 0x06000211 RID: 529 RVA: 0x000097D5 File Offset: 0x000079D5
		public float compatibility { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000097DE File Offset: 0x000079DE
		// (set) Token: 0x06000213 RID: 531 RVA: 0x000097E6 File Offset: 0x000079E6
		public int snippetId { get; private set; }

		// Token: 0x06000214 RID: 532 RVA: 0x000097EF File Offset: 0x000079EF
		public Follower(int id, float compatibility)
		{
			this.snippetId = id;
			this.compatibility = compatibility;
		}
	}
}
