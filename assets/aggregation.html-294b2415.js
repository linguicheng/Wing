import{_ as t}from"./plugin-vue_export-helper-c27b6911.js";import{r as o,o as p,c as i,a as s,b as n,d as e,e as c}from"./app-1564c8ab.js";const l="/wing/assets/3.5-1-fec02e6a.png",r={},u=c(`<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p><code>网关聚合</code>可以将多个单独的请求聚合成一个请求返回给客户端。</p><h3 id="配置示例" tabindex="-1"><a class="header-anchor" href="#配置示例" aria-hidden="true">#</a> 配置示例</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Gateway&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
    <span class="token comment">// 自定义路由</span>
    <span class="token property">&quot;Routes&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span>
      <span class="token punctuation">{</span>
        <span class="token comment">// 上游配置</span>
        <span class="token property">&quot;Upstream&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
          <span class="token comment">// 请求Url</span>
          <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Aggregation&quot;</span><span class="token punctuation">,</span>
          <span class="token comment">// 请求方式</span>
          <span class="token property">&quot;Method&quot;</span><span class="token operator">:</span> <span class="token string">&quot;get&quot;</span>
        <span class="token punctuation">}</span><span class="token punctuation">,</span>
        <span class="token comment">// 下游配置</span>
        <span class="token property">&quot;Downstreams&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span>
          <span class="token punctuation">{</span>
            <span class="token comment">// 服务名称</span>
            <span class="token property">&quot;ServiceName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_3.2&quot;</span><span class="token punctuation">,</span>
            <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeatherForecast/Aggregation1&quot;</span><span class="token punctuation">,</span>
            <span class="token comment">// 请求方式</span>
            <span class="token property">&quot;Method&quot;</span><span class="token operator">:</span> <span class="token string">&quot;get&quot;</span><span class="token punctuation">,</span>
            <span class="token comment">// 聚合Key</span>
            <span class="token property">&quot;Key&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Aggregation1&quot;</span>
          <span class="token punctuation">}</span><span class="token punctuation">,</span>
          <span class="token punctuation">{</span>
            <span class="token comment">// 服务名称</span>
            <span class="token property">&quot;ServiceName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Wing.Demo_3.2&quot;</span><span class="token punctuation">,</span>
            <span class="token property">&quot;Url&quot;</span><span class="token operator">:</span> <span class="token string">&quot;WeatherForecast/Aggregation2&quot;</span><span class="token punctuation">,</span>
            <span class="token comment">// 请求方式</span>
            <span class="token property">&quot;Method&quot;</span><span class="token operator">:</span> <span class="token string">&quot;get&quot;</span><span class="token punctuation">,</span>
            <span class="token comment">// 聚合Key</span>
            <span class="token property">&quot;Key&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Aggregation2&quot;</span>
          <span class="token punctuation">}</span>
        <span class="token punctuation">]</span><span class="token punctuation">,</span>
        <span class="token comment">// 是否启用JWT认证</span>
        <span class="token property">&quot;UseJWTAuth&quot;</span><span class="token operator">:</span> <span class="token boolean">false</span><span class="token punctuation">,</span>
        <span class="token comment">// 固定Key认证</span>
        <span class="token property">&quot;AuthKey&quot;</span><span class="token operator">:</span> <span class="token string">&quot;&quot;</span>
      <span class="token punctuation">}</span>
    <span class="token punctuation">]</span><span class="token punctuation">,</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="查看运行结果" tabindex="-1"><a class="header-anchor" href="#查看运行结果" aria-hidden="true">#</a> 查看运行结果</h3>`,5),d={href:"https://gitee.com/linguicheng/wing-demo/tree/master/3.2",target:"_blank",rel:"noopener noreferrer"},m={href:"https://gitee.com/linguicheng/wing-demo/tree/master/3.5",target:"_blank",rel:"noopener noreferrer"},v={href:"http://localhost:3510/Aggregation",target:"_blank",rel:"noopener noreferrer"},k=s("figure",null,[s("img",{src:l,alt:"",tabindex:"0",loading:"lazy"}),s("figcaption")],-1);function g(q,b){const a=o("ExternalLinkIcon");return p(),i("div",null,[u,s("p",null,[n("分别启动API服务示例3.2("),s("a",d,[n("点击查看完整示例代码3.2"),e(a)]),n(")、网关示例3.5("),s("a",m,[n("点击查看完整示例代码3.5"),e(a)]),n(")，打开浏览器请求 "),s("a",v,[n("http://localhost:3510/Aggregation"),e(a)]),n(" ，返回结果如下图：")]),k])}const y=t(r,[["render",g],["__file","aggregation.html.vue"]]);export{y as default};
