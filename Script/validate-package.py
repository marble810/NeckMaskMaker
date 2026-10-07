#!/usr/bin/env python3
"""独立包静态边界检查，不替代 Unity GPU/交互回归；仅使用标准库。"""
import json
import re
import unittest
import zipfile
from io import BytesIO
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
PACKAGE = ROOT / "Packages/marble810.neckmaskmaker"


class PackageBoundaryTests(unittest.TestCase):
    def test_manifest(self):
        manifest = json.loads((PACKAGE / "package.json").read_text(encoding="utf-8"))
        self.assertEqual(manifest["name"], "marble810.neckmaskmaker")
        self.assertEqual(manifest["unity"], "2022.3")
        self.assertEqual(manifest["unityRelease"], "22f1")
        self.assertEqual(manifest["dependencies"], {})
        self.assertEqual(manifest["vpmDependencies"], {"com.vrchat.avatars": ">=3.10.2"})

    def test_assembly(self):
        asmdef = json.loads((PACKAGE / "Editor/marble810.neckmaskmaker.editor.asmdef").read_text())
        self.assertEqual(asmdef["name"], "marble810.neckmaskmaker.editor")
        self.assertEqual(asmdef["includePlatforms"], ["Editor"])
        self.assertEqual(set(asmdef["references"]), {"VRC.SDK3A", "VRC.SDKBase"})
        self.assertFalse((PACKAGE / "Runtime").exists())
        for path in (PACKAGE / "Editor").rglob("*.cs"):
            text = path.read_text(encoding="utf-8")
            self.assertIn("namespace marble810.NeckMaskMaker", text)
            self.assertNotIn("using marble810.MarbleAvatarToolbox", text)
            self.assertNotIn("Packages/marble810.marbleavatartoolbox", text)

    def test_guids_and_resources(self):
        guids = []
        for path in PACKAGE.rglob("*.meta"):
            match = re.search(r"^guid: ([0-9a-f]{32})$", path.read_text(), re.M)
            self.assertIsNotNone(match, path)
            guids.append(match[1])
        self.assertEqual(len(guids), len(set(guids)))
        self.assertIn("8afdbd39c9274d0db2af4684d791d51b", guids)
        for pattern in ("*.cs", "*.shader", "*.compute", "*.asmdef"):
            for path in PACKAGE.rglob(pattern):
                if "Tests~" not in path.parts:
                    self.assertTrue(Path(str(path) + ".meta").exists(), path)
        shaders = list((PACKAGE / "Shaders").glob("*.shader"))
        self.assertEqual(len(shaders), 2)
        for shader in shaders:
            self.assertIn('Shader "Hidden/NeckMaskMaker/', shader.read_text())

    def test_release_config(self):
        config = json.loads((ROOT / ".template/release.config.json").read_text())
        self.assertEqual(config["packagePath"], "Packages/marble810.neckmaskmaker")
        self.assertEqual(config["vpmListRepository"], "marble810/vpmlist")
        for path in (ROOT / ".github/workflows").glob("*.yml"):
            self.assertNotIn("Packages/com.example.vpm-package", path.read_text())
        for name in ("symlink-to-unity.ps1", "unlink.ps1"):
            self.assertIn("marble810.neckmaskmaker", (ROOT / "Script" / name).read_text())

    def test_zip_layout(self):
        buffer = BytesIO()
        with zipfile.ZipFile(buffer, "w") as archive:
            for path in PACKAGE.rglob("*"):
                if path.is_file():
                    archive.write(path, path.relative_to(PACKAGE).as_posix())
        with zipfile.ZipFile(buffer) as archive:
            self.assertIsNone(archive.testzip())
            self.assertIn("package.json", archive.namelist())
            self.assertIn("Shaders/NeckMaskBake.compute", archive.namelist())
            self.assertFalse(any(p.startswith("Packages/") for p in archive.namelist()))


if __name__ == "__main__":
    unittest.main(verbosity=2)
