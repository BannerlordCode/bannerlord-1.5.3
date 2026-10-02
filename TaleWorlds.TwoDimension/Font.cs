using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000006 RID: 6
	public class Font
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000291C File Offset: 0x00000B1C
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002924 File Offset: 0x00000B24
		public string Name { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000292D File Offset: 0x00000B2D
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002935 File Offset: 0x00000B35
		public int Size { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000293E File Offset: 0x00000B3E
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002946 File Offset: 0x00000B46
		public int LineHeight { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000294F File Offset: 0x00000B4F
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002957 File Offset: 0x00000B57
		public int Base { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002960 File Offset: 0x00000B60
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002968 File Offset: 0x00000B68
		public int CharacterCount { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002971 File Offset: 0x00000B71
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002979 File Offset: 0x00000B79
		public float SmoothingConstant { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002982 File Offset: 0x00000B82
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000298A File Offset: 0x00000B8A
		public float CustomScale { get; private set; } = 1f;

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002993 File Offset: 0x00000B93
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0000299B File Offset: 0x00000B9B
		public bool Smooth { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000029A4 File Offset: 0x00000BA4
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000029AC File Offset: 0x00000BAC
		public SpritePart FontSprite { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000029B5 File Offset: 0x00000BB5
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000029BD File Offset: 0x00000BBD
		public Dictionary<int, BitmapFontCharacter> Characters { get; private set; }

		// Token: 0x06000035 RID: 53 RVA: 0x000029C6 File Offset: 0x00000BC6
		public Font(string name)
		{
			this.Name = name;
			this.Characters = new Dictionary<int, BitmapFontCharacter>();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000029EC File Offset: 0x00000BEC
		public bool TryLoadFontFromPath(string path, SpriteData spriteData)
		{
			Debug.Print("Loading " + this.Name + " font, at: " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			bool flag;
			try
			{
				this.LoadFromPathAux(path, spriteData);
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("Failed to load font:" + this.Name + " at path: " + path, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.TwoDimension\\BitmapFont\\Font.cs", "TryLoadFontFromPath", 54);
				Debug.Print(string.Format("Failed to load font:{0} at path: {1}. Error:{2}", this.Name, path, ex), 0, Debug.DebugColor.White, 17592186044416UL);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A8C File Offset: 0x00000C8C
		private void LoadFromPathAux(string path, SpriteData spriteData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(path);
			XmlElement xmlElement = xmlDocument["font"];
			XmlElement xmlElement2 = xmlElement["info"];
			this._realSize = Math.Abs(Convert.ToInt32(xmlElement2.Attributes["size"].Value));
			this.Smooth = true;
			if (xmlElement2.Attributes["smooth"] != null)
			{
				this.Smooth = Convert.ToBoolean(Convert.ToInt32(xmlElement2.Attributes["smooth"].Value));
			}
			this.SmoothingConstant = 0.47f;
			if (xmlElement2.Attributes["smoothingConstant"] != null)
			{
				this.SmoothingConstant = Convert.ToSingle(xmlElement2.Attributes["smoothingConstant"].Value, CultureInfo.InvariantCulture);
			}
			if (xmlElement2.Attributes["customScale"] != null)
			{
				this.CustomScale = Convert.ToSingle(xmlElement2.Attributes["customScale"].Value, CultureInfo.InvariantCulture);
			}
			XmlElement xmlElement3 = xmlElement["common"];
			this.LineHeight = Convert.ToInt32(xmlElement3.Attributes["lineHeight"].Value);
			this.Base = Convert.ToInt32(xmlElement3.Attributes["base"].Value);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(xmlElement["pages"].ChildNodes[0].Attributes["file"].Value);
			XmlElement xmlElement4 = xmlElement["chars"];
			this.CharacterCount = Convert.ToInt32(xmlElement4.Attributes["count"].Value);
			string text = Path.ChangeExtension(path, ".bfnt");
			if (File.Exists(text))
			{
				using (BinaryReader binaryReader = new BinaryReader(File.Open(text, FileMode.Open, FileAccess.Read)))
				{
					for (int i = 0; i < this.CharacterCount; i++)
					{
						GCHandle gchandle = GCHandle.Alloc(binaryReader.ReadBytes(Marshal.SizeOf(typeof(BitmapFontCharacter))), GCHandleType.Pinned);
						BitmapFontCharacter bitmapFontCharacter = (BitmapFontCharacter)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(BitmapFontCharacter));
						this.Characters.Add(bitmapFontCharacter.ID, bitmapFontCharacter);
						gchandle.Free();
					}
					goto IL_0398;
				}
			}
			for (int j = 0; j < this.CharacterCount; j++)
			{
				XmlNode xmlNode = xmlElement4.ChildNodes[j];
				BitmapFontCharacter bitmapFontCharacter2;
				bitmapFontCharacter2.ID = Convert.ToInt32(xmlNode.Attributes["id"].Value);
				bitmapFontCharacter2.X = Convert.ToInt32(xmlNode.Attributes["x"].Value);
				bitmapFontCharacter2.Y = Convert.ToInt32(xmlNode.Attributes["y"].Value);
				bitmapFontCharacter2.Width = Convert.ToInt32(xmlNode.Attributes["width"].Value);
				bitmapFontCharacter2.Height = Convert.ToInt32(xmlNode.Attributes["height"].Value);
				bitmapFontCharacter2.XOffset = Convert.ToInt32(xmlNode.Attributes["xoffset"].Value);
				bitmapFontCharacter2.YOffset = Convert.ToInt32(xmlNode.Attributes["yoffset"].Value);
				bitmapFontCharacter2.XAdvance = Convert.ToInt32(xmlNode.Attributes["xadvance"].Value);
				this.Characters.Add(bitmapFontCharacter2.ID, bitmapFontCharacter2);
			}
			IL_0398:
			SpriteGeneric spriteGeneric = spriteData.GetSprite(fileNameWithoutExtension) as SpriteGeneric;
			SpritePart spritePart = ((spriteGeneric != null) ? spriteGeneric.SpritePart : null);
			this.FontSprite = spritePart;
			this.Size = (int)((float)this._realSize / this.CustomScale);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E7C File Offset: 0x0000107C
		public float GetWordWidth(string word, float extraPadding)
		{
			float num = 0f;
			for (int i = 0; i < word.Length; i++)
			{
				num += this.GetCharacterWidth(word[i], extraPadding);
			}
			return num;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002EB4 File Offset: 0x000010B4
		public float GetCharacterWidth(char character, float extraPadding)
		{
			float num = 0f;
			int num2 = (int)character;
			if (!this.Characters.ContainsKey(num2))
			{
				num2 = 0;
			}
			BitmapFontCharacter bitmapFontCharacter = this.Characters[num2];
			return num + ((float)bitmapFontCharacter.XAdvance + extraPadding);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002EEF File Offset: 0x000010EF
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return base.ToString();
			}
			return this.Name;
		}

		// Token: 0x04000020 RID: 32
		private int _realSize;
	}
}
