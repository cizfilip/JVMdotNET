using JVMdotNET.Core.ClassFile.Attributes;
using JVMdotNET.Core.ClassFile.ConstantPool;
using JVMdotNET.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVMdotNET.Core.ClassFile
{
    internal class JavaClass : AttributeContainer
    {
        public bool IsResolved { get; protected set; }
        public bool IsInitialized { get; set; }

        public VersionInfo Version { get; protected set; }
        public ConstantPoolItemBase[] ConstantPool { get; protected set; }
        public ClassAccessFlags AccessFlags { get; protected set; }
        public string Name { get; protected set; }
        public string Super { get; protected set; }
        public JavaClass SuperClass { get; protected set; }
        public string[] Interfaces { get; protected set; }

        protected IDictionary<string, StaticField> StaticFields { get; set; }
        protected IDictionary<string, InstanceField> InstanceFields { get; set; }
        protected IDictionary<string, MethodInfo> Methods { get; set; } // key is Name + Descriptor

        public IDictionary<string, AttributeBase> Attributes
        {
            set
            {
                base.attributes = value;
            }
        }

        //For Library classes
        protected JavaClass() 
        {
            this.Methods = new Dictionary<string, MethodInfo>();
            this.InstanceFields = new Dictionary<string, InstanceField>();
            this.StaticFields = new Dictionary<string, StaticField>();

            this.IsResolved = false;
        }

        public JavaClass(VersionInfo version, ConstantPoolItemBase[] constantPool, 
            ClassAccessFlags accessFlags, string name, string super,
            string[] interfaces)
            : this()
        {
            this.Version = version;
            this.ConstantPool = constantPool;
            this.AccessFlags = accessFlags;
            this.Name = name;
            this.Super = super;
            this.Interfaces = interfaces;
        }

        internal void AddMethods(MethodInfo[] methods)
        {
            foreach (var method in methods)
            {
                Methods.Add(method.Key, method);
            }
        }

        internal void AddFields(FieldInfo[] fields)
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

        private void ResolveInstanceFields(IEnumerable<InstanceField> instanceFields)
        {
            foreach (var instanceField in instanceFields)
            {
                if (!InstanceFields.ContainsKey(instanceField.Info.Name))
                {
                    InstanceFields.Add(instanceField.Info.Name, instanceField);
                }
            }
        }

        private void ResolveStaticFields(IEnumerable<StaticField> staticFields)
        {
            foreach (var staticField in staticFields)
            {
                if (!StaticFields.ContainsKey(staticField.Info.Name) && 
                    !staticField.Info.AccessFlags.HasFlag(FieldAccessFlags.Private))
                {
                    StaticFields.Add(staticField.Info.Name, staticField);
                }
            }
        }

        private void ResolveVirtualMethods(IDictionary<string, MethodInfo> superMethods)
        {
            foreach (var method in superMethods.Values)
            {
                if (!Methods.ContainsKey(method.Key) &&
                    !method.IsInitializationMethod &&
                    (method.AccessFlags.HasFlag(MethodAccessFlags.Public) ||
                     method.AccessFlags.HasFlag(MethodAccessFlags.Protected)))
                {
                    Methods.Add(method.Key, method);
                }
            }
        }

        public void Resolve(RuntimeClassArea classArea)
        {
            this.IsResolved = true;

            if (string.IsNullOrEmpty(Super))
            {
                return;
            }
            
            SuperClass = classArea.GetClass(Super);
            ResolveStaticFields(SuperClass.StaticFields.Values);
            ResolveInstanceFields(SuperClass.InstanceFields.Values);
            ResolveVirtualMethods(SuperClass.Methods);
        }

        public object GetStaticFieldValue(string fieldName)
        {
            return GetStaticField(fieldName).Value;
        }

        public void SetStaticFieldValue(string fieldName, object value)
        {
            GetStaticField(fieldName).Value = value;
        }

        public InstanceField GetInstanceFieldInfo(string fieldName)
        {
            InstanceField field;
            if (InstanceFields.TryGetValue(fieldName, out field))
            {
                return field;
            }

            throw new FieldNotFoundException(string.Format("Instance field {0} not found in class {1}.", fieldName, Name));
        }

        public MethodInfo GetMethodInfo(MethodRefConstantPoolItem methodRef)
        {
            return GetMethodInfo(methodRef.Key);
        }

        public MethodInfo GetMethodInfo(string methodKey)
        {
            MethodInfo method;
            if (Methods.TryGetValue(methodKey, out method))
            {
                return method;
            }

            throw new MethodNotFoundException(string.Format("Method {0} not found in class {1}.", methodKey, Name));
        }

        public bool TryGetMethodInfo(string methodKey, out MethodInfo method)
        {
            return Methods.TryGetValue(methodKey, out method);
        }

        public bool TryGetClassConstructor(out MethodInfo classConstructor)
        {
            return Methods.TryGetValue("<clinit>()V", out classConstructor);
        }

        public int GetInstanceFieldsCount()
        {
            return InstanceFields.Count;
        }

        public bool IsSubClassOf(JavaClass otherClass)
        {
            if (otherClass == null)
            {
                throw new ArgumentNullException("otherClass");
            }

            JavaClass thisSuper = this;
            while (thisSuper != null)
            {
                if (thisSuper == otherClass)
                {
                    return true;
                }
                thisSuper = thisSuper.SuperClass;
            }

            return false;
        }


        #region Private Methods
        
        private StaticField GetStaticField(string fieldName)
        {
            StaticField field;
            if (StaticFields.TryGetValue(fieldName, out field))
            {
                return field;
            }

            throw new FieldNotFoundException(string.Format("Static field {0} not found in class {1}.", fieldName, Name));
        }

        #endregion
    }
}
