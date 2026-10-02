using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	internal class ReflectionObject
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00017481 File Offset: 0x00015681
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> Creator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00017489 File Offset: 0x00015689
		public IDictionary<string, ReflectionMember> Members { get; }

		// Token: 0x06000582 RID: 1410 RVA: 0x00017491 File Offset: 0x00015691
		private ReflectionObject([Nullable(new byte[] { 2, 1 })] ObjectConstructor<object> creator)
		{
			this.Members = new Dictionary<string, ReflectionMember>();
			this.Creator = creator;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000174AB File Offset: 0x000156AB
		[return: Nullable(2)]
		public object GetValue(object target, string member)
		{
			return this.Members[member].Getter(target);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000174C4 File Offset: 0x000156C4
		public void SetValue(object target, string member, [Nullable(2)] object value)
		{
			this.Members[member].Setter(target, value);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000174DE File Offset: 0x000156DE
		public Type GetType(string member)
		{
			return this.Members[member].MemberType;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000174F1 File Offset: 0x000156F1
		public static ReflectionObject Create(Type t, params string[] memberNames)
		{
			return ReflectionObject.Create(t, null, memberNames);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000174FC File Offset: 0x000156FC
		public static ReflectionObject Create(Type t, [Nullable(2)] MethodBase creator, params string[] memberNames)
		{
			ReflectionDelegateFactory reflectionDelegateFactory = JsonTypeReflector.ReflectionDelegateFactory;
			ObjectConstructor<object> objectConstructor = null;
			if (creator != null)
			{
				objectConstructor = reflectionDelegateFactory.CreateParameterizedConstructor(creator);
			}
			else if (ReflectionUtils.HasDefaultConstructor(t, false))
			{
				Func<object> ctor = reflectionDelegateFactory.CreateDefaultConstructor<object>(t);
				objectConstructor = ([Nullable(new byte[] { 1, 2 })] object[] args) => ctor();
			}
			ReflectionObject reflectionObject = new ReflectionObject(objectConstructor);
			int i = 0;
			while (i < memberNames.Length)
			{
				string text = memberNames[i];
				MemberInfo[] member = t.GetMember(text, BindingFlags.Instance | BindingFlags.Public);
				if (member.Length != 1)
				{
					throw new ArgumentException("Expected a single member with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				MemberInfo memberInfo = member.Single<MemberInfo>();
				ReflectionMember reflectionMember = new ReflectionMember();
				MemberTypes memberTypes = memberInfo.MemberType();
				if (memberTypes == MemberTypes.Field)
				{
					goto IL_00AA;
				}
				if (memberTypes != MemberTypes.Method)
				{
					if (memberTypes == MemberTypes.Property)
					{
						goto IL_00AA;
					}
					throw new ArgumentException("Unexpected member type '{0}' for member '{1}'.".FormatWith(CultureInfo.InvariantCulture, memberInfo.MemberType(), memberInfo.Name));
				}
				else
				{
					MethodInfo methodInfo = (MethodInfo)memberInfo;
					if (methodInfo.IsPublic)
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == 0 && methodInfo.ReturnType != typeof(void))
						{
							MethodCall<object, object> call2 = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
							reflectionMember.Getter = (object target) => call2(target, Array.Empty<object>());
						}
						else if (parameters.Length == 1 && methodInfo.ReturnType == typeof(void))
						{
							MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
							reflectionMember.Setter = delegate(object target, [Nullable(2)] object arg)
							{
								call(target, new object[] { arg });
							};
						}
					}
				}
				IL_01BF:
				reflectionMember.MemberType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				reflectionObject.Members[text] = reflectionMember;
				i++;
				continue;
				IL_00AA:
				if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
				{
					reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
				}
				if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
				{
					reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
					goto IL_01BF;
				}
				goto IL_01BF;
			}
			return reflectionObject;
		}
	}
}
