import{_ as u}from"./plugin-vue_export-helper-c27b6911.js";import{r as l,o as r,c as d,a as n,b as s,d as p,w as a,e as o}from"./app-63eb0f09.js";const k="/wing/assets/3.3-1-05289da2.png",v="/wing/assets/3.3-2-07a89789.png",b={},m=o('<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p><code>Wing</code>的服务网关支持<code>WebSocket</code>请求的转发，同时支持<code>Wing</code>服务的所有负载均衡算法。</p><h3 id="状态码说明" tabindex="-1"><a class="header-anchor" href="#状态码说明" aria-hidden="true">#</a> 状态码说明</h3><ul><li><p><strong>1001</strong> 找不到下游服务</p></li><li><p><strong>1008</strong> 权限认证不通过</p></li><li><p><strong>1011</strong> 网关或服务异常</p></li></ul><h3 id="创建一个支持websocket的网关" tabindex="-1"><a class="header-anchor" href="#创建一个支持websocket的网关" aria-hidden="true">#</a> 创建一个支持WebSocket的网关</h3>',5),g=n("li",null,[n("p",null,"提前准备：安装并启动Consul")],-1),h={href:"https://gitee.com/linguicheng/wing-demo/tree/master/3.3-1",target:"_blank",rel:"noopener noreferrer"},q=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),y=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Gateway
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),w=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul

Install-Package Wing.Gateway
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),f=o(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
                    <span class="token punctuation">.</span><span class="token function">AddJwt</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
                    <span class="token punctuation">.</span><span class="token function">AddPersistence</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
                    <span class="token punctuation">.</span><span class="token function">AddGateWay</span><span class="token punctuation">(</span><span class="token keyword">new</span> <span class="token constructor-invocation class-name">WebSocketOptions</span>
                    <span class="token punctuation">{</span>
                        KeepAliveInterval <span class="token operator">=</span> TimeSpan<span class="token punctuation">.</span><span class="token function">FromMinutes</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span>
                    <span class="token punctuation">}</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="配置示例" tabindex="-1"><a class="header-anchor" href="#配置示例" aria-hidden="true">#</a> 配置示例</h3><p>支持JWT认证和固定的&quot;Key&quot;认证<br><code>UseJWTAuth</code> 是否启用JWT认证，请求示例<code>ws://localhost:3311/Wing.Demo_3.3/ws?token=abc</code><br><code>AuthKey</code> 固定的&quot;Key&quot;认证，请求示例<code>ws://localhost:3311/Wing.Demo_3.3/ws?key=abc</code></p><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Logging&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;LogLevel&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token property">&quot;Default&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Information&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Microsoft&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Warning&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Microsoft.Hosting.Lifetime&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Information&quot;</span>
    <span class="token punctuation">}</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token property">&quot;AllowedHosts&quot;</span><span class="token operator">:</span> <span class="token string">&quot;*&quot;</span><span class="token punctuation">,</span>
  <span class="token comment">// 是否启用配置中心，默认启用</span>
  <span class="token property">&quot;ConfigCenterEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Consul&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:8500&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Service&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">//Http  Grpc</span>
      <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Http&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;HealthCheck&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Timeout&quot;</span><span class="token operator">:</span> <span class="token number">10</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Name&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_3.3.1&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Host&quot;</span><span class="token operator">:</span> <span class="token string">&quot;localhost&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Port&quot;</span><span class="token operator">:</span> <span class="token number">3311</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Tag&quot;</span><span class="token operator">:</span> <span class="token string">&quot;测试&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;LoadBalancer&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">//RoundRobin  WeightRoundRobin LeastConnection</span>
        <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeightRoundRobin&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//权重</span>
        <span class="token property">&quot;Weight&quot;</span><span class="token operator">:</span> <span class="token number">50</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Scheme&quot;</span><span class="token operator">:</span> <span class="token string">&quot;https&quot;</span>
    <span class="token punctuation">}</span><span class="token punctuation">,</span>
    <span class="token comment">//定时同步数据时间间隔，单位：秒 小于等于0表示立即响应</span>
    <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Jwt&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Secret&quot;</span><span class="token operator">:</span> <span class="token string">&quot;U2FsdGVkX18E82bSYRnqfv7isu6313BQ8QNTs0TcZZK2rwviQw==&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Iss&quot;</span><span class="token operator">:</span> <span class="token string">&quot;https://gitee.com/linguicheng/Wing&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Aud&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing&quot;</span><span class="token punctuation">,</span>
    <span class="token comment">//token过期分钟数</span>
    <span class="token property">&quot;Expire&quot;</span><span class="token operator">:</span> <span class="token number">1440</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">//自动同步实体结构到数据库</span>
  <span class="token property">&quot;UseAutoSyncStructure&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
  <span class="token property">&quot;ConnectionStrings&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Wing&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Data Source=192.168.56.96;Initial Catalog=Wing;User ID=sa;Password=wing123.;MultipleActiveResultSets=true&quot;</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Gateway&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Policy&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">//  // 全局策略</span>
      <span class="token property">&quot;Global&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token property">&quot;UseJWTAuth&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span>
        <span class="token comment">//&quot;AuthKey&quot;: &quot;abc&quot;</span>
      <span class="token punctuation">}</span>
    <span class="token punctuation">}</span><span class="token punctuation">,</span>
    <span class="token comment">// 请求日志</span>
    <span class="token property">&quot;Log&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token property">&quot;IsEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
      <span class="token property">&quot;UseEventBus&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span>
    <span class="token punctuation">}</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="创建一个websocket服务" tabindex="-1"><a class="header-anchor" href="#创建一个websocket服务" aria-hidden="true">#</a> 创建一个WebSocket服务</h3>`,6),_=n("li",null,[n("p",null,"提前准备：安装并启动Consul")],-1),S={href:"https://gitee.com/linguicheng/wing-demo/tree/master/3.3",target:"_blank",rel:"noopener noreferrer"},W=n("h3",{id:"安装依赖包-1",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包-1","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),A=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"})])],-1),C=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"})])],-1),x=o(`<h3 id="program代码-1" tabindex="-1"><a class="header-anchor" href="#program代码-1" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">System<span class="token punctuation">.</span>Net<span class="token punctuation">.</span>WebSockets</span><span class="token punctuation">;</span>
