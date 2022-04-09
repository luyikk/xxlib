using System;
using System.Collections.Generic;

namespace PKGCSharp
{
    /// <summary>
    /// Test Enum
    /// </summary>
    [Flags]
    public enum EnumTypeId : long
    {
        /// <summary>
        /// A
        /// </summary>
        A = 1,
        /// <summary>
        /// B
        /// </summary>
        B = 2,
        /// <summary>
        /// C
        /// </summary>
        C = 3,
    }

}

namespace PKGCSharp
{
    public partial struct IsTestStruct
    {
        /// <summary>
        /// test i32
        /// </summary>
        public int a { get; set; }

        public int Read(xx.ObjManager om, xx.DataReader data, out IsTestStruct self)
        {
            self=this;
            int err;
            if ((err = om.ReadFrom(data, out int __a)) == 0)
                self.a = __a;
            else return err;

            return 0;
        }

        public void Write(xx.ObjManager om, xx.Data data)
        {
            om.WriteTo(data, this.a);
        }    

    }
    /// <summary>
    /// Test Struct
    /// </summary>
    public partial struct TestStruct
    {
        /// <summary>
        /// test i32
        /// </summary>
        public int a { get; set; }

        public int b { get; set; }

        /// <summary>
        /// test f32
        /// </summary>
        public float c { get; set; }

        /// <summary>
        /// test f64
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// test string
        /// </summary>
        public string sb { get; set; }

        /// <summary>
        /// test buff
        /// </summary>
        public byte[] buff { get; set; }

        public PKGCSharp.Target testclass { get; set; }

        public PKGCSharp.IsTestStruct teststruct { get; set; }

        public PKGCSharp.IsTestStruct? test_null { get; set; }

        public PKGCSharp.IsTestStruct? test_null2 { get; set; }

        public int Read(xx.ObjManager om, xx.DataReader data, out TestStruct self)
        {
            self=this;
            int err;
            if ((err = om.ReadFrom(data, out int __a)) == 0)
                self.a = __a;
            else return err;

            if ((err = om.ReadFrom(data, out int __b)) == 0)
                self.b = __b;
            else return err;

            if ((err = om.ReadFrom(data, out float __c)) == 0)
                self.c = __c;
            else return err;

            if ((err = om.ReadFrom(data, out double __x)) == 0)
                self.x = __x;
            else return err;

            if ((err = om.ReadFrom(data, out string __sb)) == 0)
                self.sb = __sb;
            else return err;

            if ((err = om.ReadFrom(data, out byte[] __buff)) == 0)
                self.buff = __buff;
            else return err;

            if ((err = om.ReadObj(data, out PKGCSharp.Target __testclass)) == 0)
                self.testclass = __testclass;
            else return err;

            if ((err = this.teststruct.Read(om, data, out var __teststruct)) != 0)
                return err;
            else
                self.teststruct= __teststruct;

            if ((err = data.ReadFixed(out byte have_test_null)) == 0)
            {
                if (have_test_null == 1)
                {
                    var __test_null =  new PKGCSharp.IsTestStruct();
                    if ((err = __test_null.Read(om, data, out __test_null)) != 0)
                        return err;
                    else
                        self.test_null=__test_null;
                }
            }
            else return err;

            if ((err = data.ReadFixed(out byte have_test_null2)) == 0)
            {
                if (have_test_null2 == 1)
                {
                    var __test_null2 =  new PKGCSharp.IsTestStruct();
                    if ((err = __test_null2.Read(om, data, out __test_null2)) != 0)
                        return err;
                    else
                        self.test_null2=__test_null2;
                }
            }
            else return err;

            return 0;
        }

        public void Write(xx.ObjManager om, xx.Data data)
        {
            om.WriteTo(data, this.a);
            om.WriteTo(data, this.b);
            om.WriteTo(data, this.c);
            om.WriteTo(data, this.x);
            om.WriteTo(data, this.sb);
            om.WriteTo(data, this.buff);
            om.WriteObj(data, this.testclass);
            this.teststruct.Write(om, data);
            if (this.test_null is null)
               data.WriteFixed((byte)0);
            else
            {
                data.WriteFixed((byte)1);
                this.test_null?.Write(om, data);
            }
            if (this.test_null2 is null)
               data.WriteFixed((byte)0);
            else
            {
                data.WriteFixed((byte)1);
                this.test_null2?.Write(om, data);
            }
        }    

    }
}

namespace PKGCSharp
{
    /// <summary>
    /// Test Class
    /// </summary>
    public partial class Target : xx.ISerde
    {
        /// <summary>
        /// test i32
        /// </summary>
        public int a { get; set; }

        public ushort GetTypeid() => 1002;

        public int Read(xx.ObjManager om, xx.DataReader data)
        {
            int err;
            if ((err = om.ReadFrom(data, out int __a)) == 0)
            this.a = __a;
            else return err;

            return 0;
        }

        public void Write(xx.ObjManager om, xx.Data data)
        {
            om.WriteTo(data, this.a);
        }

        public override string ToString()            
           => xx.ObjManager.SerializeString(this);

    }
    /// <summary>
    /// Test Class
    /// </summary>
    public partial class Foo : xx.ISerde
    {
        /// <summary>
        /// test string
        /// </summary>
        public string sb { get; set; }

