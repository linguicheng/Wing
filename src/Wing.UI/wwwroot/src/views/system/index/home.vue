<template>
  <d2-container class="page">
    <!-- <div id="criticalLv" style="width: 600;height:400px;"></div> -->
    <div style="width: 600;height:400px;">
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
                    width="200"/>
        <vxe-column field="total"
                    title="服务节点总数"/>
        <vxe-column field="criticalTotal"
                    title="死亡节点总数"
                    sortable/>
        <vxe-column field="criticalLv"
                    title="死亡率(%)"/>
      </vxe-table>
    </div>
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
      }
    }
  },
  created () {
    this.serviceCriticalLv()
  },
  mounted () {

  },
  methods: {
    async serviceCriticalLv () {
      this.serviceCritical.data = await this.$api.SERVICE_CRITICAL_RANKING()
    },
    async demo () {
      const criticalLv = await this.$api.SERVICE_CRITICAL_RANKING()
      const xData = []
      const yData = []
      criticalLv.forEach(item => {
        xData.push(item.serviceName)
        yData.push(item.criticalLv)
      })
      // 基于准备好的dom，初始化echarts实例
      const myChart = echarts.init(document.getElementById('criticalLv'))
      // 绘制图表
      myChart.setOption({
        title: {
          text: '服务死亡率'
        },
        tooltip: {},
        xAxis: {
          data: xData
        },
        yAxis: {},
        series: [
          {
            name: '死亡率',
            type: 'bar',
            data: yData
          }
        ]
      })
    }
  }
}
</script>

<style lang="scss" scoped>
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
