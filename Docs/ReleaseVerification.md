# 首次发布与 VPM 链路验证

日期：2026-10-08（本机日期）。用户确认版本：Toolbox 1.1.2 / NeckMaskMaker 0.1.0。

## 发布

- [Toolbox 1.1.2](https://github.com/marble810/MarbleAvatarToolbox/releases/tag/1.1.2)，tag commit `7a34a57`。
- [NeckMaskMaker 0.1.0](https://github.com/marble810/NeckMaskMaker/releases/tag/0.1.0)，tag commit `ccca50d`。
- 均由 Build Release 自动创建 ZIP、unitypackage、package.json；自动 Notify VPM List 成功，未手工伪造 listing 数据。

## 线上结果

- [总列表](https://marble810.github.io/vpmlist/index.json)：Toolbox 1.1.2 / 1.1.1 / 1.1.0 / 1.0.0；NeckMaskMaker 0.1.0。
- [Toolbox 独立列表](https://marble810.github.io/MarbleAvatarToolbox/index.json)：最新 1.1.2，历史版本保留。
- [NeckMaskMaker 独立列表](https://marble810.github.io/NeckMaskMaker/index.json)：0.1.0。

实际下载 ZIP 后计算 SHA256，均与各自独立列表和总列表相同：

| 包/版本 | SHA256 |
|---|---|
| marble810.marbleavatartoolbox 1.1.2 | `34d395bc56ccefd6d658f7b6598e8f8db80ee2fe667334810e2a12531018a81f` |
| marble810.neckmaskmaker 0.1.0 | `7d951cc2952a5223bf66b117513c52a053cfa668cf4ed10a369746928db24896` |

下载地址、manifest 包名/版本、规范 package ZIP 根目录、unitypackage pathname 均已检查。自动化 run 与未完人工验收见 [Validation.md](Validation.md)。

## 首次独立列表模板修正

上游固定读取 Website/index.html 与 app.js，空目录不足以完成首次部署。已将模板作为源码追踪，只忽略生成的 index.json，并在静态 CI 校验模板存在。网站修正提交 `1d510ef`、`34e689b` 不改写已发布 tag 或插件 ZIP。独立列表部署成功 run `37657972551`。
