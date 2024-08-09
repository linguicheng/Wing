<template>
  <d2-container>
    <el-dropdown size="small" class="d2-mr">
      <span class="btn-text">{{info.name ? `你好 ${info.name}` : '未登录'}}</span>
      <el-dropdown-menu slot="dropdown">
        <el-dropdown-item @click.native="updatePassword">
          <i class="el-icon-key"></i>
          修改密码
        </el-dropdown-item>
        <el-dropdown-item @click.native="logOff">
          <d2-icon name="power-off" class="d2-mr-5"/>
          注销
        </el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
    <el-dialog
      :close-on-click-modal="false"
      v-dialogDrag
      title="修改密码"
      width="450px"
      :visible.sync="editVisible"
      @open="handleEditDialogOpen"
    >
      <el-form
        ref="editForm"
        label-width="90px"
        :model="editForm"
        :rules="validRules"
        :show-message="true"
      >
        <el-row>
            <el-col :span="24">
                <el-form-item label="原密码" prop="password">
                  <el-input v-model="editForm.password" show-password/>
                </el-form-item>
            </el-col>
        </el-row>
       <el-row>
            <el-col :span="24">
                <el-form-item label="新密码" prop="newPassword">
                  <el-input v-model="editForm.newPassword" show-password/>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="确认密码" prop="surePassword">
                  <el-input v-model="editForm.surePassword" show-password/>
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
import { mapState, mapActions } from 'vuex'
import util from '@/libs/util.js'
export default {
  data () {
    return {
      validRules: {
        password: [
          {
            required: true,
            message: '原密码必填',
            trigger: 'blur'
          }
        ],
        newPassword: [
          {
            required: true,
            message: '新密码必填',
            trigger: 'blur'
          }
        ],
        surePassword: [
          {
            required: true,
            message: '确认密码必填',
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
  computed: {
    ...mapState('d2admin/user', [
      'info'
    ])
  },
  methods: {
    ...mapActions('d2admin/account', [
      'logout'
    ]),
    /**
     * @description 登出
     */
    logOff () {
      this.logout({
        confirm: true
      })
    },
    updatePassword () {
      this.editVisible = true
      this.editForm = {}
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
        if (this.editForm.newPassword !== this.editForm.surePassword) {
          this.$message.error('新密码于确认密码不一致！')
          return false
        }
        if (this.editForm.newPassword === this.editForm.password) {
          this.$message.error('新密码不能与原密码一致！')
          return false
        }
        this.editForm.id = util.cookies.get('uuid')
        if (!this.editForm.id) {
          this.$message.error('当前账号登录已失效，请重新登录！')
          return false
        }
        this.saveLoading = true
        const result = await this.$api.SYS_USER_UPDATE_PASSWORD(this.editForm).finally(_ => { this.saveLoading = false })
        if (result > 0) {
          this.$notify({
            title: '成功',
            message: '密码修改成功，请重新登录！',
            type: 'success',
            duration: 2000,
            onClose: () => this.logout({ confirm: false })
          })
        }
      })
    }
  }
}
</script>
