using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class GenCSharp_Class_Lite
{
    readonly Assembly asm;
    readonly string templateName;
    readonly string outDir;
    readonly Dictionary<ushort, Type> typeIdMappings = new Dictionary<ushort, Type>();  
    readonly List<Type> ts;
    List<Type> cs;

    public GenCSharp_Class_Lite(Assembly asm, string outDir, string templateName)
    {
        this.asm = asm;
        this.templateName = templateName;
        this.outDir = outDir;
        ts = asm._GetTypes();
    }

    public void Gen_Head(StringBuilder sb)
    {
        sb.Append(
$@"using System;
using System.Collections.Generic;
using xx;

namespace {templateName} 
{{
    public static class PkgGenMd5
    {{
        public const string value = ""{ StringHelpers.MD5PlaceHolder }""; 
    }}    
");       
     
    }


    public void Gen_Content(StringBuilder sb)
    {
        for (int i = 0; i < cs.Count; i++)
        {
            var c = cs[i];
            var o = asm.CreateInstance(c.FullName);
            var typeid = c._GetTypeId();

            var need_compatible = c._Has<TemplateLibrary.Compatible>();

            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append($@"
    namespace { c.Namespace  }
    {{
");
            }


            if (c._IsUserStruct() || c.IsValueType)
                throw new NotSupportedException($"{c.FullName} Not Supported struct!");

            sb.Append(c._GetDesc()._GetComment_CSharp(9) +
$@"
         public class {c.Name} :{c.BaseType._GetTypeBase_Csharp()}
         {{ 
");

            var fs = c._GetFieldsConsts();
            foreach (var p in fs)
            {
                var pt = p.FieldType;
                var ptn = pt._GetTypeDecl_Csharp();
                if (pt._IsUserClass())                
                    throw new Exception($"need Shared<T> class:{c.Name} field:{p.Name}");


                if (pt._IsList() && pt.GetGenericArguments()[0]._IsUserClass())
                    throw new Exception($"List<T> T not Shared<T> class:{c.Name} field:{p.Name}");

                if (!p.IsStatic)
                {
                    sb.Append(p._GetDesc()._GetComment_CSharp(13) +
$@"
             public {ptn} {p.Name} {{get;set;}}
");
                }
                else                
                    throw new NotSupportedException("static field not suppored");
            }

            sb.Append($@"
             public {(!c.IsValueType && c._HasBaseType() ? "new " : "")}ushort GetTypeid()=>{typeid};
");

            #region Read
            sb.Append($@"
             public {(!c.IsValueType && c._HasBaseType() ? "new " : "")}int Read(ObjManager om, DataReader data)
             {{");

            if (!c.IsValueType && c._HasBaseType())
            {
                sb.Append($@"
                 base.Read(om, data);
");
            }

            sb.Append(@"
                 int err;");

            if (need_compatible)
            {
                sb.Append($@"
                 if ((err = data.ReadFiexd(out uint siz)) != 0) return err;
                 int endoffset = (int)(data.Offset - sizeof(uint) + siz);
");
            }

            foreach (var f in fs)
            {
               
                if (!need_compatible)
                {
                    sb.Append($@"
                 if ((err = om.{(f.FieldType._IsShared() ? "ReadObj" : (f.FieldType._IsListShared() ? "ReadObj" : "ReadFrom"))}(data, out {f.FieldType._GetTypeDecl_Csharp()} __{f.Name.ToLower()})) == 0)
                    this.{f.Name} = __{f.Name.ToLower()};
                 else return err;
");
                }
                else
                {
                    sb.Append($@"
                 if (data.Offset >= endoffset)
                     this.{f.Name} = default;
                 else if ((err = om.{(f.FieldType._IsShared()? "ReadObj" : (f.FieldType._IsListShared()? "ReadObj": "ReadFrom"))}(data, out {f.FieldType._GetTypeDecl_Csharp()} __{f.Name.ToLower()})) == 0)
                     this.{f.Name} = __{f.Name.ToLower()};
                 else return err;
");
                }
            }
            if (need_compatible)
            {
                sb.Append($@"
                 if (data.Offset > endoffset)
                     throw new IndexOutOfRangeException($""typeid:{{ GetTypeid()}} class: Foo offset error"");
                 else
                     data.Offset = endoffset;
                 return 0;
            }}");
            }
            else
            {
                sb.Append($@"
                 return 0;
            }}");
            }

            #endregion

          
            #region Write
            sb.Append($@"

            public {(!c.IsValueType && c._HasBaseType()?"new ":"")}void Write(ObjManager om, Data data)
            {{");

            if (!c.IsValueType && c._HasBaseType())
            {
                sb.Append($@"
                 base.Write(om, data);
");
            }

            if (need_compatible)
            {
                sb.Append($@"
                 var bak = data.Length;
                 data.WriteFiexd(sizeof(uint));
");
            }


            foreach (var f in fs)
            {
                sb.Append($@"
                 om.{(f.FieldType._IsShared() ? "WriteObj" : (f.FieldType._IsListShared() ? "WriteObj" : "WriteTo"))}(data, this.{f.Name});");
            }

            if (need_compatible)
            {
                sb.Append(@"

                 data.WriteFiexdAt(bak, (uint)(data.Length - bak));
            }
        }
");
            }
            else
            {
                sb.Append(@"
            }
        }
");
            }

            #endregion


            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1))
            {
                sb.Append(@"    }");
            }
        }

        var tb = new StringBuilder();

        foreach (var item in typeIdMappings)
        {
            tb.Append($"             ObjManager.Register<{item.Value.FullName}>({item.Key});\r\n");
        }

        sb.Append($@"

    public static class AllTypes
    {{
         public static void Register()
         {{
{tb}
         }}
    }}
");


        sb.Append(@"
}
");

        sb._WriteToFile(Path.Combine(outDir, templateName + "_class.cs"));
        sb.Clear();

    }

    public void build()
    {
        // 填充 typeId for class
        foreach (var c in ts._GetClasss().Where(o => !o._Has<TemplateLibrary.Struct>()))
        {         
            var id = c._GetTypeId();
            if (id == null) {
                throw new Exception("type: " + c.FullName + " miss [TypeId(xxxxxx)]");
            }// throw new Exception("type: " + c.FullName + " miss [TypeId(xxxxxx)]");
            else
            {
                if (typeIdMappings.ContainsKey(id.Value))
                {
                    throw new Exception("type: " + c.FullName + "'s typeId is duplicated with " + typeIdMappings[id.Value].FullName);
                }
                else
                {
                    typeIdMappings.Add(id.Value, c);
                }
            }
        }

        cs = ts._GetClasssStructs();
        cs._SortByInheritRelation();

        var sb = new StringBuilder();
        Gen_Head(sb);
        Gen_Content(sb);

    }



    public static void Gen(Assembly asm, string outDir, string templateName)
    {
        new GenCSharp_Class_Lite(asm, outDir, templateName).build();
    }
}

