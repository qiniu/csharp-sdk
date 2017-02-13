#Qiniu (Cloud) C# SDK

##About

This C# SDK is built based on Qiniu Cloud API (see below), and it can be used on .NET Framework 2.0+, .NET Core and UWP (Windows 10 Universal Platforms). It will help you to build an application easily and quickly.

##Documentation

You can have a better view of this SDK by checking the documnets given below:

* [SDK-reference HTML Online](http://oiy037d6a.bkt.clouddn.com/csharp-sdk-ref-v7.2.9/index.html)

* [SDK-reference CHM Offline](http://oiy037d6a.bkt.clouddn.com/QiniuCSharpSDK-Ref-v7.2.9.chm)

Want to know more about how to using this SDK? Are you trying to find some code examples based on this SDK? These documents or links below will be helpful:

* [github/csharp-sdk-shared-examples](https://github.com/fengyhack/csharp-sdk-shared-examples)

* [SDK manual | examples](http://oiy037d6a.bkt.clouddn.com/csharp-sdk-man-v7.2.97/index.html)

##How-to-install

Releases can be found [here](https://github.com/fengyhack/csharp-sdk-shared/releases), choose the one you need and unzipped it, then add reference to the *.dll file in your project.

The latest update will always be available in the `master` branch.

Or you can install using NuGet, take Visual Studio 2013/2015 as an example, navigate to the NuGet package manager, and search `Qiniu.Shared`, or just type the scriptin the package manager console as follow:

```
Install-Package Qiniu.Shared
```

And of course, you can build totally from source.

##How-to-build

Suppose you are using Visual Studio(VS2013 and higher versions are strongly recommended) to build this SDK.

| Target | Solution file |
|--------|--------|
| .NET Framework 2.0 | Qiniu.Net20.sln |
| .NET Framework 3.5 | Qiniu.Net35.sln |
| .NET Framework 4.0 | Qiniu.Net40.sln |
| .NET Framework 4.5 | Qiniu.Net45.sln |
| .NET Framework 4.6 | Qiniu.Net46.sln |
| .NET Core | Qiniu.NetCore.sln |
| Windows10 UWP | Qiniu.UWP.sln |
| All the above | Qiniu.ALL_VER.sln |
| UnitTest(NUnit) | Qiniu.UnitTest.sln |

**Some details about building NetCore/UWP projects**

If you want to build `Qiniu.NetCore` or `Qiniu.UWP`, please copy `project.json` and `project.lock.json` (from `Qiniu.Core` or `Qiniu.UWP` folder) into `Qiniu` folder firstly, or you can just copy `project.json` and then execute `dotnet restore` command (a better way)。

For the other platforms, please delete `project.json` and `project.lock.json` in `Qiniu` folder.

##Notes on .NET Framework

**Higher version targets support async but lower ones doesn't**.

For lower version targets (.NET framework 2.0/3.0/3.5/4.0), HTTP operations is performed with `HttpWebRequest`/`HttpWebResponse` and does not support `async` operations. It should be clear that file/stream read/write functions in these targets do not support async as well.

For higher version targets (.NET Framework 4.5+, .NET Core, and UWP), HTTP operations is performed with `HttpClient` which supports `async` originally. In this case, file/stream read/write operations are implemented with async support.

##API reference

* [Storage API documentation](http://developer.qiniu.com/article/index.html#kodo-api-handbook)

* [Processing API documentation](http://developer.qiniu.com/article/index.html#dora-api-handbook)

* [Fusion CDN API documentation](http://developer.qiniu.com/article/index.html#fusion-api-handbook)

##Related resources

Good ideas and suggestions about our documentation or products are well appreciated. Welcome to visit the websites below and you may leave your words if you like.

* [Forum](http://segmentfault.com/qiniu) - You can share your ideas with each other here, talking abouthow to use Qiniu product or other related topics.

* [Request](http://support.qiniu.com/hc/request/guest/) - If the problems you post on the forum are still unsolved, you can submit it here, tech-support will give you a response as soon as posible.

* [Blog](http://blog.qiniu.com/) - Latest articles about activities and technical sharing can be found here.

* [Weibo](http://weibo.com/qiniutek)

* [FAQs](http://developer.qiniu.com/article/faqs/)

##Contribution

1. Fork

2. Create your own branch `git checkout -b my-new-feature`

3. Commit you changes `git commit -am 'Added some feature'`

4. Push your commit to the remote repo `git push origin my-new-feature`

5. Visit your github page and make a new `pull request` in your branmch `my-new-feature`


##License

Copyright (c) 2017 [qiniu.com](www.qiniu.com)

Published using on MIT license:

www.opensource.org/licenses/MIT

##Appendix

###Quick start on .NET Coree

### How-to-use

Here is a step-by-step guide.

To get ready, if you need `dotnet` tool, see https://github.com/dotnet/cli/

####1. Create

Firstly, change to the working folder, and then create a project:

	dotnet new
	dotnet restore
	
**NOTE** that before `dotnet restore` you may modify the generated `project.json` file as follow:

```json
{
    "version": "1.0.0-*",
	"buildOptions": {
         "emitEntryPoint": true
		},

    "dependencies": {
		"Microsoft.NETCore.App": {
			"version": "1.0.1"
			},
        	"Qiniu": "7.1.0.0",
        	"Newtonsoft.Json": "9.0.1"
    	},

    	"frameworks": {
        	"netcoreapp1.0": {
            	"imports": "dnxcore50"
        		}
    	},

		"runtimes": {
			"win7-x64": {},
			"win7-x86": {},
			"osx.10.10-x64": {},
			"osx.10.11-x64": {},
			"ubuntu.14.04-x64": {},
			"ubuntu.16.04-x64": {}
		}
}
```

Then write your codes.

#### 2. Publish

You may publish to the target os using command like one of them:

```script
dotnet publish -r win7-x64
dotnet publish -r ubuntu.16.04-x64
dotnet publish -r osx.10.11-x64
```

Or if you have .NET Core runtime installed, just build:

```script
dotnet build
```

#### 3. Run

**NOTE that on OSX 10.11(EI Capitan)**, openssl must be installed before running your app:

```script
brew update
brew install openssl
brew link --force openssl
ln -s /usr/local/opt/openssl/lib/libcrypto.1.0.0.dylib /usr/local/lib/
ln -s /usr/local/opt/openssl/lib/libssl.1.0.0.dylib /usr/local/lib/
```

Say, if you would like to run the *built app* `Example.dll`, just type:

```script
dotnet Example.dll
```

Or if you publish on Windows and exe file if generated, just run the exe file.

In this case, you should have `dotnet` tool installed at the very begin.

Or if you want to run the *published app* `Example`, just double-click it. 

######Quick start on .NET Coree

**What is UWP**

UWP means (Windows 10) Universal Windows Platform.

UWP project also  contains a file called `project.json`, here is an example:

```json
{
  "dependencies": {
    "Microsoft.NETCore.UniversalWindowsPlatform": "5.1.0"
  },
  "frameworks": {
    "uap10.0": {}
  },
  "runtimes": {
    "win10-arm": {},
    "win10-arm-aot": {},
    "win10-x86": {},
    "win10-x86-aot": {},
    "win10-x64": {},
    "win10-x64-aot": {}
  }
}
```