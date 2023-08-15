using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;

// https://source.dot.net/#System.Private.CoreLib/src/System/Reflection/MdImport.cs,8570c9fc391730c5
internal enum MetadataTokenKind
{
    Module = 0x00,
    TypeRef = 0x01,
    TypeDef = 0x02,
    FieldDef = 0x04,
    MethodDef = 0x06,
    ParamDef = 0x08,
    InterfaceImpl = 0x09,
    MemberRef = 0x0a,
    CustomAttribute = 0x0c,
    Permission = 0x0e,
    Signature = 0x11,
    Event = 0x14,
    Property = 0x17,
    ModuleRef = 0x1a,
    TypeSpec = 0x1b,
    Assembly = 0x20,
    AssemblyRef = 0x23,
    File = 0x26,
    ExportedType = 0x27,
    ManifestResource = 0x28,
    GenericPar = 0x2a,
    MethodSpec = 0x2b,
    String = 0x70,
    Name = 0x71,
    BaseType = 0x72,
    Invalid = 0x7F,
}