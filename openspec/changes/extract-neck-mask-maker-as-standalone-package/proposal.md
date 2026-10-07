## Why

NeckMaskMaker 需要独立安装、版本与发布，不再与 Toolbox 生命周期绑定。用户已确认方案，先将原工作区提交为 d60c4a3，再拆分，最后提交 Toolbox 的移除记录。

## What Changes

- 新增独立仓库 F:/Git/NeckMaskMaker、GitHub marble810/NeckMaskMaker 与 Editor-only 包 marble810.neckmaskmaker。
- **BREAKING**：开发版 Toolbox 不再包含 NeckMaskMaker，菜单改为 Tools/Marble/Neck Mask Maker。正式发布过的 Toolbox 版本未包含该功能。
- 独立程序集、命名空间、资源定位与偏好键；保留原资产 GUID 和算法。
- 现有 VPM 总列表追加新来源；列表 ID/URL 与 Toolbox 历史版本不变。

## Capabilities

### New Capabilities
- `neck-mask-distribution`：独立安装、包身份、发布与列表分发契约。

### Modified Capabilities
无。openspec/specs 尚无归档规格；原 NeckMask Changes 是历史开发记录，独立仓库整合最新功能规格。

## Impact

涉及两个源码仓库、vpmlist/source.json、Unity 测试工程实体包副本与链接。无需修改 Avatar/场景/原材质/输出 PNG，无需继承 NDMF/Toolbox 依赖。GitHub 推送不打发布 tag，待人工验收后另行发布。
