# for travis ci
test:

	cp packages/Newtonsoft.Json.9.0.1/lib/net40/Newtonsoft.Json.dll tools/nunit.framework.dll bin

	# 4.0
	xbuild csharp-sdk.sln;
	nunit-console bin/Qiniu.Test.dll