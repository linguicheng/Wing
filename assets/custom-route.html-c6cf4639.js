import{_ as e}from"./3.4-1-3101e58d.js";import{_ as t}from"./plugin-vue_export-helper-c27b6911.js";import{r as o,o as p,c as l,a as n,b as s,d as c,e as i}from"./app-3303d584.js";const r={},u=i(`<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p><code>Wing</code>的服务网关支持自定义请求路由配置。</p><h3 id="自定义路由配置示例" tabindex="-1"><a class="header-anchor" href="#自定义路由配置示例" aria-hidden="true">#</a> 自定义路由配置示例</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Gateway&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token comment">// 自定义路由</span>
   <span class="token property">&quot;Routes&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span>
     <span class="token punctuation">{</span>
       <span class="token comment">// 上游配置</span>
       <span class="token property">&quot;Upstream&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
         <span class="token comment">// 请求Url</span>
         <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;test/hello/{name}&quot;</span><span class="token punctuation">,</span>
         <span class="token comment">// 请求方式</span>
         <span class="token property">&quot;Method&quot;</span><span class="token operator">:</span> <span class="token string">&quot;get&quot;</span>
       <span class="token punctuation">}</span><span class="token punctuation">,</span>
       <span class="token comment">// 下游配置</span>
       <span class="token property">&quot;Downstreams&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span>
         <span class="token punctuation">{</span>
           <span class="token comment">// 服务名称</span>
           <span class="token property">&quot;ServiceName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_3.2&quot;</span><span class="token punctuation">,</span>
           <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeatherForecast/CustomRoute/{name}&quot;</span><span class="token punctuation">,</span>
           <span class="token comment">// 请求方式</span>
           <span class="token property">&quot;Method&quot;</span><span class="token operator">:</span> <span class="token string">&quot;get&quot;</span>
         <span class="token punctuation">}</span>
       <span class="token punctuation">]</span><span class="token punctuation">,</span>
       <span class="token comment">// 是否启用JWT认证</span>
       <span class="token property">&quot;UseJWTAuth&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span><span class="token punctuation">,</span>
       <span class="token comment">// 固定Key认证</span>
       <span class="token property">&quot;AuthKey&quot;</span><span class="token operator">:</span> <span class="token string">&quot;&quot;</span>
     <span class="token punctuation">}</span>
   <span class="token punctuation">]</span>
 <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行结果" tabindex="-1"><a class="header-anchor" href="#查看运行结果" aria-hidden="true">#</a> 查看运行结果</h3>`,5),d={href:"http://localhost:3410/test/hello/Wing",target:"_blank",rel:"noopener noreferrer"},m=n("figure",null,[n("img",{src:e,alt:"",tabindex:"0",loading:"lazy"}),n("figcaption")],-1);function v(k,b){const a=o("ExternalLinkIcon");return p(),l("div",null,[u,n("p",null,[s("分别启动API服务示例3.2、网关示例3.4，打开浏览器请求 "),n("a",d,[s("http://localhost:3410/test/hello/Wing"),c(a)]),s(" ，返回结果如下图：")]),m])}const g=t(r,[["render",v],["__file","custom-route.html.vue"]]);export{g as default};
