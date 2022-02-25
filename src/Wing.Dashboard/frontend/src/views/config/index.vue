<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-input v-model="serviceName"
                      clearable
                      @change="search"
                      placeholder="请输入服务名称"
                      style="width:200px"/>
            <el-button class="search"
                       type="primary"
                       icon="el-icon-search"
                       @click.prevent="search"
            >查询</el-button>
            <el-button type="primary" :loading="saveLoading" @click="save()">保存</el-button>
            <el-button type="success" icon="el-icon-plus" @click="add()">新增</el-button>
            <el-popconfirm style="margin-left:10px;"
                  title="确定要删除吗？"
                  @onConfirm="remove()">
                  <el-button slot="reference" type="danger">删除</el-button>
            </el-popconfirm>
        </div>
    </template>
    <div>
       <el-row>
          <el-col :span="5">
            <div class="table-container">
                <vxe-table ref="config"
                      border
                      resizable
                      height="100%"
                      size="medium"
                      align="left"
                      highlight-hover-row
                      highlight-current-row
                      show-overflow
                      auto-resize
                      keep-source
                      stripe
                      :edit-config="{trigger: 'click',mode: 'cell'}"
                      :loading="loading"
                      :data="configData"
                      @current-change="rowChange"
                      :mouse-config="{ selected: true }">
                  <vxe-column field="key"
                              title="配置Key"
                              :edit-render="{autofocus: '.vxe-input--inner'}">
                      <template #edit="{ row }">
                        <vxe-input v-model="row.key" type="text"></vxe-input>
                      </template>
                  </vxe-column>
                </vxe-table>
            </div>
          </el-col>
          <el-col :span="19">
            <div style="margin-left: 20px;">
              <el-input
                type="textarea"
                :rows="28"
                :placeholder="placeholder"
                :disabled="disabled"
                v-model="currentRow.value">
              </el-input>
            </div>
          </el-col>
       </el-row>
    </div>
    <template slot="footer">
        <el-pagination :current-page="pageModel.pageSize"
                      :page-size="pageModel.pageIndex"
                      :page-sizes="[15, 25, 35, 45]"
                      layout="total, sizes, prev, pager, next, jumper"
                      :total="totalCount"
                      @size-change="sizeChange"
                      @current-change="currentChange" />
    </template>
 </d2-container>
</template>

<script>
export default {
  name: 'service-config',
  data () {
    return {
      saveLoading: false,
      loading: false,
      services: [],
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: ''
      },
      disabled: true,
      configData: [],
      currentRow: {
        key: '',
        value: ''
      },
      placeholder: '请输入配置Value',
      serviceName: '',
      totalCount: 0,
      id: 1
    }
  },
  mounted () {
  },
  created () {
    this.search()
  },
  methods: {
    rowChange (e) {
      if (e.row.key.endsWith('/')) {
        this.placeholder = '该配置Key是目录，不允许编辑配置Value'
        this.disabled = true
      } else {
        this.placeholder = '请输入配置内容'
        this.disabled = false
      }
      this.currentRow = e.row
    },

    async save () {
      for (let i = 0; i < this.configData.length; i++) {
        const u = this.configData[i]
        if (!u.key) {
          this.$message.error('配置key必填')
          return
        }
      }
      this.saveLoading = true
      const result = await this.$api.CONFIG_SAVE(this.configData).catch(_ => { this.saveLoading = false })
      this.saveLoading = false
      if (result) {
        this.$notify({
          title: '成功',
          message: '保存成功',
          type: 'success',
          duration: 2000
        })
        await this.search()
      }
    },
    async search () {
      this.loading = true
      this.pageModel.data = this.serviceName ? this.serviceName : '/'
      const result = await this.$api.CONFIG_LIST(this.pageModel)
      this.loading = false
      console.log('服务配置', result)
      this.totalCount = result.totalCount
      if (!result.items) {
        return
      }
      this.configData = []
      for (const i in result.items) {
        this.configData.push({ key: i, value: result.items[i] })
      }
      if (this.configData.length > 0) {
        this.$refs.config.setCurrentRow(this.configData[0])
        this.rowChange({ row: this.configData[0] })
      }
    },

    sizeChange (val) {
      this.pageModel.pageSize = val
      this.search()
    },

    currentChange (val) {
      this.pageModel.pageIndex = val
      this.search()
    },

    add () {
      this.id++
      this.configData.push({ id: this.id, key: '', value: '' })
    },

    async remove () {
      if (this.currentRow.id) {
        this.configData.splice(this.configData.findIndex(item => item.id === this.currentRow.id), 1)
        return
      }
      const result = await this.$api.CONFIG_DELETE({ key: this.currentRow.key })
      if (result) {
        this.$message({
          type: 'success',
          message: '删除成功！'
        })
      } else {
        this.$message.error('删除失败，请稍后重试！')
      }
      await this.search()
    }
  }
}
</script>
<style lang="scss" scoped>
.search{
  margin-left: 10px;
}
</style>
