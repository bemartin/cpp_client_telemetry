#!/bin/sh

if [ "$1" == "help" ] || [ "$1" == "?" ]; then
    echo
    echo "build-maui.sh usage:"
    echo "./build-maui.sh [debug|release] [cleanall|cleanmaui] [mauionly|skipios|skipandroid] [package]"
    echo
    echo "- debug|release: build configuration to use. Default to release of not specified"
    echo "- cleanall: deletes output and temporary directories for all platforms"
    echo "- cleanmaui: deletes output and temporary directories for the MAUI solution only"
    echo "- mauionly: only builds the MAUI solution, assuming that iOS and Android SDKs have already been built"
    echo "- skipios: skip building the SDK for iOS, assuming it has already been built"
    echo "- skipandroid: skip building the SDK for Android, assuming it has already been built"
    echo "- package: packages the MAUI bindings in a NuGet package"
    echo
    exit 0
fi

if [ "$1" == "release" ] || [ "$1" == "debug" ]; then
    BUILD_CONFIGURATION="$1"
else
    BUILD_CONFIGURATION="release"
fi

if [ "$1" == "cleanmaui" ] || [ "$2" == "cleanmaui" ]; then
    CLEAN_MAUI=true
fi

if [ "$1" == "cleanall" ] || [ "$2" == "cleanall" ]; then
    CLEAN_ALL=true
fi

if [ "$1" == "mauionly" ] || [ "$2" == "mauionly" ] ||  [ "$3" == "mauionly" ] ||  [ "$4" == "mauionly" ]; then
    BUILD_MAUI_ONLY=true
fi

if [ "$1" == "skipios" ] || [ "$2" == "skipios" ] ||  [ "$3" == "skipios" ] ||  [ "$4" == "skipios" ]; then
    SKIP_IOS_BUILD=true
fi

if [ "$1" == "skipandroid" ] || [ "$2" == "skipandroid" ] ||  [ "$3" == "skipandroid" ] ||  [ "$4" == "skipandroid" ]; then
    SKIP_ANDROID_BUILD=true
fi

if [ "$1" == "package" ] || [ "$2" == "package" ] || [ "$3" == "package" ] || [ "$4" == "package" ]; then
    PACKAGE=true
fi

GREEN="\033[1;32m"
RED="\033[1;31m"
NOCOLOR="\033[0m"

if [ "$BUILD_CONFIGURATION" == "debug" ] && [ "$PACKAGE" == true ]; then
    echo "$RED ====== Cannot package in debug configuration $NOCOLOR"
    exit 1
fi

echo "$GREEN ====== Build configuration = $BUILD_CONFIGURATION $NOCOLOR"

# Clean

