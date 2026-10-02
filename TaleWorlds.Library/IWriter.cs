using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000045 RID: 69
	public interface IWriter
	{
		// Token: 0x06000232 RID: 562
		void WriteSerializableObject(ISerializableObject serializableObject);

		// Token: 0x06000233 RID: 563
		void WriteByte(byte value);

		// Token: 0x06000234 RID: 564
		void WriteSByte(sbyte value);

		// Token: 0x06000235 RID: 565
		void WriteBytes(byte[] bytes);

		// Token: 0x06000236 RID: 566
		void WriteInt(int value);

		// Token: 0x06000237 RID: 567
		void WriteUInt(uint value);

		// Token: 0x06000238 RID: 568
		void WriteShort(short value);

		// Token: 0x06000239 RID: 569
		void WriteUShort(ushort value);

		// Token: 0x0600023A RID: 570
		void WriteString(string value);

		// Token: 0x0600023B RID: 571
		void WriteColor(Color value);

		// Token: 0x0600023C RID: 572
		void WriteBool(bool value);

		// Token: 0x0600023D RID: 573
		void WriteFloat(float value);

		// Token: 0x0600023E RID: 574
		void WriteDouble(double value);

		// Token: 0x0600023F RID: 575
		void WriteULong(ulong value);

		// Token: 0x06000240 RID: 576
		void WriteLong(long value);

		// Token: 0x06000241 RID: 577
		void WriteVec2(Vec2 vec2);

		// Token: 0x06000242 RID: 578
		void WriteVec3(Vec3 vec3);

		// Token: 0x06000243 RID: 579
		void WriteVec3Int(Vec3i vec3);
	}
}