<span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> webSocketOptions <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name">WebSocketOptions</span>
<span class="token punctuation">{</span>
    KeepAliveInterval <span class="token operator">=</span> TimeSpan<span class="token punctuation">.</span><span class="token function">FromMinutes</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span>
<span class="token punctuation">}</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseWebSockets</span><span class="token punctuation">(</span>webSocketOptions<span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseDefaultFiles</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
app<span class="token punctuation">.</span><span class="token function">UseStaticFiles</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Use</span><span class="token punctuation">(</span><span class="token keyword">async</span> <span class="token punctuation">(</span>context<span class="token punctuation">,</span> next<span class="token punctuation">)</span> <span class="token operator">=&gt;</span>
<span class="token punctuation">{</span>
    <span class="token keyword">if</span> <span class="token punctuation">(</span>context<span class="token punctuation">.</span>Request<span class="token punctuation">.</span>Path <span class="token operator">==</span> <span class="token string">&quot;/ws&quot;</span><span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        <span class="token keyword">if</span> <span class="token punctuation">(</span>context<span class="token punctuation">.</span>WebSockets<span class="token punctuation">.</span>IsWebSocketRequest<span class="token punctuation">)</span>
        <span class="token punctuation">{</span>
            <span class="token keyword">using</span> <span class="token class-name"><span class="token keyword">var</span></span> webSocket <span class="token operator">=</span> <span class="token keyword">await</span> context<span class="token punctuation">.</span>WebSockets<span class="token punctuation">.</span><span class="token function">AcceptWebSocketAsync</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
            <span class="token keyword">await</span> <span class="token function">Echo</span><span class="token punctuation">(</span>webSocket<span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token punctuation">}</span>
        <span class="token keyword">else</span>
        <span class="token punctuation">{</span>
            context<span class="token punctuation">.</span>Response<span class="token punctuation">.</span>StatusCode <span class="token operator">=</span> StatusCodes<span class="token punctuation">.</span>Status400BadRequest<span class="token punctuation">;</span>
        <span class="token punctuation">}</span>
    <span class="token punctuation">}</span>
    <span class="token keyword">else</span>
    <span class="token punctuation">{</span>
        <span class="token keyword">await</span> <span class="token function">next</span><span class="token punctuation">(</span>context<span class="token punctuation">)</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span>

<span class="token punctuation">}</span><span class="token punctuation">)</span><span class="token punctuation">;</span>


app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token keyword">async</span> <span class="token return-type class-name">Task</span> <span class="token function">Echo</span><span class="token punctuation">(</span><span class="token class-name">WebSocket</span> webSocket<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token class-name"><span class="token keyword">var</span></span> buffer <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name"><span class="token keyword">byte</span></span><span class="token punctuation">[</span><span class="token number">1024</span> <span class="token operator">*</span> <span class="token number">4</span><span class="token punctuation">]</span><span class="token punctuation">;</span>
    <span class="token class-name"><span class="token keyword">var</span></span> receiveResult <span class="token operator">=</span> <span class="token keyword">await</span> webSocket<span class="token punctuation">.</span><span class="token function">ReceiveAsync</span><span class="token punctuation">(</span>
        <span class="token keyword">new</span> <span class="token constructor-invocation class-name">ArraySegment<span class="token punctuation">&lt;</span><span class="token keyword">byte</span><span class="token punctuation">&gt;</span></span><span class="token punctuation">(</span>buffer<span class="token punctuation">)</span><span class="token punctuation">,</span> CancellationToken<span class="token punctuation">.</span>None<span class="token punctuation">)</span><span class="token punctuation">;</span>

    <span class="token keyword">while</span> <span class="token punctuation">(</span><span class="token operator">!</span>receiveResult<span class="token punctuation">.</span>CloseStatus<span class="token punctuation">.</span>HasValue<span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        <span class="token keyword">await</span> webSocket<span class="token punctuation">.</span><span class="token function">SendAsync</span><span class="token punctuation">(</span>
            <span class="token keyword">new</span> <span class="token constructor-invocation class-name">ArraySegment<span class="token punctuation">&lt;</span><span class="token keyword">byte</span><span class="token punctuation">&gt;</span></span><span class="token punctuation">(</span>buffer<span class="token punctuation">,</span> <span class="token number">0</span><span class="token punctuation">,</span> receiveResult<span class="token punctuation">.</span>Count<span class="token punctuation">)</span><span class="token punctuation">,</span>
            receiveResult<span class="token punctuation">.</span>MessageType<span class="token punctuation">,</span>
            receiveResult<span class="token punctuation">.</span>EndOfMessage<span class="token punctuation">,</span>
            CancellationToken<span class="token punctuation">.</span>None<span class="token punctuation">)</span><span class="token punctuation">;</span>

        receiveResult <span class="token operator">=</span> <span class="token keyword">await</span> webSocket<span class="token punctuation">.</span><span class="token function">ReceiveAsync</span><span class="token punctuation">(</span>
            <span class="token keyword">new</span> <span class="token constructor-invocation class-name">ArraySegment<span class="token punctuation">&lt;</span><span class="token keyword">byte</span><span class="token punctuation">&gt;</span></span><span class="token punctuation">(</span>buffer<span class="token punctuation">)</span><span class="token punctuation">,</span> CancellationToken<span class="token punctuation">.</span>None<span class="token punctuation">)</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span>

    <span class="token keyword">await</span> webSocket<span class="token punctuation">.</span><span class="token function">CloseAsync</span><span class="token punctuation">(</span>
        receiveResult<span class="token punctuation">.</span>CloseStatus<span class="token punctuation">.</span>Value<span class="token punctuation">,</span>
        receiveResult<span class="token punctuation">.</span>CloseStatusDescription<span class="token punctuation">,</span>
        CancellationToken<span class="token punctuation">.</span>None<span class="token punctuation">)</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行结果" tabindex="-1"><a class="header-anchor" href="#查看运行结果" aria-hidden="true">#</a> 查看运行结果</h3><p>启动网关示例3.3-1，启动WebSocket服务实例3.3,点击“Connect”按钮连接<code>WebSocket</code>服务端，在文本框输入要发送的内容，<code>WebSocket</code>服务端会一样返回该内容，如下图：</p><figure><img src="`+k+'" alt="" tabindex="0" loading="lazy"><figcaption></figcaption></figure><figure><img src="'+v+'" alt="" tabindex="0" loading="lazy"><figcaption></figcaption></figure>',6);function R(T,I){const c=l("ExternalLinkIcon"),i=l("CodeTabs");return r(),d("div",null,[m,n("ul",null,[g,n("li",null,[n("p",null,[s("打开 Visual Studio 2022 并创建Web Api项目("),n("a",h,[s("点击查看完整示例代码3.3-1"),p(c)]),s(")")])])]),q,p(i,{id:"44",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:t,isActive:e})=>[s(".NET CLI")]),title1:a(({value:t,isActive:e})=>[s("Package Manager")]),tab0:a(({value:t,isActive:e})=>[y]),tab1:a(({value:t,isActive:e})=>[w]),_:1},8,["data"]),f,n("ul",null,[_,n("li",null,[n("p",null,[s("打开 Visual Studio 2022 并创建Web Api项目("),n("a",S,[s("点击查看完整示例代码3.3"),p(c)]),s(")")])])]),W,p(i,{id:"81",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:t,isActive:e})=>[s(".NET CLI")]),title1:a(({value:t,isActive:e})=>[s("Package Manager")]),tab0:a(({value:t,isActive:e})=>[A]),tab1:a(({value:t,isActive:e})=>[C]),_:1},8,["data"]),x])}const P=u(b,[["render",R],["__file","websocket.html.vue"]]);export{P as default};
