import{_ as t}from"./plugin-vue_export-helper-c27b6911.js";import{r as o,o as p,c as i,a as s,b as n,d as e,e as l}from"./app-82e1a1f8.js";const c="/Wing/assets/3.4-1-f4429a9d.png",r={},u=l(`<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p><code>Wing</code>的服务网关支持自定义请求路由配置。</p><h3 id="自定义路由配置示例" tabindex="-1"><a class="header-anchor" href="#自定义路由配置示例" aria-hidden="true">#</a> 自定义路由配置示例</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
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

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行结果" tabindex="-1"><a class="header-anchor" href="#查看运行结果" aria-hidden="true">#</a> 查看运行结果</h3>`,5),d={href:"https://gitee.com/linguicheng/wing-demo/tree/master/3.2",target:"_blank",rel:"noopener noreferrer"},m={href:"https://gitee.com/linguicheng/wing-demo/tree/master/3.4",target:"_blank",rel:"noopener noreferrer"},v={href:"http://localhost:3410/test/hello/Wing",target:"_blank",rel:"noopener noreferrer"},k=s("figure",null,[s("img",{src:c,alt:"",tabindex:"0",loading:"lazy"}),s("figcaption")],-1);function h(b,q){const a=o("ExternalLinkIcon");return p(),i("div",null,[u,s("p",null,[n("分别启动API服务示例3.2("),s("a",d,[n("点击查看完整示例代码3.2"),e(a)]),n(")、网关示例3.4("),s("a",m,[n("点击查看完整示例代码3.4"),e(a)]),n(")，打开浏览器请求 "),s("a",v,[n("http://localhost:3410/test/hello/Wing"),e(a)]),n(" ，返回结果如下图：")]),k])}const f=t(r,[["render",h],["__file","custom-route.html.vue"]]);export{f as default};
