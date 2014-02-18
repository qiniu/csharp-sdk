# for travis ci
test:

	cp tools/Newtonsoft.Json.dll tools/nunit.framework.dll bin
	#for OS X
	#export MON_PATH="/Library/Frameworks/Mono.framework/Libraries/mono/4.0/"
	#mono --debug /Library/Frameworks/Mono.framework/Versions/3.2.0/lib/mono/4.5/nunit-console.exe bin/Qiniu.Test.dll

	#for Linux
	#2.0||3.5
	xbuild csharp-sdk.2.0.sln;
	cp Qiniu/bin/Debug/2.0/Qiniu.2.0.dll bin/
	cp Qiniu.Test/bin/Debug/2.0/Qiniu.Test.dll bin/Qiniu.2.0.Test.dll
	nunit-console bin/Qiniu.2.0.Test.dll

	#4.0
	xbuild csharp-sdk.4.0.sln;
	cp Qiniu/bin/Debug/4.0/Qiniu.4.0.dll bin/
	cp Qiniu.Test/bin/Debug/4.0/Qiniu.Test.dll bin/Qiniu.4.0.Test.dll
	nunit-console bin/Qiniu.4.0.Test.dll
