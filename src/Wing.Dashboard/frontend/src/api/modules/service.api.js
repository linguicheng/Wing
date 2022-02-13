
export default ({ request }) => ({
  /**
   * @description 查询服务列表
   */
  SERVICE_DETAIL (data = {}) {
    // 接口请求
    return request({
      method: 'GET',
      url: 'Service/Detail',
      params: data
    })
  },
  SERVICE (data = {}) {
    // 接口请求
    return request({
      method: 'GET',
      url: 'Service/List',
      params: data
    })
  }
})
