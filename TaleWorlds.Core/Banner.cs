using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200000F RID: 15
	public class Banner
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002E64 File Offset: 0x00001064
		public string BannerCode
		{
			get
			{
				string text;
				if ((text = this._bannerCode) == null)
				{
					text = (this._bannerCode = this.Serialize());
				}
				return text;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002E8A File Offset: 0x0000108A
		public MBReadOnlyList<BannerData> BannerDataList
		{
			get
			{
				return this._bannerDataList;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002E94 File Offset: 0x00001094
		public IBannerVisual BannerVisual
		{
			get
			{
				IBannerVisual bannerVisual;
				if ((bannerVisual = this._bannerVisual) == null)
				{
					bannerVisual = (this._bannerVisual = Game.Current.CreateBannerVisual(this));
				}
				return bannerVisual;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002EBF File Offset: 0x000010BF
		public Banner()
		{
			this._bannerDataList = new MBList<BannerData>();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002ED4 File Offset: 0x000010D4
		public Banner(Banner banner)
			: this()
		{
			this._bannerCode = banner._bannerCode;
			foreach (BannerData bannerData in banner._bannerDataList)
			{
				this._bannerDataList.Add(new BannerData(bannerData));
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002F44 File Offset: 0x00001144
		public Banner(Banner banner, uint color1, uint color2)
			: this(banner)
		{
			this.ChangePrimaryColor(color1);
			this.ChangeIconColors(color2);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002F5B File Offset: 0x0000115B
		public Banner(string bannerKey)
			: this()
		{
			if (string.IsNullOrEmpty(bannerKey))
			{
				Debug.FailedAssert("Banner key is empty!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\Banner.cs", ".ctor", 73);
				return;
			}
			this.Deserialize(bannerKey);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002F89 File Offset: 0x00001189
		public Banner(string bannerKey, uint color1, uint color2)
			: this(bannerKey)
		{
			this.ChangePrimaryColor(color1);
			this.ChangeIconColors(color2);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002FA0 File Offset: 0x000011A0
		public void SetBannerVisual(IBannerVisual visual)
		{
			this._bannerVisual = visual;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002FA9 File Offset: 0x000011A9
		public BannerData GetBannerDataAtIndex(int index)
		{
			this._bannerCode = null;
			if (this._bannerDataList.Count <= index)
			{
				return null;
			}
			return this._bannerDataList[index];
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002FCE File Offset: 0x000011CE
		public int GetBannerDataListCount()
		{
			return this._bannerDataList.Count;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002FDB File Offset: 0x000011DB
		public bool IsBannerDataListEmpty()
		{
			return this._bannerDataList.IsEmpty<BannerData>();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002FE8 File Offset: 0x000011E8
		public int GetPrimaryColorId()
		{
			return this._bannerDataList[0].ColorId;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002FFB File Offset: 0x000011FB
		public int GetSecondaryColorId()
		{
			return this._bannerDataList[0].ColorId2;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000300E File Offset: 0x0000120E
		public int GetIconColorId()
		{
			return this._bannerDataList[1].ColorId;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003021 File Offset: 0x00001221
		public Vec2 GetIconSize()
		{
			return this._bannerDataList[1].Size;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003034 File Offset: 0x00001234
		public void SetPrimaryColorId(int colorId)
		{
			this._bannerCode = null;
			this._bannerDataList[0].ColorId = colorId;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000304F File Offset: 0x0000124F
		public void SetSecondaryColorId(int colorId)
		{
			this._bannerCode = null;
			this._bannerDataList[0].ColorId2 = colorId;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000306A File Offset: 0x0000126A
		public void SetIconColorId(int colorId)
		{
			this._bannerCode = null;
			this._bannerDataList[1].ColorId = colorId;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003085 File Offset: 0x00001285
		public void SetIconSize(int newSize)
		{
			this._bannerCode = null;
			this._bannerDataList[1].Size = new Vec2((float)newSize, (float)newSize);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000030A8 File Offset: 0x000012A8
		public void ChangePrimaryColor(uint mainColor)
		{
			int colorId = BannerManager.GetColorId(mainColor);
			if (colorId < 0)
			{
				return;
			}
			this._bannerCode = null;
			this._bannerDataList[0].ColorId = colorId;
			this._bannerDataList[0].ColorId2 = colorId;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000030EC File Offset: 0x000012EC
		public void ChangeBackgroundColor(uint primaryColor, uint secondaryColor)
		{
			int colorId = BannerManager.GetColorId(primaryColor);
			int colorId2 = BannerManager.GetColorId(secondaryColor);
			if (colorId < 0)
			{
				return;
			}
			if (colorId2 < 0)
			{
				return;
			}
			this._bannerCode = null;
			this._bannerDataList[0].ColorId = colorId;
			this._bannerDataList[0].ColorId2 = colorId2;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000313C File Offset: 0x0000133C
		public void ChangeIconColors(uint color)
		{
			int colorId = BannerManager.GetColorId(color);
			if (colorId < 0)
			{
				return;
			}
			this._bannerCode = null;
			for (int i = 1; i < this._bannerDataList.Count; i++)
			{
				this._bannerDataList[i].ColorId = colorId;
				this._bannerDataList[i].ColorId2 = colorId;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003198 File Offset: 0x00001398
		public void RotateBackgroundToRight()
		{
			this._bannerCode = null;
			this._bannerDataList[0].RotationValue -= 0.0027777778f;
			this._bannerDataList[0].RotationValue = ((this._bannerDataList[0].RotationValue < 0f) ? (this._bannerDataList[0].RotationValue + 1f) : this._bannerDataList[0].RotationValue);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000321C File Offset: 0x0000141C
		public void RotateBackgroundToLeft()
		{
			this._bannerCode = null;
			this._bannerDataList[0].RotationValue += 0.0027777778f;
			this._bannerDataList[0].RotationValue = ((this._bannerDataList[0].RotationValue > 0f) ? (this._bannerDataList[0].RotationValue - 1f) : this._bannerDataList[0].RotationValue);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000032A0 File Offset: 0x000014A0
		public int GetBackgroundMeshId()
		{
			return this._bannerDataList[0].MeshId;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000032B3 File Offset: 0x000014B3
		public int GetIconMeshId()
		{
			return this._bannerDataList[1].MeshId;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000032C6 File Offset: 0x000014C6
		public void SetBackgroundMeshId(int meshId)
		{
			this._bannerCode = null;
			this._bannerDataList[0].MeshId = meshId;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000032E1 File Offset: 0x000014E1
		public void SetIconMeshId(int meshId)
		{
			this._bannerCode = null;
			this._bannerDataList[1].MeshId = meshId;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000032FC File Offset: 0x000014FC
		public string Serialize()
		{
			return Banner.GetBannerCodeFromBannerDataList(this._bannerDataList);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000330C File Offset: 0x0000150C
		public void Deserialize(string message)
		{
			this._bannerCode = message;
			this._bannerVisual = null;
			this._bannerDataList.Clear();
			List<BannerData> list;
			if (Banner.TryGetBannerDataFromCode(message, out list))
			{
				this._bannerDataList.AddRange(list);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003348 File Offset: 0x00001548
		public void ClearAllIcons()
		{
			this._bannerCode = null;
			BannerData bannerData = this._bannerDataList[0];
			this._bannerDataList.Clear();
			this._bannerDataList.Add(bannerData);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003380 File Offset: 0x00001580
		public void AddIconData(BannerData iconData)
		{
			if (this._bannerDataList.Count < 33)
			{
				this._bannerCode = null;
				this._bannerDataList.Add(iconData);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000033A4 File Offset: 0x000015A4
		public void AddIconData(BannerData iconData, int index)
		{
			if (this._bannerDataList.Count < 33 && index > 0 && index <= this._bannerDataList.Count)
			{
				this._bannerDataList.Insert(index, iconData);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000033D4 File Offset: 0x000015D4
		public void RemoveIconDataAtIndex(int index)
		{
			if (index > 0 && index < this._bannerDataList.Count)
			{
				this._bannerDataList.RemoveAt(index);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000033F4 File Offset: 0x000015F4
		public static Banner CreateRandomClanBanner(int seed = -1)
		{
			return Banner.CreateRandomBannerInternal(seed, Banner.BannerIconOrientation.CentralPositionedOneIcon);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000033FD File Offset: 0x000015FD
		public static Banner CreateRandomBanner()
		{
			return Banner.CreateRandomBannerInternal(-1, Banner.BannerIconOrientation.None);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003408 File Offset: 0x00001608
		private static Banner CreateRandomBannerInternal(int seed = -1, Banner.BannerIconOrientation orientation = Banner.BannerIconOrientation.None)
		{
			Game game = Game.Current;
			MBFastRandom mbfastRandom = ((seed == -1) ? new MBFastRandom() : new MBFastRandom((uint)seed));
			Banner banner = new Banner();
			BannerData bannerData = new BannerData(BannerManager.Instance.GetRandomBackgroundId(mbfastRandom), BannerManager.Instance.GetRandomColorId(mbfastRandom), BannerManager.Instance.GetRandomColorId(mbfastRandom), new Vec2(1528f, 1528f), new Vec2(764f, 764f), false, false, 0f);
			banner.AddIconData(bannerData);
			switch ((orientation == Banner.BannerIconOrientation.None) ? mbfastRandom.Next(6) : ((int)orientation))
			{
			case 0:
				banner.CentralPositionedOneIcon(mbfastRandom);
				break;
			case 1:
				banner.CenteredTwoMirroredIcons(mbfastRandom);
				break;
			case 2:
				banner.DiagonalIcons(mbfastRandom);
				break;
			case 3:
				banner.HorizontalIcons(mbfastRandom);
				break;
			case 4:
				banner.VerticalIcons(mbfastRandom);
				break;
			case 5:
				banner.SquarePositionedFourIcons(mbfastRandom);
				break;
			}
			return banner;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000034EC File Offset: 0x000016EC
		public static Banner CreateOneColoredEmptyBanner(int colorIndex)
		{
			Banner banner = new Banner();
			BannerData bannerData = new BannerData(BannerManager.Instance.GetRandomBackgroundId(new MBFastRandom()), colorIndex, colorIndex, new Vec2(1528f, 1528f), new Vec2(764f, 764f), false, false, 0f);
			banner.AddIconData(bannerData);
			return banner;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003544 File Offset: 0x00001744
		public static Banner CreateOneColoredBannerWithOneIcon(uint backgroundColor, uint iconColor, int iconMeshId)
		{
			Banner banner = Banner.CreateOneColoredEmptyBanner(BannerManager.GetColorId(backgroundColor));
			if (iconMeshId == -1)
			{
				iconMeshId = BannerManager.Instance.GetRandomBannerIconId(new MBFastRandom());
			}
			banner.AddIconData(new BannerData(iconMeshId, BannerManager.GetColorId(iconColor), BannerManager.GetColorId(iconColor), new Vec2(512f, 512f), new Vec2(764f, 764f), false, false, 0f));
			return banner;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000035B0 File Offset: 0x000017B0
		private void CentralPositionedOneIcon(MBFastRandom random)
		{
			int randomBannerIconId = BannerManager.Instance.GetRandomBannerIconId(random);
			int randomColorId = BannerManager.Instance.GetRandomColorId(random);
			bool flag = random.NextFloat() < 0.5f;
			int num = (flag ? BannerManager.Instance.GetRandomColorId(random) : BannerManager.Instance.ReadOnlyColorPalette.Last<KeyValuePair<int, BannerColor>>().Key);
			bool flag2 = random.Next(2) == 0;
			float num2 = random.NextFloat();
			float num3 = 0f;
			if (num2 > 0.9f)
			{
				num3 = 0.25f;
			}
			else if (num2 > 0.8f)
			{
				num3 = 0.5f;
			}
			else if (num2 > 0.7f)
			{
				num3 = 0.75f;
			}
			BannerData bannerData = new BannerData(randomBannerIconId, randomColorId, num, new Vec2(512f, 512f), new Vec2(764f, 764f), flag, flag2, num3);
			this.AddIconData(bannerData);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000368C File Offset: 0x0000188C
		private void DiagonalIcons(MBFastRandom random)
		{
			int num = ((random.NextFloat() < 0.5f) ? 2 : 3);
			bool flag = random.NextFloat() < 0.5f;
			int num2 = (512 - 20 * (num + 1)) / num;
			int num3 = BannerManager.Instance.GetRandomBannerIconId(random);
			int num4 = BannerManager.Instance.GetRandomColorId(random);
			bool flag2 = random.NextFloat() < 0.5f;
			int num5 = (flag2 ? BannerManager.Instance.GetRandomColorId(random) : BannerManager.Instance.ReadOnlyColorPalette.Last<KeyValuePair<int, BannerColor>>().Key);
			int num6 = (512 - num * num2) / (num + 1);
			bool flag3 = random.NextFloat() < 0.3f;
			bool flag4 = flag3 || random.NextFloat() < 0.3f;
			for (int i = 0; i < num; i++)
			{
				num3 = (flag3 ? BannerManager.Instance.GetRandomBannerIconId(random) : num3);
				num4 = (flag4 ? BannerManager.Instance.GetRandomColorId(random) : num4);
				int num7 = i * (num2 + num6) + num6 + num2 / 2;
				int num8 = i * (num2 + num6) + num6 + num2 / 2;
				if (flag)
				{
					num8 = 512 - num8;
				}
				BannerData bannerData = new BannerData(num3, num4, num5, new Vec2((float)num2, (float)num2), new Vec2((float)(num7 + 508), (float)(num8 + 508)), flag2, false, 0f);
				this.AddIconData(bannerData);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000037F8 File Offset: 0x000019F8
		private void HorizontalIcons(MBFastRandom random)
		{
			int num = ((random.NextFloat() < 0.5f) ? 2 : 3);
			int num2 = (512 - 20 * (num + 1)) / num;
			int num3 = BannerManager.Instance.GetRandomBannerIconId(random);
			int num4 = BannerManager.Instance.GetRandomColorId(random);
			bool flag = random.NextFloat() < 0.5f;
			int num5 = (flag ? BannerManager.Instance.GetRandomColorId(random) : BannerManager.Instance.ReadOnlyColorPalette.Last<KeyValuePair<int, BannerColor>>().Key);
			int num6 = (512 - num * num2) / (num + 1);
			bool flag2 = random.NextFloat() < 0.3f;
			bool flag3 = flag2 || random.NextFloat() < 0.3f;
			for (int i = 0; i < num; i++)
			{
				num3 = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num3);
				num4 = (flag3 ? BannerManager.Instance.GetRandomColorId(random) : num4);
				int num7 = i * (num2 + num6) + num6 + num2 / 2;
				BannerData bannerData = new BannerData(num3, num4, num5, new Vec2((float)num2, (float)num2), new Vec2((float)(num7 + 508), 764f), flag, false, 0f);
				this.AddIconData(bannerData);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000392C File Offset: 0x00001B2C
		private void VerticalIcons(MBFastRandom random)
		{
			int num = ((random.NextFloat() < 0.5f) ? 2 : 3);
			int num2 = (512 - 20 * (num + 1)) / num;
			int num3 = BannerManager.Instance.GetRandomBannerIconId(random);
			int num4 = BannerManager.Instance.GetRandomColorId(random);
			bool flag = random.NextFloat() < 0.5f;
			int num5 = (flag ? BannerManager.Instance.GetRandomColorId(random) : BannerManager.Instance.ReadOnlyColorPalette.Last<KeyValuePair<int, BannerColor>>().Key);
			int num6 = (512 - num * num2) / (num + 1);
			bool flag2 = random.NextFloat() < 0.3f;
			bool flag3 = flag2 || random.NextFloat() < 0.3f;
			for (int i = 0; i < num; i++)
			{
				num3 = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num3);
				num4 = (flag3 ? BannerManager.Instance.GetRandomColorId(random) : num4);
				int num7 = i * (num2 + num6) + num6 + num2 / 2;
				BannerData bannerData = new BannerData(num3, num4, num5, new Vec2((float)num2, (float)num2), new Vec2(764f, (float)(num7 + 508)), flag, false, 0f);
				this.AddIconData(bannerData);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003A60 File Offset: 0x00001C60
		private void SquarePositionedFourIcons(MBFastRandom random)
		{
			bool flag = random.NextFloat() < 0.5f;
			bool flag2 = !flag && random.NextFloat() < 0.5f;
			bool flag3 = flag2 || random.NextFloat() < 0.5f;
			bool flag4 = random.NextFloat() < 0.5f;
			int num = BannerManager.Instance.GetRandomBannerIconId(random);
			int num2 = (flag4 ? BannerManager.Instance.GetRandomColorId(random) : BannerManager.Instance.ReadOnlyColorPalette.Last<KeyValuePair<int, BannerColor>>().Key);
			int num3 = BannerManager.Instance.GetRandomColorId(random);
			BannerData bannerData = new BannerData(num, num3, num2, new Vec2(220f, 220f), new Vec2(654f, 654f), flag4, false, 0f);
			this.AddIconData(bannerData);
			num = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num);
			num3 = (flag3 ? BannerManager.Instance.GetRandomColorId(random) : num3);
			bannerData = new BannerData(num, num3, num2, new Vec2(220f, 220f), new Vec2(874f, 654f), flag4, flag, 0f);
			this.AddIconData(bannerData);
			num = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num);
			num3 = (flag3 ? BannerManager.Instance.GetRandomColorId(random) : num3);
			bannerData = new BannerData(num, num3, num2, new Vec2(220f, 220f), new Vec2(654f, 874f), flag4, flag, flag ? 0.5f : 0f);
			this.AddIconData(bannerData);
			num = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num);
			num3 = (flag3 ? BannerManager.Instance.GetRandomColorId(random) : num3);
			bannerData = new BannerData(num, num3, num2, new Vec2(220f, 220f), new Vec2(874f, 874f), flag4, false, flag ? 0.5f : 0f);
			this.AddIconData(bannerData);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003C5C File Offset: 0x00001E5C
		private void CenteredTwoMirroredIcons(MBFastRandom random)
		{
			bool flag = random.NextFloat() < 0.5f;
			bool flag2 = random.NextFloat() < 0.5f;
			int randomBannerIconId = BannerManager.Instance.GetRandomBannerIconId(random);
			int num = (flag2 ? BannerManager.Instance.GetRandomColorId(random) : BannerManager.Instance.ReadOnlyColorPalette.Last<KeyValuePair<int, BannerColor>>().Key);
			int num2 = BannerManager.Instance.GetRandomColorId(random);
			BannerData bannerData = new BannerData(randomBannerIconId, num2, num, new Vec2(200f, 200f), new Vec2(664f, 764f), flag2, false, 0f);
			this.AddIconData(bannerData);
			num2 = (flag ? BannerManager.Instance.GetRandomColorId(random) : num2);
			bannerData = new BannerData(randomBannerIconId, num2, num, new Vec2(200f, 200f), new Vec2(864f, 764f), flag2, true, 0f);
			this.AddIconData(bannerData);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003D44 File Offset: 0x00001F44
		public uint GetPrimaryColor()
		{
			if (this._bannerDataList.Count <= 0)
			{
				return uint.MaxValue;
			}
			return BannerManager.GetColor(this._bannerDataList[0].ColorId);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003D6C File Offset: 0x00001F6C
		public uint GetSecondaryColor()
		{
			if (this._bannerDataList.Count <= 0)
			{
				return uint.MaxValue;
			}
			return BannerManager.GetColor(this._bannerDataList[0].ColorId2);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003D94 File Offset: 0x00001F94
		public uint GetFirstIconColor()
		{
			if (this._bannerDataList.Count <= 1)
			{
				return uint.MaxValue;
			}
			return BannerManager.GetColor(this._bannerDataList[1].ColorId);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003DBC File Offset: 0x00001FBC
		public int GetVersionNo()
		{
			int num = 0;
			for (int i = 0; i < this._bannerDataList.Count; i++)
			{
				num += this._bannerDataList[i].LocalVersion;
			}
			return num;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public static string GetBannerCodeFromBannerDataList(MBList<BannerData> bannerDataList)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetBannerCodeFromBannerDataList");
			bool flag = true;
			foreach (BannerData bannerData in bannerDataList)
			{
				if (!flag)
				{
					mbstringBuilder.Append('.');
				}
				flag = false;
				mbstringBuilder.Append(bannerData.MeshId);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.ColorId);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.ColorId2);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Size.x);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Size.y);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Position.x);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Position.y);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.DrawStroke ? 1 : 0);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.Mirror ? 1 : 0);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)(bannerData.RotationValue / 0.0027777778f));
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003F90 File Offset: 0x00002190
		public static bool IsValidBannerCode(string bannerCode)
		{
			List<BannerData> list;
			return !string.IsNullOrEmpty(bannerCode) && Banner.TryGetBannerDataFromCode(bannerCode, out list);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003FB0 File Offset: 0x000021B0
		public static bool TryGetBannerDataFromCode(string bannerCode, out List<BannerData> bannerDataList)
		{
			bannerDataList = new List<BannerData>();
			string[] array = bannerCode.Split(new char[] { '.' });
			int num = 0;
			while (num + 10 <= array.Length)
			{
				int num2;
				int num3;
				int num4;
				int num5;
				int num6;
				int num7;
				int num8;
				int num9;
				int num10;
				int num11;
				if (!int.TryParse(array[num], out num2) || !int.TryParse(array[num + 1], out num3) || !int.TryParse(array[num + 2], out num4) || !int.TryParse(array[num + 3], out num5) || !int.TryParse(array[num + 4], out num6) || !int.TryParse(array[num + 5], out num7) || !int.TryParse(array[num + 6], out num8) || !int.TryParse(array[num + 7], out num9) || !int.TryParse(array[num + 8], out num10) || !int.TryParse(array[num + 9], out num11))
				{
					bannerDataList.Clear();
					return false;
				}
				BannerData bannerData = new BannerData(num2, num3, num4, new Vec2((float)num5, (float)num6), new Vec2((float)num7, (float)num8), num9 == 1, num10 == 1, (float)num11 * 0.0027777778f);
				bannerDataList.Add(bannerData);
				num += 10;
			}
			int count = bannerDataList.Count;
			return true;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000040DC File Offset: 0x000022DC
		internal static void AutoGeneratedStaticCollectObjectsBanner(object o, List<object> collectedObjects)
		{
			((Banner)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000040EA File Offset: 0x000022EA
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._bannerDataList);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000040F8 File Offset: 0x000022F8
		internal static object AutoGeneratedGetMemberValue_bannerDataList(object o)
		{
			return ((Banner)o)._bannerDataList;
		}

		// Token: 0x040000FC RID: 252
		public const int MaxSize = 8000;

		// Token: 0x040000FD RID: 253
		public const int BannerFullSize = 1528;

		// Token: 0x040000FE RID: 254
		public const int BannerEditableAreaSize = 512;

		// Token: 0x040000FF RID: 255
		public const int MaxIconCount = 32;

		// Token: 0x04000100 RID: 256
		private const char Splitter = '.';

		// Token: 0x04000101 RID: 257
		public const int BackgroundDataIndex = 0;

		// Token: 0x04000102 RID: 258
		public const int BannerIconDataIndex = 1;

		// Token: 0x04000103 RID: 259
		[CachedData]
		private string _bannerCode;

		// Token: 0x04000104 RID: 260
		[SaveableField(1)]
		private readonly MBList<BannerData> _bannerDataList;

		// Token: 0x04000105 RID: 261
		[CachedData]
		private IBannerVisual _bannerVisual;

		// Token: 0x020000F3 RID: 243
		private enum BannerIconOrientation
		{
			// Token: 0x040006E6 RID: 1766
			None = -1,
			// Token: 0x040006E7 RID: 1767
			CentralPositionedOneIcon,
			// Token: 0x040006E8 RID: 1768
			CenteredTwoMirroredIcons,
			// Token: 0x040006E9 RID: 1769
			DiagonalIcons,
			// Token: 0x040006EA RID: 1770
			HorizontalIcons,
			// Token: 0x040006EB RID: 1771
			VerticalIcons,
			// Token: 0x040006EC RID: 1772
			SquarePositionedFourIcons,
			// Token: 0x040006ED RID: 1773
			NumberOfOrientation
		}
	}
}
