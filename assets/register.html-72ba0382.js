import{_ as l}from"./plugin-vue_export-helper-c27b6911.js";import{r as o,o as u,c as r,a as n,b as s,d as p,w as a,e as d}from"./app-d3c3b21c.js";const k="/Wing/assets/1.2-2-9f5677a5.png",v={},m=n("h3",{id:"什么是服务注册",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#什么是服务注册","aria-hidden":"true"},"#"),s(" 什么是服务注册?")],-1),b=n("p",null,[n("code",null,"服务注册"),s("是指服务启动后将该服务的IP、端口等信息注册到"),n("code",null,"Consul"),s("。")],-1),h=n("h3",{id:"创建一个web-api项目",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#创建一个web-api项目","aria-hidden":"true"},"#"),s(" 创建一个Web Api项目")],-1),g=n("li",null,[n("p",null,"提前准备：安装并启动Consul")],-1),q={href:"https://gitee.com/linguicheng/wing-demo/tree/master/1.2",target:"_blank",rel:"noopener noreferrer"},_=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),f=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"})])],-1),y=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul 
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"})])],-1),C=d(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Add services to the container.</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Configure the HTTP request pipeline.</span>

app<span class="token punctuation">.</span><span class="token function">UseHttpsRedirection</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthorization</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">MapControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加配置" tabindex="-1"><a class="header-anchor" href="#添加配置" aria-hidden="true">#</a> 添加配置</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token comment">// 是否启用配置中心，默认启用</span>
  <span class="token property">&quot;ConfigCenterEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Consul&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:8500&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Service&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">//Http  Grpc</span>
      <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Http&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;HealthCheck&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:1210/health&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Timeout&quot;</span><span class="token operator">:</span> <span class="token number">10</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Name&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_1.2.1&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Host&quot;</span><span class="token operator">:</span> <span class="token string">&quot;localhost&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Port&quot;</span><span class="token operator">:</span> <span class="token number">1210</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Tag&quot;</span><span class="token operator">:</span> <span class="token string">&quot;&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;LoadBalancer&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">//RoundRobin  WeightRoundRobin LeastConnection</span>
        <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeightRoundRobin&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//权重</span>
        <span class="token property">&quot;Weight&quot;</span><span class="token operator">:</span> <span class="token number">60</span>
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
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行效果" tabindex="-1"><a class="header-anchor" href="#查看运行效果" aria-hidden="true">#</a> 查看运行效果</h3><ul><li>程序运行后，打开consul UI管理界面，可以看到注册服务<code>Wing.Demo_1.2</code>，如下图：</li></ul><figure><img src="`+k+'" alt="" tabindex="0" loading="lazy"><figcaption></figcaption></figure>',7);function x(W,A){const i=o("ExternalLinkIcon"),c=o("CodeTabs");return u(),r("div",null,[m,b,h,n("ul",null,[g,n("li",null,[n("p",null,[s("打开 Visual Studio 2022 并创建Web Api项目("),n("a",q,[s("点击查看完整示例代码1.2"),p(i)]),s(")")])])]),_,p(c,{id:"24",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:e,isActive:t})=>[s(".NET CLI")]),title1:a(({value:e,isActive:t})=>[s("Package Manager")]),tab0:a(({value:e,isActive:t})=>[f]),tab1:a(({value:e,isActive:t})=>[y]),_:1},8,["data"]),C])}const w=l(v,[["render",x],["__file","register.html.vue"]]);export{w as default};
