use xxlib::*;
use xxlib_builder::*;

#[allow(dead_code)]
const  MD5:&str="#*MD5<197c8995f4ff02389041d978799ba585>*#";

#[allow(dead_code,non_snake_case)]
pub fn CodeGen_Ref_class(){
    ObjectManager::register::<PKG_Ref_TypeName>(stringify!(PKG_Ref_TypeName));
}

/// Test Ref
#[allow(unused_imports,dead_code,non_snake_case,non_camel_case_types)]
#[derive(build,Debug)]
#[cmd(typeid(1222),compatible(false))]
pub struct PKG_Ref_TypeName{
    #[cmd(default("null"))]
    pub name:String,
}