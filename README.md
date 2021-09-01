# MaterialPointMethodCollections


这个库包含了我所写的有关物质点法(Material Point Method)的一些代码。其中包括

UnityEngine-mpm2d

- 物质点法实现二维流体
- 使用Unity的ComputeShader
- 64x64网格，4096粒子，在我破笔记本上有40多帧
- 无渲染，仅用Graphics.DrawProcedural画了一些立方体
- 原子操作仅支持整数，所以我将网格上的速度以及网格的质量乘上一个很大的数。这个数越大，精度越高，但是不能超过int的范围。
- 视频请看 https://www.bilibili.com/video/bv14b4y1m7Hb
- implement 2d materal point method
- using compute shader of Unity engine
- 64x64 grids,4096 particles,40 fps on my laptop
- no rendering,only draw some cubes with Graphics.DrawProcedural
- atomic operations only support for interger,so i multiply the grid velocity and grid mass with a huge number.

## License

MIT



### 

