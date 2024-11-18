在Unity中，协程是一种基于IEnumerator的特殊方法，用于实现异步或者分时执行操作。它允许在不阻塞主线程的情况下暂停执行一段代码，然后在后续帧中继续执行。
Unity协程是由Unity引擎的内部调度系统管理的。

--------------------------------------------------------------------------------
### 协程的核心：IEnumerator
Unity的写成依赖C#中的迭代器接口IEnumerator,它支持通过yield return暂停方法执行，并在之后继续
```c#
IEnumerator MyCoroutinue()
{
    Debug.Log("开始协程");
    yield return new WaitForSeconds(1f);
    Debug.Log("1秒后继续执行")
}
```
在这段代码中：
+ yield return new WaitForSeconds(1f)暂停了协程的执行
+ 协程在暂停后返回控制权交给Unity的主线程，并在1秒后恢复执行
--------------------------------------------------------------------------------
### Unity协程的本质
协程本质上是通过Unity的引擎主线程以分时方式运行的代码块，其底层原理可以分为以下几步：
 + StartCoroutine启动协程：当调用StartCoroutine时，Unity会将协程方法的IEnumerator实例注册到内部协程调度系统中，并且开始执行。
 + 第一帧执行到yield return: 协程方法从头开始运行，执行到第一个yield return, 然后暂定执行并返回控制权给unity.
 + 暂停与恢复
    - 如果yield return是一个特定的条件（如等待时间、帧数、某些事件），Unity的写成调度器会在满足条件后恢复协程的执行
    - 协程继续从yield return 之后的代码开始运行
 + 协程结束：当协程方法中的代码执行完毕或者通过某种方式中断（StopCoroutine),协程会从调度系统中移除。
--------------------------------------------------------------------------------
### 常见的yield return 类型
yield return的作用是告诉Unity在满足某些条件后继续执行协程。常见的返回值类型及其意义
+ null 暂停到下一帧继续执行
```c#
yield return null; // 等待下一帧
```
+ WaitForSeconds 等待指定时间后继续执行
```c#
yield return new WaitForSeconds(2f); // 等待2秒
```
+ WaitUntil和WaitWhile 根据条件判断是否继续执行
```c#
yield return new WaitUntil(()=> someCondition); // 等待条件为真
yield return new WaitWhile(()=> someCondition); // 等待条件为假
```
+ CustomYieldInstruction 创建自定义的等待条件
```c#
public class WaitForEvent:CustomYieldInstruction
{
    private bool isTriggered = false;
    public override bool keepWaiting => !isTriggered;
    public void Trigger()=> isTriggered = true;
}
```
+ 另一个协程, 可以嵌套调用其他线程
```
yield return StartCoroutinue(AnotherCoroutine);
```
--------------------------------------------------------------------------------
### Unity内部协程调度的工作流程
Unity使用一个内部的调度系统来管理协程。以下是协程调度的核心机制
+ 协程注册：当调用StartCoroutine时，Unity将协程的IEnumerator注册到一个列表中。
  - Unity会通过调用IEnumerator.MoveNext()方法驱动协程的执行
  - MoveNext返回true表示协程尚未完成，false表示协程结束
+ 帧更新检查：在每帧的更新阶段，Unity会检查所有注册的协程，并调用它们的MoveNext().
  - 如果协程返回了yield return null, 它将在下一帧继续
  - 如果协程返回了WaitForSeconds或类似等待条件对象，Unity会根据这些对象的条件决定何时继续执行协程
+ 完成或中断
  - 当协程方法执行到末尾时，或者显示调用StopCoroutine停止协程时，Unity会从内部协程队列中移除该协程。
--------------------------------------------------------------------------------
### 协程的优点和局限
+ 优点：
  + 代码逻辑清晰：协程使异步操作（如等待时间、资源加载）变得更直观
  + 不阻塞主线程：协程不会阻塞Unity的主线程，游戏逻辑可以继续执行。
  + 灵活的暂定条件：支持多种形式的暂停条件，如等待时间、事件触发等   
+ 局限：
  + 主线程运行：协程依然运行在Unity主线程中，不能真正实现多线程并行
  + 不可中断等待：如果协程在等待某个条件（如WaitForSeconds), 它将无法被外部事件直接中断，除非显示调用StopCoroutine.
  + 较高的管理成本：大量协程可能导致性能问题，需要合理管理和优化
--------------------------------------------------------------------------------
### 协程与线程的区别
|特性|协程|线程|
|-------|-------|-------|
|运行方式|由Unity主线程调度|独立运行，可并行执行|
|并行能力|无法并行，只能分时执行|支持并行|
|性能开销|轻量级，调度开销低|较高，需要操作系统管理|
|使用场景|异步操作、分时任务|CPU密集型计算、多线程并发任务|
--------------------------------------------------------------------------------
### 示例：加载场景的协程
```c#
public  class SceneLoader : MonoBehaviour
{
    public Slider progressBar;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));    
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName) ;
        asyncOperation.allowSceneActivation = false;
        while(!asyncOperation.isDone)   
        {
            progressBar.value = asyncOperation.progress;
            if(asyncOperation.progress >= 0.9f)
            {
                Debug.Log("场景加载完成，等待激活...");
                asyncOperation.allowSceneActivation = true;            
            }  
            yield return null;      
        }
        Debug.Log("场景加载完成！")；
    }
}
```
--------------------------------------------------------------------------------
### 总结
Unity的写成基于IEnumerator，通过yield return 暂停和恢复代码执行。本质上是由Unity主线程调度的一种分时执行机制，适用于实现异步操作或者分帧执行任务（如等待时间、资源加载）。
合理使用协程可以让代码更简洁高效，但需要注意管理协程的生命周期，避免不必要的性能开销。
