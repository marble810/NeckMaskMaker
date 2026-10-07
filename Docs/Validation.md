# 分离验证

## 已通过

环境：Unity 2022.3.22f1，测试项目 `F:/Project_VRC_Local/Test`，Kirisame 模型。全部通过 UnityMCP 实际运行，不是仅查源码。

- 静态包边界 5 项：manifest、Editor asmdef、GUID 唯一/资源完整、发布配置、ZIP 根目录布局。
- PowerShell 3 个脚本语法解析通过；链接到独立包成功。
- `NeckMaskDistributionRegression`：独立程序集/菜单、无 Toolbox/NDMF/MA/VRCFury 程序集依赖、原 Compute GUID、新 Shader 名称、5 个偏好键继承、新值优先、旧键保留。
- `NeckMaskLocalizationRegression`：77 条三语文案、缺键回退、格式化、记忆与源码文案约束。
- `NeckMaskMakerRegression`：三环圆柱、拆点接缝、四价扇面、非流形边、Shader。
- `NeckMaskGpuRegression`：三模式、参数边界、重叠 UV、缓存、零值覆盖、八轮外扩参考、Alpha。
- `NeckMaskMaterialSlotRegression`：跨槽隔离、同材质不合并、独立预览/RT/PNG、序列化、无效槽/批量失败、原资源不变。
- `NeckMaskEdgeGpuRegression`：透视/正交、两种深度、三个角度共 12 组；最大中心误差 0.5 像素。
- `NeckMaskMakerRealAvatarRegression`：31 顶点闭合颈部循环，高度跨度 0.02526581 米。
- `NeckMaskMaterialSlotRealAvatarRegression`：真实模型三个槽输出 PNG 到项目 Temp，逐像素匹配 RT，预览子网格索引正确，原材质保持。
- 真实脚本编译/域重载前后窗口 JSON 完全相同；关闭处理释放 GPU/材质，再启用正常。
- 测试期间 Scene 始终 `isDirty=false`；工程旧 Toolbox 的其他 **114 个文件哈希未变**。当前新包与正式 Toolbox 1.1.1 共存。

## 导入边界

新增嵌入式符号链接后，第一次 AssetDatabase 刷新尚未导入新程序集；包管理器已注册包，但资产/编译图仍未更新。请求 PackageManager Resolve、递归导入新包并刷新后，程序集正常加载，全部回归通过。没有用代码改动掩盖此安装时序问题。

开发脚本创建链接不代表已完成 Unity 导入；Unity 已打开时需等待包重新解析/刷新，必要时重启编辑器。文件清单、目录存在或工具调用成功本身不算编译验收。

## 远端与未发布来源边界

独立 Repo 已创建并推送，`Validate Package` 远端 CI 通过；GitHub Pages 已配置 Actions 部署。

首次把无 Release 的新仓库直接交给 package-list-action 时，总列表生成器报 `AddRange(null)`。新增生成前来源准备与 4 项 Node 回归：无完整包 Release 时只在 CI 工作副本中暂时跳过新来源；发布 package.json 和 ZIP 后自动纳入。API/授权错误必须失败，不静默剔除旧来源。仓库内 source.json 保留两个来源，原总列表 ID/URL/Toolbox 历史版本不变。

独立 listing 同样检查完整包 Release；无版本时成功跳过部署，不发布伪造的包版本。首次失败未部署、未覆盖既有线上 index.json。

修正后总列表 run `37652980252` 完成生成与 Pages 部署；下载部署 artifact 验证原列表 ID/URL、Toolbox 的 1.0.0/1.1.0/1.1.1 全部保持，未发布新包未被伪造进版本字典。独立 listing run `37653107171` 成功按预期跳过无 Release 的部署，最新静态 CI run `37653101991` 成功。

## 首次发布验证（2026-10-08）

用户确认发布 Toolbox 1.1.2 与 NeckMaskMaker 0.1.0，并在新仓库配置 `VPMLIST_DISPATCH_TOKEN` 和 `PACKAGE_NAME`。两个 tag、Release 的 ZIP/unitypackage/package.json 均已生成，两个通知 Actions 成功触发总列表更新。未读取或复制个人 Token。

- Toolbox Build Release run `37657087590`、独立列表 `37657128272`、通知 `37657127893` 成功。
- NeckMaskMaker Build Release run `37657130553`、通知 `37657247806` 成功。
- 首次独立列表发现缺少上游固定读取的 Website/index.html 和 app.js；补齐模板并新增静态回归，修正后独立列表 run `37657972551` 和静态 CI `37657966746` 成功。源码后续修正仅影响网站/CI，没有改写 0.1.0 tag 或发布 ZIP。
- 总列表 repository_dispatch run `37657273198` 成功部署；线上总列表现在同时包含 Toolbox 1.1.2 和 NeckMaskMaker 0.1.0，Toolbox 历史版本完整保留。
- 实际下载两个 Release 的 ZIP，包根清单与独立 package.json 一致，ZIP 完整性通过；总列表与两个独立列表的下载地址、SHA256 和包身份一致。
- 解读两个 unitypackage 的 pathname，均位于规范 Packages/<package-id>/ 下；Toolbox ZIP 不含 NeckMaskMaker，新包含 Compute/Shader/Editor asmdef。
- 独立网站已渲染 0.1.0，VCC 添加入口和脚本存在，无未渲染模板占位符。

## 后续仍需手动验收

- VCC 全新安装/升级/卸载/重装与 `.unitypackage` 的真实 Unity 导入。本次没有替换测试工程开发链接或改写 VPM 锁文件。
- 人工确认 Scene 视觉、滑块跟手、实际鼠标输入拦截与关闭按钮；自动 GPU/导出回归不能替代视觉确认。
- 最小依赖编译由独立 asmdef 的引用隔离得到支持；没有宣称已另建 SDK-only 空项目或验证所有 GPU 平台。

原始本机测试结果在忽略的 `Docs/local/auto-regressions.json`、`real-avatar-regressions.json`；项目旧代码/窗口快照在 `Temp/NeckMaskMaker/extraction-backup`。
