# Neck Mask Maker 当前功能规格

## Purpose

为 Unity 2022.3.22f1 中的 VRChat Avatar 生成颈部 Mask。整合原 add/fix/preview/GPU/material-slot/layout/i18n/sampling 多轮 Changes 的最新行为；旧合并导出、CPU 计算、自动填充等已被后续需求替代，不再作为当前契约。

## Requirements

### Requirement: 手工目标与循环线
工具 MUST 允许指定 Body/Body_base，在 Scene 拾取循环线，Shift 追加、Enter 确认、Esc 取消。对象与选择存于 EditorWindow，不创建 Avatar 配置组件。

#### Scenario: 多循环线
- **WHEN** 用户追加互不连接的循环线
- **THEN** 距离计算使用各自线段，不创建跨循环线的伪连接

### Requirement: GPU 表面采样
工具 MUST 在 UV 对应表面位置求值，提供 Linear/Smooth/Constant、Max Distance、Texture Size、Dilation 与输出黑白反转；参数更新复用距离/映射缓存，几何变化重建。工具 MUST NOT 提供生产 CPU Mask 回退。

#### Scenario: 反转输出
- **WHEN** 用户开启反转后查看预览或导出 PNG
- **THEN** 每个槽的输出（含 Alpha）黑白互换，距离、映射与外扩缓存不重算

#### Scenario: GPU 不支持
- **WHEN** 当前设备不支持所需 Compute/RT 格式
- **THEN** 显示原因，预览与保存不可用，不静默改用 CPU

### Requirement: 每材质槽隔离
工具 MUST 为每个勾选有效材质槽独立烘焙、表面红色预览、贴图预览及 PNG 输出，即使不同槽引用相同 Material 也不得合并。原材质与网格不得修改。

#### Scenario: 跨槽 UV 重叠
- **WHEN** 两个槽在相同 UV 区域对应不同距离
- **THEN** 它们使用各自贡献和输出，不跨槽平均

### Requirement: PNG 输出
工具 MUST 为每槽输出一张 PNG，文件名包含角色/槽号/材质名，Alpha 恒等于 Mask。工程 Assets 内导入应采用线性、无压缩、Clamp、不生成 Mipmap 的 Mask 设置。

#### Scenario: 多槽保存
- **WHEN** 用户选择多个有效槽并保存
- **THEN** 输出独立文件且像素对应各槽最终 RT；不覆盖原材质或自动保存场景

### Requirement: 三语与资源生命周期
工具 MUST 提供中文/日本語/English 并记忆语言，新独立偏好仅在缺失时继承旧 Toolbox 值。工具关闭、域重载和无效缓存处理 MUST 释放持有的 GPU/临时网格/材质并取消回调。

#### Scenario: 域重载
- **WHEN** 编辑器脚本编译引发域重载
- **THEN** 序列化目标/循环线/参数保持，运行时资源按需重建，不改写场景
