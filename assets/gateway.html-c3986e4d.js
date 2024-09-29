import{_ as u}from"./plugin-vue_export-helper-c27b6911.js";import{r as l,o as r,c as d,a as n,b as s,d as a,w as t,e as k}from"./app-9fb7c2b2.js";const v="/Wing/assets/1.5-3-8d63490d.png",m="/Wing/assets/1.5-4-e2ffb35f.png",b={},h=n("h3",{id:"简介",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#简介","aria-hidden":"true"},"#"),s(" 简介")],-1),g=n("p",null,"服务网关是系统对外的唯一入口，它封装了系统内部架构，为每个客户端提供了定制的API，所有的客户端和消费端都通过统一的网关接入微服务，在网关层处理所有非业务功能。",-1),q=n("h3",{id:"创建一个web-api项目",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#创建一个web-api项目","aria-hidden":"true"},"#"),s(" 创建一个Web Api项目")],-1),_=n("li",null,[n("p",null,"提前准备：安装并启动Consul、RabbitMQ(可选)")],-1),f={href:"https://gitee.com/linguicheng/wing-demo/tree/master/1.5",target:"_blank",rel:"noopener noreferrer"},y=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),W=n("code",null,"Wing.Consul",-1),S=n("code",null,"Wing.Gateway",-1),C={href:"https://freesql.net/guide/#%E5%AE%89%E8%A3%85%E5%8C%85",target:"_blank",rel:"noopener noreferrer"},w=n("code",null,"FreeSql.Provider.SqlServer",-1),x=n("code",null,"EventBus",-1),P=n("code",null,"Wing.RabbitMQ",-1),A=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Gateway

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.RabbitMQ

dotnet `),n("span",{class:"token function"},"add"),s(` package FreeSql.Provider.SqlServer
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),R=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul

Install-Package Wing.Gateway

Install-Package Wing.RabbitMQ

Install-Package FreeSql.Provider.SqlServer
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),E=k(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Add services to the container.</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddPersistence</span><span class="token punctuation">(</span>FreeSql<span class="token punctuation">.</span>DataType<span class="token punctuation">.</span>SqlServer<span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddGateWay</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddEventBus</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span><span class="token comment">// 如果不想使用EventBus记录请求日志，可以删除此行代码</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Configure the HTTP request pipeline.</span>

app<span class="token punctuation">.</span><span class="token function">UseHttpsRedirection</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthorization</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">MapControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加配置" tabindex="-1"><a class="header-anchor" href="#添加配置" aria-hidden="true">#</a> 添加配置</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token comment">// 是否启用配置中心，默认启用</span>
  <span class="token property">&quot;ConfigCenterEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Consul&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:8500&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Service&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">//Http  Grpc</span>
      <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Http&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;HealthCheck&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:1510/health&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Timeout&quot;</span><span class="token operator">:</span> <span class="token number">10</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Name&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_1.5&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Host&quot;</span><span class="token operator">:</span> <span class="token string">&quot;localhost&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Port&quot;</span><span class="token operator">:</span> <span class="token number">1510</span><span class="token punctuation">,</span>
      <span class="token property">&quot;LoadBalancer&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">//RoundRobin  WeightRoundRobin LeastConnection</span>
        <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeightRoundRobin&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//权重</span>
        <span class="token property">&quot;Weight&quot;</span><span class="token operator">:</span> <span class="token number">50</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Scheme&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Developer&quot;</span><span class="token operator">:</span> <span class="token string">&quot;linguicheng&quot;</span>
    <span class="token punctuation">}</span><span class="token punctuation">,</span>
    <span class="token comment">//定时同步数据时间间隔，单位：秒 小于等于0表示立即响应</span>
    <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span><span class="token punctuation">,</span>
    <span class="token comment">//数据中心</span>
    <span class="token property">&quot;DataCenter&quot;</span><span class="token operator">:</span> <span class="token string">&quot;dc1&quot;</span><span class="token punctuation">,</span>
    <span class="token comment">//等待时间,单位：分钟</span>
    <span class="token property">&quot;WaitTime&quot;</span><span class="token operator">:</span> <span class="token number">3</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token property">&quot;ConnectionStrings&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Wing&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Data Source=192.168.56.96;User Id=sa;Password=wing123.;Initial Catalog=Wing;TrustServerCertificate=true;Pooling=true;Min Pool Size=1&quot;</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">//自动同步实体结构到数据库</span>
  <span class="token property">&quot;UseAutoSyncStructure&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
  <span class="token comment">// 如果不启用EventBus，可以删除RabbitMQ配置</span>
  <span class="token property">&quot;RabbitMQ&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;HostName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;192.168.56.99&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;UserName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;admin&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Password&quot;</span><span class="token operator">:</span> <span class="token string">&quot;admin&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;VirtualHost&quot;</span><span class="token operator">:</span> <span class="token string">&quot;/&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Port&quot;</span><span class="token operator">:</span> <span class="token number">5672</span><span class="token punctuation">,</span>
    <span class="token comment">//消息过期时间，单位：毫秒，过期会自动路由到死信队列，小于或等于0则永久有效</span>
    <span class="token property">&quot;MessageTTL&quot;</span><span class="token operator">:</span> <span class="token number">0</span><span class="token punctuation">,</span>
    <span class="token property">&quot;ExchangeName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Sample.GateWay&quot;</span><span class="token punctuation">,</span>
    <span class="token comment">//每次投递消息数量</span>
    <span class="token property">&quot;PrefetchCount&quot;</span><span class="token operator">:</span> <span class="token number">1</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Gateway&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token comment">// 请求日志</span>
    <span class="token property">&quot;Log&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">// 是否启用网关日志记录</span>
      <span class="token property">&quot;IsEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
      <span class="token comment">// 是否启用事件总线(RabbitMQ)存储日志，生产环境推荐启用，可以提升程序的性能</span>
      <span class="token property">&quot;UseEventBus&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span>
    <span class="token punctuation">}</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行效果" tabindex="-1"><a class="header-anchor" href="#查看运行效果" aria-hidden="true">#</a> 查看运行效果</h3>`,5),I=n("br",null,null,-1),M={href:"http://localhost:1510/Wing.Demo_1.2/weatherforecast",target:"_blank",rel:"noopener noreferrer"},T=n("figure",null,[n("img",{src:v,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1),L={href:"http://localhost:1310/wing/index.html#/gateWayLog",target:"_blank",rel:"noopener noreferrer"},B=n("figure",null,[n("img",{src:m,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1),N={class:"hint-container warning"},D=n("p",{class:"hint-container-title"},"服务地址组成",-1),H={href:"http://localhost:1510/Wing.Demo_1.2/weatherforecast",target:"_blank",rel:"noopener noreferrer"};function Q(U,G){const e=l("ExternalLinkIcon"),c=l("CodeTabs"),i=l("RouterLink");return r(),d("div",null,[h,g,q,n("ul",null,[_,n("li",null,[n("p",null,[s("打开 Visual Studio 2022 并创建Web Api项目("),n("a",f,[s("点击查看完整示例代码1.5"),a(e)]),s(")")])])]),y,n("p",null,[s("安装服务注册nuget包"),W,s("，服务网关nuget包"),S,s("，选择对应的数据库驱动（"),n("a",C,[s("参考FreeSql官网"),a(e)]),s("），以SqlServer为例，安装"),w,s("，请求日志支持本地消息队列和分布式消息队列进行异步持久化，基本上不影响网关性能。如果不想记录请求日志，可以不安装该包。如果想启用"),x,s("记录请求日志，需要安装RabbitMQ nuget包"),P,s("。")]),a(c,{id:"27",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:t(({value:o,isActive:p})=>[s(".NET CLI")]),title1:t(({value:o,isActive:p})=>[s("Package Manager")]),tab0:t(({value:o,isActive:p})=>[A]),tab1:t(({value:o,isActive:p})=>[R]),_:1},8,["data"]),E,n("ul",null,[n("li",null,[s("运行示例 "),a(i,{to:"/guide/quick-start/register.html"},{default:t(()=>[s("1.2")]),_:1}),s(" 并启动当前示例程序，浏览器访问"),I,n("a",M,[s("http://localhost:1510/Wing.Demo_1.2/weatherforecast"),a(e)]),s(" ，运行效果如下图：")])]),T,n("ul",null,[n("li",null,[s("运行示例 "),a(i,{to:"/guide/quick-start/ui.html"},{default:t(()=>[s("1.3")]),_:1}),s("，浏览器访问 "),n("a",L,[s("http://localhost:1310/wing/index.html#/gateWayLog"),a(e)]),s(" ，可以看到网关请求日志，运行效果如下图：")])]),B,n("div",N,[D,n("p",null,[s("请求服务地址默认是{网关IP或域名}/{服务名}/{服务路由}，例如："),n("a",H,[s("http://localhost:1510/Wing.Demo_1.2/weatherforecast"),a(e)])])])])}const z=u(b,[["render",Q],["__file","gateway.html.vue"]]);export{z as default};
