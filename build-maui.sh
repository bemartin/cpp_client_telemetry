#!/bin/sh

if [ "$1" == "help" ] || [ "$1" == "?" ]; then
    echo
    echo "build-maui.sh usage:"
    echo "./build-maui.sh [debug|release] [cleanall|cleanmaui] [mauionly|skipios|skipandroid] [package]"
    echo
    echo "- debug|release: build configuration to use. Default to release of not specified"
    echo "- cleanall: deletes output and temporary directories for all platforms"
    echo "- cleanmaui: deletes output and temporary directories for the Xamarin solution only"
    echo "- mauionly: only builds the Xamarin solution, assuming that iOS and Android SDKs have already been built"
    echo "- skipios: skip building the SDK for iOS, assuming it has already been built"
    echo "- skipandroid: skip building the SDK for Android, assuming it has already been built"
    echo "- package: packages the Xamarin bindings in a NuGet package"
    echo
    exit 0
fi

if [ "$1" == "release" ] || [ "$1" == "debug" ]; then
    BUILD_CONFIGURATION="$1"
else
    BUILD_CONFIGURATION="release"
fi

if [ "$1" == "cleanmaui" ] || [ "$2" == "cleanmaui" ]; then
    CLEAN_XAMARIN=true
fi

if [ "$1" == "cleanall" ] || [ "$2" == "cleanall" ]; then
    CLEAN_ALL=true
fi

if [ "$1" == "mauionly" ] || [ "$2" == "mauionly" ] ||  [ "$3" == "mauionly" ] ||  [ "$4" == "mauionly" ]; then
    BUILD_XAMARIN_ONLY=true
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

if [ "$CLEAN_XAMARIN" == true ] || [ "$CLEAN_ALL" == true ]; then
    echo "$GREEN ====== Cleaning Xamarin $NOCOLOR"

    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/obj
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/bin
    rm "./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/Native References/libmat.a"

    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/obj
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/bin
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/lib
    rm ./wrappers/maui/sdk/OneDsCppSdk.Android.Bindings/Jars/*.aar

    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.DotNet/obj
    rm -rf ./wrappers/maui/sdk/OneDsCppSdk.DotNet/bin
fi

# Fail on error
set -e

if [ "$BUILD_XAMARIN_ONLY" != true ]; then

    if [ "$SKIP_IOS_BUILD" != true ]; then

    # Build for iOS
    echo "$GREEN ====== Building for iOS $NOCOLOR"
    if [ "$CLEAN_ALL" == true ]; then
        DO_CLEAN="clean"
    else
        DO_CLEAN=""
    fi

    for arch in arm64 arm64e x86_64
    do
        ./build-ios.sh $DO_CLEAN $BUILD_CONFIGURATION $arch
        mv ./out/lib/libmat.a ./out/lib/libmat.$arch.a

        DO_CLEAN=""
    done

    pushd ./out/lib/
    lipo -create -output libmat.a libmat.arm64.a libmat.arm64e.a libmat.x86_64.a
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
rsync -a ./out/lib/libmat.a "./wrappers/maui/sdk/OneDsCppSdk.iOS.Bindings/Native References/"

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

# Build Xamarin Bindings Solution
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