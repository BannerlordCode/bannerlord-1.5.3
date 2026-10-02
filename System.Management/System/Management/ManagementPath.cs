using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x0200001F RID: 31
	public class ManagementPath : ICloneable
	{
		// Token: 0x0600015E RID: 350 RVA: 0x0000332B File Offset: 0x0000152B
		public ManagementPath()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000333D File Offset: 0x0000153D
		public ManagementPath(string path)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000334F File Offset: 0x0000154F
		// (set) Token: 0x06000161 RID: 353 RVA: 0x0000335B File Offset: 0x0000155B
		[RefreshProperties(RefreshProperties.All)]
		public string ClassName
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00003367 File Offset: 0x00001567
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00003373 File Offset: 0x00001573
		public static ManagementPath DefaultPath
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000337F File Offset: 0x0000157F
		public bool IsClass
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000338B File Offset: 0x0000158B
		public bool IsInstance
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00003397 File Offset: 0x00001597
		public bool IsSingleton
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000033A3 File Offset: 0x000015A3
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000033AF File Offset: 0x000015AF
		[RefreshProperties(RefreshProperties.All)]
		public string NamespacePath
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000033BB File Offset: 0x000015BB
		// (set) Token: 0x0600016A RID: 362 RVA: 0x000033C7 File Offset: 0x000015C7
		[RefreshProperties(RefreshProperties.All)]
		public string Path
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000033D3 File Offset: 0x000015D3
		// (set) Token: 0x0600016C RID: 364 RVA: 0x000033DF File Offset: 0x000015DF
		[RefreshProperties(RefreshProperties.All)]
		public string RelativePath
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600016D RID: 365 RVA: 0x000033EB File Offset: 0x000015EB
		// (set) Token: 0x0600016E RID: 366 RVA: 0x000033F7 File Offset: 0x000015F7
		[RefreshProperties(RefreshProperties.All)]
		public string Server
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

		// Token: 0x0600016F RID: 367 RVA: 0x00003403 File Offset: 0x00001603
		public ManagementPath Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000340F File Offset: 0x0000160F
		public void SetAsClass()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000341B File Offset: 0x0000161B
		public void SetAsSingleton()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003427 File Offset: 0x00001627
		object ICloneable.Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003433 File Offset: 0x00001633
		public override string ToString()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
