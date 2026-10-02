using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003D RID: 61
	public class PropertyOwnerObject
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x0001027E File Offset: 0x0000E47E
		protected void OnPropertyChanged<T>(T value, [CallerMemberName] string propertyName = null) where T : class
		{
			Action<PropertyOwnerObject, string, object> propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, propertyName, value);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00010298 File Offset: 0x0000E498
		protected void OnPropertyChanged(int value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, int> action = this.intPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000102AD File Offset: 0x0000E4AD
		protected void OnPropertyChanged(float value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, float> action = this.floatPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000102C2 File Offset: 0x0000E4C2
		protected void OnPropertyChanged(bool value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, bool> action = this.boolPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000102D7 File Offset: 0x0000E4D7
		protected void OnPropertyChanged(Vec2 value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, Vec2> vec2PropertyChanged = this.Vec2PropertyChanged;
			if (vec2PropertyChanged == null)
			{
				return;
			}
			vec2PropertyChanged(this, propertyName, value);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000102EC File Offset: 0x0000E4EC
		protected void OnPropertyChanged(Vector2 value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, Vector2> vector2PropertyChanged = this.Vector2PropertyChanged;
			if (vector2PropertyChanged == null)
			{
				return;
			}
			vector2PropertyChanged(this, propertyName, value);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00010301 File Offset: 0x0000E501
		protected void OnPropertyChanged(double value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, double> action = this.doublePropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00010316 File Offset: 0x0000E516
		protected void OnPropertyChanged(uint value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, uint> action = this.uintPropertyChanged;
			if (action == null)
			{
				return;
			}
			action(this, propertyName, value);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001032B File Offset: 0x0000E52B
		protected void OnPropertyChanged(Color value, [CallerMemberName] string propertyName = null)
		{
			Action<PropertyOwnerObject, string, Color> colorPropertyChanged = this.ColorPropertyChanged;
			if (colorPropertyChanged == null)
			{
				return;
			}
			colorPropertyChanged(this, propertyName, value);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000413 RID: 1043 RVA: 0x00010340 File Offset: 0x0000E540
		// (remove) Token: 0x06000414 RID: 1044 RVA: 0x00010378 File Offset: 0x0000E578
		public event Action<PropertyOwnerObject, string, object> PropertyChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000415 RID: 1045 RVA: 0x000103B0 File Offset: 0x0000E5B0
		// (remove) Token: 0x06000416 RID: 1046 RVA: 0x000103E8 File Offset: 0x0000E5E8
		public event Action<PropertyOwnerObject, string, bool> boolPropertyChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000417 RID: 1047 RVA: 0x00010420 File Offset: 0x0000E620
		// (remove) Token: 0x06000418 RID: 1048 RVA: 0x00010458 File Offset: 0x0000E658
		public event Action<PropertyOwnerObject, string, int> intPropertyChanged;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000419 RID: 1049 RVA: 0x00010490 File Offset: 0x0000E690
		// (remove) Token: 0x0600041A RID: 1050 RVA: 0x000104C8 File Offset: 0x0000E6C8
		public event Action<PropertyOwnerObject, string, float> floatPropertyChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600041B RID: 1051 RVA: 0x00010500 File Offset: 0x0000E700
		// (remove) Token: 0x0600041C RID: 1052 RVA: 0x00010538 File Offset: 0x0000E738
		public event Action<PropertyOwnerObject, string, Vec2> Vec2PropertyChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600041D RID: 1053 RVA: 0x00010570 File Offset: 0x0000E770
		// (remove) Token: 0x0600041E RID: 1054 RVA: 0x000105A8 File Offset: 0x0000E7A8
		public event Action<PropertyOwnerObject, string, Vector2> Vector2PropertyChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600041F RID: 1055 RVA: 0x000105E0 File Offset: 0x0000E7E0
		// (remove) Token: 0x06000420 RID: 1056 RVA: 0x00010618 File Offset: 0x0000E818
		public event Action<PropertyOwnerObject, string, double> doublePropertyChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000421 RID: 1057 RVA: 0x00010650 File Offset: 0x0000E850
		// (remove) Token: 0x06000422 RID: 1058 RVA: 0x00010688 File Offset: 0x0000E888
		public event Action<PropertyOwnerObject, string, uint> uintPropertyChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000423 RID: 1059 RVA: 0x000106C0 File Offset: 0x0000E8C0
		// (remove) Token: 0x06000424 RID: 1060 RVA: 0x000106F8 File Offset: 0x0000E8F8
		public event Action<PropertyOwnerObject, string, Color> ColorPropertyChanged;
	}
}
