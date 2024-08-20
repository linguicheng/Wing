import{_ as u}from"./plugin-vue_export-helper-c27b6911.js";import{r as c,o as r,c as d,a as n,b as s,d as o,w as a,e as i}from"./app-d3c3b21c.js";const k="/Wing/assets/5.1-1-56bb7c75.png",v="/Wing/assets/5.1-2-cfbb99bf.png",m={},b=i('<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p><code>链路追踪（tracing）</code>即调用链监控，特点是通过记录多个在请求间跨服务完成的逻辑请求信息，帮助开发人员优化性能和进行问题追踪。链路追踪可以捕获每个请求遇到的异常和错误，以及即时信息和有价值的数据。</p><div class="hint-container info"><p class="hint-container-title">为什么需要链路追踪？</p><p>在大型系统的微服务化构建中，一个系统被拆分成了许多个微服务。这些模块负责不同的功能，组合成系统，最终可以提供丰富的功能。在这种架构中，一次请求往往需要涉及到多个微服务。如果线上某个微服务出现故障，如何快速定位故障所在的微服务呢？<br> 答案就是链路追踪技术。</p></div><p><strong><code>Wing.APM</code>支持http和grpc请求追踪，支持Sql语句耗时分析，支持定时作业的链路追踪</strong></p><h3 id="安装依赖包" tabindex="-1"><a class="header-anchor" href="#安装依赖包" aria-hidden="true">#</a> 安装依赖包</h3>',5),h=n("code",null,"5.1",-1),g={href:"https://gitee.com/linguicheng/wing-demo/tree/master/5.1",target:"_blank",rel:"noopener noreferrer"},_=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.APM

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Persistence

dotnet `),n("span",{class:"token function"},"add"),s(` package FreeSql.Provider.SqlServer
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),f=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.APM

Install-Package Wing.Consul

Install-Package Wing.Persistence

Install-Package FreeSql.Provider.SqlServer
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),q=i(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Add services to the container.</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">.</span><span class="token function">AddPersistence</span><span class="token punctuation">(</span>FreeSql<span class="token punctuation">.</span>DataType<span class="token punctuation">.</span>SqlServer<span class="token punctuation">)</span><span class="token punctuation">.</span><span class="token function">AddAPM</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Configure the HTTP request pipeline.</span>

app<span class="token punctuation">.</span><span class="token function">UseHttpsRedirection</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthorization</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">MapControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加配置" tabindex="-1"><a class="header-anchor" href="#添加配置" aria-hidden="true">#</a> 添加配置</h3><p><code>Apm:IsEnabled</code> 是否开启 默认开启，false表示关闭</p><p><code>Apm:PersistenceSeconds</code> 设置定时持久化间隔，单位：秒，默认每3秒钟执行一次</p><p><code>Apm:ToListenerUrl</code> 仅监听某些请求Url</p><p><code>Apm:DoNotListenerUrl</code> 不监听的请求Url</p><p><code>Apm:ToListenerSql</code> 仅监听某些Sql</p><p><code>Apm:DoNotListenerSql</code> 不监听的Sql</p><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Apm&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;IsEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
    <span class="token property">&quot;PersistenceSeconds&quot;</span><span class="token operator">:</span><span class="token number">3</span><span class="token punctuation">,</span>
    <span class="token comment">// 仅监听某些请求Url(包含关系)</span>
    <span class="token property">&quot;ToListenerUrl&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span><span class="token punctuation">]</span><span class="token punctuation">,</span>
    <span class="token comment">// 不监听的请求Url(包含关系)</span>
    <span class="token property">&quot;DoNotListenerUrl&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span><span class="token punctuation">]</span><span class="token punctuation">,</span>
    <span class="token comment">// 仅监听的Sql(包含关系)</span>
    <span class="token property">&quot;ToListenerSql&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span><span class="token punctuation">]</span><span class="token punctuation">,</span>
    <span class="token comment">// 不监听的Sql(包含关系)</span>
    <span class="token property">&quot;DoNotListenerSql&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span><span class="token punctuation">]</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行结果" tabindex="-1"><a class="header-anchor" href="#查看运行结果" aria-hidden="true">#</a> 查看运行结果</h3>`,11),A=n("code",null,"5.1",-1),S={href:"http://localhost:5110/weatherforecast",target:"_blank",rel:"noopener noreferrer"},P=n("figure",null,[n("img",{src:k,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1),x=n("ul",null,[n("li",null,[s("作业请求追踪"),n("br"),n("img",{src:v,alt:"",loading:"lazy"})])],-1);function y(C,W){const p=c("ExternalLinkIcon"),l=c("CodeTabs");return r(),d("div",null,[b,n("p",null,[s("示例"),h,s("("),n("a",g,[s("点击查看完整示例代码"),o(p)]),s(")")]),o(l,{id:"20",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:e,isActive:t})=>[s(".NET CLI")]),title1:a(({value:e,isActive:t})=>[s("Package Manager")]),tab0:a(({value:e,isActive:t})=>[_]),tab1:a(({value:e,isActive:t})=>[f]),_:1},8,["data"]),q,n("ul",null,[n("li",null,[s("运行示例"),A,s("，浏览器访问 "),n("a",S,[s("http://localhost:5110/weatherforecast"),o(p)]),s(" ，可以看到该请求的相关信息，如下图：")])]),P,x])}const I=u(m,[["render",y],["__file","http.html.vue"]]);export{I as default};
