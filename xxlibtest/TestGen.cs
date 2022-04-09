using PKG;
using PKG.P;
using System;
using System.Collections.Generic;
using Xunit;
using xx;


namespace xxlibtest
{
    public class TestGen
    {
        [Fact]
        public void test_gen()
        {
            xx.ObjManager.SerializeStringFunc = (p) => Swifter.Json.JsonFormatter.SerializeObject(p, Swifter.Json.JsonFormatterOptions.Indented);



            CodeGen_Test.Register();
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
                    Buff = new byte[] { 1, 2, 3 },
                    Data = new List<uint> { 1, 2, 3, 4 },
                    Position = new Point
                    {
                        X = 100,
                        Y = 200,
                        Z = 555.0f
                    },


                };

                foo.Position2 = foo.Position;
                foo.My = foo;
                foo.Positions = new List<Point>
                {
                    foo.Position,foo.Position
                };

                foo.sp1.x = 1;
                foo.sp1.y = 2;
                foo.sp1.z = 3;
                foo.px = 100;

                foo.sp3 = new Point3();
                foo.sp3.x = 1;
                foo.sp3.y = 2;
                foo.sp3.z = 3;

                foo.Point2List.Add(new Point3 { x = 1, y = 2, z = 3 });

                objmanager.WriteTo(data, foo);
                var (buff, len) = data.ToArray();


                var buff_p = new byte[10 + len];
                Buffer.BlockCopy(buff, 0, buff_p, 10, len);

                var read = new xx.DataReader(buff_p, 10, len);
                Assert.True(objmanager.ReadFrom(read, out Foo a) == 0);
                Assert.True(foo.S1 == a.S1);
                Assert.True(foo.S2 == a.S2);
                Assert.True(foo.P1 == a.P1);
                Assert.True(foo.P2 == a.P2);
                Assert.True(foo.P3 == a.P3);
                Assert.True(foo.Position.X == a.Position.X);
                Assert.True(foo.Position.Y == a.Position.Y);
                Assert.True(foo.Position.Z == a.Position.Z);
                Assert.True(foo.Position2.X == a.Position2.X);
                Assert.True(foo.Position2.Y == a.Position2.Y);
                Assert.True(foo.Position2.Z == a.Position2.Z);
                Assert.True(foo.Positions.Count == a.Positions.Count);
                Assert.Equal(foo.Buff, a.Buff);
                Assert.Equal(foo.Data, a.Data);
                Assert.Equal(a, a.My);

                Assert.Equal(1, a.sp1.x);
                Assert.Equal(2, a.sp1.y);
                Assert.Equal(3, a.sp1.z);

                Assert.Null(a.sp2);

                Assert.Equal(1, a.sp3.x);
                Assert.Equal(2, a.sp3.y);
                Assert.Equal(3, a.sp3.z);


                Assert.Equal(100, a.px);

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
                    Assert.True(foo.Point2List[0].x == 1);
                    Assert.True(foo.Point2List[0].y == 2);
                    Assert.True(foo.Point2List[0].z == 3);

                }
                else
                    throw new Exception("error type");

