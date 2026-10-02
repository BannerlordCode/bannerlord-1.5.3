using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	public class TestAccessObject : AccessObject
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003568 File Offset: 0x00001768
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00003570 File Offset: 0x00001770
		[JsonProperty]
		public string UserName { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00003579 File Offset: 0x00001779
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00003581 File Offset: 0x00001781
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x060000EF RID: 239 RVA: 0x0000358A File Offset: 0x0000178A
		public TestAccessObject()
		{
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003592 File Offset: 0x00001792
		public TestAccessObject(string userName, string password)
		{
			base.Type = "Test";
			this.UserName = userName;
			this.Password = password;
		}
	}
}
