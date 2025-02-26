## 性能问题
+ Canvas Re-batch时间过长
+ Canvas Over-dirty, Re-batch次数过多
+ 生成网格顶点时间过长
+ Fill-rate overutilization
--------------------------------------------------------------------------------
每个Canvas在绘制之前都会合批，如果每一帧UI元素不变，在合批一次之后保留结果，之后的渲染一直用这个结果。
如果UI元素发生变化，需要重新合批，Re-batch   
Canvas Re-batch过程
+ 根据UI元素深度关系进行排序
+ 检查UI元素的覆盖关系
+ 检查UI元素材质并进行合批
--------------------------------------------------------------------------------
Re-build 重新计算layout和渲染网格重建
+ 在WillRenderCanvases事件调用PerformUpdate::CanvasUpdateRegistry接口
  * 通过ICanvasElement.Rebuild方法重新构建Dirty的layout组件
  * 通过ClippingRegistry.Cull方法，任何已经注册的裁剪组件Clipping Component(such as Masks)对象进行裁剪剔除操作
  * 任何Dirty的Graphic components都会被要求重新生成图形元素
+ Layout Rebuild
  * UI元素位置、大小发生变化
  * 优先计算靠近Root节点，并根据层级深度排序
+ Graphic Rebuild
   * 顶点数据被标记Dirty
   * 材质或者贴图数据被标记Dirty

--------------------------------------------------------------------------------
使用canvas的基本准则：
+ 将所有可能打断合批的层移到最下边的图层，尽量避免UI元素出现重叠区域
+ 可以拆封多个同级或者嵌套的canvas来减少Canvas的Rebatch复杂度
+ 拆分动态和静态对象放到不同canvas下，减少rebatch
+ 不适用Layout组件
+ Canvas的RenderMode尽量Overlay模式，减少Camera调用的开销

--------------------------------------------------------------------------------
UI字体
+ 避免字体框重叠，造成合批打断
+ 字体网格重建

--------------------------------------------------------------------------------
UI控件优化注意事项
+ 不需要交互的元素关闭raycaster target
+ 使用sprite的九宫格拉伸，充分减小UI Sprite大小，提高UI Atlas图集利用率
+ 对于不可见的元素，不要使用材质的透明度控制显隐，因为那样UI网格依然在绘制，也不要采用active/deactive控制显隐，因为会有GC和重建开销
