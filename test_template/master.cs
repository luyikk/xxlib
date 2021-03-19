#pragma warning disable CS0169
using TemplateLibrary;



namespace PKG.P
{
    [TypeId(12), Desc("Ponit"),Compatible]
    class Point
    {
        int X;
        int Y;
        double? Z;
    }

    [Struct,Desc("Ponit2")]
    class Point2
    {
        float x;
        float y;
    }

    [Struct, Desc("Ponit3"), Compatible]
    class Point3 :Point2
    {
        float z;
    }

    [Struct, Desc("Player"), Compatible]
    class Player : Point2
    {
        Point3 position;
        Nullable<Point3> position2;
        int? px;
    }
}


namespace PKG
{

    [TypeId(11), Desc("Base")]
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
    }


    [TypeId(13), Desc("Foo"), Compatible]
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