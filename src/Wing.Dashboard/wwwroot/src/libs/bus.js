let _Vue = null
const install = (Vue) => {
  if (Vue && _Vue !== Vue) {
    _Vue = Vue
    class Bus {
      constructor () {
        this._vueIns = new Vue()
      }

      emit (event, ...args) {
        this._vueIns.$emit(event, ...args)
      }

      on (event, cb) {
        this._vueIns.$on(event, cb)
      }

      off (event, cb) {
        this._vueIns.$off(event, cb)
      }
    }
    Vue.prototype.Bus = new Bus()
  }
}

export default install
