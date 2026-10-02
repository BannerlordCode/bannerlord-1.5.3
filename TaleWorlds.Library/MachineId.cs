using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x02000060 RID: 96
	public static class MachineId
	{
		// Token: 0x060002BB RID: 699 RVA: 0x00008512 File Offset: 0x00006712
		public static void Initialize()
		{
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				MachineId.MachineIdString = "nonwindows";
				return;
			}
			MachineId.MachineIdString = MachineId.ProcessId();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00008535 File Offset: 0x00006735
		public static int AsInteger()
		{
			if (!string.IsNullOrEmpty(MachineId.MachineIdString))
			{
				return BitConverter.ToInt32(Encoding.ASCII.GetBytes(MachineId.MachineIdString), 0);
			}
			return 0;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000855A File Offset: 0x0000675A
		private static string ProcessId()
		{
			return "" + MachineId.GetMotherboardIdentifier() + MachineId.GetCpuIdentifier() + MachineId.GetDiskIdentifier();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00008580 File Offset: 0x00006780
		private static string GetMotherboardIdentifier()
		{
			string text = "";
			try
			{
				using (ManagementObjectCollection instances = new ManagementClass("win32_baseboard").GetInstances())
				{
					foreach (ManagementBaseObject managementBaseObject in instances)
					{
						string text2 = (((ManagementObject)managementBaseObject)["SerialNumber"] as string).Trim(new char[] { ' ' });
						text += text2.Replace("-", "");
					}
				}
			}
			catch (Exception)
			{
				return "";
			}
			return text;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00008648 File Offset: 0x00006848
		private static string GetCpuIdentifier()
		{
			string text = "";
			try
			{
				using (ManagementObjectCollection instances = new ManagementClass("win32_processor").GetInstances())
				{
					foreach (ManagementBaseObject managementBaseObject in instances)
					{
						string text2 = ((ManagementObject)managementBaseObject)["ProcessorId"] as string;
						if (text2 != null)
						{
							string text3 = text2.Trim(new char[] { ' ' });
							text += text3.Replace("-", "");
						}
					}
				}
			}
			catch (Exception)
			{
				return "";
			}
			return text;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00008718 File Offset: 0x00006918
		private static string GetDiskIdentifier()
		{
			string text = "";
			try
			{
				using (ManagementObjectCollection instances = new ManagementClass("win32_diskdrive").GetInstances())
				{
					foreach (ManagementBaseObject managementBaseObject in instances)
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						if (string.Compare(managementObject["InterfaceType"] as string, "IDE", StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							string text2 = (managementObject["SerialNumber"] as string).Trim(new char[] { ' ' });
							text += text2.Replace("-", "");
						}
					}
				}
			}
			catch (Exception)
			{
				return "";
			}
			return text;
		}

		// Token: 0x04000121 RID: 289
		private static string MachineIdString;
	}
}
