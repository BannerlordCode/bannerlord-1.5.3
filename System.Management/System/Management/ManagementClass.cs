using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x02000014 RID: 20
	public class ManagementClass : ManagementObject
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00002A28 File Offset: 0x00000C28
		public ManagementClass()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00002A3A File Offset: 0x00000C3A
		public ManagementClass(ManagementPath path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002A4C File Offset: 0x00000C4C
		public ManagementClass(ManagementPath path, ObjectGetOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002A5E File Offset: 0x00000C5E
		public ManagementClass(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00002A70 File Offset: 0x00000C70
		protected ManagementClass(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002A82 File Offset: 0x00000C82
		public ManagementClass(string path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00002A94 File Offset: 0x00000C94
		public ManagementClass(string path, ObjectGetOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public ManagementClass(string scope, string path, ObjectGetOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public StringCollection Derivation
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00002AC4 File Offset: 0x00000CC4
		public MethodDataCollection Methods
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00002ADC File Offset: 0x00000CDC
		public override ManagementPath Path
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

		// Token: 0x060000BD RID: 189 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public ManagementObject CreateInstance()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002B00 File Offset: 0x00000D00
		public ManagementClass Derive(string newClassName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00002B0C File Offset: 0x00000D0C
		public ManagementObjectCollection GetInstances()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002B18 File Offset: 0x00000D18
		public ManagementObjectCollection GetInstances(EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002B24 File Offset: 0x00000D24
		public void GetInstances(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002B30 File Offset: 0x00000D30
		public void GetInstances(ManagementOperationObserver watcher, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00002B3C File Offset: 0x00000D3C
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00002B48 File Offset: 0x00000D48
		public ManagementObjectCollection GetRelatedClasses()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00002B54 File Offset: 0x00000D54
		public void GetRelatedClasses(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00002B60 File Offset: 0x00000D60
		public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002B6C File Offset: 0x00000D6C
		public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00002B78 File Offset: 0x00000D78
		public ManagementObjectCollection GetRelatedClasses(string relatedClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002B84 File Offset: 0x00000D84
		public ManagementObjectCollection GetRelatedClasses(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002B90 File Offset: 0x00000D90
		public ManagementObjectCollection GetRelationshipClasses()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00002B9C File Offset: 0x00000D9C
		public void GetRelationshipClasses(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public ManagementObjectCollection GetRelationshipClasses(string relationshipClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00002BCC File Offset: 0x00000DCC
		public ManagementObjectCollection GetRelationshipClasses(string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public CodeTypeDeclaration GetStronglyTypedClassCode(bool includeSystemClassInClassDef, bool systemPropertyClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public bool GetStronglyTypedClassCode(CodeLanguage lang, string filePath, string classNamespace)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public ManagementObjectCollection GetSubclasses()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00002BFC File Offset: 0x00000DFC
		public ManagementObjectCollection GetSubclasses(EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002C08 File Offset: 0x00000E08
		public void GetSubclasses(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00002C14 File Offset: 0x00000E14
		public void GetSubclasses(ManagementOperationObserver watcher, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
