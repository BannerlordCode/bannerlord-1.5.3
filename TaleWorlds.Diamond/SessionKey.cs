using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000028 RID: 40
	[DataContract]
	[Serializable]
	public struct SessionKey
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000340A File Offset: 0x0000160A
		public Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003412 File Offset: 0x00001612
		public SessionKey(Guid guid)
		{
			this._guid = guid;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000341B File Offset: 0x0000161B
		public SessionKey(byte[] b)
		{
			this._guid = new Guid(b);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003429 File Offset: 0x00001629
		public static SessionKey NewGuid()
		{
			return new SessionKey(Guid.NewGuid());
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003438 File Offset: 0x00001638
		public override string ToString()
		{
			return this._guid.ToString();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000345C File Offset: 0x0000165C
		public byte[] ToByteArray()
		{
			return this._guid.ToByteArray();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003477 File Offset: 0x00001677
		public static bool operator ==(SessionKey a, SessionKey b)
		{
			return a._guid == b._guid;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000348A File Offset: 0x0000168A
		public static bool operator !=(SessionKey a, SessionKey b)
		{
			return a._guid != b._guid;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000034A0 File Offset: 0x000016A0
		public override bool Equals(object o)
		{
			if (o != null && o is SessionKey)
			{
				SessionKey sessionKey = (SessionKey)o;
				return this._guid.Equals(sessionKey.Guid);
			}
			return false;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000034D8 File Offset: 0x000016D8
		public override int GetHashCode()
		{
			return this._guid.GetHashCode();
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000034F9 File Offset: 0x000016F9
		public static SessionKey Empty
		{
			get
			{
				return new SessionKey(Guid.Empty);
			}
		}

		// Token: 0x04000047 RID: 71
		[DataMember]
		private readonly Guid _guid;
	}
}
