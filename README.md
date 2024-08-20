# 1 快速入门

## 1.1 介绍
`Wing`致力于打造一个功能强大、最接地气的`.NET`微服务框架，支持`.NET6+`运行平台。支持`Consul`服务注册与发现，服务间通讯支持`http`和`grpc`调用，内置负载均衡器。支持`服务策略`配置，服务异常降级处理。支持
`Saga分布式事务`，支持三种恢复策略：向前恢复、向后恢复、先前再后。自带`配置中心`，服务配置可以在线集中统一管理。支持http/grpc/sql链路追踪`APM`及耗时分析统计。内置`服务网关`，支持全局服务策略和个性化服务策略配置。支持`RabbitMQ`事件总线，自带人性化的`Dashboard`管理界面。

### 文档地址
gitee：https://linguicheng.gitee.io/Wing （暂停访问，请使用github地址）

github：https://linguicheng.github.io/Wing

### 依赖
需要用到的第三方中间件：`Consul`(强依赖)、`RabbitMQ`(弱依赖)、`数据库`(强依赖)

### 技术交流群
`QQ`:`183015352`

## 1.2 服务注册

### 什么是服务注册?

`服务注册`是指服务启动后将该服务的IP、端口等信息注册到`Consul`。

### 创建一个Web Api项目

* 提前准备：安装并启动Consul

