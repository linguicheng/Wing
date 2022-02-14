<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-input v-model="pageModel.data.name"
                      clearable
                      @change="search"
                      placeholder="请输入服务名称"
                      style="width:200px"
                      class="search"/>
            <el-select
                v-model="pageModel.data.status"
                placeholder="请选择健康状态"
                @change="search"
                clearable
                class="search"
              >
                <el-option
                  v-for="item in statusOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                </el-option>
            </el-select>
            <el-select
                v-model="pageModel.data.serviceType"
                placeholder="请选择服务类别"
                @change="search"
                clearable
                class="search"
              >
                <el-option
                  v-for="item in serviceOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                </el-option>
            </el-select>
            <el-select
                v-model="pageModel.data.loadBalancer"
                placeholder="请选择负载均衡"
                @change="search"
                clearable
                class="search"
              >
                <el-option
                  v-for="item in loadBalancerOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                </el-option>
            </el-select>
            <el-button class="search" type="primary" icon="el-icon-search"
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
                    sortable/>
        <vxe-column field="address"
                    title="服务地址"
                    sortable/>
        <vxe-column field="status"
                    title="健康状态"
                    sortable>
           <template #default="{ row }">
              <el-tag v-if="row.status==0" type="success">健康</el-tag>
              <el-tag v-else-if="row.status==1" type="warning">警告</el-tag>
              <el-tag v-else-if="row.status==2" type="danger">死亡</el-tag>
              <el-tag v-else-if="row.status==3" type="warning">维护</el-tag>
           </template>
        </vxe-column>
        <vxe-column field="serviceType"
                    title="服务类别"
                    :formatter="['formatSelect', serviceOptions]"
                    sortable/>
        <vxe-column field="loadBalancer"
                    title="负载均衡"
                    :formatter="['formatSelect', loadBalancerOptions]"
                    sortable/>
        <vxe-column field="weight"
                    title="权重"
                    sortable/>
        <vxe-column field="developer"
                    title="负责人"
                    sortable/>
        <vxe-column
          title="操作"
          width="180"
          align="center"
          fixed="right">
            <template #default="{ row }">
              <el-popconfirm
                  title="确定要删除吗？"
                  @onConfirm="remove(row)">
                  <el-button slot="reference" type="danger">删除</el-button>
              </el-popconfirm>
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
  name: 'serviceDetail',
  data () {
    return {
      loading: false,
      services: [],
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: {
          name: '',
          serviceType: null,
          loadBalancer: null,
          status: null
        }
      },
      serviceOptions: [
        {
          value: 0,
          label: 'http'
        },
        {
          value: 1,
          label: 'grpc'
        }
      ],
      statusOptions: [
        {
          value: 0,
          label: '健康'
        },
        {
          value: 1,
          label: '警告'
        },
        {
          value: 2,
          label: '死亡'
        },
        {
          value: 3,
          label: '维护'
        }
      ],
      loadBalancerOptions: [
        {
          value: 0,
          label: '轮询'
        },
        {
          value: 1,
          label: '加权轮询'
        },
        {
          value: 2,
          label: '最小连接数'
        }
      ]
    }
  },
  mounted () {
    this.Bus.on('serviceNameSelected', val => {
      this.pageModel.data.name = val
      this.search()
    })
  },
  created () {
    this.search()
  },
  beforeDestroy () {
    this.Bus.off('serviceNameSelected')
  },
  methods: {
    async search () {
      this.loading = true
      this.services = await this.$api.SERVICE_DETAIL(this.pageModel)
      this.loading = false
    },
    async remove (row) {
      if (row.status !== 2) {
        this.$message.error('仅能删除状态为“已死亡”的服务节点！')
        return
      }
      var result = await this.$api.SERVICE_DELETE({ serviceId: row.id })
      if (result) {
        this.$message({
          type: 'success',
          message: '删除成功！'
        })
      } else {
        this.$message.error('删除失败，请稍后重试！')
      }
      await this.search()
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
