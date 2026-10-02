using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x02000013 RID: 19
	[ToolboxItem(false)]
	public class ManagementBaseObject : Component, ICloneable, ISerializable
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00002924 File Offset: 0x00000B24
		protected ManagementBaseObject(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00002936 File Offset: 0x00000B36
		public virtual ManagementPath ClassPath
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000065 RID: 101
		public object this[string propertyName]
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000295A File Offset: 0x00000B5A
		public virtual PropertyDataCollection Properties
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00002966 File Offset: 0x00000B66
		public virtual QualifierDataCollection Qualifiers
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00002972 File Offset: 0x00000B72
		public virtual PropertyDataCollection SystemProperties
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000297E File Offset: 0x00000B7E
		public virtual object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000298A File Offset: 0x00000B8A
		public bool CompareTo(ManagementBaseObject otherObject, ComparisonSettings settings)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002996 File Offset: 0x00000B96
		public new void Dispose()
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002998 File Offset: 0x00000B98
		public override bool Equals(object obj)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000029A4 File Offset: 0x00000BA4
		public override int GetHashCode()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000029BC File Offset: 0x00000BBC
		public object GetPropertyQualifierValue(string propertyName, string qualifierName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000029C8 File Offset: 0x00000BC8
		public object GetPropertyValue(string propertyName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000029D4 File Offset: 0x00000BD4
		public object GetQualifierValue(string qualifierName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000029E0 File Offset: 0x00000BE0
		public string GetText(TextFormat format)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000029EC File Offset: 0x00000BEC
		public static explicit operator IntPtr(ManagementBaseObject managementObject)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000029F8 File Offset: 0x00000BF8
		public void SetPropertyQualifierValue(string propertyName, string qualifierName, object qualifierValue)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002A04 File Offset: 0x00000C04
		public void SetPropertyValue(string propertyName, object propertyValue)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00002A10 File Offset: 0x00000C10
		public void SetQualifierValue(string qualifierName, object qualifierValue)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00002A1C File Offset: 0x00000C1C
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
