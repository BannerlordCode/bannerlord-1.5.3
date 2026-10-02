using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x02000017 RID: 23
	[ToolboxItem(false)]
	public class ManagementEventWatcher : Component
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00002C6C File Offset: 0x00000E6C
		public ManagementEventWatcher()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00002C7E File Offset: 0x00000E7E
		public ManagementEventWatcher(EventQuery query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00002C90 File Offset: 0x00000E90
		public ManagementEventWatcher(ManagementScope scope, EventQuery query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public ManagementEventWatcher(ManagementScope scope, EventQuery query, EventWatcherOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public ManagementEventWatcher(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00002CC6 File Offset: 0x00000EC6
		public ManagementEventWatcher(string scope, string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public ManagementEventWatcher(string scope, string query, EventWatcherOptions options)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00002CEA File Offset: 0x00000EEA
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00002CF6 File Offset: 0x00000EF6
		public EventWatcherOptions Options
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

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00002D02 File Offset: 0x00000F02
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00002D0E File Offset: 0x00000F0E
		public EventQuery Query
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00002D1A File Offset: 0x00000F1A
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00002D26 File Offset: 0x00000F26
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000EB RID: 235 RVA: 0x00002D32 File Offset: 0x00000F32
		// (remove) Token: 0x060000EC RID: 236 RVA: 0x00002D34 File Offset: 0x00000F34
		public event EventArrivedEventHandler EventArrived
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000ED RID: 237 RVA: 0x00002D36 File Offset: 0x00000F36
		// (remove) Token: 0x060000EE RID: 238 RVA: 0x00002D38 File Offset: 0x00000F38
		public event StoppedEventHandler Stopped
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00002D3A File Offset: 0x00000F3A
		public void Start()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00002D46 File Offset: 0x00000F46
		public void Stop()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00002D52 File Offset: 0x00000F52
		public ManagementBaseObject WaitForNextEvent()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
