build-and-test:
	mkdir bin/
	cp tools/net40/Newtonsoft.Json.dll bin/
	cp tools/net40/nunit.framework.dll bin/
	cp tools/files/* bin/


	xbuild src/Qiniu.sln

	nunit-console bin/QiniuTests.dll
