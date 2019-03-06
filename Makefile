build-and-test:
	mkdir -p bin/
	cp tools/net40/Newtonsoft.Json.dll bin/
	cp tools/net40/nunit.framework.dll bin/
	cp tools/files/* bin/


	msbuild src/Qiniu.sln
	ls bin/
	
	nunit-console bin/QiniuTests.exe
