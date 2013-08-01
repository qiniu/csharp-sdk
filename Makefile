# for travis ci
all:

	xbuild csharp-sdk.sln; cp Qiniu.Test/bin/Debug/Qiniu.Test.dll bin;
	cp Qiniu/bin/Debug/Qiniu.dll bin
	
test:

	export QINIU_ACCESS_KEY="gPhMyVzzbQ_LOjboaVsy7dbCB4JHgyVPonmhT3Dp";
	export QINIU_SECRET_KEY="OjY7IMysXu1erRRuWe7gkaiHcD6-JMJ4hXeRPZ1B";
	export QINIU_TEST_BUCKET="icattlecoder3";
	export QINIU_TEST_DOMAIN="qiniuphotos";
	export MON_PATH="/Library/Frameworks/Mono.framework/Libraries/mono/4.0/"
	mono --debug /Library/FrameworW/Mono.framework/Versions/3.2.0/lib/mono/4.5/nunit-console.exe bin/Qiniu.Test.dll
