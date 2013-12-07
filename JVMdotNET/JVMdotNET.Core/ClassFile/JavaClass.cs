using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class JavaClass : IAttributteContainer
    {
        private static readonly string[] ValidAttributeNames = { 
                                                                   InnerClassesAttribute.Name,
                                                                   EnclosingMethodAttribute.Name, 
                                                                   SyntheticAttribute.Name,
                                                                   SignatureAttribute.Name,
                                                                   SourceFileAttribute.Name,
                                                                   SourceDebugExtensionAttribute.Name,
                                                                   DeprecatedAttribute.Name,
                                                                   RuntimeVisibleAnnotationsAttribute.Name,
                                                                   RuntimeInvisibleAnnotationsAttribute.Name,
                                                                   BootstrapMethodsAttribute.Name
                                                               };

        public VersionInfo Version { get; private set; }
        public ConstantPoolItemBase[] ConstantPool { get; private set; }
        public ClassAccessFlags AccessFlags { get; private set; }
        public string Name { get; private set; }
        public string Super { get; private set; }
        public string[] Interfaces { get; private set; }
        public IDictionary<string, StaticField> StaticFields { get; private set; }
        public IDictionary<string, FieldInfo> Fields { get; private set; }
        public IDictionary<string, MethodInfo> Methods { get; private set; }
        public IDictionary<string, AttributeBase> Attributes { get; private set; }

        public JavaClass(VersionInfo version, ConstantPoolItemBase[] constantPool, 
            ClassAccessFlags accessFlags, string name, string super,
            string[] interfaces, FieldInfo[] fields, MethodInfo[] methods,
            IDictionary<string, AttributeBase> attributes)
        {
            this.Version = version;
            this.ConstantPool = constantPool;
            this.AccessFlags = accessFlags;
            this.Name = name;
            this.Super = super;
            this.Interfaces = interfaces;
            this.Attributes = attributes;

            this.Fields = new Dictionary<string, FieldInfo>();
            this.StaticFields = new Dictionary<string, StaticField>();

            LoadFields(fields);
            LoadMethods(methods);
        }

        private void LoadMethods(MethodInfo[] methods)
        {
            this.Methods = new Dictionary<string, MethodInfo>();
            foreach (var method in methods)
            {
                Methods.Add(method.Name, method);
            }
        }

        public void LoadFields(FieldInfo[] fields)
        {
            foreach (var field in fields)
            {
                if (field.AccessFlags.HasFlag(FieldAccessFlags.Static))
                {
                    StaticFields.Add(field.Name, new StaticField(field));
                }
                else
                {
                    Fields.Add(field.Name, field);
                }
            }
        }


        public object GetStaticFieldValue(string fieldName)
        {
            //TODO: co kdyz field neexistuje
            return StaticFields[fieldName].Value;
        }

        public void SetStaticFieldValue(string fieldName, object value)
        {
            //TODO: co kdyz field neexistuje
            StaticFields[fieldName].Value = value;
        }

        public FieldInfo GetFieldInfo(string fieldName)
        {
            //TODO: co kdyz field neexistuje
            return Fields[fieldName];
        }

        public string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

        public T GetAttribute<T>(string attributeName) where T : Attributes.AttributeBase
        {
            throw new NotImplementedException();
        }
    }
}
