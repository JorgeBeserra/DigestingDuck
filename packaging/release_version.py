"""Resolve release intent without publishing anything (safe on pull requests)."""
import os
import re
import subprocess
import xml.etree.ElementTree as ET
from pathlib import Path

PROJECT = "src/DigestingDuck.Desktop/DigestingDuck.Desktop.csproj"

def version(xml):
    value = ET.fromstring(xml).findtext(".//Version", "").strip()
    if not re.fullmatch(r"(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(?:-[0-9A-Za-z]+(?:[.-][0-9A-Za-z]+)*)?", value):
        raise ValueError("Defina uma versão explícita válida no projeto desktop.")
    return value

def should_release(event, ref, current, previous):
    if ref.startswith("refs/tags/"):
        if ref != "refs/tags/v" + current:
            raise ValueError("A tag deve corresponder à versão do projeto.")
        return event == "push"
    if ref != "refs/heads/master":
        return False
    return event == "workflow_dispatch" or (event == "push" and current != previous)

if __name__ == "__main__":
    current = version(Path(PROJECT).read_text())
    previous = None
    before = os.environ.get("BEFORE", "")
    if before and set(before) != {"0"}:
        old = subprocess.run(["git", "show", f"{before}:{PROJECT}"], capture_output=True, text=True)
        if old.returncode == 0:
            previous = version(old.stdout)
    publish = should_release(os.environ.get("EVENT", ""), os.environ.get("GITHUB_REF", ""), current, previous)
    with open(os.environ["GITHUB_OUTPUT"], "a") as output:
        output.write(f"version={current}\nrelease={str(publish).lower()}\n")
    print(f"Versão: {current}; publicar release: {publish}")
