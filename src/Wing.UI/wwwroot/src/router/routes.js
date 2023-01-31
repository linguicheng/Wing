import layoutHeaderAside from '@/layout/header-aside'

// 由于懒加载页面太多的话会造成webpack热更新太慢，所以开发环境不使用懒加载，只有生产环境使用懒加载
const _import = require('@/libs/util.import.' + process.env.NODE_ENV)

/**
 * 在主框架内显示
 */
const frameIn = [
  {
    path: '/',
    redirect: { name: 'index' },
    component: layoutHeaderAside,
    children: [
      // 首页
      {
        path: 'index',
        name: 'index',
        meta: {
          auth: true
        },
        component: _import('system/index')
      },
      {
        path: 'service/detail',
        name: 'service-detail',
        meta: {
          title: '服务明细',
          auth: true
        },
        component: _import('service/detail')
      },
      {
        path: 'service',
        name: 'service',
        meta: {
          title: '服务汇总',
          auth: true
        },
        component: _import('service')
      },
      {
        path: 'config',
        name: 'config',
        meta: {
          title: '配置中心',
          auth: true
        },
        component: _import('config')
      },
      {
        path: 'gateWayLog',
        name: 'gateWayLog',
        meta: {
          title: '网关日志',
          auth: true
        },
        component: _import('log')
      },
      {
        path: 'apm',
        name: 'tracer',
        meta: {
          title: '请求追踪',
          auth: true
        },
        component: _import('apm')
      },
      {
        path: 'apm/http',
        name: 'http-tracer',
        meta: {
          title: '作业请求追踪',
          auth: true
        },
        component: _import('apm/http')
      },
      {
        path: 'apm/sql',
        name: 'sql-tracer',
        meta: {
          title: '作业Sql追踪',
          auth: true
        },
        component: _import('apm/sql')
      },
      {
        path: 'saga',
        name: 'saga',
        meta: {
          title: 'Saga分布式事务',
          auth: true
        },
        component: _import('saga')
      },
      // 系统 前端日志
      {
        path: 'log',
        name: 'log',
        meta: {
          title: '前端日志',
          auth: true
        },
        component: _import('system/log')
      },
      // 刷新页面 必须保留
      {
        path: 'refresh',
        name: 'refresh',
        hidden: true,
        component: _import('system/function/refresh')
      },
      // 页面重定向 必须保留
      {
        path: 'redirect/:route*',
        name: 'redirect',
        hidden: true,
        component: _import('system/function/redirect')
      }
    ]
  }
]

/**
 * 在主框架之外显示
 */
const frameOut = [
  // 登录
  {
    path: '/login',
    name: 'login',
    component: _import('system/login')
  }
]

/**
 * 错误页面
 */
const errorPage = [
  {
    path: '*',
    name: '404',
    component: _import('system/error/404')
  }
]

// 导出需要显示菜单的
export const frameInRoutes = frameIn

// 重新组织后导出
export default [
  ...frameIn,
  ...frameOut,
  ...errorPage
]
