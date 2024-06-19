using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    public static string Token;
    
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
        if (string.IsNullOrEmpty(Username.text))
        {
            DialogController.Show("啊哦...", "用户名不能放空哦。");
            return;
        }
        if (string.IsNullOrEmpty(Password.text))
        {
            DialogController.Show("啊哦...", "密码不能放空哦。");
            return;
        }
        if (waiting)
        {
            return;
        }
        waiting = true;
        Loading.SetActive(true);
        BtnText.text = mode == AuthMode.Login ? "登录中..." : "注册中...";
        var url = mode == AuthMode.Login ? "login" : "register";
        var res = await Server.Post<AuthResponse>(url, new AuthRequest()
        {
            username = Username.text,
            password = Password.text
        });
        if (res.status == "succeed")
        {
            if (mode == AuthMode.Login)
            {
                Token = res.token;
                var info = await Server.Get<UserModel>("users/info", ("token", Token));
                Debug.Log(info.money + "," + info.partTime);
                ChaperoneController.Instance.LoadHospitals();
                DialogController.Show("登录成功", $"欢迎回来，{Username.text}！", () =>
                {
                    CanvasManager.SwitchCanvas("MainCanvas");
                });
                return;
            }
            DialogController.Show("注册成功", "您已注册成功，可以前往登录了。");
        }
        else
        {
            DialogController.Show((mode == AuthMode.Login ? "登录" : "注册") + "失败", res.message);
        }
        BtnText.text = mode == AuthMode.Login ? "登录" : "注册";
        waiting = false;
        Loading.SetActive(false);
    }
}
