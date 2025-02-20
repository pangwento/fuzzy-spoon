### 什么是Overdraw
Overdraw一帧中对同一个像素被重复绘制的次数。通常发生在多个透明或者半透明物体重叠时，GPU需要多次计算同一个像素的颜色值，导致性能下降。
### 对性能的影响
+ 造成性能瓶颈
  + Overdraw会增加GPU的填充率和计算量，如果填充率或计算量到达GPU的极限，帧率会下降导致卡顿
  + 在移动设备上，GPU资源有限，过绘的影响更加明显
+ 发热和功耗
  + Overdraw会增加GPU的工作负载，导致设备发热和功耗增加
  + 在移动设备上，会缩短电池续航时间
> 填充率：一秒内像帧缓冲区写入像素的数量，通常以百万像素每秒(Megapixels per second)或者十亿像素每秒(Giagpixel per second)来衡量   
> 计算量: 以FLOPS来衡量，每秒可以进行浮点数操作指令数量
### 如何优化Overdraw
+ 减少透明物体的使用
  + 避免使用大量透明或半透明物体
  + 对于UI,减少不必要的重叠
+ 优化渲染顺序
  + 确保不透明物体从前往后渲染，透明物体从后往前渲染
  + 使用渲染队列来控制渲染顺序
+ 降低分辨率
  + 在移动设备上，可以适当降低渲染分辨率，减少填充率需求
### 如何检测Overdraw
创建一个RT, 使用shader叠加颜色方式，进行渲染。读取RT的像素颜色值便能知道像素绘制了几次。
如使用overdraw.shader, 像素阶段对g通道每次渲染增加0.04，在渲染一帧结束后，读取像素g值，g/0.04就是overdraw数量。
