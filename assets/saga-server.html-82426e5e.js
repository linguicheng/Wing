import{_ as l}from"./plugin-vue_export-helper-c27b6911.js";import{r as c,o as u,c as r,a as n,b as s,d as p,w as a,e as d}from"./app-9fb7c2b2.js";const k={},v=n("h3",{id:"简介",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#简介","aria-hidden":"true"},"#"),s(" 简介")],-1),m=n("p",null,"Saga事务协调器主要是记录事务的执行日志和对失败事务进行重试。",-1),b=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),g=n("li",null,[n("p",null,"提前准备：安装并启动Consul、RabbitMQ(可配置不启用)")],-1),h={href:"https://freesql.net/guide/#%E5%AE%89%E8%A3%85%E5%8C%85",target:"_blank",rel:"noopener noreferrer"},_=n("code",null,"FreeSql.Provider.SqlServer",-1),f=n("code",null,"4.3",-1),S={href:"https://gitee.com/linguicheng/wing-demo/tree/master/4.3",target:"_blank",rel:"noopener noreferrer"},q=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Saga.Server

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.RabbitMQ

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(` package FreeSql.Provider.SqlServer
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),A=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Saga.Server

Install-Package Wing.RabbitMQ

Install-Package Wing.Consul

Install-Package FreeSql.Provider.SqlServer
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),x=d(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><p><code>Headers</code> 设置请求头（调用事务重试接口时，传递JWT认证Token）</p><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Add services to the container.</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddJwt</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddPersistence</span><span class="token punctuation">(</span>FreeSql<span class="token punctuation">.</span>DataType<span class="token punctuation">.</span>SqlServer<span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddEventBus</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddSaga</span><span class="token punctuation">(</span><span class="token keyword">new</span> <span class="token constructor-invocation class-name">SagaOptions</span>
            <span class="token punctuation">{</span>
                Headers <span class="token operator">=</span> <span class="token punctuation">(</span><span class="token punctuation">)</span> <span class="token operator">=&gt;</span>
                <span class="token punctuation">{</span>
                    <span class="token class-name"><span class="token keyword">var</span></span> token <span class="token operator">=</span> <span class="token interpolation-string"><span class="token string">$&quot;Bearer </span><span class="token interpolation"><span class="token punctuation">{</span><span class="token expression language-csharp">App<span class="token punctuation">.</span><span class="token generic-method"><span class="token function">GetRequiredService</span><span class="token generic class-name"><span class="token punctuation">&lt;</span>IAuth<span class="token punctuation">&gt;</span></span></span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">.</span><span class="token function">GetToken</span><span class="token punctuation">(</span><span class="token punctuation">)</span></span><span class="token punctuation">}</span></span><span class="token string">&quot;</span></span><span class="token punctuation">;</span>
                    <span class="token keyword">return</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name">Dictionary<span class="token punctuation">&lt;</span><span class="token keyword">string</span><span class="token punctuation">,</span> <span class="token keyword">string</span><span class="token punctuation">&gt;</span></span> <span class="token punctuation">{</span> <span class="token punctuation">{</span> <span class="token string">&quot;Authorization&quot;</span><span class="token punctuation">,</span> token <span class="token punctuation">}</span> <span class="token punctuation">}</span><span class="token punctuation">;</span>
                <span class="token punctuation">}</span>
            <span class="token punctuation">}</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Configure the HTTP request pipeline.</span>

app<span class="token punctuation">.</span><span class="token function">UseHttpsRedirection</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthorization</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">MapControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加配置" tabindex="-1"><a class="header-anchor" href="#添加配置" aria-hidden="true">#</a> 添加配置</h3><p><code>Saga:RetrySeconds</code> 重试失败事务间隔，单位：秒，默认5分钟</p><p><code>Saga:ReportUpdateTime</code> 定时更新报表数据</p><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Saga&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token comment">// 每隔多少秒重试失败事务 默认5分钟</span>
    <span class="token property">&quot;RetrySeconds&quot;</span><span class="token operator">:</span> <span class="token number">60</span><span class="token punctuation">,</span>
    <span class="token comment">// 指定时间进行报表数据计算</span>
    <span class="token property">&quot;ReportUpdateTime&quot;</span><span class="token operator">:</span> <span class="token string">&quot;17:05&quot;</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,7);function y(w,C){const o=c("ExternalLinkIcon"),i=c("CodeTabs");return u(),r("div",null,[v,m,b,n("ul",null,[g,n("li",null,[n("p",null,[s("创建一个Web Api项目或者Work Service项目（生产环境建议使用），选择对应的数据库驱动（"),n("a",h,[s("参考FreeSql官网"),p(o)]),s("），以SqlServer为例，安装"),_,s("。示例"),f,s("("),n("a",S,[s("点击查看完整示例代码"),p(o)]),s(")")])])]),p(i,{id:"21",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:e,isActive:t})=>[s(".NET CLI")]),title1:a(({value:e,isActive:t})=>[s("Package Manager")]),tab0:a(({value:e,isActive:t})=>[q]),tab1:a(({value:e,isActive:t})=>[A]),_:1},8,["data"]),x])}const P=l(k,[["render",y],["__file","saga-server.html.vue"]]);export{P as default};
