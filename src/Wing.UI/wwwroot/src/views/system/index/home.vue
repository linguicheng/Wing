<template>
  <d2-container class="page">
    <el-row :gutter="24">
      <el-col :span="4">
        <el-card shadow="always">
          <div class="targetText">服务死亡节点总数</div>
          <div class="targetCount"><el-link type="danger" @click="toService()">{{targetCount.serviceCriticalTotal}}</el-link></div>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card shadow="always">
          <div class="targetText">网关请求超时总数</div>
          <div class="targetCount"><el-link type="danger" @click="toGateway()">{{targetCount.gatewayTimeoutTotal}}</el-link></div>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card shadow="always">
          <div class="targetText">Apm请求超时总数</div>
          <div class="targetCount"><el-link type="danger" @click="toApm()">{{targetCount.apmTimeoutTotal}}</el-link></div>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card shadow="always">
          <div class="targetText">Apm作业请求超时总数</div>
          <div class="targetCount"><el-link type="danger" @click="toApmHttp()">{{targetCount.apmWorkHttpTimeoutTotal}}</el-link></div>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card shadow="always">
          <div class="targetText">Apm作业Sql超时总数</div>
          <div class="targetCount"><el-link type="danger" @click="toApmSql()">{{targetCount.apmWorkSqlTimeoutTotal}}</el-link></div>
        </el-card>
      </el-col>
      <el-col :span="4">
        <el-card shadow="always">
          <div class="targetText">Saga事务失败总数</div>
          <div class="targetCount"><el-link type="danger" @click="toSaga()">{{targetCount.sagaFailedTotal}}</el-link></div>
        </el-card>
      </el-col>
    </el-row>
    <el-row :gutter="20" style="margin-top:10px;">
      <el-col el-col :span="8">
        <div style="height:300px;">
          <el-divider content-position="center">服务死亡排行</el-divider>
          <vxe-table ref="criticalLv"
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
                    :loading="serviceCritical.loading"
                    :data="serviceCritical.data"
                    :mouse-config="{ selected: true }">
            <vxe-column type="seq" width="60" title="序号" />
            <vxe-column field="serviceName"
                        title="服务名称"
                        align="left"/>
            <vxe-column field="total"
                        title="服务节点总数"
                        width="110"/>
            <vxe-column field="criticalTotal"
                        title="死亡节点总数"
                        width="110"/>
            <vxe-column field="criticalLv"
                        title="死亡率(%)"
                        width="100"/>
          </vxe-table>
        </div>
      </el-col>
      <el-col el-col :span="8">
        <div style="height:300px;">
          <el-divider content-position="center">网关超时排行</el-divider>
          <vxe-table ref="gateway"
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
                    :loading="gateway.loading"
                    :data="gateway.data"
                    :mouse-config="{ selected: true }">
            <vxe-column type="seq" width="60" title="序号" />
            <vxe-column field="serviceName"
                        title="服务名称"
                        align="left"/>
            <vxe-column field="requestUrl"
                        title="请求地址"
                        width="250"/>
            <vxe-column field="usedMillSeconds"
                        title="请求耗时(毫秒)"
                        width="120"/>
          </vxe-table>
        </div>
      </el-col>
      <el-col el-col :span="8">
          <div id="gatewayMonth" style="margin-top:20px;height:385px;"></div>
      </el-col>
    </el-row>
    <el-row :gutter="20" style="margin-top:10px;">
      <el-col el-col :span="24">
          <div id="sagaFaildData" style="margin-top:20px;height:385px;"></div>
      </el-col>
    </el-row>
  </d2-container>
</template>

