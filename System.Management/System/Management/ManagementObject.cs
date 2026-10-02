using System;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x0200001A RID: 26
	public class ManagementObject : ManagementBaseObject, ICloneable
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00002E2C File Offset: 0x0000102C
		public ManagementObject()
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00002E54 File Offset: 0x00001054
		public ManagementObject(ManagementPath path)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00002E7C File Offset: 0x0000107C
		public ManagementObject(ManagementPath path, ObjectGetOptions options)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00002EA4 File Offset: 0x000010A4
		public ManagementObject(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00002ECC File Offset: 0x000010CC
		protected ManagementObject(SerializationInfo info, StreamingContext context)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00002EF4 File Offset: 0x000010F4
		public ManagementObject(string path)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00002F1C File Offset: 0x0000111C
		public ManagementObject(string path, ObjectGetOptions options)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00002F44 File Offset: 0x00001144
		public ManagementObject(string scopeString, string pathString, ObjectGetOptions options)
			: base(null, default(StreamingContext))
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00002F6B File Offset: 0x0000116B
		public override ManagementPath ClassPath
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00002F77 File Offset: 0x00001177
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00002F83 File Offset: 0x00001183
		public ObjectGetOptions Options
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

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00002F8F File Offset: 0x0000118F
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00002F9B File Offset: 0x0000119B
		public virtual ManagementPath Path
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00002FA7 File Offset: 0x000011A7
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00002FB3 File Offset: 0x000011B3
		public ManagementScope Scope
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

		// Token: 0x0600010F RID: 271 RVA: 0x00002FBF File Offset: 0x000011BF
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00002FCB File Offset: 0x000011CB
		public void CopyTo(ManagementOperationObserver watcher, ManagementPath path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00002FD7 File Offset: 0x000011D7
		public void CopyTo(ManagementOperationObserver watcher, ManagementPath path, PutOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00002FE3 File Offset: 0x000011E3
		public void CopyTo(ManagementOperationObserver watcher, string path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00002FEF File Offset: 0x000011EF
		public void CopyTo(ManagementOperationObserver watcher, string path, PutOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00002FFB File Offset: 0x000011FB
		public ManagementPath CopyTo(ManagementPath path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003007 File Offset: 0x00001207
		public ManagementPath CopyTo(ManagementPath path, PutOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00003013 File Offset: 0x00001213
		public ManagementPath CopyTo(string path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000301F File Offset: 0x0000121F
		public ManagementPath CopyTo(string path, PutOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000302B File Offset: 0x0000122B
		public void Delete()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003037 File Offset: 0x00001237
		public void Delete(DeleteOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00003043 File Offset: 0x00001243
		public void Delete(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000304F File Offset: 0x0000124F
		public void Delete(ManagementOperationObserver watcher, DeleteOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000305B File Offset: 0x0000125B
		public new void Dispose()
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000305D File Offset: 0x0000125D
		public void Get()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00003069 File Offset: 0x00001269
		public void Get(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00003075 File Offset: 0x00001275
		public ManagementBaseObject GetMethodParameters(string methodName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00003081 File Offset: 0x00001281
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000308D File Offset: 0x0000128D
		public ManagementObjectCollection GetRelated()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003099 File Offset: 0x00001299
		public void GetRelated(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000030A5 File Offset: 0x000012A5
		public void GetRelated(ManagementOperationObserver watcher, string relatedClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000030B1 File Offset: 0x000012B1
		public void GetRelated(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000030BD File Offset: 0x000012BD
		public ManagementObjectCollection GetRelated(string relatedClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000030C9 File Offset: 0x000012C9
		public ManagementObjectCollection GetRelated(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000030D5 File Offset: 0x000012D5
		public ManagementObjectCollection GetRelationships()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000030E1 File Offset: 0x000012E1
		public void GetRelationships(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000030ED File Offset: 0x000012ED
		public void GetRelationships(ManagementOperationObserver watcher, string relationshipClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000030F9 File Offset: 0x000012F9
		public void GetRelationships(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00003105 File Offset: 0x00001305
		public ManagementObjectCollection GetRelationships(string relationshipClass)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003111 File Offset: 0x00001311
		public ManagementObjectCollection GetRelationships(string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000311D File Offset: 0x0000131D
		public void InvokeMethod(ManagementOperationObserver watcher, string methodName, ManagementBaseObject inParameters, InvokeMethodOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003129 File Offset: 0x00001329
		public void InvokeMethod(ManagementOperationObserver watcher, string methodName, object[] args)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003135 File Offset: 0x00001335
		public ManagementBaseObject InvokeMethod(string methodName, ManagementBaseObject inParameters, InvokeMethodOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003141 File Offset: 0x00001341
		public object InvokeMethod(string methodName, object[] args)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000314D File Offset: 0x0000134D
		public ManagementPath Put()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00003159 File Offset: 0x00001359
		public void Put(ManagementOperationObserver watcher)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003165 File Offset: 0x00001365
		public void Put(ManagementOperationObserver watcher, PutOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003171 File Offset: 0x00001371
		public ManagementPath Put(PutOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000317D File Offset: 0x0000137D
		public override string ToString()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
