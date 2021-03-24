using System;
using System.Collections.Generic;
using Xunit;
using xx;

namespace xxlibtest
{
    public class TestObjManager
    {
        [Fact]
        public void test_objmanager_wr()
        {
            {

                var data = new xx.Data();
                var objmanager = new ObjManager();

                objmanager.WriteTo(data, (byte)1);
                objmanager.WriteTo(data, (sbyte)2);
                objmanager.WriteTo(data, true);
                objmanager.WriteTo(data, false);
                objmanager.WriteTo(data, (ushort)11);
                objmanager.WriteTo(data, (short)12);
                objmanager.WriteTo(data, (int)3);
                objmanager.WriteTo(data, (uint)4);
                objmanager.WriteTo(data, (long)5);
                objmanager.WriteTo(data, (ulong)6);
                objmanager.WriteTo(data, "123123");
                objmanager.WriteTo(data, (string)null);
                objmanager.WriteTo(data, 0.1f);
                objmanager.WriteTo(data, 0.22);

                var (buff, len) = data.ToArray();
                var reader = new DataReader(buff, 0,len);

                Assert.True(objmanager.ReadFrom(reader, out byte a) == 0);
                Assert.Equal(1, a);

                Assert.True(objmanager.ReadFrom(reader, out sbyte b) == 0);
                Assert.Equal(2, b);

                Assert.True(objmanager.ReadFrom(reader, out bool c) == 0);
                Assert.True(c);

                Assert.True(objmanager.ReadFrom(reader, out bool d) == 0);
                Assert.False(d);

                Assert.True(objmanager.ReadFrom(reader, out ushort e) == 0);
                Assert.Equal(11, e);

                Assert.True(objmanager.ReadFrom(reader, out short f) == 0);
                Assert.Equal(12, f);

                Assert.True(objmanager.ReadFrom(reader, out int g) == 0);
                Assert.Equal(3, g);

                Assert.True(objmanager.ReadFrom(reader, out uint h) == 0);
                Assert.Equal((uint)4, h);

                Assert.True(objmanager.ReadFrom(reader, out long i) == 0);
                Assert.Equal(5, i);

                Assert.True(objmanager.ReadFrom(reader, out ulong j) == 0);
                Assert.Equal((ulong)6, j);

                Assert.True(objmanager.ReadFrom(reader, out string k) == 0);
                Assert.Equal("123123", k);

                Assert.True(objmanager.ReadFrom(reader, out string l) == 0);
                Assert.Equal("", l);

                Assert.True(objmanager.ReadFrom(reader, out float m) == 0);
                Assert.Equal(0.1f, m);

                Assert.True(objmanager.ReadFrom(reader, out double n) == 0);
                Assert.Equal(0.22, n);
            }
            {
                var data = new xx.Data();
                var objmanager = new ObjManager();

                objmanager.WriteTo(data, (byte?)1);
                objmanager.WriteTo(data, (sbyte?)2);
                objmanager.WriteTo(data, true);
                objmanager.WriteTo(data, false);
                objmanager.WriteTo(data, (ushort?)11);
                objmanager.WriteTo(data, (short?)12);
                objmanager.WriteTo(data, (int?)3);
                objmanager.WriteTo(data, (uint?)4);
                objmanager.WriteTo(data, (long?)5);
                objmanager.WriteTo(data, (ulong?)null);


                var (buff, len) = data.ToArray();
                var reader = new DataReader(buff, 0,len);

                Assert.True(objmanager.ReadFrom(reader, out byte? a) == 0);
                Assert.Equal(1, a.Value);

                Assert.True(objmanager.ReadFrom(reader, out sbyte? b) == 0);
                Assert.Equal(2, b.Value);

                Assert.True(objmanager.ReadFrom(reader, out bool c) == 0);
                Assert.True(c);

                Assert.True(objmanager.ReadFrom(reader, out bool d) == 0);
                Assert.False(d);

                Assert.True(objmanager.ReadFrom(reader, out ushort? e) == 0);
                Assert.Equal(11, e.Value);

                Assert.True(objmanager.ReadFrom(reader, out short? f) == 0);
                Assert.Equal(12, f.Value);

                Assert.True(objmanager.ReadFrom(reader, out int? g) == 0);
                Assert.Equal(3, g);

                Assert.True(objmanager.ReadFrom(reader, out uint? h) == 0);
                Assert.Equal((uint)4, h);

                Assert.True(objmanager.ReadFrom(reader, out long? i) == 0);
                Assert.Equal(5, i);

                Assert.True(objmanager.ReadFrom(reader, out ulong? j) == 0);
                Assert.Null(j);

            }
        }

