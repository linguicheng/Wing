
export default ({ request }) => ({
  /**
   * @description 指标统计
   * @returns
   */
  HOME_TARGETCOUNT () {
    return request({
      method: 'GET',
      url: 'Home/TargetCount'
    })
  }
})
