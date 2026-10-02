using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000032 RID: 50
	[DataContract]
	[Serializable]
	public abstract class RestData
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00003DF5 File Offset: 0x00001FF5
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00003DFD File Offset: 0x00001FFD
		[DataMember]
		public string TypeName { get; set; }

		// Token: 0x06000132 RID: 306 RVA: 0x00003E06 File Offset: 0x00002006
		protected RestData()
		{
			this.TypeName = base.GetType().FullName;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003E1F File Offset: 0x0000201F
		public string SerializeAsJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
