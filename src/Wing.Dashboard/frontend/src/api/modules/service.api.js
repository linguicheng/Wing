
export default ({ request }) => ({
  /**
   * @description 查询服务列表
   */
  SERVICE_LIST () {
    // 接口请求
    return request({
      method: 'GET',
      url: 'Service'
    })
  }
})
