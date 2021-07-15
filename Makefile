.PHONY: build-and-test build test publish

build-and-test: build test
build:
	dotnet build src/Qiniu/Qiniu.csproj
test:
	dotnet test src/QiniuTests/QiniuTests.csproj
publish:
	dotnet publish src/Qiniu/Qiniu.csproj -c Release -f netstandard2.0
