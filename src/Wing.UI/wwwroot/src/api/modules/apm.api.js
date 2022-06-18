
export default ({ request }) => ({
  /**
   * @description 查询外部请求
   * @param {*} data
   * @returns
   */
  TRACER_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Tracer/List',
      params: data
    })
  },
  /**
   * @description 查询内部请求
   * @param {*} data
   * @returns
   */
  HTTP_TRACER_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Tracer/HttpList',
      params: data
    })
  },
  /**
   * @description 查询内部Sql
   * @param {*} data
   * @returns
   */
  SQL_TRACER_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Tracer/SqlList',
      params: data
    })
  },
  /**
   * @description 查询http追踪明细
   * @param {*} data
   * @returns
   */
  HTTP_TRACER_DETAIL (traceId) {
    return request({
      method: 'GET',
      url: 'Tracer/GetHttpDetail',
      params: { traceId }
    })
  },
  /**
   * @description 查询sql追踪明细
   * @param {*} data
   * @returns
   */
  SQL_TRACER_DETAIL (traceId) {
    return request({
      method: 'GET',
      url: 'Tracer/GetSqlDetail',
      params: { traceId }
    })
  }
})
