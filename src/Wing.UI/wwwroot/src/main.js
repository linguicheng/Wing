// Vue
import Vue from 'vue'
import i18n from './i18n'
import App from './App'
// 核心插件
import d2Admin from '@/plugin/d2admin'
// store
import store from '@/store/index'

// 菜单和路由设置
import router from './router'
import { menuHeader, menuAside } from '@/menu'
import { frameInRoutes } from '@/router/routes'

// vxe-table
import XEUtils from 'xe-utils'
import VXETable from 'vxe-table'
import 'vxe-table/lib/index.css'

import Bus from '@/libs/bus'
// import JsonViewer from 'vue-json-viewer'

// Vue.use(JsonViewer)
Vue.use(Bus)
// 核心插件
Vue.use(d2Admin)
Vue.use(VXETable)

// 自定义全局的格式化处理函数
VXETable.formats.mixin({
  // 格式化性别
  formatSex ({ cellValue }) {
    return cellValue ? (cellValue === '1' ? '男' : '女') : ''
  },
  // 格式化下拉选项
  formatSelect ({ cellValue }, list) {
    const item = list.find(item => item.value === cellValue)
    return item ? item.label : ''
  },
  // 格式化日期，默认 yyyy-MM-dd
  formatDate ({ cellValue }, format) {
    return XEUtils.toDateString(cellValue, format || 'yyyy-MM-dd')
  },
  // 四舍五入金额，每隔3位逗号分隔，默认2位数
  formatAmount ({ cellValue }, digits = 2) {
    return XEUtils.commafy(XEUtils.toNumber(cellValue), { digits })
  },
  // 格式化银行卡，默认每4位空格隔开
  formatBankcard ({ cellValue }) {
    return XEUtils.commafy(XEUtils.toValueString(cellValue), { spaceNumber: 4, separator: ' ' })
  },
  // 四舍五入,默认两位数
  formatFixedNumber ({ cellValue }, digits = 2) {
    return XEUtils.toFixed(XEUtils.round(cellValue, digits), digits)
  },
  // 向下舍入,默认两位数
  formatCutNumber ({ cellValue }, digits = 2) {
    return XEUtils.toFixed(XEUtils.floor(cellValue, digits), digits)
  }
})

new Vue({
  router,
  store,
  i18n,
  render: h => h(App),
  created () {
    // 处理路由 得到每一级的路由设置
    this.$store.commit('d2admin/page/init', frameInRoutes)
    // 设置顶栏菜单
    this.$store.commit('d2admin/menu/headerSet', menuHeader)
    // 设置侧边栏菜单
    this.$store.commit('d2admin/menu/asideSet', menuAside)
    // 初始化菜单搜索功能
    this.$store.commit('d2admin/search/init', menuHeader)
  },
  mounted () {
    // 展示系统信息
    this.$store.commit('d2admin/releases/versionShow')
    // 用户登录后从数据库加载一系列的设置
    this.$store.dispatch('d2admin/account/load')
    // 获取并记录用户 UA
    this.$store.commit('d2admin/ua/get')
    // 初始化全屏监听
    this.$store.dispatch('d2admin/fullscreen/listen')
  }
}).$mount('#app')
