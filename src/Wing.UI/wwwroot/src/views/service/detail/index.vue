<template>
  <div>
    <transition name="el-fade-in-linear">
      <keep-alive include="service-detail">
        <list v-if="order===0" @show="show" />
        <config v-else-if="order===1" @show="show" />
      </keep-alive>
    </transition>
  </div>
</template>

<script>
import list from './list.vue'
import config from './config.vue'

export default {
  name: 'service',
  components: {
    list,
    config
  },
  data () {
    return {
      order: 0
    }
  },
  methods: {
    show (order) {
      this.order = order
    }
  }
}
</script>
