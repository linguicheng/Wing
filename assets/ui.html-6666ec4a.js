import{_ as u}from"./plugin-vue_export-helper-c27b6911.js";import{r as i,o as r,c as d,a as n,b as s,d as a,w as t,e as k}from"./app-d3c3b21c.js";const v="/Wing/assets/1.3-5-4957a2f1.png",m="/Wing/assets/1.3-3-32e8b6ee.png",b="/Wing/assets/1.3-4-52fd7986.png",g={},h=n("h3",{id:"简介",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#简介","aria-hidden":"true"},"#"),s(" 简介")],-1),q=n("p",null,[n("code",null,"Wing.UI"),s("是"),n("code",null,"Wing"),s("微服务框架中的一个可视化操作管理系统，主要功能有服务治理、配置中心、APM管理、Saga分布式事务查询。")],-1),_=n("h3",{id:"创建一个web-api项目",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#创建一个web-api项目","aria-hidden":"true"},"#"),s(" 创建一个Web Api项目")],-1),f=n("li",null,[n("p",null,"提前准备：安装并启动Consul")],-1),y={href:"https://gitee.com/linguicheng/wing-demo/tree/master/1.3",target:"_blank",rel:"noopener noreferrer"},S=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),A=n("code",null,"Wing.Consul",-1),W=n("code",null,"Wing.UI",-1),C={href:"https://freesql.net/guide/#%E5%AE%89%E8%A3%85%E5%8C%85",target:"_blank",rel:"noopener noreferrer"},x=n("code",null,"FreeSql.Provider.SqlServer",-1),w=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.UI

dotnet `),n("span",{class:"token function"},"add"),s(` package FreeSql.Provider.SqlServer
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),I=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul

Install-Package Wing.UI

Install-Package FreeSql.Provider.SqlServer
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),T=k(`<h3 id="program代码" tabindex="-1"><a class="header-anchor" href="#program代码" aria-hidden="true">#</a> Program代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token class-name"><span class="token keyword">var</span></span> builder <span class="token operator">=</span> WebApplication<span class="token punctuation">.</span><span class="token function">CreateBuilder</span><span class="token punctuation">(</span>args<span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Host<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span>builder <span class="token operator">=&gt;</span> builder<span class="token punctuation">.</span><span class="token function">AddConsul</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

builder<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
                 <span class="token punctuation">.</span><span class="token function">AddWingUI</span><span class="token punctuation">(</span>FreeSql<span class="token punctuation">.</span>DataType<span class="token punctuation">.</span>SqlServer<span class="token punctuation">)</span>
                 <span class="token punctuation">.</span><span class="token function">AddJwt</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
                 <span class="token punctuation">.</span><span class="token function">AddAPM</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">var</span></span> app <span class="token operator">=</span> builder<span class="token punctuation">.</span><span class="token function">Build</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
UserContext<span class="token punctuation">.</span>UserLoginAfter <span class="token operator">=</span> user <span class="token operator">=&gt;</span>
<span class="token punctuation">{</span>
    user<span class="token punctuation">.</span>Token <span class="token operator">=</span> app<span class="token punctuation">.</span>Services<span class="token punctuation">.</span><span class="token generic-method"><span class="token function">GetService</span><span class="token generic class-name"><span class="token punctuation">&lt;</span>IAuth<span class="token punctuation">&gt;</span></span></span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">.</span><span class="token function">GetToken</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
    <span class="token keyword">return</span> user<span class="token punctuation">;</span>
<span class="token punctuation">}</span><span class="token punctuation">;</span>
<span class="token comment">// Configure the HTTP request pipeline.</span>

app<span class="token punctuation">.</span><span class="token function">UseHttpsRedirection</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthentication</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">UseAuthorization</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">MapControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

app<span class="token punctuation">.</span><span class="token function">Run</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加配置" tabindex="-1"><a class="header-anchor" href="#添加配置" aria-hidden="true">#</a> 添加配置</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token comment">// 是否启用配置中心，默认启用</span>
  <span class="token property">&quot;ConfigCenterEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Consul&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:8500&quot;</span><span class="token punctuation">,</span>
    <span class="token comment">//定时同步数据时间间隔，单位：秒 小于等于0表示立即响应</span>
    <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span><span class="token punctuation">,</span>
    <span class="token comment">//数据中心</span>
    <span class="token property">&quot;DataCenter&quot;</span><span class="token operator">:</span> <span class="token string">&quot;dc1&quot;</span><span class="token punctuation">,</span>
    <span class="token comment">//等待时间,单位：分钟</span>
    <span class="token property">&quot;WaitTime&quot;</span><span class="token operator">:</span> <span class="token number">3</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">// 数据库链接</span>
  <span class="token property">&quot;ConnectionStrings&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Wing&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Data Source=192.168.56.96;User Id=sa;Password=wing123.;Initial Catalog=Wing;TrustServerCertificate=true;Pooling=true;Min Pool Size=1&quot;</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">//自动同步实体结构到数据库</span>
  <span class="token property">&quot;UseAutoSyncStructure&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
  <span class="token comment">// 首页</span>
  <span class="token property">&quot;Home&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token comment">// 指标耗时异常统计 单位：毫秒  默认60秒</span>
    <span class="token property">&quot;Timeout&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token property">&quot;Gateway&quot;</span><span class="token operator">:</span> <span class="token number">2000</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Apm&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token property">&quot;Http&quot;</span><span class="token operator">:</span> <span class="token number">2000</span><span class="token punctuation">,</span>
        <span class="token property">&quot;WorkServiceHttp&quot;</span><span class="token operator">:</span> <span class="token number">2000</span><span class="token punctuation">,</span>
        <span class="token property">&quot;WorkServiceSql&quot;</span><span class="token operator">:</span> <span class="token number">2000</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token comment">// 查询统计时间 默认：最近一个月</span>
      <span class="token property">&quot;SearchTime&quot;</span><span class="token operator">:</span> <span class="token string">&quot;2020-01-01&quot;</span>
    <span class="token punctuation">}</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">//设置默认登录账号和密码</span>
  <span class="token property">&quot;User&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Account&quot;</span><span class="token operator">:</span> <span class="token string">&quot;admin&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Password&quot;</span><span class="token operator">:</span> <span class="token string">&quot;123456&quot;</span>
  <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">//JWT鉴权配置</span>
  <span class="token property">&quot;Jwt&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Secret&quot;</span><span class="token operator">:</span> <span class="token string">&quot;U2FsdGVkX18E82bSYRnqfv7isu6313BQ8QNTs0TcZZK2rwviQw==&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Iss&quot;</span><span class="token operator">:</span> <span class="token string">&quot;https://gitee.com/linguicheng/Wing&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Aud&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing&quot;</span><span class="token punctuation">,</span>
    <span class="token comment">//token过期分钟数</span>
    <span class="token property">&quot;Expire&quot;</span><span class="token operator">:</span> <span class="token number">1440</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行效果" tabindex="-1"><a class="header-anchor" href="#查看运行效果" aria-hidden="true">#</a> 查看运行效果</h3>`,5),P={href:"http://localhost:1310/wing",target:"_blank",rel:"noopener noreferrer"},U=n("figure",null,[n("img",{src:v,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1),E=n("p",null,"输入设置的默认账号和密码登录之后，跳转到首页，如下图：",-1),L=n("figure",null,[n("img",{src:m,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1),N=n("code",null,"Wing.Demo_1.2",-1),B=n("figure",null,[n("img",{src:b,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1);function F(H,M){const e=i("ExternalLinkIcon"),c=i("CodeTabs"),l=i("RouterLink");return r(),d("div",null,[h,q,_,n("ul",null,[f,n("li",null,[n("p",null,[s("打开 Visual Studio 2022 并创建Web Api项目("),n("a",y,[s("点击查看完整示例代码1.3"),a(e)]),s(")")])])]),S,n("p",null,[s("安装服务注册nuget包"),A,s("，UI可视化界面管理nuget包"),W,s("，选择对应的数据库驱动（"),n("a",C,[s("参考FreeSql官网"),a(e)]),s("），以SqlServer为例，安装"),x,s("。")]),a(c,{id:"27",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:t(({value:o,isActive:p})=>[s(".NET CLI")]),title1:t(({value:o,isActive:p})=>[s("Package Manager")]),tab0:t(({value:o,isActive:p})=>[w]),tab1:t(({value:o,isActive:p})=>[I]),_:1},8,["data"]),T,n("ul",null,[n("li",null,[s("程序运行后，浏览器访问 "),n("a",P,[s("http://localhost:1310/wing"),a(e)]),s(" ，进入登录界面，如下图：")])]),U,E,L,n("ul",null,[n("li",null,[s("可以看到示例 "),a(l,{to:"/guide/quick-start/register.html"},{default:t(()=>[s("1.2")]),_:1}),s(" 注入的服务"),N])]),B])}const R=u(g,[["render",F],["__file","ui.html.vue"]]);export{R as default};
