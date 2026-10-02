using System;
using System.Linq;

namespace TaleWorlds.MountAndBlade.Launcher.Library.UserDatas
{
	// Token: 0x02000018 RID: 24
	public class UserData
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000059CA File Offset: 0x00003BCA
		// (set) Token: 0x0600010B RID: 267 RVA: 0x000059D2 File Offset: 0x00003BD2
		public GameType GameType { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000059DB File Offset: 0x00003BDB
		// (set) Token: 0x0600010D RID: 269 RVA: 0x000059E3 File Offset: 0x00003BE3
		public UserGameTypeData SingleplayerData { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000059EC File Offset: 0x00003BEC
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000059F4 File Offset: 0x00003BF4
		public UserGameTypeData MultiplayerData { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000059FD File Offset: 0x00003BFD
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00005A05 File Offset: 0x00003C05
		public DLLCheckDataCollection DLLCheckData { get; set; }

		// Token: 0x06000112 RID: 274 RVA: 0x00005A0E File Offset: 0x00003C0E
		public UserData()
		{
			this.GameType = GameType.Singleplayer;
			this.SingleplayerData = new UserGameTypeData();
			this.MultiplayerData = new UserGameTypeData();
			this.DLLCheckData = new DLLCheckDataCollection();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005A40 File Offset: 0x00003C40
		public UserModData GetUserModData(bool isMultiplayer, string id)
		{
			return (isMultiplayer ? this.MultiplayerData : this.SingleplayerData).ModDatas.Find((UserModData x) => x.Id == id);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005A84 File Offset: 0x00003C84
		public uint? GetDLLLatestSizeInBytes(string dllName)
		{
			DLLCheckData dllcheckData = this.DLLCheckData.DLLData.FirstOrDefault<DLLCheckData>((DLLCheckData d) => d.DLLName == dllName);
			if (dllcheckData == null)
			{
				return null;
			}
			return new uint?(dllcheckData.LatestSizeInBytes);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public bool GetDLLLatestIsDangerous(string dllName)
		{
			DLLCheckData dllcheckData = this.DLLCheckData.DLLData.FirstOrDefault<DLLCheckData>((DLLCheckData d) => d.DLLName == dllName);
			return dllcheckData == null || dllcheckData.IsDangerous;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005B18 File Offset: 0x00003D18
		public string GetDLLLatestVerifyInformation(string dllName)
		{
			DLLCheckData dllcheckData = this.DLLCheckData.DLLData.FirstOrDefault<DLLCheckData>((DLLCheckData d) => d.DLLName == dllName);
			return ((dllcheckData != null) ? dllcheckData.DLLVerifyInformation : null) ?? "";
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005B64 File Offset: 0x00003D64
		public void SetDLLLatestSizeInBytes(string dllName, uint sizeInBytes)
		{
			this.EnsureDLLIsAdded(dllName);
			this.DLLCheckData.DLLData.Find((DLLCheckData d) => d.DLLName == dllName).LatestSizeInBytes = sizeInBytes;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005BAC File Offset: 0x00003DAC
		public void SetDLLLatestVerifyInformation(string dllName, string verifyInformation)
		{
			this.EnsureDLLIsAdded(dllName);
			this.DLLCheckData.DLLData.Find((DLLCheckData d) => d.DLLName == dllName).DLLVerifyInformation = verifyInformation;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005BF4 File Offset: 0x00003DF4
		public void SetDLLLatestIsDangerous(string dllName, bool isDangerous)
		{
			this.EnsureDLLIsAdded(dllName);
			this.DLLCheckData.DLLData.Find((DLLCheckData d) => d.DLLName == dllName).IsDangerous = isDangerous;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005C3C File Offset: 0x00003E3C
		private void EnsureDLLIsAdded(string dllName)
		{
			if (!this.DLLCheckData.DLLData.Any<DLLCheckData>((DLLCheckData d) => d.DLLName == dllName))
			{
				this.DLLCheckData.DLLData.Add(new DLLCheckData(dllName));
			}
		}
	}
}
