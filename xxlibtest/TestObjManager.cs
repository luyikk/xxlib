using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var reader = new DataReader(buff, len);

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
                var reader = new DataReader(buff, len);

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

            objmanager.WriteTo(data,new byte[] { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<byte> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<short> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<int> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<long> { 1, 2, 3, 4, 5 });
            objmanager.WriteTo(data, new List<string> { "1","2",null,"4","5" });
            objmanager.WriteTo(data, new List<long?> { 1, null, 3, null, 5 });

            var (buff, len) = data.ToArray();
            var reader = new DataReader(buff, len);

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


    }
}
