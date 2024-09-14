import{_ as l}from"./plugin-vue_export-helper-c27b6911.js";import{r as o,o as u,c as r,a as n,b as s,d as c,w as a,e as d}from"./app-1b6eba36.js";const k="/Wing/assets/5.3-fc3825d8.png",v={},m=n("h3",{id:"简介",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#简介","aria-hidden":"true"},"#"),s(" 简介")],-1),b=n("p",null,[s("支持对ORM框架"),n("code",null,"SqlSugar"),s("生成并执行的SQL语句进行跟踪与分析，以方便开发人员优化程序性能。")],-1),g=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),h=n("code",null,"5.3",-1),f={href:"https://gitee.com/linguicheng/wing-demo/tree/master/5.3",target:"_blank",rel:"noopener noreferrer"},_=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.APM.SqlSugar

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Persistence

dotnet `),n("span",{class:"token function"},"add"),s(` package FreeSql.Provider.SqlServer
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),S=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul

Install-Package Wing.APM.SqlSugar

Install-Package Wing.Persistence

Install-Package FreeSql.Provider.SqlServer
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),C=d(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">using</span> <span class="token namespace">Wing</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Add services to the container.</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddPersistence</span><span class="token punctuation">(</span>FreeSql<span class="token punctuation">.</span>DataType<span class="token punctuation">.</span>SqlServer<span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddAPM</span><span class="token punctuation">(</span>x <span class="token operator">=&gt;</span> x<span class="token punctuation">.</span><span class="token function">AddSqlSugar</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token generic-method"><span class="token function">AddScoped</span><span class="token generic class-name"><span class="token punctuation">&lt;</span>ISqlSugarClient<span class="token punctuation">&gt;</span></span></span><span class="token punctuation">(</span>s <span class="token operator">=&gt;</span>
    <span class="token punctuation">{</span>
        <span class="token class-name">SqlSugarClient</span> sqlSugar <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name">SqlSugarClient</span><span class="token punctuation">(</span><span class="token keyword">new</span> <span class="token constructor-invocation class-name">ConnectionConfig</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
        <span class="token punctuation">{</span>
            DbType <span class="token operator">=</span> DbType<span class="token punctuation">.</span>SqlServer<span class="token punctuation">,</span>
            ConnectionString <span class="token operator">=</span> builder<span class="token punctuation">.</span>Configuration<span class="token punctuation">[</span><span class="token string">&quot;ConnectionStrings:Wing.Demo&quot;</span><span class="token punctuation">]</span><span class="token punctuation">,</span>
            IsAutoCloseConnection <span class="token operator">=</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
        <span class="token punctuation">}</span><span class="token punctuation">,</span>
        db <span class="token operator">=&gt;</span>db<span class="token punctuation">.</span><span class="token function">AddWingAPM</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">return</span> sqlSugar<span class="token punctuation">;</span>
    <span class="token punctuation">}</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token comment">// Configure the HTTP request pipeline.</span>

app<span class="token punctuation">.</span><span class="token function">UseHttpsRedirection</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthorization</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">MapControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行结果" tabindex="-1"><a class="header-anchor" href="#查看运行结果" aria-hidden="true">#</a> 查看运行结果</h3>`,3),q=n("code",null,"5.3",-1),A={href:"http://localhost:5310/weatherforecast",target:"_blank",rel:"noopener noreferrer"},x=n("figure",null,[n("img",{src:k,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1);function P(w,W){const p=o("ExternalLinkIcon"),i=o("CodeTabs");return u(),r("div",null,[m,b,g,n("p",null,[s("示例"),h,s("("),n("a",f,[s("点击查看完整示例代码"),c(p)]),s(")")]),c(i,{id:"12",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:e,isActive:t})=>[s(".NET CLI")]),title1:a(({value:e,isActive:t})=>[s("Package Manager")]),tab0:a(({value:e,isActive:t})=>[_]),tab1:a(({value:e,isActive:t})=>[S]),_:1},8,["data"]),C,n("ul",null,[n("li",null,[s("运行示例"),q,s("，浏览器访问 "),n("a",A,[s("http://localhost:5310/weatherforecast"),c(p)]),s(" ，可以看到该http请求追踪信息和SQL的相关信息，如下图：")])]),x])}const T=l(v,[["render",P],["__file","sqlsugar.html.vue"]]);export{T as default};
