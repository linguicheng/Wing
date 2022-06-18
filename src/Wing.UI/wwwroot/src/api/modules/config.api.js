
export default ({ request }) => ({
  /**
   * @description 查询服务配置
   */
  CONFIG_GET (data = {}) {
    return request({
      method: 'GET',
      url: 'Config/Get',
      params: data
    })
  },
  CONFIG_LIST (data = {}) {
    return request({
      method: 'GET',
      url: 'Config/List',
      params: data
    })
  },
  /**
   * @description 删除服务配置
   */
  CONFIG_DELETE (data = {}) {
    // 接口请求
    return request({
      method: 'GET',
      url: 'Config/Delete',
      params: data
    })
  },
  /**
   * @description 保存服务配置
   */
  CONFIG_SAVE (data = {}) {
    return request({
      method: 'POST',
      url: 'Config/Save',
      data
    })
  }
})
