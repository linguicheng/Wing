
export default ({ request }) => ({
  /**
   * @description 查询服务明细
   */
  SERVICE_DETAIL (data = {}) {
    // 接口请求
    return request({
      method: 'GET',
      url: 'Service/Detail',
      params: data
    })
  },
  /**
   * @description 删除服务节点
   */
  SERVICE_DELETE (data = {}) {
    // 接口请求
    return request({
      method: 'GET',
      url: 'Service/Delete',
      params: data
    })
  },
  /**
   * @description 查询服务汇总
   * @param {*} data
   * @returns
   */
  SERVICE_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Service/List',
      params: data
    })
  }
})
