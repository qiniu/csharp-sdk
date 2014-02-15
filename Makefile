# for travis ci
test:

	cp tools/Newtonsoft.Json.dll tools/nunit.framework.dll bin
	#for OS X
	#export MON_PATH="/Library/Frameworks/Mono.framework/Libraries/mono/4.0/"
	#mono --debug /Library/Frameworks/Mono.framework/Versions/3.2.0/lib/mono/4.5/nunit-console.exe bin/Qiniu.Test.dll

	#for Linux
	#3.5
	xbuild csharp-sdk-3.5.sln;
	cp Qiniu/bin/Debug/3.5/Qiniu.dll bin
	cp Qiniu.Test/bin/Debug/Qiniu.Test.dll bin
	nunit-console -framework="3.5" bin/Qiniu.Test.dll

	#4.0
	xbuild csharp-sdk-4.0.sln;
	cp Qiniu/bin/Debug/4.0/Qiniu.dll bin
	cp Qiniu.Test/bin/Debug/Qiniu.Test.dll bin
	nunit-console -framework="4.0" bin/Qiniu.Test.dll
