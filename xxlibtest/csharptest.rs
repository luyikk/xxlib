use xxlib::*;
use xxlib_builder::*;

#[allow(dead_code)]
const  MD5:&str="#*MD5<80359ed0a7292163d1e0ca386706be13>*#";

#[allow(dead_code,non_snake_case)]
pub fn CodeGen_CSharpTest(){
    ObjectManager::register::<PKGCSharp_Target>(stringify!(PKGCSharp_Target));
    ObjectManager::register::<PKGCSharp_Foo>(stringify!(PKGCSharp_Foo));
}

/// Test Class
#[allow(unreachable_patterns,unused_imports,dead_code,non_snake_case,non_camel_case_types)]
pub const PKGCSHARP_TARGET_TYPE_ID:u16 = 1002u16;

/// Test Class
#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[derive(build,Debug)]
#[cmd(typeid(1002),compatible(false))]
pub struct PKGCSharp_Target{
    /// test i32
    #[cmd(default(55))]
    pub a:i32,
}

/// Test Class
#[allow(unreachable_patterns,unused_imports,dead_code,non_snake_case,non_camel_case_types)]
pub const PKGCSHARP_FOO_TYPE_ID:u16 = 1001u16;

/// Test Class
#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[derive(build,Debug)]
#[cmd(typeid(1001),compatible(true))]
pub struct PKGCSharp_Foo{
    /// test string
    #[cmd(default("123123"))]
    pub sb:String,
    /// test i32
    #[cmd(default(5))]
    pub a:i32,
    /// test enum
    #[cmd(default(PKGCSharp_EnumTypeId::A))]
    pub x:PKGCSharp_EnumTypeId,
    /// struct
    pub testStruct:PKGCSharp_TestStruct,
    /// test list struct
    pub testlist:Vec<PKGCSharp_TestStruct>,
    /// test list class
    pub testlist2:Vec<SharedPtr<PKGCSharp_Target>>,
    pub testlist3:Vec<i32>,
    pub test_null:Option<PKGCSharp_TestStruct>,
    pub test_null2:Option<PKGCSharp_TestStruct>,
}

#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[derive(build,Debug)]
#[cmd(compatible(false))]
pub struct PKGCSharp_IsTestStruct{
    /// test i32
    #[cmd(default(55))]
    pub a:i32,
}

/// Test Struct
#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[derive(build,Debug)]
#[cmd(compatible(false))]
pub struct PKGCSharp_TestStruct{
    /// test i32
    #[cmd(default(55))]
    pub a:i32,
    #[cmd(default(55))]
    pub b:i32,
    /// test f32
    #[cmd(default(5.0))]
    pub c:f32,
    /// test f64
    #[cmd(default(100.0))]
    pub x:f64,
    /// test string
    #[cmd(default("123123"))]
    pub sb:String,
    /// test buff
    pub buff:Vec<u8>,
    pub testclass:SharedPtr<PKGCSharp_Target>,
    pub teststruct:PKGCSharp_IsTestStruct,
    pub test_null:Option<PKGCSharp_IsTestStruct>,
    pub test_null2:Option<PKGCSharp_IsTestStruct>,
}

/// Test Enum
#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[build_enum(i64)]
pub enum PKGCSharp_EnumTypeId{
    /// A
    A = 1,
    /// B
    B = 2,
    /// C
    C = 3,
}