        /// <summary>
        /// test i32
        /// </summary>
        public int a { get; set; }

        /// <summary>
        /// test enum
        /// </summary>
        public PKGCSharp.EnumTypeId x { get; set; }

        /// <summary>
        /// struct
        /// </summary>
        public PKGCSharp.TestStruct testStruct { get; set; }

        /// <summary>
        /// test list struct
        /// </summary>
        public List<PKGCSharp.TestStruct> testlist { get; set; } = new List<PKGCSharp.TestStruct>();

        /// <summary>
        /// test list class
        /// </summary>
        public List<PKGCSharp.Target> testlist2 { get; set; }

        public List<int> testlist3 { get; set; }

        public PKGCSharp.TestStruct? test_null { get; set; }

        public PKGCSharp.TestStruct? test_null2 { get; set; }

        public ushort GetTypeid() => 1001;

        public int Read(xx.ObjManager om, xx.DataReader data)
        {
            int err;
            if ((err = data.ReadFixed(out uint siz)) != 0) return err;
            int endoffset = (int)(data.Offset - sizeof(uint) + siz);

            if (data.Offset >= endoffset)
                this.sb = default;
            else if ((err = om.ReadFrom(data, out string __sb)) == 0)
                this.sb = __sb;
            else return err;

            if (data.Offset >= endoffset)
                this.a = default;
            else if ((err = om.ReadFrom(data, out int __a)) == 0)
                this.a = __a;
            else return err;

            if (data.Offset >= endoffset)
                this.x = default;
            else if ((err = om.ReadFrom(data, out long __x)) == 0)
                this.x = (PKGCSharp.EnumTypeId) __x;
            else return err;

            var __teststruct=new PKGCSharp.TestStruct();
            if (data.Offset < endoffset && (err =  __teststruct.Read(om, data, out __teststruct)) != 0)
                return err;
            else
                this.testStruct=__teststruct;

            if (data.Offset < endoffset)
            {
                if ((err = data.ReadVarInteger(out uint testlist_len)) == 0)
                {
                    for (int i = 0; i < testlist_len; i++)
                    {
                        var p = new PKGCSharp.TestStruct();
                        if ((err = p.Read(om, data, out p)) == 0)
                            this.testlist.Add(p);
                        else return err;
                    }
                }
                else return err;
            }

            if (data.Offset >= endoffset)
                this.testlist2 = default;
            else if ((err = om.ReadObj(data, out List<PKGCSharp.Target> __testlist2)) == 0)
                this.testlist2 = __testlist2;
            else return err;

            if (data.Offset >= endoffset)
                this.testlist3 = default;
            else if ((err = om.ReadFrom(data, out List<int> __testlist3)) == 0)
                this.testlist3 = __testlist3;
            else return err;

            if (data.Offset < endoffset && (err = data.ReadFixed(out byte have_test_null)) == 0)
            {
                if (have_test_null == 1)
                {
                    var __test_null =  new PKGCSharp.TestStruct();
                    if ((err = __test_null.Read(om, data, out __test_null)) != 0)
                        return err;
                    else
                        this.test_null=__test_null;
                }
            }
            else
                return err;

            if (data.Offset < endoffset && (err = data.ReadFixed(out byte have_test_null2)) == 0)
            {
                if (have_test_null2 == 1)
                {
                    var __test_null2 =  new PKGCSharp.TestStruct();
                    if ((err = __test_null2.Read(om, data, out __test_null2)) != 0)
                        return err;
                    else
                        this.test_null2=__test_null2;
                }
            }
            else
                return err;

            if (data.Offset > endoffset)
                throw new IndexOutOfRangeException($"typeid:{ GetTypeid()} class: 'PKGCSharp.Foo' offset error");
            else
                data.Offset = endoffset;
            return 0;
        }

        public void Write(xx.ObjManager om, xx.Data data)
        {
            var bak = data.Length;
            data.WriteFixed(sizeof(uint));
            om.WriteTo(data, this.sb);
            om.WriteTo(data, this.a);
            om.WriteTo(data, (long) this.x);
            this.testStruct.Write(om, data);
            data.WriteVarInteger((uint)this.testlist.Count);
            foreach (var item in this.testlist)
                item.Write(om, data);

            om.WriteObj(data, this.testlist2);
            om.WriteTo(data, this.testlist3);
            if (this.test_null is null)
               data.WriteFixed((byte)0);
            else
            {
                data.WriteFixed((byte)1);
                this.test_null?.Write(om, data);
            }
            if (this.test_null2 is null)
               data.WriteFixed((byte)0);
            else
            {
                data.WriteFixed((byte)1);
                this.test_null2?.Write(om, data);
            }
            data.WriteFixedAt(bak, (uint)(data.Length - bak));
        }

        public override string ToString()            
           => xx.ObjManager.SerializeString(this);

    }
}


public static partial class CodeGen_CSharpTest
{
    public const string md5 = "#*MD5<835b2c13388bba0c9c50cb0e847d2c57>*#"; 
    public static void Register()
    {
         xx.ObjManager.Register<PKGCSharp.Target>(1002);
         xx.ObjManager.Register<PKGCSharp.Foo>(1001);
    }
}
