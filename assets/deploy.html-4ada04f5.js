import{_ as a}from"./plugin-vue_export-helper-c27b6911.js";import{o as e,c as i,e as n}from"./app-3303d584.js";const r={},t=n('<p>首先，把需要用到的中间件Consul(必选)、RabbitMQ(可选)、数据库(必选)部署好，Wing数据库建议单独一个库，不要跟你的业务库混在一起。</p><h3 id="部署wing后台管理-wing-ui" tabindex="-1"><a class="header-anchor" href="#部署wing后台管理-wing-ui" aria-hidden="true">#</a> 部署Wing后台管理-Wing.UI</h3><p>建议单独部署一个Web Api服务，不要混合其他业务功能，这是管理所有服务的可视化操作界面。</p><h3 id="部署网关-wing-gateway" tabindex="-1"><a class="header-anchor" href="#部署网关-wing-gateway" aria-hidden="true">#</a> 部署网关-Wing.Gateway</h3><p>建议单独部署一个Web Api服务，不要混合其他业务功能, 并且要保证资源充足，因为很多业务请求要通过它来转发，必要的时候可以用nginx来做负载均衡。</p><h3 id="部署saga事务协调器-wing-saga-server" tabindex="-1"><a class="header-anchor" href="#部署saga事务协调器-wing-saga-server" aria-hidden="true">#</a> 部署Saga事务协调器-Wing.Saga.Server</h3><p>如果有用到Saga分布式事务，就需要部署该服务，建议部署成Windows服务或linux守护进程，不要混合其他业务功能。</p>',7),s=[t];function d(o,c){return e(),i("div",null,s)}const p=a(r,[["render",d],["__file","deploy.html.vue"]]);export{p as default};
