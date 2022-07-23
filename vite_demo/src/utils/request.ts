import axios from 'axios'
import { ElMessageBox, ElMessage } from 'element-plus'

axios.defaults.headers.post['Content-Type']='application/x-www-form-urlencoded'
axios.defaults.withCredentials = true

// 创建axios实例
const service = axios.create({
    baseURL: '',
    timeout: 50000,
    crossDomain:true,
});

// response interceptor
service.interceptors.response.use(
    /**
     * If you want to get http information such as headers or status
     * Please return  response => response
     */
    
    /**
     * Determine the request status by custom code
     * Here is just an example
     * You can also judge the status by HTTP Status Code
     */
    response => {
        const res = response.data
        console.log('真实的回复为：',response)
        // if the custom code is not 200, it is judged as an error.
        if (res.errorCode != 200) {
            
            //判断token是否失效
            if(res.errorCode==400){

                ElMessage({
                    message: '您尚未登录，请先登录',
                    type: 'error',
                    duration: 5 * 1000
                })
                
                
                return Promise.reject(new Error('您尚未登录'||'Error'))
            }
            
            return Promise.reject(new Error(res.msg || 'Error'))
        } else {
            return res
        }
    },
    error => {
        console.log('Request Error!') // for debug
        return Promise.reject(error)
    }

)

export default service