if [ "$CLEAN_MAUI" == true ] || [ "$CLEAN_ALL" == true ]; then
    echo "$GREEN ====== Cleaning MAUI $NOCOLOR"

    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/obj
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/bin
    rm -rf "./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/Native References/libmat.xcframework"
    rm -f "./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/Native References/libmat.a"

    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/obj
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/bin
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib
    rm ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/Jars/*.aar

    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.DotNet/obj
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.DotNet/bin
fi

# Fail on error
set -e

if [ "$BUILD_MAUI_ONLY" != true ]; then

    if [ "$SKIP_IOS_BUILD" != true ]; then

    # Build for iOS
    echo "$GREEN ====== Building for iOS $NOCOLOR"
    if [ "$CLEAN_ALL" == true ]; then
        DO_CLEAN="clean"
    else
        DO_CLEAN=""
    fi

    # Set CMAKE_OPTS to enable sanitizer module
    export CMAKE_OPTS="-DBUILD_SANITIZER=YES"

    # Build for iOS device (iphoneos)
    for arch in arm64 arm64e
    do
        ./build-ios.sh $DO_CLEAN $BUILD_CONFIGURATION $arch iphoneos
        mv ./out/lib/libmat.a ./out/lib/libmat.iphoneos.$arch.a
        DO_CLEAN=""
    done

    # Build for iOS simulator (iphonesimulator)
    for arch in arm64 x86_64
    do
        ./build-ios.sh $BUILD_CONFIGURATION $arch iphonesimulator
        mv ./out/lib/libmat.a ./out/lib/libmat.iphonesimulator.$arch.a
    done

    # Unset CMAKE_OPTS after iOS builds
    unset CMAKE_OPTS

    pushd ./out/lib/

    # Create fat binaries for each platform
    echo "$GREEN ====== Creating fat binary for iphoneos $NOCOLOR"
    lipo -create -output libmat.iphoneos.a libmat.iphoneos.arm64.a libmat.iphoneos.arm64e.a

    echo "$GREEN ====== Creating fat binary for iphonesimulator $NOCOLOR"
    lipo -create -output libmat.iphonesimulator.a libmat.iphonesimulator.arm64.a libmat.iphonesimulator.x86_64.a

    # Create framework structures for each platform
    echo "$GREEN ====== Creating framework for iphoneos $NOCOLOR"
    mkdir -p iphoneos/libmat.framework/Headers
    cp -R ../../lib/include/public/* iphoneos/libmat.framework/Headers/
    cp libmat.iphoneos.a iphoneos/libmat.framework/libmat

    echo "$GREEN ====== Creating framework for iphonesimulator $NOCOLOR"
    mkdir -p iphonesimulator/libmat.framework/Headers
    cp -R ../../lib/include/public/* iphonesimulator/libmat.framework/Headers/
    cp libmat.iphonesimulator.a iphonesimulator/libmat.framework/libmat

    # Create XCFramework that supports both device and simulator
    echo "$GREEN ====== Creating XCFramework $NOCOLOR"
    xcodebuild -create-xcframework \
      -framework iphoneos/libmat.framework \
      -framework iphonesimulator/libmat.framework \
      -output libmat.xcframework

    popd

    fi

    if [ "$SKIP_ANDROID_BUILD" != true ]; then

    # Build for Android
    echo "$GREEN ====== Building for Android $NOCOLOR"
    pushd ./lib/android_build
    if [ "$CLEAN_ALL" == true ]; then
        gradle :maesdk:clean
    fi

    if [ "$BUILD_CONFIGURATION" == "debug" ]; then
        gradle :maesdk:assembleDebug --no-daemon
    else
        gradle :maesdk:assembleRelease --no-daemon
    fi

    # Generate javadocs
    pushd ./maesdk/src/main/java
    echo "$GREEN ====== Generating JavaDoc $NOCOLOR"
    javadoc -protected -d ../../../../../../wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/JavaDoc -Xdoclint:none -classpath "../../../build/intermediates/javac/release/classes:$ANDROID_HOME/platforms/android-34/android.jar" com.microsoft.applications.events 2>/dev/null || echo "Warning: JavaDoc generation failed, continuing..."
    popd
    popd
fi
fi

echo "$GREEN ====== Copying build artifacts $NOCOLOR"

# Copy artifacts for iOS
rsync -a ./out/lib/libmat.xcframework "./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/Native References/"

# Copy artifacts for Android
mkdir -p ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/arm64-v8a
mkdir -p ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/armeabi-v7a
mkdir -p ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/x86
mkdir -p ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/x86_64
mkdir -p ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/Jars
rsync -a ./lib/android_build/maesdk/build/intermediates/merged_native_libs/$BUILD_CONFIGURATION/mergeReleaseNativeLibs/out/lib/arm64-v8a/*.so ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/arm64-v8a/
rsync -a ./lib/android_build/maesdk/build/intermediates/merged_native_libs/$BUILD_CONFIGURATION/mergeReleaseNativeLibs/out/lib/armeabi-v7a/*.so ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/armeabi-v7a/
rsync -a ./lib/android_build/maesdk/build/intermediates/merged_native_libs/$BUILD_CONFIGURATION/mergeReleaseNativeLibs/out/lib/x86/*.so ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/x86/
rsync -a ./lib/android_build/maesdk/build/intermediates/merged_native_libs/$BUILD_CONFIGURATION/mergeReleaseNativeLibs/out/lib/x86_64/*.so ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib/x86_64/
rsync -a ./lib/android_build/maesdk/build/outputs/aar/maesdk-$BUILD_CONFIGURATION.aar ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/Jars/

# Build MAUI Bindings Solution
pushd ./wrappers/maui

if [ "$SKIP_IOS_BUILD" = true ]; then
    # Build only Android and DotNet projects when skipping iOS
    echo "$GREEN ====== Restoring NUGET packages (Android and DotNet only) $NOCOLOR"
    dotnet restore ./sdk/OneDsCppSdk.Android.Bindings/OneDsCppSdk.Android.Bindings.csproj /p:Configuration=$BUILD_CONFIGURATION
    dotnet restore ./sdk/OneDsCppSdk.DotNet.Bindings/OneDsCppSdk.DotNet.Bindings.csproj /p:Configuration=$BUILD_CONFIGURATION
    echo "$GREEN ====== Building MAUI bindings (Android and DotNet only) $NOCOLOR"
    dotnet build ./sdk/OneDsCppSdk.Android.Bindings/OneDsCppSdk.Android.Bindings.csproj /p:Configuration=$BUILD_CONFIGURATION -v:normal
    dotnet build ./sdk/OneDsCppSdk.DotNet.Bindings/OneDsCppSdk.DotNet.Bindings.csproj /p:Configuration=$BUILD_CONFIGURATION -v:normal
else
    echo "$GREEN ====== Restoring NUGET packages $NOCOLOR"
    dotnet restore ./Microsoft.Applications.Events.sln /p:Configuration=$BUILD_CONFIGURATION
    echo "$GREEN ====== Building MAUI bindings $NOCOLOR"
    dotnet build ./Microsoft.Applications.Events.sln /p:Configuration=$BUILD_CONFIGURATION -v:normal
fi

if [ "$PACKAGE" == true ]; then
    echo "$GREEN ====== Creating NuGet package $NOCOLOR"

    echo "Getting Version number from git Tags"
    VERSION=$(git describe --tags --match="v*.*.*" | sed  's/^v\([0-9]*\(\.[0-9]*\)*\).*$/\1/g')
    echo "Version: $VERSION"

    nuget pack ./Microsoft.Applications.Events.nuspec # -Version "$VERSION"
fi

popd