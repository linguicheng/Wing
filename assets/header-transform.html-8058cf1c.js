import{_ as n}from"./plugin-vue_export-helper-c27b6911.js";import{o as s,c as a,e as t}from"./app-4ef98c74.js";const e={},o=t(`<h3 id="简介" tabindex="-1"><a class="header-anchor" href="#简介" aria-hidden="true">#</a> 简介</h3><p>可配置请求头转发或不转发到下游服务或者配置响应头返回客户端。</p><h3 id="配置示例" tabindex="-1"><a class="header-anchor" href="#配置示例" aria-hidden="true">#</a> 配置示例</h3><div class="language-json line-numbers-mode" data-ext="json"><pre class="language-json"><code><span class="token punctuation">{</span>
  <span class="token property">&quot;Gateway&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
     <span class="token comment">//请求头/响应头转发</span>
    <span class="token property">&quot;HeadersTransform&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
      <span class="token comment">// 请求头转发到下游服务</span>
      <span class="token property">&quot;Request&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">// 全局</span>
        <span class="token property">&quot;Global&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
          <span class="token property">&quot;aa&quot;</span><span class="token operator">:</span> <span class="token string">&quot;bb&quot;</span>
        <span class="token punctuation">}</span><span class="token punctuation">,</span>
        <span class="token comment">// 指定下游服务</span>
        <span class="token property">&quot;Services&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span>
          <span class="token punctuation">{</span>
            <span class="token property">&quot;ServiceName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Sample.AspNetCoreService&quot;</span><span class="token punctuation">,</span>
            <span class="token property">&quot;Headers&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span> <span class="token property">&quot;cc&quot;</span><span class="token operator">:</span> <span class="token string">&quot;dd&quot;</span> <span class="token punctuation">}</span>
          <span class="token punctuation">}</span>
        <span class="token punctuation">]</span>
      <span class="token punctuation">}</span><span class="token punctuation">,</span>
      <span class="token comment">// 响应头返回给前端</span>
      <span class="token property">&quot;Response&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
        <span class="token comment">// 全局</span>
        <span class="token property">&quot;Global&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span>
          <span class="token property">&quot;ee&quot;</span><span class="token operator">:</span> <span class="token string">&quot;ff&quot;</span>
        <span class="token punctuation">}</span><span class="token punctuation">,</span>
        <span class="token comment">// 指定下游服务</span>
        <span class="token property">&quot;Services&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span>
          <span class="token punctuation">{</span>
            <span class="token property">&quot;ServiceName&quot;</span><span class="token operator">:</span> <span class="token string">&quot;Sample.AspNetCoreService&quot;</span><span class="token punctuation">,</span>
            <span class="token property">&quot;Headers&quot;</span><span class="token operator">:</span> <span class="token punctuation">{</span> <span class="token property">&quot;gg&quot;</span><span class="token operator">:</span> <span class="token string">&quot;hh&quot;</span> <span class="token punctuation">}</span>
          <span class="token punctuation">}</span>
        <span class="token punctuation">]</span>
      <span class="token punctuation">}</span>
    <span class="token punctuation">}</span><span class="token punctuation">,</span>
  <span class="token comment">//不转发请求头</span>
  <span class="token property">&quot;DoNotTransformHeaders&quot;</span><span class="token operator">:</span> <span class="token punctuation">[</span> 
    <span class="token string">&quot;connection&quot;</span><span class="token punctuation">,</span> 
    <span class="token string">&quot;user-agent&quot;</span><span class="token punctuation">,</span> 
    <span class="token string">&quot;content-length&quot;</span><span class="token punctuation">,</span> 
    <span class="token string">&quot;origin&quot;</span><span class="token punctuation">,</span> 
    <span class="token string">&quot;accept-encoding&quot;</span><span class="token punctuation">,</span> 
    <span class="token string">&quot;host&quot;</span> <span class="token punctuation">]</span><span class="token punctuation">,</span>
  <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,4),p=[o];function c(i,l){return s(),a("div",null,p)}const d=n(e,[["render",c],["__file","header-transform.html.vue"]]);export{d as default};
