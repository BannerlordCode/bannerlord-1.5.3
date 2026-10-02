using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000031 RID: 49
	public abstract class TextureProvider
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000F149 File Offset: 0x0000D349
		// (set) Token: 0x06000362 RID: 866 RVA: 0x0000F151 File Offset: 0x0000D351
		public string SourceInfo { get; set; }

		// Token: 0x06000363 RID: 867 RVA: 0x0000F15A File Offset: 0x0000D35A
		public virtual void SetTargetSize(int width, int height)
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000F15C File Offset: 0x0000D35C
		public Texture GetTextureForRender(TwoDimensionContext context, string name = null)
		{
			return this.OnGetTextureForRender(context, name);
		}

		// Token: 0x06000365 RID: 869
		protected abstract Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name);

		// Token: 0x06000366 RID: 870 RVA: 0x0000F166 File Offset: 0x0000D366
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000F168 File Offset: 0x0000D368
		public virtual void Clear(bool clearNextFrame)
		{
			this._getGetMethodCache.Clear();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000F178 File Offset: 0x0000D378
		public void SetProperty(string name, object value)
		{
			PropertyInfo property = base.GetType().GetProperty(name);
			if (property != null)
			{
				property.GetSetMethod().Invoke(this, new object[] { value });
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
		public object GetProperty(string name)
		{
			MethodInfo methodInfo;
			if (this._getGetMethodCache.TryGetValue(name, out methodInfo))
			{
				return methodInfo.Invoke(this, null);
			}
			PropertyInfo property = base.GetType().GetProperty(name);
			if (property != null)
			{
				MethodInfo getMethod = property.GetGetMethod();
				this._getGetMethodCache.Add(name, getMethod);
				return getMethod.Invoke(this, null);
			}
			return null;
		}

		// Token: 0x040001AC RID: 428
		private Dictionary<string, MethodInfo> _getGetMethodCache = new Dictionary<string, MethodInfo>();
	}
}
