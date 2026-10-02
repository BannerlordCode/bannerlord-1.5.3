using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.CodeGeneration
{
	// Token: 0x020000BB RID: 187
	public class ClassCode
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000178ED File Offset: 0x00015AED
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x000178F5 File Offset: 0x00015AF5
		public string Name { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x000178FE File Offset: 0x00015AFE
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00017906 File Offset: 0x00015B06
		public bool IsGeneric { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001790F File Offset: 0x00015B0F
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00017917 File Offset: 0x00015B17
		public int GenericTypeCount { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00017920 File Offset: 0x00015B20
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x00017928 File Offset: 0x00015B28
		public bool IsPartial { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00017931 File Offset: 0x00015B31
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x00017939 File Offset: 0x00015B39
		public ClassCodeAccessModifier AccessModifier { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00017942 File Offset: 0x00015B42
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0001794A File Offset: 0x00015B4A
		public bool IsClass { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00017953 File Offset: 0x00015B53
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0001795B File Offset: 0x00015B5B
		public List<string> InheritedInterfaces { get; private set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00017964 File Offset: 0x00015B64
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0001796C File Offset: 0x00015B6C
		public List<ClassCode> NestedClasses { get; private set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00017975 File Offset: 0x00015B75
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0001797D File Offset: 0x00015B7D
		public List<MethodCode> Methods { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00017986 File Offset: 0x00015B86
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0001798E File Offset: 0x00015B8E
		public List<ConstructorCode> Constructors { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x00017997 File Offset: 0x00015B97
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x0001799F File Offset: 0x00015B9F
		public List<VariableCode> Variables { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000179A8 File Offset: 0x00015BA8
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x000179B0 File Offset: 0x00015BB0
		public CommentSection CommentSection { get; set; }

		// Token: 0x0600070C RID: 1804 RVA: 0x000179BC File Offset: 0x00015BBC
		public ClassCode()
		{
			this.IsClass = true;
			this.IsGeneric = false;
			this.GenericTypeCount = 0;
			this.InheritedInterfaces = new List<string>();
			this.NestedClasses = new List<ClassCode>();
			this.Methods = new List<MethodCode>();
			this.Constructors = new List<ConstructorCode>();
			this.Variables = new List<VariableCode>();
			this.AccessModifier = ClassCodeAccessModifier.DoNotMention;
			this.Name = "UnnamedClass";
			this.CommentSection = null;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00017A34 File Offset: 0x00015C34
		public void GenerateInto(CodeGenerationFile codeGenerationFile)
		{
			if (this.CommentSection != null)
			{
				this.CommentSection.GenerateInto(codeGenerationFile);
			}
			string text = "";
			if (this.AccessModifier == ClassCodeAccessModifier.Public)
			{
				text += "public ";
			}
			else if (this.AccessModifier == ClassCodeAccessModifier.Internal)
			{
				text += "internal ";
			}
			if (this.IsPartial)
			{
				text += "partial ";
			}
			string text2 = "class";
			if (!this.IsClass)
			{
				text2 = "struct";
			}
			text = text + text2 + " " + this.Name;
			if (this.InheritedInterfaces.Count > 0)
			{
				text += " : ";
				for (int i = 0; i < this.InheritedInterfaces.Count; i++)
				{
					string text3 = this.InheritedInterfaces[i];
					text = text + " " + text3;
					if (i + 1 != this.InheritedInterfaces.Count)
					{
						text += ", ";
					}
				}
			}
			if (this.IsGeneric)
			{
				text += "<";
				for (int j = 0; j < this.GenericTypeCount; j++)
				{
					if (this.GenericTypeCount == 1)
					{
						text += "T";
					}
					else
					{
						text = text + "T" + j;
					}
					if (j + 1 != this.GenericTypeCount)
					{
						text += ", ";
					}
				}
				text += ">";
			}
			codeGenerationFile.AddLine(text);
			codeGenerationFile.AddLine("{");
			foreach (ClassCode classCode in this.NestedClasses)
			{
				classCode.GenerateInto(codeGenerationFile);
			}
			foreach (VariableCode variableCode in this.Variables)
			{
				string text4 = variableCode.GenerateLine();
				codeGenerationFile.AddLine(text4);
			}
			if (this.Variables.Count > 0)
			{
				codeGenerationFile.AddLine("");
			}
			foreach (ConstructorCode constructorCode in this.Constructors)
			{
				constructorCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
			foreach (MethodCode methodCode in this.Methods)
			{
				methodCode.GenerateInto(codeGenerationFile);
				codeGenerationFile.AddLine("");
			}
			codeGenerationFile.AddLine("}");
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00017CFC File Offset: 0x00015EFC
		public void AddVariable(VariableCode variableCode)
		{
			this.Variables.Add(variableCode);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00017D0A File Offset: 0x00015F0A
		public void AddNestedClass(ClassCode clasCode)
		{
			this.NestedClasses.Add(clasCode);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00017D18 File Offset: 0x00015F18
		public void AddMethod(MethodCode methodCode)
		{
			this.Methods.Add(methodCode);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00017D26 File Offset: 0x00015F26
		public void AddConsturctor(ConstructorCode constructorCode)
		{
			constructorCode.Name = this.Name;
			this.Constructors.Add(constructorCode);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00017D40 File Offset: 0x00015F40
		public void AddInterface(string interfaceName)
		{
			this.InheritedInterfaces.Add(interfaceName);
		}
	}
}
