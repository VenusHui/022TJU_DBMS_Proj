<template>
  <div class="layout">
    <el-tabs type="border-card">
      <el-tab-pane label="登录">
        <el-form
          label-position="right"
          label-width="60px"
          style="max-width: 460px"
          class="LoginForm"
        >
          <el-form-item label="邮箱：">
            <el-input v-model="phone_number" />
          </el-form-item>
          <el-form-item label="密码：">
            <el-input type="password" v-model="password" />
          </el-form-item>

          <el-row>
            <el-checkbox
              class="CheckBox"
              v-model="isAgree"
              label="同意用户使用准则"
              name="type"
            />
          </el-row>
          <el-button
            v-if="isAgree"
            type="primary"
            class="LoginBtn"
            @click="login"
          >
            登录
          </el-button>
        </el-form>
      </el-tab-pane>

      <el-tab-pane label="注册">
        <el-form
          label-position="right"
          label-width="100px"
          style="max-width: 460px"
          class="LoginForm"
        >
          <el-form-item label="邮箱：">
            <el-input v-model="rEmail" />
          </el-form-item>
          <el-form-item label="密码：">
            <el-input type="password" v-model="rPassword" />
          </el-form-item>
          <el-form-item label="确认密码：">
            <el-input
              type="password"
              v-model="confirmPassword"
              @blur="confirmFunc"
            />
          </el-form-item>
          <el-form-item label="验证码：">
            <el-row>
              <el-input
                type="password"
                v-model="identifyCode"
                class="inpWidth"
              />
              <el-button type="primary" @click="getIdentifyCode" plain>
                获取验证码
              </el-button>
            </el-row>
          </el-form-item>

          <el-row>
            <el-checkbox
              class="CheckBox"
              v-model="rAgree"
              label="同意用户使用准则"
              name="type"
            />
          </el-row>
          <el-button
            v-if="rAgree"
            type="primary"
            class="LoginBtn"
            @click="register"
          >
            注册
          </el-button>
        </el-form>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>
<script lang="ts">
import { reactive, toRefs } from "@vue/reactivity";
import { ElMessage } from "element-plus";
import { UserLogin,UserRegister } from "../api/user";
export default {
  setup() {
    const LoginForm = reactive({
      phone_number: "",
      password: "",
      isAgree: 0,
    });
    const RegisterForm = reactive({
      rEmail: "",
      rPassword: "",
      confirmPassword: "",
      identifyCode: "",
      rAgree: 0,
    });

    // 方法
    // 登录
    function login() {
      console.log(LoginForm);
      UserLogin(LoginForm);
    }
    // 注册
    function register() {
      console.log("注册", RegisterForm);
      UserRegister(RegisterForm);
    }
    // 获取验证码
    function getIdentifyCode() {
      console.log("获取验证码");
    }

    const confirmFunc = () => {
      if (RegisterForm.confirmPassword !== RegisterForm.rPassword)
        ElMessage.error("密码与确认密码不一致."); // alert方法也可采用
    };
    return {
      ...toRefs(LoginForm),
      ...toRefs(RegisterForm),
      login,
      register,
      getIdentifyCode,
      confirmFunc,
    };
  },
};
</script>

<style scoped>
.layout {
  position: absolute;
  left: calc(50% - 200px);
  top: 20%;
  width: 400px;
}
.LoginBtn {
  width: 100px;
}
.LoginForm {
  text-align: center;
}
.CheckBox {
  margin-left: 7px;
}
.inpWidth {
  width: 165px;
}
</style>