<center><h2>AUV上位机使用手册 V1.0</h2></center>

## 功能使用手册

本上位机实现命令控制AUV在水面时的运动过程以及反馈各传感器的状态信息。

此文档实时更新中。。。

---



### 启动程序

![20220216105808](https://gitee.com/tytokongjian/image/raw/master/images/202202161253703.jpg)

在解压文件中找到此 **exe** 即为上位机启动程序。

---



### 控制界面

双击软件启动界面如下：

![20220216103023](https://gitee.com/tytokongjian/image/raw/master/images/202202161253974.jpg)

界面左侧为各功能导航栏。

界面中间为AUV控制界面：

* 各按键功能都已在界面提示中标注。
* 中间红色按钮为手柄连接开关，其下方为手柄控制开关。
* 中心滑动条为运动速度调节。
* 下方左右升降条块为AUV主推速度反馈。
* 下方左中为遥感当前位置图。
* 下方右中为AUV速度档位反馈。

界面右侧为网络和串口登入按钮、当前时间、提示信息栏。

点击**舵板显示按钮**和**提示切换**按钮显示如下：

![20220216103245](https://gitee.com/tytokongjian/image/raw/master/images/202202161253707.jpg)

舵板控制分为左右单独调节和同时调节功能。右侧为信息提示栏。

---



### 登录界面

点击控制界面右上角**网络**按钮打开登录连接界面

![20220216103155](https://gitee.com/tytokongjian/image/raw/master/images/202202161253150.jpg)

若连接AUV成功则红色Logo变绿色，且控制界面**网络**按钮变绿色。

---



### 自主航行

![20220322121431](https://gitee.com/tytokongjian/image/raw/master/images/202203221223774.jpg)

AUV水下调试界面，实现控制AUV水下自主航行功能，包括：定艏、定速、定姿、定高、定深，以及舵板控制。同时包含一键启动和一键制动功能，避免调试时AUV失控或沉底。

---



### 侧扫控制

![20220322121514](https://gitee.com/tytokongjian/image/raw/master/images/202203221223256.jpg)

侧扫声呐远程控制及数据获取界面，实现下位机侧扫声呐传感器的控制以及声呐原始数据获取功能。

---



### 实时地图

![20220216103355](https://gitee.com/tytokongjian/image/raw/master/images/202202161253718.jpg)

此界面需连接互联网使用。显示AUV当前GPS坐标及其所在位置。

---



### 调试界面

![20220216103425](https://gitee.com/tytokongjian/image/raw/master/images/202202161253327.jpg)

实现一个AUV命令调试功能，需跟AUV下位机联调。

---



### 说明界面

![20220216103439](https://gitee.com/tytokongjian/image/raw/master/images/202202161253803.jpg)

完成功能的进度汇总。

---



## 源码流程简介

功能还需完善，正在抢修中。。。。。。