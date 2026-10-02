using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000146 RID: 326
	[Serializable]
	public struct PlayerSessionId
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0000D0BD File Offset: 0x0000B2BD
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x0000D0C5 File Offset: 0x0000B2C5
		[JsonProperty]
		public Guid Guid
		{
			get
			{
				return this._guid;
			}
			private set
			{
				this._guid = value;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0000D0CE File Offset: 0x0000B2CE
		public SessionKey SessionKey
		{
			get
			{
				return new SessionKey(this._guid);
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0000D0DB File Offset: 0x0000B2DB
		public PlayerSessionId(Guid guid)
		{
			this._guid = guid;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		public PlayerSessionId(SessionKey sessionKey)
		{
			this._guid = sessionKey.Guid;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000D0F3 File Offset: 0x0000B2F3
		public static PlayerSessionId NewGuid()
		{
			return new PlayerSessionId(Guid.NewGuid());
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000D0FF File Offset: 0x0000B2FF
		public override string ToString()
		{
			return this._guid.ToString();
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000D112 File Offset: 0x0000B312
		public byte[] ToByteArray()
		{
			return this._guid.ToByteArray();
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000D11F File Offset: 0x0000B31F
		public static bool operator ==(PlayerSessionId a, PlayerSessionId b)
		{
			return a._guid == b._guid;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000D132 File Offset: 0x0000B332
		public static bool operator !=(PlayerSessionId a, PlayerSessionId b)
		{
			return a._guid != b._guid;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000D148 File Offset: 0x0000B348
		public override bool Equals(object o)
		{
			return o != null && o is PlayerSessionId && this._guid.Equals(((PlayerSessionId)o).Guid);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000D17B File Offset: 0x0000B37B
		public override int GetHashCode()
		{
			return this._guid.GetHashCode();
		}

		// Token: 0x040003DE RID: 990
		private Guid _guid;
	}
}