* 打开 Visual Studio 2022 并创建Web Api项目([点击查看完整示例代码1.2](https://gitee.com/linguicheng/wing-demo/tree/master/1.2))

### 安装依赖包

```
dotnet add package Wing.Consul
```

### Program代码

```cs
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddWing();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### 添加配置

```json
{
  // 是否启用配置中心，默认启用
  "ConfigCenterEnabled": false,
  "Consul": {
    "Url": "http://localhost:8500",
    "Service": {
      //Http  Grpc
      "Option": "Http",
      "HealthCheck": {
        "Url": "http://localhost:1210/health",
        //单位：秒
        "Timeout": 10,
        //单位：秒
        "Interval": 10
      },
      "Name": "Wing.Demo_1.2.1",
      "Host": "localhost",
      "Port": 1210,
      "Tag": "",
      "LoadBalancer": {
        //RoundRobin  WeightRoundRobin LeastConnection
        "Option": "WeightRoundRobin",
        //权重
        "Weight": 60
      },
      "Scheme": "http",
      "Developer": "linguicheng"
    },
    //定时同步数据时间间隔，单位：秒 小于等于0表示立即响应
    "Interval": 10,
    //数据中心
    "DataCenter": "dc1",
    //等待时间,单位：分钟
    "WaitTime": 3
  }
}
```

### 查看运行效果

* 程序运行后，打开consul UI管理界面，可以看到注册服务`Wing.Demo_1.2`，如下图：

![输入图片说明](samples/Doc/quick-start/1.2-2.png)

## 1.3 启动UI

### 简介

`Wing.UI`是`Wing`微服务框架中的一个可视化操作管理系统，主要功能有服务治理、配置中心、APM管理、Saga分布式事务查询。

### 创建一个Web Api项目

* 提前准备：安装并启动Consul

* 打开 Visual Studio 2022 并创建Web Api项目([点击查看完整示例代码1.3](https://gitee.com/linguicheng/wing-demo/tree/master/1.3))

### 安装依赖包

安装服务注册nuget包`Wing.Consul`，UI可视化界面管理nuget包`Wing.UI`，选择对应的数据库驱动（[参考FreeSql官网](https://freesql.net/guide/#%E5%AE%89%E8%A3%85%E5%8C%85)），以SqlServer为例，安装`FreeSql.Provider.SqlServer`。

```bash
dotnet add package Wing.Consul

dotnet add package Wing.UI

dotnet add package FreeSql.Provider.SqlServer
```

### Program代码

```cs
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddWing().AddWingUI(FreeSql.DataType.SqlServer);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### 添加配置

```json
{
  // 是否启用配置中心，默认启用
  "ConfigCenterEnabled": false,
  "Consul": {
    "Url": "http://localhost:8500",
    //定时同步数据时间间隔，单位：秒 小于等于0表示立即响应
    "Interval": 10,
    //数据中心
    "DataCenter": "dc1",
    //等待时间,单位：分钟
    "WaitTime": 3
  },
  // 数据库链接
  "ConnectionStrings": {
    "Wing": "Data Source=192.168.56.96;User Id=sa;Password=wing123.;Initial Catalog=Wing;TrustServerCertificate=true;Pooling=true;Min Pool Size=1"
  },
  //自动同步实体结构到数据库
  "UseAutoSyncStructure": true,
  // 首页
  "Home": {
    // 指标耗时异常统计 单位：毫秒  默认60秒
    "Timeout": {
      "Gateway": 2000,
      "Apm": {
        "Http": 2000,
        "WorkServiceHttp": 2000,
        "WorkServiceSql": 2000
      },
      // 查询统计时间 默认：最近一个月
      "SearchTime": "2020-01-01"
    }
  },
}
```
### 查看运行效果

* 程序运行后，浏览器访问 http://localhost:1310/wing ，运行效果如下图：

![输入图片说明](samples/Doc/quick-start/1.3-3.png)

* 可以看到示例 [1.2](register.md) 注入的服务`Wing.Demo_1.2`

![输入图片说明](samples/Doc/quick-start/1.3-4.png)

## 1.4 服务发现与调用

### 什么是服务发现?

`服务发现`是指服务启动后将服务注册信息定时同步刷新到本地或实时获取`Consul`的服务信息。

### 创建一个Grpc Service项目

* 提前准备：安装并启动Consul

* 打开 Visual Studio 2022 并创建Grpc Service项目([点击查看完整示例代码1.4](https://gitee.com/linguicheng/wing-demo/tree/master/1.4))

### 安装依赖包

```bash
dotnet add package Wing.Consul
```

### Grpc健康检查

protobuf文件
```protobuf
syntax = "proto3";
package grpc.health.v1;

message HealthCheckRequest {
	string service = 1;
}

message HealthCheckResponse {
	enum ServingStatus {
		UNKNOWN = 0;
		SERVING = 1;
		NOT_SERVING = 2;
	}
	ServingStatus status = 1;
}

service Health {
	rpc Check(HealthCheckRequest) returns (HealthCheckResponse);

	rpc Watch(HealthCheckRequest) returns (stream HealthCheckResponse);
}
```
HealthCheck代码

```cs
public class HealthCheck : Health.HealthBase
{
    public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving });
    }

    public override async Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
    {
        await responseStream.WriteAsync(new HealthCheckResponse()
        { Status = HealthCheckResponse.Types.ServingStatus.Serving });
    }
}
```

### Program代码

```cs
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

// Add services to the container.

builder.Services.AddGrpc();

builder.Services.AddWing();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
```

### 添加配置

```json
{
  // 是否启用配置中心，默认启用
  "ConfigCenterEnabled": false,
  "Consul": {
    "Url": "http://localhost:8500",
    "Service": {
      //Http  Grpc
      "Option": "Grpc",
      "HealthCheck": {
        "Url": "localhost:1410",
        //单位：秒
        "Timeout": 10,
        //单位：秒
        "Interval": 10,
        "GRPCUseTLS": false
      },
      "Name": "Wing.Demo_1.4",
      "Host": "localhost",
      "Port": 1410,
      "Tag": "",
      "LoadBalancer": {
        //RoundRobin  WeightRoundRobin LeastConnection
        "Option": "WeightRoundRobin",
        //权重
        "Weight": 4
      },
      "Scheme": "http",
      "Developer": "linguicheng"
    },
    //定时同步数据时间间隔，单位：秒 小于等于0表示立即响应
    "Interval": 10,
    //数据中心
    "DataCenter": "dc1",
    //等待时间,单位：分钟
    "WaitTime": 3
  }
}
```
### 查看运行效果

* 运行当前程序并启动示例 [1.3](ui.md)，浏览器访问 http://localhost:1310/wing ，可以看到注入的Grpc服务`Wing.Demo_1.4`，运行效果如下图：

![输入图片说明](samples/Doc/quick-start/1.4-1.png)

* 在示例 [1.2](register.md) 中调用当前Grpc服务中`SayHello`方法，代码如下：

```cs
[HttpGet("hello")]
public Task<string> SayHello()
{
    return _serviceFactory.InvokeAsync("Wing.Demo_1.4", async serviceAddr =>
    {
        var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
        var greeterClient = new Greeter.GreeterClient(channel);
        var result = await greeterClient.SayHelloAsync(new HelloRequest { Name = "Wing" });
        return result.Message;
    });
}
```

* 运行示例 [1.2](register.md)，浏览器访问 http://localhost:1210/weatherforecast/hello ，运行效果如下图：

![输入图片说明](samples/Doc/quick-start/1.4-2.png)