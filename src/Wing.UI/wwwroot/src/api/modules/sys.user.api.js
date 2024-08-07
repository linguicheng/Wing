
export default ({ request }) => ({
  /**
   * @description 用户登录
   */
  SYS_USER_LOGIN (data = {}) {
    return request({
      method: 'POST',
      url: 'User/Login',
      data
    })
  }
})
