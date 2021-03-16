using System;
using System.Collections.Generic;
using Test.PKG;
using Test.PKG.P;
using Xunit;
using xx;

namespace xxlibtest
{
    public class TestGen
    {
        [Fact]
        public void test_gen()
        {
            Test.AllTypes.Register();
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
                    Buff=new byte[] {1,2,3},
                    Data=new List<uint> { 1,2,3,4},
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

                objmanager.WriteTo(data, foo);
                var (buff, len) = data.ToArray();

                var read = new xx.DataReader(buff, len);
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
                var read = new xx.DataReader(buff, len);

                Assert.True(objmanager.ReadFrom(read, out List<Foo> a) == 0);
                Assert.True(a.Count == 3);
            }
        }
    }
}
