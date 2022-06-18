
export default ({ request }) => ({
  /**
   * @description 查询服务汇总
   * @param {*} data
   * @returns
   */
  LOG_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Log/List',
      params: data
    })
  }
})
