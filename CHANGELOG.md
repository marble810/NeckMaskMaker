# 更新记录

## [0.2.1] - 2026-10-08
### 调整
- Body / Body_base 界面文案补充（头）/（身体）标注：对象栏、材质槽分栏、贴图预览与提示/错误信息，文件名仍用原始 role。

### 修复
- 开启反转后，Scene 表面红色预览不再跟随反转，始终显示原始 Mask 范围；反转只作用于贴图预览与导出的 PNG。

## [0.2.0] - 2026-10-08
### 新增
- Mask 参数新增“反转”开关：输出贴图（含 Alpha）黑白反转，表面预览、贴图预览与 PNG 导出同步生效；只影响 Pack 阶段，不重算距离与映射。

## [0.1.0] - 2026-10-08
### 新增
- 首个独立试用版本，从 MarbleAvatarToolbox 的迁移前提交 `d60c4a3dd5eac4a1b5d606f41a63dca97f36b389` 提取，保留原资产 GUID。
- Scene 颈部循环线拾取、GPU 表面距离采样、Linear/Smooth/Constant 衰减与外扩。
- 按材质槽独立烘焙、红色表面预览、Scene 贴图预览及 PNG 导出，避免跨槽 UV 串色。
- 中文/日本語/English 三语界面；继承旧 Toolbox 的语言、最近目录和预览偏好，新设置优先。
- 独立 Editor 程序集、包 ID `marble810.neckmaskmaker`、菜单 `Tools/Marble/Neck Mask Maker`，不依赖 Toolbox/NDMF/MA/VRCFury。
- 独立 GitHub Release、VPM listing 与自动通知总列表的 Actions；未发布来源不会使总列表生成失败。

### 使用边界
- 环境为 Unity 2022.3.22f1、VRChat Avatars SDK >=3.10.2，需要支持 Compute/RT 的 GPU，不提供 CPU Mask 回退。
- 已完成合成网格/GPU/材质槽/三语/线宽、真实 Avatar PNG 与域重载自动验证；Scene 视觉、滑块跟手及鼠标面板操作仍需用户确认。
- 开发链接、VPM 与 unitypackage 不得重复安装；不会修改原 Avatar/材质或自动保存场景。
