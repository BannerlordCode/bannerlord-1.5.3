using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000018 RID: 24
	public class PropertyBasedTooltipVM : TooltipBaseVM
	{
		// Token: 0x0600012F RID: 303 RVA: 0x0000471B File Offset: 0x0000291B
		public PropertyBasedTooltipVM(Type invokedType, object[] invokedArgs)
			: base(invokedType, invokedArgs)
		{
			this.TooltipPropertyList = new MBBindingList<TooltipProperty>();
			this._isPeriodicRefreshEnabled = true;
			this._periodicRefreshDelay = 2f;
			this.Refresh();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004748 File Offset: 0x00002948
		protected override void OnFinalizeInternal()
		{
			base.IsActive = false;
			this.TooltipPropertyList.Clear();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000475C File Offset: 0x0000295C
		public static void AddKeyType(string keyID, Func<string> getKeyText)
		{
			PropertyBasedTooltipVM._keyTextGetters.Add(keyID, getKeyText);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000476A File Offset: 0x0000296A
		public string GetKeyText(string keyID)
		{
			if (PropertyBasedTooltipVM._keyTextGetters.ContainsKey(keyID))
			{
				return PropertyBasedTooltipVM._keyTextGetters[keyID]();
			}
			return "";
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004790 File Offset: 0x00002990
		protected override void OnPeriodicRefresh()
		{
			base.OnPeriodicRefresh();
			foreach (TooltipProperty tooltipProperty in this.TooltipPropertyList)
			{
				tooltipProperty.RefreshDefinition();
				tooltipProperty.RefreshValue();
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000047E8 File Offset: 0x000029E8
		protected override void OnIsExtendedChanged()
		{
			if (base.IsActive)
			{
				base.IsActive = false;
				this.TooltipPropertyList.Clear();
				this.Refresh();
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000480A File Offset: 0x00002A0A
		private void Refresh()
		{
			base.InvokeRefreshData<PropertyBasedTooltipVM>(this);
			if (this.TooltipPropertyList.Count > 0)
			{
				base.IsActive = true;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00004828 File Offset: 0x00002A28
		public static void RefreshGenericPropertyBasedTooltip(PropertyBasedTooltipVM propertyBasedTooltip, object[] args)
		{
			IEnumerable<TooltipProperty> enumerable = args[0] as List<TooltipProperty>;
			propertyBasedTooltip.Mode = 0;
			Func<TooltipProperty, bool> <>9__0;
			Func<TooltipProperty, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (TooltipProperty p) => (p.OnlyShowWhenExtended && propertyBasedTooltip.IsExtended) || (!p.OnlyShowWhenExtended && p.OnlyShowWhenNotExtended && !propertyBasedTooltip.IsExtended) || (!p.OnlyShowWhenExtended && !p.OnlyShowWhenNotExtended));
			}
			foreach (TooltipProperty tooltipProperty in enumerable.Where<TooltipProperty>(func))
			{
				propertyBasedTooltip.AddPropertyDuplicate(tooltipProperty);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000048BC File Offset: 0x00002ABC
		public void AddProperty(string definition, string value, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			TooltipProperty tooltipProperty = new TooltipProperty(definition, value, textHeight, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000048E4 File Offset: 0x00002AE4
		public void AddModifierProperty(string definition, int modifierValue, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			string text = ((modifierValue > 0) ? ("+" + modifierValue.ToString()) : modifierValue.ToString());
			TooltipProperty tooltipProperty = new TooltipProperty(definition, text, textHeight, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004928 File Offset: 0x00002B28
		public void AddProperty(string definition, Func<string> value, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			TooltipProperty tooltipProperty = new TooltipProperty(definition, value, textHeight, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004950 File Offset: 0x00002B50
		public void AddProperty(Func<string> definition, Func<string> value, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			TooltipProperty tooltipProperty = new TooltipProperty(definition, value, textHeight, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004978 File Offset: 0x00002B78
		public void AddColoredProperty(string definition, string value, Color color, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			if (color == Colors.Black)
			{
				this.AddProperty(definition, value, textHeight, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			TooltipProperty tooltipProperty = new TooltipProperty(definition, value, textHeight, color, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000049B8 File Offset: 0x00002BB8
		public void AddColoredProperty(string definition, Func<string> value, Color color, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			if (color == Colors.Black)
			{
				this.AddProperty(definition, value, textHeight, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			TooltipProperty tooltipProperty = new TooltipProperty(definition, value, textHeight, color, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000049F8 File Offset: 0x00002BF8
		public void AddColoredProperty(Func<string> definition, Func<string> value, Color color, int textHeight = 0, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None)
		{
			if (color == Colors.Black)
			{
				this.AddProperty(definition, value, textHeight, TooltipProperty.TooltipPropertyFlags.None);
				return;
			}
			TooltipProperty tooltipProperty = new TooltipProperty(definition, value, textHeight, color, false, propertyFlags);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004A38 File Offset: 0x00002C38
		private void AddPropertyDuplicate(TooltipProperty property)
		{
			TooltipProperty tooltipProperty = new TooltipProperty(property);
			this.TooltipPropertyList.Add(tooltipProperty);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00004A58 File Offset: 0x00002C58
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00004A60 File Offset: 0x00002C60
		[DataSourceProperty]
		public MBBindingList<TooltipProperty> TooltipPropertyList
		{
			get
			{
				return this._tooltipPropertyList;
			}
			set
			{
				if (value != this._tooltipPropertyList)
				{
					this._tooltipPropertyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<TooltipProperty>>(value, "TooltipPropertyList");
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00004A7E File Offset: 0x00002C7E
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00004A86 File Offset: 0x00002C86
		[DataSourceProperty]
		public int Mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (value != this._mode)
				{
					this._mode = value;
					base.OnPropertyChangedWithValue(value, "Mode");
				}
			}
		}

		// Token: 0x04000083 RID: 131
		private static Dictionary<string, Func<string>> _keyTextGetters = new Dictionary<string, Func<string>>();

		// Token: 0x04000084 RID: 132
		private MBBindingList<TooltipProperty> _tooltipPropertyList;

		// Token: 0x04000085 RID: 133
		private int _mode;

		// Token: 0x02000036 RID: 54
		public enum TooltipMode
		{
			// Token: 0x040000E1 RID: 225
			DefaultGame,
			// Token: 0x040000E2 RID: 226
			DefaultCampaign,
			// Token: 0x040000E3 RID: 227
			Ally,
			// Token: 0x040000E4 RID: 228
			Enemy,
			// Token: 0x040000E5 RID: 229
			War
		}
	}
}
