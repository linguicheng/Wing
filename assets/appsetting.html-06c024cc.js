import{_ as r}from"./plugin-vue_export-helper-c27b6911.js";import{r as i,o as d,c as k,a as n,b as s,d as o,w as a,e as c}from"./app-77b79710.js";const v="/wing/assets/2.2-1-cb18a490.png",b="/wing/assets/2.2-2-1a480fba.png",m={},h=c('<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p><code>配置中心</code>是对服务配置进行统一管理，用来动态修改程序执行的机制。</p><h3 id="为什么要使用配置中心" tabindex="-1"><a class="header-anchor" href="#为什么要使用配置中心" aria-hidden="true">#</a> 为什么要使用配置中心？</h3><ul><li><p><strong>统一性</strong>：所有服务配置统一在<code>配置中心</code>管理，这样更简单，不易出错。</p></li><li><p><strong>高效性</strong>：如果一个服务有多个副本，按传统方式需要改几遍，很费时间。有了<code>配置中心</code>，只需要改一遍就可以及时同步给其他服务副本，非常高效、及时。</p></li><li><p><strong>安全性</strong>：配置跟随源码一起保存在代码库中，容易造成配置泄漏。</p></li></ul><h3 id="创建一个web-api项目" tabindex="-1"><a class="header-anchor" href="#创建一个web-api项目" aria-hidden="true">#</a> 创建一个Web Api项目</h3>',5),g=n("li",null,[n("p",null,"提前准备：安装并启动Consul")],-1),q={href:"https://gitee.com/linguicheng/wing-demo/tree/master/2.2",target:"_blank",rel:"noopener noreferrer"},f=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),_=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Consul
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"})])],-1),y=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,`Install-Package Wing.Consul 
`)]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"})])],-1),C=n("h3",{id:"program代码",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#program代码","aria-hidden":"true"},"#"),s(" Program代码")],-1),x=n("div",{class:"language-csharp line-numbers-mode","data-ext":"cs"},[n("pre",{class:"language-csharp"},[n("code",null,[n("span",{class:"token keyword"},"public"),s(),n("span",{class:"token keyword"},"static"),s(),n("span",{class:"token return-type class-name"},"IHostBuilder"),s(),n("span",{class:"token function"},"CreateHostBuilder"),n("span",{class:"token punctuation"},"("),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"string"),n("span",{class:"token punctuation"},"["),n("span",{class:"token punctuation"},"]")]),s(" args"),n("span",{class:"token punctuation"},")"),s(),n("span",{class:"token operator"},"=>"),s(`
            Host`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"CreateDefaultBuilder"),n("span",{class:"token punctuation"},"("),s("args"),n("span",{class:"token punctuation"},")"),s(`
                `),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"ConfigureWebHostDefaults"),n("span",{class:"token punctuation"},"("),s("webBuilder "),n("span",{class:"token operator"},"=>"),s(`
                `),n("span",{class:"token punctuation"},"{"),s(`
                    webBuilder`),n("span",{class:"token punctuation"},"."),n("span",{class:"token generic-method"},[n("span",{class:"token function"},"UseStartup"),n("span",{class:"token generic class-name"},[n("span",{class:"token punctuation"},"<"),s("Startup"),n("span",{class:"token punctuation"},">")])]),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`
                `),n("span",{class:"token punctuation"},"}"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddWing"),n("span",{class:"token punctuation"},"("),s("builder "),n("span",{class:"token operator"},"=>"),s(" builder"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddConsul"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`
