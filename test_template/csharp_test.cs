#pragma warning disable CS0169
using TemplateLibrary;


namespace PKGCSharp
{


    [Desc("Test Enum")]
    public enum EnumTypeId:long
    {
        [Desc("A")]
        A = 1,
        [Desc("B")]
        B = 2,
        [Desc("C")]
        C = 3
    }

    [Struct]
    public class IsTestStruct
    {
        [Desc("test i32")]
        int a = 55;
    }


    [Desc("Test Struct"), Struct]
    public class TestStruct
    {
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

        Shared<Target> testclass;
        IsTestStruct teststruct;

        Nullable<IsTestStruct> test_null;
        Nullable<IsTestStruct> test_null2;
    }

    [Desc("Test Class"), TypeId(1002)]
    public class Target
    {
        [Desc("test i32")]
        int a = 55;
    }

    [Desc("Test Class"), TypeId(1001),Compatible]
    public class Foo
    {
        [Desc("test string")]
        string sb = "123123";
        [Desc("test i32")]
        int a = 5;
        [Desc("test enum")]
        EnumTypeId x = EnumTypeId.A;
        [Desc("struct")]
        TestStruct testStruct;

        [Desc("test list struct")]
        List<TestStruct> testlist;

        [Desc("test list class")]
        List<Shared<Target>> testlist2;
        List<int> testlist3;

        Nullable<TestStruct> test_null;

        Nullable<TestStruct> test_null2;
    }
}
