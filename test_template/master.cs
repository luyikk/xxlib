#pragma warning disable CS0169
using TemplateLibrary;


namespace PKG.Enum
{
    [Desc("Test Enum")]
    public enum TypeId 
    {
        [Desc("A")]
        a = 1,
        [Desc("B")]
        b = 2,
        [Desc("C")]
        c = 3
    }
}


namespace PKG
{
    [Desc("Test Struct"),Struct]
    public class TestStruct
    {
        [Desc("test enum")]
        PKG.Enum.TypeId P =PKG.Enum.TypeId.a;
        [Desc("test i32")]
        int a = 55;
        int b = 55;
        [Desc("test f32")]
        float c = 5;
        [Desc("test f64")]
        double x = 100.0;
        [Desc("test string")]
        string sb = "123123";
        [Desc("test buff")]
        byte[] buff;
        [Desc("test btreemap")]
        Dict<int, string> table;
        [Desc("test hashmap")]
        HashMap<int, string> hashtable;
        [Desc("test hashset")]
        HashSet<int> hashset;
    }

    [Desc("Test Struct"),TypeId(12),Compatible]
    public class TestBase
    {
        float x = 1.0f;
        float y = 2.0f;
    }

   [Desc("Test Struct"),TypeId(11)]
    public class TestStruct2: TestBase
    {
        [Desc("test enum")]
        PKG.Enum.TypeId P = PKG.Enum.TypeId.a;
        [Desc("test i32")]
        int a = 55;
        int b = 55;
        [Desc("test f32")]
        float c = 5;
        [Desc("test f64")]
        double x = 100.0;
        [Desc("test string")]
        string sb = "123123";
        [Desc("test buff")]
        byte[] buff;
        [Desc("test btreemap")]
        Dict<int, string> table;
        [Desc("test hashmap")]
        HashMap<int, string> hashtable;
        [Desc("test hashset")]
        HashSet<int> hashset;
        [Desc("test weak my")]
        Weak<TestStruct2> weak_my;
        [Desc("test sharedptr my")]
        Shared<TestStruct2> shard_my;

    }
}

namespace PKG.AA
{
    [Desc("Test Struct"), Struct,Compatible]
    public class Test_B:TestStruct
    {
        [Desc("test enum")]
        PKG.Enum.TypeId P = PKG.Enum.TypeId.b;
        [Desc("test sharedptr my")]
        Shared<TestStruct2> ptr;

        [Desc("test sharedptr xy")]
        Shared<TestBase> shard_xy;
        [Desc("test ref")]
        Shared<PKG.Ref.TypeName> ref_name;
    }
}



namespace PKG.P
{
    [TypeId(122), Desc("Ponit"), Compatible]
    class Point
    {
        int X;
        int Y;
        double? Z;
    }

    [Struct, Desc("Ponit2")]
    class Point2
    {
        float x;
        float y;
    }

    [Struct, Desc("Ponit3"), Compatible]
    class Point3 : Point2
    {
        float z;
    }

    [Struct, Desc("Player"), Compatible]
    class Player : Point2
    {
        Point3 position;
        Nullable<Point3> position2;
        int? px;
        List<Point2> Point2List;
    }
}


namespace PKG
{

    [TypeId(121), Desc("Base")]
    class Base
    {
        [Desc("S1")]
        int S1;
        [Desc("S2")]
        string S2;

        PKG.P.Point3 sp1;
        Nullable<PKG.P.Point3> sp2;
        Nullable<PKG.P.Point3> sp3;
        int? px;
        List<PKG.P.Point3> Point2List;
    }


    [TypeId(123), Desc("Foo"), Compatible]
    class Foo : Base
    {
        int P1;
        float P2;
        string P3;
        byte[] Buff;
        List<uint> Data;
        Shared<PKG.P.Point> Position;
        Shared<PKG.P.Point> Position2;
        Shared<Foo> My;
        List<Shared<PKG.P.Point>> Positions;
    }

}