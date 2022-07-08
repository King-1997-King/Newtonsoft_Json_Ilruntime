using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ILRuntime.Runtime.Generated
{
    public unsafe class Newtonsoft_Json_Redirections
    {
        public static void Register(Enviorment.AppDomain app)
        {
            MethodInfo[] jsonConvertMethods = typeof(JsonConvert).GetMethods();
            for (int i = 0; i < jsonConvertMethods.Length; i++)
            {
                if (jsonConvertMethods[i].Name == "DeserializeObject" && jsonConvertMethods[i].IsGenericMethodDefinition)
                {
                    var param = jsonConvertMethods[i].GetParameters();
                    if (param.Length == 1)
                    {
                        app.RegisterCLRMethodRedirection(jsonConvertMethods[i], DeserializeObject1);
                    }
                    else
                    {
                        if (param[1].ParameterType == typeof(JsonSerializerSettings))
                        {
                            app.RegisterCLRMethodRedirection(jsonConvertMethods[i], DeserializeObject2);
                        }
                        else if (param[1].ParameterType == typeof(JsonConverter[]))
                        {
                            app.RegisterCLRMethodRedirection(jsonConvertMethods[i], DeserializeObject3);
                        }
                    }
                }
            }

            MethodInfo[] jTokenMethods = typeof(JToken).GetMethods();
            for (int i = 0; i < jTokenMethods.Length; i++)
            {
                if (jTokenMethods[i].Name == "ToObject" && jTokenMethods[i].IsGenericMethodDefinition)
                {
                    var param = jTokenMethods[i].GetParameters();
                    if (param.Length == 0)
                    {
                        app.RegisterCLRMethodRedirection(jTokenMethods[i], ToObject1);
                    }
                    else if (param.Length == 1)
                    {
                        app.RegisterCLRMethodRedirection(jTokenMethods[i], ToObject2);
                    }
                }
            }
        }

        static StackObject* DeserializeObject1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String json = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);
            var type = __method.GenericArguments[0].ReflectionType;
            var result_of_this_method = JsonConvert.DeserializeObject(json, type, (JsonSerializerSettings)null);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* DeserializeObject2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            JsonSerializerSettings settings = (JsonSerializerSettings)typeof(JsonSerializerSettings).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            string json = (string)typeof(string).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;
            var result_of_this_method = JsonConvert.DeserializeObject(json, type, settings);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* DeserializeObject3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            JsonConverter[] converter = (JsonConverter[])typeof(JsonConverter[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            string json = (string)typeof(string).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;
            var result_of_this_method = JsonConvert.DeserializeObject(json, type, converter);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToObject1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            JToken instance_of_this_method = (JToken)typeof(JToken).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;
            var result_of_this_method = instance_of_this_method.ToObject(type);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToObject2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            JsonSerializer jsonSerializer = (JsonSerializer)typeof(JsonSerializer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            JToken instance_of_this_method = (JToken)typeof(JToken).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;
            var result_of_this_method = instance_of_this_method.ToObject(type, jsonSerializer);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }
    }
}