                var json = ObjManager.SerializeString(foo);
                Console.WriteLine(json);

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
                    Position = new Point
                    {
                        X = 100,
                        Y = 200,
                    }
                };

                foo.Position2 = foo.Position;
                foo.My = foo;

                objmanager.WriteTo(data, new List<Foo> { foo, foo, foo });

                var (buff, len) = data.ToArray();
                var read = new xx.DataReader(buff, 0, len);

                Assert.True(objmanager.ReadFrom(read, out List<Foo> a) == 0);
                Assert.True(a.Count == 3);


            }
        }

        [Fact]
        public void test_gen2()
        {
            xx.ObjManager.SerializeStringFunc = (p) => Swifter.Json.JsonFormatter.SerializeObject(p, Swifter.Json.JsonFormatterOptions.Indented);

            CodeGen_CSharpTest.Register();
            var data = new xx.Data();
            var objmanager = new ObjManager();

            var foo = new PKGCSharp.Foo()
            {
                a = 100,
                sb = "33333",      
                x=PKGCSharp.EnumTypeId.C,
                testStruct = new PKGCSharp.TestStruct
                {
                    a = 1000,
                    b = 2000,
                    c = 300.3f,
                    x = 3400.02,
                    buff = new byte[] { 1, 2, 3, 4, 5 },
                    sb = "444444",
                    testclass=new PKGCSharp.Target
                    {
                        a=10
                    },
                    teststruct=new PKGCSharp.IsTestStruct
                    {
                        a=11
                    },
                    test_null=new PKGCSharp.IsTestStruct
                    {
                        a=100
                    }
                },
                testlist = new List<PKGCSharp.TestStruct>()
                {
                    new PKGCSharp.TestStruct
                    {
                        a = 1000,
                        b = 2000,
                        c = 300.3f,
                        x = 3400.02,
                        buff = new byte[] { 1, 2, 3, 4, 5 },
                        sb = "444444",
                        testclass=new PKGCSharp.Target
                        {
                            a=110
                        },
                        teststruct=new PKGCSharp.IsTestStruct
                        {
                            a=111
                        }
                    },
                    new PKGCSharp.TestStruct
                    {
                        a = 1000,
                        b = 2000,
                        c = 300.3f,
                        x = 3400.02,
                        buff = new byte[] { 1, 2, 3, 4, 5 },
                        sb = "444444",
                        testclass=new PKGCSharp.Target
                        {
                            a=101
                        },
                        teststruct=new PKGCSharp.IsTestStruct
                        {
                            a=111
                        }
                    },
                },
                testlist2=new List<PKGCSharp.Target>()
                {
                    new PKGCSharp.Target
                    {
                        a=1
                    },
                    new PKGCSharp.Target
                    {
                        a=2
                    }
                },
                test_null = new PKGCSharp.TestStruct
                {
                    a = 1000,
                    b = 2000,
                    c = 300.3f,
                    x = 3400.02,
                    buff = new byte[] { 1, 2, 3, 4, 5 },
                    sb = "444444",
                    testclass = new PKGCSharp.Target
                    {
                        a = 103
                    },
                    teststruct = new PKGCSharp.IsTestStruct
                    {
                        a = 114
                    }
                },
                testlist3=new List<int> { 1,2,3}
            };

            objmanager.WriteTo(data, foo);
            var (buff, len) = data.ToArray();

            var buff_p = new byte[10 + len];
            Buffer.BlockCopy(buff, 0, buff_p, 10, len);

            var read = new xx.DataReader(buff_p, 10, len);
            Assert.True(objmanager.ReadFrom(read, out PKGCSharp.Foo a) == 0);

            Assert.True(foo.a == a.a);
            Assert.True(foo.sb == a.sb);
            Assert.Equal(foo.x, a.x);
            Assert.True(foo.testStruct.a == a.testStruct.a);
            Assert.True(foo.testStruct.b == a.testStruct.b);
            Assert.True(foo.testStruct.c == a.testStruct.c);
            Assert.True(foo.testStruct.x == a.testStruct.x);
            Assert.Equal(foo.testStruct.buff, a.testStruct.buff);
            Assert.Equal(foo.testStruct.sb, a.testStruct.sb);
            Assert.Equal(foo.testStruct.teststruct.a, a.testStruct.teststruct.a);
            Assert.Equal(foo.testStruct.testclass.a, a.testStruct.testclass.a);
            Assert.Equal(foo.testStruct.test_null, a.testStruct.test_null);
            Assert.Equal(foo.testlist3, a.testlist3);         
            Assert.True(a.testStruct.test_null2 == null);

            Assert.True(a.testlist.Count==2);
            Assert.True(a.testlist2.Count == 2);

            Assert.True(foo.testlist[0].a == a.testlist[0].a);
            Assert.True(foo.testlist[0].b == a.testlist[0].b);
            Assert.True(foo.testlist[0].c == a.testlist[0].c);
            Assert.True(foo.testlist[0].x == a.testlist[0].x);
            Assert.Equal(foo.testlist[0].buff, a.testlist[0].buff);
            Assert.Equal(foo.testlist[0].sb, a.testlist[0].sb);
            Assert.Equal(foo.testlist[0].teststruct.a, a.testlist[0].teststruct.a);
            Assert.Equal(foo.testlist[0].testclass.a, a.testlist[0].testclass.a);

            Assert.True(foo.testlist[1].a == a.testlist[1].a);
            Assert.True(foo.testlist[1].b == a.testlist[1].b);
            Assert.True(foo.testlist[1].c == a.testlist[1].c);
            Assert.True(foo.testlist[1].x == a.testlist[1].x);
            Assert.Equal(foo.testlist[1].buff, a.testlist[1].buff);
            Assert.Equal(foo.testlist[1].sb, a.testlist[1].sb);
            Assert.Equal(foo.testlist[1].teststruct.a, a.testlist[1].teststruct.a);
            Assert.Equal(foo.testlist[1].testclass.a, a.testlist[1].testclass.a);

            Assert.True(foo.testlist2[0].a == a.testlist2[0].a);
            Assert.True(foo.testlist2[1].a == a.testlist2[1].a);


            Assert.True(a.test_null!=null);
            Assert.True(a.test_null2 == null);

            Assert.True(foo.test_null?.a == a.test_null?.a);
            Assert.True(foo.test_null?.b == a.test_null?.b);
            Assert.True(foo.test_null?.c == a.test_null?.c);
            Assert.True(foo.test_null?.x == a.test_null?.x);
            Assert.Equal(foo.test_null?.buff, a.test_null?.buff);
            Assert.Equal(foo.test_null?.sb, a.test_null?.sb);
            Assert.Equal(foo.test_null?.teststruct.a, a.test_null?.teststruct.a);
            Assert.Equal(foo.test_null?.testclass.a, a.test_null?.testclass.a);
        }
    }
}
