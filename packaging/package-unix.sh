#!/usr/bin/env bash
set -euo pipefail
rid="$1"
version="$2"
source_dir="$3"
output_dir="$4"
mkdir -p "$output_dir"
case "$rid" in
  osx-*)
    staging="$(mktemp -d)"
    trap 'rm -rf "$staging"' EXIT
    bundle="$staging/DigestingDuck.app"
    mkdir -p "$bundle/Contents/MacOS"
    cp -R "$source_dir/." "$bundle/Contents/MacOS/"
    chmod +x "$bundle/Contents/MacOS/DigestingDuck.Desktop"
    cat > "$bundle/Contents/Info.plist" <<PLIST
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0"><dict>
<key>CFBundleExecutable</key><string>DigestingDuck.Desktop</string>
<key>CFBundleIdentifier</key><string>com.jorgebeserra.digestingduck</string>
<key>CFBundleName</key><string>DigestingDuck</string>
<key>CFBundlePackageType</key><string>APPL</string>
<key>CFBundleShortVersionString</key><string>${version%%-*}</string>
<key>CFBundleVersion</key><string>${version%%-*}</string>
<key>NSHighResolutionCapable</key><true/>
</dict></plist>
PLIST
    xattr -r -d com.apple.FinderInfo "$bundle" 2>/dev/null || true
    xattr -r -d com.apple.ResourceFork "$bundle" 2>/dev/null || true
    codesign --force --deep --sign - "$bundle"
    ln -s /Applications "$staging/Applications"
    hdiutil create -volname DigestingDuck -srcfolder "$staging" -ov -format UDZO "$output_dir/DigestingDuck-${version}-${rid}.dmg"
    ;;
  linux-*)
    if [[ "$rid" == "linux-arm64" ]]; then arch=arm64; else arch=amd64; fi
    package_root="$output_dir/deb-$arch"
    mkdir -p "$package_root/DEBIAN" "$package_root/opt/digestingduck" "$package_root/usr/share/applications"
    cp -R "$source_dir/." "$package_root/opt/digestingduck/"
    chmod +x "$package_root/opt/digestingduck/DigestingDuck.Desktop"
    cat > "$package_root/DEBIAN/control" <<CONTROL
Package: digestingduck
Version: ${version/-preview./~preview.}
Architecture: $arch
Maintainer: Jorge Beserra
Depends: libx11-6, libice6, libsm6, libfontconfig1, libicu-dev, libssl3, zlib1g
Description: DigestingDuck desktop client
 Native desktop client for IQ Option, built with Avalonia and .NET.
CONTROL
    cat > "$package_root/usr/share/applications/digestingduck.desktop" <<DESKTOP
[Desktop Entry]
Name=DigestingDuck
Exec=/opt/digestingduck/DigestingDuck.Desktop
Type=Application
Terminal=false
Categories=Office;Finance;
DESKTOP
    dpkg-deb --build --root-owner-group "$package_root" "$output_dir/DigestingDuck-${version}-${rid}.deb"
    tar -czf "$output_dir/DigestingDuck-${version}-${rid}.tar.gz" -C "$source_dir" .
    ;;
  *) echo "Unsupported runtime: $rid" >&2; exit 1;;
esac
