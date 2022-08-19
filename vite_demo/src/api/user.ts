import request from '../utils/request'

export function UserLogin(data:any){
    let param= new URLSearchParams(data);
    console.log('paras',param);
    console.log('data',data);

    return request({
        url:'/Authenticate/LoginGenerateJWT',
        method:'post',
        data:param
    })
}

export function UserRegister(data:any){
    let param=new URLSearchParams(data);

    return request({
        url:'/Authenticate/RegisterGenerateJWT',
        method:'post',
        data:param
    })
}