<script>
import * as echarts from 'echarts'
export default {
  data () {
    return {
      serviceCritical: {
        loading: false,
        data: []
      },
      gateway: {
        loading: false,
        data: []
      },
      targetCount: {
        serviceCriticalTotal: 0,
        gatewayTimeoutTotal: 0,
        apmTimeoutTotal: 0,
        apmWorkHttpTimeoutTotal: 0,
        apmWorkSqlTimeoutTotal: 0,
        sagaFailedTotal: 0
      }
    }
  },
  async created () {
    await Promise.all([
      this.serviceCriticalLv(),
      this.gatewayTimeOut(),
      this.getTargetCount()])
  },
  async mounted () {
    await Promise.all([
      this.gatewayTimeoutMonth(),
      this.sagaFaildData()])
  },
  methods: {
    toGateway () {
      this.$router.push({
        path: '/gateWayLog'
      })
    },
    toService () {
      this.$router.push({
        path: '/service/detail'
      })
    },
    toApm () {
      this.$router.push({
        path: '/apm'
      })
    },
    toApmHttp () {
      this.$router.push({
        path: '/apm/http'
      })
    },
    toApmSql () {
      this.$router.push({
        path: '/apm/sql'
      })
    },
    toSaga () {
      this.$router.push({
        path: '/saga'
      })
    },
    async serviceCriticalLv () {
      this.serviceCritical.loading = true
      this.serviceCritical.data = await this.$api.SERVICE_CRITICAL_RANKING()
      this.serviceCritical.loading = false
    },
    async gatewayTimeOut () {
      this.gateway.loading = true
      this.gateway.data = await this.$api.GATEWAY_TIMEOUT_LIST()
      this.gateway.loading = false
    },
    async getTargetCount () {
      this.targetCount = await this.$api.HOME_TARGETCOUNT()
    },
    async gatewayTimeoutMonth () {
      const result = await this.$api.GATEWAY_TIMEOUT_MONTH()
      const xData = []
      const yData = []
      result.forEach(item => {
        xData.push(item.month)
        yData.push(item.count)
      })
      // 基于准备好的dom，初始化echarts实例
      const myChart = echarts.init(document.getElementById('gatewayMonth'))
      // 绘制图表
      myChart.setOption({
        title: {
          text: '最近一年网关超时统计'
        },
        tooltip: {},
        xAxis: {
          data: xData
        },
        yAxis: {},
        series: [
          {
            name: '超时总数',
            type: 'line',
            data: yData
          }
        ]
      })
    },
    async sagaFaildData () {
      const result = await this.$api.SAGA_FailedData()
      const xData = []
      const ySuccess = []
      const yExecuting = []
      const yFaild = []
      const yCancelled = []
      result.forEach(item => {
        xData.push(`${item.name}(${item.serviceName})`)
        ySuccess.push(item.successCount)
        yExecuting.push(item.executingCount)
        yFaild.push(item.faildCount)
        yCancelled.push(item.cancelledCount)
      })
      // 基于准备好的dom，初始化echarts实例
      const myChart = echarts.init(document.getElementById('sagaFaildData'))
      // 绘制图表
      myChart.setOption({
        title: {
          text: 'Saga事务失败率统计'
        },
        tooltip: {},
        xAxis: {
          data: xData
        },
        yAxis: {},
        series: [
          {
            name: '失败总数',
            type: 'bar',
            data: yFaild,
            stack: 'x'
          },
          {
            name: '取消总数',
            type: 'bar',
            data: yCancelled,
            stack: 'x'
          },
          {
            name: '成功总数',
            type: 'bar',
            data: ySuccess,
            stack: 'x'
          },
          {
            name: '执行中总数',
            type: 'bar',
            data: yExecuting,
            stack: 'x'
          }
        ]
      })
    }
  }
}
</script>

<style lang="scss" scoped>
.targetText{
  text-align:center;
}
.targetCount{
  text-align:center;
  margin-top: 20px;
}
.page {
  .logo {
    width: 120px;
  }
  .btn-group {
    color: $color-text-placehoder;
    font-size: 12px;
    line-height: 12px;
    margin-top: 0px;
    margin-bottom: 20px;
    .btn-group__btn {
      color: $color-text-sub;
      &:hover {
        color: $color-text-main;
      }
      &.btn-group__btn--link {
        color: $color-primary;
      }
    }
  }
}
</style>
