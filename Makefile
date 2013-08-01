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
	cd bin; /Library/Frameworks/Mono.framework/Versions/Current/bin/nunit-console Qiniu.Test.dll
