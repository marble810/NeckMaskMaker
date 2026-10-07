# Neck Mask Maker

VRChat Avatar 颈部遮罩生成工具，从 [MarbleAvatarToolbox](https://github.com/marble810/MarbleAvatarToolbox) 独立拆出。无需安装 Toolbox、NDMF、Modular Avatar 或 VRCFury。

## 环境与安装

- Unity **2022.3.22f1**；VRChat Avatars SDK **3.10.2 或更新版本**。
- 需要支持 Compute Shader 与所需 RenderTexture 格式的编辑器 GPU；不支持时会提示，**没有 CPU 回退**。
- 包 ID：`marble810.neckmaskmaker`；菜单：`Tools/Marble/Neck Mask Maker`。
- VPM 总列表：<https://marble810.github.io/vpmlist/index.json>，保留原订阅即可。
- 独立列表：<https://marble810.github.io/NeckMaskMaker/index.json>。
- **0.1.0 是首个独立试用版本**，下载见 [Release](https://github.com/marble810/NeckMaskMaker/releases/tag/0.1.0)。发布与列表部署完成后可通过 VCC 安装；源码提交本身不会创建新版本。
- 正式版本优先使用 VCC/VPM。ZIP 根目录即包内容；手工安装放入 `Packages/marble810.neckmaskmaker`。`.unitypackage` 也使用相同 Packages 路径，不要与 VPM、file: 或开发链接重复安装。

## 使用

1. 打开菜单，指定 `Body` 与 `Body_base`。
2. 在 Scene 中点击颈部边，Shift 追加循环线，Enter 确认，Esc 取消。
3. 勾选目标材质槽，调整 Max Distance、Linear / Smooth / Constant、Texture Size 与 Dilation。
4. 打开表面预览或 Scene 贴图预览；每槽独立计算，不把跨槽重叠 UV 混合。
5. 保存所选材质槽 Mask，每槽一张 PNG，文件名包含角色、槽号和材质名；Alpha 与 Mask 一致。

界面支持中文 / 日本語 / English。不会替换 Avatar 的原材质或保存场景。参数复用 GPU 缓存，几何变化会重建。

## 开发

在忽略的 `Script/dev.env` 中填写 `UNITYPROJECT = "F:\\Project_VRC_Local\\Test"`，然后用 PowerShell 7 运行 `Script/symlink-to-unity.ps1`。`Script/unlink.ps1` 只解除指向本仓库的符号链接，不删除实体包。

VPM 安装与开发链接互斥；切换前先关闭窗口，备份并清除旧开发实现，不能仅将新包叠加到旧 Toolbox 中。Unity 已打开时，新增链接后还需等待包管理器重新解析并刷新资产；必要时重启编辑器，不能把脚本成功当成编译完成。详细边界见 [迁移说明](Docs/Extraction.md)。

验证：`python Script/validate-package.py`；UnityMCP 回归见包内 `Tests~/README.md`。测试需要真实 Unity GPU，不用静态检查代替功能验收。

## 发布

发布脚本与三个 workflow 来自 `marble-vpm-template`，配置位于 `.template/release.config.json`。

1. 先完成待验收项；运行 `pwsh Script/prepare-release.ps1 patch|minor|major` 生成下一版本说明。
2. 补全文档后再次运行，脚本会 commit、tag、push 并触发 Release。初次 0.1.0 直接使用现有版本清单和对应 CHANGELOG 打同名 tag，后续版本再使用递增脚本。
3. Release 产出 `.zip`、`.unitypackage`、`package.json`；成功后生成独立 listing，并通知总列表。
4. 启用 GitHub Pages 的 Actions 部署。自动跨仓库通知还需配置 `VPMLIST_DISPATCH_TOKEN`：目标 `marble810/vpmlist` 的 Contents:write 最小权限 Token；不要复制个人 gh 登录 Token。
5. 未配置通知 Token 时可在总列表仓库手动运行 `Build Repo Listing`。首个 Release / listing / dispatch 的远端整链测试是首次发布验收，不与源码拆分混为一谈。

源代码版权归原作者；本次迁移未擅自增加开源许可证。
