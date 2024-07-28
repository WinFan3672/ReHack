all: win64 win32 linux mac
linux:
	dotnet build ReHack -r linux-x64 -o build-linux/
win32:
	dotnet build ReHack -r win-x86 -o build-win32/
win64:
	dotnet build ReHack -r win-x64 -o build-win64/
mac:
	dotnet build ReHack -r osx-arm64 -o build-osx/
clean:
	dotnet clean ReHack
	-rm -r build build-win64
	-rm -r build build-win32
	-rm -r build build-osx
	-rm -r build build-linux
