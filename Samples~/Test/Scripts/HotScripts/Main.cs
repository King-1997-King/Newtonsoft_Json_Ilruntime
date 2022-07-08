using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 这个类是运行在ilruntime中的
/// </summary>
public class Main
{

    /// <summary>
    /// 一切热更代码从这里开始
    /// </summary>
    public static void Start()
    {
        Test1 data1 = new Test1();
        data1.m_int = 5;
        data1.m_float = 3.5f;
        data1.m_string = "mj";

        string str = JsonConvert.SerializeObject(data1);
        Test1 data2 = JsonConvert.DeserializeObject(str, typeof(Test1)) as Test1;
        Debug.LogError(JsonConvert.SerializeObject(data2));
    }

    public class Test1
    {
        public int m_int;
        public float m_float;
        public string m_string;
    }
}
