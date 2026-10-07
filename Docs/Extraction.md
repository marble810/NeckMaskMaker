# Toolbox 分离记录

## 来源与范围

源仓库 `marble810/MarbleAvatarToolbox`，检查点 `d60c4a3dd5eac4a1b5d606f41a63dca97f36b389`。迁移 5 个功能 C#、3 个 Shader/Compute 及 .meta、全部 NeckMask 回归、初始计划；补充独立程序集、偏好兼容和包分发配置。未重写原仓库历史，不继承原仓库版本/tag。

旧包/命名空间 `marble810.marbleavatartoolbox` / `marble810.MarbleAvatarToolbox.NeckMaskMaker`；新包/命名空间 `marble810.neckmaskmaker` / `marble810.NeckMaskMaker`。旧正式版本没有该功能，不需要给其新加强制依赖。

## 本地开发迁移

测试项目 `F:/Project_VRC_Local/Test` 原 Toolbox 为实体副本。先关闭旧窗口，将旧源码、测试、Shader 与目录 meta 备份到 `Temp/NeckMaskMaker/extraction-backup`；只移除相关路径，保留其余 Toolbox 内容与 VPM 清单。随后接入独立包链接并刷新 Unity。

原文件 GUID 保留；目录 meta 来源差异单独备份，新包以源仓库 meta 为准。两份实现不能同时装入 Unity，改 namespace 也不能消除 GUID 冲突。旧窗口安全关闭重开，增加 MovedFrom 信息但不承诺不同编辑器布局的一切内部状态无损转移；窗口快照保存在备份中，场景没有保存或改写。

语言、最近目录和预览尺寸/坐标会迁移到 `NeckMaskMaker.*`：只写缺失的新键，保留旧键，新值优先。原 PNG、Avatar、场景和材质不受影响。Compute 优先 GUID 定位，再回退规范包路径。

开发链接和正式 VPM 不能并存。不要手工伪造 `vpm-manifest.json` 的 locked 记录；正式安装交给 VCC/VPM 解析。UPM file: 仅用于开发，不会自动安装 vpmDependencies，需要事先安装 SDK。脚本拒绝覆盖实体目录或其他仓库的链接。

## 分发

总列表保持 `com.marble810.vpmlist` 与 `https://marble810.github.io/vpmlist/index.json`，source.json 追加新 Repo，不删除 Toolbox 来源/版本。新独立 listing 只列新包。同一包版本在两个列表必须使用相同 Release ZIP 与完整元数据，不能人工编辑生成 JSON。

无 Release 时列表中没有新包，这是预期状态，不代表迁移失败。GitHub Pages、首次 Release、下载哈希及 VCC 安装需按发布验收逐项完成。自动通知 Secret 必须另行配置，已有仓库的 Secret 无法读回/复制。

## 回滚

1. 关闭独立窗口，解除新包链接。
2. 从 Temp 备份恢复工程旧 NeckMask 路径与原 meta，再刷新 Unity。
3. 源文件可以从 Toolbox 检查点恢复；不要 hard reset 覆盖其他工作。
4. source.json 可单独撤销新 Repo。此操作不卸载用户包；已发布 tag/产物不得覆盖重打，使用新修订版本。

## 待用户/首次发布验收

旧历史 Changes 尚留有 Scene 拖动视觉、面板实际鼠标输入、滑块跟手和真实模型端到端输出的人工确认；它们不因拆分而自动变成通过。当前测试模型或窗口目标无效时，真实模型脚本不能当成已运行。具体自动验证结果见 `Validation.md`。
