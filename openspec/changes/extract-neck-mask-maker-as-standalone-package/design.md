## Context

迁移前提交：d60c4a3dd5eac4a1b5d606f41a63dca97f36b389。原工具为 Assembly-CSharp-Editor 中的 5 个源文件、3 个 GPU 资源，未出现在已发布版本。测试项目 F:/Project_VRC_Local/Test 的 Toolbox 为实体目录，目录 meta 与仓库有差异。vpmlist 存在用户图标 WIP，不纳入本次提交。

## Decisions

- 本地 ../NeckMaskMaker 与远端 marble810/NeckMaskMaker；包 marble810.neckmaskmaker，独立 Editor asmdef 和 marble810.NeckMaskMaker 命名空间。菜单 Tools/Marble/Neck Mask Maker。
- 原 .meta 文件 GUID 保留，新包/asmdef/helper 的 GUID 新建。关闭旧窗口、备份工程中的旧实现，再定向移除副本并链接独立包，禁止两份 GUID 同时被 Unity 导入。
- Compute 通过 GUID 定位、规范 Packages 路径回退，支持 VPM/file:/符号链接及迁移期间定位；不复制 CPU 回退。
- 新偏好键 NeckMaskMaker.* 仅在缺失时导入 MarbleAvatarToolbox.NeckMaskMaker.*，旧键不删除，不覆盖已存在的新值。窗口增加 MovedFrom 标记，换程序集期间通过备份安全关闭重开。
- 必需环境 Unity 2022.3.22f1 与 VRChat SDK。新包不依赖 Toolbox/NDMF/MA/VRCFury。原 Toolbox 依赖和版本不变。
- VPM 总列表 ID/URL 不变，source.json githubRepos 追加独立 Repo；独立 Pages 列表仅输出新包。发布产物和总列表由 workflow 生成，不提交人工修改 index.json。
- 发布骨架使用 ../marble-vpm-template，不搬示例 Runtime。独立 ZIP 包根正确，unitypackage 导出规范 Packages 路径。源码推送无 tag，初始 manifest 0.1.0 为未发布开发版本；人工确认后单独发首版。
- 不复制现有个人 GH 登录凭据到 Actions。跨仓库自动通知需独立最小权限 Token；无法读取已有 Secret 时记录配置待办，不伪造验证结果。
- Toolbox 仅 README 转为独立插件链接，移除该插件源码、测试和初始计划，不创建代理菜单或新依赖。Toolbox checkpoint 与 extraction commit 不自动推送，避免迁移检查点意外发布。

## Validation

先跑合成拓扑/GPU/材质槽/语言/线宽回归，再验证独立程序集、两种安装共存、GUID 唯一、偏好迁移、关闭与重载资源释放。真实模型测试仅当当前窗口目标有效；否则明确记录待用户手测。不修改/保存场景。

## Rollback

项目副本先备份在 Temp/NeckMaskMaker/extraction-backup。回滚须先解除新包链接，再恢复旧实体实现；源代码可从 checkpoint 恢复。已公开版本不重打 tag，后续问题发布修订版本。撤销列表来源不会卸载用户包。
