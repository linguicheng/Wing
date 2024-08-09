<template>
  <d2-container>
    <template slot="header">
        <div>
            <el-input v-model="pageModel.data.userName"
                      clearable
                      @change="search"
                      placeholder="请输入姓名"
                      class="search"
                      style="width:200px"/>
            <el-input v-model="pageModel.data.userAccount"
                      clearable
                      @change="search"
                      placeholder="请输入账号"
                      class="search"
                      style="width:200px"/>
            <el-button class="search"
                       type="primary"
                       icon="el-icon-search"
                       @click.prevent="search"
            >查询</el-button>
            <el-button type="success"
                       @click="add"
                       icon="el-icon-plus"
            >新增</el-button>
        </div>
    </template>
     <div class="table-container">
      <vxe-table ref="xTable"
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
                 :data="list.items"
                 :mouse-config="{ selected: true }">
        <vxe-column field="userAccount"
                    title="账号"
                    sortable/>
        <vxe-column field="userName"
                    title="姓名"
                    sortable/>
        <vxe-column field="creationTime"
                    title="创建时间"
                    :formatter="['formatDate','yyyy-MM-dd HH:mm:ss']"
                    sortable/>
        <vxe-column field="createdName"
                    title="创建人"
                    sortable/>
         <vxe-column field="enabled"
                    title="是否可用"
                    sortable>
            <template v-slot="{ row }">
              <el-switch
                v-model="row.enabled"
                active-value="Y"
                inactive-value="N"
              />
            </template>
        </vxe-column>
        <vxe-column field="phone"
                    title="联系方式"
                    sortable/>
        <vxe-column field="remark"
                    title="备注"
                    sortable/>
        <vxe-column
          title="操作"
          width="300"
          align="center"
          fixed="right">
            <template #default="{ row }">
              <el-button type="primary"
                       size="small"
                       @click.prevent="edit(row)"
                       >编辑</el-button>
              <el-button type="success"
                         size="small"
                         @click.prevent="unlock(row)"
                         style="margin-right:10px;">解锁</el-button>
               <el-popconfirm
                  title="确定要重置密码吗？"
                  @onConfirm="reset(row)">
                  <el-button slot="reference" size="small" type="warning" style="margin-right:10px;">重置</el-button>
              </el-popconfirm>
              <el-popconfirm
                  title="确定要删除吗？"
                  @onConfirm="remove(row)">
                  <el-button slot="reference" size="small" type="danger">删除</el-button>
              </el-popconfirm>
            </template>
        </vxe-column>
      </vxe-table>
    </div>
    <template slot="footer">
      <el-pagination :current-page="pageModel.pageIndex"
                     :page-size="pageModel.pageSize"
                     :page-sizes="[15, 25, 35, 45]"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="list.totalCount"
                     @size-change="sizeChange"
                     @current-change="currentChange" />
    </template>
    <el-dialog
      :close-on-click-modal="false"
      v-dialogDrag
      :title="editTitle"
      width="60%"
      :visible.sync="editVisible"
      @open="handleEditDialogOpen"
    >
      <el-form
        ref="editForm"
        label-width="120px"
        :model="editForm"
        :rules="validRules"
        :show-message="true"
      >
        <el-row>
            <el-col :span="12">
                <el-form-item label="账号" prop="userAccount">
                  <el-input v-model="editForm.userAccount" />
                </el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="姓名" prop="userName">
                  <el-input v-model="editForm.userName" />
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="部门" prop="dept">
                  <el-input v-model="editForm.dept" />
                </el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="岗位" prop="station">
                  <el-input v-model="editForm.station" />
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
                <el-form-item label="联系方式" prop="phone">
                  <el-input v-model="editForm.phone" />
                </el-form-item>
            </el-col>
             <el-col :span="12">
                <el-form-item label="是否可用" prop="enabled">
                  <el-switch v-model="editForm.enabled" active-value="Y" inactive-value="N" />
                </el-form-item>
            </el-col>
        </el-row>
         <el-row>
          <el-col :span="24">
                <el-form-item label="备注" prop="remark">
                  <el-input v-model="editForm.remark" />
                </el-form-item>
            </el-col>
        </el-row>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editVisible = false">取消</el-button>
        <el-button type="primary" :loading="saveLoading" @click="save">保存</el-button>
      </span>
    </el-dialog>
  </d2-container>
</template>

<script>
export default {
  name: 'user',
  data () {
    return {
      loading: false,
      list: [],
      pageModel: {
        pageSize: 15,
        pageIndex: 1,
        data: {
          userName: '',
          userAccount: ''
        }

      },
      validRules: {
        userAccount: [
          {
            required: true,
            message: '账号必填',
            trigger: 'blur'
          }
        ],
        userName: [
          {
            required: true,
            message: '姓名必填',
            trigger: 'blur'
          }
        ]
      },
      saveLoading: false,
      editVisible: false,
      editForm: {
        enabled: 'Y'
      },
      editTitle: ''
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
      this.list = await this.$api.SYS_USER_LIST(this.pageModel)
      this.loading = false
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
      this.editTitle = '新增'
      this.editForm = { enabled: 'Y' }
      this.editVisible = true
    },
    edit (row) {
      this.editTitle = '编辑'
      this.editForm = Object.assign({}, row)
      this.editVisible = true
    },
    async unlock (row) {
      const result = await this.$api.SYS_USER_UNLOCKED(row.id)
      if (result > 0) {
        this.$message({
          type: 'success',
          message: '解锁成功！'
        })
      } else {
        this.$message.error('解锁失败，请稍后重试！')
      }
    },
    async remove (row) {
      const result = await this.$api.SYS_USER_DELETE(row.id)
      if (result > 0) {
        this.$message({
          type: 'success',
          message: '删除成功！'
        })
      } else {
        this.$message.error('删除失败，请稍后重试！')
      }
      await this.search()
    },
    async reset (row) {
      const result = await this.$api.SYS_USER_RESET_PASSWORD(row.id)
      if (result > 0) {
        this.$message({
          type: 'success',
          message: '密码重置成功！'
        })
      } else {
        this.$message.error('密码重置失败，请稍后重试！')
      }
    },
    handleEditDialogOpen () {
      this.$nextTick(() => {
        this.$refs.editForm && this.$refs.editForm.clearValidate()
      })
    },
    save () {
      this.$refs.editForm.validate(async (valid) => {
        if (!valid) {
          return false
        }
        this.saveLoading = true
        let result = 0
        if (this.editTitle === '新增') {
          result = await this.$api.SYS_USER_ADD(this.editForm).finally(_ => { this.saveLoading = false })
        } else {
          result = await this.$api.SYS_USER_UPDATE(this.editForm).finally(_ => { this.saveLoading = false })
          this.editVisible = false
        }

        if (result > 0) {
          this.$notify({
            title: '成功',
            message: '保存成功',
            type: 'success',
            duration: 2000
          })
          await this.search()
        }
      })
    }
  }
}
</script>
<style lang="scss" scoped>
</style>
