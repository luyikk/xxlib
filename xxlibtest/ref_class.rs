use xxlib::*;
use xxlib_builder::*;

#[allow(dead_code)]
const  MD5:&str="#*MD5<22670ffa4d2344cb7039e8ed23995106>*#";

#[allow(dead_code,non_snake_case)]
pub fn CodeGen_Ref_class(){
    ObjectManager::register::<PKG_Ref_TypeName>(stringify!(PKG_Ref_TypeName));
}

/// Test Ref
pub const PKG_REF_TYPENAME_TYPE_ID:u16 = 1222u16;
#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[derive(build,Debug)]
#[cmd(typeid(1222),compatible(false))]
pub struct PKG_Ref_TypeName{
    #[cmd(default("null"))]
    pub name:String,
}