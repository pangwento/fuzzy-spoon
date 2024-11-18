优化GC的核心是减少分配和延长对象生命周期，通过以下手段可以有效降低GC压力：
+ 使用对象池管理短生命周期的对象
+ 避免不必要的动态分配，尤其是字符串、列表和临时对象
+ 优化资源加载和内存管理
+ 利用Unity提供的攻击监测和分析内存分配热点。

--------------------------------------------------------------------------------
### 减少频繁的内存分配
+ 避免在循环中创建新的对象，如字符串拼接、临时列表等
```c#
// 不推荐：每帧都会创建新的字符串对象
void Update()
{
    string message = "Score:" + score.ToString();
}
// 推荐：使用StringBuilder或者缓存字符串
private StringBuilder sb = new StringBuilder();
void Update()
{
    sb.Clear();
    sb.Append("Score:").Append(score);
    Debug.Log(sb.ToString());
}
```
+ 对象池：
对于频繁创建和销毁的对象(如敌人、子弹、特效等），使用对象池复用对象，避免重复分配和销毁内存
```c#
public class ObjectPool
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    public GameObject Get(GameObject prefab)
    {
        if(pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;        
        }    
        return GameObject.Instantiate(prefab);
    }
    public void Reture(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);'    
    }
}
```
### 避免频繁的字符串操作
字符串是不可变对象，每次修改字符串都会创建新的实例。
解决方案：
+ 使用StringBuilder代替频繁的字符串拼接
+ 使用预分配的数组或者列表存储固定格式的字符串，避免动态分配
### 优化集合操作
+ 使用List<T>.Clear代替新创建列表new List<T>()
+ 避免频繁的数组、列表扩容，初始化集合时，提前设置合适的容量
### 避免大型对象的频繁分配
回收代价比较高   
解决方案：
+ 将大对象拆分为多个小对象，减少分配到LOH的概率
+ 避免频繁创建大数组，改用使用预先分配的固定大小数组
### 减少频繁的资源加载和销毁
资源加载（Instantiate或者Resource.Load)和销毁(Destroy)会带来显著的GC开销。   
解决方案：
+ 对象池复用：优化频繁实例化和销毁的对象
+ 异步加载资源：使用Addressables或者AssetBundle进行异步加载，减少一次性加载带来的GC压力
+ 延迟销毁：延迟消耗对象，批量处理资源清理
### 优化脚本设计
+ 避免闭包分配
+ 避免多余的new分配：对于重复调用的逻辑，使用静态实例或缓存来避免多次分配
### 使用内存分析工具
+ Profiler:查看垃圾回收频率、内存分配位置
+ MemoryProfiler:查看堆内存和大型对象的分配情况
+ DeepProfiling:查找代码中频繁分配内存的地方
### 避免临时协程分配
在协程中使用匿名方法或IEnumerator会产生额外的GC压力    
解决方案：
使用静态协程或对象池复用IEnumerator
```c#
// 不推荐
IEnumerator MyCoroutine()
{
    yield return new WaitForSeconds(1f);
}
// 推荐 使用预分配
private static readonly WaitForSeconds wait = new WaitForSecond(1f);
IEnmerator MyCoroutine()
{
    yield return wait;
}
```
### 避免频繁分配临时Vector、Quaternion等
### 管理脚本生命周期
+ 减少Update调用频率
+ 避免空的update方法
### 避免过多的动态分配UI元素
+ 缓存常用的UI元素
+ 避免频繁调用Instantiate创建UI
+ 使用TMP代替普通的UI.Text
### 使用Incremental GC(增量式垃圾回收)
