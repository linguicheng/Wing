
export default ({ request }) => ({
  /**
   * @description 查询网关请求信息
   * @param {*} data
   * @returns
   */
  GATEWAY_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Gateway/List',
      params: data
    })
  },
  GATEWAY_TIMEOUT_LIST () {
    return request({
      method: 'GET',
      url: 'Gateway/TimeoutList'
    })
  },
  GATEWAY_TIMEOUT_MONTH () {
    return request({
      method: 'GET',
      url: 'Gateway/TimeoutMonth'
    })
  }
})
