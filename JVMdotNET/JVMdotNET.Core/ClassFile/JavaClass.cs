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

        public bool IsResolved { get; private set; }
        public VersionInfo Version { get; private set; }
        public ConstantPoolItemBase[] ConstantPool { get; private set; }
        public ClassAccessFlags AccessFlags { get; private set; }
        public string Name { get; private set; }
        public string Super { get; private set; }
        public JavaClass SuperClass { get; private set; }
        public string[] Interfaces { get; private set; }
        public IDictionary<string, StaticField> StaticFields { get; private set; }
        public IDictionary<string, InstanceField> InstanceFields { get; private set; }
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
            this.IsResolved = false;

            this.InstanceFields = new Dictionary<string, InstanceField>();
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

        private void LoadFields(IEnumerable<FieldInfo> fields)
        {
            int instanceFieldsLength = InstanceFields.Count;
            int i = 0;

            foreach (var field in fields)
            {
                if (field.AccessFlags.HasFlag(FieldAccessFlags.Static))
                {
                    StaticFields.Add(field.Name, new StaticField(field));
                }
                else
                {
                    InstanceFields.Add(field.Name, new InstanceField(field, instanceFieldsLength + i));
                    i++;
                }
            }
        }

        public void Resolve(RuntimeClassArea classArea)
        {
            if (IsResolved)
            {
                return;
            }
            
            SuperClass = classArea.GetClass(Super);
            SuperClass.Resolve(classArea);
            LoadFields(SuperClass.InstanceFields.Values.Select(f => f.Info));
            this.IsResolved = true;
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

        //TODO: ??
        public InstanceField GetInstanceFieldInfo(string fieldName)
        {
            //TODO: co kdyz field neexistuje
            return InstanceFields[fieldName];
        }

        public string[] ValidAttributes
        {
            get { return ValidAttributeNames; }
        }

        public T GetAttribute<T>(string attributeName) where T : Attributes.AttributeBase
        {
            AttributeBase attribute;
            if (Attributes.TryGetValue(attributeName, out attribute))
            {
                return (T)attribute;
            }
            else
            {
                throw new InvalidOperationException(string.Format("Attribute with name {0} not found in class item {1}.", attributeName, Name));
            }
        }
    }
}
