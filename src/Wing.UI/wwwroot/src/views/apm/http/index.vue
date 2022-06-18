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
                v-model="pageModel.data.requestUrl"
                clearable
                @change="search"
                placeholder="请输入请求地址"
                class="search"
                style="width:200px"/>
            <el-date-picker
                v-model="requestTime"
                type="datetimerange"
                class="search"
                @change="search"
                :picker-options="pickerOptions"
                range-separator="至"
                start-placeholder="请求时间"
                end-placeholder="请求时间">
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
        <vxe-table ref="httpTable"
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
          <vxe-column field="requestUrl"
                        title="请求地址"
                        fixed="left"
                        align="left"
                        width="400"
                        sortable/>
          <vxe-column field="serviceName"
                      title="服务名称"
                      width="200"
                      sortable/>
          <vxe-column field="serverIp"
                      title="服务端IP"
                      width="150"
                      sortable/>
          <vxe-column field="serviceUrl"
                      title="服务地址"
                      width="200"
                      sortable/>
          <vxe-column field="requestType"
                      title="请求类型"
                      width="120"
                      sortable/>
          <vxe-column field="requestTime"
                      title="请求时间"
                      width="200"
                      :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
                      sortable/>
          <vxe-column field="responseTime"
                      title="响应时间"
                      width="200"
                      :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
                      sortable/>
          <vxe-column field="usedMillSeconds"
                      title="请求耗时(毫秒)"
                      width="150"
                      sortable/>
          <vxe-column field="requestMethod"
                      title="请求方式"
                      width="120"
                      sortable/>
          <vxe-column field="requestValue"
                      title="请求内容"
                      width="300"/>
          <vxe-column field="responseValue"
                      title="响应内容"
                      width="300"/>
          <vxe-column field="statusCode"
                      title="状态码"
                      width="100"
                      sortable/>
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
  name: 'http-tracer',
  data () {
    return {
      loading: false,
      result: [],
      requestTime: '',
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: {
          serviceName: '',
          requestUrl: '',
          requestTime: []
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
      if (this.requestTime) {
        this.pageModel.data.requestTime = this.requestTime
      } else {
        this.pageModel.data.requestTime = []
      }
      this.result = await this.$api.HTTP_TRACER_LIST(this.pageModel)
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
