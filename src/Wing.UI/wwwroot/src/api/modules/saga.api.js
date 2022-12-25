
export default ({ request }) => ({
  /**
   * @description 查询事务列表
   * @param {*} data
   * @returns
   */
  SAGA_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Saga/List',
      params: data
    })
  },
  /**
   * @description 查询事务单元明细
   * @param {*} data
   * @returns
   */
  SAGA_DETAIL (tranId) {
    return request({
      method: 'GET',
      url: 'Saga/Detail',
      params: { tranId }
    })
  }
})
