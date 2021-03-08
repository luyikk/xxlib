using System;
using TemplateLibrary;




namespace PKG
{

    [TypeId(11), Desc("Base"),Compatible]
    class Base
    {
        [Desc("S1")]
        int S1;
        [Desc("S2")]
        string S2;
    }

    [TypeId(12), Desc("Ponit")]
    class Point
    {
        int X;
        int Y;
        float? Z;
    }

    [TypeId(13), Desc("Foo"), Compatible]
    class Foo : Base
    {
        int P1;
        float P2;
        string P3;
        byte[] Buff;
        List<uint> Data;
        Shared<Point> Position;
        Shared<Point> Position2;
        Shared<Foo> My;
        List<Shared<Point>> Positions;
    }

}