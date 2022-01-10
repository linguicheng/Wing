<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-button type="primary" icon="el-icon-search"
            @click.prevent="search"
            >查询</el-button>
        </div>
    </template>
     <div class="table-container">
      <vxe-table ref="xTable"
                 border
                 resizable
                 height="100%"
                 size="medium"
                 align="center"
                 highlight-hover-row
                 highlight-current-row
                 show-overflow
                 auto-resize
                 keep-source
                 stripe
                 :loading="loading"
                 :data="services"
                 :mouse-config="{ selected: true }">
        <vxe-table-column field="name"
                          title="服务名称"
                          :edit-render="{name: '$input', props: {readonly: true}}" />
        <vxe-table-column field="serviceAddress"
                          title="服务地址"
                          :edit-render="{name: '$input', props: {readonly: true}}" />
        <vxe-table-column field="serviceType"
                          title="服务类别"
                          :edit-render="{name: '$input', props: {readonly: true}}" />
        <vxe-table-column field="loadBanlancer"
                          title="负载均衡"
                          :edit-render="{name: '$input', props: {readonly: true}}" />
      </vxe-table>
    </div>
    <!-- <template slot="footer">
      <el-pagination :current-page="formData.Page"
                     :page-size="formData.Limit"
                     :page-sizes="[15, 25, 35, 45]"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="totalPage"
                     @size-change="handleSizeChange"
                     @current-change="handleCurrentChange" />
    </template> -->
  </d2-container>
</template>

<script>
export default {
  name: 'service',
  data () {
    return {
      loading: false,
      services: []
    }
  },
  created () {
    this.search()
  },
  methods: {
    async search () {
      this.loading = true
      this.services = await this.$api.SERVICE_LIST()
      this.loading = false
    },
    edit () {

    },
    remove () {

    }
  }
}
</script>
