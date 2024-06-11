using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    private enum AuthMode
    {
        Login, Register
    }
    
    public TMP_InputField Username, Password;
    public TMP_Text MessageText, BtnText, SwitchBtn;
    
    public GameObject Loading;

    private AuthMode mode = AuthMode.Login;
    private bool waiting = false;

    public void OnSwitchMode()
    {
        if (waiting)
        {
            return;
        }
        mode = mode == AuthMode.Login ? AuthMode.Register : AuthMode.Login;
        SwitchBtn.text = mode == AuthMode.Login ? "还没有账号？现在注册→\n" : "←已经有帐号了？点击登录";
        BtnText.text = mode == AuthMode.Login ? "登录" : "注册";
        MessageText.text = "";
    }

    public async void Execute()
    {
        if (waiting)
        {
            return;
        }
        waiting = true;
        Loading.SetActive(true);
        MessageText.text = "";
        BtnText.text = mode == AuthMode.Login ? "登录中..." : "注册中...";
        await Task.Delay(1500); // 发疯
        var url = mode == AuthMode.Login ? "login" : "register";
        var res = await Server.Post<AuthResponse>(url, new AuthRequest()
        {
            username = Username.text,
            password = Password.text
        });
        if (res.status == "succeed")
        {
            Debug.Log("成功！！！");
            // todo
        }
        else
        {
            MessageText.text = res.message;
        }
        BtnText.text = mode == AuthMode.Login ? "登录" : "注册";
        waiting = false;
        Loading.SetActive(false);
    }
}
