using Google.Protobuf;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using System.IO;
using UnityEngine;

/// <summary>
/// 测试环境最开始执行的代码
/// 用于启动ilruntime
/// 
/// 提示: 仅仅是为了最简单的启动ilruntime, 真实项目中还要做其他很多事情, 但是与本项目无关, 因此省略
/// </summary>
public class StartMono : MonoBehaviour
{
    AppDomain appdomain;

    void Start()
    {
        // 本项目对于 Newtonsoft.Json 的修改是对于 ilruntime 的扩展支持,
        // 并没有修改 Newtonsoft.Json 在正常情况下的运行
        //
        // 解开这两行注释, 将以 非ilruntime 的方式运行测试代码.
        // 在测试新的案例的时候如果报错, 
        // 可以先看看在 非ilruntime 情况下, 即 Newtonsoft.Json 本身是否是支持这种案例写法的
        // 如果 Newtonsoft.Json 本身不支持, 则在 ilruntime 模式下也不支持
        //Main.Start();
        //Debug.LogError("以非ilruntime模式运行");
        //return;

        Debug.LogError("以ilruntime模式运行");
        appdomain = new AppDomain();

        byte[] dll = File.ReadAllBytes("Library/ScriptAssemblies/HotScripts.dll");
        byte[] pdb = File.ReadAllBytes("Library/ScriptAssemblies/HotScripts.pdb");

        System.IO.MemoryStream fs = new MemoryStream(dll);
        System.IO.MemoryStream p = new MemoryStream(pdb);

        appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

        MustAdd();

        appdomain.Invoke("Main", "Start", null, null);
    }

    /// <summary>
    /// 必须要添加的代码
    /// 
    /// 这个函数里面的是必须要添加的代码, 但是具体添加到什么地方由自己的ilruntime项目架构决定
    /// </summary>
    private void MustAdd()
    {
        Newtonsoft_Json_Redirections.Register(appdomain);

    }
}
