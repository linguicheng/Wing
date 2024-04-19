# 快速入门

## 介绍
`Wing`致力于打造一个功能强大、最接地气的`.NET`微服务框架，支持`.NET Core 3.1+`运行平台。支持`Consul`服务注册与发现，服务间通讯支持`http`和`grpc`调用，内置负载均衡器。支持`服务策略`配置，服务异常降级处理。支持
`Saga分布式事务`，支持三种恢复策略：向前恢复、向后恢复、先前再后。自带`配置中心`，服务配置可以在线集中统一管理。支持http/grpc/sql链路追踪`APM`及耗时分析统计。内置`服务网关`，支持全局服务策略和个性化服务策略配置。支持`RabbitMQ`事件总线，自带人性化的`Dashboard`管理界面。

## 文档地址
https://linguicheng.gitee.io/wing

## 依赖
需要用到的第三方中间件：`Consul`(强依赖)、`RabbitMQ`(弱依赖)、`数据库`(强依赖，目前支持SqlServer、Oracle、MySql、PostgreSql、Sqlite)

## 技术交流群
`QQ`:`183015352`
