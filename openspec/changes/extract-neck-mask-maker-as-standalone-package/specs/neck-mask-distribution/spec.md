## ADDED Requirements

### Requirement: 独立包身份
系统 MUST 使用 marble810.neckmaskmaker 包 ID 和独立 Editor 程序集，不依赖 MarbleAvatarToolbox、NDMF、MA 或 VRCFury。

#### Scenario: 最小安装
- **WHEN** Unity 2022.3.22f1 项目只安装 VRChat SDK 与 NeckMaskMaker
- **THEN** 窗口、GPU 资源、逐材质槽预览与 PNG 导出可用

### Requirement: 安全迁移本地开发副本
迁移 MUST 保留原资产 GUID，不允许旧实现与新包同时导入；新偏好存在时不得被旧值覆盖。迁移不得改写场景、Avatar 或原材质。

#### Scenario: 旧偏好导入
- **WHEN** 新偏好键缺失且旧键存在
- **THEN** 新键继承旧值，旧键保持原样

#### Scenario: 开发包切换
- **WHEN** 测试工程包含旧实体 Toolbox 副本
- **THEN** 先备份并定向移除旧插件，再链接新包，其他 Toolbox 文件保留

### Requirement: 分离列表与发布
系统 MUST 为新包提供独立发布配置和独立 listing，并向现有总列表追加新来源而不改变现有列表 ID、URL 或 Toolbox 历史版本。

#### Scenario: 用户保留旧订阅
- **WHEN** 总列表成功重建且新包存在已发布版本
- **THEN** 原订阅可同时发现 Toolbox 和 NeckMaskMaker

#### Scenario: 无发布 tag 的源码迁移
- **WHEN** 只推送独立仓库源码而未发布 Release
- **THEN** 不声称新包已可从 VPM 安装，不打未经人工确认的发布 tag
