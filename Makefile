# for travis ci
test:

	cp packages/Newtonsoft.Json.9.0.1/lib/net20/Newtonsoft.Json.dll

	# 2.0
	xbuild csharp-sdk.2.0.sln;