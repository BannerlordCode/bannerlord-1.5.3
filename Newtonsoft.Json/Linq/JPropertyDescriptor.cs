using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(0)]
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x0002A1FB File Offset: 0x000283FB
		public JPropertyDescriptor(string name)
			: base(name, null)
		{
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0002A205 File Offset: 0x00028405
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002A20D File Offset: 0x0002840D
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0002A210 File Offset: 0x00028410
		[NullableContext(2)]
		public override object GetValue(object component)
		{
			JObject jobject = component as JObject;
			if (jobject == null)
			{
				return null;
			}
			return jobject[this.Name];
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0002A229 File Offset: 0x00028429
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002A22C File Offset: 0x0002842C
		[NullableContext(2)]
		public override void SetValue(object component, object value)
		{
			JObject jobject = component as JObject;
			if (jobject != null)
			{
				JToken jtoken = (value as JToken) ?? new JValue(value);
				jobject[this.Name] = jtoken;
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0002A261 File Offset: 0x00028461
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0002A264 File Offset: 0x00028464
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002A270 File Offset: 0x00028470
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0002A273 File Offset: 0x00028473
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0002A27F File Offset: 0x0002847F
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
