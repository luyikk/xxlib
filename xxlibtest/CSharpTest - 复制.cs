using System;
using System.Collections.Generic;

namespace PKGCSharp1
{

    /// <summary>
    /// "Test Enum"
    /// </summary>
    [Flags]
    public enum EnumTypeId : int
    {
        /// <summary>
        /// A
        /// </summary>
        a = 1,
        /// <summary>
        /// B
        /// </summary>
        b = 2,
        /// <summary>
        /// C
        /// </summary>
        c = 3
    }

}
    namespace PKGCSharp1
{
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

        public int Read(xx.ObjManager om, xx.DataReader data, out TestStruct self)
        {
            self = this;
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
        }

    }
}

namespace PKGCSharp1
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

        public PKGCSharp.TestStruct? test_null { get; set; }

        public ushort GetTypeid() => 1001;

        public int Read(xx.ObjManager om, xx.DataReader data)
        {
            int err;
            if ((err = om.ReadFrom(data, out string __sb)) == 0)
                this.sb = __sb;
            else return err;

            if ((err = om.ReadFrom(data, out int __a)) == 0)
                this.a = __a;
            else return err;

            if ((err = this.testStruct.Read(om, data, out var __teststruct)) != 0)
                return err;
            else
                this.testStruct = __teststruct;

            if ((err = data.ReadVarInteger(out uint testlist_len)) == 0)
            {
                for (int i = 0; i < testlist_len; i++)
                {
                    PKGCSharp.TestStruct p = new PKGCSharp.TestStruct();
                    if ((err = p.Read(om, data, out p)) == 0)
                        this.testlist.Add(p);
                    else return err;
                }
            }
            else return err;

            if ((err = om.ReadObj(data, out List<PKGCSharp.Target> __testlist2)) == 0)
                this.testlist2 = __testlist2;
            else return err;

            if ((err = data.ReadFixed(out byte have_test_null)) == 0)
            {
                if (have_test_null == 1)
                {
                    var p = new PKGCSharp.TestStruct();
                    if ((err = p.Read(om, data, out p)) != 0)
                        return err;
                    else
                        this.test_null = p;
                }
            }
            else return err;

            return 0;
        }

        public void Write(xx.ObjManager om, xx.Data data)
        {
            om.WriteTo(data, this.sb);
            om.WriteTo(data, this.a);
            this.testStruct.Write(om, data);
            data.WriteVarInteger((uint)this.testlist.Count);
            foreach (var item in this.testlist)
                item.Write(om, data);

            om.WriteObj(data, this.testlist2);
            if (this.test_null is null)
                data.WriteFixed((byte)0);
            else
            {
                data.WriteFixed((byte)1);
                this.test_null?.Write(om, data);
            }
        }

        public override string ToString()
           => xx.ObjManager.SerializeString(this);

    }
}

