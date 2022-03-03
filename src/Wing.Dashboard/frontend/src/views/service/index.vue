<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-input v-model="pageModel.data"
                      clearable
                      @change="search"
                      placeholder="请输入服务名称"
                      style="width:200px"/>
            <el-button class="search"
                       type="primary"
                       icon="el-icon-search"
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
                 :edit-config="{
                  trigger: 'click',
                  mode: 'row',
                  showStatus: true,
                 }"
                 :loading="loading"
                 :data="services.items"
                 :mouse-config="{ selected: true }">
        <vxe-column field="name"
                    title="服务名称"
                    width="200"
                    sortable/>
        <vxe-column field="total"
                    title="服务节点总数"
                    sortable/>
        <vxe-column field="healthyTotal"
                    title="健康节点总数"
                    sortable/>
        <vxe-column field="healthyLv"
                    title="健康率(%)"
                    sortable/>
        <vxe-column field="criticalTotal"
                    title="死亡节点总数"
                    sortable/>
        <vxe-column field="criticalLv"
                    title="死亡率(%)"
                    sortable/>
        <vxe-column field="warningTotal"
                    title="警告节点总数"
                    sortable/>
        <vxe-column field="warningLv"
                    title="警告率(%)"
                    sortable/>
        <vxe-column field="maintenanceTotal"
                    title="维护节点总数"
                    sortable/>
        <vxe-column field="maintenanceLv"
                    title="维护率(%)"
                    sortable/>
        <vxe-column
          title="操作"
          width="150"
          align="center"
          fixed="right">
            <template #default="{ row }">
              <el-button type="primary" @click.prevent="toDetail(row)">查看明细</el-button>
            </template>
        </vxe-column>
      </vxe-table>
    </div>
    <template slot="footer">
      <el-pagination :current-page="pageModel.pageSize"
                     :page-size="pageModel.pageIndex"
                     :page-sizes="[15, 25, 35, 45]"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="services.totalCount"
                     @size-change="sizeChange"
                     @current-change="currentChange" />
    </template>
  </d2-container>
</template>

<script>
export default {
  name: 'service-list',
  data () {
    return {
      loading: false,
      services: [],
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: ''
      }
    }
  },
  created () {
    this.search()
  },
  methods: {
    toDetail (row) {
      this.$router.push({
        path: '/service/detail'
      })
      const timer = setTimeout(() => {
        this.Bus.emit('serviceNameToDetail', row.name)
        clearTimeout(timer)
      })
    },
    async search () {
      this.loading = true
      this.services = await this.$api.SERVICE(this.pageModel)
      this.loading = false
    },
    sizeChange (val) {
      this.pageModel.pageSize = val
      this.search()
    },
    currentChange (val) {
      this.pageModel.pageIndex = val
      this.search()
    }
  }
}
</script>
<style lang="scss" scoped>
.search{
  margin-left: 10px;
}
</style>
