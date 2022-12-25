<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-input
                v-model="pageModel.data.name"
                clearable
                @change="search"
                placeholder="请输入事务名称"
                class="search"
                style="width:200px"/>
            <el-input
                v-model="pageModel.data.serviceName"
                clearable
                @change="search"
                placeholder="请输入服务名称"
                class="search"
                style="width:200px"/>
            <el-date-picker
                v-model="createdTime"
                type="datetimerange"
                class="search"
                @change="search"
                :picker-options="pickerOptions"
                range-separator="至"
                start-placeholder="创建时间"
                end-placeholder="创建时间">
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
                  :mouse-config="{ selected: true }"
                  :tree-config="{transform: true}">
          <vxe-column field="name"
                      title="事务名称"
                      width="200"
                      fixed="left"
                      align="left"
                      tree-node
                      sortable/>
          <vxe-column field="status"
                      title="事务状态"
                      fixed="left"
                      width="120"
                      sortable>
           <template #default="{ row }">
              <el-tag v-if="row.status==0" type="success">执行成功</el-tag>
              <el-tag v-else-if="row.status==1" type="info">执行中</el-tag>
              <el-tag v-else-if="row.status==2" type="danger">执行失败</el-tag>
              <el-tag v-else-if="row.status==3" type="warning">已取消</el-tag>
           </template>
          </vxe-column>
          <vxe-column field="retryResult"
                    title="重试结果"
                    width="120"
                    sortable>
           <template #default="{ row }">
              <el-tag v-if="row.retryResult==1" type="success">成功</el-tag>
              <el-tag v-else-if="row.retryResult==2" type="danger">失败</el-tag>
           </template>
          </vxe-column>
          <vxe-column field="retryAction"
                      title="重试动作"
                      width="120"
                      sortable/>
          <vxe-column field="policy"
                    title="执行策略"
                    width="120"
                    :formatter="['formatSelect', policyOptions]"
                    sortable/>
          <vxe-column field="committedCount"
                      title="向前恢复次数"
                      width="200"
                      sortable/>
          <vxe-column field="cancelledCount"
                      title="向后恢复次数"
                      width="200"
                      sortable/>
          <vxe-column field="breakerCount"
                      title="熔断条件"
                      width="120"
                      sortable/>
          <vxe-column field="customCount"
                      title="先前再后条件"
                      width="200"
                      sortable/>
          <vxe-column field="serviceName"
                      title="服务名称"
                      width="200"
                      sortable/>
          <vxe-column field="serviceType"
                    title="服务类别"
                    width="120"
                    :formatter="['formatSelect', serviceOptions]"
                    sortable/>
          <vxe-column field="createdTime"
                      title="创建时间"
                      width="200"
                      :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
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
          <vxe-column field="description"
                      title="说明"
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
              :loading="detailLoading"
              :data="detailData"
              :mouse-config="{ selected: true }">
            <vxe-column field="orderNo"
                        title="序号"
                        fixed="left"
                        width="100"
                        sortable/>
            <vxe-column field="name"
                        title="事务单元"
                        fixed="left"
                        width="180"
                        sortable/>
            <vxe-column field="status"
                        title="事务状态"
                        fixed="left"
                        width="120"
                        sortable>
              <template #default="{ row }">
                  <el-tag v-if="row.status==0" type="success">执行成功</el-tag>
                  <el-tag v-else-if="row.status==1" type="info">执行中</el-tag>
                  <el-tag v-else-if="row.status==2" type="danger">执行失败</el-tag>
                  <el-tag v-else-if="row.status==3" type="warning">已取消</el-tag>
              </template>
            </vxe-column>
            <vxe-column field="retryResult"
                    title="重试结果"
                    width="120"
                    sortable>
           <template #default="{ row }">
              <el-tag v-if="row.retryResult==1" type="success">成功</el-tag>
              <el-tag v-else-if="row.retryResult==2" type="danger">失败</el-tag>
           </template>
          </vxe-column>
          <vxe-column field="retryAction"
                      title="重试动作"
                      width="120"
                      sortable/>
            <vxe-column field="committedCount"
                        title="向前恢复次数"
                        width="200"
                        sortable/>
            <vxe-column field="cancelledCount"
                        title="向后恢复次数"
                        width="200"
                        sortable/>
          <vxe-column field="createdTime"
                      title="创建时间"
                      width="200"
                      :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
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
          <vxe-column field="description"
                      title="说明"
                      width="100"/>
          <vxe-column field="errorMsg"
                      title="异常情况"
                      width="300"/>
      </vxe-table>
    </div>
  </d2-container>
</template>

<script>
export default {
  name: 'saga',
  data () {
    return {
      loading: false,
      detailLoading: false,
      result: [],
      detailData: [],
      createdTime: '',
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: {
          serviceName: '',
          name: '',
          createdTime: []
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
      policyOptions: [
        {
          value: 0,
          label: '向前恢复'
        },
        {
          value: 1,
          label: '向后恢复'
        },
        {
          value: 2,
          label: '先前再后'
        }
      ],
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
    async rowSelected (e) {
      await this.detail(e.row.id)
    },
    async search () {
      this.loading = true
      if (this.createdTime) {
        this.pageModel.data.createdTime = this.createdTime
      } else {
        this.pageModel.data.createdTime = []
      }
      this.result = await this.$api.SAGA_LIST(this.pageModel)
      this.loading = false
    },
    async detail (tranId) {
      this.detailLoading = true
      this.detailData = await this.$api.SAGA_DETAIL(tranId)
      this.detailLoading = false
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
