build-and-test:
	mkdir -p bin/
	cp tools/net40/Newtonsoft.Json.dll bin/
	cp tools/net40/nunit.framework.dll bin/
	cp tools/files/* bin/
<<<<<<< HEAD
	
	msbuild src/Qiniu.sln
=======
        msbuild src/Qiniu.sln
>>>>>>> f354e68f73bc7383528e9a5ea67f760e2f458492
	nunit-console bin/Qiniu.dll
