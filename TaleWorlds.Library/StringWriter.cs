using System;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x02000092 RID: 146
	public class StringWriter : IWriter
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00012973 File Offset: 0x00010B73
		public string Data
		{
			get
			{
				return this._stringBuilder.ToString();
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00012980 File Offset: 0x00010B80
		public StringWriter()
		{
			this._stringBuilder = new StringBuilder();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00012993 File Offset: 0x00010B93
		private void AddToken(string token)
		{
			this._stringBuilder.Append(token);
			this._stringBuilder.Append(" ");
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000129B3 File Offset: 0x00010BB3
		public void WriteSerializableObject(ISerializableObject serializableObject)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000129BA File Offset: 0x00010BBA
		public void WriteByte(byte value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000129C8 File Offset: 0x00010BC8
		public void WriteBytes(byte[] bytes)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000129CF File Offset: 0x00010BCF
		public void WriteInt(int value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000129DD File Offset: 0x00010BDD
		public void WriteShort(short value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000129EB File Offset: 0x00010BEB
		public void WriteString(string value)
		{
			this.WriteInt(value.Length);
			this.AddToken(value);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00012A00 File Offset: 0x00010C00
		public void WriteColor(Color value)
		{
			this.WriteFloat(value.Red);
			this.WriteFloat(value.Green);
			this.WriteFloat(value.Blue);
			this.WriteFloat(value.Alpha);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00012A32 File Offset: 0x00010C32
		public void WriteBool(bool value)
		{
			this.AddToken(value ? "1" : "0");
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00012A49 File Offset: 0x00010C49
		public void WriteFloat(float value)
		{
			this.AddToken((value == 0f) ? "0" : Convert.ToString(value));
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00012A66 File Offset: 0x00010C66
		public void WriteUInt(uint value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00012A74 File Offset: 0x00010C74
		public void WriteULong(ulong value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00012A82 File Offset: 0x00010C82
		public void WriteLong(long value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00012A90 File Offset: 0x00010C90
		public void WriteVec2(Vec2 vec2)
		{
			this.WriteFloat(vec2.x);
			this.WriteFloat(vec2.y);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00012AAA File Offset: 0x00010CAA
		public void WriteVec3(Vec3 vec3)
		{
			this.WriteFloat(vec3.x);
			this.WriteFloat(vec3.y);
			this.WriteFloat(vec3.z);
			this.WriteFloat(vec3.w);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00012ADC File Offset: 0x00010CDC
		public void WriteVec3Int(Vec3i vec3)
		{
			this.WriteInt(vec3.X);
			this.WriteInt(vec3.Y);
			this.WriteInt(vec3.Z);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00012B02 File Offset: 0x00010D02
		public void WriteSByte(sbyte value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00012B10 File Offset: 0x00010D10
		public void WriteUShort(ushort value)
		{
			this.AddToken(Convert.ToString(value));
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00012B1E File Offset: 0x00010D1E
		public void WriteDouble(double value)
		{
			this.AddToken((value == 0.0) ? "0" : Convert.ToString(value));
		}

		// Token: 0x0400019C RID: 412
		private StringBuilder _stringBuilder;
	}
}
