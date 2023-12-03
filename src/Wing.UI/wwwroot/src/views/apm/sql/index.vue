<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-input
                v-model="pageModel.data.serviceName"
                clearable
                @change="search"
                placeholder="请输入服务名称"
                class="search"
                style="width:200px"/>
            <el-input
                v-model="pageModel.data.action"
                clearable
                @change="search"
                placeholder="请输入执行动作"
                class="search"
                style="width:200px"/>
            <el-date-picker
                v-model="beginTime"
                type="datetimerange"
                class="search"
                @change="search"
                :picker-options="pickerOptions"
                range-separator="至"
                start-placeholder="开始执行时间"
                end-placeholder="开始执行时间">
            </el-date-picker>
            <el-button
                class="search"
                type="primary"
                icon="el-icon-search"
                @click.prevent="search">查询
            </el-button>
        </div>
    </template>
     <div class="table-container">
        <vxe-table ref="sqlTable"
                  class="mytable-scrollbar"
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
                  :data="result.items"
                  :mouse-config="{ selected: true }">
          <vxe-column field="serviceName"
                      title="服务名称"
                      fixed="left"
                      width="200"
                      sortable/>
          <vxe-column field="serviceUrl"
                      title="服务地址"
                      width="200"
                      sortable/>
          <vxe-column field="action"
                      title="执行动作"
                      width="120"
                      sortable/>
          <vxe-column field="beginTime"
                      title="开始执行时间"
                      width="200"
                      :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
                      sortable/>
          <vxe-column field="endTime"
                      title="结束执行时间"
                      width="200"
                      :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
                      sortable/>
          <vxe-column field="usedMillSeconds"
                      title="执行耗时(毫秒)"
                      width="150"
                      sortable/>
          <vxe-column field="sql"
                      title="执行Sql"
                      width="300"/>
          <vxe-column field="exception"
                      title="异常说明"
                      width="300"/>
        </vxe-table>
    </div>
    <template slot="footer">
      <el-pagination :current-page="pageModel.pageIndex"
                     :page-size="pageModel.pageSize"
                     :page-sizes="[15, 25, 35, 45]"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="result.totalCount"
                     @size-change="sizeChange"
                     @current-change="currentChange" />
    </template>
  </d2-container>
</template>

<script>
export default {
  name: 'sql-tracer',
  data () {
    return {
      loading: false,
      result: [],
      beginTime: '',
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: {
          serviceName: '',
          action: '',
          beginTime: []
        }
      },
      pickerOptions: {
        shortcuts: [{
          text: '最近一小时',
          onClick (picker) {
            const end = new Date()
            const start = new Date()
            start.setTime(start.getTime() - 3600 * 1000)
            picker.$emit('pick', [start, end])
          }
        }, {
          text: '最近一天',
          onClick (picker) {
            const end = new Date()
            const start = new Date()
            start.setTime(start.getTime() - 3600 * 1000 * 24)
            picker.$emit('pick', [start, end])
          }
        }, {
          text: '最近一周',
          onClick (picker) {
            const end = new Date()
            const start = new Date()
            start.setTime(start.getTime() - 3600 * 1000 * 24 * 7)
            picker.$emit('pick', [start, end])
          }
        }]
      }
    }
  },
  created () {
    this.search()
  },
  methods: {
    async search () {
      this.loading = true
      if (this.beginTime) {
        this.pageModel.data.beginTime = this.beginTime
      } else {
        this.pageModel.data.beginTime = []
      }
      this.result = await this.$api.SQL_TRACER_LIST(this.pageModel)
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
