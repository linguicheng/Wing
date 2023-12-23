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
     <div class="table-main">
        <vxe-table ref="mainTable"
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
                  @current-change="rowSelected"
                  :mouse-config="{ selected: true }">
          <vxe-column field="serviceName"
                      title="下游服务名称"
                      fixed="left"
                      width="200"
                      sortable/>
          <vxe-column field="requestUrl"
                        title="请求URL"
                        fixed="left"
                        align="left"
                        width="400"
                        sortable/>
          <vxe-column field="downstreamUrl"
                      title="下游地址"
                      width="150"
                      sortable/>
          <vxe-column field="gateWayServerIp"
                      title="网关服务器地址"
                      width="150"
                      sortable/>
          <vxe-column field="clientIp"
                      title="客户端请求IP"
                      width="150"
                      sortable/>
          <vxe-column field="serviceAddress"
                      title="下游服务地址"
                      width="200"
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
          <vxe-column field="policy"
                      title="网关策略"
                      width="300"/>
          <vxe-column field="authKey"
                      title="简单令牌"
                      width="300"/>
          <vxe-column field="token"
                      title="JWT令牌"
                      width="300"/>
          <vxe-column field="exception"
                      title="异常说明"
                      width="300"/>
        </vxe-table>
        <div style="padding:5px;">
          <el-pagination :current-page="pageModel.pageIndex"
                        :page-size="pageModel.pageSize"
                        :page-sizes="[15, 25, 35, 45]"
                        layout="total, sizes, prev, pager, next, jumper"
                        :total="result.totalCount"
                        @size-change="sizeChange"
                        @current-change="currentChange" />
        </div>
    </div>
    <div class="table-detail">
      <el-tabs type="border-card">
        <el-tab-pane label="服务聚合请求明细">
            <vxe-table ref="detailTable"
                    class="mytable-scrollbar"
                    border
                    resizable
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
                    :loading="detail.loading"
                    :data="detail.result"
                    :mouse-config="{ selected: true }">
              <vxe-column field="serviceName"
                          title="服务名称"
                          fixed="left"
                          width="200"
                          sortable/>
              <vxe-column field="requestUrl"
                            title="请求URL"
                            fixed="left"
                            align="left"
                            width="400"
                            sortable/>
              <vxe-column field="downstreamUrl"
                          title="下游地址"
                          width="150"
                          sortable/>
              <vxe-column field="gateWayServerIp"
                          title="网关服务器地址"
                          width="150"
                          sortable/>
              <vxe-column field="clientIp"
                          title="客户端请求IP"
                          width="150"
                          sortable/>
              <vxe-column field="serviceAddress"
                          title="下游服务地址"
                          width="200"
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
              <vxe-column field="policy"
                          title="网关策略"
                          width="300"/>
              <vxe-column field="authKey"
                          title="简单令牌"
                          width="300"/>
              <vxe-column field="token"
                          title="JWT令牌"
                          width="300"/>
              <vxe-column field="exception"
                          title="异常说明"
                          width="300"/>
            </vxe-table>
        </el-tab-pane>
      </el-tabs>
    </div>
  </d2-container>
</template>

<script>
export default {
  name: 'gateway',
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
      },
      detail: {
        loading: false,
        result: []
      }
    }
  },
  created () {
    this.search()
  },
  methods: {
    async rowSelected (e) {
      this.detail.loading = true
      this.detail.result = await this.$api.GATEWAY_DETAIL_LIST(e.row.id)
      this.detail.loading = false
    },
    async search () {
      this.loading = true
      if (this.requestTime) {
        this.pageModel.data.requestTime = this.requestTime
      } else {
        this.pageModel.data.requestTime = []
      }
      this.result = await this.$api.GATEWAY_LIST(this.pageModel)
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
