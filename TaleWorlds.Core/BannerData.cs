using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000013 RID: 19
	public class BannerData
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000425A File Offset: 0x0000245A
		public int LocalVersion
		{
			get
			{
				return this._localVersion;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004262 File Offset: 0x00002462
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000426A File Offset: 0x0000246A
		public int MeshId
		{
			get
			{
				return this._meshId;
			}
			set
			{
				if (value != this._meshId)
				{
					this._meshId = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000428A File Offset: 0x0000248A
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00004292 File Offset: 0x00002492
		public int ColorId
		{
			get
			{
				return this._colorId;
			}
			set
			{
				if (value != this._colorId)
				{
					this._colorId = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x000042B2 File Offset: 0x000024B2
		// (set) Token: 0x060000CA RID: 202 RVA: 0x000042BA File Offset: 0x000024BA
		public int ColorId2
		{
			get
			{
				return this._colorId2;
			}
			set
			{
				if (value != this._colorId2)
				{
					this._colorId2 = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000042DA File Offset: 0x000024DA
		// (set) Token: 0x060000CC RID: 204 RVA: 0x000042E2 File Offset: 0x000024E2
		public Vec2 Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (value != this._size)
				{
					this._size = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00004307 File Offset: 0x00002507
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000430F File Offset: 0x0000250F
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004334 File Offset: 0x00002534
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000433C File Offset: 0x0000253C
		public bool DrawStroke
		{
			get
			{
				return this._drawStroke;
			}
			set
			{
				if (value != this._drawStroke)
				{
					this._drawStroke = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000435C File Offset: 0x0000255C
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00004364 File Offset: 0x00002564
		public bool Mirror
		{
			get
			{
				return this._mirror;
			}
			set
			{
				if (value != this._mirror)
				{
					this._mirror = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004384 File Offset: 0x00002584
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000438C File Offset: 0x0000258C
		public float RotationValue
		{
			get
			{
				return this._rotationValue;
			}
			set
			{
				if (value != this._rotationValue)
				{
					this._rotationValue = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000043AC File Offset: 0x000025AC
		public float Rotation
		{
			get
			{
				return 6.2831855f * this.RotationValue;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000043BC File Offset: 0x000025BC
		public BannerData(int meshId, int colorId, int colorId2, Vec2 size, Vec2 position, bool drawStroke, bool mirror, float rotationValue)
		{
			this.MeshId = meshId;
			this.ColorId = colorId;
			this.ColorId2 = colorId2;
			this.Size = size;
			this.Position = position;
			this.DrawStroke = drawStroke;
			this.Mirror = mirror;
			this.RotationValue = rotationValue;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000440C File Offset: 0x0000260C
		public BannerData(BannerData bannerData)
			: this(bannerData.MeshId, bannerData.ColorId, bannerData.ColorId2, bannerData.Size, bannerData.Position, bannerData.DrawStroke, bannerData.Mirror, bannerData.RotationValue)
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004450 File Offset: 0x00002650
		public override bool Equals(object obj)
		{
			BannerData bannerData;
			return (bannerData = obj as BannerData) != null && bannerData.MeshId == this.MeshId && bannerData.ColorId == this.ColorId && bannerData.ColorId2 == this.ColorId2 && bannerData.Size.X == this.Size.X && bannerData.Size.Y == this.Size.Y && bannerData.Position.X == this.Position.X && bannerData.Position.Y == this.Position.Y && bannerData.DrawStroke == this.DrawStroke && bannerData.Mirror == this.Mirror && bannerData.RotationValue == this.RotationValue;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004545 File Offset: 0x00002745
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000454D File Offset: 0x0000274D
		internal static void AutoGeneratedStaticCollectObjectsBannerData(object o, List<object> collectedObjects)
		{
			((BannerData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000455B File Offset: 0x0000275B
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000455D File Offset: 0x0000275D
		internal static object AutoGeneratedGetMemberValue_colorId2(object o)
		{
			return ((BannerData)o)._colorId2;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000456F File Offset: 0x0000276F
		internal static object AutoGeneratedGetMemberValue_size(object o)
		{
			return ((BannerData)o)._size;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004581 File Offset: 0x00002781
		internal static object AutoGeneratedGetMemberValue_position(object o)
		{
			return ((BannerData)o)._position;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004593 File Offset: 0x00002793
		internal static object AutoGeneratedGetMemberValue_drawStroke(object o)
		{
			return ((BannerData)o)._drawStroke;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000045A5 File Offset: 0x000027A5
		internal static object AutoGeneratedGetMemberValue_mirror(object o)
		{
			return ((BannerData)o)._mirror;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000045B7 File Offset: 0x000027B7
		internal static object AutoGeneratedGetMemberValue_rotationValue(object o)
		{
			return ((BannerData)o)._rotationValue;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000045C9 File Offset: 0x000027C9
		internal static object AutoGeneratedGetMemberValue_meshId(object o)
		{
			return ((BannerData)o)._meshId;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000045DB File Offset: 0x000027DB
		internal static object AutoGeneratedGetMemberValue_colorId(object o)
		{
			return ((BannerData)o)._colorId;
		}

		// Token: 0x04000108 RID: 264
		public const float RotationPrecision = 0.0027777778f;

		// Token: 0x04000109 RID: 265
		[CachedData]
		private int _localVersion;

		// Token: 0x0400010A RID: 266
		[SaveableField(1)]
		private int _meshId;

		// Token: 0x0400010B RID: 267
		[SaveableField(2)]
		private int _colorId;

		// Token: 0x0400010C RID: 268
		[SaveableField(3)]
		public int _colorId2;

		// Token: 0x0400010D RID: 269
		[SaveableField(4)]
		public Vec2 _size;

		// Token: 0x0400010E RID: 270
		[SaveableField(5)]
		public Vec2 _position;

		// Token: 0x0400010F RID: 271
		[SaveableField(6)]
		public bool _drawStroke;

		// Token: 0x04000110 RID: 272
		[SaveableField(7)]
		public bool _mirror;

		// Token: 0x04000111 RID: 273
		[SaveableField(8)]
		public float _rotationValue;
	}
}
