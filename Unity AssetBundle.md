AssetBundle 是 Unity 提供的一种资源打包和按需加载机制，用于将游戏资源（如模型、纹理、音频等）打包成独立的文件，以便在运行时按需加载。这使得游戏可以通过动态加载和卸载资源来节省内存和提升性能，尤其适合大规模游戏或需要频繁更新的项目。
### AssetBundle概念
+ AssetBundle是一种unity特有的文件格式，用于将游戏资源打包成一个或多个文件，通常是.assetbundle扩展名
+ 按需加载：AssetBundle支持按需加载，这意味着资源只会在需要时加载，而不是一开始就全部加载到内存中。
+ 扩平台：可以为不同平台粉白打包AssetBundle，从而优化不同平台的资源加载

#### AssetBundle基本使用
+ 创建和打包AssetBundle
    * 设置AssetBundle标签
    * 编写打包脚本
```c#
public class AssetBundleBuilder
{
    [MenuItem("Tools/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string outputPath = "Assets/AssetBundles";
        if(!System.IO.Directory.Exists(outputPath))
            System.IO.Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);
    }
}
```
+ 加载AssetBundle
  * 从文件系统加载
```c#
    public class AssetBundleLoader : MonoBehaivour
    {
        private AssetBundle bundle;
        void Start()
        {
            string path = Application.streamingAssetPath + "/AssetBundles/mybundle";
            bundle = AssetBundle.LoadFromFile(path);
            
            if(bundle != null)
            {
                GameObject prefab = bundle.LoadAsset<GameObject>("myprefab");
                Instantiate(prefab);
            }
            else
            {
                Debug.Log("AssetBundle加载失败")；
            }
        }
        void OnDestroy()
        {
            if(bundle != null)
                bundle.Unload(false); // false 表示保留已加载的资源
        }
    }
```
   * 从远程服务器加载bundle
```c#
    IEnumerator LoadFromRemote(string url)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.Success)
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            GameObject prefab = bundle.LoadAsset<GameObject>("myprefab");
            Instantiate(prefab);
        }
        else
        {
            Debug.LogError("加载远程AssetBundle失败：" + request.error);
        }
    }
```
+ 卸载AssetBundle
```c#
    if(bundle != null)
    {
        bundle.Unload(false);
    }
```

#### AssetBundle的依赖管理
unity会为每个AssetBundle生成一个Manifest文件，记录所有AssetBundle的依赖关系
+ 获取和加载依赖
```c#
    AssetBundle manifestBundle = AssetBundle.LoadFromFile("path/to/AssetBundles");
    AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    string[] dependencies = manifest.GetAllDependencies("mybundle");
    foreach(string dependency in dependencies)
    {
        AssetBundle.LoadFromFile("path/to/AssetBundles/"+ dependency);
    }
```
### AssetBundle的优化
为了提升加载速度和减少内存使用，以下是一些常用的AssetBundle优化方法：
+ 合并小资源
将多个小资源合并到一个AssetBundle中，减少AssetBundle文件数量，减少加载和依赖管理的复杂度
+ 按场景或模块划分AssetBundle
根据游戏的不同场景或者功能模块划分AssetBundle，这样可以按需加载相关资源
+ 异步加载
使用异步加载方法（如LoadFromFileAsync和LoadAssetAsync)来避免阻塞主线程，提升性能
+ 使用压缩和加密
    * 压缩：AssetBundle支持压缩，可以减小文件体积，节省磁盘和网络带宽
    * 加密：可以通过加密保护AssetBundle文件，防止被修改或盗用。
### AssetBundle的更行和版本管理
+ 使用版本控制管理AssetBundle,确保客户端始终加载最新的资源
+ 通过Hash值判断是否需要下载更新
```c#
    string hash = manifest.GetAssetBunldeHash("mybundle").ToString();
```
### 如何检查AssetBundle循环依赖
AssetBundle的循环依赖指的是两个或者多个AssetBundle相互依赖，形成一个闭环依赖关系
+ 使用AssetBundle Manifest文件检测循环依赖
    每个构件AssetBundle时，Unity会生成一个AssetBundleManifest文件。该文件记录了所有AssetBundle的依赖关系，包括每个AssetBundle依赖的其他AssetBundle文件。
    + 1.生成和获取Manifest文件。BuildPipeline.BuildAssetBundles方法构建AB,并生成对应的Manifest文件。
    + 2.读取Manifest文件并检查依赖关系。读取所有AB之间的依赖关系，通过地柜查找依赖关系，检测是否存在循环依赖。
    ```c#
    public  class AssetBundleDependencyChercker : MonoBehaviour
    {
        public static void CheckAssetBundleDependencies(string assetBundlePath)
        {
            var manifestBundle = AssetBundle.LoadFromFile(assetBundlePath + "/AssetBundles");
            var manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            
            string[] assetBundles = manifest.GetAllAssetBundles();
            Dictionary<string, List<string>> dependencies = new Dictionary<string, List<string>>();
            forearch(var ab in assetBundles)
            {
                string[] deps = manifest.GetAllDependencies(ab);
                dependencies[ab] = new List<string>(deps);
            }
            
            foreach(var bundle in dependencies)
            {
                HashSet<string> visited = new HashSet<string>();
                if(HasCircularDependency(bundle.Key, dependencies, visited))
                {
                    Debug.LogError("Found circular dependency in AssetBundle:"+bundle.Key);
                }
            }
        }
        
        private static bool HasCircularDependency(string assetBundle, Dictionary<string, List<string>> dependencies, HashSet<string> visited)
        {
            if(visited.Contains(assetBundle))
            {
                return true;
            }
            visited.Add(assetBundle);
            
            foreach(var dep in dependencies[assetBundle])
            {
                if(HasCircularDependency(dep, dependecies, visited))
                {
                    return true;
                }
            }
            visited.Remove(assetBundle);
            return false;
        }
    }
