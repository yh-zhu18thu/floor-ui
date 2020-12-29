# MRTK-Quest-Sample

Sample project for setting up with [MRTK-Quest](https://github.com/provencher/MRTK-Quest). Used only for educational purposes.

**As of [MRTK 2.5](https://microsoft.github.io/MixedRealityToolkit-Unity/version/releases/2.5.0/Documentation/ReleaseNotes.html), Oculus platforms are officially supported, leveraging the code that powers MRTK-Quest.**

Some users seem to have issues with getting MRTK and the Oculus integration properly imported into their projects.

As such, this repo serves as an example setup for the official MRTK Oculus integration, using examples from MRTK-Quest. MRTK-Quest 1.2 is not a part of this project.

Further, to make this easier for people who struggle with Git to download, the repository **does not** use submodules, symlinks or git lfs.

Current setup:
- [MRTK](https://github.com/microsoft/MixedRealityToolkit-Unity) V2.5.1
- [Oculus Integration](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022) v20.1
- Unity 2019.4.12f1

Note:
- While developping with this sample, you may have to run this cript below, which will bind the Oculus prefab references to the OculusXRSDKDeviceManagerProfile  ![Install Oculus](https://user-images.githubusercontent.com/7420990/97363151-81ef0700-1878-11eb-9b49-0dc26e120a79.png)

分工安排：

| 负责人 | 任务                                                         | 耦合内容                                                     |
| :----: | ------------------------------------------------------------ | ------------------------------------------------------------ |
| 张潇宇 | 绘制演示demo室内场景                                         | 一些GameObject是可以因地板ui的操控产生变化的，所以这些GameObject的script中要加入相应的信号（相当于对外的接口） |
| 朱翊豪 | 测试唤醒ui的交互模式，尝试视线注视和直接使用camera的rotation两种方式 | 配合谢哥的实际ui设计和卓神的QRCode定位，能够在用户选中时让操控相应的GameObject所需的信号变化 |
| 谢昱清 | 绘制地板ui界面                                               | 适配张神的场景设计，计划好在哪些位置可以显示哪些不同的ui     |
| 张奕卓 | 通过QRCode定位脚在何方                                       | 根据谢哥的界面设计，能够在模块中确定脚处于哪个方位，这个作为一个信号 |