`)])]),n("div",{class:"highlight-lines"},[n("br"),n("br"),n("br"),n("br"),n("br"),n("div",{class:"highlight-line"}," ")]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),w=n("div",{class:"language-csharp line-numbers-mode","data-ext":"cs"},[n("pre",{class:"language-csharp"},[n("code",null,[n("span",{class:"token keyword"},"using"),s(),n("span",{class:"token namespace"},"Wing"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"var")]),s(" builder "),n("span",{class:"token operator"},"="),s(" WebApplication"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"CreateBuilder"),n("span",{class:"token punctuation"},"("),s("args"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

builder`),n("span",{class:"token punctuation"},"."),s("Host"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddWing"),n("span",{class:"token punctuation"},"("),s("builder "),n("span",{class:"token operator"},"=>"),s(" builder"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddConsul"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token comment"},"// Add services to the container."),s(`

builder`),n("span",{class:"token punctuation"},"."),s("Services"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddControllers"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

builder`),n("span",{class:"token punctuation"},"."),s("Services"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddWing"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"var")]),s(" app "),n("span",{class:"token operator"},"="),s(" builder"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"Build"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token comment"},"// Configure the HTTP request pipeline."),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"UseHttpsRedirection"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"UseAuthorization"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"MapControllers"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"Run"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`
`)])]),n("div",{class:"highlight-lines"},[n("div",{class:"highlight-line"}," "),n("br"),n("br"),n("br"),n("div",{class:"highlight-line"}," "),n("br"),n("br"),n("br"),n("br"),n("br"),n("div",{class:"highlight-line"}," "),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br")]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),A=c(`<h3 id="startup代码" tabindex="-1"><a class="header-anchor" href="#startup代码" aria-hidden="true">#</a> Startup代码</h3><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">public</span> <span class="token return-type class-name"><span class="token keyword">void</span></span> <span class="token function">ConfigureServices</span><span class="token punctuation">(</span><span class="token class-name">IServiceCollection</span> services<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
    services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
</code></pre><div class="highlight-lines"><br><br><br><div class="highlight-line"> </div><br></div><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加consul配置" tabindex="-1"><a class="header-anchor" href="#添加consul配置" aria-hidden="true">#</a> 添加Consul配置</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token comment">// 是否启用配置中心，默认启用</span>
  <span class="token property">&quot;ConfigCenterEnabled&quot;</span><span class="token operator">:</span> <span class="token boolean">true</span><span class="token punctuation">,</span>
  <span class="token property">&quot;Consul&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:8500&quot;</span><span class="token punctuation">,</span>
    <span class="token property">&quot;Service&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">//Http  Grpc</span>
      <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Http&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;HealthCheck&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;http://localhost:2210/health&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Timeout&quot;</span><span class="token operator">:</span> <span class="token number">10</span><span class="token punctuation">,</span>
        <span class="token comment">//单位：秒</span>
        <span class="token property">&quot;Interval&quot;</span><span class="token operator">:</span> <span class="token number">10</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Name&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_2.2&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Host&quot;</span><span class="token operator">:</span> <span class="token string">&quot;localhost&quot;</span><span class="token punctuation">,</span>
      <span class="token property">&quot;Port&quot;</span><span class="token operator">:</span> <span class="token number">2210</span><span class="token punctuation">,</span>
      <span class="token property">&quot;LoadBalancer&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">//RoundRobin  WeightRoundRobin LeastConnection</span>
        <span class="token property">&quot;Option&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeightRoundRobin&quot;</span><span class="token punctuation">,</span>
        <span class="token comment">//权重</span>
        <span class="token property">&quot;Weight&quot;</span><span class="token operator">:</span> <span class="token number">60</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token comment">// 配置中心的Key，如果为空，则取服务名</span>
      <span class="token property">&quot;ConfigKey&quot;</span><span class="token operator">:</span> <span class="token string">&quot;&quot;</span><span class="token punctuation">,</span>
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
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="配置中心添加配置信息" tabindex="-1"><a class="header-anchor" href="#配置中心添加配置信息" aria-hidden="true">#</a> 配置中心添加配置信息</h3>`,5),W={href:"http://localhost:1310/wing/index.html#/config",target:"_blank",rel:"noopener noreferrer"},T=n("code",null,"Wing.Demo_2.2",-1),H=n("code",null,"json",-1),B=c('<figure><img src="'+v+`" alt="" tabindex="0" loading="lazy"><figcaption></figcaption></figure><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
   <span class="token property">&quot;Test&quot;</span><span class="token operator">:</span><span class="token string">&quot;hello wing&quot;</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行效果" tabindex="-1"><a class="header-anchor" href="#查看运行效果" aria-hidden="true">#</a> 查看运行效果</h3><ul><li>读取配置中心添加的配置信息</li></ul><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token punctuation">[</span><span class="token attribute"><span class="token class-name">HttpGet</span></span><span class="token punctuation">]</span>
<span class="token keyword">public</span> <span class="token return-type class-name"><span class="token keyword">string</span></span> <span class="token function">Test</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token keyword">return</span> App<span class="token punctuation">.</span>Configuration<span class="token punctuation">[</span><span class="token string">&quot;Test&quot;</span><span class="token punctuation">]</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,5),N={href:"http://localhost:2210/weatherforecast/test",target:"_blank",rel:"noopener noreferrer"},S=n("figure",null,[n("img",{src:b,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1);function E(R,I){const p=i("ExternalLinkIcon"),l=i("CodeTabs"),u=i("RouterLink");return d(),k("div",null,[h,n("ul",null,[g,n("li",null,[n("p",null,[s("打开 Visual Studio 2022 并创建Web Api项目("),n("a",q,[s("点击查看完整示例代码2.2"),o(p)]),s(")")])])]),f,o(l,{id:"44",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:t,isActive:e})=>[s(".NET CLI")]),title1:a(({value:t,isActive:e})=>[s("Package Manager")]),tab0:a(({value:t,isActive:e})=>[_]),tab1:a(({value:t,isActive:e})=>[y]),_:1},8,["data"]),C,o(l,{id:"55",data:[{id:".NET Core 3.1"},{id:".NET 6.0"}]},{title0:a(({value:t,isActive:e})=>[s(".NET Core 3.1")]),title1:a(({value:t,isActive:e})=>[s(".NET 6.0")]),tab0:a(({value:t,isActive:e})=>[x]),tab1:a(({value:t,isActive:e})=>[w]),_:1},8,["data"]),A,n("ul",null,[n("li",null,[s("运行示例 "),o(u,{to:"/guide/quick-start/ui.html"},{default:a(()=>[s("1.3")]),_:1}),s("，浏览器访问 "),n("a",W,[s("http://localhost:1310/wing/index.html#/config"),o(p)]),s(" ，添加配置Key"),T,s("并输入"),H,s("格式配置，如下图：")])]),B,n("ul",null,[n("li",null,[s("运行当前示例程序，浏览器访问 "),n("a",N,[s("http://localhost:2210/weatherforecast/test"),o(p)]),s(" ,可以看到我们添加的配置，如下图：")])]),S])}const D=r(m,[["render",E],["__file","appsetting.html.vue"]]);export{D as default};