        [Fact]
        public void test_objmanager_list()
        {
            var data = new xx.Data();
            var objmanager = new ObjManager();

            objmanager.WriteTo(data, new byte[] { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<byte> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<short> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<int> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<long> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<string> { "1", "2", null, "4", "5" });
            objmanager.WriteTo(data, new List<long?> { 1, null, 3, null, 5 });

            var (buff, len) = data.ToArray();
            var reader = new DataReader(buff,0, len);

            Assert.True(objmanager.ReadFrom(reader, out byte[] a) == 0);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, a);

            Assert.True(objmanager.ReadFrom(reader, out List<byte> b) == 0);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, b);

            Assert.True(objmanager.ReadFrom(reader, out List<short> c) == 0);
            Assert.Equal(new short[] { 1, 2, 3, 4, 5 }, c);

            Assert.True(objmanager.ReadFrom(reader, out List<int> d) == 0);
            Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, d);

            Assert.True(objmanager.ReadFrom(reader, out List<long> e) == 0);
            Assert.Equal(new long[] { 1, 2, 3, 4, 5 }, e);

            Assert.True(objmanager.ReadFrom(reader, out List<string> f) == 0);
            Assert.Equal(new string[] { "1", "2", "", "4", "5" }, f);

            Assert.True(objmanager.ReadFrom(reader, out List<long?> g) == 0);
            Assert.Equal(new long?[] { 1, null, 3, null, 5 }, g);
        }



        public class StructOne
        {
            public int S1 { get; set; }
            public string S2 { get; set; }

            public int Read(ObjManager om, DataReader data)
            {
                int err;
                if ((err = data.ReadFixed(out uint siz)) != 0) return err;
                int endoffset = (int)(data.Offset - sizeof(uint) + siz);

                if (data.Offset > endoffset)
                    S1 = default;
                else if ((err = om.ReadFrom(data, out int __s1)) == 0)
                    S1 = __s1;
                else return err;


                if (data.Offset > endoffset)
                    S2 = default;
                else if ((err = om.ReadFrom(data, out string __s2)) == 0)
                    S2 = __s2;
                else return err;


                if (data.Offset > endoffset)
                    throw new IndexOutOfRangeException($"Struct: StructOne offset error");
                else
                    data.Offset = endoffset;

                return 0;
            }


            public void Write(ObjManager om, Data data)
            {
                var bak = data.Length;
                data.WriteFixed(sizeof(uint));
                om.WriteTo(data, this.S1);
                om.WriteTo(data, this.S2);
                data.WriteFixedAt(bak, (uint)(data.Length - bak));
            }

        }


        public class StructTow : StructOne
        {
            public int P1 { get; set; }
            public float P2 { get; set; }
            public string P3 { get; set; }
            public Ponit Position { get; set; }
            public Ponit Position2 { get; set; }
            public Foo My { get; set; }
            public List<Ponit> Positions { get; set; }

            public new int Read(ObjManager om, DataReader data)
            {
                base.Read(om, data);
                int err;
                if ((err = om.ReadFrom(data, out int __p1)) == 0)
                    P1 = __p1;
                else return err;

                if ((err = om.ReadFrom(data, out float __p2)) == 0)
                    P2 = __p2;
                else return err;

                if ((err = om.ReadFrom(data, out string __p3)) == 0)
                    P3 = __p3;
                else return err;

                if ((err = om.ReadObj(data, out Ponit __position)) == 0)
                    Position = __position;
                else return err;

                if ((err = om.ReadObj(data, out Ponit __position2)) == 0)
                    Position2 = __position2;
                else return err;

                if ((err = om.ReadObj(data, out Foo __foo)) == 0)
                    My = __foo;
                else return err;

                if ((err = om.ReadObj(data, out List<Ponit> __positions)) == 0)
                    Positions = __positions;
                else return err;

                return 0;
            }

            public new void Write(ObjManager om, Data data)
            {
                base.Write(om, data);

                om.WriteTo(data, this.P1);
                om.WriteTo(data, this.P2);
                om.WriteTo(data, this.P3);
                om.WriteObj(data, this.Position);
                om.WriteObj(data, this.Position2);
                om.WriteObj(data, this.My);
                om.WriteObj(data, this.Positions);
            }

        }


        public class Base : ISerde
        {
            public int S1 { get; set; }
            public string S2 { get; set; }

            public List<StructOne> S3 { get; set; } = new List<StructOne>();

            public ushort GetTypeid()
            {
                return 1;
            }

            public int Read(ObjManager om, DataReader data)
            {
                int err;
                if ((err = data.ReadFixed(out uint siz)) != 0) return err;
                int endoffset = (int)(data.Offset - sizeof(uint) + siz);

                if (data.Offset >= endoffset)
                    S1 = default;
                else if ((err = om.ReadFrom(data, out int __s1)) == 0)
                    S1 = __s1;
                else return err;


                if (data.Offset >= endoffset)
                    S2 = default;
                else if ((err = om.ReadFrom(data, out string __s2)) == 0)
                    S2 = __s2;
                else return err;

                if (data.Offset < endoffset)
                {
                    if ((err = data.ReadVarInteger(out uint s3_len)) == 0)
                    {
                        for (int i = 0; i < s3_len; i++)
                        {
                            var p = new StructOne();
                            if ((err = p.Read(om, data)) == 0)
                                this.S3.Add(p);
                            else return err;
                        }
                    }
                    else return err;
                }               



                if (data.Offset > endoffset)
                    throw new IndexOutOfRangeException($"typeid:{GetTypeid()} class:Foo offset error");
                else
                    data.Offset = endoffset;

                return 0;
            }

            public void Write(ObjManager om, Data data)
            {
                var bak = data.Length;
                data.WriteFixed(sizeof(uint));
                om.WriteTo(data, this.S1);
                om.WriteTo(data, this.S2);

                data.WriteVarInteger((uint)this.S3.Count);
                foreach (var item in S3)
                    item.Write(om, data);

                data.WriteFixedAt(bak, (uint)(data.Length - bak));
            }
        }

        public class Ponit : ISerde
        {
            public int X { get; set; }
            public int Y { get; set; }

            public ushort GetTypeid()
            {
                return 1002;
            }

            public int Read(ObjManager om, DataReader data)
            {
                int err;
                if ((err = data.ReadFixed(out uint siz)) != 0) return err;
                int endoffset = (int)(data.Offset - sizeof(uint) + siz);

                if (data.Offset >= endoffset)
                    X = default;
                else if ((err = om.ReadFrom(data, out int __x)) == 0)
                    X = __x;
                else return err;

                if (data.Offset >= endoffset)
                    Y = default;
                else if ((err = om.ReadFrom(data, out int __y)) == 0)
                    Y = __y;
                else return err;

                if (data.Offset > endoffset)
                    throw new IndexOutOfRangeException($"typeid:{GetTypeid()} class:Foo offset error");
                else
                    data.Offset = endoffset;

                return 0;
            }

            public void Write(ObjManager om, Data data)
            {
                var bak = data.Length;
                data.WriteFixed(sizeof(uint));
                om.WriteTo(data, this.X);
                om.WriteTo(data, this.Y);
                data.WriteFixedAt(bak, (uint)(data.Length - bak));
            }
        }

        public class Foo : Base, ISerde
        {
            public int P1 { get; set; }
            public float P2 { get; set; }
            public string P3 { get; set; }
            public Ponit Position { get; set; }
            public Ponit Position2 { get; set; }
            public Foo My { get; set; }
            public List<Ponit> Positions { get; set; }

            public StructOne ST1 { get; set; } = new StructOne();

            public StructTow ST2 { get; set; } = new StructTow();

            public StructOne ST3 { get; set; }

            public new ushort GetTypeid()
            {
                return 1000;
            }

            public new int Read(ObjManager om, DataReader data)
            {
                base.Read(om, data);
                int err;
                if ((err = data.ReadFixed(out uint siz)) != 0) return err;
                int endoffset = (int)(data.Offset - sizeof(uint) + siz);

                if (data.Offset >= endoffset)
                    P1 = default;
                else if ((err = om.ReadFrom(data, out int __p1)) == 0)
                    P1 = __p1;
                else return err;

                if (data.Offset >= endoffset)
                    P2 = default;
                else if ((err = om.ReadFrom(data, out float __p2)) == 0)
                    P2 = __p2;
                else return err;

                if (data.Offset >= endoffset)
                    P3 = default;
                else if ((err = om.ReadFrom(data, out string __p3)) == 0)
                    P3 = __p3;
                else return err;

                if (data.Offset >= endoffset)
                    Position = default;
                else if ((err = om.ReadObj(data, out Ponit __position)) == 0)
                    Position = __position;
                else return err;

                if (data.Offset >= endoffset)
                    Position2 = default;
                else if ((err = om.ReadObj(data, out Ponit __position2)) == 0)
                    Position2 = __position2;
                else return err;

                if (data.Offset >= endoffset)
                    My = default;
                else if ((err = om.ReadObj(data, out Foo __foo)) == 0)
                    My = __foo;
                else return err;

                if (data.Offset >= endoffset)
                    Positions = default;
                else if ((err = om.ReadObj(data, out List<Ponit> __positions)) == 0)
                    Positions = __positions;
                else return err;


                if (data.Offset < endoffset && (err = ST1.Read(om, data)) != 0)
                     return err;

                if (data.Offset < endoffset && (err = ST2.Read(om, data)) != 0)
                    return err;

                if (data.Offset < endoffset && (err = data.ReadFixed(out byte have_s3)) == 0)
                {
                    if (have_s3 == 1)
                    {
                        this.ST3 = new StructOne();
                        if ((err = this.ST3.Read(om, data)) != 0)
                            return err;
                    }
                }
                else
                    return err;





                if (data.Offset > endoffset)
                    throw new IndexOutOfRangeException($"typeid:{GetTypeid()} class:Foo offset error");
                else
                    data.Offset = endoffset;

                return 0;


                //base.Read(om, data);
                //int err;
                //if ((err = om.ReadFrom(data, out int __p1)) == 0)
                //    P1 = __p1;
                //else return err;

                //if ((err = om.ReadFrom(data, out float __p2)) == 0)
                //    P2 = __p2;
                //else return err;

                //if ((err = om.ReadFrom(data, out string __p3)) == 0)
                //    P3 = __p3;
                //else return err;

                //if ((err = om.ReadObj(data, out Ponit __position)) == 0)
                //    Position = __position;
                //else return err;

                //if ((err = om.ReadObj(data, out Ponit __position2)) == 0)
                //    Position2 = __position2;
                //else return err;

                //if ((err = om.ReadObj(data, out Foo __foo)) == 0)
                //    My = __foo;
                //else return err;

                //if ((err = om.ReadObj(data, out List<Ponit> __positions)) == 0)
                //    Positions = __positions;
                //else return err;

                //if ((err = this.ST1.Read(om, data)) != 0)
                //    return err;

                //if ((err = this.ST2.Read(om, data)) != 0)
                //    return err;


                //if ((err = data.ReadFixed(out byte have_st3)) == 0)
                //{
                //    if (have_st3 == 1)
                //    {
                //        this.ST3 = new StructOne();
                //        if ((err = this.ST3.Read(om, data)) != 0)
                //            return err;
                //    }
                //}
                //else return err;

                //return 0;
            }

            public new void Write(ObjManager om, Data data)
            {
                base.Write(om, data);

                var bak = data.Length;
                data.WriteFixed(sizeof(uint));
                om.WriteTo(data, this.P1);
                om.WriteTo(data, this.P2);
                om.WriteTo(data, this.P3);
                om.WriteObj(data, this.Position);
                om.WriteObj(data, this.Position2);
                om.WriteObj(data, this.My);
                om.WriteObj(data, this.Positions);
                this.ST1.Write(om, data);
                this.ST2.Write(om, data);
                if (this.ST3 is null)
                    data.WriteFixed((byte)0);
                else
                {
                    data.WriteFixed((byte)1);
                    this.ST3.Write(om, data);
                }
                data.WriteFixedAt(bak, (uint)(data.Length - bak));


                //om.WriteTo(data, this.P1);
                //om.WriteTo(data, this.P2);
                //om.WriteTo(data, this.P3);
                //om.WriteObj(data, this.Position);
                //om.WriteObj(data, this.Position2);
                //om.WriteObj(data, this.My);
                //om.WriteObj(data, this.Positions);

                //this.ST1.Write(om, data);
                //this.ST2.Write(om, data);
                //if (this.ST3 is null)
                //    data.WriteFixed((byte)0);
                //else
                //{
                //    data.WriteFixed((byte)1);
                //    this.ST3.Write(om, data);
                //}
            }

            public override string ToString()            
                => ObjManager.SerializeString(this);
            
        }


        [Fact]
        public void test_objmanager_obj()
        {
            ObjManager.Register<Base>(1);
            ObjManager.Register<Foo>(1000);
            ObjManager.Register<Ponit>(1002);
            {

                var data = new xx.Data();
                var objmanager = new ObjManager();

                var foo = new Foo
                {
                    S1 = 555,
                    S2 = "6666",
                    P1 = 1,
                    P2 = 0.55f,
                    P3 = "123123",
                    Position = new Ponit
                    {
                        X = 100,
                        Y = 200
                    },

                  
                };

                foo.Position2 = foo.Position;
                foo.My = foo;
                foo.Positions = new List<Ponit>
                {
                    foo.Position,foo.Position
                };

                objmanager.WriteTo(data, foo);
                var (buff, len) = data.ToArray();

                var read = new xx.DataReader(buff,0, len);
                Assert.True(objmanager.ReadFrom(read, out Foo a) == 0);
                Assert.True(foo.S1 == a.S1);
                Assert.True(foo.S2 == a.S2);
                Assert.True(foo.P1 == a.P1);
                Assert.True(foo.P2 == a.P2);
                Assert.True(foo.P3 == a.P3);
                Assert.True(foo.Position.X == a.Position.X);
                Assert.True(foo.Position.Y == a.Position.Y);
                Assert.True(foo.Position2.X == a.Position2.X);
                Assert.True(foo.Position2.Y == a.Position2.Y);
                Assert.True(foo.Positions.Count == a.Positions.Count);
                Assert.Equal(a, a.My);


                read.Offset = 0;

                Assert.True(objmanager.ReadFrom(read, out ISerde v) == 0);
                if (v is Foo b)
                {
                    Assert.True(foo.S1 == b.S1);
                    Assert.True(foo.S2 == b.S2);
                    Assert.True(foo.P1 == b.P1);
                    Assert.True(foo.P2 == b.P2);
                    Assert.True(foo.P3 == b.P3);
                    Assert.True(foo.Position.X == b.Position.X);
                    Assert.True(foo.Position.Y == b.Position.Y);
                    Assert.True(foo.Position2.X == b.Position2.X);
                    Assert.True(foo.Position2.Y == b.Position2.Y);
                    Assert.Equal(b, b.My);
                }
                else
                    throw new Exception("error type");
            }

            {


                var data = new xx.Data();
                var objmanager = new ObjManager();

                var foo = new Foo
                {
                    S1 = 555,
                    S2 = "6666",
                    P1 = 1,
                    P2 = 0.55f,
                    P3 = "123123",
                    Position = new Ponit
                    {
                        X = 100,
                        Y = 200
                    }                   
                };

                foo.Position2 = foo.Position;
                foo.My = foo;
                foo.ST1.S1 = 122;
                foo.ST1.S2 = "211";
                foo.S3.Add(new StructOne
                {
                    S1 = 100,
                    S2 = "3333"
                });
                foo.ST2.S1 = 333;
                foo.ST2.S2 = "444";
                foo.ST2.My = foo;
                foo.ST2.P1 = 1;
                foo.ST2.P2 = 0.54f;
                foo.ST2.P3 = "321321";
                foo.Position = new Ponit
                {
                    X = 300,
                    Y = 400
                };

                foo.ST2.Position2 = foo.Position;

                foo.ST3 = new StructOne
                {
                    S1 = 111,
                    S2 = "ST3"
                };
                

                objmanager.WriteTo(data, new List<Foo> { foo, foo, foo });

                var (buff, len) = data.ToArray();

                var read = new xx.DataReader(buff, 0,len);

                Assert.True(objmanager.ReadFrom(read, out List<Foo> a) == 0);
                Assert.True(a.Count == 3);

                var x = a[0];

                Assert.True(x.ST1.S1 == foo.ST1.S1);
                Assert.True(x.ST1.S2 == foo.ST1.S2);
                Assert.True(x.ST2.S1 == foo.ST2.S1);
                Assert.True(x.ST2.S2 == foo.ST2.S2);
                Assert.True(x.ST3.S1 == foo.ST3.S1);
                Assert.True(x.ST3.S2 == foo.ST3.S2);

            }
        }
       

    }
}
