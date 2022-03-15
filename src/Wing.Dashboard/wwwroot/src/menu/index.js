import { uniqueId } from 'lodash'

/**
 * @description 给菜单数据补充上 path 字段
 * @description https://github.com/d2-projects/d2-admin/issues/209
 * @param {Array} menu 原始的菜单数据
 */
function supplementPath (menu) {
  return menu.map(e => ({
    ...e,
    path: e.path || uniqueId('d2-menu-empty-'),
    ...e.children ? {
      children: supplementPath(e.children)
    } : {}
  }))
}

export const menuHeader = supplementPath([
])

export const menuAside = supplementPath([
  { path: '/index', title: '首页', icon: 'home' },
  {
    title: '服务管理',
    icon: 'folder-o',
    children: [
      { path: '/service', title: '服务汇总' },
      { path: '/service/detail', title: '服务明细' }
    ]
  },
  { path: '/config', title: '配置中心', icon: 'folder-o' },
  { path: '/gateWayLog', title: '网关日志', icon: 'folder-o' }
])
