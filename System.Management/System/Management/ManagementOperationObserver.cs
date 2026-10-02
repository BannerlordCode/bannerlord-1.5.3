using System;

namespace System.Management
{
	// Token: 0x0200001D RID: 29
	public class ManagementOperationObserver
	{
		// Token: 0x0600014E RID: 334 RVA: 0x000032C5 File Offset: 0x000014C5
		public ManagementOperationObserver()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600014F RID: 335 RVA: 0x000032D7 File Offset: 0x000014D7
		// (remove) Token: 0x06000150 RID: 336 RVA: 0x000032D9 File Offset: 0x000014D9
		public event CompletedEventHandler Completed
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000151 RID: 337 RVA: 0x000032DB File Offset: 0x000014DB
		// (remove) Token: 0x06000152 RID: 338 RVA: 0x000032DD File Offset: 0x000014DD
		public event ObjectPutEventHandler ObjectPut
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000153 RID: 339 RVA: 0x000032DF File Offset: 0x000014DF
		// (remove) Token: 0x06000154 RID: 340 RVA: 0x000032E1 File Offset: 0x000014E1
		public event ObjectReadyEventHandler ObjectReady
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000155 RID: 341 RVA: 0x000032E3 File Offset: 0x000014E3
		// (remove) Token: 0x06000156 RID: 342 RVA: 0x000032E5 File Offset: 0x000014E5
		public event ProgressEventHandler Progress
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000032E7 File Offset: 0x000014E7
		public void Cancel()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
