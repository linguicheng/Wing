
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
  },
  SYS_USER_LIST (data = {}) {
    return request({
      method: 'POST',
      url: 'User/List',
      data
    })
  },
  SYS_USER_ADD (data = {}) {
    return request({
      method: 'POST',
      url: 'User/Add',
      data
    })
  },
  SYS_USER_UPDATE (data = {}) {
    return request({
      method: 'POST',
      url: 'User/Update',
      data
    })
  },
  SYS_USER_UPDATE_PASSWORD (data = {}) {
    return request({
      method: 'POST',
      url: 'User/UpdatePassword',
      data
    })
  },
  SYS_USER_RESET_PASSWORD (id) {
    return request({
      method: 'POST',
      url: `User/ResetPassword?id=${id}`
    })
  },
  SYS_USER_DELETE (id) {
    return request({
      method: 'POST',
      url: `User/Delete?id=${id}`
    })
  },
  SYS_USER_UNLOCKED (id) {
    return request({
      method: 'POST',
      url: `User/Unlocked?id=${id}`
    })
  },
  SYS_USER_UpdateTheme (data = {}) {
    return request({
      method: 'POST',
      url: 'User/UpdateTheme',
      data
    })
  }
})
