import{_ as p}from"./plugin-vue_export-helper-c27b6911.js";import{r as i,o as u,c as r,a as n,b as s,d as c,w as a,e as d}from"./app-77b79710.js";const k={},v=n("h3",{id:"简介",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#简介","aria-hidden":"true"},"#"),s(" 简介")],-1),b=n("p",null,"Saga事务协调器主要是记录事务的执行日志和对失败事务进行重试。",-1),m=n("h3",{id:"安装依赖包",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#安装依赖包","aria-hidden":"true"},"#"),s(" 安装依赖包")],-1),g=n("li",null,[n("p",null,"提前准备：安装并启动Consul、RabbitMQ(可配置不启用)、数据库支持(SqlServer、Oracle、MySql、PostgreSql)")],-1),h=n("code",null,"4.3",-1),f={href:"https://gitee.com/linguicheng/wing-demo/tree/master/4.3",target:"_blank",rel:"noopener noreferrer"},y=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s("dotnet "),n("span",{class:"token function"},"add"),s(` package Wing.Saga.Server

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.RabbitMQ

dotnet `),n("span",{class:"token function"},"add"),s(` package Wing.Consul

dotnet `),n("span",{class:"token function"},"add"),s(" package Wing.SqlServer"),n("span",{class:"token punctuation"},"("),s("可选Wing.MySql/Wing.Oracle/Wing.PostgreSQL"),n("span",{class:"token punctuation"},")"),s(`
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),_=n("div",{class:"language-bash line-numbers-mode","data-ext":"sh"},[n("pre",{class:"language-bash"},[n("code",null,[s(`Install-Package Wing.Saga.Server

Install-Package Wing.RabbitMQ

Install-Package Wing.Consul

Install-Package Wing.SqlServer`),n("span",{class:"token punctuation"},"("),s("可选Wing.MySql/Wing.Oracle/Wing.PostgreSQL"),n("span",{class:"token punctuation"},")"),s(`
`)])]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),S=n("h3",{id:"program代码",tabindex:"-1"},[n("a",{class:"header-anchor",href:"#program代码","aria-hidden":"true"},"#"),s(" Program代码")],-1),w=n("div",{class:"language-csharp line-numbers-mode","data-ext":"cs"},[n("pre",{class:"language-csharp"},[n("code",null,[n("span",{class:"token keyword"},"public"),s(),n("span",{class:"token keyword"},"static"),s(),n("span",{class:"token return-type class-name"},"IHostBuilder"),s(),n("span",{class:"token function"},"CreateHostBuilder"),n("span",{class:"token punctuation"},"("),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"string"),n("span",{class:"token punctuation"},"["),n("span",{class:"token punctuation"},"]")]),s(" args"),n("span",{class:"token punctuation"},")"),s(),n("span",{class:"token operator"},"=>"),s(`
            Host`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"CreateDefaultBuilder"),n("span",{class:"token punctuation"},"("),s("args"),n("span",{class:"token punctuation"},")"),s(`
                `),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"ConfigureWebHostDefaults"),n("span",{class:"token punctuation"},"("),s("webBuilder "),n("span",{class:"token operator"},"=>"),s(`
                `),n("span",{class:"token punctuation"},"{"),s(`
                    webBuilder`),n("span",{class:"token punctuation"},"."),n("span",{class:"token generic-method"},[n("span",{class:"token function"},"UseStartup"),n("span",{class:"token generic class-name"},[n("span",{class:"token punctuation"},"<"),s("Startup"),n("span",{class:"token punctuation"},">")])]),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`
                `),n("span",{class:"token punctuation"},"}"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddWing"),n("span",{class:"token punctuation"},"("),s("builder "),n("span",{class:"token operator"},"=>"),s(" builder"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddConsul"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`
`)])]),n("div",{class:"highlight-lines"},[n("br"),n("br"),n("br"),n("br"),n("br"),n("div",{class:"highlight-line"}," ")]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),A=n("div",{class:"language-csharp line-numbers-mode","data-ext":"cs"},[n("pre",{class:"language-csharp"},[n("code",null,[n("span",{class:"token keyword"},"using"),s(),n("span",{class:"token namespace"},"Wing"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"var")]),s(" builder "),n("span",{class:"token operator"},"="),s(" WebApplication"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"CreateBuilder"),n("span",{class:"token punctuation"},"("),s("args"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

builder`),n("span",{class:"token punctuation"},"."),s("Host"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddWing"),n("span",{class:"token punctuation"},"("),s("builder "),n("span",{class:"token operator"},"=>"),s(" builder"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddConsul"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token comment"},"// Add services to the container."),s(`

builder`),n("span",{class:"token punctuation"},"."),s("Services"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddControllers"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

builder`),n("span",{class:"token punctuation"},"."),s("Services"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddWing"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),s(`
            `),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddJwt"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),s(`
            `),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddPersistence"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),s(`
            `),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddEventBus"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),s(`
            `),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"AddSaga"),n("span",{class:"token punctuation"},"("),s("serviceProvider "),n("span",{class:"token operator"},"=>"),s(`
            `),n("span",{class:"token punctuation"},"{"),s(`
                `),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"var")]),s(" token "),n("span",{class:"token operator"},"="),s(),n("span",{class:"token interpolation-string"},[n("span",{class:"token string"},'$"Bearer '),n("span",{class:"token interpolation"},[n("span",{class:"token punctuation"},"{"),n("span",{class:"token expression language-csharp"},[s("serviceProvider"),n("span",{class:"token punctuation"},"."),n("span",{class:"token generic-method"},[n("span",{class:"token function"},"GetRequiredService"),n("span",{class:"token generic class-name"},[n("span",{class:"token punctuation"},"<"),s("IAuth"),n("span",{class:"token punctuation"},">")])]),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"GetToken"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")")]),n("span",{class:"token punctuation"},"}")]),n("span",{class:"token string"},'"')]),n("span",{class:"token punctuation"},";"),s(`
                `),n("span",{class:"token keyword"},"return"),s(),n("span",{class:"token keyword"},"new"),s(),n("span",{class:"token constructor-invocation class-name"},"SagaOptions"),s(`
                `),n("span",{class:"token punctuation"},"{"),s(`
                    Headers `),n("span",{class:"token operator"},"="),s(),n("span",{class:"token keyword"},"new"),s(),n("span",{class:"token constructor-invocation class-name"},[s("Dictionary"),n("span",{class:"token punctuation"},"<"),n("span",{class:"token keyword"},"string"),n("span",{class:"token punctuation"},","),s(),n("span",{class:"token keyword"},"string"),n("span",{class:"token punctuation"},">")]),s(),n("span",{class:"token punctuation"},"{"),s(),n("span",{class:"token punctuation"},"{"),s(),n("span",{class:"token string"},'"Authorization"'),n("span",{class:"token punctuation"},","),s(" token "),n("span",{class:"token punctuation"},"}"),n("span",{class:"token punctuation"},"}"),s(`
                `),n("span",{class:"token punctuation"},"}"),n("span",{class:"token punctuation"},";"),s(`
            `),n("span",{class:"token punctuation"},"}"),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token class-name"},[n("span",{class:"token keyword"},"var")]),s(" app "),n("span",{class:"token operator"},"="),s(" builder"),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"Build"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

`),n("span",{class:"token comment"},"// Configure the HTTP request pipeline."),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"UseHttpsRedirection"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"UseAuthorization"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"MapControllers"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`

app`),n("span",{class:"token punctuation"},"."),n("span",{class:"token function"},"Run"),n("span",{class:"token punctuation"},"("),n("span",{class:"token punctuation"},")"),n("span",{class:"token punctuation"},";"),s(`
`)])]),n("div",{class:"highlight-lines"},[n("div",{class:"highlight-line"}," "),n("br"),n("br"),n("br"),n("div",{class:"highlight-line"}," "),n("br"),n("br"),n("br"),n("br"),n("br"),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("div",{class:"highlight-line"}," "),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br"),n("br")]),n("div",{class:"line-numbers","aria-hidden":"true"},[n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"}),n("div",{class:"line-number"})])],-1),W=d(`<h3 id="startup代码" tabindex="-1"><a class="header-anchor" href="#startup代码" aria-hidden="true">#</a> Startup代码</h3><p><code>Headers</code> 设置请求头（调用事务重试接口时，传递JWT认证Token）</p><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">public</span> <span class="token return-type class-name"><span class="token keyword">void</span></span> <span class="token function">ConfigureServices</span><span class="token punctuation">(</span><span class="token class-name">IServiceCollection</span> services<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    services<span class="token punctuation">.</span><span class="token function">AddControllers</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
    services<span class="token punctuation">.</span><span class="token function">AddWing</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddJwt</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddPersistence</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddEventBus</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
            <span class="token punctuation">.</span><span class="token function">AddSaga</span><span class="token punctuation">(</span>serviceProvider <span class="token operator">=&gt;</span>
            <span class="token punctuation">{</span>
                <span class="token class-name"><span class="token keyword">var</span></span> token <span class="token operator">=</span> <span class="token interpolation-string"><span class="token string">$&quot;Bearer </span><span class="token interpolation"><span class="token punctuation">{</span><span class="token expression language-csharp">serviceProvider<span class="token punctuation">.</span><span class="token generic-method"><span class="token function">GetRequiredService</span><span class="token generic class-name"><span class="token punctuation">&lt;</span>IAuth<span class="token punctuation">&gt;</span></span></span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">.</span><span class="token function">GetToken</span><span class="token punctuation">(</span><span class="token punctuation">)</span></span><span class="token punctuation">}</span></span><span class="token string">&quot;</span></span><span class="token punctuation">;</span>
                <span class="token keyword">return</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name">SagaOptions</span>
                <span class="token punctuation">{</span>
                    Headers <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name">Dictionary<span class="token punctuation">&lt;</span><span class="token keyword">string</span><span class="token punctuation">,</span> <span class="token keyword">string</span><span class="token punctuation">&gt;</span></span> <span class="token punctuation">{</span> <span class="token punctuation">{</span> <span class="token string">&quot;Authorization&quot;</span><span class="token punctuation">,</span> token <span class="token punctuation">}</span><span class="token punctuation">}</span>
                <span class="token punctuation">}</span><span class="token punctuation">;</span>
            <span class="token punctuation">}</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>

<span class="token keyword">public</span> <span class="token keyword">class</span> <span class="token class-name">SagaOptions</span>
<span class="token punctuation">{</span>
    <span class="token doc-comment comment">/// <span class="token tag"><span class="token tag"><span class="token punctuation">&lt;</span>summary</span><span class="token punctuation">&gt;</span></span></span>
    <span class="token doc-comment comment">/// 设置请求头</span>
    <span class="token doc-comment comment">/// <span class="token tag"><span class="token tag"><span class="token punctuation">&lt;/</span>summary</span><span class="token punctuation">&gt;</span></span></span>
    <span class="token keyword">public</span> <span class="token return-type class-name">Dictionary<span class="token punctuation">&lt;</span><span class="token keyword">string</span><span class="token punctuation">,</span> <span class="token keyword">string</span><span class="token punctuation">&gt;</span></span> Headers <span class="token punctuation">{</span> <span class="token keyword">get</span><span class="token punctuation">;</span> <span class="token keyword">set</span><span class="token punctuation">;</span> <span class="token punctuation">}</span>
<span class="token punctuation">}</span>
</code></pre><div class="highlight-lines"><br><br><br><div class="highlight-line"> </div><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br><br></div><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="添加配置" tabindex="-1"><a class="header-anchor" href="#添加配置" aria-hidden="true">#</a> 添加配置</h3><p><code>Saga:RetrySeconds</code> 重试失败事务间隔，单位：秒，默认5分钟</p><p><code>Saga:ReportUpdateTime</code> 定时更新报表数据</p><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Saga&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token comment">// 每隔多少秒重试失败事务 默认5分钟</span>
    <span class="token property">&quot;RetrySeconds&quot;</span><span class="token operator">:</span> <span class="token number">60</span><span class="token punctuation">,</span>
    <span class="token comment">// 指定时间进行报表数据计算</span>
    <span class="token property">&quot;ReportUpdateTime&quot;</span><span class="token operator">:</span> <span class="token string">&quot;17:05&quot;</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,7);function C(q,x){const l=i("ExternalLinkIcon"),o=i("CodeTabs");return u(),r("div",null,[v,b,m,n("ul",null,[g,n("li",null,[n("p",null,[s("创建一个Web Api项目或者Work Service项目（生产环境建议使用），示例"),h,s("("),n("a",f,[s("点击查看完整示例代码"),c(l)]),s(")")])])]),c(o,{id:"21",data:[{id:".NET CLI"},{id:"Package Manager"}]},{title0:a(({value:t,isActive:e})=>[s(".NET CLI")]),title1:a(({value:t,isActive:e})=>[s("Package Manager")]),tab0:a(({value:t,isActive:e})=>[y]),tab1:a(({value:t,isActive:e})=>[_]),_:1},8,["data"]),S,c(o,{id:"32",data:[{id:".NET Core 3.1"},{id:".NET 6.0"}]},{title0:a(({value:t,isActive:e})=>[s(".NET Core 3.1")]),title1:a(({value:t,isActive:e})=>[s(".NET 6.0")]),tab0:a(({value:t,isActive:e})=>[w]),tab1:a(({value:t,isActive:e})=>[A]),_:1},8,["data"]),W])}const B=p(k,[["render",C],["__file","saga-server.html.vue"]]);export{B as default};
